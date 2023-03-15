using Point_of_Sale.Entities;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Promotion;

namespace Point_of_Sale.IRepositories
{
    public interface IPromotionRepository
    {
        Task<PromotionModel> Promotion(long id);
        Task<PromotionModel> PromotionCreate(PromotionModel model);
        Task<PromotionModel> PromotionModify(PromotionModel model);
        Task<bool> PromotionDelete(long id);
        Task<bool> PromotionApprove(long id, long userid); 
        Task<PaginationSet<PromotionModel>> PromotionList(PromotionSearch search); 
    }
}
