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
    /// ComplexTankPage.xaml 的交互逻辑
    /// </summary>
    public partial class ComplexTankPage : Page
    {
        private Frame ParentFrame;
        private MainWindowViewModel ParentViewModel;

        public ComplexTankPage()
        {
            InitializeComponent();
            Loaded += ComplexTankPage_Loaded;
        }

        private void ComplexTankPage_Loaded(object sender, RoutedEventArgs e)
        {
            Window ParentWindow = Application.Current.MainWindow;
            ParentFrame = ParentWindow.Content as Frame;
            ParentViewModel = ParentWindow.DataContext as MainWindowViewModel;

            if (ParentFrame == null || ParentViewModel == null)
                MessageBox.Show("Error");
        }

        private void Click_Return(object sender, RoutedEventArgs e)
        {
            if (ParentFrame.CanGoBack)
                StaticFunc.SwitchGoBack(ParentFrame);
        }
        #region 图片显示处鼠标Enter Leave事件 (用于展示通知消息)
        private void Input1_MouseEnter(object sender, MouseEventArgs e)
        {
            ViewModel.Input1TopNoty = true;
        }

        private void Input2_MouseEnter(object sender, MouseEventArgs e)
        {
            ViewModel.Input2TopNoty = true;
        }

        private void Output_MouseEnter(object sender, MouseEventArgs e)
        {
            ViewModel.OutputTopNoty = true;
        }

        private void Output_MouseLeave(object sender, MouseEventArgs e)
        {
            ViewModel.OutputTopNoty = false;
        }

        private void Input2_MouseLeave(object sender, MouseEventArgs e)
        {
            ViewModel.Input2TopNoty = false;
        }

        private void Input1_MouseLeave(object sender, MouseEventArgs e)
        {
            ViewModel.Input1TopNoty = false;
        }
        #endregion

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
                ImageConvertHelper.BeginLoadSource((System.Drawing.Bitmap)data.GetData(DataFormats.Bitmap), (newSrc) => ViewModel.Input1Source = newSrc);
            }
            else if (data.GetDataPresent(DataFormats.FileDrop))
            {

            }
        }

        private void Input2Drop(object sender, DragEventArgs e)
        {

        }
    }
}
