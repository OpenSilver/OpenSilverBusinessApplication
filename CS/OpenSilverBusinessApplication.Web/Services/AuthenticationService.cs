using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using OpenRiaServices.DomainServices.Server.ApplicationServices;
using System;
using System.Linq;
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
    public class AuthenticationService : DomainService, IAuthentication<User>
    {
        public ApplicationSignInManager SignInManager => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
        public ApplicationUserManager UserManager => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;

        public User Login(string userName, string password, bool isPersistent, string customData)
        {
            var response = SignInManager.PasswordSignIn(userName, password, isPersistent, shouldLockout: false);

            switch (response)
            {
                case SignInStatus.Success:
                    return GetAuthenticatedUser(userName);

                case SignInStatus.RequiresVerification:
                    // customdata can be used to pass 2fa token
                    throw new NotImplementedException();

                default:
                    return null;
            }
        }

        private User GetAnonymousUser()
        {
            return new User() { Name = string.Empty, Roles = Enumerable.Empty<string>() };
        }

        private User GetAuthenticatedUser(string userName)
        {
            var user = UserManager.FindByName(userName);

            if (user == null) return null;

            return new User()
            {
                Name = user.UserName,
                Roles = user.Roles.Select(iur => iur.RoleId),
                PasswordAnswer = user.PasswordAnswer,
                PasswordQuestion = user.PasswordQuestion
            };
        }

        public User Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return GetAnonymousUser();
        }

        public User GetUser()
        {
            var identity = HttpContext.Current.User.Identity;

            return identity.IsAuthenticated
                ? GetAuthenticatedUser(identity.Name)
                : GetAnonymousUser();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}