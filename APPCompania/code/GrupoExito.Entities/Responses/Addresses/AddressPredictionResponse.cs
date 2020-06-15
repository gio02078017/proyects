namespace GrupoExito.Entities.Responses.Addresses
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class AddressPredictionResponse : ResponseBase
    {
        public AddressPredictionResponse()
        {
            Predictions = new List<Prediction>();
        }

        [JsonProperty("predictions")]
        public IList<Prediction> Predictions { get; set; }
    }
}
