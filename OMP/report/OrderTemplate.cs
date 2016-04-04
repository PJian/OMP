using System;
using EntityManagementService.entity;

namespace OMP.report
{
    public partial class OrderTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public OrderTemplate(PageOptions options)
        {
            InitializeComponent();
            TopMargin.HeightF += options.TopMargin;
        }
    }
}
