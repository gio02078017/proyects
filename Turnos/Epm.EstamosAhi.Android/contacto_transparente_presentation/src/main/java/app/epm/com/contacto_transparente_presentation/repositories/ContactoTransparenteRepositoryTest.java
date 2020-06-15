package app.epm.com.contacto_transparente_presentation.repositories;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.contacto_transparente_domain.business_models.GrupoInteres;
import app.epm.com.contacto_transparente_domain.business_models.Incidente;

import app.epm.com.contacto_transparente_domain.business_models.ItemUI;
import app.epm.com.contacto_transparente_domain.business_models.ParametrosEvidencia;
import app.epm.com.contacto_transparente_domain.business_models.Respuesta;

import app.epm.com.contacto_transparente_domain.contacto_transparente.IContactoTransparenteRepository;

/**
 * Created by leidycarolinazuluagabastidas on 10/03/17.
 */

public class ContactoTransparenteRepositoryTest implements IContactoTransparenteRepository {


    @Override
    public List<GrupoInteres> getGruposDeInteres() throws RepositoryError {
        return null;
    }

    @Override
    public Incidente getIncidente(String codigoIncidente) throws RepositoryError {
        return null;
    }

    @Override
    public ItemUI sendIncident(Incidente incident) throws RepositoryError {
        return null;
    }

    @Override
    public Respuesta sendEvidenceFiles(ParametrosEvidencia evidenceParameter) throws RepositoryError {
        return null;
    }

}
