using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MazdaInventory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DealerSearchPage : ContentPage
    {
        public Boolean isDealer = false;
        public DealerSearchPage(Boolean isDealer)
        {
            InitializeComponent();
            this.isDealer = isDealer;
            this.Title = "DEALER SEARCH";

            // Define the binding context
            BindingContext = this;
            

            if (isDealer)
            {
                //corpHeading.IsVisible = false;
            }
            radius.SelectedIndex = 0;
        }

        private void OnZipTapped(object sender, EventArgs e)
        {
            zipLabel.TextColor = Color.FromHex("#101010");
            zipImage.Source = "Rectangle";
            dealerLabel.TextColor = Color.FromHex("#999999");
            dealerImage.Source = "RectangleDealerNotSelected";
            corpLabel.TextColor = Color.FromHex("#999999");
            corpImage.Source = "RectangleCorpNotSelected";
            zipContent.IsVisible = true;
            dealerContent.IsVisible = false;
            corpContent.IsVisible = false;
        }
        private void OnDealerTapped(object sender, EventArgs e)
        {
            zipLabel.TextColor = Color.FromHex("#999999");
            zipImage.Source = "RectangleZipNotSelected";
            dealerLabel.TextColor = Color.FromHex("#101010");
            dealerImage.Source = "RectangleDealerSelected";
            corpLabel.TextColor = Color.FromHex("#999999");
            corpImage.Source = "RectangleCorpNotSelected";
            zipContent.IsVisible = false;
            dealerContent.IsVisible = true;
            corpContent.IsVisible = false;
        }
        private void OnCorpTapped(object sender, EventArgs e)
        {
            zipLabel.TextColor = Color.FromHex("#999999");
            zipImage.Source = "RectangleZipNotSelected";
            dealerLabel.TextColor = Color.FromHex("#999999");
            dealerImage.Source = "RectangleDealerNotSelected";
            corpLabel.TextColor = Color.FromHex("#101010");
            corpImage.Source = "RectangleCorpSelected";
            zipContent.IsVisible = false;
            dealerContent.IsVisible = false;
            corpContent.IsVisible = true;
        }
        private void searchClicked(object sender, EventArgs e)
        {
            DisplayAlert("Search Alert", "searchAlert", "OK");
        }

        private void nextClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FilterPage());
        }
    }
}