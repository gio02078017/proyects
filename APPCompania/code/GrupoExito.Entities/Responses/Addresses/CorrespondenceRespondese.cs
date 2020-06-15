namespace GrupoExito.Entities.Responses.Addresses
{
    using GrupoExito.Entities.Responses.Base;

    public class CorrespondenceRespondese : ResponseBase
    {
        public bool HaveCorreponseAddres { get; set; }

        public bool Error { get; set; }

        public string SuccessString { get; set; }
    }
}
