package com.epm.app.mvvm.turn.models;

import android.content.res.Resources;

public class DetailOfFormalities {

    private String description;
    private Resources image;

    public DetailOfFormalities(String mName) {
        this.description = mName;
    }

    public String getName() {
        return description;
    }


}
