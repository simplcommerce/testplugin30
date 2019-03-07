using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace PluginTest
{
    public static class Global
    {
        public static IList<Plugin> Plugins { get; set; } = new List<Plugin>();

        public static IMvcBuilder MvcBuilder { get; set; }
    }
}
