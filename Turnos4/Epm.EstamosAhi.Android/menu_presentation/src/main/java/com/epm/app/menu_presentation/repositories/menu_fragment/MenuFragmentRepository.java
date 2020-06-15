package com.epm.app.menu_presentation.repositories.menu_fragment;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.MensajeDTO;
import com.epm.app.menu_presentation.dto.EmailUsuarioDTO;
import com.epm.app.menu_presentation.helpers.Mapper;
import com.epm.app.menu_presentation.services.IMenuFragmentServices;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import app.epm.com.security_domain.business_models.ChangePasswordResponse;
import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_domain.security.ISecurityRepository;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.services.ServicesFactory;
import retrofit.RetrofitError;

/**
 * Created by juan on 11/04/17.
 */

public class MenuFragmentRepository implements ISecurityRepository {

    private IMenuFragmentServices menuFragmentServices;
    private Gson gson;

    public MenuFragmentRepository(ICustomSharedPreferences customSharedPreferences) {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences);
        menuFragmentServices = (IMenuFragmentServices) servicesFactory.getInstance(IMenuFragmentServices.class);
        gson = new GsonBuilder().disableHtmlEscaping().create();
    }

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
        try {
            EmailUsuarioDTO emailUsuarioDTO = Mapper.convertLogOutDTOToDomain(emailUsuarioRequest);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(emailUsuarioDTO));
            MensajeDTO mensajeDTO = menuFragmentServices.logOut(dataDTO, idOneSignal);
            return Mapper.convertMensajeDTOToDomain(mensajeDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }
}
