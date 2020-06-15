package app.epm.com.utilities.helpers;

import android.os.AsyncTask;
import android.util.Log;

import org.jsoup.Jsoup;

import java.io.IOException;

import app.epm.com.utilities.utils.Constants;


public class VersionAppPlayStore extends AsyncTask<String, String, String> {

    private String version;

    @Override
    protected String doInBackground(String... params) {
        try {
            version = Jsoup.connect(Constants.URL_APP_PLAY_STORE)
                    .timeout(60000)
                    .get()
                    .select(Constants.DIV_HTML_APP_PLAY_STORE)
                    .first()
                    .ownText();
        } catch (IOException e) {
            Log.e("Exception", e.toString());
        }
        return version;
    }
}
