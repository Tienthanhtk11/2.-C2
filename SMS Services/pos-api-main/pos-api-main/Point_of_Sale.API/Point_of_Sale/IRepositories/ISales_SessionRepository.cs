using Point_of_Sale.Entities;
using Point_of_Sale.Model;
using Point_of_Sale.Model.SaleSession;

namespace Point_of_Sale.IRepositories
{
    public interface ISales_SessionRepository
    {
        Task<long> Sales_SessionCreate(Sales_Session model);
        Task<Sales_SessionModel> Sales_SessionDetail(long id,long id_staff);
        Task<long> Sales_SessionCurrentId(long id_staff);
        Task<Sales_Session> Sales_SessionModify(Sales_Session model);
        Task<bool> Sales_SessionDelete(long id);
        Task<PaginationSet<Sales_SessionModel>> Sales_SessionList(SaleSessionSearch search);
    }
}
