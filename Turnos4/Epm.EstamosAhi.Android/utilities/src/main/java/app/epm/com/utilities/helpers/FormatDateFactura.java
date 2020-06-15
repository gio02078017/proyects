package app.epm.com.utilities.helpers;

import android.util.Log;

import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

import app.epm.com.utilities.utils.Constants;

/**
 * Created by jhonjairohernandezmona on 3/07/15.
 */

public class FormatDateFactura {


    public String formatDate(String valor, String formatIni, String formatFinal) {

        final String t;
        t = valor.replace("T", " ");
        return convertStrinToDate(t, formatIni, formatFinal);
    }

    public String convertStrinToDate(String date, String formatIni, String formatFinal) {
        Date fecha = null;

        SimpleDateFormat dateFormat = new SimpleDateFormat(formatIni);// FORMATYMD "yyyy-MM-dd"
        try {
            fecha = dateFormat.parse(date);
        } catch (ParseException e) {
            Log.e("Exception", e.toString());
        }
        Locale locale = new Locale("es");
        SimpleDateFormat postFormater = new SimpleDateFormat(formatFinal, locale); // FORMATMD = "MMMM dd";
        String newDateStr = postFormater.format(fecha);

        return primeroLetraMayuscula(newDateStr);
    }

    public String primeroLetraMayuscula(String texto) {
        char primeraLetra = texto.charAt(0);
        return Character.toUpperCase(primeraLetra) + texto.substring(1, texto.length()).toLowerCase();
    }

    public String moneda(int valor) {
        DecimalFormatSymbols symbols = new DecimalFormatSymbols();
        symbols.setDecimalSeparator(',');
        symbols.setGroupingSeparator('.');
        DecimalFormat decimalFormat = new DecimalFormat("###,###.##", symbols);
        String number = "$ " + decimalFormat.format(valor);
        return number;
    }

    public String monedaSomos(int valor) {
        DecimalFormatSymbols symbols = new DecimalFormatSymbols();
        symbols.setDecimalSeparator(',');
        symbols.setGroupingSeparator('.');
        DecimalFormat decimalFormat = new DecimalFormat("###,###.##", symbols);
        String number = decimalFormat.format(valor);
        return number;
    }

    public String moneda(double valor) {
        DecimalFormatSymbols symbols = new DecimalFormatSymbols();
        symbols.setDecimalSeparator(',');
        symbols.setGroupingSeparator('.');
        DecimalFormat decimalFormat = new DecimalFormat("###,###", symbols);
        String number = "$ " + decimalFormat.format(valor);
        return number;
    }

    public String FormatDateddMMyyyyHHmmss(String stringDate, String baseFormat, String experedFormat) {
        SimpleDateFormat formatter, FORMATTER;
        String resultado = Constants.EMPTY_STRING;

        formatter = new SimpleDateFormat(baseFormat);
        //formatter = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSz");
        String oldDate = stringDate.concat(".000+02:00");

        FORMATTER = new SimpleDateFormat(experedFormat);
        //FORMATTER = new SimpleDateFormat("MM/dd/yyyy hh:mm.ss");
        Date date;
        //formatter = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.S");
        //FORMATTER = new SimpleDateFormat("dd-MM-yyyy HH:mm:ss.S");

        try {
            date = formatter.parse(oldDate.substring(0, 26) + oldDate.substring(27, 27));
            //date = formatter.parse(oldDate.substring(0, 21));
            resultado = FORMATTER.format(date);
        } catch (ParseException e) {
            Log.e("Exception", e.toString());
        }
        resultado = resultado.substring(0, resultado.length());
        return resultado.replace("-", "/");
    }

    public static String monthsOfYears(String month, String language) {

        if (language.contains("English")) {
            switch (month) {
                case Constants.JANUARY:
                    return Constants.ENERO;

                case Constants.FEBRUARY:
                    return Constants.FEBRERO;

                case Constants.MARCH:
                    return Constants.MARZO;

                case Constants.APRIL:
                    return Constants.ABRIL;

                case Constants.MAY:
                    return Constants.MAYO;

                case Constants.JUNE:
                    return Constants.JUNIO;

                case Constants.JULY:
                    return Constants.JULIO;

                case Constants.AUGUST:
                    return Constants.AGOSTO;

                case Constants.SEPTEMBER:
                    return Constants.SEPTIEMBRE;

                case Constants.OCTOBER:
                    return Constants.OCTUBRE;

                case Constants.NOVEMBER:
                    return Constants.NOVIEMBRE;

                case Constants.DECEMBER:
                    return Constants.DICIEMBRE;

                default:
                    break;
            }
            return month;
        } else {
            return month;
        }
    }
}