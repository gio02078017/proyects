namespace GrupoExito.Utilities.Contracts.Users
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Users;
    using System.Threading.Tasks;

    public interface ILoginService
    {
        Task<LoginResponse> Login(UserCredentials user);
    }
}
