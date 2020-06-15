package com.epm.app.repositories.noticias;

import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.dto.NoticiasEventosDTO;
import com.epm.app.helpers.Mapper;
import com.epm.app.services.INoticiasServices;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.container_domain.noticias.INoticiasRepository;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.services.ServicesFactory;
import retrofit.RetrofitError;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public class NoticiasRepository implements INoticiasRepository {

    private INoticiasServices noticiasServices;



    public NoticiasRepository(ICustomSharedPreferences customSharedPreferences) {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences);
        noticiasServices = (INoticiasServices) servicesFactory.getInstance(INoticiasServices.class);
    }

    @Override
    public List<NoticiasEventos> getNoticias() throws RepositoryError {
        try{
            List<NoticiasEventosDTO> noticiasEventosDTO = noticiasServices.getNoticias();
            return Mapper.convertListNoticiasEventosDTOToDomain(noticiasEventosDTO);
        }catch (RetrofitError retrofitError){
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }

    }
}
