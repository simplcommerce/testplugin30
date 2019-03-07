using System.Reflection;

namespace PluginTest
{
    public class Plugin
    {
        public Assembly Assembly { get; set; }

        public PluginLoadContext PluginLoadContext { get; set; }
    }
}
