using Point_of_Sale.Entities;

namespace Point_of_Sale.Model
{
    public class Warehouse_Export_Model : IAuditableEntity
    {
        public long export_id { get; set; }
        public long partner_id { get; set; }
        public string? code { get; set; }
        public long warehouse_id { get; set; }
        public string? note { get; set; }
        public string? content { get; set; }
        public DateTime export_date { get; set; }
        public string? source_address { get; set; }
        public byte status_id { get; set; }
        public byte type { get; set; }// 0 la xuat ban sieu thi, 1 la xuat tra nha cung cap, 2 la xuat kho huy ,3 xuat kho smartgap,4 xuất kho ảo
        public long? customer_id { get; set; }
        public long? warehouse_destination_id { get; set; } //kho dich
        public List<Warehouse_Export_Product>? Products { set; get; }  
    }
}
