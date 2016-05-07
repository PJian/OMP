using EntityManagementService.entity;
using System;

namespace OMP.report
{
    public partial class OrderLabelTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public OrderLabelTemplate(PageOptions options)
        {
            InitializeComponent();
            TopMargin.HeightF += options.TopMargin;
        }
    }
}
