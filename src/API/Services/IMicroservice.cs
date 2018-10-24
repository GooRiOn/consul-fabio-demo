using RestEase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IMicroservice
    {
        [Get("/api/values")]
        Task<IEnumerable<string>> GetValuesAsync();
    }
}
