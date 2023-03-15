namespace SMS_Services.Model
{
    public class Customer : IAuditableEntity
    {
        public string user_name { get; set; }
        public string password { get; set; }
        public string passcode { get; set; } = "";
        public string name { get; set; } = "";
        public string email { get; set; } = "";
        public int cash { get; set; } = 0;
        public bool active { get; set; } = true;
    }
}
