package app.epm.com.utilities.helpers;

import android.util.Patterns;

import java.util.Calendar;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import app.epm.com.utilities.utils.Constants;

/**
 * Created by JoseTabares on 13/07/16.
 */
public class Validations {

    /**
     * Valida un parametro, si es nulo retorna una excepción.
     *
     * @param data data
     * @param <T>  Tipo de dato
     */
    public static <T> void validateNullParameter(T data) {
        if (data == null) {
            throw new IllegalArgumentException(Constants.NULL_PARAMETERS);
        }
    }

    /**
     * Valida un parametro boolean
     *
     * @param data data
     * @param <T> Tipo de dato
     * @return data
     */
    public <T> boolean validateNullParameterBoolean(T data) {
        return data != null;
    }


    /**
     * Validamos todos los datos y si es nulo retorna falso
     * @param data
     * @param <T>
     */

    public static <T> boolean validateNullParameterBoolean(T... data) {
        for (T oneData : data) {
            if (oneData == null) {
                return false;
            }

        }
        return true;
    }

    /**
     * Valida varios parametros, si alguno es nulo retorna una excepción.
     *
     * @param data data
     * @param <T>  Tipo de dato
     */
    @SafeVarargs
    public static <T> void validateNullParameter(T... data) {
        for (T oneData : data) {
            if (oneData == null) {
                throw new IllegalArgumentException(Constants.NULL_PARAMETERS);
            }
        }
    }

    /**
     * Valida un parametro, si está vacío retorna una excepción.
     *
     * @param string texto
     */
    public static void validateEmptyParameter(String string) {
        if (string.isEmpty()) {
            throw new IllegalArgumentException(Constants.EMPTY_PARAMETERS);
        }
    }

    /**
     * Valida varios parametros, si alguno está vacío retorna una excepción.
     *
     * @param strings texto
     */
    public static void validateEmptyParameter(String... strings) {
        for (String string : strings) {
            if (string.isEmpty()) {
                throw new IllegalArgumentException(Constants.EMPTY_PARAMETERS);
            }
        }
    }

    public  static boolean validateEmptyParameterBoolean(String... strings) {
        for (String string : strings) {
            if (string.isEmpty()) {
                return false;
            }
        }
        return true;
    }

    public  static boolean validateEmptyParameterBoolean(String string) {
            if (string.isEmpty()) {
                return false;
            }
        return true;
    }

    public static boolean validatePhone(String data, int max, int min) {
        Pattern pattern = Pattern.compile("\\s");
        Matcher matcher = pattern.matcher(data);
        boolean found = matcher.find();
        boolean numeric = isNumeric(data);
        if (data.length() > max || found || !numeric || data.length() < min) {
            return true;
        }
        return false;
    }

    public static boolean isNumeric(String string) throws IllegalArgumentException {
        boolean isnumeric = false;

        if (string != null && !string.equals("")) {
            isnumeric = true;
            char chars[] = string.toCharArray();

            for (int d = 0; d < chars.length; d++) {
                isnumeric &= Character.isDigit(chars[d]);

                if (!isnumeric)
                    break;
            }
        }
        return isnumeric;
    }

    /**
     * Valida si dos fechas son iguales.
     *
     * @param calendarToday     calendarToday.
     * @param calendarToCompare calendarToCompare.
     * @return True/False.
     */
    public static boolean areEqualsDates(Calendar calendarToday, Calendar calendarToCompare) {
        if (calendarToday.get(Calendar.YEAR) != calendarToCompare.get(Calendar.YEAR)) {
            return false;
        }

        if (calendarToday.get(Calendar.DAY_OF_MONTH) != calendarToCompare.get(Calendar.DAY_OF_MONTH)) {
            return false;
        }

        if (calendarToday.get(Calendar.MONTH) != calendarToCompare.get(Calendar.MONTH)) {
            return false;
        }

        return true;
    }

    public <T> boolean dataIsNotNull(T data) {
        return data != null;
    }

    public static String getIdFromListByString(String text, List<TypeDanioOrFraude> list) {
        String id = "";
        for (TypeDanioOrFraude item : list) {
            if (item.getNameType().equals(text)) {
                return item.getId();
            }
        }
        return id;
    }

    public static boolean validateEmail(String email) {

        Pattern pattern = Pattern.compile(Constants.REGULAR_EXPRESSION_CORRECT_EMAIL);
        Matcher matcher = pattern.matcher(email);

        return matcher.matches();
    }




}
