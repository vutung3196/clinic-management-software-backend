#pragma checksum "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5bb3a5e53744500800b5ccbcd78b5a1166bd3f09"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Pages_ProjectDetails_Index), @"mvc.1.0.razor-page", @"/Pages/ProjectDetails/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute("RouteTemplate", "{ProjectId}")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5bb3a5e53744500800b5ccbcd78b5a1166bd3f09", @"/Pages/ProjectDetails/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"23ac09be4bcfaa7f9829a01d1a134874eaae1f3b", @"/Pages/_ViewImports.cshtml")]
    public class Pages_ProjectDetails_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
 if (!String.IsNullOrEmpty(Model.Message))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h2>");
#nullable restore
#line 6 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
   Write(Model.Message);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n");
#nullable restore
#line 7 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
}
else
{


#line default
#line hidden
#nullable disable
            WriteLiteral("<h2>Project (Razor Page)</h2>\r\n");
            WriteLiteral("<h3>Name: ");
#nullable restore
#line 13 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
     Write(Model.Project.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n");
            WriteLiteral("<b>Items:</b>\r\n");
            WriteLiteral("<ul>\r\n");
#nullable restore
#line 18 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
     foreach (var item in Model.Project.Items)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <li>\r\n            ");
#nullable restore
#line 21 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
       Write(item.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 21 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
                         if (item.IsDone)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <span>DONE!</span>\r\n");
#nullable restore
#line 24 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
            }
            else
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <span>NOT DONE</span>\r\n");
#nullable restore
#line 28 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("            <br />");
#nullable restore
#line 29 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
             Write(item.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </li>\r\n");
#nullable restore
#line 31 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</ul>\r\n");
#nullable restore
#line 33 "D:\VuTung\Funix\Certificate 8\Codes\backend\clinic-management-software-backend\ClinicManagementSoftware\src\ClinicManagementSoftware.Web\Pages\ProjectDetails\Index.cshtml"
}

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ClinicManagementSoftware.Web.Pages.ToDoRazorPage.IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ClinicManagementSoftware.Web.Pages.ToDoRazorPage.IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ClinicManagementSoftware.Web.Pages.ToDoRazorPage.IndexModel>)PageContext?.ViewData;
        public ClinicManagementSoftware.Web.Pages.ToDoRazorPage.IndexModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
