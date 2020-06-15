namespace GrupoExito.DataAgent.DataBase
{
    using Couchbase.Lite;
    using Couchbase.Lite.Query;
    using GrupoExito.Entities.Constants.Products;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using System;
    using System.Collections.Generic;

    public class DocumentsDataBase : IDocumentsDataBase
    {
        #region Attributes

        private Database database;

        private static DocumentsDataBase instance;

        public static DocumentsDataBase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DocumentsDataBase();
                }

                return instance;
            }
        }

        #endregion

        #region Constructor

        public DocumentsDataBase()
        {
            if (database == null)
            {
                database = new Database(ConstDataBase.Document);
            }
        }

        #endregion

        #region Soat

        public Soat GetSoat(string plate)
        {
            var document = database.GetDocument(plate);

            if (document != null)
            {
                return JsonService.Deserialize<Soat>(document.GetString(ConstDataBase.Soat));
            }

            return null;
        }

        public List<Soat> GetSoats()
        {
            List<Soat> soats = new List<Soat>();

            using (var query = QueryBuilder.Select(SelectResult.All()).From(DataSource.Database(database)))
            {
                foreach (Result item in query.Execute())
                {
                    var dict = item.GetDictionary(database.Name);

                    string value = dict?.GetString(ConstDataBase.Soat);

                    if (!string.IsNullOrEmpty(value))
                    {
                        soats.Add(JsonService.Deserialize<Soat>(value));
                    }
                }
            }

            return soats;
        }

        public void UpSertSoat(Soat soat)
        {
            var document = database.GetDocument(soat.Plate);
            document = document ?? new MutableDocument(soat.Plate);

            using (var mutableDoc = document.ToMutable())
            {
                mutableDoc.SetString(ConstDataBase.Soat, JsonService.Serialize<Soat>(soat));
                database.Save(mutableDoc);
            }
        }

        public void DeleteSoat(string plate)
        {
            var document = database.GetDocument(plate);

            if (document != null)
            {
                using (var mutableDoc = document.ToMutable())
                {
                    database.Delete(mutableDoc);
                }
            }
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (database != null)
            {
                database.Close();
            }
        }

        #endregion

    }
}
