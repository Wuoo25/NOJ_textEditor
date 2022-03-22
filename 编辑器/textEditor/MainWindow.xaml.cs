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
using System.Windows.Threading;
using System.IO;
using Microsoft.Win32;
using System.Reflection;

namespace WpfApp3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string s_FileName = "";
        private static string FilePath;

        TextPointer postion = null;

        public MainWindow()
        {
            InitializeComponent();

            /*时间有关*/
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0,0,1);
            dispatcherTimer.Start();
            textblock2.Text = DateTime.Now.ToString();

        }

        /*新建文件功能*/
        private void Nem_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            textbox1.Document.Blocks.Clear();
            s_FileName = "";

        }
        /*从键盘快捷键新建文件功能*/
        private void Nem_MenuItem_KeyDown(Object sender, KeyEventArgs e)
        {
            if((Keyboard.GetKeyStates(Key.LeftCtrl)&KeyStates.Down)>0)    //ctrl是否按下
            {
                if ((Keyboard.GetKeyStates(Key.N) & KeyStates.Down) > 0)
                    Nem_MenuItem_Click(sender,e);
            }
        }

        /*关闭文件*/
        private void End_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /*从键盘快捷键关闭文件功能*/
        private void End_MenuItem_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) > 0)    //ctrl是否按下
            {
                if ((Keyboard.GetKeyStates(Key.X) & KeyStates.Down) > 0)
                    Application.Current.Shutdown() ;
            }
        }

        /*字体选择*/
        private void combobox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FontFamily fontName = new FontFamily(combobox1.SelectedItem.ToString());
            textbox1.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty,fontName);
        }
        private void combobox1_Loaded(object sender, RoutedEventArgs e)
        {
            System.Drawing.FontFamily[] families = System.Drawing.FontFamily.Families;

            foreach (System.Drawing.FontFamily family in families)
                combobox1.Items.Add(family.Name);

        }

        /*显示时间*/
        private void dispatcherTimer_Tick(object sender,EventArgs e)
        {
            textblock2.Text = DateTime.Now.ToString();
        }

        /*颜色选择*/
        private void combobox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString(combobox3.SelectedItem.ToString());
            textbox1.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
        }
        private void combobox3_Loaded(object sender, RoutedEventArgs e)
        {
            Type type = typeof(System.Windows.Media.Brushes);
            System.Reflection.PropertyInfo[] info = type.GetProperties();
            foreach (System.Reflection.PropertyInfo pi in info)
            {
                string colorName = pi.Name;
                combobox3.Items.Add(colorName);
            }
        }

        /*帮助按钮版权窗口*/
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            About d1 = new About();
            d1.ShowDialog();
        }

        /*打开文件*/
        private void Open_MenuItem_Click(object sender, RoutedEventArgs e)
        {

            textbox1.Document.Blocks.Clear();
            s_FileName = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();    //对话框选择要打开的文件
           
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog1.Filter = "Rtf文件(*.Rtf)|*.rtf|所有文件(*.*)|*.*|Txt文件(*.Txt)|*.txt|所有文件(*.*)|*.*";


            if (openFileDialog1.ShowDialog().Value)
            {            

                FilePath = dialog.FileName;
                var name = FilePath.Split('.');

                s_FileName = openFileDialog1.FileName;
                String fileExtension = System.IO.Path.GetExtension(s_FileName).ToUpper();
                if(fileExtension==".RTF")
                {

                    s_FileName = openFileDialog1.FileName;

                    using (FileStream fileStream = File.OpenRead(s_FileName))
                    {
                        TextRange textRange = new TextRange(textbox1.Document.ContentStart,textbox1.Document.ContentEnd);
                        if (textRange.CanLoad(DataFormats.Rtf))
                            textRange.Load(fileStream, DataFormats.Rtf);
                    }
                }
                else if (name[name.Length - 1] == "txt")
                {
                    //OpenFileWS(s_FileName);
                    StreamReader sR = File.OpenText(s_FileName);
                    string nextLine;
                    while ((nextLine = sR.ReadLine()) != null)
                    {
                        Console.WriteLine(nextLine);
                        LoadTXTFile();
                    }
                    sR.Close();
                }


            }

        }

        private void LoadTXTFile()
        {

            if (FilePath != null)
            {
                // 获取文件对象
                FileStream file = File.OpenRead(FilePath);

                // 创建读取流,默认字符编码
                StreamReader reader = new StreamReader(file, Encoding.UTF8);

                String line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    textbox1.AppendText(line + "\r\n");
                }
                // 关闭资源
                reader.Close();
                file.Close();
            }
            else
            {
                Console.WriteLine("文件不为空！！！");
            }

        }


        /*另存为*/
        private void ASave_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "富文本|*.rtf|文本文件|*.txt";
            if (saveFileDialog1.ShowDialog().Value)
            {
                s_FileName = saveFileDialog1.FileName;
                using (FileStream fileStream = File.Create(s_FileName))
                {
                    TextRange textRange = new TextRange(textbox1.Document.ContentStart, textbox1.Document.ContentEnd);
                    if (textRange.CanSave(DataFormats.Rtf))
                        textRange.Save(fileStream, DataFormats.Rtf);     
                    else if (textRange.CanSave(DataFormats.Text))
                    {
                    textRange.Save(fileStream,DataFormats.Text); 
                    }

                }
            }
        }

        /*保存文件*/
        private void Save_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            if (s_FileName.Length!=0)
            {
                using (FileStream fileStream = File.Create(s_FileName))
                {
                    TextRange textRange = new TextRange(textbox1.Document.ContentStart, textbox1.Document.ContentEnd);
                    if (textRange.CanSave(DataFormats.Rtf))
                        textRange.Save(fileStream, DataFormats.Rtf);
                    else if (textRange.CanSave(DataFormats.Text))
                    {
                        textRange.Save(fileStream, DataFormats.Text);
                    }
                }
            }
            else
            {
                ASave_MenuItem_Click(sender, e);
            }
        }

        public void find(string findString)
        {
            

        }

        public void replace(string replaceString)
        {

        }


    }
}
