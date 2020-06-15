using System;
using System.Collections.Generic;
using GrupoExito.Entities;

namespace GrupoExito.Models.Contracts
{
    public interface ICorrespondenceAddressModel
    {
        void HandleError(Exception ex);
        void CitiesFetched(IList<City> cities);
        void AddressSaved();
    }
}
