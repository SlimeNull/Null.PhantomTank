using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Null.PhantomTank;
using Null.PhantomTank.Wpf.Model;

namespace Null.PhantomTank.Wpf.ViewModel
{
    class SingleTankViewModel : INotifyPropertyChanged
    {
        private bool input1TopNoty;
        private bool outputTopNoty;
        private BitmapImage input1Source;
        private BitmapImage outputSource;
        private System.Drawing.Image input1;
        private System.Drawing.Bitmap output;
        private bool appearOnBlack = true;
        private bool appearOnWhite;
        private int lightness = 127;

        public int Lightness
        {
            get => lightness; set
            {
                lightness = value;
                OnPropertyChanged("Lightness");
                OnPropertyChanged("OutputPanelBackground");
            }
        }
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
            get => input1Source == null;
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
            get => outputSource == null;
        }
        public bool AppearOnWhite
        {
            get => appearOnWhite; set
            {
                appearOnWhite = value;
                OnPropertyChanged("AppearOnWhite");
            }
        }
        public bool AppearOnBlack
        {
            get => appearOnBlack; set
            {
                appearOnBlack = value;
                OnPropertyChanged("AppearOnBlack");
            }
        }
        public string Input1TopNotyText
        {
            get
            {
                return Input1Source == null ? "单击或拖动图片到这里以导入" : "右击以重置图片输入源";
            }
        }
        public string OutputTopNotyText
        {
            get
            {
                return (Input1Source == null) ? "请先导入源图像" : (OutputSource == null ? "单击以进行渲染" : "单击保存或右击重置图像");
            }
        }
        public string Input1BottomNotyText
        {
            get
            {
                return Input1Source == null ? "尚未导入" : "";
            }
        }
        public string OutputBottomNotyText
        {
            get
            {
                return OutputSource == null ? "尚未渲染" : "";
            }
        }
        public System.Drawing.Image Input1
        {
            get => input1; set
            {
                if (input1 != null)
                    input1.Dispose();

                input1 = value;

                if (value != null)
                    ImageConvertHelper.BeginLoadSource(value, (src) => Input1Source = src);
                else
                    Input1Source = null;
            }
        }
        public System.Drawing.Bitmap Output
        {
            get => output; set
            {
                if (output != null)
                    output.Dispose();

                output = value;

                if (value != null)
                    ImageConvertHelper.BeginLoadSource(value, (src) => OutputSource = src);
                else
                    OutputSource = null;
            }
        }
        public BitmapImage Input1Source
        {
            get => input1Source; private set
            {
                input1Source = value;
                OnPropertyChanged("Input1Source");
                OnPropertyChanged("Input1TopNotyText");
                OnPropertyChanged("Input1BottomNoty");
                OnPropertyChanged("Input1BottomNotyText");
                OnPropertyChanged("OutputTopNotyText");
            }
        }
        public BitmapImage OutputSource
        {
            get => outputSource; private set
            {
                outputSource = value;
                OnPropertyChanged("OutputSource");
                OnPropertyChanged("OutputTopNotyText");
                OnPropertyChanged("OutputBottomNoty");
                OnPropertyChanged("OutputBottomNotyText");
            }
        }
        public TankType TankType
        {
            get
            {
                if (AppearOnWhite)
                    return TankType.AppearOnWhite;
                else
                    return TankType.AppearOnBlack;
            }
        }
        public Brush OutputPanelBackground
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(255, (byte)lightness, (byte)lightness, (byte)lightness));
            }
        }

        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
