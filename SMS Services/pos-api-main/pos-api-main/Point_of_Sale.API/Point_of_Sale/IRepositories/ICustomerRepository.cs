using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Customer;

namespace Point_of_Sale.IRepositories
{
    public interface ICustomerRepository
    {
        Task<CustomerModel> Customer(long id);
        Task<PaginationSet<CustomerModel>> CustomerList(SearchBase searchBase);
        Task<CustomerModel> CustomerCreate(CustomerModel Customers);
        Task<CustomerModel> CustomerModify(CustomerModel CustomerModel);
        Task<string> CustomerDelete(long id);
        Task<Customer_Member_Config> ConfigCreate(Customer_Member_Config config);
        Task<Customer_Member_Config> ConfigUpdate(Customer_Member_Config config);
        Task<string> ConfigDelete(long id, long user_id);
        Task<PaginationSet<Customer_Point_History_Model>> List_Customer_Point(long customer_id, int page_size, int page_number);
        Task<Customer_Member_Config> ConfigDetail(long config_id);
        Task<List<Customer_Member_Config>> ConfigList();
        Task<Customer_Member_Config> ConfigActive();
    }
}
