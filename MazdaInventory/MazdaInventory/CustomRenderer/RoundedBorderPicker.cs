using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MazdaInventory.CustomRenderer
{
    public class RoundedBorderPicker : Picker
    {
        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(string), typeof(RoundedBorderPicker), string.Empty);

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }
}
