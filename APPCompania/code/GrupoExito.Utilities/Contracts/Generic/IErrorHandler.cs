using System;
namespace GrupoExito.Utilities.Contracts.Generic
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
