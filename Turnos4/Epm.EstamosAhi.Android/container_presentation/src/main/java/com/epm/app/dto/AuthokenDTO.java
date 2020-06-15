package com.epm.app.dto;

import com.epm.app.business_models.dto.MensajeDTO;

import app.epm.com.security_presentation.dto.UsuarioDTO;

/**
 * Created by mateoquicenososa on 7/12/16.
 */

public class AuthokenDTO {
    private MensajeDTO Mensaje;
    private UsuarioDTO Usuario;
    private boolean Invitado;

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

    public boolean isInvitado() {
        return Invitado;
    }

    public void setInvitado(boolean invitado) {
        Invitado = invitado;
    }
}
