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
    public partial class BottomToTopBackdropPopupPage : BackdropPage, INotifyPropertyChanged
    {

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