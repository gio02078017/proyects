using System;
using System.Collections.Generic;
using System.Text;

namespace GrupoExito.Entities.Entiites
{
    public class OrderInformation
    {
        public List<Order> HomeDelivery { get; set; }

        public List<Order> PickUp { get; set; }
    }
}
