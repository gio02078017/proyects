namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Models
{
    public class AlertModel
    {
        #region Attributes
        private string _title;
        private string _message;
        private string _code;
        private string _actionText;
        #endregion

        #region Properties
        public string Title { get => _title; set => _title = value; }
        public string Message { get => _message; set => _message = value; }
        public string ActionText { get => _actionText; set => _actionText = value; }
        public string Code { get => _code; set => _code = value; }
        #endregion

        #region Constructors
        public AlertModel(string title, string message, string code, string actionText)
        {
            this.Title = title;
            this.Message = message;
            this.Code = code;
            this.ActionText = actionText;
        }
        #endregion
    }
}
