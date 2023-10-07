Imports OpenRiaServices.DomainServices.Server.ApplicationServices

Namespace OpenSilverBusinessApplication.Web

    ''' <summary>
    ''' Class containing information about the authenticated user.
    ''' </summary>
    Partial Public Class User
        Inherits UserBase

        ' NOTE: Profile properties can be added for use in application.
        ' To enable profiles, edit the appropriate section of web.config file.
        '
        ' public string MyProfileProperty { get; set; }

        ''' <summary>
        ''' Gets and sets the friendly name of the user.
        ''' </summary>
        Public Property FriendlyName As String
    End Class

End Namespace