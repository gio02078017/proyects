package com.epm.app.business_models.dto;

/**
 * Created by leidycarolinazuluagabastidas on 1/03/17.
 */

public class LayerDTO extends Layer{

    private FieldsDTO Fields;

    public FieldsDTO getFields() {
        return Fields;
    }

    public void setFields(FieldsDTO fields) {
        Fields = fields;
    }
}
