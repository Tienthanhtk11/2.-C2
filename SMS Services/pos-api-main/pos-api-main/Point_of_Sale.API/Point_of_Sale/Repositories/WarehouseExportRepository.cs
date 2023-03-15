using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Export;

namespace Point_of_Sale.Repositories
{
    internal class WarehouseExportRepository : IWarehouseExportRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public WarehouseExportRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<Warehouse_Export_Model> Warehouse_ExportDetail(long id)
        {
            return await Task.Run(async () =>
            {
                Warehouse_Export warehouse_Export = await _context.Warehouse_Export.FirstOrDefaultAsync(r => r.id == id);
                Warehouse_Export_Model model = _mapper.Map<Warehouse_Export_Model>(warehouse_Export);
                model.Products = await _context.Warehouse_Export_Product.Where(r => r.export_id == id && !r.is_delete).ToListAsync();
                return model;
            });
        }
        public async Task<Warehouse_Export_Print_Model> Warehouse_ExportPrint(long id)
        {
            return await Task.Run(async () =>
            {

                var model = await (from a in _context.Warehouse_Export
                                   //join b in _context.Partner on a.partner_id equals b.id
                                   join c in _context.Admin_User on a.userAdded equals c.id
                                   where a.id == id & !a.is_delete
                                   select new Warehouse_Export_Print_Model
                                   {
                                       id = a.id,
                                       code = a.code,
                                       content = a.content,
                                       export_date = a.export_date,
                                       note = a.note,
                                       partner_id = a.partner_id,
                                       source_address = a.source_address,
                                       type = a.type,
                                       user_name = c.full_name,
                                       warehouse_id = a.warehouse_id,
                                       userAdded = a.userAdded,
                                   }).FirstOrDefaultAsync();
                if (model.partner_id != null && model.partner_id != 0)
                {
                    var partner = _context.Partner.FirstOrDefault(x => x.id == model.partner_id && x.is_delete);
                    if (partner!=null)
                    {
                        model.partner_name = partner.name;
                        model.partner_phone = partner.phone;
                    }
                }
                model.products = await (from a in _context.Warehouse_Export_Product
                                        join b in _context.Product on a.product_id equals b.id
                                        where a.export_id == id & !a.is_delete
                                        select new Warehouse_Export_Print_Product
                                        {
                                            name = b.name,
                                            product_id = a.product_id,
                                            note = a.note,
                                            import_price = a.import_price,
                                            exp_date = a.exp_date,
                                            export_id = a.export_id,
                                            price = a.price,
                                            quantity = a.quantity,
                                            barcode = a.barcode,
                                            unit_code = a.unit_code,
                                            warning_date = a.warning_date,
                                            batch_number = a.batch_number,
                                            is_weigh = a.is_weigh,
                                            packing_code = a.packing_code,
                                            partner_id = a.partner_id,
                                            products_warehouse_id = a.products_warehouse_id
                                        }).ToListAsync();

                return model;
            });
        }
        public async Task<Warehouse_Export_Model> Warehouse_ExportCreate(Warehouse_Export_Model model)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Warehouse_Export Export = _mapper.Map<Warehouse_Export>(model);
                        Export.dateAdded = DateTime.Now;
                        int count = _context.Warehouse_Export.Count(x => x.type == model.type && x.code.Contains(model.code));
                        count++;
                        Export.code += count;
                        _context.Warehouse_Export.Add(Export);
                        _context.SaveChanges();
                        foreach (var item in model.Products)
                        {
                            item.dateAdded = DateTime.Now;
                            item.export_id = Export.id;
                            item.userAdded = Export.userAdded;
                        }
                        //kiem tra kho xuat huy ton tai chua
                        if (model.type == 2 && model.warehouse_id != 0)
                        {
                            var warehouseVirtual = await _context.Warehouse.FirstOrDefaultAsync(e => e.parent_id == model.warehouse_id && e.type == 0 && !e.is_delete);
                            if (warehouseVirtual == null)
                            {
                                warehouseVirtual = new Warehouse
                                {
                                    name = "Kho hủy " + model.warehouse_id,
                                    code = "Kho hủy " + model.warehouse_id,
                                    type = 0,
                                    id_come = 0,
                                    district_id = 0,
                                    id_ecom = 0,
                                    parent_id = model.warehouse_id,
                                    address = "",
                                    province_id = 0,
                                    userAdded = model.warehouse_id,
                                    dateAdded = DateTime.Now,
                                    description = "",
                                    ward_id = 0,
                                    is_active = true 
                                };
                                _context.Warehouse.Add(warehouseVirtual);
                                _context.SaveChanges(); 
                            }
                            Export.warehouse_destination_id = warehouseVirtual.id;
                        }
                        //kiem tra kho xuat luan chuyen mac dicnh doi tac la smartgap
                        if (model.type == 3)
                        {
                            var partner = await _context.Partner.FirstOrDefaultAsync(e => e.type == 1 & !e.is_delete);
                            if (partner == null)
                            {
                                partner = new Partner
                                {
                                    type = 1,
                                    id_ecom = 0,
                                    dateAdded = DateTime.Now,
                                    userAdded = Export.userAdded,
                                    name = "Đối tác smartgap",
                                    phone = "0000000000000",
                                    introduce = "Hệ thống tự tạo partner cho xuât kho"
                                };
                                _context.Partner.Add(partner);
                                _context.SaveChanges();
                            }
                            Export.partner_id = partner.id;
                        }
                        
