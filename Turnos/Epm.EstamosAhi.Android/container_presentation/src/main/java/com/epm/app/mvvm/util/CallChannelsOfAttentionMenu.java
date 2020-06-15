package com.epm.app.mvvm.util;

import android.content.Context;
import android.content.res.Resources;

import com.epm.app.R;

import java.io.IOException;
import java.io.InputStream;

import app.epm.com.utilities.utils.ConvertUtilities;

public class CallChannelsOfAttentionMenu extends CallFileResourceRaw {

    private Context context;

    public CallChannelsOfAttentionMenu(Context context) {
        super(context);
        this.context = context;
    }

    @Override
    public String readFile() {
        try {
            Resources r = context.getResources();
            InputStream is = r.openRawResource(R.raw.json_channels_of_attention_menu);
            String myText = ConvertUtilities.convertStreamToString(is);
            is.close();
            return myText;
        }catch(IOException e){
            return null;
        }
    }
}
