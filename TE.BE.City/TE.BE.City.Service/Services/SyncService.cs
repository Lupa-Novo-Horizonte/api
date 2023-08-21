using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Device.Location;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting.Enum;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace TE.BE.City.Service.Services;

public class SyncService : BackgroundService
{
    private const int generalDelay = 1 * 10 * 1000; // 30 seconds
    private List<Issue> listIssue;
    private List<(Issue, Issue)> listTuplaIssue;
    struct Issue
    {
        public string longitude;
        public string latitude;
        public int ocorrencyId;
        public int ocorrencyType;
    }
    private List<NewsPriorityEntity> listNewsPriorityEntity;
    
    private readonly IAsphaltService _asphaltService;
    private readonly ICollectService _collectService;
    private readonly ILightService _lightService;
    private readonly ISewerService _sewerService;
    private readonly ITrashService _trashService;
    private readonly IWaterService _waterService;
    private readonly INewsService _newsService;
    private readonly IConfiguration _config;
    IServiceScope scope;


    public SyncService(IServiceProvider serviceProvider)
    {
        scope = serviceProvider.CreateScope();
        
        _asphaltService = scope.ServiceProvider.GetRequiredService<IAsphaltService>();
        _collectService = scope.ServiceProvider.GetRequiredService<ICollectService>();
        _lightService = scope.ServiceProvider.GetRequiredService<ILightService>();
        _sewerService = scope.ServiceProvider.GetRequiredService<ISewerService>();
        _trashService = scope.ServiceProvider.GetRequiredService<ITrashService>();
        _waterService = scope.ServiceProvider.GetRequiredService<IWaterService>();
        _newsService = scope.ServiceProvider.GetRequiredService<INewsService>();
        _config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                listIssue = new List<Issue>();
                listTuplaIssue = new List<(Issue, Issue)>();
                listNewsPriorityEntity = new List<NewsPriorityEntity>();

                Console.WriteLine("###");
                Console.WriteLine("Task started");
                await Task.Delay(generalDelay, stoppingToken);
                await LoadData();
                await PrepareData();
                await CalculateCluster();
                await CalculateWeight();
                await UpdatePriorityTable();
                Console.WriteLine("Task ended\n");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
        
    /// <summary>
    /// Load all data from DB and translate in one snigle list
    /// </summary>
    private async Task LoadData()
    {
        var asphaltEntityList = await _asphaltService.GetFilter(DateTime.MinValue, DateTime.MinValue, IsProblem.Problem);
        var collectEntityList = await _collectService.GetFilter(DateTime.MinValue, DateTime.MinValue, IsProblem.Problem);
        var lightEntityList = await _lightService.GetFilter(DateTime.MinValue, DateTime.MinValue, IsProblem.Problem);
        var trashEntityList = await _trashService.GetFilter(DateTime.MinValue, DateTime.MinValue, IsProblem.Problem);
        var sewerEntityList = await _sewerService.GetFilter(DateTime.MinValue, DateTime.MinValue, IsProblem.Problem);
        var waterEntityList = await _waterService.GetFilter(DateTime.MinValue, DateTime.MinValue, IsProblem.Problem);

        foreach (var item in asphaltEntityList)
        {
            listIssue.Add(new Issue()
            {
                latitude =  item.Latitude,
                longitude = item.Longitude,
                ocorrencyId = item.Id,
                ocorrencyType = (int)TypeIssue.Asphalt
            });    
        }
        
        foreach (var item in collectEntityList)
        {
            listIssue.Add(new Issue()
            {
                latitude =  item.Latitude,
                longitude = item.Longitude,
                ocorrencyId = item.Id,
                ocorrencyType = (int)TypeIssue.Collect
            });    
        }
        
        foreach (var item in lightEntityList)
        {
            listIssue.Add(new Issue()
            {
                latitude =  item.Latitude,
                longitude = item.Longitude,
                ocorrencyId = item.Id,
                ocorrencyType = (int)TypeIssue.Light
            });    
        }
        
        foreach (var item in trashEntityList)
        {
            listIssue.Add(new Issue()
            {
                latitude =  item.Latitude,
                longitude = item.Longitude,
                ocorrencyId = item.Id,
                ocorrencyType = (int)TypeIssue.Trash
            });    
        }
        
        foreach (var item in sewerEntityList)
        {
            listIssue.Add(new Issue()
            {
                latitude =  item.Latitude,
                longitude = item.Longitude,
                ocorrencyId = item.Id,
                ocorrencyType = (int)TypeIssue.Sewer
            });    
        }
        
        foreach (var item in waterEntityList)
        {
            listIssue.Add(new Issue()
            {
                latitude =  item.Latitude,
                longitude = item.Longitude,
                ocorrencyId = item.Id,
                ocorrencyType = (int)TypeIssue.Water
            });    
        }

        //Console.WriteLine($"´Load data count: {listIssue.Count}");
    }

    /// <summary>
    /// Convert list of data in a list of all possible combination without repetition. ex a==b and b==a or a==a.
    /// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples
    /// </summary>
    private async Task PrepareData()
    {
        foreach ((Issue a, Issue b) in GetAllPairs(listIssue))
        {
            if (!string.IsNullOrWhiteSpace(a.latitude) && !string.IsNullOrWhiteSpace(b.latitude))
            {
                listTuplaIssue.Add((a, b));
            }
        }
    }

    private static IEnumerable<(T, T)> GetAllPairs<T>(IList<T> source)
    {
        return source.SelectMany((_, i) => source.Where((_, j) => i < j),
            (x, y) => (x, y));
    }

    /// <summary>
    /// Calculate the distance btw two geolocation and identify cluster.
    /// </summary>
    private async Task CalculateCluster()
    {
        foreach (var tuplaIssue in listTuplaIssue)
        {
            var originCoordenate = new GeoCoordinate();
            originCoordenate.Longitude = double.Parse(tuplaIssue.Item1.longitude);
            originCoordenate.Latitude = double.Parse(tuplaIssue.Item1.latitude);

            var destinyCoordenate = new GeoCoordinate();
            destinyCoordenate.Longitude = double.Parse(tuplaIssue.Item2.longitude);
            destinyCoordenate.Latitude = double.Parse(tuplaIssue.Item2.latitude);

            var distance = Convert.ToInt32(originCoordenate.GetDistanceTo(destinyCoordenate));

            int points = int.Parse(_config["ClusterScore:default"]);

            if (distance <= int.Parse(_config["ClusterDistance"]))
            {
                points += int.Parse(_config["ClusterScore:closeTo"]); ;
                if (tuplaIssue.Item1.ocorrencyType == tuplaIssue.Item2.ocorrencyType)
                    points += int.Parse(_config["ClusterScore:sameType"]);
            }

            await UpdateCluster(tuplaIssue.Item1, points);
            await UpdateCluster(tuplaIssue.Item2, points);
        }
    }

    /// <summary>
    /// Add or update the list of priority with higher score
    /// </summary>
    /// <param name="item"></param>
    /// <param name="points"></param>
    /// <returns></returns>
    private async Task UpdateCluster(Issue item, int points)
    {
        //Console.WriteLine($"Id={item.ocorrencyId} type={item.ocorrencyType} score={points}");

        var item1 = listNewsPriorityEntity.Find(c => c.OccurrenceId == item.ocorrencyId && c.OccurrenceType == (TypeIssue)item.ocorrencyType);
        if (item1 == null)
        {
            listNewsPriorityEntity.Add(new NewsPriorityEntity()
            {
                OccurrenceId = item.ocorrencyId,
                Latitude = item.latitude,
                Longitude = item.longitude,
                OccurrenceType = (TypeIssue)item.ocorrencyType,
                Score = points
            });
        }
        else
        {
            if (item1.Score < points)
            {
                listNewsPriorityEntity.Remove(item1);
                listNewsPriorityEntity.Add(new NewsPriorityEntity()
                {
                    OccurrenceId = item.ocorrencyId,
                    Latitude = item.latitude,
                    Longitude = item.longitude,
                    OccurrenceType = (TypeIssue)item.ocorrencyType,
                    Score = points
                });
            }
        }
    }

    /// <summary>
    /// GEnerate the weight of each item based on the score.
    /// </summary>
    /// <returns></returns>
    private async Task CalculateWeight()
    {
        int totalScore = listNewsPriorityEntity.Sum(c => c.Score);

        foreach (var item in listNewsPriorityEntity)
        {
            int weight = (item.Score * 100) / totalScore;
            item.Weight = weight;

            Console.WriteLine($"Score= {item.Score} total score={totalScore} weight= {weight}");
        }
    }

    /// <summary>
    /// Clean and Update database table with all news priorities.
    /// </summary>
    /// <returns></returns>
    private async Task UpdatePriorityTable()
    {
        await _newsService.UpdatePriorityTable(listNewsPriorityEntity);
        Console.WriteLine($"Table updated= {listNewsPriorityEntity.Count} items.");
    }
}