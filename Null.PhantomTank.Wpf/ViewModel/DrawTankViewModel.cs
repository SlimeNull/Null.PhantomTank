using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Null.PhantomTank.Wpf.ViewModel
{
    class DrawTankViewModel : INotifyPropertyChanged
    {
        private bool blackBackground;
        private bool whiteBackground = true;
        private bool input1TopNoty;
        private bool input1BottomNoty = true;
        private bool input2TopNoty;
        private bool input2BottomNoty = true;
        private bool outputTopNoty;
        private bool outputBottomNoty = true;
        private ImageSource input1Source;
        private ImageSource input2Source;
        private ImageSource outputSource;
        private double constrastRatio;

        public bool Input1TopNoty
        {
            get => input1TopNoty; set
            {
                input1TopNoty = value;
                OnPropertyChanged("Input1TopNoty");
            }
        }
        public bool Input1BottomNoty
        {
            get => input1BottomNoty; set
            {
                input1BottomNoty = value;
                OnPropertyChanged("Input1BottomNoty");
            }
        }
        public bool Input2TopNoty
        {
            get => input2TopNoty; set
            {
                input2TopNoty = value;
                OnPropertyChanged("Input2TopNoty");
            }
        }
        public bool Input2BottomNoty
        {
            get => input2BottomNoty; set
            {
                input2BottomNoty = value;
                OnPropertyChanged("Input2BottomNoty");
            }
        }
        public bool OutputTopNoty
        {
            get => outputTopNoty; set
            {
                outputTopNoty = value;
                OnPropertyChanged("OutputTopNoty");
            }
        }
        public bool OutputBottomNoty
        {
            get => outputBottomNoty; set
            {
                outputBottomNoty = value;
                OnPropertyChanged("OutputBottomNoty");
            }
        }
        public string Input1TopNotyText
        {
            get
            {
                return Input1Source == null ? "单击或拖动图片到这里以导入" : "双击以重置图片输入源";
            }
        }
        public string Input2TopNotyText
        {
            get
            {
                return Input1Source == null ? "单击或拖动图片到这里以导入" : "双击以重置图片输入源";
            }
        }
        public string OutputTopNotyText
        {
            get
            {
                return (Input1Source == null || Input2Source == null) ? "请先导入源图像" : (OutputSource == null ? "单击以进行渲染" : "单击以重新渲染");
            }
        }
        public string Input1BottomNotyText
        {
            get
            {
                return Input1Source == null ? "尚未导入" : "";
            }
        }
        public string Input2BottomNotyText
        {
            get
            {
                return Input2Source == null ? "尚未导入" : "";
            }
        }
        public string OutputBottomNotyText
        {
            get
            {
                return OutputSource == null ? "尚未渲染" : "";
            }
        }
        public ImageSource Input1Source
        {   
            get => input1Source; set
            {
                input1Source = value;
                OnPropertyChanged("Input1Source");
                OnPropertyChanged("Input1TopNotyText");
                OnPropertyChanged("Input1BottomNotyText");
            }
        }
        public ImageSource Input2Source
        {
            get => input2Source; set
            {
                input2Source = value;
                OnPropertyChanged("Input2Source");
                OnPropertyChanged("Input2TopNotyText");
                OnPropertyChanged("Input2BottomNotyText");
            }
        }
        public ImageSource OutputSource
        {
            get => outputSource; set
            {
                outputSource = value;
                OnPropertyChanged("OutputSource");
                OnPropertyChanged("OutputTopNotyText");
                OnPropertyChanged("OutputBottomNotyText");
            }
        }
        public double ConstrastRatio
        {
            get => constrastRatio; set
            {
                constrastRatio = value;
                OnPropertyChanged("ConstrastRatio");
            }
        }
        public bool BlackBackground
        {
            get => blackBackground; set
            {
                blackBackground = value;
                OnPropertyChanged("BlackBackground");
                OnPropertyChanged("OutputPanelBackground");
            }
        }
        public bool WhiteBackground
        {
            get => whiteBackground; set
            {
                whiteBackground = value;
                OnPropertyChanged("WhiteBackground");
                OnPropertyChanged("OutputPanelBackground");
            }
        }
        public Brush OutputPanelBackground { get => BlackBackground ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White); }

        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
