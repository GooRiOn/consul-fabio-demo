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

        public ValuesController(IConsulClient client, HostSettings settings)
        {
            var registryName = settings.Services
                .FirstOrDefault(s => s.ServiceName == "Microservice1")?.RegistryName;

            var serviceRegistration = client.Catalog.Service(registryName)
                .Result.Response.First();

            var host = $"{serviceRegistration.ServiceAddress}:{serviceRegistration.ServicePort}";

            _microservice = RestClient.For<IMicroservice>(host);
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
            => await _microservice.GetValuesAsync();
    }
}
