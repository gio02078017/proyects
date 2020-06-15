package com.epm.app.business_models.dto;

/**
 * Created by leidycarolinazuluagabastidas on 1/03/17.
 */

public class BaseMapsDTO {


    private LayerBaseDTO LayerBase;

    public LayerBaseDTO getLayer() {
        return LayerBase;
    }

    public void setLayer(LayerBaseDTO layer) {
        this.LayerBase = layer;
    }
}
