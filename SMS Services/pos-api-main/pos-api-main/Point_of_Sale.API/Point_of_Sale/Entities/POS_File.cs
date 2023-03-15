using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("file")]
    public class POS_File
    {
        [Key]
        public long id { get; set; }
        public long idtable { get; set; } // id ban ghi so huu file
        [StringLength(100)]
        public string tablename { get; set; } = string.Empty; // ten bang theo enum
        [StringLength(100)]
        public string name_guid { set; get; } = string.Empty; // ten file tren server
        [StringLength(250)]
        public string name { set; get; } = string.Empty; // ten file upload tu may
        public string ipserver { set; get; } = string.Empty; // ip server
        public byte type { set; get; } // = 1 là ảnh avata
        public string path { set; get; } = string.Empty; //duong dan tren server
        public string file_type { set; get; } = string.Empty; // loai file .mp4, .png....
        public bool is_delete { set; get; }
    }
}
