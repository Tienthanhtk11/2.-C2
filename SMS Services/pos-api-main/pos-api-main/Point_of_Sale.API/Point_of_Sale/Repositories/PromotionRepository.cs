using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Promotion;
using Point_of_Sale.Model.Receipt;
using System.Security.Permissions;

namespace Point_of_Sale.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {

        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public PromotionRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<PromotionModel> Promotion(long id)
        {
            return await Task.Run(async () =>
            {
                PromotionModel model = new PromotionModel();
                var promotion = await _context.Promotion.FirstOrDefaultAsync(r => r.id == id);
                model = _mapper.Map<PromotionModel>(promotion);
                return model;
            });
        }

        public async Task<PromotionModel> PromotionCreate(PromotionModel model)
        {
            return await Task.Run(() =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Promotion promotion = _mapper.Map<Promotion>(model);
                        promotion.dateAdded = DateTime.Now; 
                        _context.Promotion.Add(promotion);
                        _context.SaveChanges();
                        if (model.warehouses != null && model.warehouses.Count > 0)
                        {
                            model.warehouses.ForEach(e =>
                            {
                                e.promotion_id = promotion.id;
                                e.userAdded = promotion.userAdded;
                                e.dateAdded = DateTime.Now;
                            });
                            _context.Promotion_Warehouse.AddRange(model.warehouses);
                        }
                        if (model.scheduleTimes != null && model.scheduleTimes.Count > 0)
                        {
                            model.scheduleTimes.ForEach(e =>
                            {
                                e.promotion_id = promotion.id;
                                e.userAdded = promotion.userAdded;
                                e.dateAdded = DateTime.Now;
                            });
                            _context.Promotion_Schedule_Time.AddRange(model.scheduleTimes);
                        }
                        if (model.schedules != null && model.schedules.Count > 0)
                        {
                            model.schedules.ForEach(e =>
                            {
                                e.promotion_id = promotion.id;
                                e.userAdded = promotion.userAdded;
                                e.dateAdded = DateTime.Now;
                            });
                            _context.Promotion_Schedule.AddRange(model.schedules);
                        }
                        promotion.code = "P-" + promotion.id; 
                        _context.Promotion.Update(promotion);
                        transaction.Commit();
                        PromotionModel response = _mapper.Map<PromotionModel>(promotion);
                        return response;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new PromotionModel();
                    }
                }
            });
        }

        public async Task<bool> PromotionDelete(long id)
        {
            return await Task.Run(async () =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    { 
                        var promotion_Products = _context.Promotion_Product.Where(r => r.promotion_id == id);
                        if (promotion_Products != null && promotion_Products.Any())
                        {
                            foreach (var item in promotion_Products)
                            {
                                item.is_delete = false;
                            }
                            _context.Promotion_Product.UpdateRange(promotion_Products);
                        } 
                        var promotion_Product_Items = _context.Promotion_Product_Item.Where(r => r.promotion_id == id);
                        if (promotion_Product_Items != null && promotion_Product_Items.Any())
                        {
                            foreach (var item in promotion_Product_Items)
                            {
                                item.is_delete = false;
                            }
                            _context.Promotion_Product_Item.UpdateRange(promotion_Product_Items);
                        }
                        var promotion = await _context.Promotion.FirstOrDefaultAsync(r => r.id == id);
                        if (promotion != null)
                        {
                            promotion.is_delete = true;
                            _context.Promotion.Update(promotion);
                            _context.SaveChanges();
                        } 
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            });
        }


        public async Task<PaginationSet<PromotionModel>> PromotionList(PromotionSearch search)
        {
            return await Task.Run(async () =>
            {
                PaginationSet<PromotionModel> response = new PaginationSet<PromotionModel>();
                var listItem = from a in _context.Promotion 
                               join b in _context.Admin_User on a.userAdded equals b.userAdded
                               where !a.is_delete
                               select new PromotionModel
                               {  
                                   name = a.name,
                                   note = a.note,
                                   id = a.id,  
                                   type = a.type,
                                   code = a.code,
                                   status = a.status,
                                   date_start = a.date_start,
                                   date_end = a.date_end,
                                   all_warehouse = a.all_warehouse,
                                   userAdded = a.userAdded,
                                   user_name = b.full_name,
                                   dateAdded = a.dateAdded
                               };
                if (search.keyword is not null and not "")
                {
                    listItem = listItem.Where(r => r.code.Contains(search.keyword) || r.name.Contains(search.keyword));
                }
                if (search.type is not null)
                {
                    listItem = listItem.Where(r => r.type == search.type);
                }
                 
                if (search.page_number > 0)
                {
                    response.totalcount = listItem.Select(x => x.id).Count();
                    response.page = search.page_number;
                    response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / search.page_size);
                    response.lists = await listItem.OrderByDescending(r => r.id).Skip(search.page_size * (search.page_number - 1)).Take(search.page_size).ToListAsync();
                }
                else
                {
                    response.lists = await listItem.OrderByDescending(r => r.id).ToListAsync();
                }
                return response;
            });
        }

        public async Task<PromotionModel> PromotionModify(PromotionModel model)
        {
            return await Task.Run(async () =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    string response = "0";
                    try
                    {
                        var promotion = await _context.Promotion.FirstOrDefaultAsync(r => r.id == model.id);
                        if (promotion is not null)
                        {
                            promotion.status = 0;
                            promotion.name = model.name;
                            promotion.note = model.note;
                            promotion.date_start = model.date_start;
                            promotion.date_end = model.date_end;
                            promotion.userUpdated = model.userUpdated;
                            promotion.dateUpdated = DateTime.Now; 
                            _context.Promotion.Update(promotion);
                            _context.SaveChanges();
                        } 
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
        public async Task<bool> PromotionApprove(long id, long userid)
        {
            return await Task.Run(async () =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                { 
                    try
                    {
                        var promotion = await _context.Promotion.FirstOrDefaultAsync(r => r.id == id);
                        if (promotion is not null)
                        {
                            promotion.status = 1;
                            promotion.userUpdated = userid;
                            promotion.dateUpdated = DateTime.Now;
                            _context.Promotion.Update(promotion);
                            _context.SaveChanges();
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback(); 
                        return false;
                    }
                }
            });
        }
        
    }
}
