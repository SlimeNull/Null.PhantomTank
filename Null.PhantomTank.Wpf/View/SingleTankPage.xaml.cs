using Microsoft.Win32;
using Null.PhantomTank.Wpf.Model;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Null.PhantomTank.Wpf.View
{
    /// <summary>
    /// SingleTankPage.xaml 的交互逻辑
    /// </summary>
    public partial class SingleTankPage : Page
    {
        private Frame ParentFrame;
        private MainWindowViewModel ParentViewModel;

        public SingleTankPage()
        {
            InitializeComponent();
            Loaded += SingleTankPage_Loaded;
        }

        private void SingleTankPage_Loaded(object sender, RoutedEventArgs e)
        {
            Window ParentWindow = Application.Current.MainWindow;
            ParentFrame = ParentWindow.Content as Frame;
            ParentViewModel = ParentWindow.DataContext as MainWindowViewModel;

            if (ParentFrame == null || ParentViewModel == null)
                MessageBox.Show("Page Load Failed.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Click_Return(object sender, RoutedEventArgs e)
        {
            if (ParentFrame.CanGoBack)
                StaticFunc.SwitchGoBack(ParentFrame);
        }

        private void InputDragEnter(object sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            if (data.GetDataPresent(DataFormats.Bitmap) || data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Handled = true;
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void Input1Drop(object sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            if (data.GetDataPresent(DataFormats.Bitmap))
            {
                var bmp = (System.Drawing.Bitmap)data.GetData(DataFormats.Bitmap);
                ViewModel.Input1 = bmp;
            }
            else if (data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                    ImageConvertHelper.BeginLoadImage(files[0], (newImg) =>
                    {
                        ViewModel.Input1 = newImg;
                    });
            }
        }

        OpenFileDialog ofd = new OpenFileDialog()
        {
            Title = "选择一个图片",
            Filter = "All|*.jpg;*.jpeg;*.png;*.gif;*.tiff|JPEG|*.jpg;*jpeg|PNG|*png|GIF|*gif|TIFF|*.tiff",
            CheckFileExists = true,
            Multiselect = false
        };

        private void ImportSource1(object sender, MouseButtonEventArgs e)
        {
            if (ofd.ShowDialog().GetValueOrDefault(false))
            {
                ImageConvertHelper.BeginLoadImage(ofd.FileName, (newImg) =>
                {
                    ViewModel.Input1 = newImg;
                });
            }
        }

        private void ResetInput1(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Input1 = null;
        }

        private void ResetOutput(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Output = null;
        }

        SaveFileDialog sfd = new SaveFileDialog()
        {
            FileName = "PhantomTank.png",
            Filter = "All|*.jpg;*.jpeg;*.png;*.gif;*.tiff|JPEG|*.jpg;*jpeg|PNG|*png|GIF|*gif|TIFF|*.tiff",
            CheckPathExists = true,
        };
        private void OutputClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.OutputSource == null)
            {
                var src1 = ViewModel.Input1;
                if (src1 != null)
                {
                    ImageConvertHelper.BeginConvertImage(src1, ViewModel.TankType, (img) =>
                    {
                        ViewModel.Output = img;
                    });
                }
                else
                {
                    MessageBox.Show("在渲染前, 请先指定输入源!", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (sfd.ShowDialog().GetValueOrDefault(false))
                {
                    ViewModel.Output.Save(sfd.FileName);
                }
            }
        }

        private void ResetOptions(object sender, RoutedEventArgs e)
        {
            ViewModel.AppearOnBlack = true;
            ViewModel.Lightness = 127;
        }

        private void SaveOutput(object sender, RoutedEventArgs e)
        {
            if (sfd.ShowDialog().GetValueOrDefault(false))
            {
                ViewModel.Output.Save(sfd.FileName);
            }
        }
    }
}
