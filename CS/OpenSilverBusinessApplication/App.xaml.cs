﻿#if OPENSILVER

using OpenRiaServices.DomainServices.Client;
using OpenRiaServices.DomainServices.Client.ApplicationServices;
using OpenSilverBusinessApplication.Web;

#endif

using System;
using System.Windows;

namespace OpenSilverBusinessApplication
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.Application_Startup;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();

            WebContext webContext = new WebContext();
            this.ApplicationLifetimeObjects.Add(webContext);

            ((DomainClientFactory)DomainContext.DomainClientFactory).ServerBaseUri = new Uri("http://localhost:54837/", UriKind.Absolute);

            //webContext.Authentication = new WindowsAuthentication();
            webContext.Authentication = new FormsAuthentication()
            {
                DomainContext = new AuthenticationContext()
            };
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // This will enable you to bind controls in XAML to WebContext.Current properties.
            this.Resources.Add("WebContext", WebContext.Current);

            // This will automatically authenticate a user when using Windows authentication or when the user chose "Keep me signed in" on a previous login attempt.
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            this.RootVisual = new MainPage();
        }

        /// <summary>
        /// Invoked when the <see cref="LoadUserOperation"/> completes.
        /// Use this event handler to switch from the "loading UI" you created in <see cref="InitializeRootVisual"/> to the "application UI".
        /// </summary>
        private void Application_UserLoaded(LoadUserOperation operation)
        {
            if (operation.HasError)
            {
                ErrorWindow.Show(operation.Error);
                operation.MarkErrorAsHandled();
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                e.Handled = true;
                ErrorWindow.Show(e.ExceptionObject);
            }
        }
    }
}