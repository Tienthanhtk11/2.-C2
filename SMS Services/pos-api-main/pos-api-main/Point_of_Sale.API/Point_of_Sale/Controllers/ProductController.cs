using ECom.Framework.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Product;

namespace Point_of_Sale.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductRepository _productRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductController(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("product")]
        public async Task<IActionResult> Product(long id)
        {
            try
            {
                var products = await this._productRepository.Product(id);
                return Ok(new ResponseSingleContentModel<ProductModel>
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
        [HttpPost("product-create")]
        public async Task<IActionResult> ProductCreate([FromBody] Product model)
        {
            try
            {
                var validator = ValitRules<Product>
                    .Create()
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    if (!await this._productRepository.ProductCheckDuplicate(model))
                    {
                        return Ok(new ResponseSingleContentModel<Product>
                        {
                            StatusCode = 400,
                            Message = "Thêm mới trùng bản ghi",
                            Data = model
                        });
                    }
                    model.userAdded = userid(_httpContextAccessor);
                    var product = await this._productRepository.ProductCreate(model);
                    return Ok(new ResponseSingleContentModel<Product>
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
        [HttpPost("product-modify")]
        public async Task<IActionResult> ProductModify([FromBody] Product model)
        {
            try
            {
                var validator = ValitRules<Product>
                    .Create()
                    .Ensure(m => m.price, rule => rule.IsGreaterThan(0))
                  .Ensure(m => m.barcode, rule => rule.Required())
                    .Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    //.Ensure(m => m.note, rule => rule.Required())
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    if (!await this._productRepository.ProductCheckDuplicate(model))
                    {
                        return Ok(new ResponseSingleContentModel<Product>
                        {
                            StatusCode = 400,
                            Message = "Cập nhật trùng bản ghi",
                            Data = model
                        });
                    }
                    var product = await this._productRepository.ProductModify(model);
                    model.userUpdated = userid(_httpContextAccessor);
                    return Ok(new ResponseSingleContentModel<Product>
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
        [HttpDelete("product-delete")]
        public async Task<IActionResult> ProductDelete(long product_id)
        {
            try
            {
                long user_id = userid(_httpContextAccessor);

                var response = await this._productRepository.ProductDelete(product_id, user_id);
                return response
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = ""
                    })
                    : (IActionResult)Ok(new ResponseSingleContentModel<IResponseData>
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
        [HttpPost("product-list")]
        public async Task<IActionResult> ProductList(ProductSearchModel search)
        {
            try
            {
                var products = await this._productRepository.ProductList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<ProductViewModel>>
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

        [HttpGet("product-warehouse-list")]
        public async Task<IActionResult> ProductWarehouseList(string? keyword, int page_size, int page_number, long warehouse_id)
        {
            try
            {
                var products = await this._productRepository.ProductWarehouseList(keyword, page_size, page_number, warehouse_id);
                return Ok(new ResponseSingleContentModel<PaginationSet<ProductWarehouseModel>>
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
        [HttpGet("product-warehouse-list2")]
        public async Task<IActionResult> ProductWarehouseList2(string? keyword, int page_size, int page_number, long warehouse_id)
        {
            try
            {
                var products = await this._productRepository.ProductWarehouseList2(keyword, page_size, page_number, warehouse_id);
                return Ok(new ResponseSingleContentModel<PaginationSet<ProductWarehouseModel2>>
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
        [HttpGet("product-warehouse-stock")]
        public async Task<IActionResult> ProductStockList(string? keyword, int page_size, int page_number, long warehouse_id)
        {
            try
            {
                var products = await this._productRepository.ProductStockList(keyword, page_size, page_number, warehouse_id);
                return Ok(new ResponseSingleContentModel<PaginationSet<ProductWarehouseModel>>
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

        [HttpGet("product-request-warehouse-list")]
        public async Task<IActionResult> ProductRequsetWarehouseList(string? keyword, int page_size, int page_number, long warehouse_id)

        {
            try
            {
                var products = await this._productRepository.ProductRequestWarehouseList(keyword, page_size, page_number, warehouse_id);
                return Ok(new ResponseSingleContentModel<PaginationSet<ProductRequestWarehouseModel>>
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
        [HttpGet("product-request-warehouse-list2")]
        public async Task<IActionResult> ProductRequestWarehouseList2(long partner_id, long warehouse_id)

        {
            try
            {
                var products = await this._productRepository.ProductRequestWarehouseList2(partner_id, warehouse_id);
                return Ok(new ResponseSingleContentModel<List<ProductRequestWarehouseModel2>>
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
        [HttpGet("product-warehouse-barcode")]
        public async Task<IActionResult> ProductWarehouse(string barcode, long warehouse_id)
        {
            try
            {
                var products = await this._productRepository.ProductWarehouse(barcode, warehouse_id);
                return Ok(new ResponseSingleContentModel<ProductWarehouseModel>
                {
                    StatusCode = 200,
                    Message = "Lấy thông tin thành công.",
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
        [HttpGet("product-warehouse")]
        public async Task<IActionResult> ProductWarehouse(long id)
        {
            try
            {
                var products = await this._productRepository.ProductWarehouseGetById(id);
                return Ok(new ResponseSingleContentModel<Product_Warehouse>
                {
                    StatusCode = 200,
                    Message = "Lấy thông tin thành công.",
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
        [HttpPost("product-detail-warehouse-list")]
        public async Task<IActionResult> ProductDetailWarehouseList(ProductSearchModel search)
        {
            try
            {
                var products = await this._productRepository.ProductDetailWarehouseList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<ProductWarehouseModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy thông tin thành công.",
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
        [HttpPost("product-warehouse-modify")]
        public async Task<IActionResult> ProductWarehouseModify([FromBody] Product_Warehouse model)
        {
            try
            {
                var validator = ValitRules<Product_Warehouse>
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
                    var product = await this._productRepository.ProductWarehouseModify(model);
                    return product.id == 0
                        ? Ok(new ResponseSingleContentModel<Product_Warehouse>
                        {
                            StatusCode = 400,
                            Message = "Nhập trùng barcode",
                            Data = model
                        })
                        : (IActionResult)Ok(new ResponseSingleContentModel<Product_Warehouse>
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

        [HttpPost("product-warehouse-modify-print-barcode")]
        public async Task<IActionResult> ProductWarehouseModifyPrintBarcode([FromBody] List<long> products)
        {
            try
            {

                string check = await this._productRepository.ProductWarehouseModifyPrintBarcode(products);
                return check == "0"
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = null
                    })
                    : (IActionResult)Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Có lỗi trong quá trình xử lý " + check,
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
        [HttpPost("product-warehouse-modify-print-price")]
        public async Task<IActionResult> ProductWarehouseModifyPrintPrice([FromBody] List<long> products)
        {
            try
            {

                string check = await this._productRepository.ProductWarehouseModifyPrintPrice(products);
                return check == "0"
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = null
                    })
                    : (IActionResult)Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Có lỗi trong quá trình xử lý " + check,
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

        [HttpPost("update-product-warehouse")]
        public async Task<IActionResult> UpdateFromExcel()
        {
            var files = Request.Form.Files;
            if (files.Any(f => f.Length == 0))
                return BadRequest();

            var stream = files[0].OpenReadStream();

            await this._productRepository.UpdateFromExcel(stream);

            return Ok();
        }

        [HttpPost("product-change-price-history-list")]
        public async Task<IActionResult> GetChangePriceHistory(SearchWarehousePriceHistory search)
        {
            try
            {
                var productsHistory = await this._productRepository.GetChangePriceHistory(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<WarehousePriceHistoryModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy thông tin thành công.",
                    Data = productsHistory
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

        [HttpPost("product-promotion-warehouse-list")]
        public async Task<IActionResult> ProductWarehousePromotionList(SearchPromotionModel search)
        {
            try
            {
                var products = await this._productRepository.ProductWarehousePromotionList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<ProductWarehouseModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy thông tin thành công.",
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

        // sản phẩm + NCC
        [HttpPost("product-partner-create")]
        public async Task<IActionResult> ProductPartnerCreate([FromBody] Product_Partner_Model model)
        {
            try
            {
                var response = await this._productRepository.Product_Partner_Create(model);
                if (response == null)
                {
                    return Ok(new ResponseSingleContentModel<IResponseData>
                    {
                        StatusCode = 500,
                        Message = "Đã tồn tại sản phẩm này trong danh sách.",
                        Data = null
                    });
                }
                return Ok(new ResponseSingleContentModel<Product_Partner_Model>
                {
                    StatusCode = 200,
                    Message = "Tạo mới thành công.",
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
        [HttpPost("product-partner-update")]
        public async Task<IActionResult> ProductPartnerUpdate([FromBody] Product_Partner_Model model)
        {
            try
            {

                var response = await this._productRepository.Product_Partner_Update(model);
                if (response == null)
                {
                    return Ok(new ResponseSingleContentModel<IResponseData>
                    {
                        StatusCode = 500,
                        Message = "Đã tồn tại sản phẩm này trong danh sách.",
                        Data = null
                    });
                }
                return Ok(new ResponseSingleContentModel<Product_Partner_Model>
                {
                    StatusCode = 200,
                    Message = "Cập nhật thành công.",
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
        [HttpGet("get-product-partner-by-partner")]
        public async Task<IActionResult> ProductPartnerListByPartner(long partner_id, string? keyword)
        {
            try
            {
                var response = await this._productRepository.Product_Partner_List_By_Partner(partner_id, keyword);
                return Ok(new ResponseSingleContentModel<List<Product_Partner_Model>>
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
        [HttpGet("get-product-partner-by-product")]
        public async Task<IActionResult> ProductPartnerListByProduct(long product_id)
        {
            try
            {
                var response = await this._productRepository.Product_Partner_List_By_Product(product_id);
                return Ok(new ResponseSingleContentModel<List<Product_Partner_Model>>
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
        // sản phẩm + combo
        [HttpPost("combo-create")]
        public async Task<IActionResult> Product_Structure_Create([FromBody] Combo_Model model)
        {
            try
            {
                var response = await this._productRepository.ComboCreate(model);
                if (response == null)
                    return Ok(new ResponseSingleContentModel<Combo_Model>
                    {
                        StatusCode = 400,
                        Message = "Không được tạo barcode trùng với sản phẩm/combo đã có",
                        Data = null
                    });
                return Ok(new ResponseSingleContentModel<Combo_Model>
                {
                    StatusCode = 200,
                    Message = "Tạo mới thành công.",
                    Data = response
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
        [HttpPost("combo-update")]
        public async Task<IActionResult> comboUpdate([FromBody] Combo_Model model)
        {
            try
            {
                var response = await this._productRepository.ComboUpdate(model);
                if (response == null)
                    return Ok(new ResponseSingleContentModel<Combo_Model>
                    {
                        StatusCode = 400,
                        Message = "Không được tạo barcode trùng với sản phẩm/combo đã có",
                        Data = null
                    });
                return Ok(new ResponseSingleContentModel<Combo_Model>
                {
                    StatusCode = 200,
                    Message = "Cập nhật thành công.",
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
        [HttpGet("combo-detail")]
        public async Task<IActionResult> ComboDetail(long combo_id)
        {
            try
            {
                var response = await this._productRepository.Combo_Detail(combo_id);
                return Ok(new ResponseSingleContentModel<Combo_Model>
                {
                    StatusCode = 200,
                    Message = "Lấy thông tin thành công.",
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
        [HttpPost("combo-list")]
        public async Task<IActionResult> ComboList(ProductSearchModel search)
        {
            try
            {
                var products = await this._productRepository.ComboList(search);
                return Ok(new ResponseSingleContentModel<PaginationSet<ProductWarehouseModel2>>
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
        [AllowAnonymous]
        [HttpGet("sync-non-unicode")]
        public async Task<IActionResult> Sync_Non_Unicode()
        {
            try
            {
                var response = await this._productRepository.Sync_Non_Unicode();
                return Ok(new ResponseSingleContentModel<bool>
                {
                    StatusCode = 200,
                    Message = "Cập nhật tin thành công.",
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
    }
}
