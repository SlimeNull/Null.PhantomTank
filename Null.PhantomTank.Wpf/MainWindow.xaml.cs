using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Null.PhantomTank.Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (sender, e) => DragMove();

            this.Opacity = 0;

            DisplayPage.Content = ViewModel.MainPage;
            Loaded += (sender, e) =>
            {
                Duration dur = new Duration(TimeSpan.FromMilliseconds(100));
                DoubleAnimation ani = new DoubleAnimation(0, 1, dur);

                BeginAnimation(OpacityProperty, ani);
            };
        }
    }
}
