using System.ComponentModel.DataAnnotations.Schema;
namespace Point_of_Sale.Entities
{
    [Table("warehouse_inventory")]
    public class Warehouse_Inventory : IAuditableEntity
    {
        public string code { get; set; }
        public long warehouse_id { get; set; } 
        public string content { get; set; }
        public DateTime inventory_date { get; set; }
        public byte status_id { get; set; } // 0 la chua duyet, 1 la da duyet, 2 la tu choi
        public byte type { get; set; } 

    }
}
