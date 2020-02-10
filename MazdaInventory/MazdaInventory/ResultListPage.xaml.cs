using MazdaInventory.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MazdaInventory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultListPage : ContentPage
    {
        public ResultListPage()
        {
            InitializeComponent();
            this.Title = "RESULTS";
            this.BindingContext = new CarViewModel();
        }
    }
}