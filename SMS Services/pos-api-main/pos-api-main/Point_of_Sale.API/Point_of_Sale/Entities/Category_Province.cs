using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("category_province")]
    public class Category_Province : IAuditableEntity
    {
        [MaxLength(30)]
        public string zipcode { set; get; } = string.Empty;
        [MaxLength(250)]
        public string city { set; get; } = string.Empty;
        [StringLength(20)]
        public string language_code { set; get; } = string.Empty;
        public int order { set; get; }
        public byte status_id { set; get; }
        public bool is_transport { set; get; }
        public bool is_deleted { get; set; }

    }
}
