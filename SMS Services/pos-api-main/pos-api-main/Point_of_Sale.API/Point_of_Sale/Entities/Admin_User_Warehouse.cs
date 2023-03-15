using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("admin_user_warehouse")]
    public class Admin_User_Warehouse:IAuditableEntity
    {
        public long user_id { get; set; }
        public long warehouse_id { get; set; }       
    }
    
}
