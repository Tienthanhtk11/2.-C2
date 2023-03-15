using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;

namespace Point_of_Sale.Repositories
{
    internal class VoucherRepository : IVoucherRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public VoucherRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<string> VoucherCreate(VoucherModel model)
        {
            return await Task.Run(() =>
           {
               using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
               {
                   string response = "";
                   try
                   {
                       Voucher voucher = _mapper.Map<Voucher>(model);
                       voucher.dateAdded = DateTime.Now;
                       //string code = Encryptor.RadomVoucher();
                       var check = _context.Voucher.Where(r => r.code == voucher.code /*&& voucher.active_date > r.end_date*/).Count();
                       if (check > 0)
                           return "Mã voucher đã tồn tại, vui lòng chọn mã khác!";
                       _context.Voucher.Add(voucher);
                       _context.SaveChanges();
                       transaction.Commit();
                       return voucher.id.ToString();
                   }
                   catch (Exception ex)
                   {
                       response = ex.Message + " - " + ex.StackTrace;
                       return response;
                   }
               }
           });
        }
        public async Task<VoucherModel> VoucherModify(VoucherModel model)
        {
            return await Task.Run(() =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    string response = "0";
                    try
                    {
                        Voucher voucher = _context.Voucher.FirstOrDefault(r => r.id == model.id);
                        voucher.name = model.name;
                        //  voucher.code = model.code;
                        voucher.reduction_rate = model.reduction_rate;
                        voucher.description = model.description;
                        voucher.reduction_price = model.reduction_price;
                        voucher.maximum_reduction = model.maximum_reduction;
                        voucher.used_quantity = model.used_quantity;
                        voucher.type = model.type;
                        voucher.status_id = model.status_id;
                        _context.Voucher.Update(voucher);
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
            });
        }
        public async Task<bool> VoucherDelete(long id)
        {
            return await Task.Run(() =>
            {
                using IDbContextTransaction transaction = _context.Database.BeginTransaction();
                try
                {
                    Voucher voucher = _context.Voucher.FirstOrDefault(r => r.id == id);
                    voucher.is_delete = true;
                    _context.Voucher.Update(voucher);
                    _context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }
        public async Task<VoucherModel> VoucherGetById(long id)
        {
            return await Task.Run(() =>
            {
                Voucher voucher = _context.Voucher.FirstOrDefault(r => r.id == id);
                return Task.FromResult(_mapper.Map<VoucherModel>(voucher));
            });
        }
        public async Task<PaginationSet<VoucherModel>> VoucherList(VoucherSearch search)
        {
            await Task.CompletedTask;

            PaginationSet<VoucherModel> response = new();
            IQueryable<VoucherModel> listItem = from a in _context.Voucher
                                                join c in _context.Admin_User on a.userAdded equals c.id
                                                where !a.is_delete
                                                select new VoucherModel
                                                {
                                                    id = a.id,
                                                    name = a.name,
                                                    code = a.code,
                                                    reduction_rate = a.reduction_rate,
                                                    description = a.description,
                                                    reduction_price = a.reduction_price,
                                                    maximum_reduction = a.maximum_reduction,
                                                    used_quantity = a.used_quantity,
                                                    type = a.type,
                                                    end_date = a.end_date,
                                                    active_date = a.active_date,
                                                    status_id = a.status_id,
                                                    dateAdded = a.dateAdded,
                                                    userAdded = a.userAdded,
                                                    created_name = c.full_name,
                                                };
            if (search.start_date != null)
            {
                var start_date = search.start_date.Value.Date;
                listItem = listItem.Where(r => r.dateAdded >= start_date);
            }
            if (search.end_date != null)
            {
                var end_date = search.end_date.Value.Date.AddDays(1);
                listItem = listItem.Where(r => end_date >= r.dateAdded);
            }
            if (search.code is not null and not "")
            {
                listItem = listItem.Where(r => r.code.Contains(search.code) || r.name.Contains(search.code));
            }
            if (search.userAdded != null)
            {
                listItem = listItem.Where(r => r.userAdded == search.userAdded);
            }
            if (search.status != null)
            {
                listItem = listItem.Where(r => r.status_id == search.status);
            }
            if (search.page_number > 0)
            {
                response.totalcount = listItem.Select(x => x.id).Count();
                response.page = search.page_number;
                response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / search.page_size);
                response.lists = await listItem.OrderByDescending(r => r.dateAdded).Skip(search.page_size * (search.page_number - 1)).Take(search.page_size).ToListAsync();
            }
            else
            {
                response.lists = await listItem.OrderByDescending(r => r.dateAdded).ToListAsync();
            }
            return response;
        }

    }
}



