using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("category_district")]
    public class Category_District : IAuditableEntity
    {
        [MaxLength(200)]
        public string name { get; set; } = string.Empty;
        [MaxLength(50)]
        public string code { get; set; } = string.Empty;
        [MaxLength(1500)]
        public string note { get; set; } = string.Empty;
        public long province_id { get; set; }
        [StringLength(20)]
        public string language_code { set; get; } = string.Empty;
        public int order { set; get; }
        public byte status_id { set; get; }
        public bool is_deleted { get; set; }
    }
}
