using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Receipt;

namespace Point_of_Sale.Repositories
{
    internal class ReceiptRepository : IReceiptRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public ReceiptRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<ReceiptModel> Receipt(long id)
        {
            return await Task.Run(async () =>
            {
                ReceiptModel model = new ReceiptModel();
                Warehouse_Receipt request = await _context.Warehouse_Receipt.FirstOrDefaultAsync(r => r.id == id);
                if (request != null)
                {
                    model = _mapper.Map<ReceiptModel>(request);
                    var listProduct = await _context.Warehouse_Receipt_Product.Where(r => r.receipt_id == id && !r.is_delete).ToListAsync();
                    //check fill price = 0
                    var listId = listProduct.Select(e => e.product_id).ToList();
                    var listProductWarehouse = await _context.Product_Warehouse.Where(p => p.warehouse_id == request.warehouse_id && !p.is_delete && listId.Contains(p.product_id)).ToListAsync();
                    foreach (var item in listProduct)
                    {
                        var updatePrice = listProductWarehouse.FirstOrDefault(x => x.warehouse_id == item.warehouse_id && x.product_id == item.product_id);
                        if (updatePrice is not null)
                        {
                            item.price = item.price == 0 ? updatePrice.price : item.price;
                            item.import_price = item.import_price == 0 ? updatePrice.import_price : item.import_price;
                        }
                    }

                    model.Products = _mapper.Map<List<Receipt_ProductModel>>(listProduct);
                }

                return model;
            });
        }
        public async Task<string> ReceiptCreate(ReceiptModel model)
        {
            return await Task.Run(() =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    string respone = "0";
                    try
                    {
                        Warehouse_Receipt receipt = _mapper.Map<Warehouse_Receipt>(model);
                        receipt.dateAdded = DateTime.Now;
                        int count = _context.Warehouse_Receipt.Count(x => x.type == model.type && x.code.Contains(model.code));
                        count++;
                        receipt.code += count;
                        _context.Warehouse_Receipt.Add(receipt);
                        _context.SaveChanges();
                        var listProduct = _mapper.Map<List<Warehouse_Receipt_Product>>(model.Products);

                        foreach (var item in listProduct)
                        {
                            item.receipt_id = receipt.id;
                            item.partner_id = receipt.partner_id;
                            item.dateAdded = receipt.dateAdded;
                            item.userAdded = model.userAdded;

                        }
                        _context.Warehouse_Receipt_Product.AddRange(listProduct);
                        _context.SaveChanges();
                        transaction.Commit();
                        return Task.FromResult(respone);

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        respone = ex.Message + " - " + ex.StackTrace;
                        return Task.FromResult(respone);
                    }

                };
            });
        }
        public async Task<string> GetBarcode(long id)
        {
            return await Task.Run(async () =>
                    {
                        return await (from a in _context.Warehouse_Receipt_Product
                                      join b in _context.Product_Warehouse on a.product_id equals b.product_id
                                      where a.category_packing_code == b.packing_code && a.category_unit_code == b.unit_code && a.exp_date == b.exp_date
                                      && a.id == id
                                      select b.barcode).FirstOrDefaultAsync() ?? String.Empty;
                    });
        }
        public async Task<string> ReceiptModify(ReceiptModel model)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    string respone = "0";
                    try
                    {
                        Warehouse_Receipt receipt = await _context.Warehouse_Receipt.FirstOrDefaultAsync(r => r.id == model.id); ;
                        receipt.dateUpdated = DateTime.Now;
                        receipt.warehouse_id = model.warehouse_id;
                        receipt.partner_id = model.partner_id;
                        receipt.request_id = model.request_id;
                        receipt.note = model.note;
                        receipt.content = model.content;
                        receipt.transfer_date = model.transfer_date;
                        receipt.status_id = model.status_id;
                        receipt.type = model.type;
                        receipt.delivery_address = model.delivery_address;
                        receipt.userUpdated = model.userUpdated;
                        _context.Warehouse_Receipt.Update(receipt);

                        var listProduct = _mapper.Map<List<Warehouse_Receipt_Product>>(model.Products);
                        foreach (var item in model.Products)
                        {
                            if (item.id == 0)
                            {
                                var product = _mapper.Map<Warehouse_Receipt_Product>(item);
                                product.receipt_id = receipt.id;
                                product.partner_id = receipt.partner_id;
                                product.is_weigh = item.is_weigh;
                                product.is_promotion = item.is_promotion;
                                product.dateAdded = receipt.dateUpdated ?? DateTime.Now;
                                product.userAdded = model.userAdded;
                                _context.Warehouse_Receipt_Product.Add(product);
                            }
                            else
                            {
                                Warehouse_Receipt_Product product = await _context.Warehouse_Receipt_Product.FirstOrDefaultAsync(r => r.id == item.id);
                                if (product != null)
                                {
                                    product.product_id = item.product_id;
                                    product.partner_id = receipt.partner_id;
                                    product.quantity = item.quantity;
                                    product.userUpdated = model.userUpdated;
                                    product.weight = item.weight;
                                    product.category_unit_code = item.category_unit_code;
                                    product.category_packing_code = item.category_packing_code;
                                    product.warehouse_id = item.warehouse_id;
                                    product.note = item.note;
                                    product.barcode = item.barcode;
                                    product.exp_date = item.exp_date;
                                    product.warning_date = item.warning_date;
                                    product.batch_number = item.batch_number;
                                    product.price = item.price;
                                    product.import_price = item.import_price;
                                    product.is_weigh = item.is_weigh;
                                    product.is_promotion = item.is_promotion;
                                    product.is_delete = item.is_delete;
                                    _context.Warehouse_Receipt_Product.Update(product);

                                }

                            }
                        }
                        if (model.status_id == 1)
                        {
                            List<long> listProductId = model.Products.Select(r => r.product_id).ToList();
                            var listProductWarehouse = await _context.Product_Warehouse.Where(r => r.warehouse_id == model.warehouse_id && listProductId.Contains(r.product_id) && !r.is_delete).ToListAsync();

                            List<Product_Warehouse_History> products_warehouse_history = new List<Product_Warehouse_History>();
                            foreach (var item in model.Products)
                            {
                                Product_Warehouse product = listProductWarehouse.Where(r =>
                                r.product_id == item.product_id && r.barcode == item.barcode && r.is_promotion == item.is_promotion).FirstOrDefault();
                                if (product != null && product.id != 0)
                                {
                                    if (item.price != 0)
                                    {
                                        product.price = item.price;
                                    }
                                    if (item.import_price != 0)
                                    {
                                        product.import_price = item.import_price;
                                    }
                                    product.quantity_stock += item.quantity;
                                    product.dateUpdated = DateTime.Now;
                                    _context.Product_Warehouse.Update(product);
                                }
                                else
                                {
                                    Product_Warehouse product_warehouse = new Product_Warehouse
                                    {
                                        import_price = item.import_price,
                                        price = item.price,
                                        quantity_stock = item.quantity,
                                        quantity_sold = 0,
                                        barcode = item.barcode,
                                        batch_number = item.batch_number,
                                        is_weigh = item.is_weigh,
                                        is_promotion = item.is_promotion,
                                        warehouse_id = model.warehouse_id,
                                        packing_code = item.category_packing_code,
                                        unit_code = item.category_unit_code,
                                        product_id = item.product_id,
                                        //warning_date = item.warning_date,
                                        exp_date = item.exp_date,
                                        dateAdded = DateTime.Now,
                                    };
                                    _context.Product_Warehouse.Add(product_warehouse);
                                }
                                //update lich su nhap kho
                                Product_Warehouse_History history = new Product_Warehouse_History
                                {
                                    code = receipt.code,
                                    product_id = item.product_id,
                                    id_table = item.receipt_id,
                                    product_warehouse_id = item.id,
                                    type = 1,
                                    quantity = item.quantity,
                                    price = item.price,
                                    unit_code = item.category_unit_code,
                                    packing_code = item.category_packing_code,
                                    import_price = item.import_price,
                                    exp_date = item.exp_date,
                                    warning_date = item.warning_date,
                                    warehouse_id = item.warehouse_id,
                                    batch_number = item.batch_number,
                                    userAdded = model.userAdded,
                                    is_promotion = item.is_promotion,
                                };
                                history.quantity_in_stock = product != null ? product.quantity_stock + item.quantity : item.quantity;

                                products_warehouse_history.Add(history);
                            }
                            _context.Product_Warehouse_History.AddRange(products_warehouse_history);

                        }
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
        public async Task<PaginationSet<ReceiptViewModel>> ReceiptList(SearchReceipt search)
        {
            return await Task.Run(async () =>
            {
                PaginationSet<ReceiptViewModel> response = new PaginationSet<ReceiptViewModel>();
                var listItem = from a in _context.Warehouse_Receipt
                               join b in _context.Warehouse on a.warehouse_id equals b.id
                               join c in _context.Partner on a.partner_id equals c.id
                               join d in _context.Admin_User on a.userUpdated equals d.id into ad
                               from dd in ad.DefaultIfEmpty()
                               where !a.is_delete
                               select new ReceiptViewModel
                               {
                                   status_id = a.status_id,
                                   partner_id = a.partner_id,
                                   content = a.content,
                                   note = a.note,
                                   id = a.id,
                                   user_name = dd.full_name,
                                   dateUpdate = a.dateUpdated,
                                   partner_name = c.name,
                                   delivery_address = a.delivery_address,
                                   request_id = a.request_id,
                                   transfer_date = a.transfer_date,
                                   warehouse_id = a.warehouse_id,
                                   warehouse_name = b.name,
                                   type = a.type,
                                   code = a.code,
                                   total_amount = _context.Warehouse_Receipt_Product.Where(x => x.receipt_id == a.id & !x.is_delete).Sum(y => y.import_price * y.quantity)
                               };
                if (search.keyword is not null and not "")
                {
                    listItem = listItem.Where(r => r.content.Contains(search.keyword));
                }
                if (search.type != 0)
                {
                    listItem = listItem.Where(r => r.type == search.type);
                }
                if (search.status_id != null && search.status_id != 10)
                    listItem = listItem.Where(r => r.status_id == search.status_id);
                if (search.warehouse_id > 0)
                    listItem = listItem.Where(r => r.warehouse_id == search.warehouse_id);
                if (search.partner_id > 0)
                    listItem = listItem.Where(r => r.partner_id == search.partner_id);
                if (search.start_date != null)
                    listItem = listItem.Where(r => r.transfer_date >= search.start_date);
                if (search.end_date != null)
                    listItem = listItem.Where(r => search.end_date.Value.AddDays(1) > r.transfer_date);

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
        public async Task<bool> ReceiptDelete(long id)
        {
            try
            {
                Warehouse_Receipt receipt = await _context.Warehouse_Receipt.FirstOrDefaultAsync(r => r.id == id);
                receipt.is_delete = true;
                _context.Warehouse_Receipt.Update(receipt);

                var listProduct = await _context.Warehouse_Receipt_Product.Where(r => r.receipt_id == id).ToListAsync();
                foreach (var item in listProduct)
                {
                    item.is_delete = true;
                }
                _context.Warehouse_Receipt_Product.UpdateRange(listProduct);

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<Receipt_Print_Model> ReceiptExportPrint(long id)
        {
            return await Task.Run(async () =>
            {

                var model = await (from a in _context.Warehouse_Receipt
                                   join b in _context.Partner on a.partner_id equals b.id
                                   join c in _context.Admin_User on a.userAdded equals c.id
                                   join d in _context.Warehouse on a.warehouse_id equals d.id
                                   where a.id == id & !a.is_delete
                                   select new Receipt_Print_Model
                                   {
                                       id = a.id,
                                       code = a.code,
                                       content = a.content,
                                       receipt_date = a.dateAdded,
                                       note = a.note,
                                       partner_id = a.partner_id,
                                       partner_name = b.name,
                                       partner_phone = b.phone,
                                       delivery_address = a.delivery_address,
                                       type = a.type,
                                       user_name = c.full_name,
                                       warehouse_id = a.warehouse_id,
                                       warehouse_name = d.name,
                                       userAdded = b.userAdded,
                                   }).FirstOrDefaultAsync();

                model.products = await (from a in _context.Warehouse_Receipt_Product
                                        join b in _context.Product on a.product_id equals b.id
                                        where a.receipt_id == id & !a.is_delete
                                        select new Receipt_Print_Product
                                        {
                                            name = b.name,
                                            product_id = a.product_id,
                                            note = a.note,
                                            import_price = a.import_price,
                                            exp_date = a.exp_date,
                                            receipt_id = a.receipt_id,
                                            price = a.price,
                                            quantity = a.quantity,
                                            barcode = a.barcode,
                                            unit_code = a.category_unit_code,
                                            warning_date = a.warning_date,
                                            batch_number = a.batch_number,
                                            is_weigh = a.is_weigh,
                                            packing_code = a.category_packing_code,
                                            partner_id = a.partner_id,
                                            warehouse_id = a.warehouse_id
                                        }).ToListAsync();
                var listId = model.products.Select(e => e.product_id).ToList();
                var listProductWarehouse = await _context.Product_Warehouse.Where(p => p.warehouse_id == model.warehouse_id && !p.is_delete && listId.Contains(p.product_id)).ToListAsync();
                foreach (var item in model.products)
                {
                    var updatePrice = listProductWarehouse.FirstOrDefault(x => x.warehouse_id == item.warehouse_id && x.product_id == item.product_id);
                    if (updatePrice is not null)
                    {
                        item.price = item.price == 0 ? updatePrice.price : item.price;
                        item.import_price = item.import_price == 0 ? updatePrice.import_price : item.import_price;
                        item.products_warehouse_id = updatePrice.id;
                    }
                }

                return model;
            });
        }

    }
}
