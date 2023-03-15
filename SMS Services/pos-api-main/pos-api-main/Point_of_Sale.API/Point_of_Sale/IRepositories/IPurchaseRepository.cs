using Point_of_Sale.Model;
using Point_of_Sale.Model.Purchase;

namespace Point_of_Sale.IRepositories
{
    public interface IPurchaseRepository
    {
        Task<PurchaseModel> Purchase(long id);
        Task<string> PurchaseCreate(PurchaseModel model);
        Task<string> PurchaseDelete(long id);
        Task<string> PurchaseModify(PurchaseModel model);
        Task<PaginationSet<PurchaseViewModel>> PurchaseList(string? keyword, byte? status_id, DateTime? start_date, DateTime? end_date, int page_size, int page_number);
        Task<double> QuantityInventory(long warehouse_id, long product_id);

        
    }
}
