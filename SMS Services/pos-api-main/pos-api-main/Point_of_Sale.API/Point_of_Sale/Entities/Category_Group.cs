using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("category_group")]

    public class Category_Group : IAuditableEntity // bảng nhóm hàng
    {
        [StringLength(20)]
        public string code { get; set; } = string.Empty;
        [StringLength(250)]
        public string name { set; get; } = string.Empty;
        [StringLength(1500)]
        public string note { set; get; } = string.Empty;
        public long stalls_id { get; set; }        // id danh mục quầy hàng 
        public long category_id { get; set; }        // id danh mục quầy hàng 
        public bool is_active { get; set; } = true;
        public string search_name { get; set; } = string.Empty;

    }
}
