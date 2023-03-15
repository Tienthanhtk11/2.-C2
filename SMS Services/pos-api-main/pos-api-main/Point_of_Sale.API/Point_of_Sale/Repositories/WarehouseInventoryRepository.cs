using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model.Inventory;
using Point_of_Sale.Model;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace Point_of_Sale.Repositories
{
    internal class WarehouseInventoryRepository : IWarehouseInventoryRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public WarehouseInventoryRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<Warehouse_Inventory_Model> Warehouse_InventoryDetail(long id)
        {
            return await Task.Run(async () =>
            {
                Warehouse_Inventory_Model model = await (from a in _context.Warehouse_Inventory
                                                         join b in _context.Warehouse on a.warehouse_id equals b.id
                                                         join c in _context.Admin_User on a.userAdded equals c.id
                                                         where a.id == id & !a.is_delete
                                                         select new Warehouse_Inventory_Model
                                                         {
                                                             id = a.id,
                                                             warehouse_id = b.id,
                                                             code = a.code,
                                                             content = a.content,
                                                             dateAdded = a.dateAdded,
                                                             dateUpdated = a.dateUpdated,
                                                             status_id = a.status_id,
                                                             userAdded = a.userAdded,
                                                             user_name = c.full_name,
                                                             warehouse_name = b.name,
                                                             inventory_date = a.inventory_date
                                                         }).FirstOrDefaultAsync() ?? new Warehouse_Inventory_Model();
                model.products = await (from a in _context.Warehouse_Inventory_Product
                                        join b in _context.Product on a.product_id equals b.id
                                        where a.warehouse_inventory_id == id & !a.is_delete
                                        select new Warehouse_Inventory_Product_Model
                                        {
                                            name = b.name,
                                            product_id = a.product_id,
                                            note = a.note,
                                            import_price = a.import_price,
                                            warehouse_inventory_id = a.warehouse_inventory_id,
                                            price = a.price,
                                            quantity = a.quantity,
                                            barcode = a.barcode,
                                            quantity_stock = a.quantity_stock,
                                            unit_code = a.unit_code,
                                            id = a.id,
                                            batch_number = a.batch_number,
                                            is_weigh = a.is_weigh ?? false,
                                            packing_code = a.packing_code,
                                            products_warehouse_id = a.products_warehouse_id
                                        }).ToListAsync();
                return model;
            });
        }
        public async Task<Warehouse_Inventory_Print_Model> Warehouse_InventoryPrint(long id)
        {
            return await Task.Run(async () =>
            {

                var model = await (from a in _context.Warehouse_Inventory
                                   join c in _context.Admin_User on a.userAdded equals c.id
                                   where a.id == id & !a.is_delete
                                   select new Warehouse_Inventory_Print_Model
                                   {
                                       id = a.id,
                                       code = a.code,
                                       content = a.content,
                                       inventory_date = a.inventory_date,
                                       type = a.type,
                                       user_name = c.full_name,
                                       warehouse_id = a.warehouse_id,
                                   }).FirstOrDefaultAsync();

                model.products = await (from a in _context.Warehouse_Inventory_Product
                                        join b in _context.Product on a.product_id equals b.id
                                        where a.id == id & !a.is_delete
                                        select new Warehouse_Inventory_Product_Model
                                        {
                                            name = b.name,
                                            product_id = a.product_id,
                                            note = a.note,
                                            import_price = a.import_price,
                                            warehouse_inventory_id = a.warehouse_inventory_id,
                                            price = a.price,
                                            quantity = a.quantity,
                                            barcode = a.barcode,
                                            unit_code = a.unit_code,
                                            id = a.id,
                                            batch_number = a.batch_number,
                                            is_weigh = a.is_weigh ?? false,
                                            packing_code = a.packing_code,
                                            products_warehouse_id = a.products_warehouse_id
                                        }).ToListAsync();

                return model;
            });
        }
        public async Task<Warehouse_Inventory_Model> Warehouse_InventoryCreate(Warehouse_Inventory_Model model)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Warehouse_Inventory Inventory = _mapper.Map<Warehouse_Inventory>(model);
                        Inventory.dateAdded = DateTime.Now;
                        Inventory.code = "Inventory-";
                        _context.Warehouse_Inventory.Add(Inventory);
                        _context.SaveChanges();
                        Inventory.code = "Inventory-" + Inventory.id.ToString();
                        var products = new List<Warehouse_Inventory_Product>();
                        foreach (var item in model.products)
                        {
                            item.dateAdded = DateTime.Now;
                            item.warehouse_inventory_id = Inventory.id;
                            item.userAdded = Inventory.userAdded;
                            products.Add(_mapper.Map<Warehouse_Inventory_Product>(item));
                        }

                        _context.Warehouse_Inventory_Product.AddRange(products);
                        _context.Warehouse_Inventory.Update(Inventory);
                        _context.SaveChanges();
                        transaction.Commit();
                        Warehouse_Inventory_Model response = _mapper.Map<Warehouse_Inventory_Model>(Inventory);
                        response.products = model.products;
                        return response;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        return new Warehouse_Inventory_Model();
                    }

                };
            });
        }

        public async Task<Warehouse_Inventory_Model> Warehouse_InventoryModify(Warehouse_Inventory_Model model)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Warehouse_Inventory Inventory = await _context.Warehouse_Inventory.FirstOrDefaultAsync(r => r.id == model.id); ;
                        Inventory.dateUpdated = DateTime.Now;
                        Inventory.warehouse_id = model.warehouse_id;
                        Inventory.content = model.content;
                        Inventory.status_id = model.status_id;
                        Inventory.type = model.type;
                        _context.Warehouse_Inventory.Update(Inventory);
                        var productdb = await _context.Warehouse_Inventory_Product.Where(x => x.warehouse_inventory_id == model.id && !x.is_delete).ToListAsync();
                        productdb.ForEach(x => x.is_delete = true);
                        _context.Warehouse_Inventory_Product.UpdateRange(productdb);
                        var products = new List<Warehouse_Inventory_Product>();
                        foreach (var item in model.products)
                        {
                            item.id = 0;
                            item.warehouse_inventory_id = model.id;
                            item.dateAdded = DateTime.Now;
                            item.userAdded = model.userAdded;
                            products.Add(_mapper.Map<Warehouse_Inventory_Product>(item));
                        }
                        _context.Warehouse_Inventory_Product.AddRange(products);
                        _context.SaveChanges();
                        transaction.Commit();
                        return model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new Warehouse_Inventory_Model();
                    }

                };
            });
        }
        public async Task<PaginationSet<Warehouse_Inventory_Model>> Warehouse_InventoryList(Warehouse_Inventory_Search search)
        {
            return await Task.Run(async () =>
            {
                PaginationSet<Warehouse_Inventory_Model> response = new PaginationSet<Warehouse_Inventory_Model>();
                var listItem = from a in _context.Warehouse_Inventory
                               join b in _context.Warehouse on a.warehouse_id equals b.id
                               join c in _context.Admin_User on a.userAdded equals c.id
                               where !a.is_delete 
                               select new Warehouse_Inventory_Model
                               {
                                   status_id = a.status_id,
                                   content = a.content,
                                   id = a.id,
                                   warehouse_id = a.warehouse_id,
                                   warehouse_name = b.name,
                                   type = a.type,
                                   code = a.code,
                                   inventory_date = a.inventory_date,
                                   dateAdded = a.dateAdded,
                                   userAdded = a.userAdded,
                                   user_name = c.full_name
                               };
                if (search.keyword is not null and not "")
                {
                    listItem = listItem.Where(r => r.content.Contains(search.keyword));
                }
                if (search.status_id is not null and not 10)
                    listItem = listItem.Where(r => r.status_id == search.status_id);
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
        public async Task<bool> Warehouse_InventoryDelete(long id)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var Inventory = await _context.Warehouse_Inventory.FirstOrDefaultAsync(x => x.id == id);
                        Inventory.is_delete = true;
                        _context.Warehouse_Inventory.Update(Inventory);
                        var Inventory_products = await _context.Warehouse_Inventory_Product.Where(x => x.warehouse_inventory_id == id && !x.is_delete).ToListAsync();
                        Inventory_products.ForEach(x => x.is_delete = true);
                        _context.Warehouse_Inventory_Product.UpdateRange(Inventory_products);
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;

                    }

                };
            });
        }

        public async Task<bool> Warehouse_InventoryReject(long id, long userUpdated)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Warehouse_Inventory Inventory = await _context.Warehouse_Inventory.FirstOrDefaultAsync(r => r.id == id);
                        if (Inventory is null)
                        {
                            return false;
                        }
                        Inventory.dateUpdated = DateTime.Now;
                        Inventory.userUpdated = userUpdated;
                        Inventory.status_id = 2;
                        _context.Warehouse_Inventory.Update(Inventory);
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        return false;
                    }

                };
            });
        }

        public async Task<bool> Warehouse_InventoryConfirm(long id, long userUpdated)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Warehouse_Inventory inventory = await _context.Warehouse_Inventory.FirstOrDefaultAsync(r => r.id == id);
                        if (inventory is null)
                        {
                            return false;
                        }

                        var products = await _context.Warehouse_Inventory_Product.Where(e => e.warehouse_inventory_id == id && !e.is_delete).ToListAsync();
                        if (!products.Any())
                        {
                            return false;
                        }
                        //kiểm tra tồn tại kho ảo
                        var virtualWarehouse = await _context.Warehouse.FirstOrDefaultAsync(x => x.parent_id == inventory.warehouse_id & x.type == 2 & !x.is_delete);
                        if (virtualWarehouse is null)
                        {
                            virtualWarehouse = new Warehouse
                            {
                                code = "auto_create_vitural_" + inventory.warehouse_id,
                                type = 2,
                                name = "Kho ảo smartgap",
                                description = "kho ảo của kho có id:" + inventory.warehouse_id,
                                is_delete = false,
                                dateAdded = DateTime.Now,
                                parent_id = inventory.warehouse_id
                            };
                            _context.Warehouse.Add(virtualWarehouse);
                            _context.SaveChanges();
                        }

                        // check partner
                        var partner = await _context.Partner.FirstOrDefaultAsync(x => x.type == 2 & !x.is_delete);
                        if (partner is null)
                        {
                            partner = new Partner
                            {
                                type = 2,
                                id_ecom = 0,
                                dateAdded = DateTime.Now,
                                name = "auto create kiểm kho",
                                phone = "0000000000000",
                                introduce = "Hệ thống tự tạo partner cho kho ảo"
                            };
                            _context.Partner.Add(partner);
                            _context.SaveChanges();
                        }

                        var ids = products.Select(e => e.products_warehouse_id).ToList();
                        var idprods = products.Select(e => e.product_id).ToList();
                        var productsWarehouse = await _context.Product_Warehouse.Where(e => ids.Contains(e.id) && !e.is_delete).AsNoTracking().ToListAsync();

                        var productsVirtual = await (from a in _context.Product_Warehouse
                                                     join b in _context.Warehouse on a.warehouse_id equals b.id
                                                     where a.warehouse_id == virtualWarehouse.id & idprods.Contains(a.product_id) & !a.is_delete & b.type == 2
                                                     select a
                                                                ).AsNoTracking().ToListAsync();

                        List<Product_Warehouse_History> products_warehouse_historys = new();
                        List<Product_Warehouse> products_warehouse_virtual = new();
                        List<Product_Warehouse> products_warehouse_virtual_add = new();

                        Warehouse_Export export = new Warehouse_Export //xuất kho ảo
                        {
                            code = "WE-Inventory-",
                            content = "Xuất kho vào kho ảo của kiểm kho",
                            partner_id = partner.id,
                            customer_id = 0,
                            warehouse_destination_id = virtualWarehouse.id,
                            type = 4,
                            status_id = 1,
                            warehouse_id = inventory.warehouse_id,
                            export_date = DateTime.Now,
                            userAdded = inventory.userAdded,
                            dateAdded = DateTime.Now
                        };
                        List<Warehouse_Export_Product> products_warehouse_export = new();

                        Warehouse_Receipt receipt = new Warehouse_Receipt
                        {
                            code = "WR-Inventory-",
                            content = "Nhập kho vào kho khi thừa kiểm kho",
                            partner_id = partner.id,
                            transfer_date = DateTime.Now,
                            request_id = 0,
                            type = 4,
                            status_id = 1,
                            warehouse_id = inventory.warehouse_id,
                            userAdded = inventory.userAdded,
                            dateAdded = DateTime.Now
                        };
                        List<Warehouse_Receipt_Product> products_warehouse_receipt = new();

                        foreach (var item in productsWarehouse)
                        {
                            var quantity_productx = products.FirstOrDefault(x => x.products_warehouse_id == item.id);
                            var quantityChange = quantity_productx.quantity - item.quantity_stock;
                            bool add = false;
                            var productVirtualWarehouse = productsVirtual.FirstOrDefault(x => x.product_id == item.product_id);
                            if (productVirtualWarehouse is null)
                            {
                                add = true;
                                productVirtualWarehouse = _mapper.Map<Product_Warehouse>(item);
                                productVirtualWarehouse.id = 0;
                                productVirtualWarehouse.userAdded = inventory.userAdded;
                                productVirtualWarehouse.dateAdded = DateTime.Now;
                                productVirtualWarehouse.warehouse_id = virtualWarehouse.id;
                                productVirtualWarehouse.quantity_sold = 0;
                            }

                            if (quantity_productx.quantity > item.quantity_stock)  // tạo phiếu nhập vào kho
                            {
                                products_warehouse_receipt.Add(new Warehouse_Receipt_Product
                                {
                                    barcode = item.barcode,
                                    batch_number = item.batch_number,
                                    category_packing_code = item.packing_code,
                                    category_unit_code = item.unit_code,
                                    dateAdded = DateTime.Now,
                                    exp_date = item.exp_date ?? DateTime.Now,
                                    import_price = item.import_price,
                                    note = quantity_productx.note,
                                    partner_id = partner.id,
                                    product_id = item.product_id,
                                    price = item.price,
                                    is_weigh = item.is_weigh ?? false,
                                    quantity = quantityChange,
                                    weight = 0,
                                    warehouse_id = item.warehouse_id,
                                    warning_date = 0,
                                    userAdded = inventory.userAdded

                                });
                            }
                            else if (quantity_productx.quantity < item.quantity_stock) // xuất dư vào kho ảo
                            {
                                products_warehouse_export.Add(new Warehouse_Export_Product
                                {
                                    unit_code = item.unit_code,
                                    packing_code = item.packing_code,
                                    partner_id = partner.id,
                                    import_price = item.import_price,
                                    note = quantity_productx.note,
                                    is_weigh = item.is_weigh,
                                    is_promotion = item.is_promotion,
                                    product_id = item.product_id,
                                    exp_date = item.exp_date,
                                    price = item.price,
                                    quantity = -quantityChange,
                                    warehouse_id = productVirtualWarehouse.warehouse_id,
                                    barcode = item.barcode,
                                    batch_number = item.batch_number,
                                    dateAdded = DateTime.Now,
                                    products_warehouse_id = productVirtualWarehouse.id,
                                    userAdded = inventory.userAdded,
                                    warning_date = 0
                                });
                                productVirtualWarehouse.quantity_stock += quantityChange;
                                if (add)
                                {
                                    productVirtualWarehouse.quantity_stock = quantityChange;
                                    products_warehouse_virtual_add.Add(productVirtualWarehouse);
                                }
                                else
                                {
                                    products_warehouse_virtual.Add(productVirtualWarehouse);
                                }
                            }

                            if (quantity_productx.quantity != item.quantity_stock)// lưu lịch sử thay đổi
                            {
                                Product_Warehouse_History history = new()
                                {
                                    code = inventory.code,
                                    product_id = item.product_id,
                                    id_table = inventory.id,
                                    product_warehouse_id = item.id,
                                    type = 4,
                                    quantity = quantityChange,
                                    quantity_in_stock = item.quantity_stock,
                                    import_price = item.import_price,
                                    price = item.price,
                                    sale_price = item.sale_price,
                                    unit_code = item.unit_code,
                                    packing_code = item.packing_code,
                                    exp_date = item.exp_date,
                                    //warning_date = product.warning_date,
                                    warehouse_id = item.warehouse_id,
                                    batch_number = item.batch_number,
                                    userAdded = inventory.userAdded,
                                    is_promotion = item.is_promotion,
                                    dateAdded = DateTime.Now
                                };
                                products_warehouse_historys.Add(history);
                            }
                            // cập nhật lại kho
                            item.quantity_stock = quantity_productx.quantity;
                        }
   
                        if (products_warehouse_export.Count > 0)// thêm xuất kho ảo
                        {
                            _context.Warehouse_Export.Add(export);
                            _context.SaveChanges();
                            export.code += export.id;
                            foreach (var item in products_warehouse_export)
                            {
                                item.export_id = export.id;
                            }
                            _context.Warehouse_Export.Update(export);
                            _context.Warehouse_Export_Product.AddRange(products_warehouse_export);

                        }
                        if (products_warehouse_receipt.Count > 0)// thêm nhập kho
                        {
                            _context.Warehouse_Receipt.Add(receipt);
                            _context.SaveChanges();
                            receipt.code += receipt.id;
                            foreach (var item in products_warehouse_receipt)
                            {
                                item.receipt_id = receipt.id;
                            }
                            _context.Warehouse_Receipt.Update(receipt);
                            _context.Warehouse_Receipt_Product.AddRange(products_warehouse_receipt);
                        }

                        _context.Product_Warehouse_History.AddRange(products_warehouse_historys);//tạo lịch sử
                        inventory.dateUpdated = DateTime.Now;
                        inventory.userUpdated = userUpdated;
                        inventory.status_id = 1;
                        _context.Warehouse_Inventory.Update(inventory);
                        _context.SaveChanges();

                        transaction.Commit();
                        bool check = true;
                        if (products_warehouse_virtual_add.Count > 0)
                        {
                            check = await CreateWarehouseVirtualProduct(products_warehouse_virtual_add);//thêm sp vào kho ảo
                        }
                        if (products_warehouse_virtual.Count > 0)
                        {
                            check= await UpdateWarehouseVirtualProduct(products_warehouse_virtual);// cập nhật kho ảo
                        }
                        check = await UpdateWarehouseProduct(productsWarehouse); //cập nhật kho

                        return check;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();

                        return false;
                    }
                    
                };
            });
        }

        private async Task<bool> CreateWarehouseVirtualProduct(List<Product_Warehouse> product_Warehouses)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    { 
                        if (product_Warehouses.Count > 0)
                        {
                            _context.Product_Warehouse.AddRange(product_Warehouses);//thêm sp vào kho ảo
                            _context.SaveChanges();
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        return false;
                    }

                };
            });
        }

        private async Task<bool> UpdateWarehouseVirtualProduct(List<Product_Warehouse> product_Warehouses)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (product_Warehouses.Count > 0)
                        {
                            var ids = product_Warehouses.Select(e => e.id).ToList();
                            var data = _context.Product_Warehouse.Where(e => ids.Contains(e.id));
                            foreach (var item in data)
                            {
                                item.quantity_stock = product_Warehouses.FirstOrDefault(e => e.id == item.id).quantity_stock;
                            }
                            _context.Product_Warehouse.UpdateRange(data);//update sp vào kho ảo
                            _context.SaveChanges();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        return false;
                    }

                };
            });
        }

        private async Task<bool> UpdateWarehouseProduct(List<Product_Warehouse> product_Warehouses)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (product_Warehouses.Count > 0)
                        {
                            var ids = product_Warehouses.Select(e => e.id).ToList();
                            var data = _context.Product_Warehouse.Where(e => ids.Contains(e.id));
                            foreach (var item in data)
                            {
                                item.quantity_stock = product_Warehouses.FirstOrDefault(e => e.id == item.id).quantity_stock;
                            }
                            _context.Product_Warehouse.UpdateRange(data);//update sp vào kho thât
                            _context.SaveChanges();
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        return false;
                    }

                };
            });
        }


    }
}
