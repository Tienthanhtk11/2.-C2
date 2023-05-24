using SMS_Services.Entity;
using SMS_Services.Model;

namespace SMS_Services.Repository
{
    public interface ICustomerRepository
    {
        #region Customer
        Task<Customer> CustomerCreate(Customer model);
        Task<Customer> CustomerModify(Customer model);
        Task<List<Customer>> CustomerList(string? user_name);
        Task<SMS_Request_Customer> GetSMSRequest(long customer_id);
        Task<List<SMS_Template>> ListSMSTemplate(long customer_id);
        Task<List<SMS_Request_Customer>> ListSMSRequest(long customer_id);
        Task<bool> CreateSMSRequest(List<SMS_Request_Customer> model);
        void CustomerPing(long customer_id);
        int Customer_Authenticate(LoginModel login);
        Task<Order> OrderCreate(Order model);
        Task<Order?> OrderModify(Order model);
        Task<List<Order>> OrderList();
        Task<Order> OrderDetail(long id);
        Task<Customer> Customer_Check(string user_name);
        Task<bool> Customer_Check_Active(string user_name, long user_id);
        Task<Config_Port> Config_Port_Create(Config_Port model);
        Task<Config_Port> Config_Port_Modify(Config_Port model);
        Task<List<Config_Port>> Config_Port_List(long customer_id);
        Task<List<Customer_Config_Phone_Number>> Customer_Config_Phone_Number_Create(List<Customer_Config_Phone_Number> model);
        Task<List<Customer_Config_Phone_Number>> Get_Customer_Config_Phone_Number(long customer_id);
        Task<List<Phone>> Get_List_Phone_Number();
        #endregion
    }
}
