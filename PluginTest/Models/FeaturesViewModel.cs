using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeInfo = System.Reflection.TypeInfo;

namespace PluginTest.Models
{
    public class FeaturesViewModel
    {
        public List<TypeInfo> Controllers { get; set; }

        public List<MetadataReference> MetadataReferences { get; set; } = new List<MetadataReference>();

        public List<TypeInfo> TagHelpers { get; set; }

        public List<TypeInfo> ViewComponents { get; set; }

        public IList<string> Routes { get; set; } = new List<string>();
    }
}
