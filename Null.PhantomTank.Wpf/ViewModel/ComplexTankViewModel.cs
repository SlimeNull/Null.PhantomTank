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
    class ComplexTankViewModel : INotifyPropertyChanged
    {
        private bool input1TopNoty;
        private bool input2TopNoty;
        private bool outputTopNoty;
        private BitmapImage input1Source;
        private BitmapImage input2Source;
        private BitmapImage outputSource;
        private bool uniformToFill;
        private bool uniform;
        private bool stretch;
        private bool noResize = true;
        private System.Drawing.Image input1;
        private System.Drawing.Image input2;
        private System.Drawing.Bitmap output;
        private int lightness = 255;
        private int ratioBarValue= 10;

        public int RatioBarValue
        {
            get => ratioBarValue; set
            {
                ratioBarValue = value;
                OnPropertyChanged("RatioBarValue");
                OnPropertyChanged("ConstrastRatio");
                OnPropertyChanged("ConstrastRatioString");
            }
        } // 0 ~ 20

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
            get => input2Source == null;
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
        public string Input1TopNotyText
        {
            get
            {
                return Input1Source == null ? "单击或拖动图片到这里以导入" : "右击以重置图片输入源";
            }
        }
        public string Input2TopNotyText
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
                return (Input1Source == null || Input2Source == null) ? "请先导入源图像" : (OutputSource == null ? "单击以进行渲染" : "单击保存或右击重置图像");
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
        public System.Drawing.Image Input2
        {
            get => input2; set
            {
                if (input2 != null)
                    input2.Dispose();

                input2 = value;

                if (value != null)
                    ImageConvertHelper.BeginLoadSource(value, (src) => Input2Source = src);
                else
                    Input2Source = null;
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
        public BitmapImage Input2Source
        {
            get => input2Source; private set
            {
                input2Source = value;
                OnPropertyChanged("Input2Source");
                OnPropertyChanged("Input2TopNotyText");
                OnPropertyChanged("Input2BottomNoty");
                OnPropertyChanged("Input2BottomNotyText");
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
        public float ConstrastRatio
        {
            get
            {
                return (ratioBarValue - 0f) / (20 - ratioBarValue);
            }
        }
        public string ConstrastRatioString
        {
            get
            {
                return $"{ratioBarValue - 0f}/{20 - ratioBarValue}";
            }
        }
        public int Lightness
        {
            get => lightness; set
            {
                lightness = value;
                OnPropertyChanged("Lightness");
                OnPropertyChanged("OutputPanelBackground");
            }
        }
        public bool NoResize
        {
            get => noResize; set
            {
                noResize = value;
                OnPropertyChanged("NoResize");
            }
        }
        public bool Stretch
        {
            get => stretch; set
            {
                stretch = value;
                OnPropertyChanged("Stretch");
            }
        }
        public bool Uniform
        {
            get => uniform; set
            {
                uniform = value;
                OnPropertyChanged("Uniform");
            }
        }
        public bool UniformToFill
        {
            get => uniformToFill; set
            {
                uniformToFill = value;
                OnPropertyChanged("UniformToFill");
            }
        }
        public ResizeMode ResizeMode
        {
            get
            {
                if (noResize)
                    return ResizeMode.NoResize;
                else if (stretch)
                    return ResizeMode.Stretch;
                else if (uniform)
                    return ResizeMode.Uniform;
                else if (uniformToFill)
                    return ResizeMode.UniformToFill;
                else
                    return ResizeMode.NoResize;
            }
        }
        public Brush OutputPanelBackground
        {
            get
            {
                return new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, (byte)lightness, (byte)lightness, (byte)lightness));
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
