namespace GrupoExito.Entities.Entiites.Generic.Contents
{
    using System.Collections.Generic;

    public class Tutorial : Content
    {
        public Tutorial()
        {
            this.ImagesTutorial = new List<ContentImage>();
        }

        public IList<ContentImage> ImagesTutorial { get; set; }
    }
}
