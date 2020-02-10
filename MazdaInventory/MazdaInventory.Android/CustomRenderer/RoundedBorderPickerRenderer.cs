using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Xamarin.Forms.Platform.Android;
using MazdaInventory.Droid.CustomRenderer;
using MazdaInventory.CustomRenderer;
using Xamarin.Forms;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Support.V4.Content;

[assembly: ExportRenderer(typeof(RoundedBorderPicker), typeof(RoundedBorderPickerRenderer))]
namespace MazdaInventory.Droid.CustomRenderer
{
    class RoundedBorderPickerRenderer : PickerRenderer
    {
        RoundedBorderPicker element;
        public RoundedBorderPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            element = (RoundedBorderPicker)this.Element;

            if (Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
                Control.Background = AddPickerStyles(element.Image);

        }

        public LayerDrawable AddPickerStyles(string imagePath)
        {
            Drawable border = ContextCompat.GetDrawable(this.Context, Resource.Drawable.roundedBorder);

            Drawable[] layers = { border, GetDrawable(imagePath) };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);
            return layerDrawable;
        }

        private BitmapDrawable GetDrawable(string imagePath)
        {
            var drawable = ContextCompat.GetDrawable(this.Context, Resource.Drawable.DropDown);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 40, 40, true));
            result.Gravity = Android.Views.GravityFlags.Right;

            return result;
        }

    }
}