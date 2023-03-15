using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SMS_Services.Model;
using SMS_Services.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SMS_Services.Controllers
{
    [Route("api/customer/")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ISMSRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public CustomerController(ISMSRepository Repository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this._config = configuration;
            this._repository = Repository;
            this._httpContextAccessor = httpContextAccessor;
            this._config = configuration;
        }
        #region Customer
        [HttpPost("create")]
        public async Task<IActionResult> CustomerCreate([FromBody] Customer model)
        {
            try
            {
                var response = await this._repository.CustomerCreate(model);
                return Ok(new ResponseSingleContentModel<Customer>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = response
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("modify")]
        public async Task<IActionResult> CustomerModify([FromBody] Customer model)
        {
            try
            {
                var Customer = await this._repository.CustomerModify(model);
                return Ok(new ResponseSingleContentModel<Customer>
                {
                    StatusCode = 200,
                    Message = "Cập nhật thành công",
                    Data = model
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
        [HttpGet("list")]
        public async Task<IActionResult> CustomerList(string? Customer_name)
        {
            try
            {
                var response = await _repository.CustomerList(Customer_name);

                return Ok(new ResponseSingleContentModel<List<Customer>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<List<Customer>>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = new()
                });
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> CustomerLogin(LoginModel login)
        {
            try
            {

                Customer user = await _repository.Customer_Check(login.user_name);
                if (user != null)
                {
                    int checkAccount = _repository.Customer_Authenticate(login);
                    UserTokenModel userAuthen = new();
                    if (checkAccount == 1)
                    {
                        ClaimModel claim = new ClaimModel
                        {
                            email = user.email,
                            name = user.name,
                            id = user.id,
                            user_name = user.user_name,
                        };
                        string tokenString = GenerateToken(claim);
                        userAuthen.token = tokenString;
                        userAuthen.id = user.id;
                        userAuthen.user_name = user.user_name;
                        userAuthen.name = user.name;
                        userAuthen.token = tokenString;
                        return Ok(new ResponseSingleContentModel<UserTokenModel>
                        {
                            StatusCode = 200,
                            Message = "Đăng nhập thành công",
                            Data = userAuthen
                        });
                    }
                    else
                    {
                        return Ok(new ResponseSingleContentModel<string>
                        {
                            StatusCode = 500,
                            Message = "Sai tài khoản hoặc mật khẩu",
                            Data = null
                        });
                    }
                }
                else
                {
                    return Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Tài khoản không tồn tại trong hệ thống",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = string.Empty
                });
            }
        }
        private string GenerateToken(ClaimModel user)
        {
            var identity = GetClaims(user);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["TokenSettings:Key"]));
            var token = new JwtSecurityToken(
              _config["TokenSettings:Issuer"],
              _config["TokenSettings:Audience"],
              expires: DateTime.Now.AddHours(9),
              claims: identity,
              signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
              );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private IEnumerable<Claim> GetClaims(ClaimModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.user_name.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.name),
                new Claim(JwtRegisteredClaimNames.Email, user.email),
                new Claim(JwtRegisteredClaimNames.Sid, user.id.ToString())
            };
            return claims;
        }
        #endregion
    }
}
