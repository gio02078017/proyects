package app.epm.com.utilities.controls;

import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;

/**
 * Created by josetabaresramirez on 20/09/16.
 */
public class Formats {

    /**
     * Permite formatear un dia o mes añadiendo un cero al inicio en el caso de ser un número del 1 al 9
     *
     * @param dayOrMonth dayOrMonth.
     * @return Día o Mes formateado(String).
     */
    public static String formatDayOrMonthInTwoLength(int dayOrMonth) {
        String dayString = String.valueOf(dayOrMonth);
        if (dayString.length() == 1) {
            dayString = String.valueOf(0).concat(dayString);
        }
        return dayString;
    }

    /**
     * Permite formatear los números en moneda.
     *
     * @param value valor.
     * @return Valor en formato moneda.
     */
    public static String getCurrency(long value) {
        DecimalFormatSymbols decimalFormatSymbols = new DecimalFormatSymbols();
        decimalFormatSymbols.setDecimalSeparator('.');
        decimalFormatSymbols.setGroupingSeparator('.');
        DecimalFormat decimalFormat = new DecimalFormat("###,###,###,###,###,###,###,###", decimalFormatSymbols);
        return decimalFormat.format(value);
    }
}
