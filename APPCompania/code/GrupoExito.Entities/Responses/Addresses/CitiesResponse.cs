namespace GrupoExito.Entities.Responses.Addresses
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class CitiesResponse : ResponseBase
    {
        public CitiesResponse()
        {
            this.Cities = new List<City>();
        }

        [JsonProperty("cities")]
        public IList<City> Cities { get; set; }
    }
}
