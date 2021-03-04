using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Null.PhantomTank.Wpf.View;

namespace Null.PhantomTank.Wpf.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly MainPage mainPage;
        private readonly ComplexTankPage complexTankPage;
        private readonly SingleTankPage singleTankPage;
        private Page displayPage;

        public MainWindowViewModel()
        {
            this.mainPage = new MainPage();
            this.singleTankPage = new SingleTankPage();
            this.complexTankPage = new ComplexTankPage();
        }
        public Page DisplayPage
        {
            get => displayPage; 
            set
            {
                displayPage = value;
                OnPropertyChanged("DisplayPage");
            }
        }
        public MainPage MainPage => mainPage;
        public SingleTankPage SingleTankPage => singleTankPage;
        public ComplexTankPage ComplexTankPage => complexTankPage;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
