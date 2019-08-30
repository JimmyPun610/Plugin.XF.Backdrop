# Plugin.XF.Backdrop
Xamarin Form Backdrop Control, making use of Rg.Plugins.Popup
# Installation
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

3. Popup the page
```
  await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new SampleBottomToTopPopupPage());
```

# Available parameter
```
SwipeToCloseTime(double) - The time in ms needed to close popup by swiping (default 300ms)
PageContent(View) - The content view of the popup
RoundedCorners(string[]) - "topleft", "topright", "bottomleft", "bottomright", "all", "none"
CornerRadius(double)
BorderColor(Color)
BorderThincness(float)
ShadowColor(Color)
ShadowOpacity(float)
ShadowRadius(float)
HorizontalShadowOffset(double)
VerticalShadowOffset(double)
```