                        _context.Warehouse_Export_Product.AddRange(model.Products); 
                        _context.SaveChanges();
                        transaction.Commit();
                        Warehouse_Export_Model response = _mapper.Map<Warehouse_Export_Model>(Export);
                        response.Products = model.Products;
                        return response;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        return new Warehouse_Export_Model();
                    }

                };
            });
        }

        public async Task<Warehouse_Export_Model> Warehouse_ExportModify(Warehouse_Export_Model model)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Warehouse_Export Export = await _context.Warehouse_Export.FirstOrDefaultAsync(r => r.id == model.id); ;
                        Export.dateUpdated = DateTime.Now;
                        Export.warehouse_id = model.warehouse_id;
                        Export.partner_id = model.partner_id;
                        Export.note = model.note;
                        Export.content = model.content;
                        Export.export_date = model.export_date;
                        Export.warehouse_destination_id = model.warehouse_destination_id;
                        Export.source_address = model.source_address;
                        Export.status_id = 0;
                        Export.type = model.type;
                        //kiem tra kho xuat huy ton tai chua
                        if (model.type == 2 && model.warehouse_id != 0)
                        {
                            var warehouseVirtual = await _context.Warehouse.FirstOrDefaultAsync(e => e.parent_id == model.warehouse_id && e.type == 0 && !e.is_delete);
                            if (warehouseVirtual == null)
                            {
                                warehouseVirtual = new Warehouse
                                {
                                    name = "Kho hủy " + model.warehouse_id,
                                    code = "Kho hủy " + model.warehouse_id,
                                    type = 0,
                                    id_come = 0,
                                    district_id = 0,
                                    id_ecom = 0,
                                    parent_id = model.warehouse_id,
                                    address = "",
                                    province_id = 0,
                                    userAdded = model.warehouse_id,
                                    dateAdded = DateTime.Now,
                                    description = "",
                                    ward_id = 0,
                                    is_active = true
                                };
                                _context.Warehouse.Add(warehouseVirtual);
                                _context.SaveChanges();
                            }
                            Export.warehouse_destination_id = warehouseVirtual.id; 
                        }
                        if (model.type == 3)
                        {
                            var partner = await _context.Partner.FirstOrDefaultAsync(e => e.type == 1 & !e.is_delete);
                            if (partner == null)
                            {
                                partner = new Partner
                                {
                                    type = 1,
                                    id_ecom = 0,
                                    dateAdded = DateTime.Now,
                                    userAdded = Export.userAdded, 
                                    name = "Đối tác smartgap",
                                    phone = "0000000000000",
                                    introduce = "Hệ thống tự tạo partner cho xuât kho"
                                };
                                _context.Partner.Add(partner);
                                _context.SaveChanges();
                            }
                            Export.partner_id = partner.id;
                        } 
                        _context.Warehouse_Export.Update(Export);
                        var productdb = await _context.Warehouse_Export_Product.Where(x => x.export_id == model.id && !x.is_delete).ToListAsync();
                        productdb.ForEach(x => x.is_delete = true);
                        _context.Warehouse_Export_Product.UpdateRange(productdb);
                        foreach (var item in model.Products)
                        {
                            item.id = 0;
                            item.export_id = model.id;
                            item.dateAdded = DateTime.Now;
                            item.userAdded = model.userAdded;
                        }
                        _context.Warehouse_Export_Product.AddRange(model.Products);
                        _context.SaveChanges();
                        transaction.Commit();
                        return model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new Warehouse_Export_Model();
                    }

                };
            });
        }
        public async Task<PaginationSet<Warehouse_ExportViewModel>> Warehouse_ExportList(long partner_id, long warehouse_id, byte? status_id, string? keyword, int page_number, int page_size, DateTime start_date, DateTime end_date)
        {
            return await Task.Run(async () =>
            {
                PaginationSet<Warehouse_ExportViewModel> response = new PaginationSet<Warehouse_ExportViewModel>();
                var listItem = from a in _context.Warehouse_Export
                               join b in _context.Warehouse on a.warehouse_id equals b.id
                               join d in _context.Warehouse on a.warehouse_destination_id equals d.id into gd
                               from ds in gd.DefaultIfEmpty()
                               join c in _context.Partner on a.partner_id equals c.id into gp
                               from gpar in gp.DefaultIfEmpty()
                               where !a.is_delete && a.type != 0 & a.type != 4 && a.export_date >= start_date && end_date.AddDays(1) > a.export_date
                               select new Warehouse_ExportViewModel
                               {
                                   status_id = a.status_id,
                                   partner_id = a.partner_id,
                                   content = a.content,
                                   note = a.note,
                                   id = a.id,
                                   partner_name = gpar.name ?? String.Empty,
                                   source_address = a.source_address,
                                   export_date = a.export_date,
                                   warehouse_id = a.warehouse_id,
                                   warehouse_destination_id = ds.id,
                                   warehouse_destination_name = ds.name ?? String.Empty,
                                   warehouse_name = b.name,
                                   type = a.type,
                                   code = a.code, 
                                   total_amount = _context.Warehouse_Export_Product.Where(x => x.export_id == a.id & !x.is_delete).Sum(y =>y.price * y.quantity)
                               };
                if (keyword is not null and not "")
                {
                    listItem = listItem.Where(r => r.content.Contains(keyword));
                }
                if (status_id is not null and not 10)
                    listItem = listItem.Where(r => r.status_id == status_id);
                if (warehouse_id > 0)
                    listItem = listItem.Where(r => r.warehouse_id == warehouse_id);
                if (partner_id > 0)
                    listItem = listItem.Where(r => r.partner_id == partner_id);
                if (page_number > 0)
                {
                    response.totalcount = listItem.Select(x => x.id).Count();
                    response.page = page_number;
                    response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / page_size);
                    response.lists = await listItem.OrderByDescending(r => r.id).Skip(page_size * (page_number - 1)).Take(page_size).ToListAsync();
                }
                else
                {
                    response.lists = await listItem.OrderByDescending(r => r.id).ToListAsync();
                }
                return response;
            });
        }
        public async Task<bool> Warehouse_ExportDelete(long id)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var export = await _context.Warehouse_Export.FirstOrDefaultAsync(x => x.id == id);
                        export.is_delete = true;
                        _context.Warehouse_Export.Update(export);
                        var export_products = await _context.Warehouse_Export_Product.Where(x => x.export_id == id && !x.is_delete).ToListAsync();
                        export_products.ForEach(x => x.is_delete = true);
                        _context.Warehouse_Export_Product.UpdateRange(export_products);
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

        public async Task<bool> Warehouse_ExportConfirm(long id, long userUpdated)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Warehouse_Export Export = await _context.Warehouse_Export.FirstOrDefaultAsync(r => r.id == id); 
                        Export.status_id = 1; 
                        Export.userUpdated = userUpdated;
                        _context.Warehouse_Export.Update(Export); 
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return true;
                    }

                };
            });

        }

        public async Task<bool> Warehouse_ExportApprove(long id, long userUpdated)
        {
            return await Task.Run(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //update warehouse_export 
                        var warehouse_export = await _context.Warehouse_Export.FirstOrDefaultAsync(x => x.id == id);
                        // check
                        if (!string.IsNullOrEmpty(await Warehouse_ExportVerify(warehouse_export.warehouse_id, warehouse_export.warehouse_destination_id ?? 0, warehouse_export.type)))
                        {
                            transaction.Commit();
                            return false;
                        }

                        warehouse_export.status_id = 2;
                        warehouse_export.userUpdated = userUpdated;
                        //get warehouse_export_products de tru stock
                        var warehouse_export_products = await _context.Warehouse_Export_Product.Where(x => x.export_id == id && !x.is_delete).ToListAsync();
                        //khoi tao warehouse history
                        List<Product_Warehouse_History> products_warehouse_historys = new List<Product_Warehouse_History>();
                        //get products_warehouse kho xuat
                        List<long> product_ids = warehouse_export_products.Select(x => x.products_warehouse_id).ToList();
                        var products_warehouse_export = await _context.Product_Warehouse.Where(x => product_ids.Contains(x.id) && !x.is_delete).ToListAsync();

                        // tao nhap kho 
                        Warehouse warehouse = _context.Warehouse.FirstOrDefault(x => x.id == warehouse_export.warehouse_destination_id);
                        if (warehouse_export.type == 3) // tạo phiếu nhập
                        {
                            Warehouse_Receipt receipt = new()
                            {
                                code = "NLC."+ warehouse.code+".",
                                type = 2,
                                partner_id = warehouse_export.partner_id,
                                status_id = 0,
                                userAdded = warehouse_export.userAdded,
                                warehouse_id = warehouse_export.warehouse_destination_id ?? 0,
                                transfer_date = DateTime.Now,
                                delivery_address = warehouse_export.source_address,
                                content = warehouse_export.content,
                                note = warehouse_export.note,
                                dateAdded = DateTime.Now
                            };
                            int count = _context.Warehouse_Export.Count(x => x.type == receipt.type && x.code.Contains(receipt.code));
                            count++;
                            receipt.code += count;
                            _context.Warehouse_Receipt.Add(receipt);
                            _context.SaveChanges();
                            var listProduct = new List<Warehouse_Receipt_Product>();

                            foreach (var item in warehouse_export_products)
                            {
                                listProduct.Add(new Warehouse_Receipt_Product
                                {
                                    id = 0,
                                    receipt_id = receipt.id,
                                    partner_id = receipt.partner_id,
                                    dateAdded = receipt.dateAdded,
                                    warehouse_id = receipt.warehouse_id,
                                    userAdded = warehouse_export.userAdded,
                                    category_packing_code = item.packing_code,
                                    category_unit_code = item.unit_code,
                                    barcode = item.barcode ?? String.Empty,
                                    exp_date = item.exp_date ?? DateTime.Now,
                                    batch_number = item.batch_number,
                                    import_price = item.import_price,
                                    is_weigh = item.is_weigh ?? false,
                                    price = item.price,
                                    product_id = item.product_id,
                                    quantity = item.quantity,
                                    warning_date = item.warning_date,
                                    note = item.note ?? String.Empty,
                                });

                            }
                            _context.Warehouse_Receipt_Product.AddRange(listProduct);
                            _context.Warehouse_Receipt.Update(receipt);
                        }

                        // xuat huy
                        if (warehouse_export.type == 2) // nhập trục tiếp vào kho hủy và ko tạo phiếu nhập
                        {
                            foreach (var item in warehouse_export_products)
                            {
                                var productImport = await _context.Product_Warehouse.FirstOrDefaultAsync(x => x.product_id == item.product_id && x.warehouse_id == warehouse_export.warehouse_destination_id && !x.is_delete);
                                if (productImport == null)
                                {
                                    productImport = new Product_Warehouse
                                    {
                                        product_id = item.product_id,
                                        quantity_stock = item.quantity,
                                        price = item.price,
                                        packing_code = item.packing_code,
                                        import_price = item.import_price,
                                        exp_date = item.exp_date,
                                        unit_code = item.unit_code,
                                        userAdded = item.userAdded,
                                        //warning_date = item.warning_date,
                                        barcode = item.barcode,
                                        is_weigh = item.is_weigh,
                                        warehouse_id = warehouse_export.warehouse_destination_id ?? 0,
                                        batch_number = item.batch_number
                                    };
                                    _context.Product_Warehouse.Add(productImport);

                                }
                                else
                                {
                                    productImport.quantity_stock += item.quantity;
                                    _context.Product_Warehouse.Update(productImport);
                                }

                                //create history for product import
                                Product_Warehouse_History history = new()
                                {
                                    code = "ImportProductDestruction",
                                    quantity_in_stock = productImport.quantity_stock,
                                    product_id = item.product_id,
                                    id_table = item.export_id,
                                    product_warehouse_id = item.products_warehouse_id,
                                    type = 1,
                                    quantity = item.quantity,
                                    import_price = item.import_price,
                                    price = item.price,
                                    unit_code = item.unit_code,
                                    packing_code = item.packing_code,
                                    exp_date = item.exp_date,
                                    warning_date = item.warning_date,
                                    warehouse_id = item.warehouse_id,
                                    batch_number = item.batch_number,
                                    barcode = item.barcode,
                                    is_promotion = item.is_promotion,
                                    userAdded = userUpdated
                                };
                                products_warehouse_historys.Add(history);
                            }
                        } 

                        // type == 1 xuất kho trả nhà cung cấp  thì xuất kho
                        // xuat kho 
                        foreach (var item in warehouse_export_products)
                        {
                            //update product in stock
                            var product = products_warehouse_export.FirstOrDefault(x => x.id == item.products_warehouse_id);
                            if (product != null)
                            {
                                product.quantity_stock -= item.quantity;
                                //create history for product export
                                Product_Warehouse_History history = new()
                                {
                                    code = warehouse_export.code,
                                    quantity_in_stock = product.quantity_stock,
                                    product_id = item.product_id,
                                    id_table = item.export_id,
                                    product_warehouse_id = product.id,
                                    type = 2,
                                    quantity = item.quantity,
                                    import_price = item.import_price,
                                    price = item.price,
                                    unit_code = item.unit_code,
                                    packing_code = item.packing_code,
                                    exp_date = item.exp_date,
                                    warning_date = item.warning_date,
                                    warehouse_id = item.warehouse_id,
                                    batch_number = item.batch_number,
                                    barcode = item.barcode,
                                    is_promotion = item.is_promotion,
                                    userAdded = userUpdated
                                };
                                products_warehouse_historys.Add(history);
                            }
                        }

                        _context.Warehouse_Export.Update(warehouse_export);
                        _context.Product_Warehouse.UpdateRange(products_warehouse_export);
                        if (products_warehouse_historys.Count() > 0)
                        {
                            _context.Product_Warehouse_History.AddRange(products_warehouse_historys);
                        }

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

        public async Task<string> Warehouse_ExportVerify(long source_id, long destination_id, int type)
        {
            return await Task.Run(async () =>
            {
                if (type == 1 || type == 2)
                {
                    return "";
                }
                if (source_id == destination_id)
                {
                    return "Không xuất kho chính kho của mình";
                }
                var warehouseSource = await _context.Warehouse.FirstOrDefaultAsync(e => e.id == source_id);
                var warehouseDes = await _context.Warehouse.FirstOrDefaultAsync(e => e.id == destination_id);

                if (warehouseSource == null || warehouseDes == null)
                {
                    return "Kho không đúng, vui lòng thử lại";
                }

                if (warehouseSource.type is 0 or 2)
                {
                    return "Kho hàng hủy hoặc kho thất thoát không xuất kho được";
                }

                var warehouseChilds = await _context.Warehouse.Where(e => e.parent_id == source_id).ToListAsync();

                var checkDesIsChild = warehouseChilds != null && warehouseChilds.Any(e => e.id == destination_id);

                return (warehouseDes.parent_id != 0 || warehouseDes.type != 1) && !checkDesIsChild
                    ? "Không xuất sản phẩm sang kho con của kho khác hoặc kho hủy không phải kho con của kho hiện tại"
                    : "";
            });
        }
    }
}