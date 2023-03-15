using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Product;

namespace Point_of_Sale.IRepositories
{
    internal class PartnerRepository : IPartnerRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public PartnerRepository(ApplicationContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<PartnerModel> Partner(long id)
        {
            return await Task.Run(async () =>
            {
                PartnerModel model = new();
                Partner Partner = await _context.Partner.FirstOrDefaultAsync(r => r.id == id);
                model = _mapper.Map<PartnerModel>(Partner);
                return model;
            });
        }
        public async Task<PartnerModel> PartnerCreate(PartnerModel model)
        {
            return await Task.Run(() =>
            {
                var partner_db = _context.Partner.Where(x => x.name == model.name || x.taxcode == model.taxcode && !x.is_delete).FirstOrDefault();
                if (partner_db != null)
                {
                    return null;
                }
                Partner Partner = _mapper.Map<Partner>(model);
                Partner.dateAdded = DateTime.Now;
                Partner.type = 0;
                Partner.search_name = ConvertText.RemoveUnicode(Partner.name.ToLower()) + "-" + ConvertText.RemoveUnicode(Partner.code ?? "");
                int count = _context.Partner.Count(x => x.province_code == model.province_code);
                count++;
                if (count < 1000)
                {
                    string gen_code = Encryptor.auto_gen_code(count);
                    Partner.code = Partner.code + gen_code;
                }
                else
                    Partner.code += count;
                _context.Partner.Add(Partner);
                _context.SaveChanges();
                PartnerModel Partner_response = _mapper.Map<PartnerModel>(Partner);
                return Partner_response;
            });
        }
        public async Task<PartnerModel> PartnerModify(PartnerModel model)
        {
            return await Task.Run(() =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    string response = "0";
                    try
                    {

                        var partner_db = _context.Partner.Where(x => (x.name == model.name || x.taxcode == model.taxcode) && !x.is_delete && x.id != model.id).FirstOrDefault();
                        if (partner_db != null)
                        {
                            return null;
                        }
                        Partner Partner = _context.Partner.FirstOrDefault(r => r.id == model.id);
                        Partner.name = model.name;
                        Partner.id_ecom = model.id_ecom;
                        Partner.phone = model.phone;
                        Partner.website = model.website;
                        Partner.email = model.email;
                        Partner.taxcode = model.taxcode;
                        Partner.introduce = model.introduce;
                        Partner.search_name = ConvertText.RemoveUnicode(Partner.name.ToLower()) + "-" + ConvertText.RemoveUnicode(Partner.code ?? "");

                        _context.Partner.Update(Partner);
                        _context.SaveChanges();
                        transaction.Commit();
                        return model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response = ex.Message + " - " + ex.StackTrace;
                        return model;
                    }
                }
                return model;
            });
        }
        public async Task<bool> PartnerDelete(long id)
        {
            return await Task.Run(() =>
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Partner Partner = _context.Partner.FirstOrDefault(r => r.id == id);
                        Partner.is_delete = true;
                        _context.Partner.Update(Partner);
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            });
        }
        public async Task<PartnerModel> PartnerGetById(long id)
        {
            return await Task.Run(() =>
            {
                PartnerModel model = new PartnerModel();
                Partner Partner = _context.Partner.FirstOrDefault(r => r.id == id);
                return Task.FromResult(model);
            });
        }
        public async Task<PaginationSet<PartnerModel>> PartnerList(SearchBase search)
        {
            await Task.CompletedTask;

            PaginationSet<PartnerModel> response = new();
            IQueryable<PartnerModel> listItem = from a in _context.Partner
                                                where !a.is_delete
                                                select new PartnerModel
                                                {
                                                    id = a.id,
                                                    search_name = a.search_name,
                                                    name = a.name,
                                                    id_ecom = a.id_ecom,
                                                    phone = a.phone,
                                                    website = a.website,
                                                    email = a.email,
                                                    code = a.code,
                                                    taxcode = a.taxcode,
                                                    province_code = a.province_code,
                                                    introduce = a.introduce,
                                                    dateAdded = a.dateAdded,
                                                    userAdded = a.userAdded,
                                                };

            if (search.keyword is not null and not "")
            {
                search.keyword = ConvertText.RemoveUnicode(search.keyword);
                listItem = listItem.Where(r => r.search_name.Contains(search.keyword) || r.code.Contains(search.keyword));
            }
            if (search.province_code != null && search.province_code != 0)
            {
                listItem = listItem.Where(r => r.province_code == search.province_code);
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
        public async Task<List<Partner>> PartnerListAll()
        {
            return await _context.Partner.Where(x => !x.is_delete).OrderByDescending(r => r.dateAdded).ToListAsync();
        }

    }
}
