package app.epm.com.contacto_transparente_domain.business_models;

import java.io.Serializable;

/**
 * Created by leidycarolinazuluagabastidas on 9/03/17.
 */

public class GrupoInteres implements Serializable {

    private String id;
    private String descripcion;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }
}
