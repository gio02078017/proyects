namespace GrupoExito.Logic.Models.Generic
{
    using GrupoExito.Entities.Entiites.Generic.Contents;
    using GrupoExito.Entities.Parameters.Generic;
    using GrupoExito.Entities.Responses.Generic;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ContentsModel
    {
        private IContentsService _contentsService { get; set; }

        public ContentsModel(IContentsService contentsService)
        {
            _contentsService = contentsService;
        }

        public IList<Tutorial> GetTutorials()
        {
            return JsonService.Deserialize<List<Tutorial>>(AppConfigurations.TutorialsItem);
        }

        public async Task<ContentHomeResponse> GetContentHome()
        {
            return await _contentsService.GetContentHome().ConfigureAwait(false);
        }

        public async Task<PromotionResponse> GetPromotions(PromotionParameters parameters)
        {
            return await _contentsService.GetPromotions(parameters).ConfigureAwait(false);
        }
    }
}
