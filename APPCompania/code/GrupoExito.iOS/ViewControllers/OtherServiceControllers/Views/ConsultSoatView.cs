using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OtherServiceControllers.Views
{
    public partial class ConsultSoatView : UIView
    {
        #region Attributes
        private DocumentTypesModel _documentTypesModel;
        private EventHandler reloadDocumentEvent;
        #endregion

        #region Properties
        public UIPickerView documentType
        {
            get { return documentTypePickerView; }
        }

        public UIButton documentTypeB
        {
            get { return documentTypeButton; }
        }

        public UITextField documentNumber
        {
            get { return documentNumberTextField; }
        }

        public UITextField plateTruck
        {
            get { return plateTruckTextField; }
        }

        public UIButton consult
        {
            get { return consultButton; }
        }

        public UIButton cancel
        {
            get { return cancelButton; }
        }

        private IList<DocumentType> documentsType;
        public IList<DocumentType> DocumentsType { get => documentsType; set => documentsType = value; }
        public EventHandler ReloadDocumentEvent { get => reloadDocumentEvent; set => reloadDocumentEvent = value; }
        #endregion

        #region Constructors 
        static ConsultSoatView()
        {
            //Static Constructor this class
        }

        protected ConsultSoatView(IntPtr handle) : base(handle)
        {
            _documentTypesModel = new DocumentTypesModel(new DocumentTypesService(DeviceManager.Instance));
        }
        #endregion

        #region Override methods
        public override void AwakeFromNib()
        {
            DocumentsType = new List<DocumentType>();
            consultButton.Enabled = false;
            this.LoadCorners();
            this.LoadHandlers();
            this.LoadColors();
            this.LoadDocumentsType();
            this.ValidateTexField();
        }
        #endregion

        #region Methods 

        private void ValidateCharacters()
        {
            documentNumberTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 13;
            };

            plateTruck.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 6;
            };
        }
        
        private void LoadFonts()
        {
            try
            {
                //Load font size and style
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.ConsultSoatView, ConstantMethodName.LoadFonts);
            }
        }

        private void LoadCorners()
        {
            documentTypePickerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            documentTypePickerView.Layer.BorderColor = UIColor.LightGray.ColorWithAlpha(0.5f).CGColor;
            documentTypePickerView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            consultButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            cancelButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            cancelButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
            cancelButton.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
        }


        private void ValidateTexField()
        { 
            plateTruckTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 6;
            };

            documentNumberTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 13;
            };
        }

        public async Task LoadDocumentsType()
        {
            try
            {
                documentTypeButton.Enabled = false;
                var response = await _documentTypesModel.GetDocumentTypes();
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                    //PresentViewController(alertController, true, null);
                }
                else
                {
                    consultButton.Enabled = true;
                    DocumentsType = response.DocumentTypes;
                    PullSpinners();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.ConsultSoatView, ConstantMethodName.GetDocumentTypes);
            }
        }

        private void PullSpinners()
        {
            documentTypePickerView.DataSource = new PickerViewSource(DocumentsType);
            documentTypePickerView.Delegate = new PickerViewDelegate(DocumentsType, documentTypePickerView);
            documentTypePickerView.ReloadAllComponents();
            documentTypePickerView.ReloadComponent(0);
            documentTypeButton.Enabled = true;
        }

        private void LoadHandlers()
        {
            try
            {
                documentNumberTextField.ShouldReturn += (textField) =>
                {
                    if (plateTruckTextField.Text.Trim().Equals(string.Empty))
                    {
                        plateTruckTextField.BecomeFirstResponder();
                    }
                    else
                    {
                        documentNumberTextField.ResignFirstResponder();
                    }
                    return true;
                };

                plateTruckTextField.ShouldReturn += (textField) =>
                {
                    plateTruckTextField.ResignFirstResponder();
                    return true;
                };
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.ConsultSoatView, ConstantMethodName.LoadHandler);
            }
        }

        private void LoadColors()
        {
            consultButton.BackgroundColor = ConstantColor.UiPrimary;
        }
        #endregion
    }
}

