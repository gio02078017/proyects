namespace GrupoExito.Logic.Models.Users
{
    using GrupoExito.Entities.Responses.Users;
    using GrupoExito.Utilities.Contracts.Users;
    using System.Threading.Tasks;

    public class PointsModel
    {
        private IPointsService _pointsService { get; set; }

        public PointsModel(IPointsService pointsService)
        {
            _pointsService = pointsService;
        }

        public async Task<PointsResponse> GetUserPoints()
        {
            return await _pointsService.GetUserPoints();
        }
    }
}
