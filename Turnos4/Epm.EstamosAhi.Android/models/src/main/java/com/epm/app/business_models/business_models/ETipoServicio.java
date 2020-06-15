package com.epm.app.business_models.business_models;


/**
 * Created by leidycarolinazuluagabastidas on 2/03/17.
 */

public enum ETipoServicio {

    Agua("Aguas", 1), Energia("Energía", 2), Gas("Gas", 3), Iluminaria("Iluminaria", 0);

    private static final String EMPTY_STRING = "";

    String name;
    int value;

    public String getName() {
        return name;
    }

    public int getValue() {
        return value;
    }


    ETipoServicio(String name, int value) {
        this.name = name;
        this.value = value;

    }

    public static String getNameService(int servicio) {
        for (ETipoServicio tipoServicio : ETipoServicio.values()) {
            if (servicio == tipoServicio.getValue()) {
                return tipoServicio.getName();
            }
        }
        return EMPTY_STRING;
    }
}
