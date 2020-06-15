namespace GrupoExito.Logic.Models.Generic
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Utilities.Contracts.Generic;
    using System.Collections.Generic;

    public class DocumentsDataBaseModel
    {
        private IDocumentsDataBase _documentsDataBase { get; set; }

        public DocumentsDataBaseModel(IDocumentsDataBase documentsDataBase)
        {
            _documentsDataBase = documentsDataBase;
        }

        public Soat GetSoat(string plate)
        {
            return _documentsDataBase.GetSoat(plate);
        }

        public List<Soat> GetSoats()
        {
            return _documentsDataBase.GetSoats();
        }

        public void UpSertSoat(Soat soat)
        {
            _documentsDataBase.UpSertSoat(soat);
        }

        public void DeleteSoat(string plate)
        {
            _documentsDataBase.DeleteSoat(plate);
        }
    }
}
