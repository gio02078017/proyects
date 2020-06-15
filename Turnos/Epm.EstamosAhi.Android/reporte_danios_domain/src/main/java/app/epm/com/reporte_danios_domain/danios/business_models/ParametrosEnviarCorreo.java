package app.epm.com.reporte_danios_domain.danios.business_models;

/**
 * Created by Jquinterov on 3/7/2017.
 */

public class ParametrosEnviarCorreo {
    private String correoElectronico;
    private String numeroRadicado;
    private String nombreServicio;

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public String getNumeroRadicado() {
        return numeroRadicado;
    }

    public void setNumeroRadicado(String numeroRadicado) {
        this.numeroRadicado = numeroRadicado;
    }

    public String getNombreServicio() {
        return nombreServicio;
    }

    public void setNombreServicio(String nombreServicio) {
        this.nombreServicio = nombreServicio;
    }
}
