#pragma checksum "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d814f7cb7b1121b2db0b91575060779e78011025"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_IndexClient), @"mvc.1.0.view", @"/Views/Home/IndexClient.cshtml")]
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
#nullable restore
#line 1 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\_ViewImports.cshtml"
using WebAppl.Internet_banking;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\_ViewImports.cshtml"
using WebAppl.Internet_banking.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
using Internet_banking.Core.Application.Enums;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
using Internet_banking.Core.Application.ViewModels.Products;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d814f7cb7b1121b2db0b91575060779e78011025", @"/Views/Home/IndexClient.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f2d66b996239bef094defcbd7d3cb2b985ad6790", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Home_IndexClient : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<ProductsVM>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
  
    ViewData["Title"] = "Mis Productos";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"container\">\r\n    <h2 class=\"fw-bolder py-3\">Bienvenido ");
#nullable restore
#line 9 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                                     Write(Model[0].client.Username);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n    <div class=\"row \">\r\n");
#nullable restore
#line 11 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
         foreach (var item in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\" col-4\">\r\n                <div class=\"card\">\r\n                    <div class=\"card-header\">\r\n                        ");
#nullable restore
#line 16 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                   Write(item.TypeAccount.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                    <div class=\"card-body\">\r\n                        <h5 class=\"card-title\">");
#nullable restore
#line 19 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                                          Write(item.Code);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n");
#nullable restore
#line 20 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                         if (item.IdAccount == (int)TypesAccountEnum.CuentaPrincipal || item.IdAccount == (int)TypesAccountEnum.Cuentadeahorro)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <p class=\"card-text\">Saldo : ");
#nullable restore
#line 22 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                                                    Write(item.Amount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n");
#nullable restore
#line 23 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                        }
                        else if (item.IdAccount == (int)TypesAccountEnum.Tarjetadecredito)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <p class=\"card-text\">Limite de la tarjeta : ");
#nullable restore
#line 26 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                                                                   Write(item.Amount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                            <p class=\"card-text\">Total Tomado : ");
#nullable restore
#line 27 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                                                           Write(item.Paid);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n");
#nullable restore
#line 28 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                        }
                        else if (item.IdAccount == (int)TypesAccountEnum.Prestamo)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <p class=\"card-text\">Prestamo : ");
#nullable restore
#line 31 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                                                       Write(item.Amount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                            <p class=\"card-text\">Total pagado : ");
#nullable restore
#line 32 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                                                           Write(item.Paid);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n");
#nullable restore
#line 33 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </div>\r\n                </div>\r\n            </div>\r\n");
#nullable restore
#line 37 "C:\Users\Eleikel\Desktop\ITLA\Programacion III\NetBanking\Internet_banking\WebAppl.Internet banking\Views\Home\IndexClient.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n</div>\r\n\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<ProductsVM>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
