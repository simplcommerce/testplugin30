﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PluginTest
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //var rootDir = new DirectoryInfo(_hostingEnvironment.ContentRootPath).Parent;
            //var dir = Path.Combine(rootDir.ToString(), "Plugin1", "bin", "Debug", "netcoreapp3.0");

            //var pluginContext = new PluginLoadContext(Path.Combine(dir, "Plugin1.dll"));
            //Assembly pluginAssembly = pluginContext.LoadFromAssemblyName(new AssemblyName("Plugin1"));

            //var plugin = new Plugin
            //{
            //    Assembly = pluginAssembly,
            //    PluginLoadContext = pluginContext
            //};
            //Global.Plugins.Add(plugin);


            var mvcBuilder = services.AddMvc().AddNewtonsoftJson();
            Global.MvcBuilder = mvcBuilder;


            mvcBuilder.AddRazorOptions(o =>
            {
               // o.AdditionalCompilationReferences.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Plugin1")).Location));
            });

            //var partFactory = ApplicationPartFactory.GetApplicationPartFactory(pluginAssembly);
            //foreach (var part in partFactory.GetApplicationParts(pluginAssembly))
            //{
            //    mvcBuilder.PartManager.ApplicationParts.Add(part);
            //}

            //var relatedAssemblies = RelatedAssemblyAttribute.GetRelatedAssemblies(pluginAssembly, throwOnError: true);
            //foreach (var assembly in relatedAssemblies)
            //{
            //    partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
            //    foreach (var part in partFactory.GetApplicationParts(assembly))
            //    {
            //        mvcBuilder.PartManager.ApplicationParts.Add(part);
            //    }
            //}
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting(routes =>
            {
                routes.MapControllerRoute(
                  name: "MyArea",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapControllerRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRazorPages();
            });

            app.UseCookiePolicy();

            app.UseAuthorization();
        }
    }
}
