using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("warehouse_request")]
    public class Warehouse_Request : IAuditableEntity
    {

        public long partner_id { get; set; }  
        public string? code { get; set; }
        public string? shipper { get; set; }  // nguoi giao
        public long warehouse_id { get; set; } 
        public string? note { get; set; }
        public string? content { get; set; }
        public DateTime transfer_date { get; set; }
        public string? delivery_address { get; set; }
        public byte status_id { get; set; }// 0 là chưa xử lý 1 đã xử lý
        public byte type { get; set; }
    }
}
