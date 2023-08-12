using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Device.Location;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Service.Services;

public class BackgroundService : IBackgroundService
{
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
    
    public BackgroundService(IConfiguration config, 
        IAsphaltService asphaltService,
        ICollectService collectService,
        ILightService lightService,
        ISewerService sewerService,
        ITrashService trashService,
        IWaterService waterService,
        INewsService newsService
    )
    {
        _asphaltService = asphaltService;
        _collectService = collectService;
        _lightService = lightService;
        _sewerService = sewerService;
        _trashService = trashService;
        _waterService = waterService;
        _newsService = newsService;
        _config = config;
        listIssue = new List<Issue>();
        listTuplaIssue = new List<(Issue, Issue)>();
        listNewsPriorityEntity = new List<NewsPriorityEntity>();
    }
    
    /// <summary>
    /// This method should be executed as fire/forget.
    /// Execute all the logic to generate priority list.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task ExecuteAsync()
    {
        await LoadData();
        await PrepareData();
        await CalculateCluster();
        await CalculateWeight();
        await UpdatePriorityTable();
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

        Console.WriteLine("Inicio");
        Console.WriteLine(asphaltEntityList.Count());
        Console.WriteLine(collectEntityList.Count());
        Console.WriteLine(lightEntityList.Count());
        Console.WriteLine(trashEntityList.Count());
        Console.WriteLine(sewerEntityList.Count());
        Console.WriteLine(waterEntityList.Count());
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
                Console.WriteLine($"Pair A: {a.ocorrencyId}, {a.ocorrencyType} / Pair B: {b.ocorrencyId}, {b.ocorrencyType}");
                listTuplaIssue.Add((a, b));
            }
            else
                Console.WriteLine($"Path item");
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
            originCoordenate.Latitude = double.Parse(tuplaIssue.Item1.longitude);

            var destinyCoordenate = new GeoCoordinate();
            destinyCoordenate.Longitude = double.Parse(tuplaIssue.Item2.longitude);
            destinyCoordenate.Latitude = double.Parse(tuplaIssue.Item2.longitude);

            var distance = originCoordenate.GetDistanceTo(destinyCoordenate);

            int points = int.Parse(_config["ClusterScore:default"]);

            if (distance <= int.Parse(_config["ClusterDistance"]))
            {
                points += int.Parse(_config["ClusterScore:closeTo"]); ;
                if (tuplaIssue.Item1.ocorrencyType == tuplaIssue.Item2.ocorrencyType)
                    points += int.Parse(_config["ClusterScore:sameType"]); ;
            }

            await UpdateCluster(tuplaIssue.Item1, points);
            await UpdateCluster(tuplaIssue.Item2, points);
        }

        Console.WriteLine("Meio");
        Console.WriteLine(listTuplaIssue.Count());
    }

    /// <summary>
    /// Add or update the list of priority with higher score
    /// </summary>
    /// <param name="item"></param>
    /// <param name="points"></param>
    /// <returns></returns>
    private async Task UpdateCluster(Issue item, int points)
    {
        var item1 = listNewsPriorityEntity.Find(c => c.OccurrenceId == item.ocorrencyId);
        if (item1 == null)
        {
            Console.WriteLine("Null: " + item.ocorrencyId);
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
            Console.WriteLine("Not null: " + item1.OccurrenceId);
            if (item1.Score < points)
            {
                Console.WriteLine("Score menor: " + item1.OccurrenceId);

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
        }
        Console.WriteLine("Final");
        Console.WriteLine(listNewsPriorityEntity.Count());
    }

    /// <summary>
    /// Clean and Update database table with all news priorities.
    /// </summary>
    /// <returns></returns>
    private async Task UpdatePriorityTable()
    {
        await _newsService.UpdatePriorityTable(listNewsPriorityEntity);
        Console.WriteLine("Tabela atualizada.");
    }
}