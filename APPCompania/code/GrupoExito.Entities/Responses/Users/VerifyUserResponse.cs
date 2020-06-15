namespace GrupoExito.Entities.Responses.Users
{
    using GrupoExito.Entities.Responses.Base;

    public class VerifyUserResponse : ResponseBase
    {
        public bool Verified { get; set; }
        public bool IsUpdated { get; set; }
    }
}
