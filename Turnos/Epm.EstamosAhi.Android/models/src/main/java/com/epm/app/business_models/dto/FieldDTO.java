package com.epm.app.business_models.dto;


/**
 * Created by leidycarolinazuluagabastidas on 1/03/17.
 */

public class FieldDTO {

    private String Id;

    private String Visible;

    private String Editable;

    private String Key;

    public String getId() {
        return Id;
    }

    public void setId(String id) {
        Id = id;
    }

    public String getVisible() {
        return Visible;
    }

    public void setVisible(String visible) {
        Visible = visible;
    }

    public String getEditable() {
        return Editable;
    }

    public void setEditable(String editable) {
        Editable = editable;
    }

    public String getKey() {
        return Key;
    }

    public void setKey(String key) {
        Key = key;
    }
}
