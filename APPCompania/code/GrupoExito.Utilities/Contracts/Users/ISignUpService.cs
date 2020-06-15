namespace GrupoExito.Utilities.Contracts.Users
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Users;
    using System.Threading.Tasks;

    public interface ISignUpService
    {
        Task<ClientResponse> RegisterUser(User user);
    }
}
