package app.epm.com.contacto_transparente_presentation.repositories;

import com.epm.app.business_models.business_models.RepositoryError;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.epm.app.business_models.dto.DataDTO;

import java.util.List;

import app.epm.com.contacto_transparente_domain.business_models.GrupoInteres;
import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_domain.contacto_transparente.IContactoTransparenteRepository;
import app.epm.com.contacto_transparente_domain.business_models.ItemUI;
import app.epm.com.contacto_transparente_domain.business_models.ParametrosEvidencia;
import app.epm.com.contacto_transparente_domain.business_models.Respuesta;
import app.epm.com.contacto_transparente_presentation.dto.AnswerDTO;
import app.epm.com.contacto_transparente_presentation.dto.EvidenceParametersDTO;
import app.epm.com.contacto_transparente_presentation.dto.IncidentDTO;
import app.epm.com.contacto_transparente_presentation.dto.InterestGroupDTO;
import app.epm.com.contacto_transparente_presentation.dto.ItemUIDTO;
import app.epm.com.contacto_transparente_presentation.helper.Mapper;
import app.epm.com.contacto_transparente_presentation.services.IContactoTransparenteServices;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.services.ServicesFactory;
import retrofit.RetrofitError;

/**
 * Created by leidycarolinazuluagabastidas on 10/03/17.
 */

public class ContactoTransparenteRepository implements IContactoTransparenteRepository {

    private IContactoTransparenteServices contactoTransparenteServices;
    private Gson gson;

    public ContactoTransparenteRepository(ICustomSharedPreferences customSharedPreferences) {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences);
        contactoTransparenteServices = (IContactoTransparenteServices) servicesFactory.getInstance(IContactoTransparenteServices.class);
        gson = new GsonBuilder().disableHtmlEscaping().create();
    }

    @Override
    public List<GrupoInteres> getGruposDeInteres() throws RepositoryError {
        try {
            List<InterestGroupDTO> interestGroupDTOs = contactoTransparenteServices.getListaGrupoInteres();
            return Mapper.convertListInterestGroupDTOToDomain(interestGroupDTOs);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);

        }

    }

    @Override
    public Incidente getIncidente(String codigoIncidente) throws RepositoryError {
        try {
            List<IncidentDTO> IncidenteDTO = contactoTransparenteServices.getConsultaIncidente(codigoIncidente);
            IncidentDTO item = IncidenteDTO.get(0);
            if (item.getCodigo() != 0 && item.getTexto() != null) {
                throw new RepositoryError(item.getTexto());
            } else {
                return Mapper.convertIncidentDTOToDomain(IncidenteDTO.get(0));
            }

        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }

    }

    @Override
    public ItemUI sendIncident(Incidente incident) throws RepositoryError {
        try {
            IncidentDTO incidentDTO = Mapper.convertIncidentDomainToDTO(incident);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(incidentDTO));
            ItemUIDTO itemUIDTO = contactoTransparenteServices.sendIncident(dataDTO);
            return Mapper.convertItemUIDTOToDomain(itemUIDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public Respuesta sendEvidenceFiles(ParametrosEvidencia evidenceParameter) throws RepositoryError {
        try {
            EvidenceParametersDTO evidenceParametersDTO = Mapper.convertEvidenceParameterDomainToDTO(evidenceParameter);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(evidenceParametersDTO));
            AnswerDTO answerDTO = contactoTransparenteServices.sendEvidenceFiles(dataDTO);
            return Mapper.convertRespuestaDTOToDomain(answerDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }


}
