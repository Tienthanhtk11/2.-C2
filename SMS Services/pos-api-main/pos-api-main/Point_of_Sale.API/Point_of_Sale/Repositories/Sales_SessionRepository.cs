using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.SaleSession;

namespace Point_of_Sale.Repositories
{
    internal class Sales_SessionRepository : ISales_SessionRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public Sales_SessionRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<long> Sales_SessionCreate(Sales_Session model)
        {
            return await Task.Run(async () =>
            {
                var sessionOld = await _context.Sales_Session.FirstOrDefaultAsync(e => e.staff_id == model.staff_id && e.status != 1);
                if (sessionOld != null)
                {
                    return sessionOld.id;
                }


                try
                {
                    model.closing_cash = model.opening_cash;
                    model.dateAdded = DateTime.Now;
                    model.start_time = DateTime.Now;
                    _context.Sales_Session.Add(model);
                    _context.SaveChanges();
                    model.code = "POS" + model.id;
                    _context.Sales_Session.Update(model);
                    _context.SaveChanges();
                    return model.id;
                }
                catch (Exception)
                {
                    return 0;
                }

            });
        }
        public async Task<Sales_SessionModel> Sales_SessionDetail(long id, long id_staff)
        {
            return await Task.Run(async () =>
            {
                Sales_Session sales_Session = await _context.Sales_Session.FirstOrDefaultAsync(x => x.id == id);

                if (sales_Session?.status == 1)
                {
                    sales_Session = await _context.Sales_Session.FirstOrDefaultAsync(e => e.staff_id == id_staff && e.status != 1);
                }

                if (sales_Session == null)
                {
                    return new Sales_SessionModel();
                }
                Sales_SessionModel model = _mapper.Map<Sales_SessionModel>(sales_Session);
                if (model != null)
                {
                    model.closing_total_proceeds = (double)_context.Order.Where(x => x.sales_session_id == id).Select(x => x.total_amount).Sum();
                    model.closing_cash = (double)_context.Order.Where(x => x.sales_session_id == id && x.payment_type == 0).Select(x => x.total_amount).Sum() + model.opening_cash;
                    model.closing_card = (double)_context.Order.Where(x => x.sales_session_id == id && x.payment_type == 1).Select(x => x.total_amount).Sum();
                    model.closing_online_transfer = (double)_context.Order.Where(x => x.sales_session_id == id && x.payment_type == 2).Select(x => x.total_amount).Sum();
                    return model;
                }
                else
                    return new Sales_SessionModel();
            });
        }
        public async Task<long> Sales_SessionCurrentId(long id_staff)
        {
            return await Task.Run(async () =>
            {
                Sales_Session sales_Session = await _context.Sales_Session
                .OrderByDescending(a => a.dateAdded).FirstOrDefaultAsync(x => x.staff_id == id_staff && x.status == 0);
                return sales_Session == null ? 0 : sales_Session.id;
            });
        }

        public async Task<Sales_Session> Sales_SessionModify(Sales_Session model)
        {
            return await Task.Run(() =>
            {

                string response = "0";
                try
                {
                    Sales_Session Sales_Session = _context.Sales_Session.FirstOrDefault(r => r.id == model.id);
                    Sales_Session.status = model.status;
                    Sales_Session.end_time = DateTime.Now;
                    _context.Sales_Session.Update(Sales_Session);
                    _context.SaveChanges();
                    return model;
                }
                catch (Exception ex)
                {
                    response = ex.Message + " - " + ex.StackTrace;
                    return model;
                }

            });
        }
        public async Task<bool> Sales_SessionDelete(long id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Sales_Session Sales_Session = _context.Sales_Session.FirstOrDefault(r => r.id == id);
                    Sales_Session.is_delete = true;
                    Sales_Session.status = 1;
                    _context.Sales_Session.Update(Sales_Session);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
        public async Task<PaginationSet<Sales_SessionModel>> Sales_SessionList(SaleSessionSearch search)
        {
            return await Task.Run(() =>
            {
                PaginationSet<Sales_SessionModel> response = new PaginationSet<Sales_SessionModel>();
                IEnumerable<Sales_SessionModel> listItem = from a in _context.Sales_Session
                                                           join b in _context.Admin_User on a.staff_id equals b.id
                                                           join c in _context.Warehouse on a.warehouse_id equals c.id
                                                           select new Sales_SessionModel
                                                           {
                                                               start_time = a.start_time,
                                                               id = a.id,
                                                               code = a.code,
                                                               status = a.status,
                                                               end_time = a.end_time,
                                                               warehouse_id = a.warehouse_id,
                                                               note = a.note,
                                                               staff_id = a.staff_id,
                                                               opening_cash = a.opening_cash,
                                                               closing_cash = (double)_context.Order.Where(x => x.sales_session_id == a.id && x.payment_type == 0).Select(x => x.total_amount).Sum() + a.opening_cash,
                                                               closing_card = (double)_context.Order.Where(x => x.sales_session_id == a.id && x.payment_type == 1).Select(x => x.total_amount).Sum(),
                                                               closing_online_transfer = (double)_context.Order.Where(x => x.sales_session_id == a.id && x.payment_type == 2).Select(x => x.total_amount).Sum(),
                                                               staff_name = b.full_name,
                                                               warehouse_name = c.name,
                                                               closing_total_proceeds = (double)_context.Order.Where(x => x.sales_session_id == a.id).Select(x => x.total_amount).Sum(),
                                                               userUpdated = a.userUpdated,
                                                           };
                if (search.keyword is not null and not "") 
                {
                    listItem = listItem.Where(r => r.staff_name.Contains(search.keyword));
                }
                if (search.status != null)
                {
                    listItem = listItem.Where(r => r.status == search.status);
                } 
                if (search.warehouse_id != null && search.warehouse_id != 0)
                {
                    listItem = listItem.Where(r => r.warehouse_id == search.warehouse_id);
                }
                if (search.page_number > 0)
                {
                    response.totalcount = listItem.Select(x => x.id).Count();
                    response.page = search.page_number;
                    response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / search.page_size);
                    response.lists = listItem.OrderByDescending(r => r.id).Skip(search.page_size * (search.page_number - 1)).Take(search.page_size).ToList();
                }
                else
                {
                    response.lists = listItem.OrderByDescending(r => r.id).ToList();
                }
                return Task.FromResult(response);
            });
        }
    }
}
