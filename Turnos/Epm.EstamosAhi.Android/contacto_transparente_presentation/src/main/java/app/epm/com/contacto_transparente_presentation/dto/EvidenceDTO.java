package app.epm.com.contacto_transparente_presentation.dto;

/**
 * Created by leidycarolinazuluagabastidas on 16/03/17.
 */

public class EvidenceDTO {

    private String NombreDelArchivoConExtension;
    private String Archivo;

    public String getNombreDelArchivoConExtension() {
        return NombreDelArchivoConExtension;
    }

    public void setNombreDelArchivoConExtension(String nombreDelArchivoConExtension) {
        NombreDelArchivoConExtension = nombreDelArchivoConExtension;
    }

    public String getArchivo() {
        return Archivo;
    }

    public void setArchivo(String archivo) {
        Archivo = archivo;
    }
}
