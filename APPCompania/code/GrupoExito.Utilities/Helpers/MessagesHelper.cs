namespace GrupoExito.Utilities.Helpers
{
    using GrupoExito.Entities.Containers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Linq;

    public class MessagesHelper
    {
        public static string GetMessage(MessagesContainer result)
        {
            if (result == null)
                return AppMessages.GeneralSuccessMessage;

            var message = result.Messages.FirstOrDefault();

            if (message != null)
            {
                if (result.Messages.Count == 1)
                    return message.Description;
                else
                {
                    string messagesList = string.Empty;

                    foreach (var item in result.Messages)
                    {
                        messagesList += "- " + item.Description + "\r\n";
                    }

                    return messagesList;
                }
            }
            else
                return AppMessages.UnexpectedErrorMessage;
        }

        public static string GetMessage(object result)
        {
            throw new NotImplementedException();
        }
    }
}
