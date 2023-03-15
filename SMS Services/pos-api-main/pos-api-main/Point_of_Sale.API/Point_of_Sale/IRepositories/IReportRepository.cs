using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Report;

namespace Point_of_Sale.IRepositories
{
    public interface IReportRepository
    {
        Task<RevenueProductModel> RevenueProducts(RevenueProductSearchModel search);
        Task<PaginationSet<InOutInventoryProductModel>> InOutInventoryProducts(InOutInventorySearchModel search);
        Task<PaginationSet<HistoryInventoryProductModel>> HistoryInventoryProducts(HistoryInventorySearchModel search);
        Task<List<Import_Export_Product_Model>> Daily_Import_Product_Report(long warehouse_id, int type, DateTime start_date, DateTime end_date);
        Task<List<Import_Export_Product_Model>> Daily_Export_Product_Report(long warehouse_id, int type, DateTime start_date, DateTime end_date);
        Task<Revenue_Book_Report> Daily_Order_List(long warehouse_id, DateTime start_date, DateTime end_date);
        Task<List<Sale_Session_Report>> Sales_SessionList(long warehouse_id, DateTime start_date, DateTime end_date);
        Task<List<Customer_Revenue>> Customer_Revenues(DateTime start_date, DateTime end_date);
        Task<List<Category_Revenue>> Category_Revenues(DateTime start_date, DateTime end_date);
        Task<List<Export_Product_Fast_Model>> Product_Export_Fast(long warehouse_id, DateTime start_date, DateTime end_date);
        Task<List<Export_Product_Fast_Model>> Product_Import_Fast(long warehouse_id, DateTime start_date, DateTime end_date);
        Task<List<Receipt_Cash_Form>> Receipt_Cash_Form_List(long warehouse_id, DateTime start_date, DateTime end_date);
        Task<List<Stock_Product>> Warehouse_Product_Fast(DateTime dateTime);
        Task<List<Stock_Product>> Warehouse_Product_Inventory_Fast(DateTime dateTime);
    }
}
