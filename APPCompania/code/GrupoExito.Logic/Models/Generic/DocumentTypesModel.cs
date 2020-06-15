namespace GrupoExito.Logic.Models.Generic
{
    using GrupoExito.Entities.Responses.Generic;
    using GrupoExito.Utilities.Contracts.Generic;
    using System.Threading.Tasks;

    public class DocumentTypesModel
    {
        private IDocumentTypesService _documentTypesService { get; set; }

        public DocumentTypesModel(IDocumentTypesService documentTypesService)
        {
            _documentTypesService = documentTypesService;
        }

        public async Task<DocumentTypeResponse> GetDocumentTypes()
        {
            return await _documentTypesService.GetDocumentTypes();
        }
    }
}
