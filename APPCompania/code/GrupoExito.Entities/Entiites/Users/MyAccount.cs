namespace GrupoExito.Entities.Entiites
{
    using System.Collections.Generic;

    public class MyAccount
    {
        public MyAccount()
        {
            Menu = new List<MenuItem>();
        }

        public IList<MenuItem> Menu { get; set; }

        public User User { get; set; }
    }
}
