namespace GrupoExito.Entities.Responses.Users
{
    using GrupoExito.Entities.Responses.Base;

    public class UserTypeResponse : ResponseBase
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
