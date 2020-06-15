package app.epm.com.contacto_transparente_domain.contacto_transparente;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.contacto_transparente_domain.business_models.GrupoInteres;
import app.epm.com.contacto_transparente_domain.business_models.Incidente;

import app.epm.com.contacto_transparente_domain.business_models.ItemUI;
import app.epm.com.contacto_transparente_domain.business_models.ParametrosEvidencia;
import app.epm.com.contacto_transparente_domain.business_models.Respuesta;


/**
 * Created by leidycarolinazuluagabastidas on 9/03/17.
 */

public interface IContactoTransparenteRepository {

    List<GrupoInteres> getGruposDeInteres() throws RepositoryError;

    Incidente getIncidente(String codigoIncidente) throws RepositoryError;

    ItemUI sendIncident(Incidente incident) throws RepositoryError;

    Respuesta sendEvidenceFiles(ParametrosEvidencia evidenceParameter) throws RepositoryError;

}
