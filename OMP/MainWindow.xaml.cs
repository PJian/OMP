using OMP.reg;
using OMP.util;
using SerialNum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace OMP
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (this.ORM == null)
            {
                this.ORM = createSoft();
            }
            RegisterTableMsg.registSoft = ORM;
            //初始化,如果软件信息在注册表中没有，则写入信息
            CodeUtil.iniSoft();
        }
        private Soft ORM { get; set; }
        private DispatcherTimer regTimer = null;
        private WinRegist regWin { get; set; }
        private void checkRegist()
        {
            //注册检查
            CodeUtil.checkRegisterSuc(showRegWin, expired, inTrial, regErr);
        }
        private Soft createSoft()
        {
            ORM = new Soft();
            ORM.SoftName = "ORM";
            ORM.SoftVersion = "Beta1.0";
            ORM.TryDays = 3;
            ORM.KeySalt = 10;
            return ORM;
        }

        /// <summary>
        /// 试用期满
        /// </summary>
        private void expired()
        {
            WinForReg regNumWin = new WinForReg();
            regNumWin.RegSoft = ORM;
            regNumWin.ShowDialog();

        }
        /// <summary>
        /// 还在试用期
        /// </summary>
        private void inTrial(int r)
        {
            //启动定时器，定期显示注册窗口
            //显示注册窗口
            regTimer = new DispatcherTimer();
            //regTimer.Interval = TimeSpan.FromMinutes(5);
            regTimer.Interval = TimeSpan.FromSeconds(5);
            //  regTimer.Interval = TimeSpan.FromMinutes(10);
            regTimer.Tick += timer_Tick;
            regTimer.IsEnabled = true;
        }
        void timer_Tick(object sender, EventArgs e)
        {
            int trialDays = CodeUtil.getTrailDays();
            if (trialDays < 0)
            {
                expired();
            }
            else
            {
                showRegWin();
            }
        }
        /// <summary>
        /// 显示注册窗口
        /// </summary>
        private void showRegWin()
        {
            this.regWin = new WinRegist();
            if (this.ORM == null)
            {
                this.ORM = createSoft();
            }
            this.regWin.soft = this.ORM;
            if (!ClosedUtil.regWinIsOpen)
            {
                ClosedUtil.regWinIsOpen = true;
                this.regWin.ShowDialog();
            }

        }
        private void regErr()
        {
            if (MessageBox.Show("注册信息有误!") == MessageBoxResult.OK)
            {
                Process.GetCurrentProcess().Kill();//试用期结束，结束程序
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //进行注册检查
            checkRegist();
         
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
           
            System.Environment.Exit(0);
        }

    }
}
