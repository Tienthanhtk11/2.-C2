using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using OfficeOpenXml.Style.XmlAccess;
using SMS_Services.Common;
using SMS_Services.Entity;
using SMS_Services.Model;
using System.IO.Ports;
using System.ServiceModel.Description;
using System.Text.RegularExpressions;

namespace SMS_Services.Repository
{

    internal class SMSRepository : ISMSRepository
    {
        private readonly ApplicationDbContext _context;
        public SMSRepository(ApplicationDbContext context
            ) => _context = context;
        SerialPort serialPort = new();
        ExtractSMS extractSMS = new();
        #region SMS REPO
        public async Task<string> InsertSMS_Bulk(List<OrderDetails> list)
        {
            return await Task.Run(async () =>
            {
                _context.OrderDetails.AddRange(list);
                _context.SaveChanges();
                return "";
            });
        }
        public async Task<Order> InsertOrder(Order contract)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    contract.dateAdded = DateTime.Now;
                    _context.Order.Add(contract);
                    _context.SaveChanges();
                    return contract;
                }
                catch (Exception ex) { return null; }
            });
        }
        public async Task<Order> UpdateOrder(Order contract)
        {
            return await Task.Run(async () =>
            {
                Order contract_db = await _context.Order.AsNoTracking().FirstOrDefaultAsync(x => x.id == contract.id);
                if (contract_db != null)
                {
                    contract.dateUpdated = DateTime.Now;
                    _context.Order.Update(contract);
                    _context.SaveChanges();
                }
                return contract;
            });
        }
        public async Task<Order> GetOrderByOrderNo(string order_code)
        {
            Order contract_db = await _context.Order.AsNoTracking().FirstOrDefaultAsync(x => x.code == order_code);
            if (contract_db != null)
            {
                return contract_db;
            }
            else
                return null;
        }
        public async Task<List<OrderDetails>> GetOrderDetails(string order_code)
        {
            return await Task.Run(async () =>
            {
                List<OrderDetails> list = new();
                list = await _context.OrderDetails.Where(x => x.order_code == order_code).ToListAsync();
                return list;
            });
        }
        public async Task<List<Port>> PortList()
        {
            return _context.Port.Where(x => !x.is_delete).ToList();
        }
        public void check_port()
        {
            List<Port> list_port = new();
            for (int i = 1; i < 100; i++)
            {
                try
                {
                    SerialPort serialPort = new();
                    Console.WriteLine("COM" + i);
                    serialPort = extractSMS.OpenPort("COM" + i, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                    //+COPS: 0,0,"Mobifone"
                    string network = extractSMS.GetSimNetWork(serialPort);
                    Console.WriteLine(network);
                    Port port = new()
                    {
                        name = "COM" + i,
                        telco = "network",
                    };
                    string raw_info = extractSMS.CheckMoney(serialPort);
                    Console.WriteLine(raw_info);
                    if (raw_info != null && raw_info != "")
                    {
                        Regex phone_number_rule = new(@"(0|84)\d{9}");
                        port.phone_number = phone_number_rule.Match(raw_info).Groups[0].Value;
                        Console.WriteLine(port.phone_number);
                        Regex balance_rule = new(@"(\d+) ?(VND|d)"); //Regex balance_rule = new Regex(@"([\.\d]+) ?(VND|d)");
                        port.cash = int.Parse(balance_rule.Match(raw_info.Replace(".", string.Empty)).Groups[1].Value);
                        Console.WriteLine(port.cash);
                    }
                    else
                        port.cash = 0;
                    list_port.Add(port);
                    extractSMS.ClosePort(serialPort);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    extractSMS.ClosePort(serialPort);
                }

            }
            List<Port> list_port_db = _context.Port.Where(x => !x.is_delete).ToList();
            list_port_db.ForEach(x => x.is_delete = true);
            _context.Port.UpdateRange(list_port_db);
            _context.Port.AddRange(list_port);
            _context.SaveChanges();
        }
        public async void SendSMSDirect(string phone_number, string message)
        {
            Port current_port = new Port();
            bool connect_gsm = false;
            List<Port> ports = _context.Port.Where(x => !x.is_delete).ToList();
            foreach (var item in ports)
            {
                if (item.cash > 300)
                {
                    connect_gsm = loadPort(item.name);
                    if (connect_gsm)
                    {
                        current_port = item;
                        break;
                    }
                }
            }
            bool smsSent = extractSMS.sendMsg(this.serialPort, phone_number, message, 500);
            SendSMSHistory sms_history = new()
            {
                order_code = "single send sms api",
                phone_receive = phone_number,
                message = message,
                telco = "not check",
                sum_sms = 1,
                status = 1,
                phone_send = current_port.phone_number
            };
            _context.SendSMSHistory.Add(sms_history);
            _context.SaveChanges();

        }
        public bool loadPort(string com_port)
        {
            try
            {
                serialPort = extractSMS.OpenPort(com_port, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<List<Message_Receive>> GetListSMSByPhone(string phone_receive, string phone_send)
        {
            List<Message_Receive> ListSMS = new();
            ListSMS = _context.Message_Receive.Where(x => !x.is_delete && x.phone_receive == phone_receive && phone_send == phone_send).OrderByDescending(x => x.id).Take(5).ToList();
            return ListSMS;
        }
        public async Task<List<Message_Receive>> GetListSMSByPhone2(string phone_receive, string? phone_send)
        {
            try
            {
                List<Message_Receive> ListSMS = new();
                Port portDB = _context.Port.FirstOrDefault(x => !x.is_delete && x.phone_number == phone_receive);
                if (portDB != null)
                {
                    bool connect_gsm = loadPort(portDB.name);
                    if (connect_gsm)
                    {
                        Console.WriteLine("chay vao doc sms tren port {0} roi", portDB.name);
                        ListSMS = extractSMS.ReadSMS(this.serialPort);
                        if (ListSMS != null)
                        {
                            foreach (var sms in ListSMS)
                            { //bố sung thêm sdt nhận vào record trước khi lưu db
                                Console.WriteLine(sms.message);
                                sms.phone_receive = portDB.phone_number;
                            }
                            if (phone_send != null && phone_send != "")
                            {
                                ListSMS = ListSMS.Where(x => x.phone_send == phone_send).ToList();
                            }
                        }
                        //break;
                    }

                }
                return ListSMS;
            }
            catch (Exception exx)
            {
                await Console.Out.WriteLineAsync(exx.Message);
                throw;
            }
        }
        public async Task<List<Message_Receive>> GetListSMSReceive(long user_id)
        {
            return _context.Message_Receive.Where(x => !x.is_delete && x.userAdded == user_id).OrderByDescending(x => x.id).ToList();
        }
        public async Task<List<Message_Receive>> GetListSMSReceiveAdmin()
        {
            return _context.Message_Receive.Where(x => !x.is_delete).OrderByDescending(x => x.id).ToList();
        }
        public async Task<string> Create_SMS_Receive(List<Message_Receive> model)
        {
            try
            {
                _context.Message_Receive.AddRange(model);
                _context.SaveChanges();
                return "OK";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion
        #region Admin 
        public async Task<Admin_User> UserGetById(long id)
        {
            return _context.Admin_User.Find(id);
        }
        public async Task<Admin_User> UserCreate(Admin_User model)
        {
            model.passcode = Encryptor.RandomPassword();
            model.password = Encryptor.MD5Hash(model.password + model.passcode);
            _context.Admin_User.Add(model);
            _context.SaveChanges();
            return model;
        }
        public async Task<Admin_User> UserModify(Admin_User model)
        {
            var user = _context.Admin_User.FirstOrDefault(r => r.id == model.id);
            user.email = model.email;
            user.name = model.name;
            user.dateUpdated = DateTime.Now;
            _context.Admin_User.Update(user);
            _context.SaveChanges();
            return model;
        }
        public async Task<List<Admin_User>> UserList(string? user_name)
        {
            IEnumerable<Admin_User> listItem = _context.Admin_User;
            if (user_name != null && user_name != "")
            {
                listItem = listItem.Where(r => r.name.Contains(user_name));
            }
            List<Admin_User> lists = listItem.OrderByDescending(r => r.id).ToList();
            return lists;
        }
        public int Authenticate(LoginModel login)
        {
            Admin_User user = _context.Admin_User.Where(r => r.user_name.ToUpper() == login.user_name.ToUpper() || r.email.ToUpper() == login.user_name.ToUpper()).FirstOrDefault();
            if (user.is_delete)
            {
                return -1;
            }
            else
            {
                var passWord = Encryptor.MD5Hash(login.password + user.passcode);
                return passWord != user.password ? 2 : 1;
            }
        }
        public async Task<Admin_User> CheckUser(string user_name)
        {
            return _context.Admin_User.Where(r => r.user_name.ToUpper() == user_name.ToUpper() || r.email.ToUpper() == user_name.ToUpper()).FirstOrDefault();

        }
        public async Task<int> CheckUserExists(string user_name)
        {
            return _context.Admin_User.Where(r => r.user_name.ToUpper() == user_name.ToUpper() || r.email.ToUpper() == user_name.ToUpper()).Count();
        }
        #endregion

        #region Request SMS
        public async Task<bool> Request(Data_Upload data)
        {
            Random rnd = new Random();
            _context.SMS_Template.AddRange(data.list_sms_template);
            _context.SaveChanges();
            List<SMS_Request_Customer> list_sms = new();
            foreach (var item in data.list_phone_number)
            {
                SMS_Request_Customer sms = new()
                {
                    phone_receive = item.phone_number,
                    telco = item.phone_number
                };
                int sms_index = rnd.Next(0, data.list_sms_template.Count());
                sms.message = data.list_sms_template[sms_index].message;
                sms.template_id = data.list_sms_template[sms_index].id;
                sms.customer_id = data.list_sms_template[sms_index].customer_id;
                list_sms.Add(sms);
            }
            _context.SMS_Request_Customer.AddRange(list_sms);
            _context.SaveChanges();
            return true;

        }
        #endregion
    }
}
