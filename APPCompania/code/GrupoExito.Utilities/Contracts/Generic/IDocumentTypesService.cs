namespace GrupoExito.Utilities.Contracts.Generic
{
    using GrupoExito.Entities.Responses.Generic;
    using System.Threading.Tasks;

    public interface IDocumentTypesService
    {
        Task<DocumentTypeResponse> GetDocumentTypes();
    }
}
