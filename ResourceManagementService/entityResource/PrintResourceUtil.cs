
using EntityManagementService.entity;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;

namespace ResourceManagementService.entityResource
{
    /// <summary>
    /// 
    /// </summary>
    public class PrintResourceUtil
    {
        /// <summary>
        /// 取得全部的打印机
        /// </summary>
        /// <returns></returns>
        public static List<String> getAllPrinter()
        {
            List<String> results = new List<string>();
            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                results.Add(sPrint);
            }
            return results;
        }
        /// <summary>
        /// 取得默认打印机
        /// </summary>
        /// <returns></returns>
        public static string getDefaultPrinter()
        {
            PrintDocument print = new PrintDocument();
            return print.PrinterSettings.PrinterName;//默认打印机名
        }
        /// <summary>
        /// 进行页面的打印
        /// </summary>
        /// <param name="order"></param>
        /// <param name="pageOptions"></param>
        public static bool printOrder(Order order, PageOptions pageOptions)
        {
            //PrintQueue printQueue = isPrinterExists(pageOptions);
          
            if (pageOptions.PrintType == PrintType.ORDER)
            {
               // PrintVisual(printQueue, getPrintOrderLayout(order));
            }
            else
            {
              //  PrintVisual(printQueue, getPrintLabelLayout(order));
            }

            return true;
        }

       

    }
}
