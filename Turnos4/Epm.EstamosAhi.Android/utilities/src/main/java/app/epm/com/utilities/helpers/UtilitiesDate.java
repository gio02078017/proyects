package app.epm.com.utilities.helpers;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.Locale;

import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 12/28/17.
 */

public class UtilitiesDate {

    public static String getDateString(){
        String currentDate = new SimpleDateFormat(Constants.FORMATDMY, Locale.getDefault()).format(new Date());
        return currentDate;
    }

    public static Date getDate(){
        return convertToStringFromDate(getDateString());
    }

    public static Date convertToStringFromDate(String txtDate){
        DateFormat format = new SimpleDateFormat(Constants.FORMATDMY, Locale.getDefault());
        Date date = null;
        try {
            date = format.parse(txtDate);
        } catch (ParseException e) {
            date = null;
        }
        return date;
    }

    public static int subtractDates(Date oldDate, Date currentDate){
        int dias = 0;
        if(oldDate != null && currentDate != null){
            dias=(int) ((currentDate.getTime()-oldDate.getTime())/86400000);
        }
        return dias;
    }

    public static Date sumarDiasAFecha(Date fecha, int dias){
        if (fecha == null || dias==0) return fecha;
        Calendar calendar = Calendar.getInstance();
        calendar.setTime(fecha);
        calendar.add(Calendar.DAY_OF_YEAR, dias);
        return calendar.getTime();
    }
}