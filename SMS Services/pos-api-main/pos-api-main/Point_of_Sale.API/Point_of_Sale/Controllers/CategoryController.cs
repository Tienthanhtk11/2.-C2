using ECom.Framework.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;

namespace Point_of_Sale.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public CategoryController(ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("category-product")]
        public async Task<IActionResult> CategoryProduct(long id)
        {
            try
            {
                var products = await this._categoryRepository.CategoryProduct(id);
                return Ok(new ResponseSingleContentModel<Category_Product>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = products
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-product-create")]
        public async Task<IActionResult> CategoryProductCreate([FromBody] Category_Product model)
        {
            try
            {
                var validator = ValitRules<Category_Product>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    model.userAdded = userid(_httpContextAccessor);
                    var product = await this._categoryRepository.CategoryProductCreate(model);
                    return Ok(new ResponseSingleContentModel<Category_Product>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = product
                    });
                }
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-product-modify")]
        public async Task<IActionResult> CategoryProductModify([FromBody] Category_Product model)
        {
            try
            {
                var validator = ValitRules<Category_Product>
                    .Create()
                    //.Ensure(m => m.name, rule => rule.Required())
                    // .Ensure(m => m.code, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    //.Ensure(m => m.note, rule => rule.Required())
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    model.userUpdated = userid(_httpContextAccessor);

                    var product = await this._categoryRepository.CategoryProductModify(model);

                    return Ok(new ResponseSingleContentModel<Category_Product>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = product
                    });
                }

                // Return invalidate data
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-product-list")]
        public async Task<IActionResult> CategoryProductList([FromBody] SearchBase search)
        {
            try
            {
                var products = await this._categoryRepository.CategoryProductList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<Category_Product>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = products
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null  
                });
            }
        }

        [HttpDelete("category-product-delete")]
        public async Task<IActionResult> ProductDelete(long category_id)
        {
            try
            {
                long user_id = userid(_httpContextAccessor);

                var response = await this._categoryRepository.CategoryProductDelete(category_id, user_id);
                if (response)
                {
                    return Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = ""
                    });
                }
                else

                    return Ok(new ResponseSingleContentModel<IResponseData>
                    {
                        StatusCode = 500,
                        Message = "Không tìm thấy bản ghi",
                        Data = null
                    });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }

        [HttpGet("category-unit")]
        public async Task<IActionResult> CategoryUnit(long id)
        {
            try
            {
                var products = await this._categoryRepository.CategoryUnit(id);
                return Ok(new ResponseSingleContentModel<Category_Unit>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = products
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-unit-create")]
        public async Task<IActionResult> CategoryUnitCreate([FromBody] Category_Unit model)
        {
            try
            {
                var validator = ValitRules<Category_Unit>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    model.userAdded = userid(_httpContextAccessor);
                    var product = await this._categoryRepository.CategoryUnitCreate(model);
                    return Ok(new ResponseSingleContentModel<Category_Unit>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = product
                    });
                }
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-unit-modify")]
        public async Task<IActionResult> CategoryUnitModify([FromBody] Category_Unit model)
        {
            try
            {
                var validator = ValitRules<Category_Unit>
                    .Create()
                    //.Ensure(m => m.name, rule => rule.Required())
                    // .Ensure(m => m.code, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    //.Ensure(m => m.note, rule => rule.Required())
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    model.userUpdated = userid(_httpContextAccessor);
                    var product = await this._categoryRepository.CategoryUnitModify(model);

                    return Ok(new ResponseSingleContentModel<Category_Unit>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = product
                    });
                }

                // Return invalidate data
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-unit-list")]
        public async Task<IActionResult> CategoryUnitList([FromBody] SearchBase search)
        {
            try
            {
                var products = await this._categoryRepository.CategoryUnitList( search);
                return Ok(new ResponseSingleContentModel<PaginationSet<Category_Unit>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = products
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpDelete("category-unit-delete")]
        public async Task<IActionResult> CategoryUnitDelete(long category_id)
        {
            try
            {
                long user_id = userid(_httpContextAccessor);

                var response = await this._categoryRepository.CategoryUnitDelete(category_id, user_id);
                if (response)
                {
                    return Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = ""
                    });
                }
                else

                    return Ok(new ResponseSingleContentModel<IResponseData>
                    {
                        StatusCode = 500,
                        Message = "Không tìm thấy bản ghi",
                        Data = null
                    });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }

        [HttpGet("category-packing")]
        public async Task<IActionResult> CategoryPacking(long id)
        {
            try
            {
                var products = await this._categoryRepository.CategoryPacking(id);
                return Ok(new ResponseSingleContentModel<Category_Packing>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = products
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-packing-create")]
        public async Task<IActionResult> CategoryPackingCreate([FromBody] Category_Packing model)
        {
            try
            {
                var validator = ValitRules<Category_Packing>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    model.userAdded = userid(_httpContextAccessor);
                    var product = await this._categoryRepository.CategoryPackingCreate(model);
                    return Ok(new ResponseSingleContentModel<Category_Packing>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = product
                    });
                }
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-packing-modify")]
        public async Task<IActionResult> CategoryPackingModify([FromBody] Category_Packing model)
        {
            try
            {
                var validator = ValitRules<Category_Packing>
                    .Create()
                    //.Ensure(m => m.name, rule => rule.Required())
                    // .Ensure(m => m.code, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    //.Ensure(m => m.note, rule => rule.Required())
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    model.userUpdated = userid(_httpContextAccessor);

                    var product = await this._categoryRepository.CategoryPackingModify(model);

                    return Ok(new ResponseSingleContentModel<Category_Packing>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = product
                    });
                }

                // Return invalidate data
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-packing-list")]
        public async Task<IActionResult> CategoryPackingList([FromBody] SearchBase search)
        {
            try
            {
                var products = await this._categoryRepository.CategoryPackingList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<Category_Packing>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = products
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpDelete("category-packing-delete")]
        public async Task<IActionResult> CategoryPackingDelete(long category_id)
        {
            try
            {
                long user_id = userid(_httpContextAccessor);

                var response = await this._categoryRepository.CategoryPackingDelete(category_id, user_id);
                if (response)
                {
                    return Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = ""
                    });
                }
                else

                    return Ok(new ResponseSingleContentModel<IResponseData>
                    {
                        StatusCode = 500,
                        Message = "Không tìm thấy bản ghi",
                        Data = null
                    });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        #region Province
        [AllowAnonymous]
        [HttpGet("category-province-list")]
        public async Task<IActionResult> CategoryProvinceList(string language_code)
        {
            try
            {
                var province = await this._categoryRepository.ProvinceList(language_code);
                return Ok(new ResponseSingleContentModel<List<Category_Province>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công",
                    Data = province
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [AllowAnonymous]
        [HttpGet("category-province-list-view")]
        public async Task<IActionResult> CategoryProvinceList(string language_code, int page_number = 1, int page_size = 10)
        {
            try
            {
                var province = await this._categoryRepository.ProvinceListView(language_code, page_number, page_size);
                return Ok(new ResponseSingleContentModel<PaginationSet<Category_Province>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công",
                    Data = province
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [AllowAnonymous]
        [HttpPost("category-province-byids")]
        public async Task<IActionResult> CategoryProvinceList(List<long> ids)
        {
            try
            {
                var province = await this._categoryRepository.ProvincesListId(ids);
                return Ok(new ResponseSingleContentModel<List<Category_Province>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công",
                    Data = province
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpGet("category-province")]
        public async Task<IActionResult> CategoryProvince(long id)
        {
            try
            {
                var province = await this._categoryRepository.ProvinceGetById(id);

                return Ok(new ResponseSingleContentModel<Category_Province>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = province
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-province-create")]
        public async Task<IActionResult> CategoryProvinceCreate([FromBody] Category_Province model)
        {
            try
            {
                var validator = ValitRules<Category_Province>
                    .Create()
                    .Ensure(m => m.zipcode, rule => rule.Required())
                     .Ensure(m => m.city, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    var province = await this._categoryRepository.ProvinceCreate(model);
                    return Ok(new ResponseSingleContentModel<Category_Province>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = province
                    });
                }
                // Return invalidate data
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-province-modify")]
        public async Task<IActionResult> CategoryProvinceModify([FromBody] Category_Province model)
        {
            try
            {
                var validator = ValitRules<Category_Province>
                    .Create()
                    .Ensure(m => m.zipcode, rule => rule.Required())
                     .Ensure(m => m.city, rule => rule.Required())
                    // .Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    var province = await this._categoryRepository.ProvinceModify(model);
                    return Ok(new ResponseSingleContentModel<Category_Province>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = province
                    });
                }
                // Return invalidate data
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpDelete("category-province-delete")]
        public async Task<IActionResult> CategoryProvinceDelete(long id)
        {
            try
            {
                var province = await this._categoryRepository.ProvinceDelete(id);
                return province
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Xóa bản ghi thành công",
                        Data = null
                    })
                    : (IActionResult)Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Bản ghi không tồn tại hoặc bị xóa trước đó",
                        Data = null
                    });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        #endregion

        #region District
        [AllowAnonymous]
        [HttpGet("category-district-list")]
        public async Task<IActionResult> CategoryDistrictList(string language_code)
        {
            try
            {
                var district = await this._categoryRepository.DistrictList(language_code);
                return Ok(new ResponseSingleContentModel<List<Category_District>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công",
                    Data = district
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [AllowAnonymous]
        [HttpGet("category-district-list-province")]
        public async Task<IActionResult> DistrictListProvince(string language_code, long province_id)
        {
            try
            {
                var district = await this._categoryRepository.DistrictListProvinceId(province_id, language_code);
                return Ok(new ResponseSingleContentModel<List<Category_District>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công",
                    Data = district
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpGet("category-district")]
        public async Task<IActionResult> CategoryDistrict(long id)
        {
            try
            {
                var district = await this._categoryRepository.DistrictGetById(id);
                return Ok(new ResponseSingleContentModel<Category_District>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = district
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "",
                    Data = null
                });
            }
        }
        [HttpPost("category-district-create")]
        public async Task<IActionResult> CategoryDistrictCreate([FromBody] Category_District model)
        {
            try
            {
                var validator = ValitRules<Category_District>
                    .Create()
                    .Ensure(m => m.name, rule => rule.Required())
                     .Ensure(m => m.code, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    var district = await this._categoryRepository.DistrictCreate(model);
                    return Ok(new ResponseSingleContentModel<Category_District>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = district
                    });
                }
                // Return invalidate data
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-district-modify")]
        public async Task<IActionResult> CategoryDistrictModify([FromBody] Category_District model)
        {
            try
            {
                var validator = ValitRules<Category_District>
                    .Create()
                    .Ensure(m => m.name, rule => rule.Required())
                    .Ensure(m => m.code, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    var district = await this._categoryRepository.DistrictModify(model);

                    return Ok(new ResponseSingleContentModel<Category_District>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = district
                    });
                }
                // Return invalidate data
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpDelete("category-district-delete")]
        public async Task<IActionResult> CategoryDistrictDelete(long id)
        {
            try
            {
                var district = await this._categoryRepository.DistrictDelete(id);


                return district
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Xóa bản ghi thành công",
                        Data = null
                    })
                    : (IActionResult)Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Bản ghi không tồn tại hoặc bị xóa trước đó",
                        Data = null
                    });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        #endregion

        #region ward
        [AllowAnonymous]
        [HttpGet("category-ward-list")]
        public async Task<IActionResult> CategoryWardList(string language_code)
        {
            try
            {
                var ward = await this._categoryRepository.WardList(language_code);
                return Ok(new ResponseSingleContentModel<List<Category_Ward>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công",
                    Data = ward
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("category-ward-list-district")]
        public async Task<IActionResult> WardListDistrict(string language_code, long district_id)
        {
            try
            {
                var ward = await this._categoryRepository.WardListDistrictId(district_id, language_code);
                return Ok(new ResponseSingleContentModel<List<Category_Ward>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công",
                    Data = ward
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpGet("category-ward")]
        public async Task<IActionResult> CategoryWard(long id)
        {
            try
            {
                var ward = await this._categoryRepository.WardGetById(id);

                return Ok(new ResponseSingleContentModel<Category_Ward>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = ward
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }

        [HttpPost("category-ward-create")]
        public async Task<IActionResult> CategoryWardCreate([FromBody] Category_Ward model)
        {
            try
            {
                var validator = ValitRules<Category_Ward>
                    .Create()
                    .Ensure(m => m.name, rule => rule.Required())
                     .Ensure(m => m.code, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    var ward = await this._categoryRepository.WardCreate(model);

                    return Ok(new ResponseSingleContentModel<Category_Ward>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = ward
                    });
                }
                // Return invalidate data
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-ward-modify")]
        public async Task<IActionResult> CategoryWardModify([FromBody] Category_Ward model)
        {
            try
            {
                var validator = ValitRules<Category_Ward>
                    .Create()
                    .Ensure(m => m.name, rule => rule.Required())
                     .Ensure(m => m.code, rule => rule.Required())
                    // .Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    var ward = await this._categoryRepository.WardModify(model);

                    return Ok(new ResponseSingleContentModel<Category_Ward>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = ward
                    });
                }
                // Return invalidate data
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpDelete("category-ward-delete")]
        public async Task<IActionResult> CategoryWardDelete(long id)
        {
            try
            {
                var ward = await this._categoryRepository.WardDelete(id);
                return ward
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Xóa bản ghi thành công",
                        Data = null
                    })
                    : (IActionResult)Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Bản ghi không tồn tại hoặc bị xóa trước đó",
                        Data = null
                    });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        #endregion

        #region danh muc cap 2 3

        [HttpPost("category-stalls-create")]
        public async Task<IActionResult> CategoryStallsCreate([FromBody] Category_Stalls model)
        {
            try
            {
                var validator = ValitRules<Category_Stalls>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    model.userAdded = userid(_httpContextAccessor);
                    var response = await this._categoryRepository.CategoryStallsCreate(model);
                    if (response == null)
                    {
                        return Ok(new ResponseSingleContentModel<Category_Stalls>
                        {
                            StatusCode = 500,
                            Message = "Không được thêm trùng code, name ",
                            Data = response
                        });
                    }
                    return Ok(new ResponseSingleContentModel<Category_Stalls>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = response
                    });
                }
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-stalls-modify")]
        public async Task<IActionResult> CategoryStallsModify([FromBody] Category_Stalls model)
        {
            try
            {
                var validator = ValitRules<Category_Stalls>
                    .Create()
                    //.Ensure(m => m.name, rule => rule.Required())
                    // .Ensure(m => m.code, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    //.Ensure(m => m.note, rule => rule.Required())
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    model.userUpdated = userid(_httpContextAccessor);

                    var response = await this._categoryRepository.CategoryStallsModify(model);
                    if (response == null)
                    {
                        return Ok(new ResponseSingleContentModel<Category_Stalls>
                        {
                            StatusCode = 500,
                            Message = "Không được cập nhật trùng code, name ",
                            Data = null
                        });
                    }
                    return Ok(new ResponseSingleContentModel<Category_Stalls>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = response
                    });
                }

                // Return invalidate data
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpGet("category-stalls-list")]
        public async Task<IActionResult> CategoryStallsList(string? keyword, long category_id)
        {
            try
            {
                var response = await this._categoryRepository.CategoryStallsList(keyword, category_id);
                return Ok(new ResponseSingleContentModel<List<Category_Stalls>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = response
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }

        [HttpPost("category-group-modify")]
        public async Task<IActionResult> CategoryGroupModify([FromBody] Category_Group model)
        {
            try
            {
                var validator = ValitRules<Category_Group>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    model.userAdded = userid(_httpContextAccessor);
                    var response = await this._categoryRepository.CategoryGroupModify(model);
                    if (response == null)
                    {
                        return Ok(new ResponseSingleContentModel<Category_Stalls>
                        {
                            StatusCode = 500,
                            Message = "Không được cập nhật trùng code, name ",
                            Data = null
                        });
                    }
                    return Ok(new ResponseSingleContentModel<Category_Group>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = response
                    });
                }
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpPost("category-group-create")]
        public async Task<IActionResult> CategoryGroupCreate([FromBody] Category_Group model)
        {
            try
            {
                var validator = ValitRules<Category_Group>
                    .Create()
                    //.Ensure(m => m.name, rule => rule.Required())
                    // .Ensure(m => m.code, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    //.Ensure(m => m.note, rule => rule.Required())
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    model.userUpdated = userid(_httpContextAccessor);

                    var response = await this._categoryRepository.CategoryGroupCreate(model);
                    if (response == null)
                    {
                        return Ok(new ResponseSingleContentModel<Category_Stalls>
                        {
                            StatusCode = 500,
                            Message = "Không được thêm mới trùng code, name ",
                            Data = null
                        });
                    }
                    return Ok(new ResponseSingleContentModel<Category_Group>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = response
                    });
                }

                // Return invalidate data
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [HttpGet("category-group-list")]
        public async Task<IActionResult> CategoryGroupList(string? keyword, long stalls_id)
        {
            try
            {
                var response = await this._categoryRepository.CategoryGroupList(keyword, stalls_id);
                return Ok(new ResponseSingleContentModel<List<Category_Group>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = response
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý",
                    Data = null
                });
            }
        }
        #endregion
    }
}
