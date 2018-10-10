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

namespace WebApplication4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IConsulClient _client;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            _client = new ConsulClient(); // uses default host:port which is localhost:8500

            var agentReg = new AgentServiceRegistration()
            {
                Address = "host.docker.internal",
                ID = "uniqueid",
                Name = "serviceName",
                Port = 5008,
                Checks = new[] {new AgentServiceCheck
                {
                    Interval = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    HTTP = "http://host.docker.internal:5008/api/values"
                }},
                Tags = new[] { "urlprefix-/api/values" }
            };

            _client.Agent.ServiceRegister(agentReg).Wait();

            var a = _client.Catalog.Service("serviceName").Result;
            Console.WriteLine(a.Response.First().ServiceAddress);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            lifetime.ApplicationStopped.Register(() =>
            {
                _client.Agent.ServiceDeregister("uniqueid");
            });
            
            app.UseMvc();
        }
    }
}
