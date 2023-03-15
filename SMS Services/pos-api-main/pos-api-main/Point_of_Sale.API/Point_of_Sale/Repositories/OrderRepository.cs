using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml.Style;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Order;

namespace Point_of_Sale.Repositories
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<OrderModel> Order(long id)
        {
            return await Task.Run(async () =>
            {
                OrderModel model = new OrderModel();
                var order = await _context.Order.FirstOrDefaultAsync(r => r.id == id);
                if (order == null)
                {
                    return new OrderModel();
                }
                model = _mapper.Map<OrderModel>(order);
                var customer = await _context.Customer.FirstOrDefaultAsync(r => r.id == model.customer_id);
                if (customer != null)
                {
                    model.customer_phone = customer.phone;
                    model.customer_name = customer.name;
                }

                model.products = await (from a in _context.Order_Detail
                                        join c in _context.Product on a.product_id equals c.id
                                        join d in _context.Product_Warehouse on a.product_warehouse_id equals d.id
                                        where a.order_id == id
                                        select new OrderProduct
                                        {
                                            name = c.name,
                                            order_id = a.id,
                                            barcode = d.barcode,
                                            price = a.price,
                                            sale_price = a.sale_price,
                                            packing_code = a.packing_code,
                                            product_id = a.product_id,
                                            quantity = a.quantity,
                                            unit_code = a.unit_code,
                                            warehouse_id = a.warehouse_id
                                        }).ToListAsync();

                return model;
            });
        }
        public async Task<PaginationSet<OrderModel>> OrderList(OrderSearch search)
        {
            await Task.CompletedTask;
            PaginationSet<OrderModel> response = new();
            IQueryable<OrderModel> listItem = from a in _context.Order
                                              join b in _context.Customer on a.customer_id equals b.id
                                              join c in _context.Admin_User on a.userAdded equals c.id
                                              where !a.is_delete /*& a.type == 0*/ & a.warehouse_id == search.warehouse_id
                                              select new OrderModel
                                              {
                                                  id = a.id,
                                                  code = a.code,
                                                  member_point_value = a.member_point_value,
                                                  member_point = a.member_point,
                                                  pos_id = a.pos_id,
                                                  warehouse_id = a.warehouse_id,
                                                  sales_session_id = a.sales_session_id,
                                                  customer_id = a.customer_id,
                                                  customer_name = b.name ?? string.Empty,
                                                  staff_name = c.full_name,
                                                  customer_phone = b.phone ?? string.Empty,
                                                  voucher_id = a.voucher_id,
                                                  voucher_cost = a.voucher_cost,
                                                  sale_cost = a.sale_cost,
                                                  payment_type = a.payment_type,
                                                  status_id = a.status_id,
                                                  product_total_cost = a.product_total_cost,
                                                  total_amount = a.total_amount,
                                                  dateAdded = a.dateAdded,
                                                  order_lines = _context.Order_Detail.Where(x => x.order_id == a.id).ToList()
                                              };

            if (!string.IsNullOrEmpty(search.keyword))
            {
                listItem = listItem.Where(r => r.code.Contains(search.keyword)
                || r.customer_name.Contains(search.keyword)
                || r.customer_phone.Contains(search.keyword)
                );
            }
            if (!string.IsNullOrEmpty(search.staff_name))
            {
                listItem = listItem.Where(r => r.staff_name.Contains(search.staff_name));
            }

            if (search.start_date != null)
            {
                listItem = listItem.Where(r => r.dateAdded >= search.start_date);
            }
            if (search.end_date != null)
            {
                listItem = listItem.Where(r => search.end_date.Value.AddDays(1) >= r.dateAdded);
            }
            if (search.pos_id > 0)
            {
                listItem = listItem.Where(r => r.pos_id == search.pos_id);
            }
            if (search.staff_id > 0)
            {
                listItem = listItem.Where(r => r.userAdded == search.staff_id);
            }
            if (search.sales_session_id > 0)
            {
                listItem = listItem.Where(r => r.sales_session_id == search.sales_session_id);
            }
            if (search.customer_id > 0)
            {
                listItem = listItem.Where(r => r.customer_id == search.customer_id);
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
        public async Task<List<OrderModel>> OrderList2()
        {
            await Task.CompletedTask;
            List<OrderModel> response = new();
            IQueryable<OrderModel> listItem = from a in _context.Order
                                              join b in _context.Customer on a.customer_id equals b.id
                                              join c in _context.Admin_User on a.userAdded equals c.id
                                              where !a.is_delete & a.dateAdded >= DateTime.Now.Date & a.type == 0
                                              select new OrderModel
                                              {
                                                  id = a.id,
                                                  code = a.code,
                                                  pos_id = a.pos_id,
                                                  member_point_value = a.member_point_value,
                                                  member_point = a.member_point,
                                                  warehouse_id = a.warehouse_id,
                                                  sales_session_id = a.sales_session_id,
                                                  customer_id = a.customer_id,
                                                  customer_name = b.name ?? string.Empty,
                                                  staff_name = c.full_name,
                                                  customer_phone = b.phone ?? string.Empty,
                                                  voucher_id = a.voucher_id,
                                                  voucher_cost = a.voucher_cost,
                                                  sale_cost = a.sale_cost,
                                                  payment_type = a.payment_type,
                                                  status_id = a.status_id,
                                                  product_total_cost = a.product_total_cost,
                                                  total_amount = a.total_amount,
                                                  dateAdded = a.dateAdded,
                                                  order_lines = _context.Order_Detail.Where(x => x.order_id == a.id).ToList()
                                              };

            response = await listItem.OrderByDescending(r => r.dateAdded).ToListAsync();
            return response;
        }
        public async Task<string> OrdersCreate(List<OrderModel> orders)
        {
            try
            {
                foreach (var item in orders)
                {
                    await OrderCreate(item);
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return "false";

            }
        }
        public async Task<OrderModel> OrderCreate(OrderModel orders)
        {
            return await Task.Run(async () =>
            {
                Order order = _mapper.Map<Order>(orders);
                order.dateAdded = DateTime.Now;
                if (order.customer_id == 0)
                {
                    var passersby = await _context.Customer.FirstOrDefaultAsync(e => e.phone == "0000000");
                    if (passersby == null)
                    {
                        var pax = new Customer
                        {
                            phone = "0000000",
                            address = "HN",
                            name = "Khách vãng lai",
                            userAdded = order.userAdded
                        };
                        _context.Customer.Add(pax);
                        _context.SaveChanges();
                        order.customer_id = pax.id;
                    }
                    else
                    {
                        order.customer_id = passersby.id;
                    }
                }
                int count = _context.Order.Count(x => x.code.Contains(orders.code));
                count++;
                order.code += count;
                _context.Order.Add(order);
                _context.SaveChanges();
                orders.id = order.id;
                // add warehouse export
                Warehouse_export_create(orders);
                Customer_Member_Config customer_Member_Config = await _context.Customer_Member_Config.AsNoTracking().Where(x => x.is_active && !x.is_delete && x.end_date >= DateTime.Now && DateTime.Now >= x.start_date).FirstOrDefaultAsync();
                if (customer_Member_Config != null)
                {
                    // update poit for customer 
                    Customer_point_history_create(orders, customer_Member_Config);
                }

                //update tru kho 
                var sale_session = await _context.Sales_Session.FirstOrDefaultAsync(x => x.id == order.sales_session_id);
                if (order.payment_type == 0)
                {
                    sale_session.closing_cash += (double)order.total_amount;
                }
                if (order.payment_type == 1)
                {
                    sale_session.closing_card += (double)order.total_amount;
                }
                if (order.payment_type == 2)
                {
                    sale_session.closing_online_transfer += (double)order.total_amount;
                }
                //update so lan su dung voucher
                if (orders.voucher_id != null)
                {
                    var voucher = await _context.Voucher.FirstOrDefaultAsync(x => x.id == orders.voucher_id);
                    voucher.used_quantity += 1;
                    _context.Voucher.Update(voucher);
                }
                _context.Sales_Session.Update(sale_session);
                foreach (var item in orders.order_lines)
                {
                    item.order_id = order.id;
                }
                _context.Order_Detail.AddRange(orders.order_lines);
                _context.SaveChanges();
                OrderModel order_response = _mapper.Map<OrderModel>(order);
                order_response.order_lines = orders.order_lines;
                return order_response;
            });
        }
        public async Task<OrderModel> OrderModify(OrderModel orderModel)
        {
            return await Task.Run(async () =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Order order = await _context.Order.FirstOrDefaultAsync(x => x.id == orderModel.id);
                        order.dateUpdated = DateTime.Now;
                        order.code = orderModel.code;
                        order.pos_id = orderModel.pos_id;
                        order.warehouse_id = orderModel.warehouse_id;
                        order.sales_session_id = orderModel.sales_session_id;
                        order.customer_id = orderModel.customer_id;
                        order.voucher_id = orderModel.voucher_id;
                        order.voucher_cost = orderModel.voucher_cost;
                        order.payment_type = orderModel.payment_type;
                        order.status_id = orderModel.status_id;
                        order.product_total_cost = orderModel.product_total_cost;
                        order.total_amount = orderModel.total_amount;
                        order.bank_account = orderModel.bank_account;
                        order.transaction_code = orderModel.transaction_code;
                        order.member_point = orderModel.member_point;
                        order.member_point_value = orderModel.member_point_value;

                        _context.Order.Update(order);
                        _context.SaveChanges();
                        //remove order_lines
                        List<Order_Detail> order_detailDB = await _context.Order_Detail.Where(x => x.order_id == orderModel.id && !x.is_delete).ToListAsync();
                        order_detailDB.ForEach(x => x.is_delete = true);
                        _context.Order_Detail.UpdateRange(order_detailDB);
                        //remove Product_Warehouse_History
                        List<Product_Warehouse_History> Product_Warehouse_History_delete = await _context.Product_Warehouse_History.Where(x => x.id_table == orderModel.id && x.type == 3 && !x.is_delete).ToListAsync();
                        Product_Warehouse_History_delete.ForEach(x => x.is_delete = true);
                        _context.Product_Warehouse_History.UpdateRange(Product_Warehouse_History_delete);

                        List<long> product_ids = orderModel.order_lines.Select(x => x.product_id).ToList();
                        var products_warehouse = await _context.Product_Warehouse.Where(x => product_ids.Contains(x.product_id)).ToListAsync();

                        List<Product_Warehouse_History> products_warehouse_historys = new List<Product_Warehouse_History>();
                        await _context.SaveChangesAsync();
                        foreach (var item in orderModel.order_lines)
                        {
                            item.id = 0;
                            item.order_id = order.id;
                            item.dateAdded = DateTime.Now;
                            var product = products_warehouse.FirstOrDefault(x => x.product_id == item.product_id);
                            product.quantity_sold += item.quantity;
                            product.quantity_stock -= item.quantity;
                            Product_Warehouse_History history = new();
                            history.product_id = item.product_id;
                            history.id_table = item.order_id;
                            history.product_warehouse_id = item.id;
                            history.quantity = item.quantity;
                            history.quantity_in_stock = product.quantity_stock;
                            history.import_price = product.import_price;
                            history.price = item.price;
                            history.unit_code = item.unit_code;
                            history.packing_code = item.packing_code;
                            history.exp_date = product.exp_date;
                            history.userAdded = order.userAdded;
                            //history.warning_date = product.warning_date;
                            history.warehouse_id = product.warehouse_id;
                            history.batch_number = product.batch_number;
                            history.is_promotion = product.is_promotion;
                            products_warehouse_historys.Add(history);
                        }
                        _context.Product_Warehouse_History.AddRange(products_warehouse_historys);
                        _context.Order_Detail.AddRange(orderModel.order_lines);
                        _context.SaveChanges();
                        transaction.Commit();
                        return orderModel;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new OrderModel();
                    }
                }
            });
        }
        public async Task<string> OrderDelete(long id)
        {
            return await Task.Run(async () =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Order orderDB = await _context.Order.FirstOrDefaultAsync(x => x.id == id);
                        orderDB.is_delete = true;
                        _context.Order.Remove(orderDB);
                        //remove order Line
                        List<Order_Detail> order_detailDB = await _context.Order_Detail.Where(x => x.order_id == id).ToListAsync();
                        order_detailDB.ForEach(x => x.is_delete = true);
                        _context.Order_Detail.UpdateRange(order_detailDB);
                        //remove Product_Warehouse_History
                        List<Product_Warehouse_History> Product_Warehouse_History_delete = await _context.Product_Warehouse_History.Where(x => x.id_table == id && x.type == 3).ToListAsync();
                        Product_Warehouse_History_delete.ForEach(x => x.is_delete = true);
                        _context.Product_Warehouse_History.UpdateRange(Product_Warehouse_History_delete);
                        _context.SaveChanges();
                        transaction.Commit();
                        return "0";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return "Exception: " + ex.Message;
                    }
                }
            });
        }
        public async void Warehouse_export_create(OrderModel order)
        {
            var warehouse = _context.Warehouse.AsNoTracking().FirstOrDefault(x => x.id == order.warehouse_id);

            Warehouse_Export warehouse_Export = new()
            {
                code = "X." + warehouse.code + ".",
                dateUpdated = DateTime.Now,
                warehouse_id = order.warehouse_id,
                order_id = order.id,
                customer_id = order.customer_id,
                partner_id = 0,
                note = order.note,
                content = "Xuat ban hang",
                export_date = DateTime.Now,
                source_address = warehouse.address,
                status_id = 1,
                userAdded = order.userAdded,
                type = 0
            }; // X.Mã_Kho.STT
            int count = _context.Warehouse_Export.Count(x => x.type == warehouse_Export.type && x.code.Contains(warehouse_Export.code));
            count++;
            warehouse_Export.code += count;
            _context.Warehouse_Export.Add(warehouse_Export);
            _context.SaveChanges();
            List<long> product_ids = order.order_lines.Select(x => x.product_warehouse_id).ToList();
            var products_warehouse = _context.Product_Warehouse.Where(x => product_ids.Contains(x.id) && !x.is_delete).ToList();

            List<Product_Warehouse_History> products_warehouse_historys = new();
            List<Warehouse_Export_Product> warehouse_Export_Products = new();
            //update data tru kho
            foreach (var item in order.order_lines)
            {
                item.order_id = order.id;
                item.dateAdded = DateTime.Now;
                var product = products_warehouse.FirstOrDefault(x => x.id == item.product_warehouse_id);
                product.quantity_sold += item.quantity;
                product.quantity_stock -= item.quantity;
                Warehouse_Export_Product warehouse_Export_Product = new()
                {
                    product_id = item.product_id,
                    products_warehouse_id = item.product_warehouse_id,
                    partner_id = 0,
                    export_id = warehouse_Export.id,
                    quantity = item.quantity,
                    import_price = product.import_price,
                    price = item.price,
                    unit_code = product.unit_code,
                    packing_code = product.packing_code,
                    warehouse_id = item.warehouse_id,
                    note = warehouse_Export.note,
                    exp_date = product.exp_date,
                    batch_number = product.batch_number,
                    barcode = product.barcode,
                    userAdded = order.userAdded
                };
                warehouse_Export_Products.Add(warehouse_Export_Product);

                Product_Warehouse_History history = new()
                {
                    code = order.code,
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
                    batch_number = product.batch_number,
                    userAdded = order.userAdded
                };
                products_warehouse_historys.Add(history);
            }
            _context.Warehouse_Export_Product.AddRange(warehouse_Export_Products);
            _context.Product_Warehouse.UpdateRange(products_warehouse);
            _context.Product_Warehouse_History.AddRange(products_warehouse_historys);
            _context.SaveChanges();
        }
        public async void Customer_point_history_create(OrderModel order, Customer_Member_Config customer_Member_Config)
        {
            Customer customer = _context.Customer.FirstOrDefault(x => x.id == order.customer_id);
            if (customer != null)
            {
                int point = 0;
                if (customer_Member_Config.is_total_amount == true && order.total_amount >= customer_Member_Config.min_apply_value)
                {
                    point = (int)(order.total_amount * customer_Member_Config.ratio_point) / 100;
                }
                if (customer_Member_Config.is_total_amount == false && order.product_total_cost >= customer_Member_Config.min_apply_value)
                {
                    point = (int)(order.product_total_cost * customer_Member_Config.ratio_point) / 100;
                }
                if (point > 0)
                {
                    Customer_Point_History customer_Point_History = new()
                    {
                        type = 0,
                        number_of_point = point,
                        customer_id = order.customer_id,
                        order_id = order.id,
                        value_of_point = point * customer_Member_Config.value_of_point
                    };
                    if (customer != null)
                    {
                        customer.member_point += point;
                    }
                    _context.Customer_Point_History.Add(customer_Point_History);
                }
                if (order.member_point > 0)
                {
                    Customer_Point_History customer_Point_History = new()
                    {
                        type = 0,
                        number_of_point = order.member_point,
                        customer_id = order.customer_id,
                        order_id = order.id,
                        value_of_point = order.member_point * customer_Member_Config.value_of_point
                    };
                    if (customer != null)
                    {
                        customer.member_point -= order.member_point + point;
                    }
                    _context.Customer_Point_History.Add(customer_Point_History);
                }
                _context.Customer.Update(customer);
                _context.SaveChanges();
            }
        }
    }
}
