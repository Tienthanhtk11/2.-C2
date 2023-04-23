using SMS_Services.Model;

namespace SMS_Services.Entity
{
    public class Config_Port : IAuditableEntity
    {
        public string Mobile_Carrier { get; set; } = "";
        public string Port_Name { get; set; } = "";
        public string Phone_Number { get; set; } = "";
        public long Customer_Id { get; set; } = 0;
    }
}
