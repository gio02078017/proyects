package com.epm.app.repositories.eventos;

import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.dto.NoticiasEventosDTO;
import com.epm.app.helpers.Mapper;
import com.epm.app.services.IEventosServices;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.container_domain.eventos.IEventosRepository;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.services.ServicesFactory;
import retrofit.RetrofitError;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public class EventosRepository implements IEventosRepository {

    private IEventosServices eventosServices;

    public EventosRepository(ICustomSharedPreferences customSharedPreferences) {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences);
        eventosServices = (IEventosServices) servicesFactory.getInstance(IEventosServices.class);
    }

    @Override
    public List<NoticiasEventos> getEventos() throws RepositoryError {
        try{
            List<NoticiasEventosDTO> noticiasEventosDTO = eventosServices.Eventos();
            return Mapper.convertListNoticiasEventosDTOToDomain(noticiasEventosDTO);
        }catch (RetrofitError retrofitError){
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }
}
