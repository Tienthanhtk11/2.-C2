using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Report;

namespace Point_of_Sale.Repositories
{
    internal class ReportRepository : IReportRepository
    {
        private readonly ApplicationContext _context;

        public ReportRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<RevenueProductModel> RevenueProducts(RevenueProductSearchModel search)
        {
            await Task.CompletedTask;
            RevenueProductModel response = new();

            IQueryable<RevenueProducts> listItem = from a in _context.Product_Warehouse
                                                   join b in _context.Product on a.product_id equals b.id
                                                   join c in _context.Category_Product on b.category_code equals c.code into ab
                                                   from ba in ab.DefaultIfEmpty()
                                                   where !a.is_delete
                                                   select new RevenueProducts
                                                   {
                                                       product_warehouse_id = a.id,
                                                       product_id = a.product_id,
                                                       name = b.name,
                                                       code = a.barcode,
                                                       unit_code = a.unit_code,
                                                       category_code = ba.code,
                                                       category_name = ba.name,
                                                       price = a.price,
                                                       warehouse_id = a.warehouse_id,
                                                   }

                                                   ;
            if (!string.IsNullOrEmpty(search.keyword))
            {
                listItem = listItem.Where(r => r.name.Contains(search.keyword)
                || r.code.Contains(search.keyword)
                || r.name.ToLower().Contains(search.keyword.ToLower())
                );
            }
            if (search.category_code is not null and not "")
            {
                listItem = listItem.Where(r => r.category_code == search.category_code);
            }
            if (search.warehouse_id != 0)
            {
                listItem = listItem.Where(r => r.warehouse_id == search.warehouse_id);
            }

            response.products = await listItem.OrderBy(r => r.name).ToListAsync();
            var listIdWarehouse = response.products.Select(e => e.warehouse_id).ToList();

            IQueryable<Order_Detail> revenue = _context.Order_Detail
                    .Where(e => listIdWarehouse.Contains(e.warehouse_id));
            if (search.start_date != null)
            {
                revenue = revenue.Where(r => r.dateAdded >= search.start_date);
            }
            if (search.end_date != null)
            {
                revenue = revenue.Where(r => search.end_date.Value.AddDays(1) >= r.dateAdded);
            }
            var dataRevenue = await revenue.ToListAsync();

            foreach (var item in response.products)
            {
                var data = dataRevenue.Where(e => e.product_warehouse_id == item.product_warehouse_id).ToList();
                if (data == null || data.Count == 0)
                {
                    continue;
                }
                var sale = data.Where(x => x.sale_price > 0);
                item.sale_price = sale.Count() > 0 ? sale.FirstOrDefault()?.sale_price ?? 0 : 0;
                item.quantity = data.Sum(e => e.quantity);
                item.amount = data.Sum(e => e.quantity * e.price);
                item.sale_amount = sale.Sum(e => (e.price - e.sale_price) * e.quantity);
                item.revenue = item.amount - item.sale_amount;
            }
            if (response.products.Count > 0 && string.IsNullOrEmpty(search.keyword)
                && string.IsNullOrEmpty(search.category_code))
            {
                IQueryable<Order> calc_voucher = _context.Order
                .Where(x => x.warehouse_id == search.warehouse_id);

                if (search.start_date != null)
                {
                    calc_voucher = calc_voucher.Where(r => r.dateAdded >= search.start_date);
                }
                if (search.end_date != null)
                {
                    calc_voucher = calc_voucher.Where(r => search.end_date.Value.AddDays(1) >= r.dateAdded);
                }

                response.total_voucher = (await calc_voucher.Select(e => e.voucher_cost).ToListAsync()).Sum();
            }
            response.total_amount = response.products.Sum(x => x.amount);
            response.total_sale = response.products.Sum(x => x.sale_amount);
            response.total_revenue = response.products.Sum(x => x.revenue);
            return response;
        }

