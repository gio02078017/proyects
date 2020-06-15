using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class GenericPickerViewController : UIViewController
    {       
        string _title;
        UIPickerViewModel _model;

        private Action<nint> acceptAction;
        public Action<nint> AcceptAction { get => acceptAction; set => acceptAction = value; }

        private Action cancelAction;
        public Action CancelAction { get => cancelAction; set => cancelAction = value; }

        public GenericPickerViewController() : base("GenericPickerViewController", null)
        {
        }

        public GenericPickerViewController(string title, UIPickerViewModel model)
        {
            _title = title;
            _model = model;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            titleLabel.Text = _title;
            pickerView.Model = _model;
            LoadHandlers();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        private void LoadHandlers()
        {
            acceptButton.TouchUpInside += (o, e) =>
            {
                AcceptAction?.Invoke(pickerView.SelectedRowInComponent(0));
            };

            cancelButton.TouchUpInside += (o, e) =>
            {
                CancelAction?.Invoke();
            };
        }
    }
}

