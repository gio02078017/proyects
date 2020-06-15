package app.epm.com.security_presentation.repositories.security;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.security_domain.business_models.ChangePasswordResponse;
import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_domain.security.ISecurityRepository;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class SecurityRepositoryTest implements ISecurityRepository {

    @Override
    public Usuario login(UsuarioRequest usuarioRequest, String idOneSignal) throws RepositoryError {
        Usuario usuario = new Usuario();
        usuario.setNombres("test");
        usuario.setApellido("Repositorio de pruebas");
        usuario.setActivo(true);
        usuario.setToken("sfgsfgsfg46465");
        return usuario;
    }

    @Override
    public Mensaje register(Usuario usuario) throws RepositoryError {
        Mensaje mensaje = new Mensaje();
        mensaje.setText("Registro exitoso. Repositorio de pruebas");
        mensaje.setCode(1);
        return mensaje;
    }

    @Override
    public Mensaje resetPassword(EmailUsuarioRequest resetPasswordRequest) throws RepositoryError {
        Mensaje mensaje = new Mensaje();
        mensaje.setText("Reactivación exitosa. Repositorio de pruebas");
        mensaje.setCode(1);
        return mensaje;
    }

    @Override
    public ChangePasswordResponse changePassword(UsuarioRequest usuarioRequest) {
        ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse();
        Mensaje mensaje = new Mensaje();
        mensaje.setText("Cambio exitoso. Repositorio de pruebas");
        mensaje.setCode(1);
        changePasswordResponse.setMensaje(mensaje);
        changePasswordResponse.setToken("dfsadgfasfdasfdsa24");
        return changePasswordResponse;
    }

    @Override
    public Mensaje logOut(EmailUsuarioRequest emailUsuarioRequest, String idOneSignal) throws RepositoryError {
        Mensaje mensaje = new Mensaje();
        mensaje.setText("Sesión finalizada. Repositorio de pruebas");
        mensaje.setCode(1);
        return mensaje;
    }
}