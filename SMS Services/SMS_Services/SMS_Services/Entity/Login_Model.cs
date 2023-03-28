using Microsoft.AspNetCore.Mvc;

namespace SMS_Services.Model
{
    public class Login_Model
    {
    }
    public class UserTokenModel
    {
        public long id { get; set; }
        public string user_name { get; set; }
        public string token { get; set; }
        public string name { get; set; }
    } 
    public class CustomerTokenModel
    {
        public long id { get; set; }
        public string user_name { get; set; }
        public string token { get; set; }
        public string name { get; set; }
        public DateTime license_exp { get; set; }
        public string license_key { get; set; } = string.Empty;
    }
 
    public class LoginModel
    {
        public string user_name { set; get; }
        public string password { set; get; }
    }
}
