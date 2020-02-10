using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using MazdaInventory.Effects;
using Plugin.CurrentActivity;

namespace MazdaInventory.Droid.Effects
{
    public class RoundCornersEffectDroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                PrepareContainer();
                SetCornerRadius();
            }
            catch { }
        }

        protected override void OnDetached()
        {
            try
            {
                Container.OutlineProvider = ViewOutlineProvider.Background;
            }
            catch { }
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == RoundCornersEffect.CornerRadiusProperty.PropertyName)
                SetCornerRadius();
        }

        private void PrepareContainer()
        {
            Container.ClipToOutline = true;
        }

        private void SetCornerRadius()
        {
            var cornerRadius = RoundCornersEffect.GetCornerRadius(Element) * GetDensity();
            Container.OutlineProvider = new RoundedOutlineProvider(cornerRadius);
        }

        private static float GetDensity() =>
            CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.Density;

        private class RoundedOutlineProvider : ViewOutlineProvider
        {
            private readonly float _radius;

            public RoundedOutlineProvider(float radius)
            {
                _radius = radius;
            }

            public override void GetOutline(Android.Views.View view, Outline outline)
            {
                outline?.SetRoundRect(0, 0, view.Width, view.Height, _radius);
            }
        }
    }
}