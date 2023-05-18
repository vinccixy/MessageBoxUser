using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace MessageBox {
    /// <summary>
    /// MessageBoxUser.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxUser : Window, INotifyPropertyChanged {
        public event EventHandler<MessageBoxEventArgs> Result;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Context {
            get { return TB_Context.Text; }
            set { TB_Context.Text = value; }
        }
        TimeSpan _yesLeftTime = TimeSpan.FromSeconds(-1);
        public TimeSpan YesLeftTime {
            get { return _yesLeftTime; }
            set { _yesLeftTime = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("YesLeftTime")); }
        }
        TimeSpan _noLeftTime = TimeSpan.FromSeconds(-1);
        public TimeSpan NoLeftTime {
            get { return _noLeftTime; }
            set { _noLeftTime = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NoLeftTime")); }
        }
        bool _isLegal = false;
        DispatcherTimer _timer;

        public static void Show(string context, EventHandler<MessageBoxEventArgs> result) {
            var mb = new MessageBoxUser();
            mb.Context = context;
            mb.Result += result;
            mb.Show();
        }

        public static MessageResult ShowDialog(string context) {

            return ShowDialog(context, null, null);
        }

        public static MessageResult ShowDialog(string context, TimeSpan? yestCountDown, TimeSpan? noCountDown) {
            var mb = new MessageBoxUser();
            mb.Context = context;
            MessageResult r = null;
            if (yestCountDown != null) {
                mb.YesLeftTime = yestCountDown.Value;
            }
            if (noCountDown != null) {
                mb.NoLeftTime = noCountDown.Value;
            }
            mb.Result += (s, e) => {
                r = e.Result;
            };
            mb.ShowDialog();
            return r;
        }
        private void No_Button_Click(object sender, RoutedEventArgs e) {
            _isLegal = true;
            Close();
            Result?.Invoke(this, new MessageBoxEventArgs() { Result = new MessageResult() { IsYes = false, YesLeftTime = this.YesLeftTime, NoLeftTime = this.NoLeftTime } });
        }
        private void Yes_Button_Click(object sender, RoutedEventArgs e) {
            _isLegal = true;
            Close();
            Result?.Invoke(this, new MessageBoxEventArgs() { Result = new MessageResult() { IsYes = true, YesLeftTime = this.YesLeftTime, NoLeftTime = this.NoLeftTime } });
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = !_isLegal;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            if (YesLeftTime.TotalSeconds > 0 || NoLeftTime.TotalSeconds > 0) {
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += (S, E) => {
                    if (YesLeftTime.TotalSeconds > 0) {
                        YesLeftTime = YesLeftTime.Add(-TimeSpan.FromSeconds(1));
                    } else if (YesLeftTime.TotalSeconds == 0) {
                        _timer.Stop();
                        Yes_Button_Click(null, null);
                    }

                    if (NoLeftTime.TotalSeconds > 0) {
                        NoLeftTime = NoLeftTime.Add(-TimeSpan.FromSeconds(1));
                    } else if (NoLeftTime.TotalSeconds == 0) {
                        _timer.Stop();
                        No_Button_Click(null, null);
                    }
                };
                _timer.Start();
            }
        }
        public MessageBoxUser() {
            InitializeComponent();
            DataContext = this;
        }
    }

    public class MessageResult {
        /// <summary>
        /// 结果，Yes为true，No为false
        /// </summary>
        public bool IsYes { get; set; }
        public TimeSpan YesLeftTime { get; set; }

        public TimeSpan NoLeftTime { get; set; }
    }

    public class MessageBoxEventArgs : EventArgs {
        /// <summary>
        /// 结果，Yes为true，No为false
        /// </summary>
        public MessageResult Result { get; set; }
    }
    /*调用方法-Yes倒计时30秒
     var r = MessageBoxUser.ShowDialog("Ensure?",TimeSpan.FromSeconds(30), null);
    if (r.IsYes)
    {
    //选择了Yes
    }
    else
    {
    //选择了No
    }
    */
    /*No倒计时30s
    * var r = MessageBoxUser.ShowDialog("Ensure?",null,TimeSpan.FromSeconds(30));
    if (r.IsYes)
    {
        //选择了Yes
    }
    else
    {
    //选择了No
    }*/
}
