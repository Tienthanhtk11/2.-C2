using Point_of_Sale.Entities;

namespace Point_of_Sale.IRepositories
{
    public interface IBankAccountRepository
    {
        Task<string> bankAccountCreate(BankAccount model);
        Task<BankAccount> bankAccountModify(BankAccount model);
        Task<bool> bankAccountDelete(long id);
        Task<List<BankAccount>> bankAccountList();
    }
}
