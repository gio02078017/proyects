package com.epm.app.menu_presentation.repositories.menu_fragment;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.security_domain.business_models.ChangePasswordResponse;
import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_domain.security.ISecurityRepository;

/**
 * Created by juan on 11/04/17.
 */

public class MenuFragmentRepositoryTest implements ISecurityRepository {
    @Override
    public Usuario login(UsuarioRequest usuarioRequest, String idOneSignal) throws RepositoryError {
        return null;
    }

    @Override
    public Mensaje register(Usuario usuario) throws RepositoryError {
        return null;
    }

    @Override
    public Mensaje resetPassword(EmailUsuarioRequest resetPasswordRequest) throws RepositoryError {
        return null;
    }

    @Override
    public ChangePasswordResponse changePassword(UsuarioRequest usuarioRequest) throws RepositoryError {
        return null;
    }

    @Override
    public Mensaje logOut(EmailUsuarioRequest emailUsuarioRequest, String idOneSignal) throws RepositoryError {
        return null;
    }
}
