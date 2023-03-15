namespace SMS_Services.Model
{
    public class Admin_User : IAuditableEntity
    {
        public string name { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string passcode { get; set; } = "";
        public string email { get; set; }
    }
}
