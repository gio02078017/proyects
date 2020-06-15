package com.epm.app.repositories.noticias;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.container_domain.noticias.INoticiasRepository;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public class NoticiasRepositoryTest implements INoticiasRepository {


    @Override
    public List<NoticiasEventos> getNoticias() throws RepositoryError {
        return null;
    }
}
