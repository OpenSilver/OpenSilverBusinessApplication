Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Web
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server

' TODO: Switch to a secure endpoint when deploying the application.
'       The user's name and password should only be passed using https.
'       To do this, set the RequiresSecureEndpoint property on EnableClientAccessAttribute to true.
'
'       <EnableClientAccess(RequiresSecureEndpoint = true)>
'
'       More information on using https with a Domain Service can be found on MSDN.

''' <summary>
''' Domain Service responsible for registering users.
''' </summary>
<EnableClientAccess>
Public Class UserRegistrationService
    Inherits DomainService

    Public ReadOnly Property UserManager As ApplicationUserManager = HttpContext.Current.GetOwinContext().GetUserManager(Of ApplicationUserManager)()

    ' NOTE: This is a sample code to get your application started.
    '       In the production code you should provide a mitigation against a denial of service attack
    '       by providing CAPTCHA control functionality or verifying user's email address.

    ''' <summary>
    ''' Adds a new user with the supplied <see cref="RegistrationData"/> and <paramref name="password"/>.
    ''' </summary>
    ''' <param name="user">The registration information for this user.</param>
    ''' <param name="password">The password for the new user.</param>
    <Invoke(HasSideEffects:=True)>
    Public Function CreateUser(user As RegistrationData,
        <Required(ErrorMessage:="This field is required")>
        <RegularExpression("^.*[^a-zA-Z0-9].*$", ErrorMessage:="A password needs to contain at least one special character e.g. @ or #")>
        <StringLength(50, MinimumLength:=7, ErrorMessage:="Password must be at least 7 and at most 50 characters long")>
        password As String) As IEnumerable(Of String)

        If user Is Nothing Then
            Throw New ArgumentNullException("user")
        End If

        Dim appUser As New ApplicationUser()
        With appUser
            .UserName = user.UserName
            .Email = user.Email
            .FriendlyName = user.FriendlyName
            .PasswordQuestion = user.Question
            .PasswordAnswer = UserManager.PasswordHasher.HashPassword(user.Answer)
        End With

        Dim result As IdentityResult = UserManager.Create(appUser, password)

        If Not result.Succeeded Then
            Return result.Errors
        End If

        Return Enumerable.Empty(Of String)

    End Function

End Class