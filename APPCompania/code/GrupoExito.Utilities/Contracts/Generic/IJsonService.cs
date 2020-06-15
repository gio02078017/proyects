namespace GrupoExito.Utilities.Contracts.Generic
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IJsonService
    {
        T Deserialize<T>(string text);

        Task<TResponse> GetSerializedResponse<TResponse>(HttpResponseMessage result);

        string Serialize<T>(T entity);
    }
}
