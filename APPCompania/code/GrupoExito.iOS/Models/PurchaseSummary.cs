namespace GrupoExito.iOS.Models
{
    public class PurchaseSummary
    {
        public string title { get; set; }
        public string value { get; set; }
        public string action { get; set; }
        public bool IsVerticalStackView { get; set; }
        public bool HasContinuousLine { get; set; }

        public PurchaseSummary(string title, string value, string action, bool isVerticalCell = false, bool continuousLine = false)
        {
            this.title = title;
            this.value = value;
            this.action = action;
            this.IsVerticalStackView = isVerticalCell;
            this.HasContinuousLine = continuousLine;
        }
    }
}
