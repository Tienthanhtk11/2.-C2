using Point_of_Sale.Entities;
using Point_of_Sale.Model;
using Point_of_Sale.Model.User;

namespace Point_of_Sale.IRepositories
{
    public interface IUserRepository
    {
        Task<PaginationSet<UserModel>> UserList(string? full_name, string? username, int page_number, int page_size);
        Task<UserModel> UserGetById(long id);
        Task<UserModel> UserCreate(UserCreateModel useradd);
        Task<List<RolesModel>> GetRoleByUser(long user_id);
        Task<WarehouseModel> GetWarehouseUser(long user_id);
        Task<bool> ChangePassUser(ChangePassModel model);
        Task<Admin_User> CheckUser(string username);
        Task<int> CheckUserExists(string username, string phone_number, string email);
        int Authenticate(LoginModel login);
        Task<UserModifyModel> UserModify(UserModifyModel userupdate);
    }
}
