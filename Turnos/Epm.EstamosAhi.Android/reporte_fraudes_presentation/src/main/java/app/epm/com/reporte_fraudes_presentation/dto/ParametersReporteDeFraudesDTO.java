package app.epm.com.reporte_fraudes_presentation.dto;

/**
 * Created by mateoquicenososa on 11/04/17.
 */

public class ParametersReporteDeFraudesDTO {

    private int TipoServicio;
    private String NumeroRadicado;
    private String CorreoElectronico;
    private String Nombre;

    public int getTipoServicio() {
        return TipoServicio;
    }

    public void setTipoServicio(int tipoServicio) {
        TipoServicio = tipoServicio;
    }

    public String getNumeroRadicado() {
        return NumeroRadicado;
    }

    public void setNumeroRadicado(String numeroRadicado) {
        NumeroRadicado = numeroRadicado;
    }

    public String getCorreoElectronico() {
        return CorreoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        CorreoElectronico = correoElectronico;
    }

    public String getNombre() {
        return Nombre;
    }

    public void setNombre(String nombre) {
        Nombre = nombre;
    }
}
