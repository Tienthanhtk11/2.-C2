using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.Inventory
{
    public class Warehouse_Inventory_Model : IAuditableEntity
    {
        public string code { get; set; }
        public long warehouse_id { get; set; }
        public string warehouse_name { get; set; }
        public string content { get; set; }
        public DateTime inventory_date { get; set; }
        public byte status_id { get; set; } // 0 la chua duyet, 1 la da duyet
        public byte type { get; set; }
        public string? user_name { get; set; }
        public List<Warehouse_Inventory_Product_Model>? products { get; set; }
    }
}
