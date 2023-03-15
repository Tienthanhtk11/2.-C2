using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model.Dasboad;

namespace Point_of_Sale.Repositories
{
    internal class DasboardRepository: IDasboardRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public DasboardRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PosInfoModel> DasboardInfo(long warehouse_id)
        {
            return await Task.Run(async () =>
            {
                PosInfoModel posInfo = new PosInfoModel();
                try
                {
                    var order = await _context.Order.Where(e => e.warehouse_id == warehouse_id && !e.is_delete).Select(x => new { 
                        x.id ,
                        x.customer_id,
                        x.total_amount 
                    }).ToListAsync();
                    posInfo.total_order = order.Count;
                    posInfo.total_customer = order.GroupBy(x =>x.customer_id).Count();
                    posInfo.total_revenue = order.Sum(x => x.total_amount);
                    posInfo.total_partner = await _context.Partner.Where(x => !x.is_delete).Select(e => e.id).CountAsync();
                    return posInfo;
                }
                catch (Exception)
                { 
                    return posInfo;
                } 
            });
        }

        public async Task<RevenueModel> DasboardRevenue(ChartSearch search)
        {
            return await Task.Run(async () =>
            {
                RevenueModel data = new RevenueModel();
              
                    data.label = "Doanh thu";
                    data.data = new List<ChartDataModel>();
                    foreach (var item in search.GetChartBarSearches())
                    { 
                        data.data.Add(new ChartDataModel
                        {
                            name = item.label, 
                            total = await _context.Order.Where(e => e.dateAdded >= item.date_from && e.dateAdded <= item.date_to && !e.is_delete)
                            .Select(x =>x.total_amount).SumAsync()
                        });
                    } 
             
                return data;
            });
        }

        public async Task<List<TopProductModel>> DasboardTopProduct(ChartSearch search)
        {
            return await Task.Run(async () =>
            {
                List<TopProductModel> data = new List<TopProductModel>();
                try
                {
                    var dateModel = search.GetDataPieSearch();

                    var queryData = await (from a in _context.Order_Detail
                                 join b in _context.Product on a.product_id equals b.id
                                 where !a.is_delete & a.dateAdded >= dateModel.date_from & a.dateAdded <= dateModel.date_to
                                           select new
                                 {
                                     a.quantity,
                                     b.name, 
                                 }).ToListAsync();
                      
                    foreach (var item in queryData.GroupBy(x => x.name).OrderByDescending(e => e.Sum(z => z.quantity)).Take(5))
                    {
                        data.Add(new TopProductModel
                        {
                            name = item.Key,
                            total = item.Sum(a =>a.quantity)
                        });
                    }
                    return data;
                }
                catch (Exception)
                {
                    return data;
                }
            });
        }
    }
}
