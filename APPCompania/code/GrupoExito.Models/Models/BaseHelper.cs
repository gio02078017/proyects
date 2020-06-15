using System;
using System.Linq;
using GrupoExito.Entities.Containers;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.Models.Models
{
    public class BaseHelper
    {
        internal Exception CreateNewException(MessagesContainer result)
        {
            Exception ex = new Exception();

            if (result.Messages.Any())
            {
                String message = MessagesHelper.GetMessage(result);
                if (!string.IsNullOrEmpty(message))
                {
                    ex.Data.Add("Message", message);
                }
                else
                {
                    ex.Data.Add("Message", AppMessages.UnexpectedErrorMessage);
                }

                ex.Data.Add("Code", result.Messages[0].Code);
            }

            LogExceptionHelper.LogException(ex);
            return ex;
        }
    }
}
