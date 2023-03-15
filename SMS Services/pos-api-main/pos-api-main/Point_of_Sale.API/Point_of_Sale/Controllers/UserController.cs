using Point_of_Sale.Entities;
using Point_of_Sale.Extensions;
using Point_of_Sale.Model;
using Point_of_Sale.Model.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECom.Framework.Validator;
using Point_of_Sale.IRepositories;
using System.ComponentModel.DataAnnotations;

namespace Point_of_Sale.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserController(IUserRepository userRepository, IAdminRepository adminRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        #region user
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpPost("admin-user-create")]
        public async Task<IActionResult> UserCreate([FromBody] UserCreateModel model)
        {
            try
            {
                int checkUser = await _userRepository.CheckUserExists(model.username, model.phone_number, model.email);
                if (checkUser > 0)
                {
                    return Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 400,
                        Message = "Tài khoản, email hoặc số điện thoại đã được đăng ký vui lòng kiểm tra lại",
                        Data = string.Empty
                    });
                }
                var validator = ValitRules<UserCreateModel>
                    .Create()
                    .Ensure(m => m.full_name, rule => rule.Required())
                    .Ensure(m => m.email, rule => rule.Required())
                    .Ensure(m => m.phone_number, rule => rule.Required())
                    .Ensure(m => m.address, rule => rule.Required())
                    .Ensure(m => m.username, rule => rule.Required())
                    .Ensure(m => m.code, rule => rule.Required())
                    .Ensure(m => m.password, rule => rule.Required())
                    .Ensure(m => m.province_id, rule => rule.IsGreaterThan(0))
                    .Ensure(m => m.district_id, rule => rule.IsGreaterThan(0))
                    .Ensure(m => m.ward_id, rule => rule.IsGreaterThan(0))
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    var user = await this._userRepository.UserCreate(model);

                    return Ok(new ResponseSingleContentModel<UserModel>
                    {
                        StatusCode = 200,
                        Message = "",
                        Data = user
                    });
                }

                // Return invalidate data
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = string.Empty
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();


            }
        }
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpPost("admin-user-modify")]
        public async Task<IActionResult> UserModify([FromBody] UserModifyModel userupdate)
        {
            try
            {
                var validator = ValitRules<UserModifyModel>
                    .Create()
                    .Ensure(m => m.username, rule => rule.Required())
                     .Ensure(m => m.code, rule => rule.Required())
                     .Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    .For(userupdate)
                    .Validate();
                if (validator.Succeeded)
                {
                    var user = await this._userRepository.UserModify(userupdate);

                    return Ok(new ResponseSingleContentModel<UserModifyModel>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = userupdate
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
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpGet("admin-user-list")]
        public async Task<IActionResult> UserList(string? full_name, string? username, int page_number = 0, int page_size = 20)
        {
            try
            {
                PaginationSet<UserModel> Data = await _userRepository.UserList(full_name, username, page_number, page_size);

                return Ok(new ResponseSingleContentModel<PaginationSet<UserModel>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = Data
                });
            }
            catch (Exception)
            {

                return Ok(new ResponseSingleContentModel<UserModel>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = new()
                });
            }

        }
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpGet("admin-authorize-check")]
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        public async Task<IActionResult> GetUserById()
        {
            try
            {
                long id = userid(_httpContextAccessor);
                var user = await _userRepository.UserGetById(id);


                return Ok(new ResponseSingleContentModel<UserModel>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = user
                });
            }
            catch (Exception ex)
            {

                return Ok(new ResponseSingleContentModel<UserModel>
                {
                    StatusCode = 500,
                    Message = "Đăng nhập không thành công " + ex.Message,
                    Data = new()
                });
            }
        }
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpGet("admin-user")]
        public async Task<IActionResult> GetUserById(long id)
        {
            try
            {
                var user = await _userRepository.UserGetById(id);
                return Ok(new ResponseSingleContentModel<UserModel>
                {
                    StatusCode = 200,
                    Message = "Thêm mới thành công",
                    Data = user
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = string.Empty
                });
            }
        }
        [AllowAnonymous]
        [HttpPost("admin-login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            try
            {
                var validator = ValitRules<LoginModel>
                    .Create()
                    .Ensure(m => m.username, rule => rule.Required())
                    .Ensure(m => m.password, rule => rule.Required())
                    .For(login)
                    .Validate();

                if (validator.Succeeded)
                {
                    var user = await _userRepository.CheckUser(login.username);
                    if (user != null)
                    {

                        int checkAccount = _userRepository.Authenticate(login);
                        UserTokenModel userAuthen = new();
                        if (checkAccount == 1)
                        {
                            List<RolesModel> roles = await _userRepository.GetRoleByUser(user.id);
                            ClaimModel claim = new ClaimModel
                            {
                                email = user.email,
                                full_name = user.full_name,
                                id = user.id,
                                type = user.type,
                                username = user.username,
                                roles = roles,
                            };
                            string tokenString = GenerateToken(claim);
                            userAuthen.token = tokenString;
                            userAuthen.id = user.id;

                            userAuthen.username = user.username;
                            userAuthen.full_name = user.full_name;
                            userAuthen.token = tokenString;
                            userAuthen.roles = roles;
                            userAuthen.warehouse = await _userRepository.GetWarehouseUser(user.id);//đây là warehouse mặc định chọn trong danh sách warehouse được phân quyền
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
                // Return invalidate data
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = string.Empty
                });
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
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpPost("admin-user-changepass")]
        public async Task<IActionResult> ChangePassUser(ChangePassModel model)
        {
            try
            {
                var validator = ValitRules<ChangePassModel>
                    .Create()
                    .Ensure(m => m.passwordNew, rule => rule.Required())
                    .Ensure(m => m.passwordOld, rule => rule.Required())
                    .Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    .For(model)
                    .Validate();
               
                if (validator.Succeeded)
                {
                    bool user = await _userRepository.ChangePassUser(model);

                    return Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = null
                    });
                }

                // Return invalidate data
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 400,
                    Message = validator.ErrorMessages.JoinNewLine(),
                    Data = null
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = string.Empty
                });
                // return this.RouteToInternalServerError();
            }
        }
        private string GenerateToken(ClaimModel user)
        {
            var identity = GetClaims(user);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["TokenSettings:Key"]));
            var token = new JwtSecurityToken(
            _configuration["TokenSettings:Issuer"],
             _configuration["TokenSettings:Audience"],
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
                new Claim(JwtRegisteredClaimNames.Typ, user.type.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.username.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.full_name),
                new Claim(JwtRegisteredClaimNames.Email, user.email),
                new Claim(JwtRegisteredClaimNames.Sid, user.id.ToString())
            };

            foreach (var userRole in user.roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.role));
            }

            return claims;
        }
        #endregion

        #region admingroup
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpPost("admin-group-create")]
        public async Task<IActionResult> AdminGroupCreate([FromBody] Admin_Group model)
        {
            try
            {
                var validator = ValitRules<Admin_Group>
                    .Create()
                    .Ensure(m => m.name, rule => rule.Required())
                     .Ensure(m => m.code, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    model.userAdded = userid(_httpContextAccessor);
                    var group = await this._adminRepository.GroupCreate(model);
                    return Ok(new ResponseSingleContentModel<Admin_Group>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = group
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
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpPost("admin-group-modify")]
        public async Task<IActionResult> AdminGroupModify([FromBody] Admin_Group model)
        {
            try
            {
                var validator = ValitRules<Admin_Group>
                    .Create()
                    .Ensure(m => m.name, rule => rule.Required())
                     .Ensure(m => m.code, rule => rule.Required())
                    .Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    //.Ensure(m => m.note, rule => rule.Required())
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    var group = await this._adminRepository.GroupModify(model);

                    return Ok(new ResponseSingleContentModel<Admin_Group>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = group
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
        //[Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpGet("admin-group-list")]
        public async Task<IActionResult> AdminGroupList(long id)
        {
            try
            {
                var group = await _adminRepository.GroupList(id);
                return Ok(new ResponseSingleContentModel<List<Admin_Group>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = group
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
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpDelete("admin-group-delete")]
        public async Task<IActionResult> AdminGroupDelete(long id)
        {
            try
            {
                bool group = await _adminRepository.GroupDelete(id);
                return group
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

        #region adminGroupUser
        //[Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpGet("admin-group-user-list")]
        public async Task<IActionResult> AdminGroupUserList(long user_id)
        {
            try
            {
                var groupuser = await this._adminRepository.GroupUserList(user_id);

                return Ok(new ResponseSingleContentModel<List<Admin_Group_User>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = groupuser
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpGet("admin-group-user-list-by-group-id")]
        public async Task<IActionResult> AdminGroupUserListByGroupId(long group_id)
        {
            try
            {
                var groupuser = await this._adminRepository.GroupUserListByGroupId(group_id);

                return Ok(new ResponseSingleContentModel<List<Admin_Group_User>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = groupuser
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpPost("admin-group-user-create-list")]
        public async Task<IActionResult> AdminGroupUserCreateList([FromBody] List<Admin_Group_User> model)
        {
            try
            {
                long userAdded = userid(_httpContextAccessor);
                var groupusers = await _adminRepository.GroupUserCreateList(model, userAdded);

                return Ok(new ResponseSingleContentModel<List<Admin_Group_User>>
                {
                    StatusCode = 200,
                    Message = "Thêm mới thành công",
                    Data = groupusers
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
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpPost("admin-group-user-modify-list")]
        public async Task<IActionResult> AdminGroupUserModifyList([FromBody] List<Admin_Group_User> model)
        {
            try
            {
                long userModify = userid(_httpContextAccessor);
                var groupusers = await _adminRepository.GroupUserModifyList(model, userModify);

                return Ok(new ResponseSingleContentModel<List<Admin_Group_User>>
                {
                    StatusCode = 200,
                    Message = "Cập nhật thành công",
                    Data = groupusers
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
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpDelete("admin-group-user-delete")]
        public async Task<IActionResult> AdminGroupUserDelete(long id)
        {
            try
            {

                string? delete = await _adminRepository.GroupUserDelete(id);
                return delete == "0"
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Success",
                        Data = null
                    })
                    : (IActionResult)Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Xóa bản ghi không thành công " + delete,
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

        #region adminRole
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpPost("admin-role-create")]
        public async Task<IActionResult> AdminRoleCreate([FromBody] Admin_Role model)
        {
            try
            {
                var validator = ValitRules<Admin_Role>
                    .Create()
                    .Ensure(m => m.name, rule => rule.Required())
                     .Ensure(m => m.code, rule => rule.Required())
                    //.Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    .For(model)
                    .Validate();
                if (validator.Succeeded)
                {
                    model.userAdded = userid(_httpContextAccessor);
                    var role = await this._adminRepository.RoleCreate(model);
                    return Ok(new ResponseSingleContentModel<Admin_Role>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = role
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
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpPost("admin-role-modify")]
        public async Task<IActionResult> AdminRoleModify([FromBody] Admin_Role model)
        {
            try
            {
                var validator = ValitRules<Admin_Role>
                    .Create()
                    .Ensure(m => m.name, rule => rule.Required())
                     .Ensure(m => m.code, rule => rule.Required())
                    .Ensure(m => m.id, rule => rule.IsGreaterThan(0))
                    //.Ensure(m => m.note, rule => rule.Required())
                    .For(model)
                    .Validate();

                if (validator.Succeeded)
                {
                    var role = await this._adminRepository.RoleModify(model);

                    return Ok(new ResponseSingleContentModel<Admin_Role>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = role
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
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpGet("admin-role-list")]
        public async Task<IActionResult> AdminRoleList(long id)
        {
            try
            {
                var role = await this._adminRepository.RoleList(id);
                return Ok(new ResponseSingleContentModel<List<Admin_Role>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công.",
                    Data = role
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
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpDelete("admin-role-delete")]
        public async Task<IActionResult> AdminRoleDelete(long id)
        {
            try
            {
                var role = await this._adminRepository.RoleDelete(id);
                return role
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

        #region adminRoleGroup
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpGet("admin-role-group-list")]
        public async Task<IActionResult> AdminRoleGroupList(long role_id)
        {
            try
            {
                var rolegroups = await this._adminRepository.RoleGroupList(role_id);

                return Ok(new ResponseSingleContentModel<List<Admin_Role_Group>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = rolegroups
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpGet("admin-role-group-list-by-group-id")]
        public async Task<IActionResult> AdminRoleGroupListByGroupId(long group_id)
        {
            try
            {
                var rolegroups = await this._adminRepository.RoleGroupListByGroupId(group_id);

                return Ok(new ResponseSingleContentModel<List<Admin_Role_Group>>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = rolegroups
                });
            }
            catch (Exception)
            {
                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý",
                    Data = null
                });
            }
        }
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpPost("admin-role-group-create-list")]
        public async Task<IActionResult> AdminRoleGroupCreateList([FromBody] List<Admin_Role_Group> model)
        {
            try
            {
                var userAdded = userid(_httpContextAccessor);
                var rolegroups = await this._adminRepository.RoleGroupCreateList(model, userAdded);

                return Ok(new ResponseSingleContentModel<List<Admin_Role_Group>>
                {
                    StatusCode = 200,
                    Message = "Thêm mới thành công",
                    Data = rolegroups
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
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpPost("admin-role-group-modify-list")]
        public async Task<IActionResult> AdminRoleGroupModifyList([FromBody] List<Admin_Role_Group> model)
        {
            try
            {
                var userModify = userid(_httpContextAccessor);
                var rolegroups = await this._adminRepository.RoleGroupModifyList(model, userModify);

                return Ok(new ResponseSingleContentModel<List<Admin_Role_Group>>
                {
                    StatusCode = 200,
                    Message = "Cập nhật thành công",
                    Data = rolegroups
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
        [Authorize(Roles = "QUANLYNGUOIDUNG")]
        [HttpDelete("admin-role-group-delete")]
        public async Task<IActionResult> AdminRoleGroupDelete(long id)
        {
            try
            {
                var delete = await this._adminRepository.RoleGroupDelete(id);
                return delete == "0"
                    ? Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 200,
                        Message = "Success",
                        Data = null
                    })
                    : (IActionResult)Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Xóa bản ghi không thành công " + delete,
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

        #region adminUserWarehouse       
        [HttpGet("admin-user-warehouse-list")]
        public async Task<IActionResult> UserWarehouseList(long user_id)
        {
            try
            { 
                var userwarehouse = await _adminRepository.UserWarehouseList(user_id);
                return Ok(new ResponseSingleContentModel<List<Admin_User_Warehouse>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách thành công",
                    Data = userwarehouse
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

        [HttpPost("admin-user-warehouse-create")]
        public async Task<IActionResult> UserWarehouseCreate([FromBody] List<Admin_User_Warehouse> model)
        {
            try
            {
                bool validate = true;
                foreach (var item in model)
                {
                    var validator = ValitRules<Admin_User_Warehouse>
                   .Create()
                   .For(item)
                   .Validate();
                    if (!validator.Succeeded)
                        validate = false;
                }

                if (validate)
                {
                    model.ForEach(item => item.userAdded = userid(_httpContextAccessor));
                    var userwarehouse = await this._adminRepository.UserWarehouseCreate(model);
                    return Ok(new ResponseSingleContentModel<List<Admin_User_Warehouse>>
                    {
                        StatusCode = 200,
                        Message = "Thêm mới thành công",
                        Data = userwarehouse
                    });
                }

                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = "Lỗi trong quá trình validate dữ liệu",
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
        [HttpPost("admin-user-warehouse-modify")]
        public async Task<IActionResult> UserWarehouseModify([FromBody] List<Admin_User_Warehouse> model)
        {
            try
            {
                var validate = true;
                foreach (var item in model)
                {
                    var validator = ValitRules<Admin_User_Warehouse>
                   .Create()
                   .For(item)
                   .Validate();
                    if (!validator.Succeeded)
                        validate = false;
                }

                if (validate)
                {
                    model.ForEach(item => item.userAdded = userid(_httpContextAccessor));
                    var userwarehouse = await _adminRepository.UserWarehouseModify(model);
                    return Ok(new ResponseSingleContentModel<List<Admin_User_Warehouse>>
                    {
                        StatusCode = 200,
                        Message = "Cập nhật thành công",
                        Data = userwarehouse
                    });
                }

                return Ok(new ResponseSingleContentModel<IResponseData>
                {
                    StatusCode = 400,
                    Message = "Lỗi trong quá trình validate dữ liệu",
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
        [HttpDelete("admin-user-warehouse-delete")]
        public async Task<IActionResult> CategoryProvinceDelete(long id)
        {
            try
            {
                var userwarehouse = await _adminRepository.UserWarehouseDelete(id);
                return userwarehouse
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
    }
}
