using SMS_Services.Model;

namespace SMS_Services.Entity
{
    public class SMS_Template : IAuditableEntity
    {
        public long customer_id { get; set; }
        public string message { get; set; }
    }
}
