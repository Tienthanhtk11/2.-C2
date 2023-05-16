using SMS_Services.Model;

namespace SMS_Services.Entity
{
    public class Customer_Config_Phone_Number: IAuditableEntity
    {
        public long customer_id { get; set; }
        public string phone_number { get; set; }
        public long phone_id { get; set; }
    }
}
