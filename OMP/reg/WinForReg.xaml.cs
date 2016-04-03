using SerialNum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace OMP.reg
{
    /// <summary>
    /// WinForReg.xaml 的交互逻辑
    /// </summary>
    public partial class WinForReg : Window
    {
        public Soft RegSoft { get; set; }
        public WinForReg()
        {
            InitializeComponent();
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            CMPID.Text = HardWareInfo.getMachineCode();
        }

        /// <summary>
        /// 进行注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            var code = this.serialNumTxt.Text;
            if (string.Empty.Equals(code))
            {
                MessageBox.Show("请填写验证码");
            }
            else
            {
                RegisterTableMsg.registSoft = RegSoft;
                CodeUtil.regist(code, success, failed);
            }
        }

        public void success()
        {
            MessageBox.Show("注册成功，请重新启动程序");
        }
        public void failed()
        {
            MessageBox.Show("注册码错误");
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
