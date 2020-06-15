namespace GrupoExito.Utilities.Helpers
{
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class JsonService
    {
        public static T Deserialize<T>(string text)
        {
            T deserializedObject = JsonConvert.DeserializeObject<T>(text);
            return deserializedObject;
        }

        public static async Task<TResponse> GetSerializedResponse<TResponse>(HttpResponseMessage result)
        {            
            string response = await result.Content.ReadAsStringAsync();
            TResponse serializedResponse = JsonConvert.DeserializeObject<TResponse>(response);
            return serializedResponse;
        }

        public static string Serialize<T>(T entity)
        {
            return JsonConvert.SerializeObject(entity);
        }
    }
}