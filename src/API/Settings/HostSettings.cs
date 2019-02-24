using System.Collections.Generic;

namespace API.Settings
{
    public class HostSettings
    {
        public IEnumerable<ServiceSettings> Services { get; set; }
    }

    public class ServiceSettings
    {
        public string ServiceName { get; set; }
        public string HostUrl { get; set; }
    }
}
