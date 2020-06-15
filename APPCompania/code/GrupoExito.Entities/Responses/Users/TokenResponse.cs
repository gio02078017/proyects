namespace GrupoExito.Entities.Responses.Users
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Base;

    public class TokenResponse : ResponseBase
    {
        public Token Token { get; set; }
    }
}
