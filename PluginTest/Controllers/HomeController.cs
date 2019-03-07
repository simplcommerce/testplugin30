using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Hosting;
using PluginTest.Models;

namespace PluginTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationPartManager _partManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IActionDescriptorCollectionProvider _provider;



        public HomeController(IActionDescriptorCollectionProvider provider, ApplicationPartManager partManager, IHostingEnvironment hostingEnvironment)
        {
            _provider = provider;
            _partManager = partManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var rootDir = new DirectoryInfo(_hostingEnvironment.ContentRootPath).Parent;
            var dir = Path.Combine(rootDir.ToString(), "Plugin1", "bin", "Debug", "netcoreapp3.0");

            var pluginContext = new PluginLoadContext(Path.Combine(dir, "Plugin1.dll"));
            Assembly pluginAssembly = pluginContext.LoadFromAssemblyName(new AssemblyName("Plugin1"));

            var plugin = new Plugin
            {
                Assembly = pluginAssembly,
                PluginLoadContext = pluginContext
            };

            Global.Plugins.Add(plugin);

            var partFactory = ApplicationPartFactory.GetApplicationPartFactory(pluginAssembly);
            foreach (var part in partFactory.GetApplicationParts(pluginAssembly))
            {
                Global.MvcBuilder.PartManager.ApplicationParts.Add(part);
            }

            var viewModel = new FeaturesViewModel();

            var controllerFeature = new ControllerFeature();
            _partManager.PopulateFeature(controllerFeature);
            viewModel.Controllers = controllerFeature.Controllers.ToList();

            //var metaDataReferenceFeature = new MetadataReferenceFeature();
            //_partManager.PopulateFeature(metaDataReferenceFeature);
            //viewModel.MetadataReferences = metaDataReferenceFeature.MetadataReferences
            //                                .ToList();

            var tagHelperFeature = new TagHelperFeature();
            _partManager.PopulateFeature(tagHelperFeature);
            viewModel.TagHelpers = tagHelperFeature.TagHelpers.ToList();

            var viewComponentFeature = new ViewComponentFeature();
            _partManager.PopulateFeature(viewComponentFeature);
            viewModel.ViewComponents = viewComponentFeature.ViewComponents.ToList();

            var routes = _provider.ActionDescriptors.Items.Select(x => new {
                Action = x.RouteValues["Action"],
                Controller = x.RouteValues["Controller"],
                Name = x.AttributeRouteInfo?.Name,
                Template = x.AttributeRouteInfo?.Template
            }).ToList();

            viewModel.Routes = routes.Select(x => $"Name: {x.Name}, Controller: {x.Controller}, Action: {x.Action}, Template: {x.Template}").ToList();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult RemovePlugin()
        {
            var plugin = Global.Plugins.First();
            var partFactory = ApplicationPartFactory.GetApplicationPartFactory(plugin.Assembly);
            foreach (var part in partFactory.GetApplicationParts(plugin.Assembly))
            {
                Global.MvcBuilder.PartManager.ApplicationParts.Remove(part);
            }

            Global.Plugins.Remove(plugin);

            plugin.PluginLoadContext.Unload();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
