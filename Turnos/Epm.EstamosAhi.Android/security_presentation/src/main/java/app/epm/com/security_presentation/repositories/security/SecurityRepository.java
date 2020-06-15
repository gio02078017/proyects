package app.epm.com.security_presentation.repositories.security;


import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.MensajeDTO;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import app.epm.com.security_domain.business_models.ChangePasswordResponse;
import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_domain.security.ISecurityRepository;
import app.epm.com.security_presentation.dto.ChangePasswordDTO;
import app.epm.com.security_presentation.dto.ChangePasswordResponseDTO;
import app.epm.com.security_presentation.dto.EmailUsuarioDTO;
import app.epm.com.security_presentation.dto.UsuarioDTO;
import app.epm.com.security_presentation.dto.UsuarioResponseDTO;
import app.epm.com.security_presentation.helpers.Mapper;
import app.epm.com.security_presentation.services.ISecurityServices;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.services.ServicesFactory;
import retrofit.RetrofitError;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class SecurityRepository implements ISecurityRepository {

    private ISecurityServices securityServices;
    private Gson gson;

    public SecurityRepository(ICustomSharedPreferences customSharedPreferences) {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences);
        securityServices = (ISecurityServices) servicesFactory.getInstance(ISecurityServices.class);
        gson = new GsonBuilder().disableHtmlEscaping().create();
    }

    @Override
    public Usuario login(UsuarioRequest usuarioRequest, String idOneSignal) throws RepositoryError {
        try {
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(usuarioRequest));
            UsuarioResponseDTO usuarioResponseDTO = securityServices.login(dataDTO,idOneSignal);
            if (usuarioResponseDTO.getUsuario() != null) {
                return Mapper.convertUsuarioDTOToDomain(usuarioResponseDTO.getUsuario());
            } else {
                throw Mapper.convertMensajeDTOToRepositoryError(usuarioResponseDTO.getMensaje());
            }
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public Mensaje register(Usuario usuario) throws RepositoryError {
        try {
            UsuarioDTO usuarioDTO = Mapper.convertRegisterUsuarioRequestDomainToDTO(usuario);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(usuarioDTO));
            MensajeDTO mensajeDTO = securityServices.registerUser(dataDTO);
            return Mapper.convertMensajeDTOToDomain(mensajeDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public Mensaje resetPassword(EmailUsuarioRequest emailUsuarioRequest) throws RepositoryError {
        try {
            EmailUsuarioDTO emailUsuarioDTO = Mapper.convertResetPasswordDTOToDomain(emailUsuarioRequest);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(emailUsuarioDTO));
            MensajeDTO mensajeDTO = securityServices.resetPassword(dataDTO);
            return Mapper.convertMensajeDTOToDomain(mensajeDTO);
        } catch (RetrofitError retrofitError){
            throw  Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public ChangePasswordResponse changePassword(UsuarioRequest usuarioRequest) throws RepositoryError  {
        try {
            ChangePasswordDTO changePasswordDTO = Mapper.convertChangePasswordDTOToDomain(usuarioRequest);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(changePasswordDTO));
            ChangePasswordResponseDTO changePasswordResponseDTO = securityServices.changePassword(dataDTO);
            return Mapper.convertChangePasswordResponseDTOToDomain(changePasswordResponseDTO);
        } catch (RetrofitError retrofitError){
            throw  Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public Mensaje logOut(EmailUsuarioRequest emailUsuarioRequest, String idOneSignal) throws RepositoryError {
        try {
            EmailUsuarioDTO emailUsuarioDTO = Mapper.convertLogOutDTOToDomain(emailUsuarioRequest);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(emailUsuarioDTO));
            MensajeDTO mensajeDTO = securityServices.logOut(dataDTO, idOneSignal);
            return Mapper.convertMensajeDTOToDomain(mensajeDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

}
