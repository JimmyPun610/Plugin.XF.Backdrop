using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.XF.Backdrop.Sample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SampleBottomToTopPopupPage : Plugin.XF.Backdrop.BottomToTopBackdropPopupPage
    {
        string url = "https://google.com";
        public SampleBottomToTopPopupPage()
        {
            InitializeComponent();
            AddressLabel.Text = url;
            UrlWebViewSource source = new UrlWebViewSource();
            source.Url = url;
            WebPage.Source = source;
        }
    }
}