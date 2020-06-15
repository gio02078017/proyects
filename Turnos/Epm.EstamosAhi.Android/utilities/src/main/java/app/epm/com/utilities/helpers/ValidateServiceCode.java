package app.epm.com.utilities.helpers;

import app.epm.com.utilities.R;
import app.epm.com.utilities.utils.Constants;

public class ValidateServiceCode {

    private static int titleError;
    private static int error;
    private static int titleError404 = R.string.title_error;
    private static int error404 = R.string.text_error_500;
    private static int errorCode;

    public ValidateServiceCode() {
        errorCode = Constants.DEFAUL_ERROR_CODE;
    }

    public static boolean captureServiceErrorCode(int code){
        errorCode = code;
        if(code == 200){
            return true;
        }if(code == Constants.UNAUTHORIZED_ERROR_CODE){
             setTitleError(R.string.title_error);
             setError(R.string.text_unauthorized);
             return false;
        }if(code == Constants.NOT_FOUND_ERROR_CODE){
            setTitleError(getTitleError404());
            setError(getError404());
            return false;
        }if (code == 400){
            setTitleError(R.string.title_error);
            setError(R.string.text_error_500);
            return false;
        }else {
            setTitleError(R.string.title_error);
            setError(R.string.text_error_500);
        }
        return false;
    }



    public static int getTitleError() {
        return titleError;
    }



    public static int getError() {
        return error;
    }


    public static int getTitleError404() {
        return titleError404;
    }

    public static void setTitleError404(int titleError404) {
        ValidateServiceCode.titleError404 = titleError404;
    }

    public static int getError404() {
        return error404;
    }

    public static void setTitleError(int titleError) {
        ValidateServiceCode.titleError = titleError;
    }

    public static void setError(int error) {
        ValidateServiceCode.error = error;
    }

    public static void setError404(int error404) {
        ValidateServiceCode.error404 = error404;
    }

    public static int getErrorCode() {
        return errorCode;
    }
}
