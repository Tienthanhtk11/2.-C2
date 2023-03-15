namespace Point_of_Sale.Model.User
{
    public class UserTokenModel
    {
        public long id { get; set; }
        public string username { get; set; }
        public string token { get; set; }
        public string full_name { get; set; }
        public List<RolesModel> roles { get; set; }
        public WarehouseModel warehouse { get; set; }
        
    }
    public class RolesModel
    {
        public string role { get; set; }
        public List<string>? role_detail { get; set; }
    } 
    public class Roles_Name
    {
        public string role { get; set; }
    }
    public class LoginModel
    {
        public string username { set; get; }
        public string password { set; get; }
    }

    public class ChangePassModel
    {
        public long id { set; get; }
        public string passwordOld { set; get; }
        public string passwordNew { set; get; }
    }
    public class WarehouseModel
    {
        public long id { set; get; }
        public string name { set; get; }
        public string address { set; get; }
    }
}
