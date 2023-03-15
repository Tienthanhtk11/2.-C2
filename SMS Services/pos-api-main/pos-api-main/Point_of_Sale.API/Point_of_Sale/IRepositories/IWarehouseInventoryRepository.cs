using Point_of_Sale.Model.Inventory;
using Point_of_Sale.Model;

namespace Point_of_Sale.IRepositories
{
    public interface IWarehouseInventoryRepository
    {
        Task<Warehouse_Inventory_Model> Warehouse_InventoryDetail(long id);
        Task<Warehouse_Inventory_Print_Model> Warehouse_InventoryPrint(long id);
        Task<Warehouse_Inventory_Model> Warehouse_InventoryCreate(Warehouse_Inventory_Model model);
        Task<Warehouse_Inventory_Model> Warehouse_InventoryModify(Warehouse_Inventory_Model model);
        Task<bool> Warehouse_InventoryDelete(long id);
        Task<PaginationSet<Warehouse_Inventory_Model>> Warehouse_InventoryList(Warehouse_Inventory_Search search);
        Task<bool> Warehouse_InventoryConfirm(long id, long userUpdated);
        Task<bool> Warehouse_InventoryReject(long id, long userUpdated);
        
    }
}
