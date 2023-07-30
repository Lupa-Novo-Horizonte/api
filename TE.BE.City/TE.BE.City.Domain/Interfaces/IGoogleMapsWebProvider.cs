using System.Threading.Tasks;

namespace TE.BE.City.Domain.Interfaces;

public interface IGoogleMapsWebProvider
{
    public Task<string> GetAddress(string longitude, string latitude);
}