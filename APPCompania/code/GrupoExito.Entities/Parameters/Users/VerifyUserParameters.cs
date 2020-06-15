namespace GrupoExito.Entities.Parameters.Users
{
    public class VerifyUserParameters
    {
        public string DocumentNumber { get; set; }

        public string CellPhone { get; set; }

        public string SiteId { get; set; }

        public string Code { get; set; }

        public bool IsValidated { get; set; }
    }
}
