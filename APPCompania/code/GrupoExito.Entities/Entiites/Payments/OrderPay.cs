namespace GrupoExito.Entities.Entiites.Payments
{
    using Newtonsoft.Json;

    public class OrderPay
    {
        public string NumberDues { get; set; }
        public string Bin { get; set; }
        public string TypePayment { get; set; }
        public string PluDispatch { get; set; }
        public string TypeDispatch { get; set; }
        public string shippingCost { get; set; }
        public string TypeOperationPayment { get; set; }
        public string DateSelected { get; set; }
        public string TypeModality { get; set; }
        public string Number { get; set; }
        public int MinutesPromiseDelivery { get; set; }

        [JsonProperty("PaymentForm")]
        public int PaymentType { get; set; }

        [JsonProperty("placeOfDelivery")]
        public string Address { get; set; }

        [JsonProperty("userSchedule")]
        public string Schedule { get; set; }

        [JsonProperty("TypeShippingGroup")]
        public string TypeOfDispatch { get; set; }

        [JsonProperty("dependencyAddress")]
        public string DependencyAddress { get; set; }

        [JsonProperty("dependencyName")]
        public string DependencyName { get; set; }

        public bool Contingency { get; set; }
    }
}
