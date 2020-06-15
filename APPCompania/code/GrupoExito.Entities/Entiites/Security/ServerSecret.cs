namespace GrupoExito.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ServerSecret
    {
        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }
    }
}
