package com.epm.app.repositories.container;

import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.MensajeDTO;
import com.epm.app.business_models.business_models.ListasGenerales;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.dto.InformacionEspacioPromocionalDTO;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.container_domain.business_models.EmailUsuarioRequest;
import app.epm.com.container_domain.business_models.InformacionEspacioPromocional;
import app.epm.com.container_domain.container.IContainerRepository;

import com.epm.app.dto.AuthokenDTO;
import com.epm.app.dto.EmailUsuarioDTO;
import com.epm.app.dto.ListasGeneralesDTO;
import com.epm.app.helpers.Mapper;
import com.epm.app.services.ISecurityServices;

import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.services.ServicesFactory;
import retrofit.RetrofitError;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class ContainerRepository implements IContainerRepository {

    private ISecurityServices securityServices;
    private Gson gson;

    public ContainerRepository(ICustomSharedPreferences customSharedPreferences) {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences);
        securityServices = (ISecurityServices) servicesFactory.getInstance(ISecurityServices.class);
        gson = new GsonBuilder().disableHtmlEscaping().create();
    }

    @Override
    public ListasGenerales getGeneralList() throws RepositoryError {
        try {
            ListasGeneralesDTO listasGeneralesDTO = securityServices.listGenerals();
            return Mapper.convertListasGeneralesDTOToDomain(listasGeneralesDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public Authoken getGuestLogin() throws RepositoryError {
        try {
            AuthokenDTO authokenDTO = securityServices.guestLogin();
            return Mapper.convertAuthokenDTOToDomain(authokenDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public Authoken getAutoLogin(String token) throws RepositoryError {
        try {
            AuthokenDTO authokenDTO = securityServices.autoLogin(token);
            return Mapper.convertAuthokenDTOToDomain(authokenDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public InformacionEspacioPromocional getEspacioPromocional() throws RepositoryError {
        try {
            InformacionEspacioPromocionalDTO informacionEspacioPromocionalDTO = securityServices.getInformacionEspacioPromocional();
            return Mapper.convertInformacionDTOToDomain(informacionEspacioPromocionalDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }
}