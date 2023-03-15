using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Purchase;
using static Humanizer.On;

namespace Point_of_Sale.Repositories
{
    internal class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public PurchaseRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<PurchaseModel> Purchase(long id)
        {
            return await Task.Run(async () =>
            {
                Purchase request = await _context.Purchase.FirstOrDefaultAsync(r => r.id == id);
                PurchaseModel model = _mapper.Map<PurchaseModel>(request);
                List<Purchase_Product> listProduct = await _context.Purchase_Product.Where(r => r.purchase_id == id && !r.is_delete).ToListAsync();
                List<long> product_ids = listProduct.Select(x => x.product_id).Distinct().ToList();
                List<Product> list_product = _context.Product.Where(x => product_ids.Contains(x.id) && !x.is_delete).ToList();
                List<Product_Warehouse> list_product_warehouse = _context.Product_Warehouse.Where(x => product_ids.Contains(x.id) && !x.is_delete && x.warehouse_id == model.warehouse_id).ToList();
                foreach (var item in listProduct)
                {
                    PurchaseProductModel purchaseProduct = _mapper.Map<PurchaseProductModel>(item);
                    Product product = list_product.FirstOrDefault(x => x.id == item.product_id);
                    Product_Warehouse product_warehouse = list_product_warehouse.FirstOrDefault(x => x.product_id == item.product_id);
                    if (product!=null)
                    {
                        purchaseProduct.product_name = product.name;
                    }
                    if (product_warehouse != null)
                    {
                        purchaseProduct.quantity_inventory = product_warehouse.quantity_stock;
                    }
                    model.Products.Add(purchaseProduct);
                }

                return model;
            });
        }
        public async Task<string> PurchaseCreate(PurchaseModel model)
        {
            return await Task.Run(() =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    string respone = "0";
                    try
                    {

                        Purchase purchase = _mapper.Map<Purchase>(model);

                        purchase.status_id = 0;
                        _context.Purchase.Add(purchase);
                        _context.SaveChanges();

                        var listProduct = _mapper.Map<List<Purchase_Product>>(model.Products);

                        foreach (var item in listProduct)
                        {
                            item.purchase_id = purchase.id;
                            item.dateAdded = purchase.dateAdded;
                            item.userAdded = purchase.userAdded;
                            item.warehouse_id = purchase.warehouse_id;
                        }
                        _context.Purchase_Product.AddRange(listProduct);
                        purchase.code = "PW-" + purchase.id;

                        _context.Purchase.Update(purchase);
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
        public async Task<string> PurchaseDelete(long id)
        {
            Purchase purchase = await _context.Purchase.FirstOrDefaultAsync(r => r.id == id);
            purchase.is_delete = true;
            var listProduct = await _context.Purchase_Product.Where(r => r.purchase_id == id && !r.is_delete).ToListAsync();

            _context.Purchase.Update(purchase);
            foreach (var item in listProduct)
            {
                item.is_delete = true;
            }
            //_context.Purchase.Update(purchase);
            _context.Purchase_Product.UpdateRange(listProduct);
            _context.SaveChanges();
            return "0";
        }

        public async Task<string> PurchaseModify(PurchaseModel model)
        {
            return await Task.Run(async () =>
            {
                //using (var transaction = _context.Database.BeginTransaction())
                //{
                string respone = "0";
                try
                {
                    Purchase purchase = await _context.Purchase.FirstOrDefaultAsync(r => r.id == model.id); ;
                    purchase.dateUpdated = DateTime.Now;
                    purchase.warehouse_id = model.warehouse_id;
                    purchase.partner_id = model.partner_id;
                    purchase.note = model.note;
                    purchase.content = model.content;
                    purchase.transfer_date = model.transfer_date;
                    purchase.status_id = model.status_id;
                    _context.Purchase.Update(purchase);

                    var listProduct = _mapper.Map<List<Purchase_Product>>(model.Products);

                    List<Purchase_Product> product_db = await _context.Purchase_Product.Where(r => r.purchase_id == purchase.id).ToListAsync();
                    foreach (var item in product_db)
                    {
                        item.is_delete = true;
                    }
                    _context.Purchase_Product.UpdateRange(product_db);
                    List<Purchase_Product> product_new = new();

                    foreach (var item in model.Products)
                    {
                        Purchase_Product product = new();
                        //product.id = 0;
                        product.product_id = item.product_id;
                        product.quantity = item.quantity;
                        product.userUpdated = model.userUpdated;
                        product.category_unit_code = item.category_unit_code;
                        product.category_packing_code = item.category_packing_code;
                        product.warehouse_id = item.warehouse_id;
                        product.barcode = item.barcode;
                        product.note = item.note;
                        product.price = item.price;
                        product.import_price = item.import_price;
                        product.is_weigh = item.is_weigh;
                        product.is_promotion = item.is_promotion;
                        product.is_delete = item.is_delete;
                        product.purchase_id = model.id;
                        product_new.Add(product);
                    }
                    _context.Purchase_Product.AddRange(product_new);
                    _context.SaveChanges();

                    if (model.status_id == 1)
                    {
                        Warehouse_Receipt receipt = new Warehouse_Receipt
                        {
                            request_id = 0,
                            code = "RW-" + purchase.code,
                            content = purchase.content,
                            status_id = 0,
                            dateAdded = DateTime.Now,
                            userAdded = model.userUpdated ?? 0,
                            delivery_address = "",
                            note = model.note,
                            type = 1,
                            partner_id = model.partner_id,
                            warehouse_id = model.warehouse_id,
                            transfer_date = model.transfer_date,
                            is_delete = false

                        };
                        _context.Warehouse_Receipt.Add(receipt);
                        _context.SaveChanges();
                        List<Warehouse_Receipt_Product> listProudcts = new List<Warehouse_Receipt_Product>();
                        //check auto fill giá nhập và giá bán khi = 0
                        var listID = listProduct.Select(x => x.id).ToList();
                        var listProudctsWarehouse = await (from a in _context.Product_Warehouse
                                                           join b in _context.Purchase_Product on a.product_id equals b.product_id
                                                           where a.warehouse_id == model.warehouse_id & !b.is_delete & listID.Contains(b.id) & (b.import_price == 0 || b.price == 0)
                                                           select new
                                                           {
                                                               a.price,
                                                               a.import_price,
                                                               b.product_id,
                                                               b.id,
                                                               product_warehouse_id = a.id,
                                                               a.barcode,
                                                           }).ToListAsync();

                        foreach (var item in listProduct)
                        {
                            string barcode = item.barcode ?? string.Empty;
                            var updatePrice = listProudctsWarehouse.FirstOrDefault(e => e.id == item.id && !string.IsNullOrEmpty(e.barcode));
                            if (updatePrice is not null)
                            {
                                item.import_price = updatePrice.import_price;
                                item.price = updatePrice.price;
                                barcode = updatePrice.barcode;// auto fill barcode
                            }
                            Warehouse_Receipt_Product product = new Warehouse_Receipt_Product
                            {
                                import_price = item.import_price,
                                barcode = barcode,
                                batch_number = "",
                                category_packing_code = item.category_packing_code,
                                category_unit_code = item.category_unit_code,
                                exp_date = DateTime.Now,
                                is_weigh = false,
                                is_promotion = item.is_promotion,
                                note = item.note,
                                partner_id = model.partner_id,
                                price = item.price,
                                quantity = item.quantity,
                                receipt_id = receipt.id,
                                product_id = item.product_id,
                                warehouse_id = model.warehouse_id,
                                dateAdded = receipt.dateAdded,
                                dateUpdated = receipt.dateUpdated,
                                userAdded = receipt.userAdded,
                                weight = 0,
                                warning_date = 0,
                            };
                            listProudcts.Add(product);
                        }
                        _context.Warehouse_Receipt_Product.AddRange(listProudcts);
                    }
                    _context.SaveChanges();
                    //transaction.Commit();
                    return respone;
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();

                    respone = ex.Message + " - " + ex.StackTrace;
                    return respone;
                }

                //};
            });
        }
        public async Task<PaginationSet<PurchaseViewModel>> PurchaseList(string? keyword, byte? status_id, DateTime? start_date, DateTime? end_date, int page_size, int page_number)
        {
            return await Task.Run(async () =>
            {
                PaginationSet<PurchaseViewModel> response = new PaginationSet<PurchaseViewModel>();
                IQueryable<PurchaseViewModel> listItem = from a in _context.Purchase
                                                         join b in _context.Warehouse on a.warehouse_id equals b.id into ab
                                                         from bb in ab.DefaultIfEmpty()
                                                         join c in _context.Partner on a.partner_id equals c.id into ac
                                                         from cc in ac.DefaultIfEmpty()
                                                         where !a.is_delete
                                                         select new PurchaseViewModel
                                                         {
                                                             search_name = cc.search_name,
                                                             content = a.content,
                                                             transfer_date = a.transfer_date,
                                                             code = a.code,
                                                             partner_id = a.partner_id,
                                                             warehouse_name = bb.name,
                                                             partner_name = cc.name,
                                                             status_id = a.status_id,
                                                             warehouse_id = a.warehouse_id,
                                                             note = a.note,
                                                             id = a.id,
                                                             userAdded = a.userAdded,
                                                             total_amount = _context.Purchase_Product.Where(x => x.purchase_id == a.id & !x.is_delete).Sum(y => y.import_price * y.quantity)
                                                         };
                if (status_id is not null and not 10)
                {
                    listItem = listItem.Where(r => r.status_id == status_id);
                }
                if (keyword is not null and not "")
                {
                    keyword = ConvertText.RemoveUnicode(keyword);
                    listItem = listItem.Where(r => r.content.Contains(keyword) || r.search_name.Contains(keyword) || r.code.Contains(keyword));
                }
                if (start_date != null)
                {
                    DateTime start = start_date.Value.Date;
                    listItem = listItem.Where(r => r.transfer_date >= start_date);
                }
                if (end_date != null)
                {
                    var end = end_date.Value.Date.AddDays(1);
                    listItem = listItem.Where(r => end_date >= r.transfer_date);
                }
                if (page_number > 0)
                {
                    response.totalcount = listItem.Select(x => x.id).Count();
                    response.page = page_number;
                    response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / page_size);
                    response.lists = await listItem.OrderByDescending(r => r.id).Skip(page_size * (page_number - 1)).Take(page_size).ToListAsync();
                }
                else
                {
                    response.lists = listItem.OrderByDescending(r => r.id).ToList();
                }
                return response;
            });
        }
        public async Task<double> QuantityInventory(long warehouse_id, long product_id)
        {
            return await Task.Run(async () =>
            {
                var request = await _context.Product_Warehouse.FirstOrDefaultAsync(r => r.product_id == product_id && r.warehouse_id == warehouse_id);

                return request?.quantity_stock ?? 0;
            });
        }
    }
}
