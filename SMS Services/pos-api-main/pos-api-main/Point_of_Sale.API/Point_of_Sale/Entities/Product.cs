﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("product")]
    public class Product : IAuditableEntity
    {
        public string code { get; set; } = string.Empty;
        public string search_name { get; set; } = string.Empty;
        public string? name { get; set; }
        public string? note { get; set; }
        public string? item_code { get; set; }
        public string? category_code { get; set; }
        public bool is_active { get; set; }
        public long? partner_id { get; set; } = 0;
        public long? category_id { get; set; } = 0;
        public long? category_stalls_id { get; set; } = 0;
        public long? category_group_id { get; set; } = 0;
        public string? barcode { get; set; }
        public string? unit_code { get; set; } // mã đon tính
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public double price { get; set; } = 0;

    }
}
