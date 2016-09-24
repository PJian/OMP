using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityManagementService.entity
{

    /// <summary>
    /// 订单
    /// {
    ///"ocode":"1356079599100502",
    ///"uid":"25",
    ///"name":"李四",
    ///"phone":"13560795991",
    ///"addr":"上海市宝山区锦秋花园11栋12号",
    ///"shop":"东门罗森便利店",
    ///"totalpreprice":"31.7",
    ///"freight":"0",
    ///"delivery_way":"pickup",
    ///"time":"2016-03-22 19:51:00",
    ///"goods":[
    ///{
    ///"gcode":"B1002",
    ///"gname":"膳博士肋排(无杂骨)",
    ///"gunitprice":"26",
    ///"gunit":"斤",
    ///"gpreweight":"1",
    ///"gpreprice":"26"
    ///},
    ///]
    ///}
    /// </summary>
    public class Order : INotifyPropertyChanged
    {
        public string ocode { get; set; }

        public string uid { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string addr { get; set; }
        public string shop { get; set; }
        public string totalpreprice { get; set; }
        public double freight { get; set; }
        public string delivery_way { get; set; }
        public string deliverydate { get; set; }
        public string deliverytime { get; set; }
        public string time { get; set; }

        public List<Good> goods { get; set; }

        public double TotalPrice { get; set; }

        private bool _isSelect;

        public bool IsSelect
        {
            get { return _isSelect; }
            set
            {
                _isSelect = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSelect"));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
