using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using static Humanizer.On;

namespace Point_of_Sale.Repositories
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Category_Product> CategoryProduct(long id)
        {

            var product = _context.Category_Product.Where(r => r.id == id).FirstOrDefault();

            return product;

        }
        public async Task<PaginationSet<Category_Product>> CategoryProductList(SearchBase search)
        {
            return await Task.Run(() =>
            {
                PaginationSet<Category_Product> response = new PaginationSet<Category_Product>();
                IEnumerable<Category_Product> listItem = _context.Category_Product.Where(r => !r.is_delete);

                if (search.keyword != null && search.keyword != "")
                {
                    search.keyword = ConvertText.RemoveUnicode(search.keyword);
                    listItem = listItem.Where(r => r.search_name.Contains(search.keyword.ToLower()));
                }
                if (search.is_active != null)
                {
                    listItem = listItem.Where(r => r.status_id == search.is_active);
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
        public async Task<Category_Product> CategoryProductCreate(Category_Product category)
        {
            return await Task.Run(async () =>
            {
                category.search_name = ConvertText.RemoveUnicode(category.name.ToLower()) + "-" + ConvertText.RemoveUnicode(category.code.ToLower() ?? "");
                category.dateAdded = DateTime.Now;
                _context.Category_Product.Add(category);
                _context.SaveChanges();
                return category;
            });
        }
        public async Task<Category_Product> CategoryProductModify(Category_Product category)
        {
            return await Task.Run(async () =>
            {
                var check = await _context.Category_Product.AsNoTracking().FirstOrDefaultAsync(e => e.id == category.id);
                if (check.code != category.code)
                {
                    var products = _context.Product.Where(e => e.category_code == category.code);
                    foreach (var item in products)
                    {
                        item.code = category.code;
                        item.dateUpdated = DateTime.Now;
                        item.userUpdated = category.userUpdated;
                    }
                    _context.Product.UpdateRange(products);
                }
                category.search_name = ConvertText.RemoveUnicode(category.name.ToLower()) + "-" + ConvertText.RemoveUnicode(category.code ?? "");

                category.dateUpdated = DateTime.Now;
                _context.Category_Product.Update(category);
                _context.SaveChanges();
                return category;
            });
        }
        public async Task<bool> CategoryProductDelete(long category_id, long user_id)
        {
            return await Task.Run(async () =>
            {
                var model = _context.Category_Product.Where(r => r.id == category_id).FirstOrDefault();
                if (model == null || model.id == 0)
                {
                    return false;
                }
                else
                {
                    model.userUpdated = user_id;
                    model.dateUpdated = DateTime.Now;
                    model.is_delete = true;
                    _context.Category_Product.Update(model);
                }
                _context.SaveChanges();
                return true;
            });
        }

        // danh muc quay hang
        public async Task<List<Category_Stalls>> CategoryStallsList(string? keyword, long category_id)
        {
            return await Task.Run(() =>
            {
                List<Category_Stalls> response = new List<Category_Stalls>();
                IEnumerable<Category_Stalls> listItem = _context.Category_Stalls.Where(r => !r.is_delete && r.category_id == category_id);

                if (keyword != null && keyword != "")
                {
                    keyword = ConvertText.RemoveUnicode(keyword);
                    listItem = listItem.Where(r => r.search_name.Contains(keyword.ToLower()));
                }
                response = listItem.OrderByDescending(r => r.id).ToList();

                return response;
            });
        }
        public async Task<Category_Stalls> CategoryStallsCreate(Category_Stalls category)
        {
            return await Task.Run(async () =>
            {
                Category_Stalls category_db = _context.Category_Stalls.FirstOrDefault(x => x.category_id == category.category_id && (x.name == category.name || x.code == category.code));
                if (category_db == null)
                {
                    category.search_name = ConvertText.RemoveUnicode(category.name.ToLower()) + "-" + ConvertText.RemoveUnicode(category.code ?? "");
                    category.dateAdded = DateTime.Now;
                    _context.Category_Stalls.Add(category);
                    _context.SaveChanges();
                    return category;
                }
                else
                    return null;
            });
        }
        public async Task<Category_Stalls> CategoryStallsModify(Category_Stalls category)
        {
            return await Task.Run(async () =>
            {
                var category_db = _context.Category_Stalls.FirstOrDefault(x => x.category_id == category.category_id && (x.name == category.name || x.code == category.code) && x.id != category.id);
                if (category_db == null)
                {
                    category.search_name = ConvertText.RemoveUnicode(category.name.ToLower()) + "-" + ConvertText.RemoveUnicode(category.code ?? "");
                    category.dateUpdated = DateTime.Now;
                    _context.Category_Stalls.Update(category);
                    _context.SaveChanges();
                    return category;
                }
                else
                    return null;
            });
        }
        // danh muc nhom hang hoa
        public async Task<List<Category_Group>> CategoryGroupList(string? keyword, long stalls_id)
        {
            return await Task.Run(() =>
            {
                List<Category_Group> response = new();
                IEnumerable<Category_Group> listItem = _context.Category_Group.Where(r => !r.is_delete && r.stalls_id == stalls_id);

                if (keyword != null && keyword != "")
                {
                    keyword = ConvertText.RemoveUnicode(keyword);
                    listItem = listItem.Where(r => r.search_name.Contains(keyword.ToLower()));
                }
                response = listItem.OrderByDescending(r => r.id).ToList();

                return response;
            });
        }
        public async Task<Category_Group> CategoryGroupCreate(Category_Group category)
        {
            return await Task.Run(async () =>
            {
                var category_db = _context.Category_Group.FirstOrDefault(x => x.stalls_id == category.stalls_id && (x.name == category.name || x.code == category.code));
                if (category_db == null)
                {
                    category.search_name = ConvertText.RemoveUnicode(category.name.ToLower()) + "-" + ConvertText.RemoveUnicode(category.code ?? "");
                    category.dateAdded = DateTime.Now;
                    _context.Category_Group.Add(category);
                    _context.SaveChanges();
                    return category;
                }
                else
                    return null;
            });
        }
        public async Task<Category_Group> CategoryGroupModify(Category_Group category)
        {
            return await Task.Run(async () =>
            {
                var category_db = _context.Category_Group.AsNoTracking().FirstOrDefault(x => x.stalls_id == category.stalls_id && (x.name == category.name || x.code == category.code) && x.id != category.id);
                if (category_db == null)
                {
                    category.search_name = ConvertText.RemoveUnicode(category.name.ToLower()) + "-" + ConvertText.RemoveUnicode(category.code ?? "");
                    category.dateUpdated = DateTime.Now;
                    _context.Category_Group.Update(category);
                    _context.SaveChanges();
                    return category;
                }
                else
                    return null;
            });
        }


        public async Task<Category_Unit> CategoryUnit(long id)
        {

            var product = _context.Category_Unit.Where(r => r.id == id).FirstOrDefault();

            return product;

        }
        public async Task<PaginationSet<Category_Unit>> CategoryUnitList(SearchBase search)
        {
            return await Task.Run(() =>
            {
                PaginationSet<Category_Unit> response = new();
                IEnumerable<Category_Unit> listItem = _context.Category_Unit.Where(r => !r.is_delete);

                if (search.keyword != null && search.keyword != "")
                {
                    search.keyword = ConvertText.RemoveUnicode(search.keyword);
                    listItem = listItem.Where(r => r.search_name.Contains(search.keyword.ToLower()));
                }
                if (search.is_active != null)
                {
                    listItem = listItem.Where(r => r.status_id == search.is_active);
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
        public async Task<Category_Unit> CategoryUnitCreate(Category_Unit category)
        {
            return await Task.Run(async () =>
            {
                category.search_name = ConvertText.RemoveUnicode(category.name.ToLower()) + "-" + ConvertText.RemoveUnicode(category.code.ToLower() ?? "");

                category.dateAdded = DateTime.Now;
                _context.Category_Unit.Add(category);
                _context.SaveChanges();
                return category;
            });
        }
        public async Task<Category_Unit> CategoryUnitModify(Category_Unit category)
        {
            return await Task.Run(async () =>
            {
                var check = await _context.Category_Unit.AsNoTracking().FirstOrDefaultAsync(e => e.id == category.id);
                if (check is not null && check.code != category.code)
                {
                    var products = _context.Product_Warehouse.Where(e => e.unit_code == check.code);
                    foreach (var item in products)
                    {
                        item.dateUpdated = DateTime.Now;
                        item.userUpdated = category.userUpdated;
                    }
                    _context.Product_Warehouse.UpdateRange(products);
                }
                category.search_name = ConvertText.RemoveUnicode(category.name.ToLower()) + "-" + ConvertText.RemoveUnicode(category.code.ToLower() ?? "");

                category.dateUpdated = DateTime.Now;
                _context.Category_Unit.Update(category);

                _context.SaveChanges();
                return category;
            });
        }
        public async Task<bool> CategoryUnitDelete(long category_id, long user_id)
        {
            return await Task.Run(async () =>
            {
                var model = _context.Category_Unit.Where(r => r.id == category_id).FirstOrDefault();
                if (model == null || model.id == 0)
                {
                    return false;
                }
                else
                {
                    model.userUpdated = user_id;
                    model.dateUpdated = DateTime.Now;
                    model.is_delete = true;
                    _context.Category_Unit.Update(model);
                }
                _context.SaveChanges();
                return true;
            });
        }

        public async Task<Category_Packing> CategoryPacking(long id)
        {

            var product = _context.Category_Packing.Where(r => r.id == id).FirstOrDefault();

            return product;

        }
        public async Task<PaginationSet<Category_Packing>> CategoryPackingList(SearchBase search)
        {
            return await Task.Run(() =>
            {
                PaginationSet<Category_Packing> response = new PaginationSet<Category_Packing>();
                IEnumerable<Category_Packing> listItem = _context.Category_Packing.Where(r => !r.is_delete);

                if (search.keyword != null && search.keyword != "")
                {
                    search.keyword = ConvertText.RemoveUnicode(search.keyword);
                    listItem = listItem.Where(r => r.search_name.Contains(search.keyword.ToLower()));
                }
                if (search.is_active != null)
                {
                    listItem = listItem.Where(r => r.status_id == search.is_active);
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
        public async Task<Category_Packing> CategoryPackingCreate(Category_Packing category)
        {
            return await Task.Run(async () =>
            {
                category.search_name = ConvertText.RemoveUnicode(category.name.ToLower()) + "-" + ConvertText.RemoveUnicode(category.code.ToLower() ?? "");

                category.dateAdded = DateTime.Now;
                _context.Category_Packing.Add(category);
                _context.SaveChanges();
                return category;
            });
        }
        public async Task<Category_Packing> CategoryPackingModify(Category_Packing category)
        {
            return await Task.Run(async () =>
            {
                var check = await _context.Category_Packing.AsNoTracking().FirstOrDefaultAsync(e => e.id == category.id);
                if (check is not null && check.code != category.code)
                {
                    var products = _context.Product_Warehouse.Where(e => e.packing_code == check.code);
                    foreach (var item in products)
                    {
                        item.dateUpdated = DateTime.Now;
                        item.userUpdated = category.userUpdated;
                    }
                    _context.Product_Warehouse.UpdateRange(products);
                }
                category.search_name = ConvertText.RemoveUnicode(category.name.ToLower()) + "-" + ConvertText.RemoveUnicode(category.code.ToLower() ?? "");

                category.dateUpdated = DateTime.Now;
                _context.Category_Packing.Update(category);

                _context.SaveChanges();
                return category;
            });
        }
        public async Task<bool> CategoryPackingDelete(long category_id, long user_id)
        {
            return await Task.Run(async () =>
            {
                var model = _context.Category_Packing.Where(r => r.id == category_id).FirstOrDefault();
                if (model == null || model.id == 0)
                {
                    return false;
                }
                else
                {
                    model.userUpdated = user_id;
                    model.dateUpdated = DateTime.Now;
                    model.is_delete = true;
                    _context.Category_Packing.Update(model);
                }
                _context.SaveChanges();
                return true;
            });
        }

        public async Task<Category_District> DistrictCreate(Category_District model)
        {
            Category_District district = _mapper.Map<Category_District>(model);
            district.dateAdded = DateTime.Now;
            _context.Category_District.Add(district);
            _context.SaveChanges();
            model = _mapper.Map<Category_District>(district);
            return model;
        }
        public async Task<Category_District> DistrictGetById(long id)
        {
            Category_District district = await _context.Category_District.FirstOrDefaultAsync(r => r.id == id);
            Category_District model = _mapper.Map<Category_District>(district);
            return model;
        }
        public async Task<Category_District> DistrictModify(Category_District model)
        {
            Category_District district = await _context.Category_District.FirstOrDefaultAsync(r => r.id == model.id);
            district.note = model.note;
            district.code = model.code;
            district.name = model.name;
            district.order = model.order;
            district.status_id = model.status_id;
            district.is_deleted = model.is_deleted;
            district.province_id = model.province_id;
            district.userUpdated = model.userUpdated;
            district.language_code = model.language_code;
            _context.Category_District.Update(district);
            _context.SaveChanges();
            return model;
        }
        public async Task<List<Category_District>> DistrictList(string language_code)
        {
            List<Category_District> districts = new();
            districts = await (from b in _context.Category_District

                               select new Category_District
                               {
                                   id = b.id,
                                   name = b.name,
                                   note = b.note,
                                   code = b.code,
                                   status_id = b.status_id,
                                   order = b.order,
                                   is_deleted = b.is_deleted,
                                   province_id = b.province_id,
                                   userAdded = b.userAdded,
                                   userUpdated = b.userUpdated,
                                   language_code = b.language_code,

                               }).OrderBy(r => r.order).ToListAsync();
            return districts;
        }
        public async Task<List<Category_District>> DistrictListProvinceId(long province_id, string language_code)
        {
            List<Category_District> districts = new();
            districts = await (from b in _context.Category_District

                               select new Category_District
                               {
                                   id = b.id,
                                   name = b.name,
                                   note = b.note,
                                   code = b.code,
                                   status_id = b.status_id,
                                   order = b.order,
                                   is_deleted = b.is_deleted,
                                   userAdded = b.userAdded,
                                   userUpdated = b.userUpdated,
                                   language_code = b.language_code,
                                   province_id = b.province_id,

                               }).OrderBy(r => r.order).ToListAsync();
            return districts;
        }
        public async Task<bool> DistrictDelete(long id)
        {
            try
            {
                Category_District district = await _context.Category_District.FirstOrDefaultAsync(r => r.id == id);
                district.is_deleted = true;
                _context.Category_District.Update(district);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<Category_Province> ProvinceCreate(Category_Province model)
        {
            Category_Province province = _mapper.Map<Category_Province>(model);
            province.dateAdded = DateTime.Now;
            _context.Category_Province.Add(province);
            _context.SaveChanges();
            model = _mapper.Map<Category_Province>(province);
            return model;
        }
        public async Task<Category_Province> ProvinceGetById(long id)
        {
            Category_Province province = await _context.Category_Province.FirstOrDefaultAsync(r => r.id == id);
            Category_Province model = _mapper.Map<Category_Province>(province);
            return model;
        }
        public async Task<Category_Province> ProvinceModify(Category_Province model)
        {
            Category_Province province = await _context.Category_Province.FirstOrDefaultAsync(r => r.id == model.id);
            province.zipcode = model.zipcode;
            province.city = model.city;
            province.order = model.order;
            province.status_id = model.status_id;
            province.is_deleted = model.is_deleted;
            province.userUpdated = model.userUpdated;
            province.language_code = model.language_code;
            province.dateUpdated = DateTime.Now;
            _context.Category_Province.Update(province);
            _context.SaveChanges();
            return model;
        }
        public async Task<List<Category_Province>> ProvinceList(string language_code)
        {
            List<Category_Province> provinces = new();
            provinces = await (from c in _context.Category_Province
                               select new Category_Province
                               {
                                   id = c.id,
                                   zipcode = c.zipcode,
                                   city = c.city,
                                   order = c.order,
                                   status_id = c.status_id,
                                   is_deleted = c.is_deleted,
                                   userAdded = c.userAdded,
                                   userUpdated = c.userUpdated,
                                   language_code = c.language_code,

                               }).OrderBy(r => r.order).ToListAsync();
            return provinces;
        }
        public async Task<PaginationSet<Category_Province>> ProvinceListView(string language_code, int page_number, int page_size)
        {

            PaginationSet<Category_Province> response = new PaginationSet<Category_Province>();
            IQueryable<Category_Province> listItem = from a in _context.Category_Province
                                                     where !a.is_deleted && a.language_code == language_code
                                                     orderby a.zipcode
                                                     select new Category_Province
                                                     {
                                                         city = a.city,
                                                         id = a.id,

                                                     };
            if (page_number > 0)
            {
                response.totalcount = listItem.Select(x => x.id).Count();
                response.page = page_number;
                response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / page_size);
                response.lists = await listItem.Skip(page_size * (page_number - 1)).Take(page_size).ToListAsync();
            }
            else
            {
                response.lists = await listItem.ToListAsync();
            }
            return response;
        }
        public async Task<List<Category_Province>> ProvincesListId(List<long> ids)
        {
            List<Category_Province> provinces = new();
            provinces = await (from c in _context.Category_Province

                               where ids.Contains(c.id) && c.is_transport == true
                               && !c.is_deleted
                               select new Category_Province
                               {
                                   id = c.id,
                                   zipcode = c.zipcode,
                                   city = c.city,
                                   order = c.order,
                                   status_id = c.status_id,
                                   is_deleted = c.is_deleted,
                                   userAdded = c.userAdded,
                                   userUpdated = c.userUpdated,
                                   language_code = c.language_code,

                               }).OrderBy(r => r.order).ToListAsync();
            return provinces;
        }
        public async Task<bool> ProvinceDelete(long id)
        {
            try
            {
                Category_Province province = await _context.Category_Province.FirstOrDefaultAsync(r => r.id == id);
                province.is_deleted = true;
                _context.Category_Province.Update(province);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<Category_Ward> WardCreate(Category_Ward model)
        {
            Category_Ward ward = _mapper.Map<Category_Ward>(model);
            ward.dateAdded = DateTime.Now;
            _context.Category_Ward.Add(ward);
            _context.SaveChanges();
            model = _mapper.Map<Category_Ward>(ward);
            return model;
        }
        public async Task<Category_Ward> WardGetById(long id)
        {
            Category_Ward ward = await _context.Category_Ward.FirstOrDefaultAsync(r => r.id == id);
            Category_Ward model = _mapper.Map<Category_Ward>(ward);
            return model;
        }
        public async Task<Category_Ward> WardModify(Category_Ward model)
        {
            Category_Ward ward = await _context.Category_Ward.FirstOrDefaultAsync(r => r.id == model.id);
            ward.name = model.name;
            ward.code = model.code;
            ward.note = model.note;
            ward.status_id = model.status_id;
            ward.order = model.order;
            ward.is_deleted = model.is_deleted;
            ward.district_id = model.district_id;
            ward.language_code = model.language_code;
            ward.dateUpdated = DateTime.Now;
            _context.Category_Ward.Update(ward);
            _context.SaveChanges();
            return model;
        }
        public async Task<List<Category_Ward>> WardListDistrictId(long district_id, string language_code)
        {
            List<Category_Ward> wards = new();
            wards = await (from d in _context.Category_Ward
                           select new Category_Ward
                           {
                               id = d.id,
                               name = d.name,
                               note = d.note,
                               code = d.code,
                               order = d.order,
                               status_id = d.status_id,
                               district_id = d.district_id,
                               userAdded = d.userAdded,
                               userUpdated = d.userUpdated,
                               language_code = d.language_code,
                           }).OrderBy(r => r.order).ToListAsync();
            return wards;
        }
        public async Task<List<Category_Ward>> WardList(string language_code)
        {
            List<Category_Ward> wards = new();
            wards = await (from d in _context.Category_Ward
                           select new Category_Ward
                           {
                               id = d.id,
                               name = d.name,
                               code = d.code,
                               note = d.note,
                               order = d.order,
                               status_id = d.status_id,
                               is_deleted = d.is_deleted,
                               district_id = d.district_id,
                               userAdded = d.userAdded,
                               userUpdated = d.userUpdated,
                               language_code = d.language_code,
                           }).OrderBy(r => r.order).ToListAsync();
            return wards;
        }
        public async Task<bool> WardDelete(long id)
        {
            try
            {
                Category_Ward ward = await _context.Category_Ward.FirstOrDefaultAsync(r => r.id == id);

                ward.is_deleted = true;
                _context.Category_Ward.Update(ward);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
