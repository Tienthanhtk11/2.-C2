using Point_of_Sale.Entities;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Product;

namespace Point_of_Sale.IRepositories
{
    public interface IPartnerRepository
    {
        Task<PartnerModel> Partner(long id);
        Task<PartnerModel> PartnerCreate(PartnerModel model);
        Task<PartnerModel> PartnerModify(PartnerModel model);
        Task<bool> PartnerDelete(long id);
        Task<PartnerModel> PartnerGetById(long id);
        Task<PaginationSet<PartnerModel>> PartnerList(SearchBase search);
        Task<List<Partner>> PartnerListAll();
    }
}
