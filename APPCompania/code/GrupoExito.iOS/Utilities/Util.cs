using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using CoreGraphics;
using GlobalToast;
using GrupoExito.Entities;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities.Constant;
using Microsoft.AppCenter.Crashes;
using UIKit;

namespace GrupoExito.iOS.Utilities
{
    public static class Util
    {
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var item in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(item);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(item);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static City SearchCity(string word, IList<City> cities)
        {
            foreach (City current in cities)
            {
                if (Util.RemoveDiacritics(current.Name.ToLower()).Equals(word.ToLower()))
                {
                    return current;
                }
            }

            return null;
        }

        public static string Capitalize(String word)
        {
            try
            {
                string[] wordSeparate = word.Split(' ');
                string result = string.Empty;

                for (int i = 0; i < wordSeparate.Length; i++)
                {
                    string firtsLetter = wordSeparate[i].Substring(0, 1).ToUpper();
                    string ottherLetter = wordSeparate[i].Substring(1);
                    result += firtsLetter + ottherLetter + " ";
                }

                return result;
            }
            catch (Exception)
            {
                return word;
            }
        }

        public static int ConvertToUnit(decimal weight, decimal presentation)
        {
            string value = Math.Round(weight / presentation, 3).ToString();
            bool status = int.TryParse(value, out int result);

            if (status)
            {
                return result;
            }
            else
            {
                status = int.TryParse(value.ToString().Replace(',', '.'), out result);

                if (status)
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }

        public static decimal ConvertToWeight(int units, decimal presentation)
        {
            decimal value = Math.Round(units * presentation / 1, 3);
            bool status = decimal.TryParse(value.ToString(), out decimal result);

            if (status)
            {
                return result;
            }
            else
            {
                status = decimal.TryParse(value.ToString().Replace(',', '.'), out result);

                if (status)
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }

        public static UIImage[] LoadAnimationImage(string nameFolderAnimation, int count)
        {
            UIImage[] images = new UIImage[count];
            try
            {
                for (int i = 0; i < images.Length; i++)
                {
                    images[i] = UIImage.FromFile(nameFolderAnimation + "/" + (i + 1));
                }
            }
            catch (Exception exception)
            {
                LogException(exception, ConstantControllersName.UIViewControllerBaseProduct, ConstantMethodName.LoadData);
            }

            return images;
        }

        public static UIImage[] LoadAnimationImageWithReverse(string nameFolderAnimation, int count)
        {
            UIImage[] images = new UIImage[count * 2];

            try
            {
                int currentCount = 0;

                for (int i = count; i > 0; i--)
                {
                    images[currentCount] = UIImage.FromFile(nameFolderAnimation + "/" + (i));
                    currentCount++;
                }

                for (int i = 1; i <= images.Length; i++)
                {
                    images[currentCount] = UIImage.FromFile(nameFolderAnimation + "/" + (i));
                    currentCount++;
                }
            }
            catch (Exception exception)
            {
                LogException(exception, ConstantControllersName.UIViewControllerBaseProduct, ConstantMethodName.LoadData);
            }
            return images;
        }

        public static void LogException(Exception exception, String viewControllerName, String methodName)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>() {
                { viewControllerName, methodName } };

            var st = new StackTrace(exception, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            properties.Add(ConstantControllersName.LineError, line.ToString());

            Crashes.TrackError(exception, properties);
        }

        public static void SetConstraint(UIView view, nfloat constant, int value)
        {
            NSLayoutConstraint[] constraints = view.Constraints;

            foreach (NSLayoutConstraint constraint in constraints)
            {
                if (constraint.Constant == constant)
                {
                    constraint.Constant = value;
                }
            }
        }


        public static nfloat GetTableHeightProducts(IList<Product> CustomerProducts)
        {
            nfloat newHeight = 0;
            try
            {
                for (int i = 0; i < CustomerProducts.Count; i += 2)
                {
                    if (((i + 1) < CustomerProducts.Count && CustomerProducts[i + 1].Quantity > 0) || CustomerProducts[i].Quantity > 0)
                    {
                        newHeight += ConstantViewSize.ProductHeightWithProductsAdded;
                    }
                    else if (((i - 1) >= 0 && CustomerProducts[i - 1].Quantity > 0) || CustomerProducts[i].Quantity > 0)
                    {
                        newHeight += ConstantViewSize.ProductHeightWithProductsAdded;
                    }
                    else
                    {
                        newHeight += ConstantViewSize.ProductHeightCell;
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.GetTableHeightProducts);
            }
            return newHeight;
        }


        public static Toast LoadCenterToast(string text, float duration = 2000)
        {
            ToastAppearance appearance = new ToastAppearance
            {
                MessageColor = UIColor.FromRGB(255, 255, 255),
                Color = ConstantColor.UiBackgroundToastMake,
                MessageTextAlignment = UITextAlignment.Center,
                TitleFont = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.SubtitleGeneric),
                CornerRadius = ConstantStyle.CornerRadius
            };

            return Toast.MakeToast(text)
                .SetPosition(ToastPosition.Center)
                .SetLayout(new ToastLayout())
                .SetAppearance(appearance)
                .SetDuration(duration);
        }

        public static  void CreateShadowLayer(UIView ContentView, nfloat radius, float opacity)
        {
            UIView view = ContentView;
            view.Layer.BorderWidth = 1.0f;
            view.Layer.BorderColor = UIColor.Clear.CGColor;
            view.Layer.ShadowColor = UIColor.LightGray.CGColor;
            view.Layer.ShadowOffset = new CGSize(0, 0);
            view.Layer.ShadowRadius = radius;
            view.Layer.ShadowOpacity = opacity;
            view.Layer.MasksToBounds = false;
        }
    }

    public static class CustomExtensions
    {
        public static void StartSpinner(this UIViewController viewController, ref bool isSpinnerAdded, CustomSpinnerView customSpinner)
        {
            if (!isSpinnerAdded)
            {
                isSpinnerAdded = true;
                viewController.View.AddSubview(customSpinner);
                customSpinner.Frame = viewController.NavigationController.View.Frame;
                customSpinner.Start();
                viewController.View.BringSubviewToFront(customSpinner);
                viewController.NavigationController?.SetNavigationBarHidden(true, false);
            }
        }

        public static void StopSpinner(this UIViewController viewController, ref bool isSpinnerAdded, CustomSpinnerView customSpinner)
        {
            if (isSpinnerAdded)
            {
                isSpinnerAdded = false;
                customSpinner.Stop();
                customSpinner.RemoveFromSuperview();
                viewController.View.SendSubviewToBack(customSpinner);
                viewController.NavigationController?.SetNavigationBarHidden(false, false);
            }
        }
    }
}
