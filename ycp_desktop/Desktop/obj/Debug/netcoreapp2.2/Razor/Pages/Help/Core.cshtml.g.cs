#pragma checksum "X:\Projects\Private\P19003_SaaS\ycp_desktop\Desktop\Pages\Help\Core.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1c063141d5c496ea78c11d8d16dc95c51915213d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BlueDesktop.Pages.Help.Pages_Help_Core), @"mvc.1.0.razor-page", @"/Pages/Help/Core.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/Help/Core.cshtml", typeof(BlueDesktop.Pages.Help.Pages_Help_Core), null)]
namespace BlueDesktop.Pages.Help
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "X:\Projects\Private\P19003_SaaS\ycp_desktop\Desktop\Pages\_ViewImports.cshtml"
using BlueDesktop;

#line default
#line hidden
#line 5 "X:\Projects\Private\P19003_SaaS\ycp_desktop\Desktop\Pages\_ViewImports.cshtml"
using dpz.Mvc;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1c063141d5c496ea78c11d8d16dc95c51915213d", @"/Pages/Help/Core.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"997af14da44f044b816518594e8fab17dd5425d7", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Help_Core : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("sign", "Desktop_Setting_Pwd", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::dpz.Mvc.TagHelpers.DpzpTagHelper __dpz_Mvc_TagHelpers_DpzpTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "X:\Projects\Private\P19003_SaaS\ycp_desktop\Desktop\Pages\Help\Core.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Pages/Shared/Desktop.cshtml";

#line default
#line hidden
            BeginContext(95, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(97, 1432, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("dpzp", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1c063141d5c496ea78c11d8d16dc95c51915213d3665", async() => {
                BeginContext(130, 1392, true);
                WriteLiteral(@"
    <div class=""pg-nav""><div class=""pg-nav-box""><div class=""pg-nav-title"">{{Title}}</div></div></div>
    <div class=""pg-form"">
        <table>
            <tr>
                <td style=""width:100px;text-align:right;""><s>内核名称</s></td>
                <td>{{info.title}}({{info.name}})</td>
            </tr>
            <tr>
                <td style=""text-align:right;""><s>内核版本</s></td>
                <td>{{version}}</td>
            </tr>
            <tr>
                <td style=""text-align:right;""><s>内核说明</s></td>
                <td>{{info.description}}</td>
            </tr>
            <tr>
                <td style=""text-align:right;""><s>更新说明</s></td>
                <td>
                    <div class=""pg-history"">
                        <dl>
                            <template v-for=""history in info.builds"" v-bind:key=""history.build"">
                                <dt>Version {{history.version}} build {{history.build}}</dt>
                                <template v-for");
                WriteLiteral(@"=""(note,index) in history.notes"">
                                    <dd>{{index+1}}、{{note}}</dd>
                                </template>
                                <dd>&nbsp;</dd>
                            </template>
                        </dl>
                    </div>
                </td>
            </tr>
        </table>
    </div>
");
                EndContext();
            }
            );
            __dpz_Mvc_TagHelpers_DpzpTagHelper = CreateTagHelper<global::dpz.Mvc.TagHelpers.DpzpTagHelper>();
            __tagHelperExecutionContext.Add(__dpz_Mvc_TagHelpers_DpzpTagHelper);
            __dpz_Mvc_TagHelpers_DpzpTagHelper.Sign = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1529, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Pages_Help_Core> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Pages_Help_Core> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Pages_Help_Core>)PageContext?.ViewData;
        public Pages_Help_Core Model => ViewData.Model;
    }
}
#pragma warning restore 1591
