#pragma checksum "X:\Projects\Private\P19003_SaaS\ycp_entity\Entity\Pages\AosUsers\Edit.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c0366b08a76df53a9c253077bddd980e872f9aec"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BlueDesktop.Pages.AosUsers.Pages_AosUsers_Edit), @"mvc.1.0.razor-page", @"/Pages/AosUsers/Edit.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/AosUsers/Edit.cshtml", typeof(BlueDesktop.Pages.AosUsers.Pages_AosUsers_Edit), null)]
namespace BlueDesktop.Pages.AosUsers
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c0366b08a76df53a9c253077bddd980e872f9aec", @"/Pages/AosUsers/Edit.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"997af14da44f044b816518594e8fab17dd5425d7", @"/Pages/_ViewImports.cshtml")]
    public class Pages_AosUsers_Edit : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("dpz-id", "Tool", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("dig-tools"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("dpz-id", "Form", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("dig-form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("sign", "Manage_Authorization_Add", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::dpz.Mvc.TagHelpers.DpzIdHelper __dpz_Mvc_TagHelpers_DpzIdHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "X:\Projects\Private\P19003_SaaS\ycp_entity\Entity\Pages\AosUsers\Edit.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Pages/Shared/Desktop.cshtml";

    string cachePath = System.IO.Directory.GetCurrentDirectory() + site.Config.Orm.CachePath;
    Dictionary<string, string> cache = new Dictionary<string, string>();
    dpz.Mvc.UI.XmlUIForVue ui = new dpz.Mvc.UI.XmlUIForVue(site.Config.Database.Aos, cache, site.Config.Orm.XmlUrl, cachePath);

#line default
#line hidden
            BeginContext(395, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(397, 531, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("dpzp", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c0366b08a76df53a9c253077bddd980e872f9aec5384", async() => {
                BeginContext(435, 38, true);
                WriteLiteral("\r\n    <div class=\"dig-body\">\r\n        ");
                EndContext();
                BeginContext(473, 247, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c0366b08a76df53a9c253077bddd980e872f9aec5807", async() => {
                    BeginContext(510, 204, true);
                    WriteLiteral("\r\n            <a href=\"javascript:;\" v-on:click=\"onSave\"><img v-bind:src=\"Images.Save\" />保存</a>\r\n            <a href=\"javascript:;\" v-on:click=\"onCancel\"><img v-bind:src=\"Images.Cancel\" />取消</a>\r\n        ");
                    EndContext();
                }
                );
                __dpz_Mvc_TagHelpers_DpzIdHelper = CreateTagHelper<global::dpz.Mvc.TagHelpers.DpzIdHelper>();
                __tagHelperExecutionContext.Add(__dpz_Mvc_TagHelpers_DpzIdHelper);
                __dpz_Mvc_TagHelpers_DpzIdHelper.DpzId = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(720, 10, true);
                WriteLiteral("\r\n        ");
                EndContext();
                BeginContext(730, 177, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c0366b08a76df53a9c253077bddd980e872f9aec7496", async() => {
                    BeginContext(766, 58, true);
                    WriteLiteral("\r\n            <div class=\"dig-form-box\">\r\n                ");
                    EndContext();
                    BeginContext(825, 46, false);
#line 19 "X:\Projects\Private\P19003_SaaS\ycp_entity\Entity\Pages\AosUsers\Edit.cshtml"
           Write(Html.Raw(ui.GetEditFormHtml("Aos","AosUsers")));

#line default
#line hidden
                    EndContext();
                    BeginContext(871, 30, true);
                    WriteLiteral("\r\n            </div>\r\n        ");
                    EndContext();
                }
                );
                __dpz_Mvc_TagHelpers_DpzIdHelper = CreateTagHelper<global::dpz.Mvc.TagHelpers.DpzIdHelper>();
                __tagHelperExecutionContext.Add(__dpz_Mvc_TagHelpers_DpzIdHelper);
                __dpz_Mvc_TagHelpers_DpzIdHelper.DpzId = (string)__tagHelperAttribute_2.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(907, 14, true);
                WriteLiteral("\r\n    </div>\r\n");
                EndContext();
            }
            );
            __dpz_Mvc_TagHelpers_DpzpTagHelper = CreateTagHelper<global::dpz.Mvc.TagHelpers.DpzpTagHelper>();
            __tagHelperExecutionContext.Add(__dpz_Mvc_TagHelpers_DpzpTagHelper);
            __dpz_Mvc_TagHelpers_DpzpTagHelper.Sign = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(928, 4, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Pages_AosUsers_Edit> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Pages_AosUsers_Edit> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Pages_AosUsers_Edit>)PageContext?.ViewData;
        public Pages_AosUsers_Edit Model => ViewData.Model;
    }
}
#pragma warning restore 1591
