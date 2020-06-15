package app.epm.com.utilities.utils;

import android.content.Context;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;

public class ConvertUtilities {

    private static Context context;

    public ConvertUtilities(Context context) {
        this.context = context;
    }

    public static int resourceId(String type, String imageName)
    {

        if(context.getPackageName() == null ) return -1;

        int resID = context.getResources().getIdentifier(imageName,
                type, context.getPackageName());

        return resID;
    }

    public static String convertStreamToString(InputStream is)  throws IOException
    {
        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        int i = is.read();
        while (i != -1)
        {
            baos.write(i);
            i = is.read();
        }
        return baos.toString();
    }

    public static long convertFromMinutesToMilliseconds(int minutes){
        return (long)(minutes * 60000);
    }




}
