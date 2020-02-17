using System;
using System.Collections.Generic;
using MazdaInventory.Commons;
using MazdaInventory.ConnectionManager;
using MazdaInventory.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MazdaInventory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DealerSearchPage : ContentPage, IConnectionCallbacks
    {
        public Boolean isDealer = false;


        public DealerSearchPage(Boolean isDealer)
        {
            InitializeComponent();
            IsBusy = false;
            this.isDealer = isDealer;
            this.Title = "DEALER SEARCH";

            // Define the binding context
            BindingContext = this;


            if (isDealer)
            {
                corpHeading.IsVisible = false;
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
            IsBusy = true;
            int distance = 50;
            switch (radius.SelectedIndex)
            {
                case 0:
                    distance = 50;
                    break;
                case 1:
                    distance = 100;
                    break;
                case 2:
                    distance = 150;
                    break;
                case 3:
                    distance = 200;
                    break;
                case 4:
                    distance = 250;
                    break;

            }
            MainController ms = new MainController();
            ms.GetDealerData("zip", zipCode.Text, distance+"",this, "zip");
        }

        private void nextClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FilterPage());
        }

        public void ConnectionWasSuccessFullWithResult(object result, string RequestID)
        {
            Dictionary<String, Object> lDictionary = (Dictionary<String, Object>)result;
            String lContent = (String)lDictionary[RequestID];
            List<Dealer> dealers = Utilities.GetDealersFromResponse(lContent);
            IsBusy = false;
            DisplayAlert("Success", "Success","Ok");
        }

        public void ConnectionFailedWithError(object error, string RequestID)
        {
            IsBusy = false;
            DisplayAlert("Faliure", "Faliure", "Ok");
        }
    }
}