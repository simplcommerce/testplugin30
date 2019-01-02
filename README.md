# Test plugin on asp.net core 2.2

Dynamic load plugin1 (razor class library) into the PluginTest.

On the PluginTest override some views in the plugin 1.

Uncomment line 69 in Startup.cs 

```
o.AdditionalCompilationReferences.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Plugin1")).Location));
```

it will work
