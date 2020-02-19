using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml;
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
        public Boolean isZipTapped = true;
        public Boolean isDealerTapped = false;
        public Boolean isCorpTapped = false;

        ObservableCollection<Dealer> dealers = new ObservableCollection<Dealer>();
        ObservableCollection<Dealer> favDealers = new ObservableCollection<Dealer>();
        String selectedDealers = "";

        ICommand MyCommand;

        public DealerSearchPage(Boolean isDealer)
        {
            InitializeComponent();
            IsBusy = false;
            this.isDealer = isDealer;
            this.Title = "DEALER SEARCH";
            // Define the binding context
            BindingContext = this;

            dealerList.ItemsSource = dealers;
            dealerList.HeightRequest = Utilities.getHeightOfListView(dealers.Count);

            favDealerList.ItemsSource = favDealers;
            favDealerList.HeightRequest = Utilities.getHeightOfListView(favDealers.Count);

            if (isDealer)
            {
                corpHeading.IsVisible = false;
            }
            radius.SelectedIndex = 0;

            MyCommand = new Command(() =>
            {
                selectedDealers = "";
                for (int i = 0; i < dealers.Count; i++)
                {
                    Dealer item = dealers[i];

                    if (item.RowCheck)
                    {
                        selectedDealers = selectedDealers + item.Id + "|";
                    }
                }
                selectedDealers = selectedDealers.Substring(0, selectedDealers.Length - 1);
                App.Current.MainPage.DisplayAlert("Title", selectedDealers + " item have been selected", "Cancel");

            });
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
            isZipTapped = true;
            isDealerTapped = false;
            isCorpTapped = false;
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
            isZipTapped = false;
            isDealerTapped = true;
            isCorpTapped = false;
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
            isZipTapped = false;
            isDealerTapped = false;
            isCorpTapped = true;
        }
        private void searchClicked(object sender, EventArgs e)
        {
            IsBusy = true;
            dealers = new ObservableCollection<Dealer>();
            dealerList.ItemsSource = "";
            MainController ms = new MainController();
            if (isZipTapped)
            {
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
                ms.GetDealerData("zip", zipCode.Text, distance + "", this, "zip");
            }
            else if (isDealerTapped)
            {
                ms.GetDealerData("name", dealerName.Text, "", this, "name");
            }
        }

        private void DealerChecked(object sender, EventArgs e)
        {
            Plugin.InputKit.Shared.Controls.CheckBox checkBox = (Plugin.InputKit.Shared.Controls.CheckBox)sender;
            if (checkBox.IsChecked)
            {
                checkBox.BoxBackgroundColor = Color.Black;
            }
            else
            {
                checkBox.BoxBackgroundColor = Color.White;
            }
        }

        private void nextClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FilterPage());
        }

        public void ConnectionWasSuccessFullWithResult(object result, string RequestID)
        {
            IsBusy = false;
            Dictionary<String, Object> lDictionary = (Dictionary<String, Object>)result;
            String lContent = (String)lDictionary[RequestID];
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(lContent);

            XmlElement root = doc.DocumentElement;

            string resultXML = root.Attributes["status"].Value;
            Console.WriteLine("resultXML" + resultXML);
            if (resultXML.Equals("ok"))
            {
                XmlNodeList allDealer = root.GetElementsByTagName("dealers");
                XmlDocument dealersDoc = new XmlDocument();
                dealersDoc.LoadXml(allDealer[0].OuterXml);

                XmlNodeList dealerNodeList = dealersDoc.GetElementsByTagName("dealer");

                foreach (XmlNode dealerNode in dealerNodeList)
                {
                    XmlDocument dealerDoc = new XmlDocument();
                    dealerDoc.LoadXml(dealerNode.OuterXml);
                    XmlElement dealerElement = dealerDoc.DocumentElement;
                    Dealer dealer = new Dealer();
                    dealer.Id = dealerElement.Attributes["id"].Value;
                    dealer.Name = dealerElement.Attributes["name"].Value;
                    dealer.RowCheck = false;
                    dealer.CheckedCommand = MyCommand;
                    dealers.Add(dealer);
                }
                dealerList.HeightRequest = Utilities.getHeightOfListView(dealers.Count);
                dealerList.ItemsSource = dealers;
            }
            DisplayAlert("Success", "Success", "Ok");
        }

        public void ConnectionFailedWithError(object error, string RequestID)
        {
            IsBusy = false;
            DisplayAlert("Faliure", "Faliure", "Ok");
        }
    }
}