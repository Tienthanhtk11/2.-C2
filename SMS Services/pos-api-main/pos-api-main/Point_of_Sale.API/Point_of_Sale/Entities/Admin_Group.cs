using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("admin_group")]
    public class Admin_Group : IAuditableEntity
    {
        public string name { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public string note { get; set; } = string.Empty;

    }
}