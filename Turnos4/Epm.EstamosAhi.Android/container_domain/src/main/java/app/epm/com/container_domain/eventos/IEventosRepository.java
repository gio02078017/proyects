package app.epm.com.container_domain.eventos;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public interface IEventosRepository {
    List<NoticiasEventos> getEventos() throws RepositoryError;
}
