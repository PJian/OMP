using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityManagementService.entity
{
    /// <summary>
    /// 货物类
    /// "gcode":"B1002",
    /// "gname":"膳博士肋排(无杂骨)",
    /// "gunitprice":"26",
    /// "gunit":"斤",
    /// "gpreweight":"1",
    /// "gpreprice":"26"
    /// </summary>
    public class Good
    {
        public string gcode { get; set; }
        public string gname { get; set; }
        public string gunitprice { get; set; }
        public string gunit { get; set; }
        public string gpreweight { get; set; }

        public string gpreprice { get; set; } 
    }
}
