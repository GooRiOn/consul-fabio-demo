using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get() => new string[] { "value1", "value2" };
    }
}
