using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Point_of_Sale.Model.Export
{
    public class Warehouse_ExportViewModel
    {
        public long id { get; set; }
        public long partner_id { get; set; }
        public string partner_name { set; get; }
        public string? code { get; set; }
        public long warehouse_id { get; set; }
        public string warehouse_name { set; get; }
        public long? warehouse_destination_id { get; set; } //kho dich
        public string warehouse_destination_name { set; get; } 

        public string? note { get; set; }
        public string? content { get; set; }
        public DateTime transfer_date { get; set; }
        public string? delivery_address { get; set; }
        public byte status_id { get; set; }
        public byte type { get; set; }
        public long export_id { get; set; }
        public DateTime export_date { get; set; }
        public string? source_address { get; set; }
        public double total_amount { set; get; }// tổng tiền xuất
        
    }
}
