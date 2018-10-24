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

        // This method gets called by the runtime. Use this method to  add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var client = new ConsulClient();
            var agentReg = new AgentServiceRegistration()
            {
                Address = "http://localhost",
                ID = "MY_UNIQUE_ID",
                Name = "Microservice1",
                Port = 5001,
                Checks = new[] {new AgentServiceCheck
                {
                    Interval = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    HTTP = "http://host.docker.internal:5001/api/values"
                }}
            };

            client.Agent.ServiceRegister(agentReg).Wait();

            app.UseMvc();
        }
    }
}
