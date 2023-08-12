using System.Threading.Tasks;

namespace TE.BE.City.Domain.Interfaces;

public interface IBackgroundService
{
    Task ExecuteAsync();
}