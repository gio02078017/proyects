using Android.Text;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities.Constants;
using GrupoExito.Utilities.Resources;
using Java.Lang;
using System.Text.RegularExpressions;

namespace GrupoExito.Android.Utilities.Listeners
{
    public class CustomInputFilter : Java.Lang.Object, IInputFilter
    {
        string TypeValidation;
        string NameRegularExpresion;
        int MaxLength { get; set; }

        public CustomInputFilter(string TypeValidation, string NameRegularExpresion, int MaxLength)
        {
            this.TypeValidation = TypeValidation;
            this.NameRegularExpresion = NameRegularExpresion;
            this.MaxLength = MaxLength;
        }

        public int AppConfiguration { get; private set; }

        public ICharSequence FilterFormatted(ICharSequence source, int start, int end, ISpanned dest, int dstart, int dend)
        {
            string data = source.ToString();

           if (TypeValidation.Equals(ConstantEventName.SpaceFilter))
            {
                if (data.Contains((char)32))
                {
                    data = string.Empty;
                }
            }
            else if (TypeValidation.Equals(ConstantEventName.RegexFilter))
            {
                Regex regexString = null;

                if (NameRegularExpresion.Equals(ConstRegularExpression.NumberMoreLetter))
                {
                    regexString = new Regex(@AppConfigurations.NumberMoreLetterRegularExpression);
                }

                if (regexString != null)
                {
                    if (!regexString.IsMatch(data))
                    {
                        data = string.Empty;
                    }
                }
            }

            if (MaxLength > -1)
            {
                if (dend >= MaxLength)
                {
                    data = string.Empty;
                }
            }

            Java.Lang.ICharSequence charSequence = new Java.Lang.StringBuffer(data);
            return charSequence;
        }
    }
}