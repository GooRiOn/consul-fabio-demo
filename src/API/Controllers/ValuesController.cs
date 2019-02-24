using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using RestEase;
using System.Threading.Tasks;
using Consul;
using API.Settings;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IMicroservice _microservice;

        public ValuesController()
        {
            var query = new ConsulClient().Catalog.Service("Microservice1").GetAwaiter().GetResult();
            var instance = LoadBalance();    
            

            var host = $"{instance.ServiceAddress}:{instance.ServicePort}";

            _microservice = RestClient.For<IMicroservice>(host);

            CatalogService LoadBalance()
                => query.Response.First();
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
            => await _microservice.GetValuesAsync();
    }
}
