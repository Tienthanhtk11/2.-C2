using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("admin_role_group")]
    public class Admin_Role_Group:IAuditableEntity
    {
        public long group_id { get; set; }
        public long role_id { get; set; }
        [NotMapped]
        public List<Admin_Role_Group_Detail>? details { get; set; }  
    }

    [Table("admin_role_group_detail")]
    public class Admin_Role_Group_Detail : IAuditableEntity
    {
        public long role_group_id { get; set; }
        public long role_detail_id { get; set; }
    }
}
