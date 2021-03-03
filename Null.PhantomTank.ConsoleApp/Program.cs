using Null.ArgsParser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Null.PhantomTank.ConsoleApp
{
    class Program
    {
        class ExecuteArgs
        {
            public TankType Type;
            public float Ratio = 1;
            public ResizeMode Resize = ResizeMode.NoResize;
            public Image Source1, Source2;
            public bool Combine, Convert;
            public string OutputPath;
        }
        class StartupArgs
        {
            public string Type;
            public string Ratio;
            public string Resize;
            public string Output;

            public string Source1;
            public string Source2;

            public bool Combine;
            public bool Convert;
            public bool Help;

            public string[] ExtraContent;
        }
        static void ErrorExit(string title, string description, int exitCode)
        {
            Console.WriteLine(
                $"Tips: {title}\n" +
                $"      {description}");
            Environment.Exit(exitCode);
        }
        static ExecuteArgs InitializeApp(string[] args)
        {
            Arguments arguments = new Arguments(
                new CommandLine("Combine",
                    new FieldArgument("Ratio"),
                    new FieldArgument("Resize"),
                    new FieldArgument("Output"),
                    new StringArgument("Source1"),
                    new StringArgument("Source2")),
                new CommandLine("Convert",
                    new FieldArgument("Type"),
                    new FieldArgument("Output"),
                    new StringArgument("Source1")),
                new CommandLine("Help"))
            { IgnoreCase = true };

            int index = 0;
            if (!arguments.TryParse(ref args, ref index))
                ErrorExit("参数分析失败", "请确认参数是否正确", -1);

            StartupArgs startArgs = arguments.ToObject<StartupArgs>();
            ExecuteArgs result = new ExecuteArgs();

            result.OutputPath = startArgs.Output != null ? startArgs.Output : Path.GetRandomFileName() + ".png";

            if (startArgs.Combine)
            {
                result.Combine = true;

                if (startArgs.Ratio != null)
                {
                    if (float.TryParse(startArgs.Ratio, out float ratio))
                    {
                        result.Ratio = ratio;
                    }
                    else
                    {
                        ErrorExit("参数无效.", "'Ratio' 需被指定为一个浮点数.", -1);
                    }
                }
                else
                {
                    result.Ratio = 1;
                }

                if (startArgs.Resize != null)
                {
                    if (Enum.TryParse<ResizeMode>(startArgs.Resize, out ResizeMode resize))
                    {
                        result.Resize = resize;
                    }
                    else
                    {
                        ErrorExit("参数无效.", "'Ratio' 需指定为 'NoResize', 'Stretch', 'Uniform' 或 'UniformToFill'.", -1);
                    }
                }

                try
                {
                    if(startArgs.Source1 == null && startArgs.Source2 == null)
                        ErrorExit("无法载入图像", "没有指定足够的参数, 需要两个图像", -1);

                    result.Source1 = Image.FromFile(startArgs.Source1);
                    result.Source2 = Image.FromFile(startArgs.Source2);
                }
                catch
                {
                    ErrorExit("无法载入图像", "请检查路径是否填写正确, 以及图像格式是否受支持.", -1);
                }

                return result;
            }
            else if (startArgs.Convert)
            {
                result.Convert = true;
                if (startArgs.Type != null)
                {
                    if (Enum.TryParse<TankType>(startArgs.Type, out TankType type))
                    {
                        result.Type = type;
                    }
                    else
                    {
                        ErrorExit("参数无效.", "'Type' 需指定为 'AppearOnBlack' 或 'AppearOnWhite'.", -1);
                    }
                }
                else
                {
                    result.Type = TankType.AppearOnBlack;
                }

                try
                {
                    if (startArgs.Source1 == null)
                        ErrorExit("无法载入图像", "没有指定足够的参数, 需要一个图像", -1);

                    result.Source1 = Image.FromFile(startArgs.Source1);
                }
                catch (IndexOutOfRangeException)
                {
                    ErrorExit("无法载入图像", "没有指定足够的参数, 需要一个图像", -1);
                }
                catch
                {
                    ErrorExit("无法载入图像", "请检查路径是否填写正确, 以及图像格式是否受支持.", -1);
                }

                return result;
            }
            else if (startArgs.Help || startArgs.ExtraContent.Contains("/?"))
            {
                Console.WriteLine(string.Join("\n",
                    $"Null.PhantomTank : 幻影坦克制作工具",
                    $"",
                    $"    拼合图像:",
                    $"    Null.PhantomTank Combine [Ratio=1] [Resize=NoResize] [Output=RandomFilename] Source1 Source2",
                    $"        Ratio    对比度权重比值, 默认为1.",
                    $"        Resize   原图尺寸调整方式, 默认是NoResize, 可以是以下值:",
                    $"            NoResize      : 不调整原图尺寸, 图片在输出中居中.",
                    $"            Stretch       : 源图将被进行拉伸以适应输出图片的尺寸.",
                    $"            Uniform       : 图片将按比例缩放以适应输出图片的尺寸, 图片大小不会超过输出尺寸.",
                    $"            UniformToFill : 图片将按比例缩放以覆盖整个输出图片, 比例不会变更, 多余的部分会被丢弃.",
                    $"        Output   输出路径, 如果不指定, 则会使用随机的文件名以保存图像.",
                    $"        Source1 : 源文件1, 必须是受支持的图片类型, 输出图片在白色背景下可以显现出这个图像.",
                    $"        Source2 : 源文件2, 必须是受支持的图片类型, 输出图片在黑色背景下可以显现出这个图像.",
                    $"",
                    $"    转换图像:",
                    $"    Null.PhantomTank Convert [Type=AppearOnBlack] [Output=RandomFilename] Source",
                    $"        Type     幻影坦克的类型, 默认是AppearOnBlack, 可以是以下值:",
                    $"            AppearOnBlack : 输出的幻影坦克, 在黑色背景下才可显现出图像",
                    $"            AppearOnWhite : 输出的幻影坦克, 在白色背景下才可以显现出图像",
                    $"        Output   与前面的一致, 为输出路径, 若不指定则使用随机文件名.",
                    $"        Source   源文件, 必须是受支持的图片类型, 是输出幻影坦克要显示的图像",
                    $"",
                    $"    显示帮助:",
                    $"    Null.PhantomTank Help",
                    $"    Null.PhantomTank /?",
                    $"",
                    $"    关于程序:",
                    $"        作者: SlimeNull.  项目仓库: https://github.com/SlimeNull/Null.PhantomTank"));

                Environment.Exit(0);
            }
            else
            {
                ErrorExit("没有指定操作.", "使用 'Null.PhantomTank Help' 查看帮助.", -1);
            }

            return null;
        }
        static void Main(string[] args)
        {
            ExecuteArgs exeArgs = InitializeApp(args);

            if (exeArgs.Combine)
            {
                Bitmap result = PhantomTank.CombineImage(exeArgs.Source1, exeArgs.Source2, exeArgs.Resize, exeArgs.Ratio);

                try
                {
                    result.Save(exeArgs.OutputPath);
                    ErrorExit("操作成功", $"保存路径: \"{exeArgs.OutputPath}\"", 0);
                }
                catch(Exception e)
                {
                    ErrorExit("保存失败", $"异常描述: {e.Message}", -2);
                }
            }
            else if (exeArgs.Convert)
            {
                Bitmap result = PhantomTank.ConvertImage(exeArgs.Source1, exeArgs.Type);

                try
                {
                    result.Save(exeArgs.OutputPath);
                    ErrorExit("操作成功", $"保存路径: \"{exeArgs.OutputPath}\"", 0);
                }
                catch (Exception e)
                {
                    ErrorExit("保存失败", $"异常描述: {e.Message}", -2);
                }
            }
            else
            {
                ErrorExit("操作无效", "此问题不应出现, 原因是参数分析导致异常后, 程序未退出", -2);
            }
        }
    }
}
