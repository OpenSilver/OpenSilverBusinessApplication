Imports OpenRiaServices.DomainServices.Client
Imports OpenRiaServices.DomainServices.Client.ApplicationServices
Imports System
Imports System.ComponentModel.DataAnnotations

Namespace OpenSilverBusinessApplication.LoginUI

    ''' <summary>
    ''' This internal entity is used to ease the binding between the UI controls (DataForm and the label displaying a validation error) and the log on credentials entered by the user.
    ''' </summary>
    Public Class LoginInfo Inherits ComplexObject

        Private userName As String
        Private rememberMe As Boolean
        Private currentLoginOperation As LoginOperation

        ''' <summary>
        ''' Gets and sets the user name.
        ''' </summary>
        <Display(Name = "User name")>
        <Required>
        Public Property UserName As String
            Get
                Return Me.userName
            End Get
            Set(value As String)
                If Me.userName <> value
                    Me.ValidateProperty("UserName", value)
                    Me.userName = value
                    Me.RaisePropertyChanged("UserName")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a function that returns the password.
        ''' </summary>
        Friend Property PasswordAccessor As Func(Of String)

        ''' <summary>
        ''' Gets and sets the password.
        ''' </summary>
        <Required>
        Public Property Password As String
            Get
                If Me.PasswordAccessor Is Nothing Then
                    Return String.Empty
                Else
                    Return Me.PasswordAccessor()
                End If
            End Get
            Set(value As String)
                Me.ValidateProperty("Password", value)

                ' Do not store the password in a private field as it should not be stored in memory in plain-text.
                ' Instead, the supplied PasswordAccessor serves as the backing store for the value.
                Me.RaisePropertyChanged("Password")                
            End Set
        End Property

        ''' <summary>
        ''' Gets and sets the value indicating whether the user's authentication information should be recorded for future logins.
        ''' </summary>
        <Display(Name = "Keep me signed in")>
        Property RememberMe As Boolean
            Get
                Return Me.rememberMe
            End Get
            Set(value As Boolean)
                If Me.rememberMe <> value Then
                    Me.ValidateProperty("RememberMe", value)
                    Me.rememberMe = value
                    Me.RaisePropertyChanged("RememberMe")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the current login operation.
        ''' </summary>
        Friend Property CurrentLoginOperation As LoginOperation
            Get
                Return Me.currentLoginOperation
            End Get
            Set(value As LoginOperation)

                If Me.currentLoginOperation <> value Then

                    If Me.currentLoginOperation IsNot Nothing
                        RemoveHandler Me.currentLoginOperation.Completed, AddressOf Sub(s, e) Me.CurrentLoginOperationChanged()
                    End If

                    Me.currentLoginOperation = value

                    If Me.currentLoginOperation IsNot Nothing
                        AddHandler Me.currentLoginOperation.Completed, AddressOf Sub(s, e) Me.CurrentLoginOperationChanged()
                    End If

                    Me.CurrentLoginOperationChanged()
                End If

            End Set
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the user is presently being logged in.
        ''' </summary>
        <Display(AutoGenerateField = false)>
        Public Property IsLoggingIn As Boolean
            Get
                Return Me.CurrentLoginOperation IsNot Nothing AndAlso Not Me.CurrentLoginOperation.IsComplete
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the user can presently log in.
        ''' </summary>
        <Display(AutoGenerateField = false)>
        Public Property CanLogIn As Boolean
            Get
                Return Not Me.IsLoggingIn
            End Get
        End Property

        ''' <summary>
        ''' Raises operation-related property change notifications when the current login operation changes.
        ''' </summary>
        Private Sub CurrentLoginOperationChanged()
            Me.RaisePropertyChanged("IsLoggingIn")
            Me.RaisePropertyChanged("CanLogIn")
        End Sub

        ''' <summary>
        ''' Creates a new <see cref="LoginParameters"/> instance using the data stored in this entity.
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, Me.RememberMe, Nothing)
        End Function

    End Class

End Namespace