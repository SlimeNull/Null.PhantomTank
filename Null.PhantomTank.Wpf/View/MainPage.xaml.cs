﻿using Null.PhantomTank.Wpf.Model;
using Null.PhantomTank.Wpf.ViewModel;
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

namespace Null.PhantomTank.Wpf.View
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        private Frame ParentFrame;
        private Window ParentWindow;
        private MainWindowViewModel ParentViewModel;

        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ParentWindow = Application.Current.MainWindow;
            ParentFrame = ParentWindow.Content as Frame;
            ParentViewModel = ParentWindow.DataContext as MainWindowViewModel;

            if (ParentFrame == null || ParentViewModel == null)
                MessageBox.Show("Page Load Failed.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Click_SingleTank(object sender, RoutedEventArgs e)
        {
            StaticFunc.SwitchForwardPage(ParentFrame, ParentViewModel.SingleTankPage);
        }

        private void Click_ComplexTank(object sender, RoutedEventArgs e)
        {
            StaticFunc.SwitchForwardPage(ParentFrame, ParentViewModel.ComplexTankPage);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            CloseApplication();
        }

        private void Minimium_Click(object sender, RoutedEventArgs e)
        {
            MinimiumWindow();
        }

        public static void CloseApplication()
        {
            Duration dur = new Duration(TimeSpan.FromMilliseconds(100));
            DoubleAnimation ani = new DoubleAnimation(1, 0, dur);

            ani.Completed += (sender, e) => Application.Current.Shutdown();

            Window MainWindow = Application.Current.MainWindow;
            MainWindow.BeginAnimation(OpacityProperty, ani);
        }
        public void MinimiumWindow()
        {
            Duration dur = new Duration(TimeSpan.FromMilliseconds(100));
            DoubleAnimation ani = new DoubleAnimation(1, 0, dur, FillBehavior.Stop) { };

            ani.Completed += (sender, e) =>
            {
                ParentWindow.Opacity = 1;
                ParentWindow.WindowState = WindowState.Minimized;
            };

            Window MainWindow = Application.Current.MainWindow;
            MainWindow.BeginAnimation(OpacityProperty, ani);
        }
    }
}
