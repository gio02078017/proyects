using System;
using System.Collections.Generic;
using GrupoExito.Entities;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Contracts.Generic;

namespace GrupoExito.Models.Contracts
{
    public interface ISummaryViewModel : IErrorHandler
    {
        void HasNotCorrespondenceAddress();
        void OrderUploaded();
        void ConnectionUnavailable();
        void ProductsDeletedDueToDependencyChange(List<Product> deletedProducts);
    }
}
