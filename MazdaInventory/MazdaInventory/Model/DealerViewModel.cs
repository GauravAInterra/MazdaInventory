using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MazdaInventory.Model
{
    public class DealerViewModel
    {

        public ObservableCollection<Dealer> dlrCollections { get; set; }
        public ObservableCollection<Dealer> selectedList;

        ICommand MyCommand;

        public DealerViewModel()
        {
            MyCommand = new Command(() =>
            {
                selectedList = new ObservableCollection<Dealer>();

                for (int i = 0; i < dlrCollections.Count; i++)
                {
                    Dealer item = dlrCollections[i];

                    if (item.RowCheck)
                    {
                        selectedList.Add(item);
                    }
                }

                App.Current.MainPage.DisplayAlert("Title", selectedList.Count + " item have been selected", "Cancel");

            });

        }
    }
}
