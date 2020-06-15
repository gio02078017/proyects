package app.epm.com.security_presentation.dto;


import com.epm.app.business_models.dto.MensajeDTO;

/**
 * Created by ocadavid on 13/12/2016.
 */

public class ChangePasswordResponseDTO {
    private String Token;
    private MensajeDTO Mensaje;

    public String getToken() {
        return Token;
    }

    public void setToken(String token) {
        Token = token;
    }

    public MensajeDTO getMensaje() {
        return Mensaje;
    }

    public void setMensaje(MensajeDTO mensaje) {
        Mensaje = mensaje;
    }
}
