package com.epm.app.repositories.lineas_de_atencion;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.container_domain.business_models.LineaDeAtencion;
import app.epm.com.container_domain.lineas_de_atencion.ILineasDeAtencionRepository;

/**
 * Created by root on 30/03/17.
 */

public class LineasDeAtencionRepositoryTest implements ILineasDeAtencionRepository {

    @Override
    public List<LineaDeAtencion> getLineasDeAtencion() throws RepositoryError {
        return null;
    }
}
