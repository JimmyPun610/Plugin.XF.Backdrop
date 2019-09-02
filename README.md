# Plugin.XF.Backdrop
Xamarin Form Backdrop Control, making use of Rg.Plugins.Popup

# Supporting Platform
Android, iOS10+

# Preview
<table>
  <tr>
    <td>iOS
    </td>
      <td>Android
        </td>
  </tr>
    <tr>
    <td><img src="https://github.com/JimmyPun610/Plugin.XF.Backdrop/blob/master/iOSPreview.gif" width="200">
    </td>
      <td><img src="https://github.com/JimmyPun610/Plugin.XF.Backdrop/blob/master/AndroidPreview.gif" width="200">
        </td>
  </tr>
  </table>
  

# Installation
Install nuget package to all your project
```
Install-Package Plugin.XF.Backdrop -Version 1.0.0
```
# How to use
Please refer to Sample project for more information
1. In iOS project, in your AppDelegate.cs add below code
```
  LoadApplication(new App());
  Rg.Plugins.Popup.Popup.Init();
  Plugin.XF.Backdrop.iOS.Initializer.Init();
  return base.FinishedLaunching(app, options);
```

2. In Forms project, create a bottom to top backdrop popup page, set the page content and parameter if needed
```
<?xml version="1.0" encoding="utf-8" ?>
<backdrop:BottomToTopBackdropPopupPage xmlns="http://xamarin.com/schemas/2014/forms"
                                       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                                       xmlns:d="http://xamarin.com/schemas/2014/forms/design"
                                       xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
                                       mc:Ignorable="d"
                                       xmlns:backdrop="clr-namespace:Plugin.XF.Backdrop;assembly=Plugin.XF.Backdrop"
                                       x:Class="Plugin.XF.Backdrop.Sample.SampleBottomToTopPopupPage"
                                       RoundedCorners="topleft,topright" ShadowOpacity="0" BorderThickness="3" ShadowColor="Transparent"
                                       BorderColor="White" CornerRadius="-1" IndicatorColor="White">
    <backdrop:BottomToTopBackdropPopupPage.PageContent>
        <ContentView>
            <StackLayout BackgroundColor="White" HorizontalOptions="FillAndExpand" VerticalOptions="FillAndExpand" Spacing="0">
                <ContentView BackgroundColor="White" Padding="10" VerticalOptions="Center">
                    <Label x:Name="AddressLabel"/>
                </ContentView>
                <ContentView HeightRequest="1" HorizontalOptions="FillAndExpand" BackgroundColor="Gray"/>
                <WebView x:Name="WebPage" HorizontalOptions="FillAndExpand" VerticalOptions="FillAndExpand"/>
            </StackLayout>
        </ContentView>
    </backdrop:BottomToTopBackdropPopupPage.PageContent>
</backdrop:BottomToTopBackdropPopupPage>
```
3. Popup the page
```
  await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new SampleBottomToTopPopupPage());
```

# Available parameter
```
SwipeToCloseTime(double) - The time in ms needed to close popup by swiping (default 300ms)
PageContent(View) - The content view of the popup
IndicatorColor(Color) - Top indicator color
RoundedCorners(string[]) - "topleft", "topright", "bottomleft", "bottomright", "all", "none"
CornerRadius(double) - "-1" = Auto
BorderColor(Color)
BorderThincness(float)
ShadowColor(Color)
ShadowOpacity(float)
ShadowRadius(float)
HorizontalShadowOffset(double)
VerticalShadowOffset(double)
```
