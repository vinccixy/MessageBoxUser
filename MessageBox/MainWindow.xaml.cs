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

namespace MessageBox {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            //调用Yes倒计时方法
            var r = MessageBoxUser.ShowDialog("Ensure?", TimeSpan.FromSeconds(30), null);
            if (r.IsYes) {
                //选择了Yes
            } else {
                //选择了No
            }
        }
    }
}
