namespace GrupoExito.Entities.Responses.Users
{
    using GrupoExito.Entities.Responses.Base;
    using System;

    public class PointsResponse : ResponseBase
    {
        public double AvailablePoints { get; set; }

        public double ExpirationPoints { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? AcumulatedDate { get; set; }

        public int StatusSystem { get; set; }
    }
}
