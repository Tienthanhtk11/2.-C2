using Point_of_Sale.Model;
using Point_of_Sale.Model.Request;
using Point_of_Sale.Model.Warehouse;

namespace Point_of_Sale.IRepositories
{
    public interface IRequestRepository
    {
        Task<string> RequestModifyStatus(long id, byte status);
        Task<RequestModel> Request(long id);
        Task<string> RequestCreate(RequestModel model);
        Task<string> RequestModify(RequestModel model);
        Task<PaginationSet<RequestViewModel>> RequestList(Warehouse_Request_Search search);
    }
}
