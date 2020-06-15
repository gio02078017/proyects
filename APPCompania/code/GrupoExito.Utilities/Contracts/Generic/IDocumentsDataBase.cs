namespace GrupoExito.Utilities.Contracts.Generic
{
    using GrupoExito.Entities.Entiites;
    using System.Collections.Generic;

    public interface IDocumentsDataBase
    {
        #region Soat

        Soat GetSoat(string plate);

        List<Soat> GetSoats();

        void UpSertSoat(Soat soat);

        void DeleteSoat(string plate);

        #endregion

        void Dispose();
    }
}
