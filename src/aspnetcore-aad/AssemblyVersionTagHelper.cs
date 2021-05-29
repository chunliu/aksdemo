using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace aspnetcore_aad
{
    [HtmlTargetElement("AssemblyVersion", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AssemblyVersionTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            output.Content.Append(
                $"v{GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}"
            );
        }
    }
}
