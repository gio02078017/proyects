package com.epm.app.repositories.lineas_de_atencion;

import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.dto.LineaDeAtencionDTO;
import com.epm.app.helpers.Mapper;
import com.epm.app.services.ILineasDeAtencionServices;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.List;

import app.epm.com.container_domain.business_models.LineaDeAtencion;
import app.epm.com.container_domain.lineas_de_atencion.ILineasDeAtencionRepository;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.services.ServicesFactory;
import retrofit.RetrofitError;

/**
 * Created by root on 30/03/17.
 */

public class LineasDeAtencionRepository implements ILineasDeAtencionRepository {

    private ILineasDeAtencionServices lineasDeAtencionServices;
   
    public LineasDeAtencionRepository(ICustomSharedPreferences customSharedPreferences) {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences);
        lineasDeAtencionServices = (ILineasDeAtencionServices) servicesFactory.getInstance(ILineasDeAtencionServices.class);
    }

    @Override
    public List<LineaDeAtencion> getLineasDeAtencion() throws RepositoryError {
        try {
            List<LineaDeAtencionDTO>  lineaDeAtencionDTO = lineasDeAtencionServices.consultLineasDeAtencion();
            return Mapper.convertListLineasDeAtencionDTOToDomain(lineaDeAtencionDTO);
        }catch (RetrofitError retrofitError){
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);

        }
    }
}
