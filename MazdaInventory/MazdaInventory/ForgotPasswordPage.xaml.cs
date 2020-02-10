using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MazdaInventory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
            this.Title = "FORGOT PASSWORD";
        }

        private void submitClicked(object sender, EventArgs e)
        {
            DisplayAlert("Submit Alert", "submitAlert", "OK");
        }
    }
}