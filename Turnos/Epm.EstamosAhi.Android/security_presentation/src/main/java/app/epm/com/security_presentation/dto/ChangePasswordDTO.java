package app.epm.com.security_presentation.dto;

/**
 * Created by ocadavid on 1/12/2016.
 */
public class ChangePasswordDTO {

    private String CorreoElectronico;

    private String ContraseniaActual;

    private String ContraseniaNueva;

    public String getCorreoElectronico() {
        return CorreoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        CorreoElectronico = correoElectronico;
    }

    public String getContraseniaActual() {
        return ContraseniaActual;
    }

    public void setContraseniaActual(String contraseniaActual) {
        ContraseniaActual = contraseniaActual;
    }

    public String getContraseniaNueva() {
        return ContraseniaNueva;
    }

    public void setContraseniaNueva(String contraseniaNueva) {
        ContraseniaNueva = contraseniaNueva;
    }
}
