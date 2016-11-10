using DevExpress.XtraReports.UI;
using EntityManagementService.entity;
using OMP.reg;
using OMP.report;
using OMP.util;
using ResourceManagementService.entityResource;
using SerialNum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace OMP
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Order> allOrders;
        private List<Good> currentGoods;
        private Order currentOrder;
        private List<string> allPrinters;
        private String am = "";


        private PageOptions pageOptions;
        public MainWindow()
        {
            InitializeComponent();
            if (ORM == null)
            {
                ORM = createSoft();
            }
            RegisterTableMsg.registSoft = ORM;

            CodeUtil.iniSoft();
        }
        private Soft ORM { get; set; }
        private DispatcherTimer regTimer = null;
        private WinRegist regWin { get; set; }
        private void checkRegist()
        {
            CodeUtil.checkRegisterSuc(showRegWin, expired, inTrial, regErr);
        }
        private Soft createSoft()
        {
            ORM = new Soft() { SoftName = "OoderManagePrintPJV1", SoftVersion = "Beta1.0", TryDays = 30, KeySalt = 10 };
            return ORM;
        }

        /// <summary>
        /// 试用期满
        /// </summary>
        private void expired()
        {
            var regNumWin = new WinForReg() { RegSoft = ORM };
            regNumWin.ShowDialog();
        }
        /// <summary>
        /// 还在试用期
        /// </summary>
        private void inTrial(int r)
        {
            regTimer = new DispatcherTimer() { Interval = TimeSpan.FromMinutes(5), IsEnabled = true };
            //  regTimer.Interval = TimeSpan.FromSeconds(5);
            regTimer.Tick += timer_Tick;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            var trialDays = CodeUtil.getTrailDays();
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
            regWin = new WinRegist();
            if (ORM == null)
            {
                ORM = createSoft();
            }
            regWin.soft = ORM;
            if (!ClosedUtil.regWinIsOpen)
            {
                ClosedUtil.regWinIsOpen = true;
                regWin.ShowDialog();
            }
        }
        private void regErr()
        {
            if (MessageBox.Show("注册信息有误!") == MessageBoxResult.OK)
            {
                Process.GetCurrentProcess().Kill();
            }
        }
        /// <summary>
        /// 程序初始化
        /// </summary>
        private void init()
        {
            allOrders = null;
            currentGoods = null;
            currentOrder = null;
            allPrinters = PrintResourceUtil.getAllPrinter();
            combobox_print.ItemsSource = allPrinters;
            combobox_print.SelectedItem = PrintResourceUtil.getDefaultPrinter();
            pageOptions = new PageOptions()
            {
                PageNum = 1,
                TopMargin = 1,
                Printer = PrintResourceUtil.getDefaultPrinter(),
                PrintType = PrintType.ORDER
            };
            grid_print.DataContext = pageOptions;
        }

        private void initTab1()
        {
            dataGrid_order.ItemsSource = null;
            dataGrid_goods.ItemsSource = null;
            grid_orderDetailInfo.DataContext = null;
        }
        private void initTab2()
        {
            dataGrid_goodsStateTab.ItemsSource = null;
            dataGrid_orderStateTab.ItemsSource = null;
            grid_orderStateChange.DataContext = null;
        }

        private void initTab3()
        {
            dataGrid_goodsChangeTab.ItemsSource = null;
            dataGrid_orderUpdateTab.ItemsSource = null;
            grid_orderUpdateChange.DataContext = null;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            checkRegist();
            init();
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void btn_getPricedOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderUtil.getPayOrder(showOrdersInTabPrint);
        }
        /// <summary>
        /// 在订单打印tab中显示订单信息
        /// </summary>
        /// <param name="orders"></param>
        private void showOrdersInTabPrint(List<Order> orders)
        {
            allOrders = orders;
            dataGrid_order.ItemsSource = allOrders;
        }
        /// <summary>
        /// 在订单状态tab中显示订单信息
        /// </summary>
        /// <param name="orders"></param>
        private void showOrdersInTabStatus(List<Order> orders)
        {
            allOrders = orders;
            dataGrid_orderStateTab.ItemsSource = allOrders;
        }

        private void dataGrid_order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_order.SelectedItem != null)
            {
                currentOrder = dataGrid_order.SelectedItem as Order;
                grid_orderDetailInfo.DataContext = currentOrder;
                currentGoods = currentOrder.goods;
                dataGrid_goods.ItemsSource = currentGoods;
            }
        }

        private void btn_getPricedOrderStateTab_Click(object sender, RoutedEventArgs e)
        {
            OrderUtil.getPayOrder(showOrdersInTabStatus);
        }

        private void dataGrid_orderStateTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_orderStateTab.SelectedItem != null)
            {
                currentOrder = dataGrid_orderStateTab.SelectedItem as Order;
                dataGrid_orderStateTab.DataContext = currentOrder;
                currentGoods = currentOrder.goods;
                dataGrid_goodsStateTab.ItemsSource = currentGoods;
            }
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            initTab1();
        }
        private void TabItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            initTab2();
        }

        private void TabItem_Loaded_2(object sender, RoutedEventArgs e)
        {
            initTab3();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var allSelectedOrder = getAllSelectedOrder();
            if (allSelectedOrder == null || allSelectedOrder.Count <= 0)
            {
                allSelectedOrder = new List<Order>();
                if (dataGrid_orderStateTab.SelectedItem != null)
                {
                    allSelectedOrder.Add(dataGrid_orderStateTab.SelectedItem as Order);
                }
            }

            foreach (Order item in allSelectedOrder)
            {
                OrderUtil.changeOrderStatus(item, () =>
                    {
                        label_msg.Content = "状态修改成功";

                        //OrderUtil.getPayOrder(showOrdersInTabStatus);
                        if (this.am.Equals("am"))
                        {
                            OrderUtil.getAMOrder(showOrdersInTabStatus);
                        }
                        else {
                            OrderUtil.getPMOrder(showOrdersInTabStatus);
                        }
                    });
            }
        }

        private void btn_sendText_Click(object sender, RoutedEventArgs e)
        {
            var allSelectedOrder = getAllSelectedOrder();
            if (allSelectedOrder == null || allSelectedOrder.Count <= 0)
            {
                allSelectedOrder = new List<Order>();
                if (dataGrid_orderStateTab.SelectedItem != null)
                {
                    allSelectedOrder.Add(dataGrid_orderStateTab.SelectedItem as Order);
                }
            }
            foreach (Order item in allSelectedOrder)
            {
                OrderUtil.sendOrderMsgText(item, () =>
                    {
                        label_msg.Content = "短信发送成功";
                        OrderUtil.getTextFailedOrder(showOrdersInTabStatus);
                    });
            }
        }

        private void btn_getSendedOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderUtil.getSendOrder(orders =>
                {
                    allOrders = orders;
                    dataGrid_orderUpdateTab.ItemsSource = allOrders;
                });
        }

        private void dataGrid_orderUpdateTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_orderUpdateTab.SelectedItem != null)
            {
                currentOrder = dataGrid_orderUpdateTab.SelectedItem as Order;
                currentGoods = currentOrder.goods;
                grid_orderUpdateChange.DataContext = currentOrder;
                dataGrid_goodsChangeTab.ItemsSource = currentGoods;
            }
        }

        private void btn_updateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_goodsChangeTab.ItemsSource != null)
            {
                OrderUtil.updateOrder(currentOrder, () =>
                    {
                        MessageBox.Show("修改成功");
                        OrderUtil.getSendOrder(orders =>
                            {
                                allOrders = orders;
                                dataGrid_orderUpdateTab.ItemsSource = allOrders;
                            });
                    });
            }
        }
        /// <summary>
        /// 将原始的order信息转换为打印的order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private List<Order> getLabelOrder(Order order)
        {
            List<Order> orders = new List<Order>();
            orders.Add(new Order()
            {
                uid = order.uid,
                ocode = order.ocode,
                name = order.name,
                goods = order.goods,
                phone = order.phone,
                addr = order.addr,
                delivery_way = order.delivery_way.Equals("pickup") ? "自提" : string.Empty,
                shop = order.shop,
                time = order.time,
                freight = order.freight,
                totalpreprice = order.totalpreprice,
                TotalPrice = double.Parse(order.totalpreprice) + order.freight
            });

            return orders;
        }

        private void combobox_print_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pageOptions != null)
            {
                pageOptions.Printer = combobox_print.SelectedItem as string;
            }
        }
        /// <summary>
        /// 取得选中的订单
        /// </summary>
        /// <returns></returns>
        private List<Order> getAllSelectedOrder()
        {
            var allSelectOrder = new List<Order>();
            if (allOrders != null || allOrders.Count > 0)
            {
                foreach (Order item in allOrders)
                {
                    if (item.IsSelect)
                    {
                        allSelectOrder.Add(item);
                    }
                }
            }
            return allSelectOrder;
        }

        private void btn_printLabel_Click(object sender, RoutedEventArgs e)
        {
            pageOptions.PrintType = PrintType.LABEL;
            orderPrint();
        }

        private void btn_printOrderPreview_Click(object sender, RoutedEventArgs e)
        {
            pageOptions.PrintType = PrintType.ORDER;
            orderPreview();
        }
        /// <summary>
        /// 创建报表
        /// </summary>
        /// <returns></returns>
        private List<XtraReport> createReportTemplate()
        {
            var xtraReports = new List<XtraReport>();
            if (currentOrder == null)
            {
                MessageBox.Show("请选择订单");
                return null;
            }
            List<Order> selectedOrder = getAllSelectedOrder();
            if (selectedOrder == null)
            {
                selectedOrder = new List<Order>();
            }
            if (selectedOrder.Count <= 0)
            {
                selectedOrder.Add(this.currentOrder);
            }

            foreach (Order order in selectedOrder)
            {
                if (pageOptions.PrintType == PrintType.ORDER)
                {
                    OrderTemplate report = new OrderTemplate(pageOptions);
                    List<Order> orders = new List<Order>();
                    orders.Add(order);
                    report.DataSource = orders;
                    xtraReports.Add(report);
                }
                else
                {
                    xtraReports.Add(new OrderLabelTemplate(pageOptions) { DataSource = getLabelOrder(order) });
                }
            }

            return xtraReports;
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        private void orderPreview()
        {
            var xtraReports = createReportTemplate();
            if (xtraReports == null || xtraReports.Count <= 0)
            {
                return;
            }
            foreach (XtraReport item in xtraReports)
            {
                var pt = new ReportPrintTool(item);
                pt.ShowPreviewDialog();
            }
        }
        /// <summary>
        /// 打印报表，
        /// pageOptions 中的pageNum 指定打印次数
        /// </summary>
        private void orderPrint()
        {
            if (pageOptions.Printer == null || pageOptions.Printer.Equals(string.Empty))
            {
                MessageBox.Show("请选择打印机");
                return;
            }
            var xtraReports = createReportTemplate();
            if (xtraReports == null || xtraReports.Count <= 0)
            {
                return;
            }
            foreach (XtraReport item in xtraReports)
            {
                ReportPrintTool pt = new ReportPrintTool(item);
                pt.PrinterSettings.Copies = pageOptions.PageNum;
                pt.Print(pageOptions.Printer);
                
            }
        }

        private void btn_printLabelPreview_Click(object sender, RoutedEventArgs e)
        {
            pageOptions.PrintType = PrintType.LABEL;
            orderPreview();
        }

        private void btn_printOrder_Click(object sender, RoutedEventArgs e)
        {
            pageOptions.PrintType = PrintType.ORDER;
            orderPrint();
        }

        private void checkbox_All_Checked(object sender, RoutedEventArgs e)
        {
            if (allOrders != null)
            {
                foreach (Order item in allOrders)
                {
                    item.IsSelect = true;
                }
            }
        }

        private void checkbox_All_Unchecked(object sender, RoutedEventArgs e)
        {
            if (allOrders != null)
            {
                foreach (Order item in allOrders)
                {
                    item.IsSelect = false;
                }
            }
        }

        private void tabCtrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                initTab1();
                initTab2();
                initTab3();
            }
        }

        private void checkbox_AllStateTab_Checked(object sender, RoutedEventArgs e)
        {
            if (allOrders != null)
            {
                foreach (Order item in allOrders)
                {
                    item.IsSelect = true;
                }
            }
        }

        private void checkbox_AllStateTab_Unchecked(object sender, RoutedEventArgs e)
        {
            if (allOrders != null)
            {
                foreach (Order item in allOrders)
                {
                    item.IsSelect = false;
                }
            }
        }

        private void btn_getTextFiledOrderStateTab_Click(object sender, RoutedEventArgs e)
        {
            OrderUtil.getTextFailedOrder(showOrdersInTabStatus);
        }

        private void btn_getAMOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderUtil.getAMOrder(showOrdersInTabPrint);
        }

        private void btn_getPMOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderUtil.getPMOrder(showOrdersInTabPrint);
        }

        private void btn_getCanceledOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderUtil.getCanceledOrder(showOrdersInTabPrint);
        }

        private void btn_getPricedAMStateTab_Click(object sender, RoutedEventArgs e)
        {
            am = "am";
            OrderUtil.getAMOrder(showOrdersInTabStatus);
        }

        private void btn_getPricedPMStateTab_Click(object sender, RoutedEventArgs e)
        {
            am = "bm";
            OrderUtil.getPMOrder(showOrdersInTabStatus);
        }
    }
}
