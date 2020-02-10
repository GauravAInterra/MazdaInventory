using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MazdaInventory.CustomRenderer;
using MazdaInventory.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedBorderPicker), typeof(RoundedBorderPickerRenderer))]
namespace MazdaInventory.iOS.CustomRenderer
{
    class RoundedBorderPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            var element = (RoundedBorderPicker)this.Element;

            if (this.Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
            {
                var DropDown = UIImage.FromBundle(element.Image);
                Control.RightViewMode = UITextFieldViewMode.Always;
                Control.RightView = new UIImageView(DropDown);
            }
        }
    }
}