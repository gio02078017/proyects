package app.epm.com.security_domain.security;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.security_domain.business_models.ChangePasswordResponse;
import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.business_models.UsuarioRequest;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public interface ISecurityRepository {

    Usuario login(UsuarioRequest usuarioRequest, String idOneSignal) throws RepositoryError;

    Mensaje register(Usuario usuario) throws RepositoryError;

    Mensaje resetPassword(EmailUsuarioRequest resetPasswordRequest) throws RepositoryError;

    ChangePasswordResponse changePassword(UsuarioRequest usuarioRequest)  throws RepositoryError;

    Mensaje logOut(EmailUsuarioRequest emailUsuarioRequest, String idOneSignal) throws RepositoryError;
}
