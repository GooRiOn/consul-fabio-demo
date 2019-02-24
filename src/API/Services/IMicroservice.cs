using RestEase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IMicroservice
    {
        [Get("/api/Values")]
        Task<IEnumerable<string>> GetValuesAsync();
    }
}
