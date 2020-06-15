package app.epm.com.security_domain.security;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.security_domain.business_models.ChangePasswordResponse;
import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.utilities.helpers.Validations;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class SecurityBusinessLogic {

    private ISecurityRepository securityRepository;

    public SecurityBusinessLogic(ISecurityRepository securityRepository) {
        this.securityRepository = securityRepository;
    }

    public Usuario login(UsuarioRequest usuarioRequest, String idOneSignal) throws RepositoryError {
        Validations.validateNullParameter(usuarioRequest);
        Validations.validateNullParameter(usuarioRequest.getCorreoElectronico(), usuarioRequest.getContrasenia());
        Validations.validateEmptyParameter(usuarioRequest.getCorreoElectronico(), usuarioRequest.getContrasenia());
        return securityRepository.login(usuarioRequest,idOneSignal);
    }

    public Mensaje register(Usuario usuario) throws RepositoryError {
        Validations.validateNullParameter(usuario);
        Validations.validateNullParameter(usuario.getCorreoElectronico(), usuario.getNombres(),
                usuario.getApellido(), usuario.getIdTipoIdentificacion(),
                usuario.getNumeroIdentificacion(), usuario.getContrasenia());
        Validations.validateEmptyParameter(usuario.getCorreoElectronico(), usuario.getNombres(),
                usuario.getApellido(), usuario.getNumeroIdentificacion(),
                usuario.getContrasenia());
        return securityRepository.register(usuario);
    }

    public Mensaje resetPassword(EmailUsuarioRequest resetPasswordRequest) throws RepositoryError {
        Validations.validateNullParameter(resetPasswordRequest);
        Validations.validateNullParameter(resetPasswordRequest.getCorreoElectronico());
        Validations.validateEmptyParameter(resetPasswordRequest.getCorreoElectronico());
        return securityRepository.resetPassword(resetPasswordRequest);
    }


    public ChangePasswordResponse changePassword(UsuarioRequest usuarioRequest) throws RepositoryError {
        Validations.validateNullParameter(usuarioRequest);
        Validations.validateNullParameter(usuarioRequest.getContrasenia());
        Validations.validateNullParameter(usuarioRequest.getContraseniaNueva());
        return securityRepository.changePassword(usuarioRequest);
    }

    public Mensaje logOut(EmailUsuarioRequest emailUsuarioRequest, String idOneSignal) throws RepositoryError {
        Validations.validateNullParameter(emailUsuarioRequest);
        return securityRepository.logOut(emailUsuarioRequest,idOneSignal);
    }
}
