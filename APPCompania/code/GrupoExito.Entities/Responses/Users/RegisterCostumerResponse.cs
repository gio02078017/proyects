using GrupoExito.Entities.Responses.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrupoExito.Entities.Responses.Users
{
    public class RegisterCostumerResponse : ResponseBase
    {
        public bool Activate { get; set; }
    }
}
