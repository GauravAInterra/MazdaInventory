using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MazdaInventory.Droid.CustomRenderer;
using MazdaInventory.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedBorderEntry), typeof(RoundedBorderEntryRenderer))]
namespace MazdaInventory.Droid.CustomRenderer
{
    public class RoundedBorderEntryRenderer : EntryRenderer
    {
        public RoundedBorderEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = Context.GetDrawable(Resource.Drawable.roundedBorder);
            }
        }
    }
}