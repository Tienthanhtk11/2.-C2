using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Warehouse;
using static Humanizer.On;

namespace Point_of_Sale.Repositories
{
    internal class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public WarehouseRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Warehouse> Warehouse(long id)
        {

            Warehouse? warehouse = await _context.Warehouse.Where(r => r.id == id).FirstOrDefaultAsync();

            return warehouse;

        }
        public async Task<PaginationSet<Warehouse_Model>> WarehouseList(SearchBase searchBase)
        {
            return await Task.Run(() =>
            {
                PaginationSet<Warehouse_Model> response = new();

                IQueryable<Warehouse_Model> listItem = from a in _context.Warehouse
                                                       join b in _context.Warehouse on a.parent_id equals b.id into p
                                                       from parent in p.DefaultIfEmpty()
                                                       where !a.is_delete & a.type != 2
                                                       select new Warehouse_Model
                                                       {
                                                           id = a.id,
                                                           search_name = a.search_name,
                                                           code = a.code,
                                                           parent_id = a.parent_id,
                                                           address = a.address,
                                                           description = a.description,
                                                           district_id = a.district_id,
                                                           id_ecom = a.id_ecom,
                                                           is_active = a.is_active,
                                                           name = a.name,
                                                           province_code = a.province_code,
                                                           parent_name = parent.name,
                                                           province_id = a.province_id,
                                                           type = a.type,
                                                           ward_id = a.ward_id
                                                       };
                if (searchBase.keyword is not null and not "")
                {
                    searchBase.keyword = ConvertText.RemoveUnicode(searchBase.keyword);
                    listItem = listItem.Where(r => r.search_name.Contains(searchBase.keyword));
                }
                if (searchBase.type >= 0 && searchBase.type < 3)
                    listItem = listItem.Where(r => r.type == searchBase.type);

                if (searchBase.page_number > 0)
                {
                    response.totalcount = listItem.Select(x => x.id).Count();
                    response.page = searchBase.page_number;
                    response.maxpage = (int)Math.Ceiling((decimal)response.totalcount / searchBase.page_size);
                    response.lists = listItem.OrderByDescending(r => r.id).Skip(searchBase.page_size * (searchBase.page_number - 1)).Take(searchBase.page_size).ToList();
                }
                else
                {
                    response.lists = listItem.OrderByDescending(r => r.id).ToList();
                }
                return response;
            });
        }
        public async Task<Warehouse> WarehouseCreate(Warehouse_Model model)
        {
            return await Task.Run(async () =>
            {
                Warehouse warehouse = _mapper.Map<Warehouse>(model);

                int count = _context.Warehouse.Count(x => x.province_code == warehouse.province_code);
                count++;
                List<string> code = warehouse.code.Split().ToList();

                if (count < 1000)
                {
                    string gen_code = Encryptor.auto_gen_code(count);
                    warehouse.code += gen_code + model.warehouse_type;
                }
                else
                    warehouse.code += count + model.warehouse_type;
                warehouse.dateAdded = DateTime.Now;
                warehouse.search_name = ConvertText.RemoveUnicode(warehouse.name.ToLower()) + "-" + ConvertText.RemoveUnicode(warehouse.code ?? "");
                _context.Warehouse.Add(warehouse);
                _context.SaveChanges();
                return warehouse;
            });
        }
        public async Task<Warehouse> WarehouseModify(Warehouse_Model warehouse)
        {
            return await Task.Run(async () =>
            {
                var model = _context.Warehouse.FirstOrDefault(x => x.id == warehouse.id);
                if (model != null)
                {
                    model.code = warehouse.code;
                    model.name = warehouse.name;
                    model.description = warehouse.description;
                    model.type = warehouse.type;
                    model.parent_id = warehouse.parent_id;
                    model.address = warehouse.address;
                    model.is_active = warehouse.is_active;
                    model.dateUpdated = DateTime.Now;
                    model.userUpdated = warehouse.userUpdated;
                    model.search_name = ConvertText.RemoveUnicode(model.name.ToLower()) + "-" + ConvertText.RemoveUnicode(model.code ?? "");

                }
                _context.Warehouse.Update(model);
                _context.SaveChanges();
                return model;
            });
        }
        public async Task<bool> WarehouseCheckDuplicate(Warehouse_Model warehouse)
        {
            return await Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(warehouse.name) || string.IsNullOrEmpty(warehouse.code))
                {
                    return true;
                }
                if (warehouse.id == 0) // add
                {
                    return _context.Warehouse.Any(x => x.name == warehouse.name || x.code == warehouse.code);
                }
                return _context.Warehouse.Any(x => (x.name == warehouse.name || x.code == warehouse.code) && x.id != warehouse.id);// edit
            });
        }
        public async Task<List<UserWarehouseModel>> WarehouseUserList(long user_id)
        {
            return await Task.Run(async () =>
            {
                List<UserWarehouseModel> userwarehouses = await (from a in _context.Admin_User_Warehouse
                                                                 join b in _context.Warehouse on a.warehouse_id equals b.id
                                                                 where a.user_id == user_id & !a.is_delete & b.type == 1
                                                                 select new UserWarehouseModel
                                                                 {
                                                                     user_id = a.user_id,
                                                                     id = b.id,
                                                                     code = b.code,
                                                                     address = b.address,
                                                                     province_code = b.province_code,
                                                                     name = b.name
                                                                 }).ToListAsync();
                return userwarehouses;
            });
        }

    }
}
