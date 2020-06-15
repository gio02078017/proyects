using GrupoExito.Utilities.Resources;
using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace GrupoExito.Utilities.Helpers
{
    public static class StringFormat
    {
        public static string ToPrice(decimal number)
        {
            CultureInfo culture = new CultureInfo("es-CO");
            return number.ToString("C0", culture);
        }

        public static string ToPercerntaje(decimal discountPercent)
        {
            return string.Concat(Convert.ToInt32(discountPercent), "%");
        }

        public static string ToQuantity(int total)
        {
            return total.ToString();
        }

        public static DateTime ParseFormatDateTime(string date)
        {
            string[] formats = { AppConfigurations.DateFormat };
            var dateOfBirth = DateTime.ParseExact(date, formats, new CultureInfo("es-ES"), DateTimeStyles.None);
            return dateOfBirth;

        }

        public static string Capitalize(string name)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(name);
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string DoubleToString(double value)
        {
            return value.ToString("0.00000000", CultureInfo.GetCultureInfo("en-US"));
        }

        public static string DateWithPrefix(DateTime? date, string prefix = "")
        {
            string formatDate = prefix + " " + Convert.ToDateTime(date).ToString("dd MMM yyyy");
            return formatDate.Trim();
        }

        public static string NumberFormatForThousand(double number)
        {
            return string.Format("{0:N0}", number).Replace(",", ".");
        }

        public static string CutString(string name, int tamano)
        {
            if (name.Length > tamano)
            {
                return name.Substring(0, tamano);
            }

            return name;
        }

        public static string SplitName(string name)
        {
            string[] names = name.Split(' ');

            if (names != null && names.Length > 0)
            {
                return StringFormat.CutString(names[0], 15);
            }
            else
            {
                return StringFormat.CutString(name, 15);
            }

        }

        public static string AddTimeToDateNow(int MinutesPromiseDelivery)
        {
            DateTime localDate = DateTime.Now.AddMinutes(MinutesPromiseDelivery);
            return localDate.ToLongDateString() + " " + localDate.ToShortTimeString();
        }

        public static string WordToUpper(string value)
        {
            string result = string.Empty;
            string[] array = value.Split(' ');

            foreach (var item in array)
            {
                result = result + " " + FirstCharToUpper(item);
            }

            return result;
        }

        private static string FirstCharToUpper(string value)
        {
            char[] array = value.ToCharArray();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i == 0 ? char.ToUpper(array[i]) : char.ToLower(array[i]);
            }

            return new string(array);
        }
    }
}
