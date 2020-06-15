package app.epm.com.contacto_transparente_domain.contacto_transparente;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.contacto_transparente_domain.business_models.GrupoInteres;
import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_domain.business_models.ItemUI;
import app.epm.com.contacto_transparente_domain.business_models.ParametrosEvidencia;
import app.epm.com.contacto_transparente_domain.business_models.Respuesta;
import app.epm.com.utilities.helpers.Validations;

/**
 * Created by leidycarolinazuluagabastidas on 9/03/17.
 */

public class ContactoTransparenteBusinessLogic {

    private IContactoTransparenteRepository contactoTransparenteRepository;

    public ContactoTransparenteBusinessLogic(IContactoTransparenteRepository contactoTransparenteRepository) {
        this.contactoTransparenteRepository = contactoTransparenteRepository;
    }

    public List<GrupoInteres> getGruposDeInteres() throws RepositoryError {
        return contactoTransparenteRepository.getGruposDeInteres();
    }

    public Incidente getIncidente(String codigoIncidente) throws RepositoryError {
        return contactoTransparenteRepository.getIncidente(codigoIncidente);
    }

    public ItemUI sendIncident(Incidente incident) throws RepositoryError {
        Validations.validateNullParameter(incident.getDescripcion());
        Validations.validateNullParameter(incident.getLugarEnDondeSucedio());
        Validations.validateNullParameter(incident.getFechaDeteccion());
        Validations.validateEmptyParameter(incident.getDescripcion());
        Validations.validateEmptyParameter(incident.getLugarEnDondeSucedio());
        Validations.validateEmptyParameter(incident.getFechaDeteccion());
        return contactoTransparenteRepository.sendIncident(incident);
    }

    public Respuesta sendEvidenceFiles(ParametrosEvidencia evidenceParameter) throws RepositoryError {
        return contactoTransparenteRepository.sendEvidenceFiles(evidenceParameter);
    }
}
