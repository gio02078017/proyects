package com.epm.app.repositories.eventos;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.container_domain.eventos.IEventosRepository;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public class EventosRepositoryTest implements IEventosRepository {
    @Override
    public List<NoticiasEventos> getEventos() throws RepositoryError {
        return null;
        //TODO: implementar logica
    }
}
