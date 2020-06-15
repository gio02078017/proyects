namespace GrupoExito.Logic.Models.Generic
{
    using GrupoExito.Entities.Responses.Generic;
    using GrupoExito.Utilities.Contracts.Generic;
    using System.Threading.Tasks;

    public class AppVersionModel
    {
        private IAppVersionService _appVersionService { get; set; }

        public AppVersionModel(IAppVersionService appVersionService)
        {
            _appVersionService = appVersionService;
        }

        public async Task<AppVersionResponse> GetAppVersion(string operatingASystem)
        {
            return await _appVersionService.GetAppVersion(operatingASystem).ConfigureAwait(false);
        }
    }
}
