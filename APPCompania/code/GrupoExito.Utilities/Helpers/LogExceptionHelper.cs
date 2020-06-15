using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GrupoExito.Utilities.Helpers
{
    public class LogExceptionHelper
    {
        public LogExceptionHelper()
        {
        }

        public static void LogException(Exception exception)
        {
            //Dictionary<string, string> properties = new Dictionary<string, string>() { { className, methodName } };
            //var st = new StackTrace(exception, true);
            //var frame = st.GetFrame(0);
            //var line = frame.GetFileLineNumber();
            //properties.Add(ConstantControllersName.LineError, line.ToString());

            //Crashes.TrackError(exception, properties);
        }
    }
}
