using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Null.PhantomTank.Wpf.Model
{
    public static class StaticFunc
    {
        public static void SwitchForwardPage(Frame frame, Page pageTo)
        {
            TranslateTransform translate = new TranslateTransform();
            frame.RenderTransform = translate;

            Duration dur1 = new Duration(TimeSpan.FromMilliseconds(100));
            Duration dur2 = new Duration(TimeSpan.FromMilliseconds(80));

            DoubleAnimation ani1 = new DoubleAnimation(1, 0, dur1);
            DoubleAnimation ani2 = new DoubleAnimation(0, -50, dur1);

            DoubleAnimation ani3 = new DoubleAnimation(0, 1, dur2);
            DoubleAnimation ani4 = new DoubleAnimation(50, 0, dur2);

            ani4.DecelerationRatio = 1;

            ani2.Completed += (sender, e) =>
            {
                ani3.Completed += (sender2, e2) => frame.Opacity = 1;
                ani4.Completed += (sender2, e2) => frame.RenderTransform = null;

                frame.Navigate(pageTo);
                frame.BeginAnimation(Frame.OpacityProperty, ani3);
                translate.BeginAnimation(TranslateTransform.XProperty, ani4);
            };

            frame.BeginAnimation(Frame.OpacityProperty, ani1);
            translate.BeginAnimation(TranslateTransform.XProperty, ani2);
        }
        public static void SwitchBackTo(Frame frame, Page pageTo)
        {
            TranslateTransform translate = new TranslateTransform();
            frame.RenderTransform = translate;

            Duration dur1 = new Duration(TimeSpan.FromMilliseconds(100));
            Duration dur2 = new Duration(TimeSpan.FromMilliseconds(80));

            DoubleAnimation ani1 = new DoubleAnimation(1, 0, dur2);
            DoubleAnimation ani2 = new DoubleAnimation(0, 50, dur1);

            DoubleAnimation ani3 = new DoubleAnimation(0, 1, dur1);
            DoubleAnimation ani4 = new DoubleAnimation(-50, 0, dur2);

            ani2.Completed += (sender, e) =>
            {
                ani3.Completed += (sender2, e2) => frame.Opacity = 1;
                ani4.Completed += (sender2, e2) => frame.RenderTransform = null;

                frame.Navigate(pageTo);
                frame.BeginAnimation(Frame.OpacityProperty, ani3);
                translate.BeginAnimation(TranslateTransform.XProperty, ani4);
            };

            frame.BeginAnimation(Frame.OpacityProperty, ani1);
            translate.BeginAnimation(TranslateTransform.XProperty, ani2);
        }
        public static void SwitchGoForward(Frame frame)
        {
            TranslateTransform translate = new TranslateTransform();
            frame.RenderTransform = translate;

            Duration dur1 = new Duration(TimeSpan.FromMilliseconds(100));
            Duration dur2 = new Duration(TimeSpan.FromMilliseconds(80));

            DoubleAnimation ani1 = new DoubleAnimation(1, 0, dur1);
            DoubleAnimation ani2 = new DoubleAnimation(0, -50, dur1);

            DoubleAnimation ani3 = new DoubleAnimation(0, 1, dur2);
            DoubleAnimation ani4 = new DoubleAnimation(50, 0, dur2);

            ani4.DecelerationRatio = 1;

            ani2.Completed += (sender, e) =>
            {
                ani3.Completed += (sender2, e2) => frame.Opacity = 1;
                ani4.Completed += (sender2, e2) => frame.RenderTransform = null;

                frame.GoForward();
                frame.BeginAnimation(Frame.OpacityProperty, ani3);
                translate.BeginAnimation(TranslateTransform.XProperty, ani4);
            };

            frame.BeginAnimation(Frame.OpacityProperty, ani1);
            translate.BeginAnimation(TranslateTransform.XProperty, ani2);
        }
        public static void SwitchGoBack(Frame frame)
        {
            TranslateTransform translate = new TranslateTransform();
            frame.RenderTransform = translate;

            Duration dur1 = new Duration(TimeSpan.FromMilliseconds(100));
            Duration dur2 = new Duration(TimeSpan.FromMilliseconds(80));

            DoubleAnimation ani1 = new DoubleAnimation(1, 0, dur2);
            DoubleAnimation ani2 = new DoubleAnimation(0, 50, dur1);

            DoubleAnimation ani3 = new DoubleAnimation(0, 1, dur1);
            DoubleAnimation ani4 = new DoubleAnimation(-50, 0, dur2);

            ani2.Completed += (sender, e) =>
            {
                ani3.Completed += (sender2, e2) => frame.Opacity = 1;
                ani4.Completed += (sender2, e2) => frame.RenderTransform = null;

                frame.GoBack();
                frame.BeginAnimation(Frame.OpacityProperty, ani3);
                translate.BeginAnimation(TranslateTransform.XProperty, ani4);
            };

            frame.BeginAnimation(Frame.OpacityProperty, ani1);
            translate.BeginAnimation(TranslateTransform.XProperty, ani2);
        }
    }
}
