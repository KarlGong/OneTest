using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OneTestApi.Controllers.DTOs;
using OneTestApi.Data;
using OneTestApi.Models;
using OneTestApi.Services;

namespace OneTestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc().AddJsonOptions(
                options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(true));
                });

            services.AddAutoMapper(config =>
            {
                config.AllowNullCollections = true;
                config.AllowNullDestinationValues = true;
                config.CreateMissingTypeMaps = true;
            
                config.CreateMap<TestCase, TestNodeDto>().ForMember(tnd => tnd.Type, opt => opt.ResolveUsing(tn => "case"));
                config.CreateMap<TestSuite, TestNodeDto>().ForMember(tnd => tnd.Type, opt => opt.ResolveUsing(tn => "suite"));
                config.CreateMap<TestProject, TestNodeDto>().ForMember(tnd => tnd.Type, opt => opt.ResolveUsing(tn => "project"));
            });
            
            services.AddDbContext<OneTestDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("mysql")));

            services.AddTransient<ITestNodeService, TestNodeService>();
            services.AddTransient<ITestProjectService, TestProjectService>();
            services.AddTransient<ITestSuiteService, TestSuiteService>();
            services.AddTransient<ITestCaseService, TestCaseService>();
            services.AddTransient<ITestCaseTagService, TestCaseTagService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            
            app.UseDeveloperExceptionPage();
            
            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}