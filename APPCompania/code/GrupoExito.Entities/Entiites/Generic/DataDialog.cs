using System;
using System.Collections.Generic;
using System.Text;

namespace GrupoExito.Entities.Entiites
{
    public class DataDialog
    {
        public string TitleDialog { get; set; }

        public string MensajeDialog { get; set; }

        public string ButtonYesName { get; set; }

        public string ButtonNotName { get; set; }

        public DataDialog(string TitleDialog, string MensajeDialog)
        {
            this.TitleDialog = TitleDialog;
            this.MensajeDialog = MensajeDialog;
        }

        public DataDialog(string TitleDialog, string MensajeDialog, String ButtonYesName, String ButtonNotName)
        {
            this.TitleDialog = TitleDialog;
            this.MensajeDialog = MensajeDialog;
            this.ButtonYesName = ButtonYesName;
            this.ButtonNotName = ButtonNotName;
        }

    }
}
