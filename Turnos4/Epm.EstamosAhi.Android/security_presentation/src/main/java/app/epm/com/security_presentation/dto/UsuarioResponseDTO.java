package app.epm.com.security_presentation.dto;

import com.epm.app.business_models.dto.MensajeDTO;

/**
 * Created by josetabaresramirez on 18/11/16.
 */

public class UsuarioResponseDTO {

    private MensajeDTO Mensaje;
    private UsuarioDTO Usuario;

    public MensajeDTO getMensaje() {
        return Mensaje;
    }

    public void setMensaje(MensajeDTO mensaje) {
        Mensaje = mensaje;
    }

    public UsuarioDTO getUsuario() {
        return Usuario;
    }

    public void setUsuario(UsuarioDTO usuario) {
        Usuario = usuario;
    }
}
