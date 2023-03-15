using Point_of_Sale.Model;
using Point_of_Sale.Model.Export;

namespace Point_of_Sale.IRepositories
{
    public interface IWarehouseExportRepository
    {
        Task<Warehouse_Export_Model> Warehouse_ExportDetail(long id);
        Task<Warehouse_Export_Print_Model> Warehouse_ExportPrint(long id); 
        Task<Warehouse_Export_Model> Warehouse_ExportCreate(Warehouse_Export_Model model);
        Task<Warehouse_Export_Model> Warehouse_ExportModify(Warehouse_Export_Model model);
        Task<bool> Warehouse_ExportDelete(long id);
        Task<PaginationSet<Warehouse_ExportViewModel>> Warehouse_ExportList(long partner_id, long warehouse_id, byte? status_id, string? keyword, int page_number, int page_size, DateTime start_date, DateTime end_date);
        Task<bool> Warehouse_ExportConfirm(long id,long userUpdated);
        Task<bool> Warehouse_ExportApprove(long id,long userUpdated); 
        Task<string> Warehouse_ExportVerify(long source_id, long destination_id,int type);

    }
}
