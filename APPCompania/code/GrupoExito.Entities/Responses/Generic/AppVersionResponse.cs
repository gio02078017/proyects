namespace GrupoExito.Entities.Responses.Generic
{
    using GrupoExito.Entities.Responses.Base;

    public class AppVersionResponse : ResponseBase
    {
        public string MinimumVersion { get; set; }

        public string CurrentVersion { get; set; }
    }
}
