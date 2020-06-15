package app.epm.com.container_domain.business_models;

import java.util.Date;

/**
 * Created by mateoquicenososa on 3/5/18.
 */

public class InformacionEspacioPromocional {

    private String imagen;
    private Date fechaCreacion;
    private int modulo;

    public String getImagen() {
        return imagen;
    }

    public void setImagen(String imagen) {
        this.imagen = imagen;
    }

    public Date getFechaCreacion() {
        return fechaCreacion;
    }

    public void setFechaCreacion(Date fechaCreacion) {
        this.fechaCreacion = fechaCreacion;
    }

    public int getModulo() {
        return modulo;
    }

    public void setModulo(int modulo) {
        this.modulo = modulo;
    }
}
