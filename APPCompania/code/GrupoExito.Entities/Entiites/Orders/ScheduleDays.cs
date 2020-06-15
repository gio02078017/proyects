namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ScheduleDays
    {
        public ScheduleDays()
        {
            this.Hours = new List<ScheduleHours>();
        }

        [JsonProperty("idDay")]
        public int Id { get; set; }

        [JsonProperty("descriptionDay")]
        public string Description { get; set; }

        [JsonProperty("schedules")]
        public IList<ScheduleHours> Hours { get; set; }
    }
}
