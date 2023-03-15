using AutoMapper;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging.Abstractions;
using OfficeOpenXml;
using Point_of_Sale.Controllers;
using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Product;
using System.ComponentModel;
using System.Linq;
using static Humanizer.On;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace Point_of_Sale.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ProductModel> Product(long id)
        {

            var product = _context.Product.Where(r => r.id == id).FirstOrDefault();
            ProductModel model = _mapper.Map<ProductModel>(product);
            model.listDetail = _context.Product_Warehouse.Where(r => r.product_id == id).ToList();
            return model;

        }
        public async Task<PaginationSet<ProductViewModel>> ProductList(ProductSearchModel search)
        {
            return await Task.Run(() =>
            {

                PaginationSet<ProductViewModel> response = new();
                IEnumerable<ProductViewModel> listItem = from a in _context.Product
                                                         join b in _context.Category_Product on a.category_code equals b.code into ab
                                                         from ba in ab.DefaultIfEmpty()
                                                         join c in _context.Partner on a.partner_id equals c.id into ac
                                                         from ca in ac.DefaultIfEmpty()
                                                         where !a.is_delete
                                                         select new ProductViewModel
                                                         {
                                                             category_code = a.category_code,
                                                             search_name = a.search_name,
                                                             item_code = a.item_code,
                                                             name = a.name,
                                                             category_name = ba.name,
                                                             price = a.price,
                                                             packing_code = a.packing_code,
                                                             unit_code = a.unit_code,
                                                             barcode = "'"+a.barcode,
                                                             code = a.code,
                                                             is_active = a.is_active,
                                                             dateAdded = a.dateAdded,
                                                             note = a.note,
                                                             id = a.id,
                                                             partner_id = a.partner_id,
                                                             partner_name = ca.name,
                                                             userAdded = a.userAdded,
                                                             userUpdated = a.userUpdated,
                                                         };
                if (search.keyword != null && search.keyword != "")
                {
                    search.keyword = ConvertText.RemoveUnicode(search.keyword);
                    listItem = listItem.Where(r => r.search_name.Contains(search.keyword.ToLower()));
                }
                //if (search.category_code is not null and not "")
                //{
                //    listItem = listItem.Where(r => r.category_code == search.category_code || (r.item_code != null && r.item_code == search.category_code));
                //}
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
                return response;
            });
        }
        public async Task<PaginationSet<ProductWarehouseModel2>> ComboList(ProductSearchModel search)
        {
            return await Task.Run(() =>
            {
                PaginationSet<ProductWarehouseModel2> response = new();
                IEnumerable<ProductWarehouseModel2> listItem = from a in _context.Combo
                                                               join b in _context.Category_Product on a.category_code equals b.code
                                                               where !a.is_delete
                                                               select new ProductWarehouseModel2
                                                               {
                                                                   name = a.name,
                                                                   code = a.code,
                                                                   barcode = a.barcode,
                                                                   search_name = a.search_name,
                                                                   category_code = a.category_code,
                                                                   category_name = b.name,
                                                                   product_id = a.id,
                                                                   price = a.price,
                                                                   unit_code = a.unit_code,
                                                                   packing_code = a.packing_code,
                                                                   note = a.note,
                                                                   id = a.id,
                                                                   childs = (from r in _context.Product_Combo
                                                                             join t in _context.Product on r.product_id equals t.id
                                                                             where !r.is_delete && !t.is_delete && r.combo_id == a.id
                                                                             select new Product_Combo_Model
                                                                             {
                                                                                 id = r.id,
                                                                                 product_id = r.product_id,
                                                                                 combo_id = r.combo_id,
                                                                                 product_name = t.name,
                                                                                 packing_code = r.packing_code,
                                                                                 unit_code = r.unit_code,
                                                                                 product_quantity = r.product_quantity,
                                                                                 note = r.note,
                                                                             }).ToList()
                                                               };

                if (!string.IsNullOrEmpty(search.keyword))
                {
                    search.keyword = ConvertText.RemoveUnicode(search.keyword);
                    listItem = listItem.Where(r => r.search_name.Contains(search.keyword.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.category_code))
                {
                    listItem = listItem.Where(r => r.category_code == search.category_code || (r.code != null && r.code == search.category_code)).ToList();
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
                return response;
            });
        }

        public async Task<PaginationSet<ProductWarehouseModel>> ProductWarehouseList(string? keyword, int page_size, int page_number, long warehouse_id)
        {
            return await Task.Run(() =>
            {
                PaginationSet<ProductWarehouseModel> response = new PaginationSet<ProductWarehouseModel>();
                IEnumerable<ProductWarehouseModel> listItem = from a in _context.Product_Warehouse
                                                              join b in _context.Product on a.product_id equals b.id
                                                              join c in _context.Warehouse on a.warehouse_id equals c.id
                                                              where !a.is_delete && a.warehouse_id == warehouse_id & c.type != 2
                                                              select new ProductWarehouseModel
                                                              {
                                                                  search_name = b.search_name,
                                                                  name = b.name,
                                                                  barcode = a.barcode,
                                                                  product_id = a.product_id,
                                                                  quantity_sold = a.quantity_sold,
                                                                  quantity_stock = a.quantity_stock,
                                                                  import_price = a.import_price,
                                                                  price = a.price,
                                                                  sale_price = a.sale_price,
                                                                  unit_code = a.unit_code,
                                                                  packing_code = a.packing_code,
                                                                  exp_date = a.exp_date,
                                                                  warehouse_id = a.warehouse_id,
                                                                  id = a.id,
                                                                  batch_number = a.batch_number,
                                                                  userAdded = a.userAdded,
                                                                  userUpdated = a.userUpdated,
                                                              };
                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = ConvertText.RemoveUnicode(keyword);
                    listItem = listItem.Where(r => r.search_name.Contains(keyword.ToLower()));
                }
                if (page_number > 0)
                {
                    response.totalcount = listItem.Select(x => x.id).Count();
                    response.page = page_number;
                    response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / page_size);
                    response.lists = listItem.OrderByDescending(r => r.id).Skip(page_size * (page_number - 1)).Take(page_size).ToList();
                }
                else
                {
                    response.lists = listItem.OrderByDescending(r => r.id).ToList();
                }
                return response;
            });
        }
        public async Task<PaginationSet<ProductWarehouseModel2>> ProductWarehouseList2(string? keyword, int page_size, int page_number, long warehouse_id)
        {
            return await Task.Run(() =>
            {
                PaginationSet<ProductWarehouseModel2> response = new PaginationSet<ProductWarehouseModel2>();
                IEnumerable<ProductWarehouseModel2> list_warehouse_product = from a in _context.Product_Warehouse
                                                                             join b in _context.Product on a.product_id equals b.id
                                                                             join c in _context.Warehouse on a.warehouse_id equals c.id
                                                                             where !a.is_delete && a.warehouse_id == warehouse_id & c.type != 2
                                                                             select new ProductWarehouseModel2
                                                                             {
                                                                                 search_name = b.search_name,
                                                                                 name = b.name,
                                                                                 barcode = a.barcode,
                                                                                 product_id = a.product_id,
                                                                                 price = a.price,
                                                                                 sale_price = a.sale_price,
                                                                                 unit_code = a.unit_code,
                                                                                 packing_code = a.packing_code,
                                                                                 warehouse_id = a.warehouse_id,
                                                                                 id = a.id,
                                                                                 batch_number = a.batch_number,
                                                                             };
                IEnumerable<ProductWarehouseModel2> list_combo = from a in _context.Combo
                                                                 where !a.is_delete
                                                                 select new ProductWarehouseModel2
                                                                 {
                                                                     search_name = a.search_name,
                                                                     name = a.name,
                                                                     barcode = a.barcode,
                                                                     product_id = a.id,
                                                                     price = a.price,
                                                                     unit_code = a.unit_code,
                                                                     packing_code = a.packing_code,
                                                                     id = a.id,
                                                                     childs = (from r in _context.Product_Combo
                                                                               join t in _context.Product on r.product_id equals t.id
                                                                               where !r.is_delete && !t.is_delete && r.combo_id == a.id
                                                                               select new Product_Combo_Model
                                                                               {
                                                                                   id = r.id,
                                                                                   product_id = r.product_id,
                                                                                   combo_id = r.combo_id,
                                                                                   product_name = t.name,
                                                                                   packing_code = r.packing_code,
                                                                                   unit_code = r.unit_code,
                                                                                   product_quantity = r.product_quantity,
                                                                                   note = r.note,
                                                                               }).ToList()
                                                                 };

                List<ProductWarehouseModel2> list_product = new();
                list_product = list_warehouse_product.ToList();
                list_product.AddRange(list_combo.ToList());
                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = ConvertText.RemoveUnicode(keyword);
                    list_product = list_product.Where(r => r.search_name.Contains(keyword.ToLower())).ToList();
                }
                if (page_number > 0)
                {
                    response.totalcount = list_product.Select(x => x.id).Count();
                    response.page = page_number;
                    response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / page_size);
                    response.lists = list_product.OrderByDescending(r => r.id).Skip(page_size * (page_number - 1)).Take(page_size).ToList();
                }
                else
                {
                    response.lists = list_product.OrderByDescending(r => r.id).ToList();
                }
                return response;
            });
        }
        public async Task<PaginationSet<ProductWarehouseModel>> ProductStockList(string? keyword, int page_size, int page_number, long warehouse_id)
        {
            return await Task.Run(() =>
            {
                PaginationSet<ProductWarehouseModel> response = new PaginationSet<ProductWarehouseModel>();
                IEnumerable<ProductWarehouseModel> listItem = from a in _context.Product_Warehouse
                                                              join b in _context.Product on a.product_id equals b.id
                                                              join c in _context.Warehouse on a.warehouse_id equals c.id
                                                              where !a.is_delete && a.warehouse_id == warehouse_id & c.type == 1 && a.quantity_stock > 0
                                                              select new ProductWarehouseModel
                                                              {

                                                                  name = b.name,
                                                                  search_name = b.search_name,
                                                                  barcode = a.barcode,
                                                                  product_id = a.product_id,
                                                                  quantity_sold = a.quantity_sold,
                                                                  quantity_stock = a.quantity_stock,
                                                                  import_price = a.import_price,
                                                                  price = a.price,
                                                                  sale_price = a.sale_price,
                                                                  unit_code = a.unit_code,
                                                                  packing_code = a.packing_code,
                                                                  exp_date = a.exp_date,
                                                                  warehouse_id = a.warehouse_id,
                                                                  id = a.id,
                                                                  batch_number = a.batch_number,
                                                                  userAdded = a.userAdded,
                                                                  userUpdated = a.userUpdated,
                                                              };
                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = ConvertText.RemoveUnicode(keyword);
                    listItem = listItem.Where(r => r.search_name.Contains(keyword.ToLower()));
                }
                if (page_number > 0)
                {
                    response.totalcount = listItem.Select(x => x.id).Count();
                    response.page = page_number;
                    response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / page_size);
                    response.lists = listItem.OrderByDescending(r => r.id).Skip(page_size * (page_number - 1)).Take(page_size).ToList();
                }
                else
                {
                    response.lists = listItem.OrderByDescending(r => r.id).ToList();
                }
                return response;
            });
        }
        public async Task<PaginationSet<ProductRequestWarehouseModel>> ProductRequestWarehouseList(string? keyword, int page_size, int page_number, long warehouse_id)
        {
            return await Task.Run(() =>
            {
                PaginationSet<ProductRequestWarehouseModel> response = new PaginationSet<ProductRequestWarehouseModel>();
                IEnumerable<ProductRequestWarehouseModel> listItem = from a in _context.Product
                                                                     join b in _context.Product_Warehouse.Where(e => e.warehouse_id == warehouse_id && !e.is_delete) on a.id equals b.product_id into whh
                                                                     from wh in whh.DefaultIfEmpty()
                                                                     where !a.is_delete
                                                                     select new ProductRequestWarehouseModel
                                                                     {
                                                                         id = a.id,
                                                                         search_name = a.search_name,
                                                                         product_name = a.name ?? "",
                                                                         barcode = a.barcode ?? wh.barcode ?? "",
                                                                         product_id = a.id,
                                                                         quantity_stock = wh != null ? wh.quantity_stock : 0,
                                                                         price = a.price,
                                                                         unit_code = string.IsNullOrEmpty(a.unit_code) ? wh.unit_code : a.unit_code,
                                                                         packing_code = string.IsNullOrEmpty(a.packing_code) ? wh.packing_code : a.packing_code,
                                                                         warehouse_id = wh != null ? wh.warehouse_id : 0,
                                                                         product_warehouse_id = wh.id
                                                                     };

                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = ConvertText.RemoveUnicode(keyword);
                    listItem = listItem.Where(r => r.search_name.Contains(keyword.ToLower())).ToList();
                }
                if (page_number > 0)
                {
                    response.totalcount = listItem.Select(x => x.id).Count();
                    response.page = page_number;
                    response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / page_size);
                    response.lists = listItem.OrderByDescending(r => r.id).Skip(page_size * (page_number - 1)).Take(page_size).ToList();
                }
                else
                {
                    response.lists = listItem.OrderByDescending(r => r.id).ToList();
                }
                return response;
            });
        }
        public async Task<List<ProductRequestWarehouseModel2>> ProductRequestWarehouseList2(long partner_id, long warehouse_id)
        {
            return await Task.Run(() =>
            {
                List<ProductRequestWarehouseModel2> response = new();
                IEnumerable<ProductRequestWarehouseModel2> listItem = from a in _context.Product_Partner
                                                                          //join b in _context.Product_Warehouse on a.product_id equals b.product_id
                                                                      join c in _context.Product on a.product_id equals c.id
                                                                      where !a.is_delete && a.partner_id == partner_id
                                                                      select new ProductRequestWarehouseModel2
                                                                      {
                                                                          id = a.id,
                                                                          product_id = a.product_id,
                                                                          price = c.price,
                                                                          product_name = c.name,
                                                                          barcode = c.barcode,
                                                                          import_price = a.price,
                                                                          unit_code = a.unit_code,
                                                                          packing_code = a.packing_code,
                                                                          partner_id = a.partner_id,
                                                                      };

                response = listItem.OrderByDescending(r => r.id).ToList();
                if (response.Count() > 0)
                {
                    List<long> product_ids = response.Select(x => x.product_id).ToList();
                    List<Product_Warehouse> list_product = _context.Product_Warehouse.Where(x => product_ids.Contains(x.id) && !x.is_delete && x.warehouse_id == warehouse_id).ToList();
                    foreach (var item in response)
                    {
                        Product_Warehouse product = list_product.FirstOrDefault(x => x.product_id == item.product_id);
                        if (product != null)
                        {
                            item.quantity_stock = product.quantity_stock;
                            item.barcode = product.barcode;
                            item.price = product.price;
                            item.warehouse_id = product.warehouse_id;
                            item.product_warehouse_id = product.id;
                        }
                    }
                }
                return response;
            });
        }
        public async Task<ProductWarehouseModel> ProductWarehouse(string barcode, long warehouse_id)
        {
            return await Task.Run(async () =>
            {
                var item = await (from a in _context.Product_Warehouse
                                  join b in _context.Product on a.product_id equals b.id
                                  where !a.is_delete && (a.barcode == barcode) && a.warehouse_id == warehouse_id //& !a.is_promotion
                                  select new ProductWarehouseModel
                                  {
                                      name = b.name,
                                      barcode = a.barcode,
                                      product_id = a.product_id,
                                      quantity_sold = a.quantity_sold,
                                      quantity_stock = a.quantity_stock,
                                      price = a.price,
                                      sale_price = a.sale_price,
                                      unit_code = a.unit_code,
                                      packing_code = a.packing_code,
                                      exp_date = a.exp_date,
                                      warehouse_id = a.warehouse_id,
                                      id = a.id,
                                      batch_number = a.batch_number,
                                      userAdded = a.userAdded,
                                      userUpdated = a.userUpdated,
                                  }).FirstOrDefaultAsync();
                return item ?? new ProductWarehouseModel();
            });
        }
        public async Task<Product> ProductCreate(Product product)
        {
            return await Task.Run(async () =>
            {
                product.dateAdded = DateTime.Now;
                product.search_name = ConvertText.RemoveUnicode(product.name.ToLower()) + "-" + ConvertText.RemoveUnicode(product.code ?? "") + "-" + ConvertText.RemoveUnicode(product.barcode ?? "");
                _context.Product.Add(product);
                _context.SaveChanges();
                return product;
            });
        }
        public async Task<bool> ProductDelete(long product_id, long user_id)
        {
            return await Task.Run(() =>
            {
                var model = _context.Product.Where(r => r.id == product_id).FirstOrDefault();
                if (model == null || model.id == 0)
                {
                    return Task.FromResult(false);
                }
                else
                {
                    model.userUpdated = user_id;
                    model.dateUpdated = DateTime.Now;
                    model.is_delete = true;
                    _context.Product.Update(model);
                }
                _context.SaveChanges();
                return Task.FromResult(true);
            });
        }
        public async Task<Product> ProductModify(Product product)
        {
            return await Task.Run(() =>
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    List<Product_Warehouse_Price_History> histories = new List<Product_Warehouse_Price_History>();
                    List<Product_Warehouse> productswarehouse = _context.Product_Warehouse.Where(r => r.product_id == product.id && r.price != product.price && r.quantity_stock > 0 && !r.is_delete).ToList();
                    if (productswarehouse.Count() > 0)
                    {
                        foreach (Product_Warehouse item in productswarehouse)
                        {
                            Product_Warehouse_Price_History history = new Product_Warehouse_Price_History
                            {
                                product_id = item.product_id,
                                sale_price = item.sale_price,
                                sale_price_old = item.price,
                                dateAdded = DateTime.Now,
                                userAdded = product.userUpdated ?? 0,
                                import_price = item.import_price,
                                price_old = item.price,
                                price = product.price,
                                import_price_old = item.import_price,
                                product_warehouse_id = item.id
                            };
                            histories.Add(history);
                            item.price = product.price;

                        }
                        _context.Product_Warehouse_Price_History.AddRangeAsync(histories);
                        _context.Product_Warehouse.UpdateRange(productswarehouse);
                    }
                    product.dateUpdated = DateTime.Now;
                    product.search_name = ConvertText.RemoveUnicode(product.name.ToLower()) + "-" + ConvertText.RemoveUnicode(product.code ?? "") + "-" + ConvertText.RemoveUnicode(product.barcode ?? "");

                    _context.Product.Update(product);
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    throw;
                }

                return Task.FromResult(product);
            });
        }
        public async Task<bool> ProductCheckDuplicate(Product product)
        {
            return await Task.Run(async () =>
            {

                return string.IsNullOrEmpty(product.name)
                    ? false
                    : (await _context.Product.FirstOrDefaultAsync(e => (product.name == e.name || product.barcode == e.barcode) && e.id != product.id && !e.is_delete)) == null;
            });
        }
        public async Task<Product_Warehouse> ProductWarehouseModify(Product_Warehouse model)
        {
            return await Task.Run(async () =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var notUpdateBarcode = await _context.Product_Warehouse.AsNoTracking().FirstOrDefaultAsync(e => e.id == model.id && e.barcode == model.barcode);

                        if (notUpdateBarcode == null)
                        {
                            var checkDuplicate = await _context.Product_Warehouse.AsNoTracking().FirstOrDefaultAsync(e => e.barcode == model.barcode && !e.is_delete);
                            if (checkDuplicate == null)
                            {
                                await CheckChangePrice(model);
                                model.dateUpdated = DateTime.Now;
                                _context.Product_Warehouse.Update(model);
                                _context.SaveChanges();
                                transaction.Commit();
                                return model;
                            }
                            return new Product_Warehouse();
                        }
                        await CheckChangePrice(model);
                        model.dateUpdated = DateTime.Now;
                        _context.Product_Warehouse.Update(model);
                        _context.SaveChanges();
                        transaction.Commit();
                        return model;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new Product_Warehouse();
                    }
                }
            });
        }
        private async Task<bool> CheckChangePrice(Product_Warehouse model)
        {
            return await Task.Run(async () =>
            {
                bool check = false;
                var database = await _context.Product_Warehouse.AsNoTracking().FirstOrDefaultAsync(e => e.id == model.id);
                if (database != null)
                    if (database.price != model.price || database.import_price != model.import_price || database.sale_price != model.sale_price)
                    {
                        check = true;
                        _context.Product_Warehouse_Price_History.Add(new Product_Warehouse_Price_History
                        {
                            sale_price = model.sale_price,
                            price = model.price,
                            import_price = model.import_price,
                            product_id = model.product_id,
                            product_warehouse_id = model.id,
                            sale_price_old = database.sale_price,
                            import_price_old = database.import_price,
                            price_old = database.price,
                            dateAdded = DateTime.Now,
                            userAdded = model.userUpdated ?? model.userAdded,
                        });
                        _context.SaveChanges();
                    }
                return check;
            });
        }
        public async Task<string> ProductWarehouseModifyPrintBarcode(List<long> model)
        {
            return await Task.Run(async () =>
            {
                string respons = "0";
                try
                {
                    foreach (var item in model)
                    {
                        if (item != 0)
                        {
                            Product_Warehouse product = await _context.Product_Warehouse.FirstOrDefaultAsync(e => e.id == item);
                            product.print_bacode = product.print_bacode == null ? 1 : product.print_bacode + 1;
                            _context.Product_Warehouse.Update(product);
                        }
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {

                    respons = ex.Message;
                }
                return respons;
            });
        }
        public async Task<string> ProductWarehouseModifyPrintPrice(List<long> model)
        {
            return await Task.Run(async () =>
            {
                string respons = "0";
                try
                {
                    foreach (var product in from item in model
                                            where item != 0
                                            let product = _context.Product_Warehouse.FirstOrDefault(e => e.id == item)
                                            select product)
                    {
                        product.is_printed = true;
                        _context.Product_Warehouse.Update(product);
                    }

                    _context.SaveChanges();
                }
                catch (Exception ex)
                {

                    respons = ex.Message;
                }
                return respons;
            });
        }
        public async Task<Product_Warehouse> ProductWarehouseGetById(long id)
        {
            return await Task.Run(async () =>
            {
                var model = await _context.Product_Warehouse.FirstOrDefaultAsync(r => r.id == id);

                return model;
            });
        }
        public async Task<PaginationSet<ProductWarehouseModel>> ProductDetailWarehouseList(ProductSearchModel search)
        {
            return await Task.Run(async () =>
            {
                PaginationSet<ProductWarehouseModel> response = new();
                IQueryable<ProductWarehouseModel> listItem = from a in _context.Product_Warehouse
                                                             join b in _context.Product on a.product_id equals b.id
                                                             join d in _context.Warehouse on a.warehouse_id equals d.id
                                                             join c in _context.Category_Product on b.category_code equals c.code into bc
                                                             from cc in bc.DefaultIfEmpty()
                                                             where !a.is_delete & d.type != 2/* && a.quantity_stock > 0 */
                                                             select new ProductWarehouseModel
                                                             {
                                                                 name = b.name,
                                                                 barcode = a.barcode,
                                                                 product_id = a.product_id,
                                                                 quantity_sold = a.quantity_sold,
                                                                 quantity_stock = a.quantity_stock,
                                                                 price = a.price,
                                                                 import_price = a.import_price,
                                                                 sale_price = a.sale_price,
                                                                 unit_code = a.unit_code,
                                                                 category_code = b.category_code,
                                                                 category_product_name = cc.name,
                                                                 packing_code = a.packing_code,
                                                                 exp_date = a.exp_date,
                                                                 //warning_date = a.warning_date,
                                                                 warehouse_id = a.warehouse_id,
                                                                 id = a.id,
                                                                 batch_number = a.batch_number,
                                                                 userAdded = a.userAdded,
                                                                 userUpdated = a.userUpdated,
                                                                 is_printed = a.is_printed,
                                                                 is_weigh = a.is_weigh,
                                                                 is_promotion = a.is_promotion,
                                                                 print_bacode = a.print_bacode,
                                                             };
                if (search.is_scale != null)
                {
                    listItem = listItem.Where(r => r.is_weigh == search.is_scale);
                }
                if (search.start_date != null)
                {
                    DateTime start = search.start_date.Value.Date;
                    listItem = listItem.Where(r => r.exp_date >= start);
                }
                if (search.end_date != null)
                {
                    var end = search.end_date.Value.Date.AddDays(1);
                    listItem = listItem.Where(r => end >= r.exp_date);
                }
                if (search.status_price > 0)
                {
                    if (search.status_price == 1)
                        listItem = listItem.Where(r => r.price == 0);
                    if (search.status_price == 2)
                        listItem = listItem.Where(r => r.price > 0);
                }
                if (search.keyword is not null and not "")
                {
                    listItem = listItem.Where(r => r.name.Contains(search.keyword) || r.barcode.Contains(search.keyword));
                }
                if (search.warehouse_id > 0)
                    listItem = listItem.Where(r => r.warehouse_id == search.warehouse_id);
                //if (search.partner_id > 0)
                //listItem = listItem.Where(r => r.partner_id == search.partner_id);
                if (search.category_code is not null and not "")
                    listItem = listItem.Where(r => r.category_code == search.category_code);
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
                List<long> product_ids = response.lists.Select(x => x.product_id).Distinct().ToList();
                //them ten ncc 
                IQueryable<Warehouse_Receipt_View> querry_partner = from a in _context.Warehouse_Receipt
                                                                    join b in _context.Warehouse_Receipt_Product on a.id equals b.receipt_id
                                                                    join c in _context.Partner on a.partner_id equals c.id
                                                                    where product_ids.Contains(b.product_id) && !a.is_delete && a.status_id == 1
                                                                    select new Warehouse_Receipt_View
                                                                    {
                                                                        id = b.id,
                                                                        product_id = b.product_id,
                                                                        partner_id = a.partner_id,
                                                                        partner_name = c.name
                                                                    };
                List<Warehouse_Receipt_View> list_product_partner = await querry_partner.OrderByDescending(x => x.id).ToListAsync();

                //them ten category
                List<Category_Packing> packings = _context.Category_Packing.Where(r => !r.is_delete).ToList();
                List<Category_Unit> units = _context.Category_Unit.Where(r => !r.is_delete).ToList();
                foreach (var item in response.lists)
                {
                    Category_Packing packing = packings.FirstOrDefault(r => r.code == item.packing_code);
                    if (packing != null)
                        item.packing_name = packing.name;
                    Category_Unit unit = units.FirstOrDefault(r => r.code == item.unit_code);
                    if (unit != null)
                        item.unit_name = unit.name;
                    Warehouse_Receipt_View product_partner = list_product_partner.FirstOrDefault(r => r.product_id == item.product_id);
                    if (product_partner != null)
                    {
                        item.partner_id = product_partner.partner_id;
                        item.partner_name = product_partner.partner_name;
                    }

                }
                return response;
            });
        }
        public async Task UpdateFromExcel(Stream file)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(file))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["Sheet1"];
                List<ProductWarehouseModel> listItems = new();
                for (int row = 2; ; row++)
                {

                    if (worksheet.Cells["A" + row].Value == null)
                    {
                        break;
                    }
                    ProductWarehouseModel item = new();
                    item.id = long.Parse(worksheet.Cells["A" + row].Value.ToString());
                    item.product_id = long.Parse(worksheet.Cells["C" + row].Value.ToString());
                    item.quantity_stock = double.Parse(worksheet.Cells["E" + row].Value.ToString());
                    item.barcode = worksheet.Cells["S" + row].Value.ToString();
                    listItems.Add(item);
                    List<long> ids = listItems.Select(x => x.id).ToList();
                    List<Product_Warehouse> product_Warehouses = _context.Product_Warehouse.Where(x => ids.Contains(x.id)).ToList();
                    foreach (var product in listItems)
                    {
                        Product_Warehouse productdb = product_Warehouses.FirstOrDefault(x => x.id == product.id && x.product_id == product.product_id);
                        if (productdb != null)
                        {
                            productdb.quantity_stock = product.quantity_stock;
                            productdb.barcode = product.barcode;
                        }
                    }
                    _context.Product_Warehouse.UpdateRange(product_Warehouses);
                    _context.SaveChanges();
                }
            }

        }
        public async Task<PaginationSet<WarehousePriceHistoryModel>> GetChangePriceHistory(SearchWarehousePriceHistory search)
        {
            return await Task.Run(async () =>
            {
                PaginationSet<WarehousePriceHistoryModel> response = new();
                IQueryable<WarehousePriceHistoryModel> listItem = from a in _context.Product_Warehouse_Price_History
                                                                  join b in _context.Product on a.product_id equals b.id
                                                                  join c in _context.Product_Warehouse on a.product_warehouse_id equals c.id
                                                                  join d in _context.Warehouse on c.warehouse_id equals d.id
                                                                  join e in _context.Admin_User on a.userAdded equals e.id
                                                                  where !a.is_delete/* && a.quantity_stock > 0 */
                                                                  select new WarehousePriceHistoryModel
                                                                  {
                                                                      id = a.id,
                                                                      product_warehouse_id = a.product_warehouse_id,
                                                                      product_name = b.name ?? string.Empty,
                                                                      barcode = c.barcode,
                                                                      product_id = a.product_id,
                                                                      unit_code = c.unit_code,
                                                                      warehouse_id = c.warehouse_id,
                                                                      warehouse_name = d.name,
                                                                      userAdded = a.userAdded,
                                                                      userUpdated = a.userUpdated,
                                                                      price = a.price,
                                                                      import_price = a.import_price,
                                                                      sale_price = a.sale_price,
                                                                      import_price_old = a.import_price_old,
                                                                      dateAdded = a.dateAdded,
                                                                      dateUpdated = a.dateUpdated,
                                                                      price_old = a.price_old,
                                                                      sale_price_old = a.sale_price_old,
                                                                      user_change = e.full_name
                                                                  };

                if (search.start_date != null)
                {
                    DateTime start = search.start_date.Value.Date;
                    listItem = listItem.Where(r => r.dateAdded >= start);
                }
                if (search.end_date != null)
                {
                    var end = search.end_date.Value.Date.AddDays(1);
                    listItem = listItem.Where(r => end >= r.dateAdded);
                }

                if (search.keyword is not null and not "")
                {
                    listItem = listItem.Where(r => r.product_name.Contains(search.keyword) || r.barcode.Contains(search.keyword));
                }
                if (search.warehouse_id > 0)
                    listItem = listItem.Where(r => r.warehouse_id == search.warehouse_id);
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
        public async Task<PaginationSet<ProductWarehouseModel>> ProductWarehousePromotionList(SearchPromotionModel search)
        {
            return await Task.Run(() =>
            {
                PaginationSet<ProductWarehouseModel> response = new PaginationSet<ProductWarehouseModel>();
                IEnumerable<ProductWarehouseModel> listItem = from a in _context.Product_Warehouse
                                                              join b in _context.Product on a.product_id equals b.id
                                                              join c in _context.Warehouse on a.warehouse_id equals c.id
                                                              where !a.is_delete && a.warehouse_id == search.warehouse_id & c.type != 2 & a.is_promotion == search.is_promotion
                                                              select new ProductWarehouseModel
                                                              {
                                                                  name = b.name,
                                                                  barcode = a.barcode,
                                                                  product_id = a.product_id,
                                                                  quantity_sold = a.quantity_sold,
                                                                  quantity_stock = a.quantity_stock,
                                                                  import_price = a.import_price,
                                                                  price = a.price,
                                                                  sale_price = a.sale_price,
                                                                  unit_code = a.unit_code,
                                                                  packing_code = a.packing_code,
                                                                  exp_date = a.exp_date,
                                                                  //warning_date = a.warning_date,
                                                                  warehouse_id = a.warehouse_id,
                                                                  id = a.id,
                                                                  batch_number = a.batch_number,
                                                                  userAdded = a.userAdded,
                                                                  userUpdated = a.userUpdated,
                                                              };
                if (!string.IsNullOrEmpty(search.keyword))
                {
                    listItem = listItem.Where(r => r.name.Contains(search.keyword)
                    || r.barcode.Contains(search.keyword)
                    || r.name.ToLower().Contains(search.keyword.ToLower())
                    );
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
                return response;
            });
        }

        // product + NCC
        public async Task<Product_Partner_Model> Product_Partner_Create(Product_Partner_Model model)
        {
            return await Task.Run(async () =>
            {
                Product_Partner product_db = _context.Product_Partner.FirstOrDefault(x => x.product_id == model.product_id && x.partner_id == model.partner_id && !x.is_delete);
                if (product_db != null)
                {
                    return null;
                }
                Product_Partner product = _mapper.Map<Product_Partner>(model);
                product.dateAdded = DateTime.Now;
                _context.Product_Partner.Add(product);
                _context.SaveChanges();
                model = _mapper.Map<Product_Partner_Model>(product);
                return model;
            });
        }
        public async Task<Product_Partner_Model> Product_Partner_Update(Product_Partner_Model model)
        {
            return await Task.Run(async () =>
            {
                Product_Partner product_db = _context.Product_Partner.FirstOrDefault(x => x.product_id == model.product_id && x.partner_id == model.partner_id && !x.is_delete && x.id != model.id);
                if (product_db != null)
                {
                    return null;
                }
                Product_Partner product = _mapper.Map<Product_Partner>(model);
                product.dateUpdated = DateTime.Now;
                _context.Product_Partner.Update(product);
                _context.SaveChanges();
                return model;
            });
        }
        public async Task<List<Product_Partner_Model>> Product_Partner_List_By_Partner(long partner_id, string keyword = "")
        {
            return await Task.Run(async () =>
            {
                List<Product_Partner_Model> product_Partner_Models = new();
                IQueryable<Product_Partner_Model> listItem = from a in _context.Product_Partner
                                                             join b in _context.Product on a.product_id equals b.id
                                                             //join c in _context.Product_Partner on b.id equals c.product_id
                                                             where !a.is_delete && !b.is_delete && a.partner_id == partner_id
                                                             select new Product_Partner_Model
                                                             {
                                                                 id = a.id,
                                                                 product_name = b.name,
                                                                 product_id = a.product_id,
                                                                 partner_id = a.partner_id,
                                                                 unit_code = a.unit_code,
                                                                 packing_code = a.packing_code,
                                                                 note = a.note,
                                                                 price = a.price,
                                                             };
                if (keyword != null && keyword != "")
                {
                    listItem = listItem.Where(x => x.product_name.Contains(keyword));
                }
                product_Partner_Models = await listItem.OrderByDescending(r => r.id).ToListAsync();
                return product_Partner_Models;
            });
        }
        public async Task<List<Product_Partner_Request_Model>> Product_Partner_List_By_Partner_2(long partner_id, DateTime start_date/*, DateTime end_date*/)
        {
            return await Task.Run(async () =>
            {
                List<Product_Partner_Request_Model> product_Partner_Models = new();
                IQueryable<Product_Partner_Request_Model> listItem = from a in _context.Product_Warehouse
                                                                     join b in _context.Product on a.product_id equals b.id
                                                                     join c in _context.Product_Partner on b.id equals c.product_id
                                                                     where !a.is_delete && !b.is_delete && !c.is_delete && c.partner_id == partner_id
                                                                     select new Product_Partner_Request_Model
                                                                     {
                                                                         id = c.id,
                                                                         product_name = b.name,
                                                                         product_id = a.product_id,
                                                                         partner_id = c.partner_id,
                                                                         unit_code = c.unit_code,
                                                                         packing_code = c.packing_code,
                                                                         note = c.note,
                                                                         price = c.price,
                                                                     };

                product_Partner_Models = await listItem.OrderByDescending(r => r.id).ToListAsync();
                List<long> product_ids = product_Partner_Models.Select(x => x.product_id).ToList();
                if (product_ids.Count() > 0)
                {
                    List<Product_Warehouse_History> product_Warehouse_Histories = _context.Product_Warehouse_History.Where(x => product_ids.Contains(x.product_id) && x.dateAdded >= start_date.AddDays(-1)).ToList();
                }
                return product_Partner_Models;
            });
        }

        public async Task<List<Product_Partner_Model>> Product_Partner_List_By_Product(long product_id)
        {
            return await Task.Run(async () =>
            {
                List<Product_Partner_Model> product_Partner_Models = new List<Product_Partner_Model>();
                IQueryable<Product_Partner_Model> listItem = from a in _context.Product_Partner
                                                             join b in _context.Partner on a.partner_id equals b.id
                                                             where !a.is_delete && !b.is_delete && a.product_id == product_id
                                                             select new Product_Partner_Model
                                                             {
                                                                 id = a.id,
                                                                 partner_name = b.name,
                                                                 product_id = a.product_id,
                                                                 unit_code = a.unit_code,
                                                                 packing_code = a.packing_code,
                                                                 note = a.note,
                                                                 partner_id = a.partner_id,
                                                                 price = a.price,
                                                             };

                product_Partner_Models = await listItem.OrderByDescending(r => r.id).ToListAsync();
                return product_Partner_Models;
            });
        }

        // Product + combo
        public async Task<Combo_Model> ComboCreate(Combo_Model model)
        {
            return await Task.Run(async () =>
            {
                var combo_db = _context.Combo.FirstOrDefault(x => x.name == model.name || x.barcode == model.barcode && !x.is_delete);
                var product_db = _context.Product.FirstOrDefault(x => x.name == model.name || x.barcode == model.barcode && !x.is_delete);
                if (product_db != null || combo_db! != null)
                {
                    return null;
                }
                Combo combo = _mapper.Map<Combo>(model);
                combo.dateAdded = DateTime.Now;
                combo.search_name = ConvertText.RemoveUnicode(combo.name.ToLower()) + "-" + ConvertText.RemoveUnicode(combo.code ?? "") + "-" + ConvertText.RemoveUnicode(combo.barcode ?? "");
                _context.Combo.Add(combo);
                _context.SaveChanges();
                List<Product_Combo> list_child = new();
                foreach (var item in model.childs)
                {
                    Product_Combo product = _mapper.Map<Product_Combo>(item);
                    product.dateAdded = DateTime.Now;
                    product.userAdded = model.userAdded;
                    product.combo_id = combo.id;
                    list_child.Add(product);
                }
                if (list_child.Count() > 0)
                {
                    _context.Product_Combo.AddRange(list_child);
                }

                _context.SaveChanges();
                model.id = combo.id;
                return model;
            });
        }
        public async Task<Combo_Model> ComboUpdate(Combo_Model model)
        {
            return await Task.Run(async () =>
            {
                var combo_db = _context.Combo.AsNoTracking().FirstOrDefault(x => x.name == model.name || x.barcode == model.barcode && !x.is_delete && x.id != model.id);
                var product_db = _context.Product.AsNoTracking().FirstOrDefault(x => x.name == model.name || x.barcode == model.barcode && !x.is_delete && x.id != model.id);
                if (product_db != null || combo_db! != null)
                {
                    return null;
                }
                Combo combo = _mapper.Map<Combo>(model);
                combo.dateUpdated = DateTime.Now;
                combo.userUpdated = model.userUpdated;
                combo.search_name = ConvertText.RemoveUnicode(combo.name.ToLower()) + "-" + ConvertText.RemoveUnicode(combo.code ?? "") + "-" + ConvertText.RemoveUnicode(combo.barcode ?? "");
                _context.Combo.Update(combo);
                List<Product_Combo> list_child_new = new();
                List<Product_Combo> list_child_update = new();
                foreach (var item in model.childs)
                {
                    Product_Combo product = _mapper.Map<Product_Combo>(item);
                    if (item.id == 0)
                    {
                        product.dateAdded = DateTime.Now;
                        product.userAdded = model.userAdded;
                        product.combo_id = combo.id;
                        list_child_new.Add(product);
                    }
                    else
                    {
                        list_child_update.Add(product);
                    }
                }
                if (list_child_new.Count() > 0)
                {
                    _context.Product_Combo.AddRange(list_child_new);
                }
                if (list_child_update.Count() > 0)
                {
                    _context.Product_Combo.UpdateRange(list_child_update);
                }

                _context.SaveChanges();
                return model;
            });
        }
        public async Task<Combo_Model> Combo_Detail(long combo_id)
        {
            Combo_Model model = new Combo_Model();
            Combo combo = _context.Combo.Where(r => r.id == combo_id && !r.is_delete).FirstOrDefault();
            if (combo == null)
            {
                return null;
            }
            model = _mapper.Map<Combo_Model>(combo);
            //List < Product_Combo_Model > lis = new();
            IQueryable<Product_Combo_Model> list_child = from r in _context.Product_Combo
                                                         join t in _context.Product on r.product_id equals t.id
                                                         where !r.is_delete && !t.is_delete && r.combo_id == combo_id
                                                         select new Product_Combo_Model
                                                         {
                                                             id = r.id,
                                                             product_id = r.product_id,
                                                             combo_id = r.combo_id,
                                                             product_name = t.name,
                                                             packing_code = r.packing_code,
                                                             unit_code = r.unit_code,
                                                             product_quantity = r.product_quantity,
                                                             note = r.note,
                                                         };
            model.childs = list_child.ToList();

            return model;
        }


        public async Task<bool> Sync_Non_Unicode()
        {
            List<Product> products = await _context.Product.Where(x => !x.is_delete).ToListAsync();
            foreach (var item in products)
            {
                item.search_name = ConvertText.RemoveUnicode(item.name.ToLower()) + "-" + ConvertText.RemoveUnicode(item.code ?? "") + "-" + ConvertText.RemoveUnicode(item.barcode ?? "");
            }
            List<Combo> combos = await _context.Combo.Where(x => !x.is_delete).ToListAsync();
            foreach (var item in combos)
            {
                item.search_name = ConvertText.RemoveUnicode(item.name.ToLower()) + "-" + ConvertText.RemoveUnicode(item.code ?? "") + "-" + ConvertText.RemoveUnicode(item.barcode ?? "");
            }

            List<Warehouse> warehouses = await _context.Warehouse.Where(x => !x.is_delete).ToListAsync();
            foreach (var item in warehouses)
            {
                item.search_name = ConvertText.RemoveUnicode(item.name.ToLower()) + "-" + ConvertText.RemoveUnicode(item.code ?? "");
            }
            List<Partner> partners = await _context.Partner.Where(x => !x.is_delete).ToListAsync();
            foreach (var item in partners)
            {
                item.search_name = ConvertText.RemoveUnicode(item.name.ToLower()) + "-" + ConvertText.RemoveUnicode(item.code ?? "");
            }
            List<Category_Packing> Category_Packings = await _context.Category_Packing.Where(x => !x.is_delete).ToListAsync();
            foreach (var item in Category_Packings)
            {
                item.search_name = ConvertText.RemoveUnicode(item.name.ToLower()) + "-" + ConvertText.RemoveUnicode(item.code ?? "");
            }
            List<Category_Stalls> Category_Stallss = await _context.Category_Stalls.Where(x => !x.is_delete).ToListAsync();
            foreach (var item in Category_Stallss)
            {
                item.search_name = ConvertText.RemoveUnicode(item.name.ToLower()) + "-" + ConvertText.RemoveUnicode(item.code ?? "");
            }
            List<Category_Product> Category_products = await _context.Category_Product.Where(x => !x.is_delete).ToListAsync();
            foreach (var item in Category_products)
            {
                item.search_name = ConvertText.RemoveUnicode(item.name.ToLower()) + "-" + ConvertText.RemoveUnicode(item.code ?? "");
            }
            List<Category_Unit> Category_Units = await _context.Category_Unit.Where(x => !x.is_delete).ToListAsync();
            foreach (var item in Category_Units)
            {
                item.search_name = ConvertText.RemoveUnicode(item.name.ToLower()) + "-" + ConvertText.RemoveUnicode(item.code ?? "");
            }
            List<Category_Group> Category_Groups = await _context.Category_Group.Where(x => !x.is_delete).ToListAsync();
            foreach (var item in Category_Groups)
            {
                item.search_name = ConvertText.RemoveUnicode(item.name.ToLower()) + "-" + ConvertText.RemoveUnicode(item.code ?? "");
            }
            _context.Category_Group.UpdateRange(Category_Groups);
            _context.Category_Unit.UpdateRange(Category_Units);
            _context.Category_Packing.UpdateRange(Category_Packings);
            _context.Category_Stalls.UpdateRange(Category_Stallss);
            _context.Category_Product.UpdateRange(Category_products);
            _context.Combo.UpdateRange(combos);
            _context.Product.UpdateRange(products);
            _context.Warehouse.UpdateRange(warehouses);
            _context.Partner.UpdateRange(partners);
            _context.SaveChanges();
            return true;
        }
    }
}
