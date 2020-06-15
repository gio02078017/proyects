package com.epm.app.business_models.dto;

import java.util.List;

/**
 * Created by josetabaresramirez on 24/02/17.
 */

public class CityFromArcgisDTO {

    private List<Feature> features;

    public List<Feature> getFeatures() {
        return features;
    }

    public void setFeatures(List<Feature> features) {
        this.features = features;
    }

    public class Feature {
        private Attributes attributes;

        public Attributes getAttributes() {
            return attributes;
        }

        public void setAttributes(Attributes attributes) {
            this.attributes = attributes;
        }
    }

    public class Attributes {

        private String UBICACION;

        public String getUBICACION() {
            return UBICACION;
        }

        public void setUBICACION(String UBICACION) {
            this.UBICACION = UBICACION;
        }
    }
}
