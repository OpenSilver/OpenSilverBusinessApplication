﻿using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server.ApplicationServices;
using System.Security.Principal;
using System.Web.Security;
using System.Web;

namespace OpenSilverBusinessApplication.Web
{
    // TODO: Switch to a secure endpoint when deploying the application.
    //       The user's name and password should only be passed using https.
    //       To do this, set the RequiresSecureEndpoint property on EnableClientAccessAttribute to true.
    //
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       More information on using https with a Domain Service can be found on MSDN.

    /// <summary>
    /// Domain Service responsible for authenticating users when they log on to the application.
    ///
    /// Most of the functionality is already provided by the AuthenticationBase class.
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User>
    {
        protected override void IssueAuthenticationToken(IPrincipal principal, bool isPersistent)
        {
            var ticket = new FormsAuthenticationTicket(principal.Identity.Name, isPersistent, 30);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            authCookie.Secure = true;
            authCookie.SameSite = SameSiteMode.None;
            authCookie.HttpOnly = true; // testing for true or false yet

            HttpContext.Current.Response.Cookies.Add(authCookie);
        }
    }
}