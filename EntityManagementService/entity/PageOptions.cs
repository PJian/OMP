using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityManagementService.entity
{
    /// <summary>
    /// 打印设置
    /// </summary>
    public class PageOptions
    {
        public int TopMargin { get; set; }
        public short PageNum { get; set; }
        public PrintType PrintType { get; set; }
        public string Printer { get; set; }
    }

    public enum PrintType { 
        LABEL,//打印标签
        ORDER//打印订单
    }
}
