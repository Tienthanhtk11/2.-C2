using Point_of_Sale.Entities;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Warehouse;

namespace Point_of_Sale.IRepositories
{
    public interface IWarehouseRepository
    {
        Task<Warehouse> Warehouse(long id);
        Task<Warehouse> WarehouseCreate(Warehouse_Model model);

        Task<Warehouse> WarehouseModify(Warehouse_Model warehouse);

        Task<PaginationSet<Warehouse_Model>> WarehouseList(SearchBase searchBase);
        Task<bool> WarehouseCheckDuplicate(Warehouse_Model warehouse);
        Task<List<UserWarehouseModel>> WarehouseUserList(long user_id);

        
    }
}
