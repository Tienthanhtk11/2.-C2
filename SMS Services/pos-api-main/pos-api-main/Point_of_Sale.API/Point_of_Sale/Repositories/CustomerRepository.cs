using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Customer;

namespace Point_of_Sale.Repositories
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CustomerRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CustomerModel> Customer(long id)
        {
            return await Task.Run(async () =>
            {
                CustomerModel model = new CustomerModel();
                Customer Customer = await _context.Customer.FirstOrDefaultAsync(r => r.id == id);


                model = _mapper.Map<CustomerModel>(Customer);
                model.count_purchases = _context.Order.Where(x => x.customer_id == id).Count();
                model.total_price  = (double)_context.Order.Where(x => x.customer_id == id).Select(x => x.total_amount).Sum();
                return model;
            });
        }
        public async Task<PaginationSet<CustomerModel>> CustomerList(SearchBase searchBase)
        {
            await Task.CompletedTask;
            PaginationSet<CustomerModel> response = new PaginationSet<CustomerModel>();
            IQueryable<CustomerModel> listItem = from a in _context.Customer
                                                 select new CustomerModel
                                                 {
                                                     id = a.id,
                                                     dateUpdated = a.dateUpdated,
                                                     member_point = a.member_point,
                                                     name = a.name,
                                                     phone = a.phone,
                                                     province_code = a.province_code,
                                                     code = a.code,
                                                     email = a.email,
                                                     address = a.address,
                                                     count_purchases = _context.Order.Where(x => x.customer_id == a.id).Count(),
                                                     total_price = (double)_context.Order.Where(x => x.customer_id == a.id).Select(x => x.total_amount).Sum(),
                                                     dateAdded = a.dateAdded,
                                                     userAdded = a.userAdded
                                                 };

            if (searchBase.keyword != null && searchBase.keyword != "")
            {
                listItem = listItem.Where(r => r.name.Contains(searchBase.keyword) || r.phone.Contains(searchBase.keyword) || r.code.Contains(searchBase.keyword));
            }
            if (searchBase.province_code!= null && searchBase.province_code != 0 )
            {
                listItem = listItem.Where(r => r.province_code == searchBase.province_code);
            }

            if (searchBase.page_number > 0)
            {
                response.totalcount = listItem.Select(x => x.id).Count();
                response.page = searchBase.page_number;
                response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / searchBase.page_size);
                response.lists = await listItem.OrderByDescending(r => r.dateAdded).Skip(searchBase.page_size * (searchBase.page_number - 1)).Take(searchBase.page_size).ToListAsync();
            }
            else
            {
                response.lists = await listItem.OrderByDescending(r => r.dateAdded).ToListAsync();
            }
            return response;
        }
        public async Task<CustomerModel> CustomerCreate(CustomerModel Customers)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var checkCustomer = await _context.Customer.FirstOrDefaultAsync(e =>
                     e.phone == Customers.phone);
                    if (checkCustomer != null)
                    {
                        return _mapper.Map<CustomerModel>(checkCustomer);
                    }
                    Customer Customer = _mapper.Map<Customer>(Customers);
                    Customer.dateAdded = DateTime.Now;
                    int count = _context.Customer.Count(x => x.province_code == Customers.province_code);
                    count++;
                    if (count < 1000)
                    {
                        string gen_code = Encryptor.auto_gen_code(count);
                        Customer.code += gen_code;
                    }
                    else
                        Customer.code += count;
                    _context.Customer.Add(Customer);
                    _context.SaveChanges();
                    CustomerModel Customer_response = _mapper.Map<CustomerModel>(Customer);
                    return Customer_response;
                }
                catch (Exception ex)
                {
                    return new CustomerModel();
                }
            });
        }
        public async Task<CustomerModel> CustomerModify(CustomerModel CustomerModel)
        {
            return await Task.Run(async () =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Customer Customer = await _context.Customer.FirstOrDefaultAsync(x => x.id == CustomerModel.id);
                        Customer.dateUpdated = DateTime.Now;
                        Customer.name = CustomerModel.name;
                        Customer.phone = CustomerModel.phone;
                        Customer.email = CustomerModel.email;
                        Customer.address = CustomerModel.address;
                        Customer.count_purchases = CustomerModel.count_purchases;
                        Customer.total_price = CustomerModel.total_price;
                        _context.Customer.Update(Customer);
                        _context.SaveChanges();

                        transaction.Commit();
                        return CustomerModel;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new CustomerModel();
                    }
                }
            });
        }
        public async Task<string> CustomerDelete(long id)
        {
            return await Task.Run(async () =>
            {
                Customer CustomerDB = await _context.Customer.FirstOrDefaultAsync(x => x.id == id);
                CustomerDB.is_delete = true;
                CustomerDB.dateUpdated = DateTime.Now;
                _context.Customer.Update(CustomerDB);
                _context.SaveChanges();
                return "0";
            });
        }
        public async Task<Customer_Member_Config> ConfigCreate(Customer_Member_Config config)
        {
            return await Task.Run(async () =>
            {
                if (config.is_active == true)
                {
                    var config_db = _context.Customer_Member_Config.Where(x => x.is_active).ToList();
                    if (config_db.Count()>0)
                    {
                        foreach (var item in config_db)
                        {
                            item.is_active = false;
                        }
                        _context.Customer_Member_Config.UpdateRange(config_db);
                    }
                }
                _context.Customer_Member_Config.Add(config);
                _context.SaveChanges();
                return config;
            });
        }
        public async Task<Customer_Member_Config> ConfigUpdate(Customer_Member_Config config)
        {
            return await Task.Run(async () =>
            {
                if (config.is_active == true)
                {
                    var config_db = _context.Customer_Member_Config.Where(x => x.is_active).ToList();
                    foreach (var item in config_db)
                    {
                        item.is_active = false;
                    }
                    _context.Customer_Member_Config.UpdateRange(config_db);
                }
                _context.Customer_Member_Config.Update(config);
                _context.SaveChanges();
                return config;
            });
        }
        public async Task<Customer_Member_Config> ConfigDetail(long config_id)
        {
            await Task.CompletedTask;
            Customer_Member_Config config_db = await _context.Customer_Member_Config.FirstOrDefaultAsync(x => x.id == config_id && !x.is_delete);
            if (config_db != null)
            {
                return config_db;
            }
            else
                return null;
        }
        public async Task<Customer_Member_Config> ConfigActive()
        {
            await Task.CompletedTask;
            Customer_Member_Config config_db = await _context.Customer_Member_Config.FirstOrDefaultAsync(x => x.is_active && DateTime.Now >= x.start_date && x.end_date >= DateTime.Now && !x.is_delete);
            if (config_db != null)
            {
                return config_db;
            }
            else
                return null;
        }
        public async Task<List<Customer_Member_Config>> ConfigList()
        {
            await Task.CompletedTask;
            List<Customer_Member_Config> config_db = new();
            config_db = await _context.Customer_Member_Config.Where(x => !x.is_delete).OrderByDescending(x => x.id).ToListAsync();
            return config_db;
        }
        public async Task<string> ConfigDelete(long id, long user_id)
        {
            return await Task.Run(async () =>
            {
                Customer_Member_Config config_db = await _context.Customer_Member_Config.FirstOrDefaultAsync(x => x.id == id && !x.is_delete);
                if (config_db != null)
                {
                    config_db.is_delete = true;
                    config_db.dateUpdated = DateTime.Now;
                    config_db.userUpdated = user_id;
                    _context.Customer_Member_Config.Update(config_db);
                    _context.SaveChanges();
                    return "Xóa thành công";
                }
                else
                    return "Bản ghi không tồn tại hoặc đã bị xóa trước đó";

            });
        }

        public async Task<PaginationSet<Customer_Point_History_Model>> List_Customer_Point(long customer_id, int page_size, int page_number)
        {
            await Task.CompletedTask;
            PaginationSet<Customer_Point_History_Model> response = new PaginationSet<Customer_Point_History_Model>();
            IQueryable<Customer_Point_History_Model> listItem = from a in _context.Customer_Point_History
                                                                join b in _context.Order on a.order_id equals b.id
                                                                select new Customer_Point_History_Model
                                                                {
                                                                    id = a.id,
                                                                    dateAdded = a.dateAdded,
                                                                    order_code = b.code,
                                                                    order_id = a.order_id,
                                                                    customer_id = a.customer_id,
                                                                    number_of_point = a.number_of_point,
                                                                    value_of_point = a.value_of_point,
                                                                    type = a.type,
                                                                };  
            if (customer_id != 0)
            {
                listItem = listItem.Where(r => r.customer_id == customer_id);
            }
            if (page_number > 0)
            {
                response.totalcount = listItem.Select(x => x.id).Count();
                response.page = page_number;
                response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / page_size);
                response.lists = await listItem.OrderByDescending(r => r.dateAdded).Skip(page_size * (page_number - 1)).Take(page_size).ToListAsync();
            }
            else
            {
                response.lists = await listItem.OrderByDescending(r => r.dateAdded).ToListAsync();
            }
            return response;
        }
    }
}

