using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.Model;

namespace Point_of_Sale.IRepositories
{
    public interface ICategoryRepository
    {
        Task<Category_Product> CategoryProduct(long id);
        Task<PaginationSet<Category_Product>> CategoryProductList(SearchBase search);
        Task<Category_Product> CategoryProductCreate(Category_Product product);
        Task<Category_Product> CategoryProductModify(Category_Product product);
        Task<bool> CategoryProductDelete(long category_id, long user_id);
        Task<Category_Unit> CategoryUnit(long id);
        Task<Category_Unit> CategoryUnitCreate(Category_Unit product);
        Task<Category_Unit> CategoryUnitModify(Category_Unit product);
        Task<PaginationSet<Category_Unit>> CategoryUnitList(SearchBase search);
        Task<bool> CategoryUnitDelete(long category_id, long user_id);
        Task<Category_Packing> CategoryPacking(long id);
        Task<PaginationSet<Category_Packing>> CategoryPackingList(SearchBase search);
        Task<Category_Packing> CategoryPackingCreate(Category_Packing product);
        Task<Category_Packing> CategoryPackingModify(Category_Packing product);
        Task<bool> CategoryPackingDelete(long category_id, long user_id);
        #region province
        Task<Category_Province> ProvinceCreate(Category_Province model);
        Task<List<Category_Province>> ProvincesListId(List<long> ids);
        Task<Category_Province> ProvinceGetById(long id);
        Task<Category_Province> ProvinceModify(Category_Province model);
        Task<List<Category_Province>> ProvinceList(string language_code);
        Task<PaginationSet<Category_Province>> ProvinceListView(string language_code, int page_number, int page_size);
        Task<bool> ProvinceDelete(long id);
        #endregion

        #region district
        Task<Category_District> DistrictCreate(Category_District model);
        Task<Category_District> DistrictGetById(long id);
        Task<Category_District> DistrictModify(Category_District model);
        Task<List<Category_District>> DistrictList(string language_code);
        Task<List<Category_District>> DistrictListProvinceId(long province_id, string language_code);
        Task<bool> DistrictDelete(long id);
        #endregion

        #region ward
        Task<Category_Ward> WardCreate(Category_Ward model);
        Task<Category_Ward> WardGetById(long id);
        Task<Category_Ward> WardModify(Category_Ward model);
        Task<List<Category_Ward>> WardList(string language_code);
        Task<List<Category_Ward>> WardListDistrictId(long district_id, string language_code);
        Task<bool> WardDelete(long id);
        #endregion

        #region danh muc cap 2 3
        Task<List<Category_Stalls>> CategoryStallsList(string? keyword, long category_id);
        Task<Category_Stalls> CategoryStallsCreate(Category_Stalls category);
        Task<Category_Stalls> CategoryStallsModify(Category_Stalls category);
        // danh muc nhom hang hoa
        Task<List<Category_Group>> CategoryGroupList(string? keyword, long stalls_id);
        Task<Category_Group> CategoryGroupCreate(Category_Group category);
        Task<Category_Group> CategoryGroupModify(Category_Group category);
        #endregion
    }
}
