package app.epm.com.container_domain.eventos;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public class EventosBussinesLogic {

    private IEventosRepository eventosRepository;

    public EventosBussinesLogic(IEventosRepository eventosRepository) {
        this.eventosRepository = eventosRepository;
    }


    public List<NoticiasEventos> getEventos() throws RepositoryError {
        return eventosRepository.getEventos();
    }
}
