using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("admin_role")]
    public class Admin_Role : IAuditableEntity
    {
        public string name { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public string note { get; set; } = string.Empty;
        [NotMapped]
        public List<Admin_Role_Detail>? role_Details { get; set; }  
    }
}
