package app.epm.com.contacto_transparente_domain.business_models;

/**
 * Created by leidycarolinazuluagabastidas on 16/03/17.
 */

public class Evidencia {

    private String nombreDelArchivo;
    private String archivo;

    public String getNombreDelArchivo() {
        return nombreDelArchivo;
    }

    public void setNombreDelArchivo(String nombreDelArchivo) {
        this.nombreDelArchivo = nombreDelArchivo;
    }

    public String getArchivo() {
        return archivo;
    }

    public void setArchivo(String archivo) {
        this.archivo = archivo;
    }
}
