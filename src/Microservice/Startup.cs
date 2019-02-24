using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Consul;

namespace Microservice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }
       
        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime, IHostingEnvironment env)
        {
            var client = new ConsulClient();

            var serviceId = Guid.NewGuid().ToString();
            var agentReg = new AgentServiceRegistration()
            {
                Address = "http://localhost",
                ID = serviceId,
                Name = "Microservice1",
                Port = 5001
            };

            client.Agent.ServiceRegister(agentReg).GetAwaiter().GetResult();

            app.UseMvc();

            appLifetime.ApplicationStopped.Register(() => client.Agent.ServiceDeregister(serviceId));
        }
    }
}
