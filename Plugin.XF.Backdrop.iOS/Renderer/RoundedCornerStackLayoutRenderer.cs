using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Plugin.XF.Backdrop;
using Plugin.XF.Backdrop.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedCornerStackLayout), typeof(RoundedCornerStackLayoutRenderer))]
namespace Plugin.XF.Backdrop.iOS.Renderer
{

    public class RoundedCornerStackLayoutRenderer : ViewRenderer
    {
        private bool _isDisposed;
        public static void Init() { }
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (Element == null) return;

            Element.PropertyChanged += OnElementOnPropertyChanged;
        }

        private void OnElementOnPropertyChanged(object sender, PropertyChangedEventArgs e1)
        {
            if (_isDisposed || NativeView == null) return;

            NativeView.SetNeedsDisplay();
            NativeView.SetNeedsLayout();
        }

        public override void Draw(CGRect rect)
        {
            var view = (RoundedCornerStackLayout)Element;

            UIRectCorner corners = 0;

            if (view.RoundedCorners.ToLower().Contains("topleft"))
                corners = corners | UIRectCorner.TopLeft;

            if (view.RoundedCorners.ToLower().Contains("topright"))
                corners = corners | UIRectCorner.TopRight;

            if (view.RoundedCorners.ToLower().Contains("bottomright"))
                corners = corners | UIRectCorner.BottomRight;

            if (view.RoundedCorners.ToLower().Contains("bottomleft"))
                corners = corners | UIRectCorner.BottomLeft;

            if (view.RoundedCorners.ToLower().Contains("all"))
                corners = UIRectCorner.AllCorners;
            var radius = view.CornerRadius;
            if (radius == -1)
                radius = (float)view.Width / 16;
            var mPath = UIBezierPath.FromRoundedRect(Layer.Bounds, corners, new CGSize(radius, radius)).CGPath;


            Layer.ShadowColor = view.ShadowColor.ToCGColor();
            Layer.ShadowOffset = new CGSize(view.HorizontalShadowOffset, view.VerticalShadowOffset);
            Layer.ShadowOpacity = view.ShadowOpacity;
            Layer.ShadowRadius = view.ShadowRadius;


            if (Layer.Sublayers == null || Layer.Sublayers.Length <= 0) return;

            var subLayer = this.Layer.Sublayers[0];
            subLayer.CornerRadius = (float)radius;
            subLayer.Mask = new CAShapeLayer
            {
                Frame = Layer.Bounds,
                Path = mPath,
            };
        }

        protected override void Dispose(bool disposing)
        {
            Element.PropertyChanged -= OnElementOnPropertyChanged;
            base.Dispose(disposing);
            _isDisposed = true;
        }
    }

}