        public async Task<PaginationSet<InOutInventoryProductModel>> InOutInventoryProducts(InOutInventorySearchModel search)
        {
            return await Task.Run(async () =>
            {
                PaginationSet<InOutInventoryProductModel> response = new PaginationSet<InOutInventoryProductModel>();
                IEnumerable<InOutInventoryProductModel> listItem = from a in _context.Product_Warehouse
                                                                   join c in _context.Product on a.product_id equals c.id
                                                                   join d in _context.Warehouse on a.warehouse_id equals d.id
                                                                   join b in _context.Category_Product on c.category_code equals b.code into ab
                                                                   from ba in ab.DefaultIfEmpty()
                                                                   where !a.is_delete
                                                                   select new InOutInventoryProductModel
                                                                   {
                                                                       category_code = c.category_code,
                                                                       name = c.name,
                                                                       barcode = a.barcode,
                                                                       category_name = ba.name,
                                                                       product_id = a.product_id,
                                                                       product_warehouse_id = a.id,
                                                                       warehouse_id = a.warehouse_id,
                                                                       warehouse_name = d.name,
                                                                       unit_code = a.unit_code,
                                                                       quantity_inventory = a.quantity_stock,
                                                                       quantity_sold = a.quantity_sold
                                                                   };
                if (!string.IsNullOrEmpty(search.keyword))
                {
                    listItem = listItem.Where(r => r.name.Contains(search.keyword)
                    || r.barcode.Contains(search.keyword)
                    || r.name.ToLower().Contains(search.keyword.ToLower())
                    );
                }
                if (search.warehouse_id is not 0)
                {
                    listItem = listItem.Where(r => r.warehouse_id == search.warehouse_id);
                }
                if (!string.IsNullOrEmpty(search.category_code))
                {
                    listItem = listItem.Where(r => r.category_code == search.category_code);
                }

                if (search.page_number > 0)
                {
                    response.totalcount = listItem.Select(x => x.product_warehouse_id).Count();
                    response.page = search.page_number;
                    response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / search.page_size);
                    response.lists = listItem.OrderByDescending(r => r.product_warehouse_id).Skip(search.page_size * (search.page_number - 1)).Take(search.page_size).ToList();
                }
                else
                {
                    response.lists = listItem.OrderByDescending(r => r.product_warehouse_id).ToList();
                }
                foreach (var item in response.lists)
                {
                    item.quantity_in = await _context.Warehouse_Receipt_Product.Where(e =>
                    e.product_id == item.product_id && !e.is_delete && e.warehouse_id == search.warehouse_id).SumAsync(x => x.quantity);

                    item.quantity_out = await (from x in _context.Warehouse_Export_Product
                                               join y in _context.Warehouse_Export on x.export_id equals y.id
                                               where !x.is_delete & x.warehouse_id == search.warehouse_id & x.product_id == item.product_id
                                               & y.type != 0
                                               select x.quantity).SumAsync();
                }
                return response;
            });
        }

