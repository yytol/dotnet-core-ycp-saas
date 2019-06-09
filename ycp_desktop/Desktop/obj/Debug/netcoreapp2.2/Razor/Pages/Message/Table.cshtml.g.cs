#pragma checksum "X:\Projects\Private\P19003_SaaS\ycp_desktop\Desktop\Pages\Message\Table.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "eb8f6bb7018284f580020b837ad148e8ea71b8f0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BlueDesktop.Pages.Message.Pages_Message_Table), @"mvc.1.0.razor-page", @"/Pages/Message/Table.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/Message/Table.cshtml", typeof(BlueDesktop.Pages.Message.Pages_Message_Table), null)]
namespace BlueDesktop.Pages.Message
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"eb8f6bb7018284f580020b837ad148e8ea71b8f0", @"/Pages/Message/Table.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"997af14da44f044b816518594e8fab17dd5425d7", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Message_Table : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
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
#line 2 "X:\Projects\Private\P19003_SaaS\ycp_desktop\Desktop\Pages\Message\Table.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Pages/Shared/Desktop.cshtml";

#line default
#line hidden
            BeginContext(95, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(97, 2425, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("dpzp", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "eb8f6bb7018284f580020b837ad148e8ea71b8f03706", async() => {
                BeginContext(130, 2385, true);
                WriteLiteral(@"
    <div class=""pg-nav""><div class=""pg-nav-box""><div class=""pg-nav-title"">{{Title}}</div></div></div>
    <div class=""pg-list"">
        <table>
            <tr>
                <th style=""width:60px;min-width:60px;max-width:60px;"">序号</th>
                <th style=""width:600px;min-width:600px;max-width:600px;"">标题</th>
                <th style=""width:150px;min-width:150px;max-width:150px;"">发送时间</th>
            </tr>
            <tr v-for=""(row,index) in List"" v-bind:key=""row.ID"">
                <td>{{(Page-1)*PageSize+index+1}}</td>
                <td>{{row.Title}}</td>
                <td>{{row.SendTime}}</td>
            </tr>
            <tr>
                <td colspan=""3"">
                    <div class=""pg-pages"">
                        <dl>
                            <dd><i>共{{RowCount}}条，每页显示{{PageSize}}条</i></dd>
                            <dd v-if=""Page>2""><a href=""javascript:;"">首页</a></dd>
                            <dd v-if=""Page>1""><a href=""javascript:;"">上一页</a></dd>
");
                WriteLiteral(@"                            <dd v-if=""Page>1""><a href=""javascript:;"">{{Page-1}}</a></dd>
                            <dd v-if=""Page>2""><a href=""javascript:;"">{{Page-2}}</a></dd>
                            <dd v-if=""Page>3""><a href=""javascript:;"">{{Page-3}}</a></dd>
                            <dd v-if=""Page>4""><a href=""javascript:;"">{{Page-4}}</a></dd>
                            <dd v-if=""Page>5""><a href=""javascript:;"">{{Page-5}}</a></dd>
                            <dd><span>{{Page}}</span></dd>
                            <dd v-if=""Page<PageCount""><a href=""javascript:;"">{{Page+1}}</a></dd>
                            <dd v-if=""Page<PageCount-1""><a href=""javascript:;"">{{Page+2}}</a></dd>
                            <dd v-if=""Page<PageCount-2""><a href=""javascript:;"">{{Page+3}}</a></dd>
                            <dd v-if=""Page<PageCount-3""><a href=""javascript:;"">{{Page+4}}</a></dd>
                            <dd v-if=""Page<PageCount-4""><a href=""javascript:;"">{{Page+5}}</a></dd>
                ");
                WriteLiteral(@"            <dd v-if=""Page<PageCount""><a href=""javascript:;"">下一页</a></dd>
                            <dd v-if=""Page<PageCount-1""><a href=""javascript:;"">末页</a></dd>
                            <dt></dt>
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
            BeginContext(2522, 4, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Pages_Message_Table> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Pages_Message_Table> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Pages_Message_Table>)PageContext?.ViewData;
        public Pages_Message_Table Model => ViewData.Model;
    }
}
#pragma warning restore 1591