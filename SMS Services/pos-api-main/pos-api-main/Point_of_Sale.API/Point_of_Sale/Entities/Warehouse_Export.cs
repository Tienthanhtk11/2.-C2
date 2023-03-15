using System.ComponentModel.DataAnnotations.Schema;
namespace Point_of_Sale.Entities
{
    [Table("warehouse_export")]
    public class Warehouse_Export : IAuditableEntity
    {
        public long partner_id { get; set; }
        public string? code { get; set; }
        public long warehouse_id { get; set; }
        public string? note { get; set; }
        public string? content { get; set; }
        public DateTime export_date { get; set; }
        public string? source_address { get; set; } 
        public byte status_id { get; set; } // 0 la chua duyet, 1 la cho xuat kho, 2 la xuat kho
        public byte type { get; set; } // 0 la xuat ban sieu thi, 1 la xuat tra nha cung cap, 2 la xuat kho huy ,3 xuat kho smartgap, 4 xuất kho ảo
        public long? customer_id { get; set; }
        public long? order_id { get; set; }
        public long? warehouse_destination_id { get; set; } //kho dich

    }
}
