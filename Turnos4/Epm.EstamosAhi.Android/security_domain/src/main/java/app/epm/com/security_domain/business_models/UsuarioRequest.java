package app.epm.com.security_domain.business_models;

import java.io.Serializable;

/**
 * Created by josetabaresramirez on 16/11/16.
 */

public class UsuarioRequest implements Serializable {

    private String correoElectronico;
    private String contrasenia;
    private String contraseniaNueva;

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public String getContrasenia() {
        return contrasenia;
    }

    public void setContrasenia(String contrasenia) {
        this.contrasenia = contrasenia;
    }

    public String getContraseniaNueva() { return  contraseniaNueva; }

    public  void setContraseniaNueva(String contraseniaNueva) {
        this.contraseniaNueva = contraseniaNueva;
    }
}
