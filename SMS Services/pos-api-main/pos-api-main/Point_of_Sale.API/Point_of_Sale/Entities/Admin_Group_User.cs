﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("admin_group_user")]
    public class Admin_Group_User : IAuditableEntity
    {
        public long user_id { get; set; }
        public long group_id { get; set; }
    }
}
