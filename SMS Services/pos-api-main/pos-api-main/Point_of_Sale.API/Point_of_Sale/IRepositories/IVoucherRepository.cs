using Point_of_Sale.Model;

namespace Point_of_Sale.IRepositories
{
    public interface IVoucherRepository
    {
        Task<string> VoucherCreate(VoucherModel model);
        Task<VoucherModel> VoucherModify(VoucherModel model);
        Task<bool> VoucherDelete(long id);
        Task<VoucherModel> VoucherGetById(long id);
        Task<PaginationSet<VoucherModel>> VoucherList(VoucherSearch search);
      
    }
}
