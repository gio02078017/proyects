package app.epm.com.container_domain.noticias;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public interface INoticiasRepository {

    List<NoticiasEventos> getNoticias() throws RepositoryError;
}
