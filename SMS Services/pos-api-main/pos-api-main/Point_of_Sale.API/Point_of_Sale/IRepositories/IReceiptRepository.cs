using Point_of_Sale.Model;
using Point_of_Sale.Model.Receipt;

namespace Point_of_Sale.IRepositories
{
    public interface IReceiptRepository
    {
        Task<ReceiptModel> Receipt(long id);
        Task<string> ReceiptCreate(ReceiptModel model);
        Task<string> ReceiptModify(ReceiptModel model);
        Task<bool> ReceiptDelete(long id);
        Task<string> GetBarcode(long id);
        Task<Receipt_Print_Model> ReceiptExportPrint(long id); 
        Task<PaginationSet<ReceiptViewModel>> ReceiptList(SearchReceipt search);
    }
}
