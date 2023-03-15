using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("warehouse_receipt")]
    public class Warehouse_Receipt : IAuditableEntity
    {
        public long partner_id { get; set; }
        public string? code { get; set; }
        public long request_id { get; set; }
        public long warehouse_id { get; set; }
        public string? note { get; set; }
        public string? content { get; set; }
        public DateTime transfer_date { get; set; }
        public string? delivery_address { get; set; }
        public byte status_id { get; set; }  // 1 la da duyet, 2 la da tu choi, 0 la moi tao
        public byte type { get; set; }// 0 nhap tu ??, 1 nhap kho tu phieu mua hang, 2 nhap kho tu xuat kho, 4 nhập kiểm kho
    }
}
