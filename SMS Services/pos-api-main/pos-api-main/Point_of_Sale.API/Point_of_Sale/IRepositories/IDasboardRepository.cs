using Point_of_Sale.Model.Dasboad;

namespace Point_of_Sale.IRepositories
{
    public interface IDasboardRepository
    {
        Task<PosInfoModel> DasboardInfo(long warehouse_id);
        Task<RevenueModel> DasboardRevenue(ChartSearch search);
        Task<List<TopProductModel>> DasboardTopProduct(ChartSearch search);
    }
}
