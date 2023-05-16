using Microsoft.EntityFrameworkCore;
using SMS_Services.Common;
using SMS_Services.Entity;
using SMS_Services.Model;

namespace SMS_Services.Repository
{
    public class CustomerRepository: ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context
            ) => _context = context;
        #region Customer
        public async Task<Customer> CustomerCreate(Customer model)
        {
            var customer_db = _context.Customer.AsNoTracking().FirstOrDefault(x => !x.is_delete && x.user_name == model.user_name && x.email == model.email);
            if (customer_db != null)
            {
                return null;
            }
            model.license_exp = DateTime.Now.AddMonths(3);
            model.passcode = Encryptor.RandomPassword();
            model.password = Encryptor.MD5Hash(model.password + model.passcode);
            _context.Customer.Add(model);
            _context.SaveChanges();
            return model;
        }
        public async Task<Customer> CustomerModify(Customer model)
        {
            var Customer = _context.Customer.FirstOrDefault(r => r.id == model.id);
            Customer.email = model.email;
            Customer.name = model.name;
            Customer.cash = model.cash;
            Customer.active = model.active;
            Customer.dateUpdated = DateTime.Now;
            _context.Customer.Update(Customer);
            _context.SaveChanges();
            return model;
        }
        public async Task<List<Customer>> CustomerList(string? user_name)
        {
            IEnumerable<Customer> listItem = _context.Customer;
            if (user_name != null && user_name != "")
            {
                listItem = listItem.Where(r => r.user_name.Contains(user_name) && !r.is_delete);
            }
            List<Customer> lists = listItem.OrderByDescending(r => r.id).ToList();
            foreach (var item in lists)
            {
                if (item.last_active.AddMinutes(5) > DateTime.Now)
                {
                    item.status = "Online";
                }
                else
                    item.status = "Offline";
            }
            return lists;
        }
        public async Task<SMS_Request_Customer> GetSMSRequest(long customer_id)
        {
            var sms = _context.SMS_Request_Customer.FirstOrDefault(x => x.customer_id == customer_id && x.status == 0);
            sms.status = 1;
            _context.SMS_Request_Customer.Update(sms);
            _context.SaveChanges();
            return sms;
        }
        public async Task<List<SMS_Template>> ListSMSTemplate(long customer_id)
        {
            var list_temp = _context.SMS_Template.Where(x => x.customer_id == customer_id).OrderByDescending(x => x.id).ToList();
            return list_temp;
        }
        public async Task<List<SMS_Request_Customer>> ListSMSRequest(long customer_id)
        {
            var list_temp = _context.SMS_Request_Customer.Where(x => x.customer_id == customer_id && x.status ==0).OrderByDescending(x => x.id).ToList();
            return list_temp;
        }
        public async Task<bool> CreateSMSRequest(List<SMS_Request_Customer> model)
        {
            _context.SMS_Request_Customer.AddRange(model);
            _context.SaveChanges();
            return true;
        }
        public async void CustomerPing(long customer_id)
        {
            var customer = _context.Customer.FirstOrDefault(x => !x.is_delete && x.id == customer_id);
            if (customer != null)
            {
                customer.last_active = DateTime.Now;
                _context.Customer.Update(customer);
                _context.SaveChanges();
            }
        }
        public int Customer_Authenticate(LoginModel login)
        {
            Customer Customer = _context.Customer.Where(r => r.user_name.ToUpper() == login.user_name.ToUpper() || r.email.ToUpper() == login.user_name.ToUpper()).FirstOrDefault();
            if (Customer.is_delete)
            {
                return -1;
            }
            else
            {
                var passWord = Encryptor.MD5Hash(login.password + Customer.passcode);
                return passWord != Customer.password ? 2 : 1;
            }
        }
        public async Task<Order> OrderCreate(Order model)
        {
            model.code = Encryptor.RandomPassword();
            model.dateAdded = DateTime.Now;
            _context.Order.Add(model);
            _context.SaveChanges();
            foreach (var item in model.details)
            {
                item.order_id = model.id;
                item.order_code = model.code;
            }
            _context.OrderDetails.AddRange(model.details);
            _context.SaveChanges();
            return model;
        }
        public async Task<List<Order>> OrderList()
        {
            return _context.Order.Where(x => !x.is_delete).ToList();
        }
        public async Task<Order> OrderDetail(long id)
        {
            Order response = _context.Order.Find(id);
            response.details = _context.OrderDetails.Where(x => x.order_id == id && !x.is_delete).ToList();
            return response;
        }
        public async Task<Order?> OrderModify(Order model)
        {
            var order_db = _context.Order.AsNoTracking().FirstOrDefault(x => x.id == model.id);
            if (order_db != null && order_db.status != 1)
            {
                _context.Order.Update(model);
                var order_detail = _context.OrderDetails.Where(x => x.order_id == model.id && !x.is_delete).ToList();
                foreach (var item in order_detail)
                {
                    item.dateUpdated = DateTime.Now;
                    item.is_delete = true;
                }
                foreach (var item in model.details)
                {
                    item.id = 0;
                    item.order_id = model.id;
                    item.order_code = model.code;
                }
                _context.OrderDetails.UpdateRange(order_detail);
                _context.OrderDetails.AddRange(model.details);
                _context.SaveChanges();
                return model;
            }
            else
                return null;
        }
        public async Task<bool> Customer_Check_Active(string user_name, long user_id)
        {
            var customer_db = _context.Customer.FirstOrDefault(r => r.user_name.ToUpper() == user_name.ToUpper() || r.id == user_id);
            if (customer_db != null)
            {
                return true;
            }
            else return false;
        }
        public async Task<Customer> Customer_Check(string user_name)
        {
            return _context.Customer.Where(r => r.user_name.ToUpper() == user_name.ToUpper() || r.email.ToUpper() == user_name.ToUpper() && !r.is_delete).FirstOrDefault();
        }
        public async Task<Config_Port> Config_Port_Create(Config_Port model)
        {
            var port_db = _context.Config_Port.Where(x => x.Phone_Number == model.Phone_Number || x.Port_Name == model.Port_Name && x.Customer_Id == model.Customer_Id).ToList();
            _context.Config_Port.RemoveRange(port_db);
            _context.Config_Port.Add(model);
            _context.SaveChanges();
            return model;
        }
        public async Task<Config_Port> Config_Port_Modify(Config_Port model)
        {
            _context.Config_Port.Update(model);
            _context.SaveChanges();
            return model;
        }
        public async Task<List<Config_Port>> Config_Port_List(long customer_id)
        {
            return _context.Config_Port.Where(x => x.Customer_Id == customer_id).ToList();
        }
        public async Task<Customer_Config_Phone_Number> Customer_Config_Phone_Number_Create (Customer_Config_Phone_Number model)
        {
            _context.Customer_Config_Phone_Number.Add(model);
            _context.SaveChanges();
            return model;   
        }

        #endregion
    }
}
