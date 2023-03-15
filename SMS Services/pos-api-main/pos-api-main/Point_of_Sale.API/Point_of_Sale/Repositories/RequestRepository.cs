using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Request;
using Point_of_Sale.Model.Warehouse;

namespace Point_of_Sale.Repositories
{
    internal class RequestRepository : IRequestRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public RequestRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<RequestModel> Request(long id)
        {
            return await Task.Run(async () =>
            {
                Warehouse_Request request = await _context.Warehouse_Request.FirstOrDefaultAsync(r => r.id == id);
                var model = _mapper.Map<RequestModel>(request);
                var listProduct = await _context.Warehouse_Request_Product.Where(r => r.request_id == id && !r.is_delete).ToListAsync();
                model.Products = _mapper.Map<List<Request_ProductModel>>(listProduct);
                return model;
            });
        }
        public async Task<string> RequestCreate(RequestModel model)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    string respone = "0";
                    try
                    {
                        Warehouse_Request request = _mapper.Map<Warehouse_Request>(model);
                        request.dateAdded = DateTime.Now;
                        request.status_id = 0;
                        request.code = "WRQ-";
                        _context.Warehouse_Request.Add(request);
                        _context.SaveChanges();
                        request.code = request.code + request.id;
                        var listProduct = _mapper.Map<List<Warehouse_Request_Product>>(model.Products); 
                        foreach (var item in listProduct)
                        {
                            item.request_id = request.id;
                            item.dateAdded = request.dateAdded; 
                        }
                        _context.Warehouse_Request_Product.AddRange(listProduct);
                        _context.Warehouse_Request.Update(request);
                        _context.SaveChanges();
                        transaction.Commit();
                        return respone;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        respone = ex.Message + " - " + ex.StackTrace;
                        return respone;
                    }

                };
            });
        }

        public async Task<string> RequestModify(RequestModel model)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    string respone = "0";
                    try
                    { 
                        Warehouse_Request request = await _context.Warehouse_Request.FirstOrDefaultAsync(r => r.id == model.id);
                        if (request == null)
                        {
                            return respone;
                        }
                        request.dateUpdated = DateTime.Now; 
                        request.note = model.note;
                        request.content = model.content; 
                        request.warehouse_id = model.warehouse_id;
                        request.partner_id = model.partner_id;
                        request.status_id = 0;
                        request.userUpdated = model.userUpdated;
                        request.type = model.type;
                        _context.Warehouse_Request.Update(request);
                        var productdb = await _context.Warehouse_Request_Product.Where(x => x.request_id == model.id && !x.is_delete).AsNoTracking().ToListAsync();
                        productdb.ForEach(x => x.is_delete = true);
                        _context.Warehouse_Request_Product.UpdateRange(productdb);
                        var listProduct = _mapper.Map<List<Warehouse_Request_Product>>(model.Products); 
                        foreach (var item in listProduct)
                        {
                            item.id = 0;
                            item.request_id = request.id; 
                            item.dateUpdated = DateTime.Now;
                            item.userUpdated = model.userUpdated; 
                        }
                        _context.Warehouse_Request_Product.AddRange(listProduct); 
                        _context.SaveChanges();
                        transaction.Commit();
                        return respone;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        respone = ex.Message + " - " + ex.StackTrace;
                        return respone;
                    }

                };
            });
        }
        public async Task<string> RequestModifyStatus(long id,byte status)
        {
            Warehouse_Request request = await _context.Warehouse_Request.FirstOrDefaultAsync(r => r.id == id);
            request.status_id = status;
            _context.Update(request);
            _context.SaveChanges();
            return "0";
        }
        public async Task<PaginationSet<RequestViewModel>> RequestList(Warehouse_Request_Search search)
        {
            return await Task.Run(async () =>
            {
                PaginationSet<RequestViewModel> response = new PaginationSet<RequestViewModel>();
                IQueryable<RequestViewModel> listItem = from a in _context.Warehouse_Request
                                                        join b in _context.Warehouse on a.warehouse_id equals b.id into ab
                                                        from bb in ab.DefaultIfEmpty() 
                                                        where !a.is_delete
                                                        select new RequestViewModel
                                                        {
                                                            content = a.content,
                                                            delivery_address = a.delivery_address,
                                                            transfer_date = a.transfer_date,
                                                            code = a.code,
                                                            shipper = a.shipper, 
                                                            warehouse_name = bb.name, 
                                                            status_id = a.status_id,
                                                            type = a.type,
                                                            warehouse_id = a.warehouse_id,
                                                            note = a.note,
                                                            id = a.id,
                                                        };
                if (search.status_id is not null and not 10)
                {
                    listItem = listItem.Where(r => r.status_id == search.status_id);
                } 
                if (search.keyword is not null and not "")
                {
                    listItem = listItem.Where(r => r.content.Contains(search.keyword) || r.code.Contains(search.keyword));
                } 
                if (search.warehouse_id is not 0)
                {
                    listItem = listItem.Where(r => r.warehouse_id == search.warehouse_id);
                }
                if (search.start_date != null)
                {
                    DateTime start = search.start_date.Value.Date;
                    listItem = listItem.Where(r => r.transfer_date >= search.start_date);
                }
                if (search.end_date != null)
                {
                    var end = search.end_date.Value.Date.AddDays(1);
                    listItem = listItem.Where(r => search.end_date >= r.transfer_date);
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
                    response.lists = listItem.OrderByDescending(r => r.id).ToList();
                }
                return response;
            });
        }
    }
}
