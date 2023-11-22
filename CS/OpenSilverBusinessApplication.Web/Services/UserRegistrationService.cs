using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using OpenSilverBusinessApplication.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Domain Service responsible for registering users.
    /// </summary>
    [EnableClientAccess]
    public class UserRegistrationService : DomainService
    {
        public ApplicationUserManager UserManager => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

        //// NOTE: This is a sample code to get your application started.
        ////       In the production code you should provide a mitigation against a denial of service attack
        ///        by providing CAPTCHA control functionality or verifying user's email address.

        /// <summary>
        /// Adds a new user with the supplied <see cref="RegistrationData"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="user">The registration information for this user.</param>
        /// <param name="password">The password for the new user.</param>
        [Invoke(HasSideEffects = true)]
        public IEnumerable<string> CreateUser(RegistrationData user,
            [Required(ErrorMessage = "This field is required")]
            [RegularExpression("^.*[^a-zA-Z0-9].*$", ErrorMessage = "A password needs to contain at least one special character e.g. @ or #")]
            [StringLength(50, MinimumLength = 7, ErrorMessage = "Password must be at least 7 and at most 50 characters long")]
            string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var appUser = new ApplicationUser()
            {
                UserName = user.UserName,
                Email = user.Email,
                FriendlyName = user.FriendlyName,
                PasswordQuestion = user.Question,
                PasswordAnswer = UserManager.PasswordHasher.HashPassword(user.Answer)
            };

            var result = UserManager.Create(appUser, password);

            if (!result.Succeeded)
            {
                return result.Errors;
            }

            return Enumerable.Empty<string>();
        }
    }
}