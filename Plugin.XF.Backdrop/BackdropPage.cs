using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Plugin.XF.Backdrop
{
    public class BackdropPage : Rg.Plugins.Popup.Pages.PopupPage, INotifyPropertyChanged
    {
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
        
        public DateTimeOffset? StartPanDownTime = null;
        public DateTimeOffset? EndPanDownTime = null;
        public double StartY;

        public static readonly BindableProperty RoundedCornersProperty = BindableProperty.Create(nameof(RoundedCorners), typeof(string), typeof(RoundedCornerStackLayout), "All", validateValue: OnRoundedCornersPropertyValidateValue);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(RoundedCornerStackLayout), Convert.ToDouble(-1));
        public static readonly BindableProperty ShadowColorProperty = BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(RoundedCornerStackLayout), Color.Black);
        public static readonly BindableProperty VerticalShadowOffsetProperty = BindableProperty.Create(nameof(VerticalShadowOffset), typeof(double), typeof(RoundedCornerStackLayout), -1d);
        public static readonly BindableProperty HorizontalShadowOffsetProperty = BindableProperty.Create(nameof(HorizontalShadowOffset), typeof(double), typeof(RoundedCornerStackLayout), 1d);
        public static readonly BindableProperty ShadowOpacityProperty = BindableProperty.Create(nameof(ShadowOpacity), typeof(float), typeof(RoundedCornerStackLayout), 0.8f);
        public static readonly BindableProperty ShadowRadiusProperty = BindableProperty.Create(nameof(ShadowRadius), typeof(float), typeof(RoundedCornerStackLayout), 2.0f);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(RoundedCornerStackLayout), Color.Transparent);
        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(float), typeof(RoundedCornerStackLayout), 2f);
        public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create(nameof(IndicatorColor), typeof(Color), typeof(RoundedCornerStackLayout), Color.White);

        public Color IndicatorColor
        {
            get => (Color)GetValue(IndicatorColorProperty);
            set => SetValue(IndicatorColorProperty, value);
        }

        public string RoundedCorners
        {
            get => (string)GetValue(RoundedCornersProperty);
            set => SetValue(RoundedCornersProperty, value);
        }

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public float BorderThickness
        {
            get => (float)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }

        public double VerticalShadowOffset
        {
            get => (double)GetValue(VerticalShadowOffsetProperty);
            set => SetValue(VerticalShadowOffsetProperty, value);
        }

        public double HorizontalShadowOffset
        {
            get => (double)GetValue(HorizontalShadowOffsetProperty);
            set => SetValue(HorizontalShadowOffsetProperty, value);
        }

        public float ShadowOpacity
        {
            get => (float)GetValue(ShadowOpacityProperty);
            set => SetValue(ShadowOpacityProperty, value);
        }

        public float ShadowRadius
        {
            get => (float)GetValue(ShadowRadiusProperty);
            set => SetValue(ShadowRadiusProperty, value);
        }

        private static bool OnRoundedCornersPropertyValidateValue(BindableObject bindable, object value)
        {
            var allowedValues = new string[] { "topleft", "topright", "bottomleft", "bottomright", "all", "none" };

            return value.ToString().Split(',').Select(x => x.Trim().ToLower())
                                              .All(item => allowedValues.Contains(item));
        }

        /// <summary>
        /// Your ContentView
        /// </summary>
        public static readonly BindableProperty PageContentProperty =
BindableProperty.Create<BottomToTopBackdropPopupPage, View>(w => w.PageContent, default(View));

        public View PageContent
        {
            get { return (ContentView)GetValue(PageContentProperty); }
            set { SetValue(PageContentProperty, value); }
        }

        /// <summary>
        /// How fast with the swipe will trigger closing the popup, in Milliseconds
        /// </summary>
        public static readonly BindableProperty SwipeToCloseTimeProperty =
BindableProperty.Create<BottomToTopBackdropPopupPage, double>(w => w.SwipeToCloseTime, Convert.ToDouble(300));

        public double SwipeToCloseTime
        {
            get { return (double)GetValue(SwipeToCloseTimeProperty); }
            set { SetValue(SwipeToCloseTimeProperty, value); }
        }
    }
}
