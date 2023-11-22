Imports System
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin.Security.OAuth
Imports Owin

Partial Public Class Startup

    ' Enable the application to use OAuthAuthorization. You can then secure your Web APIs
    Shared Sub New()

        PublicClientId = "web"

        OAuthOptions = New OAuthAuthorizationServerOptions()
        With OAuthOptions
            .TokenEndpointPath = New PathString("/Token")
            .AuthorizeEndpointPath = New PathString("/Account/Authorize")
            .Provider = New ApplicationOAuthProvider(PublicClientId)
            .AccessTokenExpireTimeSpan = TimeSpan.FromDays(14)
            .AllowInsecureHttp = True
        End With

    End Sub

    Private Shared _OAuthOptions As OAuthAuthorizationServerOptions

    Public Shared Property OAuthOptions As OAuthAuthorizationServerOptions
        Get
            Return _OAuthOptions
        End Get
        Private Set(value As OAuthAuthorizationServerOptions)
            _OAuthOptions = value
        End Set
    End Property

    Private Shared _PublicClientId As String

    Public Shared Property PublicClientId As String
        Get
            Return _PublicClientId
        End Get
        Private Set(value As String)
            _PublicClientId = value
        End Set
    End Property

    ' For more information on configuring authentication, please visit https:'go.microsoft.com/fwlink/?LinkId=301864
    Public Sub ConfigureAuth(app As IAppBuilder)

        ' Configure the db context, user manager and signin manager to use a single instance per request
        app.CreatePerOwinContext(AddressOf ApplicationDbContext.Create)
        app.CreatePerOwinContext(Of ApplicationUserManager)(AddressOf ApplicationUserManager.Create)
        app.CreatePerOwinContext(Of ApplicationSignInManager)(AddressOf ApplicationSignInManager.Create)

        Dim cookieAuthenticationProvider As CookieAuthenticationProvider = New CookieAuthenticationProvider With {
            .OnValidateIdentity =
            SecurityStampValidator.OnValidateIdentity(Of ApplicationUserManager, ApplicationUser)(TimeSpan.FromMinutes(20), Function(manager, user) user.GenerateUserIdentityAsync(manager))
        }

        Dim cookieOptions As CookieAuthenticationOptions = New CookieAuthenticationOptions()
        With cookieOptions
            .AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            .LoginPath = New PathString("/Account/Login")
            .Provider = cookieAuthenticationProvider
        End With

        app.UseCookieAuthentication(cookieOptions)

        ' Use a cookie to temporarily store information about a user logging in with a third party login provider
        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie)

        ' Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
        app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5))

        ' Enables the application to remember the second login verification factor such as phone or email.
        ' Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
        ' This is similar to the RememberMe option when you log in.
        app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie)

        ' Enable the application to use bearer tokens to authenticate users
        app.UseOAuthBearerTokens(OAuthOptions)

        ' Uncomment the following lines to enable logging in with third party login providers
        'app.UseMicrosoftAccountAuthentication(
        '    clientId:="",
        '    clientSecret:="")

        'app.UseTwitterAuthentication(
        '    consumerKey:="",
        '    consumerSecret:="")

        'app.UseFacebookAuthentication(
        '    appId:="",
        '    appSecret:="")

        'app.UseGoogleAuthentication(New GoogleOAuth2AuthenticationOptions() With {
        '    .ClientId = "",
        '    .ClientSecret = ""})
    End Sub

End Class