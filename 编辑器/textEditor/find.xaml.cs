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
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// find.xaml 的交互逻辑
    /// </summary>
    public partial class find : Window
    {
        MainWindow mainWindow;     //主窗口相关


        public find(MainWindow window)  //window变量是主窗体的name
        {
            InitializeComponent();
            mainWindow = window;
        }

        //find
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(textBox1.Text.Length!=0)
            {
                mainWindow.find(textBox1.Text);
            }
        }
        //replace
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(textBox2.Text.Length !=0)
            {
                mainWindow.replace(textBox2.Text);
            }
        }
    }
}
