using System;
using System.Collections.Generic;
using System.Text;

namespace GrupoExito.Entities.Parameters
{
    public class OrderScheduleParameters
    {
        public string DependencyId { get; set; }
        public string IdCity { get; set; }
        public string DeliveryMode { get; set; }
        public int QuantityUnits { get; set; }
    }
}
