using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using RestEase;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IMicroservice _microservice;

        public ValuesController()
            => _microservice = RestClient.For<IMicroservice>("http://localhost:9999");

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
            => await _microservice.GetValuesAsync();
    }
}
