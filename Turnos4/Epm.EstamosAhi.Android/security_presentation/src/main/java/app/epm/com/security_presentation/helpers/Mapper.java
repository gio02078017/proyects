package app.epm.com.security_presentation.helpers;


import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.business_models.dto.MensajeDTO;


import app.epm.com.security_domain.business_models.ChangePasswordResponse;
import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_presentation.dto.ChangePasswordDTO;
import app.epm.com.security_presentation.dto.ChangePasswordResponseDTO;
import app.epm.com.security_presentation.dto.EmailUsuarioDTO;
import app.epm.com.security_presentation.dto.UsuarioDTO;
import app.epm.com.utilities.helpers.BaseMapper;

/**
 * Created by josetabaresramirez on 18/11/16.
 */

public class Mapper extends BaseMapper {

    public static Usuario convertUsuarioDTOToDomain(UsuarioDTO usuarioDTO) {
        final Usuario usuario = new Usuario();
        usuario.setApellido(usuarioDTO.getApellidos());
        usuario.setCelular(usuarioDTO.getCelular());
        usuario.setCorreoAlternativo(usuarioDTO.getCorreoAlternativo());
        usuario.setCorreoElectronico(usuarioDTO.getCorreoElectronico());
        usuario.setDireccion(usuarioDTO.getDireccion());
        usuario.setEnvioNotificacion(usuarioDTO.isEnvioNotificacion());
        usuario.setFechaNacimiento(usuarioDTO.getFechaNacimiento());
        usuario.setIdGenero(usuarioDTO.getIdGenero());
        usuario.setNombres(usuarioDTO.getNombres());
        usuario.setNumeroIdentificacion(usuarioDTO.getNumeroIdentificacion());
        usuario.setPais(usuarioDTO.getPais());
        usuario.setTelefono(usuarioDTO.getTelefono());
        usuario.setIdTipoIdentificacion(usuarioDTO.getIdTipoIdentificacion());
        usuario.setIdTipoPersona(usuarioDTO.getIdTipoPersona());
        usuario.setIdTipoVivienda(usuarioDTO.getIdTipoVivienda());
        usuario.setToken(usuarioDTO.getToken());
        usuario.setActivo(usuarioDTO.isActivo());
        usuario.setFechaRegistro(usuarioDTO.getFechaRegistro());
        usuario.setAceptoTerminosyCondiciones(usuarioDTO.isAceptoTerminosyCondiciones());
        usuario.setIdUsuario(usuarioDTO.getIdUsuario());
        usuario.setContrasenia(usuarioDTO.getContrasenia());
        return usuario;
    }

    public static RepositoryError convertMensajeDTOToRepositoryError(MensajeDTO mensaje) {
        RepositoryError repositoryError = new RepositoryError(mensaje.getTexto());
        repositoryError.setIdError(mensaje.getCodigo());
        return repositoryError;
    }

    public static UsuarioDTO convertRegisterUsuarioRequestDomainToDTO(Usuario usuario) {
        final UsuarioDTO usuarioDTO = new UsuarioDTO();
        usuarioDTO.setNombres(usuario.getNombres());
        usuarioDTO.setApellidos(usuario.getApellido());
        usuarioDTO.setContrasenia(usuario.getContrasenia());
        usuarioDTO.setNumeroIdentificacion(usuario.getNumeroIdentificacion());
        usuarioDTO.setIdTipoIdentificacion(usuario.getIdTipoIdentificacion());
        usuarioDTO.setCorreoElectronico(usuario.getCorreoElectronico());
        usuarioDTO.setAceptoTerminosyCondiciones(usuario.isAceptoTerminosyCondiciones());
        return usuarioDTO;
    }

    public static EmailUsuarioDTO convertResetPasswordDTOToDomain(EmailUsuarioRequest emailUsuarioRequest) {
        final EmailUsuarioDTO emailUsuarioDTO = new EmailUsuarioDTO();
        emailUsuarioDTO.setCorreoElectronico(emailUsuarioRequest.getCorreoElectronico());
        return emailUsuarioDTO;
    }

    /**
     * Permite convertir el usuario dominio a usuario dto
     * @param usuario informacion del usuario
     * @return usuarioDTO
     */
    public static UsuarioDTO convertUsuarioDomainToDTOProfile(Usuario usuario) {
        final UsuarioDTO usuarioDTO = new UsuarioDTO();
        usuarioDTO.setCorreoElectronico(usuario.getCorreoElectronico());
        usuarioDTO.setApellidos(usuario.getApellido());
        usuarioDTO.setNombres(usuario.getNombres());
        usuarioDTO.setIdTipoIdentificacion(usuario.getIdTipoIdentificacion());
        usuarioDTO.setNumeroIdentificacion(usuario.getNumeroIdentificacion());
        usuarioDTO.setCelular(usuario.getCelular());
        usuarioDTO.setCorreoAlternativo(usuario.getCorreoAlternativo());
        usuarioDTO.setDireccion(usuario.getDireccion());
        usuarioDTO.setFechaNacimiento(usuario.getFechaNacimiento());
        usuarioDTO.setFechaRegistro(usuario.getFechaRegistro());
        usuarioDTO.setIdGenero(usuario.getIdGenero());
        usuarioDTO.setIdTipoPersona(usuario.getIdTipoPersona());
        usuarioDTO.setIdTipoVivienda(usuario.getIdTipoVivienda());
        usuarioDTO.setPais(usuario.getPais());
        usuarioDTO.setTelefono(usuario.getTelefono());
        usuarioDTO.setAceptoTerminosyCondiciones(usuario.isAceptoTerminosyCondiciones());
        return usuarioDTO;
    }

    public static ChangePasswordDTO convertChangePasswordDTOToDomain(UsuarioRequest usuarioRequest) {
        ChangePasswordDTO changePasswordDTO = new ChangePasswordDTO();
        changePasswordDTO.setCorreoElectronico(usuarioRequest.getCorreoElectronico());
        changePasswordDTO.setContraseniaActual(usuarioRequest.getContrasenia());
        changePasswordDTO.setContraseniaNueva(usuarioRequest.getContraseniaNueva());
        return changePasswordDTO;
    }

    public static EmailUsuarioDTO convertLogOutDTOToDomain(EmailUsuarioRequest emailUsuarioRequest) {
        final EmailUsuarioDTO emailUsuarioDTO = new EmailUsuarioDTO();
        emailUsuarioDTO.setCorreoElectronico(emailUsuarioRequest.getCorreoElectronico());
        return emailUsuarioDTO;
    }

    public static ChangePasswordResponse convertChangePasswordResponseDTOToDomain(ChangePasswordResponseDTO changePasswordResponseDTO) {
        ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse();
        if(changePasswordResponseDTO.getMensaje() != null) {
            Mensaje mensaje = new Mensaje();
            mensaje.setCode(changePasswordResponseDTO.getMensaje().getCodigo());
            mensaje.setText(changePasswordResponseDTO.getMensaje().getTexto());
            changePasswordResponse.setMensaje(mensaje);
        }
        changePasswordResponse.setToken(changePasswordResponseDTO.getToken());

        return  changePasswordResponse;
    }
}
