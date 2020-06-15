package app.epm.com.security_domain.business_models;

import com.epm.app.business_models.business_models.Mensaje;


/**
 * Created by ocadavid on 13/12/2016.
 */

public class ChangePasswordResponse {

    private String token;

    private Mensaje mensaje;

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }
}
