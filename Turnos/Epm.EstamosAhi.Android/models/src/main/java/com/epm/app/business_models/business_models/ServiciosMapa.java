package com.epm.app.business_models.business_models;

import java.io.Serializable;
import java.util.List;


/**
 * Created by leidycarolinazuluagabastidas on 27/02/17.
 */

public class ServiciosMapa implements Serializable{


    private List<Mapa> mapa;

    public List<Mapa> getMapa() {
        return mapa;
    }

    public void setMapa(List<Mapa> mapa) {
        this.mapa = mapa;
    }
}


