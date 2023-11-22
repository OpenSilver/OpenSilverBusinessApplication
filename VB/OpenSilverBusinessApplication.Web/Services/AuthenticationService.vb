Imports System
Imports System.Linq
Imports System.Security.Principal
Imports System.Web
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports OpenRiaServices.DomainServices.Server.ApplicationServices

' TODO: Switch to a secure endpoint when deploying the application.
'       The user's name and password should only be passed using https.
'       To do this, set the RequiresSecureEndpoint property on EnableClientAccessAttribute to true.
'
'       [EnableClientAccess(RequiresSecureEndpoint = true)]
'
'       More information on using https with a Domain Service can be found on MSDN.

''' <summary>
''' Domain Service responsible for authenticating users when they log on to the application.
'''
''' Most of the functionality is already provided by the AuthenticationBase class.
''' </summary>
<EnableClientAccess>
Public Class AuthenticationService
    Inherits DomainService
    Implements IAuthentication(Of User)

    Public ReadOnly Property SignInManager As ApplicationSignInManager = HttpContext.Current.GetOwinContext().Get(Of ApplicationSignInManager)()

    Public ReadOnly Property UserManager As ApplicationUserManager = HttpContext.Current.GetOwinContext().GetUserManager(Of ApplicationUserManager)()

    Public ReadOnly Property AuthenticationManager As IAuthenticationManager = HttpContext.Current.GetOwinContext().Authentication

    Public Function Login(userName As String, password As String, isPersistent As Boolean, customData As String) As User Implements IAuthentication(Of User).Login

        Dim response As SignInStatus = SignInManager.PasswordSignIn(userName, password, isPersistent, shouldLockout:=False)

        Select Case response
            Case SignInStatus.Success
                Return GetAuthenticatedUser(userName)
            Case SignInStatus.RequiresVerification
                ' customdata can be used to pass 2fa token
                Throw New NotImplementedException()
            Case Else
                Return Nothing
        End Select

    End Function

    Public Function Logout() As User Implements IAuthentication(Of User).Logout

        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
        Return GetAnonymousUser()

    End Function

    Public Function GetUser() As User Implements IAuthentication(Of User).GetUser

        Dim identity As IIdentity = HttpContext.Current.User.Identity

        If identity.IsAuthenticated Then
            Return GetAuthenticatedUser(identity.Name)
        Else
            Return GetAnonymousUser()
        End If

    End Function

    Public Sub UpdateUser(user As User) Implements IAuthentication(Of User).UpdateUser
        Throw New System.NotImplementedException()
    End Sub

    Private Function GetAnonymousUser() As User

        Return New User() With {.Name = String.Empty, .Roles = Enumerable.Empty(Of String)()}

    End Function

    Private Function GetAuthenticatedUser(userName As String) As User

        Dim appUser As ApplicationUser = UserManager.FindByName(userName)

        If (appUser Is Nothing) Then Return Nothing
        Dim user As New User()
        With user
            .Name = appUser.UserName
            .Roles = appUser.Roles.Select(Function(iur) iur.RoleId)
            .PasswordAnswer = appUser.PasswordAnswer
            .PasswordQuestion = appUser.PasswordQuestion
        End With

        Return user
    End Function

End Class