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
    }
}
