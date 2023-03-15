using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml.Style;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Product;
using Point_of_Sale.Model.Refund;

namespace Point_of_Sale.Repositories
{
    internal class RefundRepository : IRefundRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public RefundRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<RefundModel> Refund(long id)
        {
            return await Task.Run(async () =>
            {
                var model = await (from a in _context.Refund
                                   join b in _context.Customer on a.customer_id equals b.id
                                   join c in _context.Admin_User on a.userAdded equals c.id
                                   where !a.is_delete & a.id == id
                                   select new RefundModel
                                   {
                                       customer_name = b.name,
                                       customer_id = b.id,
                                       code = a.code,
                                       voucher_cost = a.voucher_cost,
                                       product_sale_cost = a.product_sale_cost,
                                       member_point = a.member_point,
                                       id = a.id,
                                       customer_phone = b.phone,
                                       dateAdded = a.dateAdded,
                                       dateUpdated = a.dateUpdated,
                                       order_id = a.order_id,
                                       product_total_cost = a.product_total_cost,
                                       staff_name = c.full_name,
                                       warehouse_id = a.warehouse_id,
                                       userAdded = a.userAdded,
                                       total_refund_money = a.total_refund_money,
                                       userUpdated = a.userUpdated,
                                       note = a.note
                                   }).FirstOrDefaultAsync();
                if (model == null)
                {
                    return new RefundModel();
                }

                model.refund_order = await (from a in _context.Refund_Order
                                            where a.refund_id == id
                                            select new RefundOrderModel
                                            {
                                                refund_id = a.refund_id,
                                                id = a.id,
                                                order_id = a.order_id,
                                                order_code = a.order_code,
                                                total_refund_money = a.total_refund_money,
                                                dateAdded = a.dateAdded,
                                                products = (from x in _context.Refund_Order_Product
                                                            join y in _context.Product on x.product_id equals y.id
                                                            join z in _context.Product_Warehouse on x.product_warehouse_id equals z.id
                                                            where x.refund_id == id & x.refund_order_id == a.id
                                                            select new RefundProduct
                                                            {
                                                                id = x.id,
                                                                product_id = x.product_id,
                                                                name = y.name,
                                                                barcode = z.barcode,
                                                                dateAdded = x.dateAdded,
                                                                price = x.price,
                                                                quantity = x.quantity,
                                                                unit_code = x.unit_code,
                                                                product_batch_number = x.product_batch_number,
                                                                refund_id = x.refund_id,
                                                                packing_code = x.packing_code,
                                                                warehouse_id = x.warehouse_id,
                                                                product_warehouse_id = x.product_warehouse_id,
                                                                userAdded = x.userAdded,
                                                            }).ToList()
                                            }).ToListAsync();
                return model;
            });
        }
        public async Task<RefundModel> RefundCreate(RefundModel model)
        {
            return await Task.Run(async () =>
            {
                using IDbContextTransaction transaction = _context.Database.BeginTransaction();
                try
                {
                    Warehouse warehouse = _context.Warehouse.AsNoTracking().FirstOrDefault(x => x.id == model.warehouse_id);
                    Refund refund = _mapper.Map<Refund>(model);
                    refund.code = warehouse.code + ".R";
                    refund.dateAdded = DateTime.Now;
                    if (refund.customer_id == 0)
                    {
                        var passersby = await _context.Customer.FirstOrDefaultAsync(e => e.phone == "1111111");
                        if (passersby == null)
                        {
                            var pax = new Customer
                            {
                                phone = "1111111",
                                address = "HN",
                                name = "Khách hoàn hàng",
                                userAdded = refund.userAdded
                            };
                            _context.Customer.Add(pax);
                            _context.SaveChanges();
                            refund.customer_id = pax.id;
                        }
                        else
                        {
                            refund.customer_id = passersby.id;
                        }
                    }
                    int count = _context.Order.Count(x => x.code.Contains(refund.code));
                    count++;
                    refund.code += count;
                    model.customer_id = refund.customer_id;
                    model.id = refund.id;
                    _context.Refund.Add(refund);
                    _context.SaveChanges();

                    foreach (var (item, refundOrder) in from item in model.refund_order
                                                        let refundOrder = _mapper.Map<Refund_Order>(item)
                                                        select (item, refundOrder))
                    {

                        refundOrder.refund_id = refund.id;
                        refundOrder.dateAdded = DateTime.Now;
                        refundOrder.userAdded = refund.userAdded;
                        _context.Refund_Order.Add(refundOrder);
                        _context.SaveChanges();
                        List<Refund_Order_Product> refund_Order_Products = new List<Refund_Order_Product>();
                        foreach (var refundProduct in from RefundProduct i in item.products
                                                      let refundProduct = _mapper.Map<Refund_Order_Product>(i)
                                                      select refundProduct)
                        {
                            refundProduct.id = 0;
                            refundProduct.refund_id = refund.id;
                            refundProduct.dateAdded = DateTime.Now;
                            refundProduct.refund_order_id = refundOrder.id;
                            refundProduct.userAdded = refund.userAdded;
                            refund_Order_Products.Add(refundProduct);
                        }
                        _context.Refund_Order_Product.AddRange(refund_Order_Products);
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                    // tao don hang am 
                    model.order_id = await CreateOrderRefund(model);
                    return model;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new RefundModel();
                }
            });
        }
        private async Task<long> CreateOrderRefund(RefundModel refund)
        {
            return await Task.Run(async () =>
            {
                using IDbContextTransaction transaction = _context.Database.BeginTransaction();
                try
                {
                    Warehouse warehouse = _context.Warehouse.AsNoTracking().FirstOrDefault(x => x.id == refund.warehouse_id);
                    // cong tru point
                    Customer_point_history_create(refund);
                    OrderModel orders = new()
                    {
                        code = warehouse.code + ".RO",
                        customer_id = refund.customer_id,
                        dateAdded = DateTime.Now,
                        note = "Don hang hoan: " + string.Join(",", refund.refund_order?.Select(e => e.order_code).ToList()),
                        payment_type = 0,
                        sales_session_id = refund.sales_session_id,
                        product_total_cost = -refund.product_total_cost,
                        sale_cost = -refund.sale_cost,
                        voucher_cost = -refund.voucher_cost,
                        total_amount = -refund.total_refund_money,
                        userAdded = refund.userAdded,
                        warehouse_id = refund.warehouse_id,
                        order_lines = new List<Order_Detail>()
                    };
                   

                    Order order = _mapper.Map<Order>(orders);
                    int count = _context.Order.Count(x => x.code.Contains(order.code));
                    count++;
                    order.code += count;
                    order.type = 1;
                    _context.Order.Add(order);
                    _context.SaveChanges();
                    orders.id = order.id;
                    foreach (var item in refund.refund_order)
                    {
                        foreach (RefundProduct i in item.products)
                        {
                            orders.order_lines.Add(new Order_Detail
                            {
                                dateAdded = DateTime.Now,
                                order_id = order.id,
                                price = -i.price,
                                packing_code = i.packing_code,
                                product_id = i.product_id,
                                product_warehouse_id = i.product_warehouse_id,
                                quantity = i.quantity,
                                unit_code = i.unit_code,
                                userAdded = refund.userAdded,
                                warehouse_id = i.warehouse_id,
                                sale_price = 0
                            });
                        }
                    }

                    //update tru tien phien ban hang 
                    if (order.sales_session_id != 0)
                    {
                        var sale_session = await _context.Sales_Session.FirstOrDefaultAsync(x => x.id == order.sales_session_id && !x.is_delete);
                        if (sale_session != null)
                        {
                            sale_session.closing_cash += (double)order.total_amount;
                            _context.Sales_Session.Update(sale_session);
                        }
                    }
                   
                    _context.Order_Detail.AddRange(orders.order_lines);

                    var refundUpdate = await _context.Refund.FirstOrDefaultAsync(e => e.id == refund.id);
                    if (refundUpdate != null)
                    {
                        refundUpdate.order_id = order.id;
                        _context.Refund.Update(refundUpdate);
                    }
                    _context.SaveChanges();
                    transaction.Commit();

                    return order.id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return 0;
                }
            });
        }
        public async Task<PaginationSet<RefundModel>> RefundList(RefundSearch search)
        {
            PaginationSet<RefundModel> response = new();
            IQueryable<RefundModel> listItem = from a in _context.Refund
                                               join b in _context.Customer on a.customer_id equals b.id
                                               join c in _context.Admin_User on a.userAdded equals c.id
                                               where !a.is_delete //& a.warehouse_id == search.warehouse_id
                                               select new RefundModel
                                               {
                                                   id = a.id,
                                                   code = a.code,
                                                   product_total_cost = a.product_total_cost,
                                                   warehouse_id = a.warehouse_id,
                                                   customer_id = a.customer_id,
                                                   customer_name = b.name ?? string.Empty,
                                                   staff_name = c.full_name,
                                                   customer_phone = b.phone ?? string.Empty,
                                                   dateAdded = a.dateAdded,
                                                   note = a.note
                                               };

            if (!string.IsNullOrEmpty(search.keyword))
            {
                listItem = listItem.Where(r => r.code.Contains(search.keyword));
            }
            if (search.warehouse_id != 0)
            {
                listItem = listItem.Where(r => r.warehouse_id == search.warehouse_id);
            }

            if (search.start_date != null)
            {
                listItem = listItem.Where(r => r.dateAdded >= search.start_date);
            }
            if (search.end_date != null)
            {
                listItem = listItem.Where(r => search.end_date.Value.AddDays(1) >= r.dateAdded);
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
        public async Task<RefundModel> RefundModify(RefundModel RefundModel)
        {
            return await Task.Run(async () =>
            {
                using IDbContextTransaction transaction = _context.Database.BeginTransaction();
                try
                {

                    //_context.SaveChanges();
                    transaction.Commit();
                    return RefundModel;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return new RefundModel();
                }
            });
        }
        public async Task<List<RefundSearchOrderModel>> OrderList(string? keyword, long warehouse_id)
        {

            List<RefundSearchOrderModel> response = new();
            IQueryable<RefundSearchOrderModel> listItem = from a in _context.Order
                                                          join b in _context.Customer on a.customer_id equals b.id
                                                          join c in _context.Admin_User on a.userAdded equals c.id
                                                          where !a.is_delete & a.warehouse_id == warehouse_id
                                                          & a.type == 0
                                                          select new RefundSearchOrderModel
                                                          {
                                                              id = a.id,
                                                              code = a.code,
                                                              pos_id = a.pos_id,
                                                              warehouse_id = a.warehouse_id,
                                                              sales_session_id = a.sales_session_id,
                                                              customer_id = a.customer_id,
                                                              customer_name = b.name ?? string.Empty,
                                                              staff_name = c.full_name,
                                                              customer_phone = b.phone ?? string.Empty,
                                                              member_point = a.member_point,
                                                              voucher_id = a.voucher_id,
                                                              voucher_cost = a.voucher_cost,
                                                              sale_cost = a.sale_cost,
                                                              payment_type = a.payment_type,
                                                              status_id = a.status_id,
                                                              product_total_cost = a.product_total_cost,
                                                              total_amount = a.total_amount,
                                                              dateAdded = a.dateAdded
                                                          };

            if (!string.IsNullOrEmpty(keyword))
            {
                listItem = listItem.Where(r => r.code.Contains(keyword)
                || r.customer_name.Contains(keyword)
                || r.customer_phone.Contains(keyword)
                );
            }
            response = await listItem.OrderByDescending(r => r.dateAdded).Take(30).ToListAsync();

            List<long> listorder_id = response.Select(r => r.id).ToList();
            var listproducts = await (from a in _context.Order_Detail
                                      join c in _context.Product on a.product_id equals c.id
                                      join d in _context.Product_Warehouse on a.product_warehouse_id equals d.id
                                      where listorder_id.Contains(a.order_id) & !a.is_delete
                                      select new Model.Order.OrderProduct
                                      {
                                          id = a.id,
                                          name = c.name,
                                          order_id = a.order_id,
                                          barcode = d.barcode,
                                          price = a.price,
                                          sale_price = a.sale_price,
                                          packing_code = a.packing_code,
                                          product_id = a.product_id,
                                          quantity = a.quantity,
                                          unit_code = a.unit_code,
                                          warehouse_id = a.warehouse_id,
                                          product_warehouse_id = a.product_warehouse_id
                                      }).ToListAsync();
            foreach (var item in response)
            {
                item.products = listproducts.Where(p => p.order_id == item.id).ToList();
            }

            return response;
        }
        public async Task<OrderCheckData> OrderCheck(RefundSearchOrderModel model)
        {
            return await Task.Run(async () =>
            {

                 if (model.dateAdded <= DateTime.Now.AddDays(-3))
                {
                    return new OrderCheckData { code = 4 };
                }
                if (model.products == null || model.products.Count == 0)
                {
                    return new OrderCheckData { code = 0 };
                }

                var query = await (from a in _context.Refund_Order_Product
                                   join b in _context.Refund_Order on a.refund_order_id equals b.id
                                   where b.order_id == model.id
                                   group a by new { a.product_id } into g
                                   select new
                                   {
                                       product_id = g.Key.product_id,
                                       total = g.Sum(e => e.quantity)
                                   }).ToListAsync();
                if (query == null || query.Count == 0)
                {
                    return new OrderCheckData
                    {
                        data = model,
                        code = 1
                    };
                }

                foreach (var item in model.products)
                {
                    item.quantity -= query.FirstOrDefault(e => e.product_id == item.product_id)?.total ?? 0;
                }

                if (model.products.Count(e => e.quantity > 0) == 0)
                {
                    return new OrderCheckData { code = 3 };
                }
                return new OrderCheckData
                {
                    data = model,
                    code = 2
                };

            });
        }
        public async void Customer_point_history_create(RefundModel model)
        {
            Customer_Member_Config customer_Member_Config = _context.Customer_Member_Config.FirstOrDefault(x => x.is_active == true && !!x.is_delete && x.end_date >= DateTime.Now && DateTime.Now >= x.start_date);
            if (customer_Member_Config != null)
            {
                Customer customer = _context.Customer.FirstOrDefault(x => x.id == model.customer_id);
                if (model.member_point > 0)
                {
                    Customer_Point_History customer_Point_History = new()
                    {
                        type = 1,
                        number_of_point = model.member_point,
                        customer_id = model.customer_id,
                        order_id = model.id,
                        value_of_point = model.member_point * customer_Member_Config.value_of_point
                    };
                    if (customer != null)
                    {
                        customer.member_point += model.member_point;
                    }
                    _context.Customer_Point_History.Add(customer_Point_History);
                }
                _context.SaveChanges();

            }
        }
        public async void Warehouse_receipt_create(OrderModel orders)
        {
            var warehouse = _context.Warehouse.AsNoTracking().FirstOrDefault(x => x.id == orders.warehouse_id);

            Warehouse_Receipt warehouse_Receipt = new()
            {
                code = "N." + warehouse.code + ".",
                dateAdded = DateTime.Now,
                warehouse_id = orders.warehouse_id,
                partner_id = orders.customer_id,
                request_id = 0,
                note = orders.note,
                content = "Nhap hoan hang - phien hoan " + orders.code ,
                transfer_date = DateTime.Now,
                delivery_address = warehouse.address,
                status_id = 1,
                userAdded = orders.userAdded,
                type = 0
            }; // X.Mã_Kho.STT
            int count = _context.Warehouse_Receipt.Count(x => x.type == warehouse_Receipt.type && x.code.Contains(warehouse_Receipt.code));
            count++;
            warehouse_Receipt.code += count;
            _context.Warehouse_Receipt.Add(warehouse_Receipt);
            _context.SaveChanges();
            //update data tang sl  kho
            List<long> product_ids = orders.order_lines.Select(x => x.product_warehouse_id).ToList();
            var products_warehouse = await _context.Product_Warehouse.Where(x => product_ids.Contains(x.id) && !x.is_delete).ToListAsync();
            List<Product_Warehouse_History> products_warehouse_historys = new();
            List<Warehouse_Receipt_Product> warehouse_Receipt_Products = new();
            foreach (var item in orders.order_lines)
            {
                item.order_id = orders.id;
                item.dateAdded = DateTime.Now;
                var product = products_warehouse.FirstOrDefault(x => x.id == item.product_warehouse_id);
                product.quantity_sold -= item.quantity;
                product.quantity_stock += item.quantity;
                Product_Warehouse_History history = new()
                {
                    code = orders.code,
                    product_id = item.product_id,
                    id_table = item.order_id,
                    product_warehouse_id = item.product_warehouse_id,
                    type = 3,
                    quantity = item.quantity,
                    quantity_in_stock = product.quantity_stock,
                    import_price = product.import_price,
                    price = item.price,
                    sale_price = item.sale_price,
                    unit_code = item.unit_code,
                    packing_code = item.packing_code,
                    exp_date = product.exp_date,
                    is_promotion = product.is_promotion,
                    warehouse_id = product.warehouse_id,
                    batch_number = product.batch_number
                };
                products_warehouse_historys.Add(history);
                Warehouse_Receipt_Product warehouse_Receipt_Product = new()
                {
                    product_id = item.product_id,
                    receipt_id = warehouse_Receipt.id,
                    partner_id = orders.customer_id,
                    quantity = item.quantity,
                    import_price = product.import_price,
                    price = item.price,
                    weight = 0 ,
                    batch_number = product.batch_number,
                    category_unit_code = product.unit_code,
                    category_packing_code = product.packing_code,
                    is_weigh = product.is_weigh??false,
                    is_promotion = product.is_promotion ,
                    exp_date = product.exp_date??DateTime.Now   ,
                    warning_date = 100,
                    warehouse_id = item.warehouse_id,
                    barcode = product.barcode,
                    userAdded = orders.userAdded
                };
                warehouse_Receipt_Products.Add(warehouse_Receipt_Product);
            }
            _context.Product_Warehouse_History.AddRange(products_warehouse_historys);
            _context.Warehouse_Receipt_Product.AddRange(warehouse_Receipt_Products);
            _context.Product_Warehouse.UpdateRange(products_warehouse);

        }
    }
}
