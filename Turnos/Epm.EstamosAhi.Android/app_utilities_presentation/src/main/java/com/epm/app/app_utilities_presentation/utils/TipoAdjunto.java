package com.epm.app.app_utilities_presentation.utils;

/**
 * Created by ocadavid on 3/03/2017.
 */

public enum TipoAdjunto {
    ninguno(0, "Ninguno"),
    gallery(1, "Gallery"),
    camera(2, "Camera"),
    audio(3, "Audio");

    private final int tipo;
    private final String tipoAdjunto;

    TipoAdjunto(int tipo, String tipoAdjunto) {
        this.tipo = tipo;
        this.tipoAdjunto = tipoAdjunto;
    }
}
