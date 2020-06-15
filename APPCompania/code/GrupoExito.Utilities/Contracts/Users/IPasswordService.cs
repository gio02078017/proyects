namespace GrupoExito.Utilities.Contracts.Users
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Base;
    using System.Threading.Tasks;

    public interface IPasswordService
    {
        Task<ResponseBase> ChangePassword(UserCredentials userCredentials);

        Task<ResponseBase> ResetPassword(string email);
    }
}
