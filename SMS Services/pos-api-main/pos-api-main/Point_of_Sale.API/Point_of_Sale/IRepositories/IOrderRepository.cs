using Point_of_Sale.Model;

namespace Point_of_Sale.IRepositories
{
    public interface IOrderRepository
    {
        Task<OrderModel> Order(long id);
        Task<PaginationSet<OrderModel>> OrderList(OrderSearch search);
        Task<List<OrderModel>> OrderList2();
        Task<OrderModel> OrderCreate(OrderModel orders);
        Task<OrderModel> OrderModify(OrderModel orderModel);
        Task<string> OrderDelete(long id);
        Task<string> OrdersCreate(List<OrderModel> orders);
    }
}
