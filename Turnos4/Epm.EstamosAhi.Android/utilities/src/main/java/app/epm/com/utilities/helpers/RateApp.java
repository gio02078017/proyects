package app.epm.com.utilities.helpers;

import android.content.Context;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 12/28/17.
 */

public class RateApp {

    private static String currentDate;
    private static String date;
    private static CustomSharedPreferences sharedPreferences;

    public static boolean validateShowAlertQualifyApp(CustomSharedPreferences customSharedPreferences) {
        sharedPreferences = customSharedPreferences;
        currentDate();
        if (sharedPreferences.getString(Constants.DATE_QUALIFY_APP) == null) {
            sharedPreferences.addString(Constants.DATE_QUALIFY_APP, currentDate);
            return true;
        } else if (sharedPreferences.getInt(Constants.OPEN_QUALIFY_APP) != Constants.ZERO) {
            addTimeToDate(sharedPreferences.getInt(Constants.OPEN_QUALIFY_APP));
            sharedPreferences.addInt(Constants.OPEN_QUALIFY_APP, Constants.ZERO);
            return false;
        } else if (Long.valueOf(currentDate) > Long.valueOf(sharedPreferences.getString(Constants.DATE_QUALIFY_APP))) {
            sharedPreferences.addInt(Constants.OPEN_QUALIFY_APP, Constants.ONE);
            return true;
        } else {
            return false;
        }
    }

    private static void addTimeToDate(int number) {
        SimpleDateFormat dateFormat = new SimpleDateFormat(Constants.FORMAT_DATE_YMDHMS);
        Calendar calendar = Calendar.getInstance();
        calendar.add(Calendar.MONTH, number);
        date = dateFormat.format(calendar.getTime());
        date = date.replace("-", Constants.EMPTY_STRING);
        sharedPreferences.addString(Constants.DATE_QUALIFY_APP, date);
    }

    private static void currentDate() {
        currentDate = new SimpleDateFormat(Constants.FORMAT_DATE_YMDHMS).format(new Date());
        currentDate = currentDate.replace("-", Constants.EMPTY_STRING);
    }
}