        public async Task<PaginationSet<HistoryInventoryProductModel>> HistoryInventoryProducts(HistoryInventorySearchModel search)
        {
            return await Task.Run(async () =>
            {
                PaginationSet<HistoryInventoryProductModel> response = new PaginationSet<HistoryInventoryProductModel>();
                IEnumerable<HistoryInventoryProductModel> listItem = from a in _context.Product_Warehouse_History
                                                                     join b in _context.Warehouse on a.warehouse_id equals b.id
                                                                     where !a.is_delete & a.product_id == search.product_id
                                                                     select new HistoryInventoryProductModel
                                                                     {
                                                                         code = a.code,
                                                                         product_id = a.product_id,
                                                                         product_warehouse_id = a.id,
                                                                         unit_code = a.unit_code,
                                                                         barcode = a.barcode,
                                                                         batch_number = a.batch_number,
                                                                         dateAdded = a.dateAdded,
                                                                         id = a.id,
                                                                         id_table = a.id_table,
                                                                         import_price = a.import_price,
                                                                         price = a.price,
                                                                         packing_code = a.packing_code,
                                                                         sale_price = a.sale_price,
                                                                         quantity_in_stock = a.quantity_in_stock,
                                                                         userAdded = a.userAdded,
                                                                         quantity = a.quantity,
                                                                         warehouse_name = b.name,
                                                                         type = a.type,
                                                                         exp_date = a.exp_date,
                                                                         warehouse_id = a.warehouse_id
                                                                     };
                if (!string.IsNullOrEmpty(search.keyword))
                {
                    listItem = listItem.Where(r =>
                     r.code.Contains(search.keyword)
                    );
                }
                if (search.warehouse_id != 0)
                {
                    listItem = listItem.Where(r =>
                     r.warehouse_id == search.warehouse_id
                    );
                }
                if (search.page_number > 0)
                {
                    response.totalcount = listItem.Select(x => x.product_warehouse_id).Count();
                    response.page = search.page_number;
                    response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / search.page_size);
                    response.lists = listItem.OrderByDescending(r => r.product_warehouse_id).Skip(search.page_size * (search.page_number - 1)).Take(search.page_size).ToList();
                }
                else
                {
                    response.lists = listItem.OrderByDescending(r => r.product_warehouse_id).ToList();
                }
                return response;
            });
        }

        public async Task<List<Import_Export_Product_Model>> Daily_Import_Product_Report(long warehouse_id, int type, DateTime start_date, DateTime end_date)
        {

            return await Task.Run(async () =>
            {
                List<Import_Export_Product_Model> response = new();
                IEnumerable<Import_Export_Product_Model> listItem = from a in _context.Warehouse_Receipt_Product
                                                                    join b in _context.Warehouse_Receipt on a.receipt_id equals b.id
                                                                    join d in _context.Warehouse on b.warehouse_id equals d.id
                                                                    join c in _context.Partner on b.partner_id equals c.id
                                                                    where !b.is_delete && !a.is_delete && b.transfer_date >= start_date && end_date.AddDays(1) >= b.transfer_date && b.status_id == 1
                                                                    select new Import_Export_Product_Model
                                                                    {
                                                                        warehouse_id = b.warehouse_id,
                                                                        parent_code = b.code,
                                                                        type = b.type,
                                                                        date = b.transfer_date.ToString("dd/MM/yyyy"),
                                                                        partner_code = c.code,
                                                                        warehouse_name = d.name,
                                                                        warehouse_code = d.code,
                                                                        partner_name = c.name,
                                                                        product_id = a.product_id,
                                                                        product_unit = a.category_unit_code,
                                                                        quantity = Math.Round(a.quantity, 4),
                                                                        price = a.import_price,
                                                                        total_cost = Math.Round(a.quantity * a.import_price, 4)
                                                                    };
                response = listItem.ToList();
                if (warehouse_id != 0)
                {
                    response = response.Where(x => x.warehouse_id == warehouse_id).ToList();
                }
                if (type == 1)
                {
                    response = response.Where(x => x.type == 1 || x.type == 0).ToList();
                }
                else
                    response = response.Where(x => x.type == type).ToList();
                if (response.Count() > 0)
                {
                    List<long> product_ids = response.Select(x => x.product_id).ToList();
                    List<Product> products = _context.Product.Where(x => !x.is_delete && product_ids.Contains(x.id)).ToList();
                    List<Product_Warehouse> products_warehouse = _context.Product_Warehouse.Where(x => !x.is_delete && product_ids.Contains(x.product_id)).ToList();

                    foreach (var item in response)
                    {
                        Product product = products.FirstOrDefault(x => x.id == item.product_id);
                        if (product != null)
                        {
                            item.product_name = product.name;
                        }
                        Product_Warehouse product_Warehouse = products_warehouse.FirstOrDefault(x => x.product_id == item.product_id);
                        if (product_Warehouse != null)
                        {
                            item.product_code = product_Warehouse.barcode;
                        }
                    }
                }
                return response;
            });
        }
        public async Task<List<Import_Export_Product_Model>> Daily_Export_Product_Report(long warehouse_id, int type, DateTime start_date, DateTime end_date)
        {

            return await Task.Run(async () =>
            {
                List<Import_Export_Product_Model> response = new();
                IEnumerable<Import_Export_Product_Model> listItem = from a in _context.Warehouse_Export_Product
                                                                    join b in _context.Warehouse_Export on a.export_id equals b.id
                                                                    join d in _context.Warehouse on b.warehouse_id equals d.id
                                                                    where !b.is_delete && !a.is_delete && b.export_date >= start_date && end_date.AddDays(1) >= b.export_date && b.status_id != 0 && b.type == type
                                                                    select new Import_Export_Product_Model
                                                                    {
                                                                        warehouse_id = b.warehouse_id,
                                                                        warehouse_destination_id = b.warehouse_destination_id,
                                                                        type = b.type,
                                                                        order_id = b.order_id,
                                                                        parent_code = b.code,
                                                                        date = b.export_date.ToString("dd/MM/yyyy"),
                                                                        export_date = b.export_date,
                                                                        customer_id = b.customer_id,
                                                                        partner_id = b.partner_id,
                                                                        product_unit = a.unit_code,
                                                                        quantity = Math.Round(a.quantity, 4),
                                                                        warehouse_name = d.name,
                                                                        warehouse_code = d.code,
                                                                        product_id = a.product_id,
                                                                        price = a.price,
                                                                        total_cost = Math.Round(a.quantity * a.price, 4)
                                                                    };

                response = listItem.OrderBy(x => x.export_date).ToList();
                List<long?> customer_ids = response.Where(x => x.customer_id != null).Select(x => x.customer_id).Distinct().ToList();
                List<long?> partner_ids = response.Where(x => x.partner_id != null).Select(x => x.partner_id).Distinct().ToList();
                List<Customer> customers = new();
                List<Partner> partners = new();
                if (partner_ids.Count() > 0)
                {
                    partners = await _context.Partner.Where(x => partner_ids.Contains(x.id)).ToListAsync();
                }
                if (customer_ids.Count() > 0)
                {
                    customers = await _context.Customer.Where(x => customer_ids.Contains(x.id)).ToListAsync();
                }
                if (warehouse_id != 0)
                {
                    response = response.Where(x => x.warehouse_id == warehouse_id).ToList();
                }
                List<long> product_ids = response.Select(x => x.product_id).Distinct().ToList();
                List<long?> warehouse_ids = response.Where(x => x.warehouse_destination_id != null).Select(x => x.warehouse_destination_id).Distinct().ToList();
                List<Warehouse> list_warehouse = _context.Warehouse.Where(x => warehouse_ids.Contains(x.id)).ToList();
                List<Product> products = _context.Product.Where(x => !x.is_delete && product_ids.Contains(x.id)).ToList();
                List<Product_Warehouse> products_warehouse = _context.Product_Warehouse.Where(x => !x.is_delete && product_ids.Contains(x.product_id)).ToList();
                foreach (var item in response)
                {
                    if (item.customer_id != null)
                    {
                        Customer customer = customers.FirstOrDefault(x => x.id == item.customer_id);
                        if (customer != null)
                        {
                            item.partner_name = customer.name;
                            item.partner_code = customer.code;
                        }
                    }
                    if (item.partner_id != 0)
                    {
                        Partner partner = partners.FirstOrDefault(x => x.id == item.partner_id);
                        if (partner != null)
                        {
                            item.partner_name = partner.name;
                            item.partner_code = partner.code;
                        }
                    }
                    if (item.warehouse_destination_id != null)
                    {
                        Warehouse warehouse = list_warehouse.FirstOrDefault(x => x.id == item.warehouse_destination_id);
                        if (warehouse != null)
                        {
                            item.warehouse_destination_name = warehouse.name;
                        }
                    }
                    Product product = products.FirstOrDefault(x => x.id == item.product_id);
                    if (product != null)
                    {
                        item.product_name = product.name;
                    }
                    Product_Warehouse product_Warehouse = products_warehouse.FirstOrDefault(x => x.product_id == item.product_id);
                    if (product_Warehouse != null)
                    {
                        item.product_code = product_Warehouse.barcode;
                    }
                }
                return response;
            });
        }

        public async Task<Revenue_Book_Report> Daily_Order_List(long warehouse_id, DateTime start_date, DateTime end_date)
        {
            Revenue_Book_Report revenue_Book_Report = new();
            IEnumerable<Revenue_Book_Report_Detail> listItem = from a in _context.Order
                                                               join b in _context.Warehouse on a.warehouse_id equals b.id
                                                               join c in _context.Customer on a.customer_id equals c.id
                                                               where !a.is_delete && a.dateAdded >= start_date && end_date.AddDays(1) >= a.dateAdded
                                                               select new Revenue_Book_Report_Detail
                                                               {
                                                                   id = a.id,
                                                                   code = a.code,
                                                                   date = a.dateAdded,
                                                                   customer_id = a.customer_id,
                                                                   sale_cost = a.sale_cost,
                                                                   product_total_cost = a.product_total_cost,
                                                                   voucher_cost = a.voucher_cost,
                                                                   warehouse_id = a.warehouse_id,
                                                                   warehouse_name = b.name,
                                                                   warehouse_code = b.code,
                                                                   customer_code = c.code,
                                                                   customer_name = c.name,
                                                                   member_point_value = a.member_point_value,
                                                                   member_point = a.member_point,
                                                                   payment_type = a.payment_type,
                                                                   total_amount = a.total_amount
                                                               };
            if (warehouse_id != 0)
            {
                listItem = listItem.Where(x => x.warehouse_id == warehouse_id);
            }
            List<Revenue_Book_Report_Detail> details = listItem.OrderByDescending(r => r.id).ToList();
            revenue_Book_Report.details = details;
            revenue_Book_Report.opening_cash = 1000000;
            revenue_Book_Report.session_cash = details.Select(x => x.total_amount).Sum();
            revenue_Book_Report.closing_cash = revenue_Book_Report.opening_cash + revenue_Book_Report.session_cash;
            return revenue_Book_Report;
        }
        public async Task<List<Sale_Session_Report>> Sales_SessionList(long warehouse_id, DateTime start_date, DateTime end_date)
        {
            return await Task.Run(() =>
            {
                List<Sale_Session_Report> response = new();
                IEnumerable<Sale_Session_Report> listItem = from a in _context.Sales_Session
                                                            join b in _context.Admin_User on a.staff_id equals b.id
                                                            join c in _context.Warehouse on a.warehouse_id equals c.id
                                                            where !a.is_delete && a.start_time >= start_date && end_date.AddDays(1) >= a.end_time && a.status == 1
                                                            select new Sale_Session_Report
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
                                                                session_cash = (double)_context.Order.Where(x => x.sales_session_id == a.id && x.payment_type == 0).Select(x => x.total_amount).Sum(),
                                                                member_point = _context.Order.Where(x => x.sales_session_id == a.id).Select(x => x.member_point).Sum(),
                                                                member_point_value = (double)_context.Order.Where(x => x.sales_session_id == a.id).Select(x => x.member_point_value).Sum(),
                                                                closing_card = (double)_context.Order.Where(x => x.sales_session_id == a.id && x.payment_type == 1).Select(x => x.total_amount).Sum(),
                                                                closing_online_transfer = (double)_context.Order.Where(x => x.sales_session_id == a.id && x.payment_type == 2).Select(x => x.total_amount).Sum(),
                                                                staff_name = b.full_name,
                                                                warehouse_name = c.name,
                                                                closing_total_proceeds = (double)_context.Order.Where(x => x.sales_session_id == a.id).Select(x => x.total_amount).Sum(),
                                                                userUpdated = a.userUpdated,
                                                            };


                if (warehouse_id != null && warehouse_id != 0)
                {
                    listItem = listItem.Where(r => r.warehouse_id == warehouse_id);
                }

                response = listItem.OrderByDescending(r => r.id).ToList();

                return Task.FromResult(response);
            });

        }

        public async Task<List<Customer_Revenue>> Customer_Revenues(DateTime start_date, DateTime end_date)
        {
            return await Task.Run(async () =>
            {
                List<Customer_Revenue> response = new();
                IEnumerable<Customer_Revenue> listItem = from a in _context.Customer
                                                         where !a.is_delete
                                                         select new Customer_Revenue
                                                         {
                                                             customer_id = a.id,
                                                             customer_name = a.name,
                                                             customer_revenue = (double)_context.Order.Where(x => x.customer_id == a.id && x.dateAdded >= start_date && end_date >= x.dateAdded).Select(x => x.total_amount).Sum()
                                                         };
                response = listItem.Where(x => x.customer_revenue > 0).OrderByDescending(x => x.customer_revenue).ToList();
                return response;
            });
        }
        public async Task<List<Category_Revenue>> Category_Revenues(DateTime start_date, DateTime end_date)
        {
            return await Task.Run(async () =>
            {
                List<Category_Revenue> response = new();
                IEnumerable<Category_Revenue> listItem = from a in _context.Category_Product
                                                         where !a.is_delete
                                                         select new Category_Revenue
                                                         {
                                                             category_id = a.id,
                                                             category_name = a.name,
                                                             category_code = a.code
                                                         };
                response = listItem.ToList();
                IEnumerable<Product_Export_Model> listProductExport = from a in _context.Product
                                                                      join b in _context.Warehouse_Export_Product on a.id equals b.product_id
                                                                      join c in _context.Warehouse_Export on b.export_id equals c.id
                                                                      where !a.is_delete && c.export_date >= start_date && end_date >= c.export_date && c.status_id != 0
                                                                      select new Product_Export_Model
                                                                      {
                                                                          category_id = a.id,
                                                                          name = a.name,
                                                                          id = a.id,
                                                                          export_price = b.price,
                                                                          export_quantity = b.quantity,
                                                                          category_code = a.category_code
                                                                      };
                List<Product_Export_Model> ListProductExport = listProductExport.ToList();
                IEnumerable<Product_Import_Model> listProductImport = from a in _context.Product
                                                                      join b in _context.Warehouse_Receipt_Product on a.id equals b.product_id
                                                                      join c in _context.Warehouse_Receipt on b.receipt_id equals c.id
                                                                      where !a.is_delete && c.transfer_date >= start_date && end_date >= c.transfer_date && c.status_id == 1
                                                                      select new Product_Import_Model
                                                                      {
                                                                          category_id = a.id,
                                                                          name = a.name,
                                                                          id = a.id,
                                                                          import_price = b.import_price,
                                                                          category_code = a.category_code,
                                                                          import_quantity = b.quantity
                                                                      };
                List<Product_Import_Model> ListProductImport = listProductImport.ToList();

                foreach (var item in response)
                {
                    item.category_revenue = ListProductExport.Where(x => (x.category_id == item.category_id) || (x.category_code == item.category_code)).Sum(x => x.export_quantity * x.export_price);
                    item.category_cost_price = ListProductImport.Where(x => (x.category_id == item.category_id) || (x.category_code == item.category_code)).Sum(x => x.import_price * x.import_quantity);
                    item.category_profit = item.category_revenue - item.category_cost_price;
                }
                response = response.OrderByDescending(x => x.category_profit).ToList();
                return response;
            });
        }
        public async Task<List<Export_Product_Fast_Model>> Product_Export_Fast(long warehouse_id, DateTime start_date, DateTime end_date)
        {

            return await Task.Run(async () =>
            {
                List<Export_Product_Fast_Model> response = new();
                IEnumerable<Export_Product_Fast_Model> listItem = from a in _context.Warehouse_Export_Product
                                                                  join b in _context.Warehouse_Export on a.export_id equals b.id
                                                                  join d in _context.Warehouse on b.warehouse_id equals d.id
                                                                  join c in _context.Partner on b.partner_id equals c.id
                                                                  join e in _context.Admin_User on b.userUpdated equals e.id
                                                                  where !b.is_delete && !a.is_delete && b.export_date >= start_date && end_date.AddDays(1) >= b.export_date && b.status_id == 1
                                                                  select new Export_Product_Fast_Model
                                                                  {
                                                                      warehouse_id = b.warehouse_id,
                                                                      parent_code = b.code,
                                                                      receipt_name = e.full_name,
                                                                      date = b.export_date.ToString("dd/MM/yyyy"),
                                                                      partner_code = c.code,
                                                                      warehouse_name = d.name,
                                                                      warehouse_code = d.code,
                                                                      partner_name = c.name,
                                                                      note = b.content,
                                                                      product_id = a.product_id,
                                                                      product_unit = a.unit_code,
                                                                      quantity = Math.Round(a.quantity, 4),
                                                                      price = a.import_price,
                                                                      total_cost = Math.Round(a.quantity * a.import_price, 4)
                                                                  };
                response = listItem.ToList();
                if (warehouse_id != 0)
                {
                    response = response.Where(x => x.warehouse_id == warehouse_id).ToList();
                }
                if (response.Count() > 0)
                {
                    List<long> product_ids = response.Select(x => x.product_id).ToList();
                    List<Product> products = _context.Product.Where(x => !x.is_delete && product_ids.Contains(x.id)).ToList();
                    List<Product_Warehouse> products_warehouse = _context.Product_Warehouse.Where(x => !x.is_delete && product_ids.Contains(x.product_id)).ToList();

                    foreach (var item in response)
                    {
                        Product product = products.FirstOrDefault(x => x.id == item.product_id);
                        if (product != null)
                        {
                            item.product_name = product.name;
                        }
                        Product_Warehouse product_Warehouse = products_warehouse.FirstOrDefault(x => x.product_id == item.product_id);
                        if (product_Warehouse != null)
                        {
                            item.product_code = product_Warehouse.barcode;
                        }
                    }
                }

                return response;
            });
        }
        public async Task<List<Export_Product_Fast_Model>> Product_Import_Fast(long warehouse_id, DateTime start_date, DateTime end_date)
        {

            return await Task.Run(async () =>
            {
                List<Export_Product_Fast_Model> response = new();
                IEnumerable<Export_Product_Fast_Model> listItem = from a in _context.Warehouse_Receipt_Product
                                                                  join b in _context.Warehouse_Receipt on a.receipt_id equals b.id
                                                                  join d in _context.Warehouse on b.warehouse_id equals d.id
                                                                  join c in _context.Partner on b.partner_id equals c.id
                                                                  join e in _context.Admin_User on b.userUpdated equals e.id
                                                                  where !b.is_delete && !a.is_delete && b.transfer_date >= start_date && end_date.AddDays(1) >= b.transfer_date && b.status_id == 1
                                                                  select new Export_Product_Fast_Model
                                                                  {
                                                                      warehouse_id = b.warehouse_id,
                                                                      parent_code = b.code,
                                                                      receipt_name = e.full_name,
                                                                      date = b.transfer_date.ToString("dd/MM/yyyy"),
                                                                      partner_code = c.code,
                                                                      partner_name = c.name,
                                                                      warehouse_name = d.name,
                                                                      warehouse_code = d.code,
                                                                      note = b.content,
                                                                      product_id = a.product_id,
                                                                      product_unit = a.category_unit_code,
                                                                      quantity = Math.Round(a.quantity, 4),
                                                                      price = a.import_price,
                                                                      total_cost = Math.Round(a.quantity * a.import_price, 4)
                                                                  };
                response = listItem.ToList();
                if (warehouse_id != 0)
                {
                    response = response.Where(x => x.warehouse_id == warehouse_id).ToList();
                }
                if (response.Count() > 0)
                {
                    List<long> product_ids = response.Select(x => x.product_id).ToList();
                    List<Product> products = _context.Product.Where(x => !x.is_delete && product_ids.Contains(x.id)).ToList();
                    List<Product_Warehouse> products_warehouse = _context.Product_Warehouse.Where(x => !x.is_delete && product_ids.Contains(x.product_id)).ToList();

                    foreach (var item in response)
                    {
                        Product product = products.FirstOrDefault(x => x.id == item.product_id);
                        if (product != null)
                        {
                            item.product_name = product.name;
                        }
                        Product_Warehouse product_Warehouse = products_warehouse.FirstOrDefault(x => x.product_id == item.product_id);
                        if (product_Warehouse != null)
                        {
                            item.product_code = product_Warehouse.barcode;
                        }
                    }
                }
                return response;
            });
        }
        public async Task<List<Receipt_Cash_Form>> Receipt_Cash_Form_List(long warehouse_id, DateTime start_date, DateTime end_date)
        {

            IEnumerable<Receipt_Cash_Form> listItem = from a in _context.Order
                                                      join b in _context.Warehouse on a.warehouse_id equals b.id
                                                      join c in _context.Customer on a.customer_id equals c.id
                                                      join d in _context.Admin_User on a.userAdded equals d.id
                                                      where !a.is_delete && a.dateAdded >= start_date && end_date.AddDays(1) >= a.dateAdded
                                                      select new Receipt_Cash_Form
                                                      {
                                                          warehouse_id = a.warehouse_id,
                                                          id = a.id,
                                                          ma_gd = a.code,
                                                          ma_kh = c.code,
                                                          ong_ba = d.full_name,
                                                          ngay_ct = a.dateAdded,
                                                          so_ct = a.code,
                                                          tien_tt = a.total_amount,
                                                          dien_giaii = a.note,
                                                          ma_vv_i = b.code,
                                                      };
            if (warehouse_id != 0)
            {
                listItem = listItem.Where(x => x.warehouse_id == warehouse_id);
            }
            List<Receipt_Cash_Form> response = listItem.OrderByDescending(r => r.id).ToList();
            if (response.Count() > 0)
            {
                int index = 1;
                foreach (var item in response)
                {
                    item.ma_qs = index + "/" + item.ngay_ct.Month.ToString();
                    index++;
                }
            }
            return response;
        }

        public async Task<List<Stock_Product>> Warehouse_Product_Fast(DateTime dateTime)
        {
            return await Task.Run(() =>
            {
                List<Stock_Product> response = new();//3700 product_id vs sl tồn 
                IEnumerable<Stock_Product> listItem = from a in _context.Product_Warehouse
                                                      join b in _context.Product on a.product_id equals b.id
                                                      join c in _context.Warehouse on a.warehouse_id equals c.id
                                                      where !a.is_delete
                                                      select new Stock_Product
                                                      {
                                                          import_price = a.import_price,
                                                          warehouse_code = c.code,
                                                          product_barcode = b.barcode,
                                                          quantity = a.quantity_stock,
                                                          export_price = b.price,
                                                          product_warehouse_id = a.id,
                                                          datetime = DateTime.Now,
                                                          stock_cost = a.import_price * a.quantity_stock
                                                      };
                response = listItem.OrderByDescending(r => r.quantity).ToList();
                //  log sản phẩm theo ngày >> tồn
                //if (response.Count() > 0)
                //{
                //    foreach (var item in response)
                //    {
                //        var product_history = _context.Product_Warehouse_History.Where(x => x.product_warehouse_id == item.product_warehouse_id && dateTime >= x.dateAdded).OrderByDescending(x => x.id).FirstOrDefault();
                //        if (product_history != null)
                //        {
                //            item.quantity = product_history.quantity_in_stock;
                //            item.stock_cost = item.quantity * item.quantity;
                //        }
                //    }
                //}
                return response;
            });
        }
        public async Task<List<Stock_Product>> Warehouse_Product_Inventory_Fast(DateTime dateTime)
        {
            return await Task.Run(() =>
            {
                List<Stock_Product> response = new();
                IEnumerable<Stock_Product> listItem = from a in _context.Product_Warehouse
                                                      join b in _context.Product on a.product_id equals b.id
                                                      join c in _context.Warehouse on a.warehouse_id equals c.id
                                                      where !a.is_delete
                                                      select new Stock_Product
                                                      {
                                                          import_price = a.import_price,
                                                          warehouse_code = c.code,
                                                          product_barcode = b.barcode,
                                                          quantity = _context.Product_Warehouse_History.Where(x => x.product_warehouse_id == a.id && dateTime >= x.dateAdded).OrderByDescending(x => x.id).Select(x => x.quantity_in_stock).FirstOrDefault(),
                                                          product_category_code = b.category_code,
                                                          product_unit_code = a.unit_code,
                                                          export_price = b.price,
                                                          product_warehouse_id = a.id,
                                                          datetime = DateTime.Now,
                                                          stock_cost = a.import_price * a.quantity_stock
                                                      };
                response = listItem.Where(x => x.quantity > 0).OrderByDescending(r => r.quantity).ToList();
                var category_list = _context.Category_Product.Where(x => !x.is_delete).ToList();

                if (response.Count() > 0)
                {
                    foreach (var item in response)
                    {
                        //var product_history = /*_context.Product_Warehouse_History.Where(x => x.product_warehouse_id == item.product_warehouse_id && dateTime >= x.dateAdded).OrderByDescending(x => x.id).FirstOrDefault()*/;
                        //if (product_history != null)
                        //{
                        //item.quantity = product_history.quantity_in_stock;
                        item.stock_cost = item.quantity * item.quantity;
                        //    }
                        //    var category = category_list.FirstOrDefault(x=>x.code == item.product_category_code || x.code == item.product_category_code);
                        //    if (category!=null)
                        //    {
                        //        item.product_category_code = category.code; 
                        //    }
                    }
                }
                return response;
            });
        }

    }
}
