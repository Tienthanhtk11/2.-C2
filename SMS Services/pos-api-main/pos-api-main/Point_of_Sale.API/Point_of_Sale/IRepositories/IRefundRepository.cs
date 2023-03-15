using Point_of_Sale.Model;
using Point_of_Sale.Model.Refund;

namespace Point_of_Sale.IRepositories
{
    public interface IRefundRepository
    {
        Task<RefundModel> Refund(long id);
        Task<PaginationSet<RefundModel>> RefundList(RefundSearch search); 
        Task<RefundModel> RefundCreate(RefundModel Refunds);
        Task<RefundModel> RefundModify(RefundModel RefundModel);
        Task<List<RefundSearchOrderModel>> OrderList(string? keyword, long warehouse_id);
        Task<OrderCheckData> OrderCheck(RefundSearchOrderModel model);
        
    }
}
