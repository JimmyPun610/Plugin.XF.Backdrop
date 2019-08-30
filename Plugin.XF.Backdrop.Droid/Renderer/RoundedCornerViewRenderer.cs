using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.XF.Backdrop;
using Plugin.XF.Backdrop.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedCornerStackLayout), typeof(RoundedCornerStackLayoutRenderer))]
namespace Plugin.XF.Backdrop.Droid.Renderer
{
    public class RoundedCornerStackLayoutRenderer : ViewRenderer
    {
        public RoundedCornerStackLayoutRenderer(Context context) : base(context)
        { }

        protected override bool DrawChild(Canvas canvas, Android.Views.View child, long drawingTime)
        {
            if (Element == null) return false;

            var control = (RoundedCornerStackLayout)Element;
            
            //var drawable = GenerateBackgroundWithShadow(control, child, Color.White, Color.Black, 10, GravityFlags.Top);
            //return base.DrawChild(canvas, child, drawingTime);

            //child.Elevation = 15;

            SetClipChildren(true);

            control.Padding = new Thickness(0, 0, 0, 0);

            //Create path to clip the child         
            var path = new Path();
            path.AddRoundRect(new RectF(0, 0, Width, Height),
                              GetRadii(control),
                              Path.Direction.Ccw);

            canvas.Save();
            canvas.ClipPath(path);

            // Draw the child first so that the border shows up above it.        
            var result = base.DrawChild(canvas, child, drawingTime);

            canvas.Restore();

            DrawBorder(canvas, control, path);

            //Properly dispose        
            path.Dispose();
            return result;
        }

        public static Drawable GenerateBackgroundWithShadow(RoundedCornerStackLayout control, Android.Views.View child, Android.Graphics.Color backgroundColor,
                                                            Android.Graphics.Color shadowColor,
                                                            int elevation,
                                                            GravityFlags shadowGravity)
        {
            var radii = GetRadii(control);

            int DY;
            switch (shadowGravity)
            {
                case GravityFlags.Center:
                    DY = 0;
                    break;
                case GravityFlags.Top:
                    DY = -1 * elevation / 3;
                    break;
                default:
                case GravityFlags.Bottom:
                    DY = elevation / 3;
                    break;
            }

            var shapeDrawable = new ShapeDrawable();

            shapeDrawable.Paint.Color = backgroundColor;
            shapeDrawable.Paint.SetShadowLayer(elevation, 0, DY, shadowColor);

            child.SetLayerType(LayerType.Software, shapeDrawable.Paint);

            shapeDrawable.Shape = new RoundRectShape(radii, null, null);

            var drawable = new LayerDrawable(new Drawable[] { shapeDrawable });
            drawable.SetLayerInset(0, elevation, elevation, elevation, elevation);

            child.Background = drawable;
            return drawable;

        }

        private static float[] GetRadii(RoundedCornerStackLayout control)
        {
            
            var radius = (float)(control.CornerRadius);
            if(radius == -1)
            { 
                radius = (float)control.Width / 8;
            }
            else
            {
                radius *= 2;
            }

            var topLeft = control.RoundedCorners.ToLower().Contains("topleft") ? radius : 0;
            var topRight = control.RoundedCorners.ToLower().Contains("topright") ? radius : 0;
            var bottomLeft = control.RoundedCorners.ToLower().Contains("bottomleft") ? radius : 0;
            var bottomRight = control.RoundedCorners.ToLower().Contains("bottomright") ? radius : 0;

            if (control.RoundedCorners.ToLower().Contains("all"))
                topLeft = topRight = bottomLeft = bottomRight = radius;

            var radii = new[] { topLeft, topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft };
            return radii;
        }

        private static void DrawBorder(Canvas canvas, RoundedCornerStackLayout control, Path path)
        {
            if (control.BorderColor == Xamarin.Forms.Color.Transparent ||
                control.BorderThickness <= 0) return;

            var paint = new Paint();
            paint.AntiAlias = true;
            paint.StrokeWidth = control.BorderThickness;
            paint.SetStyle(Paint.Style.Stroke);
            paint.Color = control.BorderColor.ToAndroid();

            canvas.DrawPath(path, paint);

            paint.Dispose();
        }
    }
}