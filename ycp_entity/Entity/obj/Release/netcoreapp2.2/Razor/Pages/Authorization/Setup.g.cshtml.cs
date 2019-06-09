#pragma checksum "X:\Projects\Private\P19003_SaaS\ycp_entity\Entity\Pages\Authorization\Setup.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "105ac5b207230d9f4afa4bf4bc776204cef03fe4"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BlueDesktop.Pages.Authorization.Pages_Authorization_Setup), @"mvc.1.0.razor-page", @"/Pages/Authorization/Setup.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/Authorization/Setup.cshtml", typeof(BlueDesktop.Pages.Authorization.Pages_Authorization_Setup), null)]
namespace BlueDesktop.Pages.Authorization
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "X:\Projects\Private\P19003_SaaS\ycp_entity\Entity\Pages\_ViewImports.cshtml"
using BlueDesktop;

#line default
#line hidden
#line 5 "X:\Projects\Private\P19003_SaaS\ycp_entity\Entity\Pages\_ViewImports.cshtml"
using dpz.Mvc;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"105ac5b207230d9f4afa4bf4bc776204cef03fe4", @"/Pages/Authorization/Setup.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"997af14da44f044b816518594e8fab17dd5425d7", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Authorization_Setup : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("sign", "Authorization_Setup", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 2 "X:\Projects\Private\P19003_SaaS\ycp_entity\Entity\Pages\Authorization\Setup.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Pages/Shared/Desktop.cshtml";

#line default
#line hidden
            BeginContext(95, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(97, 2294, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("dpzp", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "105ac5b207230d9f4afa4bf4bc776204cef03fe43764", async() => {
                BeginContext(130, 2254, true);
                WriteLiteral(@"
    <div class=""pg-body"">
        <div class=""pg-nav-line"">
            <div class=""pg-nav""><s>系统授权管理</s><i></i><a href=""javascript:;"" v-on:click=""goList"">所有授权</a><i></i><s>{{Args.Name}}</s><i></i><s>安装数据表</s><i></i></div>
        </div>
        <div class=""pg-content"">
            <div class=""pg-catalog"">
                <div class=""pg-catalog-title"">表格分类</div>
                <div class=""pg-catalog-list"">
                    <ul v-for=""row in catalogs"" v-bind:key=""row.Name"">
                        <li>
                            <template v-if=""row.Selected===''"">
                                <a href=""javascript:;"" v-on:click=""onChangeCatalog($event,row)"">{{row.Title}}({{row.Name}})</a>
                            </template>
                            <template v-else>
                                <span>{{row.Title}}({{row.Name}})</span>
                            </template>
                        </li>
                    </ul>
                </div>
            </div>
  ");
                WriteLiteral(@"          <div class=""pg-list-area"">
                <div class=""pg-list"">
                    <table>
                        <tr>
                            <th style=""width:150px;"">表格名称</th>
                            <th style=""width:150px;"">配置版本</th>
                            <th style=""width:150px;"">安装版本</th>
                            <th style=""width:250px;"">操作项</th>
                        </tr>
                        <tr v-for=""row in list"" v-bind:key=""row.ID"">
                            <td>{{row.Name}}</td>
                            <td>{{row.Version}}</td>
                            <td>{{row.SetupVersion}}</td>
                            <td>
                                <template v-if=""row.NeedUpdate!=='none'"">
                                    <a href=""javascript:;"" v-on:click=""onUpdate($event,row)"">安装或更新</a>
                                </template>
                                <template v-else>
                                    <span>-</span>
       ");
                WriteLiteral("                         </template>\r\n                            </td>\r\n                        </tr>\r\n                    </table>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n");
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
            BeginContext(2391, 4, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Pages_Authorization_Setup> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Pages_Authorization_Setup> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Pages_Authorization_Setup>)PageContext?.ViewData;
        public Pages_Authorization_Setup Model => ViewData.Model;
    }
}
#pragma warning restore 1591