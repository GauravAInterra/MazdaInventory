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
    public partial class FilterPage : ContentPage
    {
        public FilterPage()
        {
            InitializeComponent();
            this.Title = "FILTER";
        }
        private void refineDealerClicked(object sender, EventArgs e)
        {
            DisplayAlert("Refine Dealer Search Alert", "searchAlert", "OK");
        }

        private void nextClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ResultListPage());
        }
    }
}