package com.epm.app.mvvm.util;

import android.content.Context;
import android.content.res.Resources;

import com.epm.app.R;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;

public abstract class CallFileResourceRaw {

    private Context context;

    public CallFileResourceRaw(Context context) {
        this.context = context;
    }

    public abstract String readFile();

}
