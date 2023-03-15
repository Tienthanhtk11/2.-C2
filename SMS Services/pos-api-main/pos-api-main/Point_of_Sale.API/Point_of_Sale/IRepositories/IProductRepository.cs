using Point_of_Sale.Entities;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Product;

namespace Point_of_Sale.IRepositories
{
    public interface IProductRepository
    {
        Task<ProductModel> Product(long id);
        Task<Product> ProductCreate(Product product);
        Task<bool> ProductDelete(long product_id, long user_id);
        Task<Product> ProductModify(Product product);
        Task<bool> ProductCheckDuplicate(Product product);
        Task<PaginationSet<ProductViewModel>> ProductList(ProductSearchModel search);
        Task<PaginationSet<ProductWarehouseModel2>> ComboList(ProductSearchModel search);
        Task<ProductWarehouseModel> ProductWarehouse(string barcode, long warehouse_id);
        Task<Product_Warehouse> ProductWarehouseGetById(long id);
        Task<Product_Warehouse> ProductWarehouseModify(Product_Warehouse model);
        Task<string> ProductWarehouseModifyPrintBarcode(List<long> model);
        Task<string> ProductWarehouseModifyPrintPrice(List<long> model);
        Task<PaginationSet<ProductRequestWarehouseModel>> ProductRequestWarehouseList(string? keyword, int page_size, int page_number, long warehouse_id);
        Task<List<ProductRequestWarehouseModel2>> ProductRequestWarehouseList2(long partner_id,long warehouse_id);
        Task<PaginationSet<ProductWarehouseModel>> ProductWarehouseList(string? keyword, int page_size, int page_number, long warehouse_id);
        Task<PaginationSet<ProductWarehouseModel2>> ProductWarehouseList2(string? keyword, int page_size, int page_number, long warehouse_id);
        Task<PaginationSet<ProductWarehouseModel>> ProductStockList(string? keyword, int page_size, int page_number, long warehouse_id);
        Task<PaginationSet<ProductWarehouseModel>> ProductDetailWarehouseList(ProductSearchModel search);
        Task UpdateFromExcel(Stream file);
        Task<PaginationSet<WarehousePriceHistoryModel>> GetChangePriceHistory(SearchWarehousePriceHistory search);
        Task<PaginationSet<ProductWarehouseModel>> ProductWarehousePromotionList(SearchPromotionModel search);

        // sản phẩm + NCC
        Task<Product_Partner_Model> Product_Partner_Create(Product_Partner_Model model);
        Task<Product_Partner_Model> Product_Partner_Update(Product_Partner_Model model);
        Task<List<Product_Partner_Model>> Product_Partner_List_By_Partner(long partner_id, string keyword = "");
        Task<List<Product_Partner_Model>> Product_Partner_List_By_Product(long product_id);

        // sản phẩm + combo 
        Task<Combo_Model> ComboCreate(Combo_Model model);
        Task<Combo_Model> ComboUpdate(Combo_Model model);
        Task<Combo_Model> Combo_Detail(long combo_id);
        // dong bo du lieu non-unicode
        Task<bool> Sync_Non_Unicode();
    }
}
