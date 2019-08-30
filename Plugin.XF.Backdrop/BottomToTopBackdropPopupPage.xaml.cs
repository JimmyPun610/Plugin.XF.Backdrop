using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.XF.Backdrop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomToTopBackdropPopupPage : Rg.Plugins.Popup.Pages.PopupPage, INotifyPropertyChanged
    {

        public static readonly BindableProperty RoundedCornersProperty = BindableProperty.Create(nameof(RoundedCorners), typeof(string), typeof(RoundedCornerStackLayout), "All", validateValue: OnRoundedCornersPropertyValidateValue);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(RoundedCornerStackLayout), Convert.ToDouble(-1));
        public static readonly BindableProperty ShadowColorProperty = BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(RoundedCornerStackLayout), Color.Black);
        public static readonly BindableProperty VerticalShadowOffsetProperty = BindableProperty.Create(nameof(VerticalShadowOffset), typeof(double), typeof(RoundedCornerStackLayout), -1d);
        public static readonly BindableProperty HorizontalShadowOffsetProperty = BindableProperty.Create(nameof(HorizontalShadowOffset), typeof(double), typeof(RoundedCornerStackLayout), 1d);
        public static readonly BindableProperty ShadowOpacityProperty = BindableProperty.Create(nameof(ShadowOpacity), typeof(float), typeof(RoundedCornerStackLayout), 0.8f);
        public static readonly BindableProperty ShadowRadiusProperty = BindableProperty.Create(nameof(ShadowRadius), typeof(float), typeof(RoundedCornerStackLayout), 2.0f);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(RoundedCornerStackLayout), Color.Transparent);
        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(float), typeof(RoundedCornerStackLayout), 2f);

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

        DateTimeOffset? StartPanDownTime = null;
        DateTimeOffset? EndPanDownTime = null;
        double StartY;
        public BottomToTopBackdropPopupPage() 
        {
            InitializeComponent();
            SetPanListener();
            this.BindingContext = this;
        }
        
        private void SetPanListener()
        {
            if(Device.RuntimePlatform == Device.Android)
            {
                CoverView.IsVisible = true;
                PanGestureRecognizer panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += AndroidPanGestureRecognizer_PanUpdated;
                CoverView.GestureRecognizers.Add(panGestureRecognizer);
            }
            else if(Device.RuntimePlatform == Device.iOS){
                CoverView.IsVisible = false;
                PanGestureRecognizer panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += iOSPanGestureRecognizer_PanUpdated;
                FullView.GestureRecognizers.Add(panGestureRecognizer);
            }
        }
        private async void iOSPanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    StartPanDownTime = DateTimeOffset.Now;
                    break;

                case GestureStatus.Running:
                    if (e.TotalY > 0)
                    {
                        Indictor.TranslateTo(0, e.TotalY + StartY, 20, Easing.Linear);
                        PageView.TranslateTo(0, e.TotalY, 20, Easing.Linear);
                    }
                        
                    break;

                case GestureStatus.Completed:
                    EndPanDownTime = DateTimeOffset.Now;
                    if (EndPanDownTime.Value.ToUnixTimeMilliseconds() - StartPanDownTime.Value.ToUnixTimeMilliseconds() < SwipeToCloseTime)
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                    else
                    {
                        Indictor.TranslateTo(0, 0, 250, Easing.Linear);
                        PageView.TranslateTo(0, 0, 250, Easing.Linear);
                    }
                    break;
            }

            if (e.StatusType == GestureStatus.Completed || e.StatusType == GestureStatus.Canceled)
            {
                StartPanDownTime = null;
                EndPanDownTime = null;
            }

        }

        private async void AndroidPanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            ContentView view = sender as ContentView;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    StartPanDownTime = DateTimeOffset.Now;
                    StartY = view.Y;
                    break;

                case GestureStatus.Running:
                    if (e.TotalY > 0)
                    {
                        Indictor.TranslateTo(0, e.TotalY + StartY, 20, Easing.Linear);
                        PageView.TranslateTo(0, e.TotalY + StartY, 20, Easing.Linear);
                    }
                    break;

                case GestureStatus.Completed:
                    EndPanDownTime = DateTimeOffset.Now;
                    if (EndPanDownTime.Value.ToUnixTimeMilliseconds() - StartPanDownTime.Value.ToUnixTimeMilliseconds() < SwipeToCloseTime)
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                    else
                    {
                        PageView.TranslateTo(0, 0, 250, Easing.Linear);
                        Indictor.TranslateTo(0, 0, 250, Easing.Linear);
                    }
                    break;
            }
            
            if(e.StatusType == GestureStatus.Completed || e.StatusType == GestureStatus.Canceled)
            {
                StartPanDownTime = null;
                EndPanDownTime = null;
            }

        }


        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

    }
}