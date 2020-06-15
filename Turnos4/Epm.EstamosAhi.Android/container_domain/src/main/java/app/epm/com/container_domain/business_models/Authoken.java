package app.epm.com.container_domain.business_models;


import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.Usuario;

/**
 * Created by mateoquicenososa on 12/12/16.
 */

public class Authoken {
    private Mensaje mensaje;
    private Usuario usuario;
    private boolean invitado;

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

    public Usuario getUsuario() {
        return usuario;
    }

    public void setUsuario(Usuario usuario) {
        this.usuario = usuario;
    }

    public boolean isInvitado() {
        return invitado;
    }

    public void setInvitado(boolean invitado) {
        this.invitado = invitado;
    }
}
