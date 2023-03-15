using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;

namespace Point_of_Sale.Repositories
{
    internal class BankAccountRepository: IBankAccountRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public BankAccountRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<string> bankAccountCreate(BankAccount model)
        {
            return await Task.Run(() =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    string response = "0";
                    try
                    {
                        model.dateAdded = DateTime.Now;
                        _context.BankAccount.Add(model);
                        _context.SaveChanges();
                        transaction.Commit();
                        return response;
                    }
                    catch (Exception ex)
                    {
                        response = ex.Message + " - " + ex.StackTrace;
                        return response;
                    }
                }
            });
        }
        public async Task<BankAccount> bankAccountModify(BankAccount model)
        {
            return await Task.Run(() =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    string response = "0";
                    try
                    {
                        //BankAccount bankAccount = _context.BankAccount.FirstOrDefault(r => r.id == model.id);
                        //bankAccount.name = model.name;
                        //bankAccount.account_number = model.account_number;
                        //bankAccount.note = model.note;
                        _context.BankAccount.Update(model);
                        _context.SaveChanges();
                        transaction.Commit();
                        return model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response = ex.Message + " - " + ex.StackTrace;
                        return model;
                    }
                }
                return model;
            });
        }
        public async Task<bool> bankAccountDelete(long id)
        {
            return await Task.Run(() =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        BankAccount bankAccount = _context.BankAccount.FirstOrDefault(r => r.id == id);
                        bankAccount.is_delete = true;
                        _context.BankAccount.Update(bankAccount);
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception )
                    {
                        return false;
                    }
                }
               
            });
        }
        public async Task<List<BankAccount>> bankAccountList()
        {
            var response = await _context.BankAccount.ToListAsync();
            return response;
        }
    }
}
