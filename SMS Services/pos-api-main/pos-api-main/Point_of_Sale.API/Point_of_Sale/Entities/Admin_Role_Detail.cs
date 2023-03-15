using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("admin_role_detail")]
    public class Admin_Role_Detail : IAuditableEntity
    {
        public long role_id { get; set; }
        public string code { get; set; }
        public string name { set; get; }
    }
}
