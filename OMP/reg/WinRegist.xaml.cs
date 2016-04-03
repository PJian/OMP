using OMP.util;
using SerialNum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OMP.reg
{
    /// <summary>
    /// WinRegist.xaml 的交互逻辑
    /// </summary>
    public partial class WinRegist : Window
    {
        public Soft soft { get; set; }
        public WinRegist()
        {
            InitializeComponent();
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            reg.RegSoft = soft;
        }

        private void reg_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (reg.Visibility == System.Windows.Visibility.Collapsed)
            {
                Close();
            }
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            ClosedUtil.regWinIsOpen = false;
        }
    }
}
