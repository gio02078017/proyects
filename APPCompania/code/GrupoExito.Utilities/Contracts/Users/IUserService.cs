namespace GrupoExito.Utilities.Contracts.Users
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Parameters.Users;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Users;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<UserResponse> GetUser();
        Task<ResponseBase> UpdateUser(User user);
        Task<UserTypeResponse> GetUserType();
        Task<SendMessageVerifyUserResponse> SendMessageVerifyUser(VerifyUserParameters parameters);
        Task<VerifyUserResponse> VerifyUser(VerifyUserParameters parameters);
        Task<ResponseBase> UpdateCellPhone(UpdateCellPhoneParameters parameters);
        Task<VerifyUserResponse> UpdateVerifyUser(VerifyUserParameters parameters);
        Task<RegisterCostumerResponse> RegisterCostumer(RegisterCostumerParameters parameters);
    }
}
