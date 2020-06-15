package app.epm.com.container_domain.lineas_de_atencion;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.container_domain.business_models.LineaDeAtencion;

/**
 * Created by root on 29/03/17.
 */

public class LineasDeAtencionBusinessLogic {

    private ILineasDeAtencionRepository lineasDeAtencionRepository;

    public LineasDeAtencionBusinessLogic(ILineasDeAtencionRepository lineasDeAtencionRepository) {
        this.lineasDeAtencionRepository = lineasDeAtencionRepository;
    }

    public List<LineaDeAtencion> getLineasDeAtencion() throws RepositoryError {
        return lineasDeAtencionRepository.getLineasDeAtencion();
    }
}
