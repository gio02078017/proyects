package com.epm.app.business_models.business_models;

import java.io.Serializable;

/**
 * Created by mateoquicenososa on 22/11/16.
 */

public class Mensaje implements Serializable{

    private int code;
    private String text;

    public Mensaje() {
        this.text = "";
        this.code = 0;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }
}
