using OpenRiaServices.DomainServices.Client;
using OpenSilverBusinessApplication.Web;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace OpenSilverBusinessApplication.Views
{
    public partial class About : Page
    {
        private readonly UserRegistrationContext _context = new UserRegistrationContext();

        public About()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _context.OpenEndPoint().Completed += About_Completed;
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            _context.SecureEndPoint().Completed += About_Completed;
        }

        private void About_Completed(object sender, System.EventArgs e)
        {
            var op = (InvokeOperation<DateTime>)sender;

            tbResult.Text = op.HasError
                ? op.Error.ToString()
                : op.Value.ToString();

            if (op.HasError) op.MarkErrorAsHandled();
        }

    }
}