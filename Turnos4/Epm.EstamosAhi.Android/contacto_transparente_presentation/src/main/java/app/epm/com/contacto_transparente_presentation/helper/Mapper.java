package app.epm.com.contacto_transparente_presentation.helper;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.contacto_transparente_domain.business_models.Evidencia;
import app.epm.com.contacto_transparente_domain.business_models.GrupoInteres;
import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_domain.business_models.ItemUI;
import app.epm.com.contacto_transparente_domain.business_models.ParametrosEvidencia;
import app.epm.com.contacto_transparente_domain.business_models.Respuesta;
import app.epm.com.contacto_transparente_presentation.dto.AnswerDTO;
import app.epm.com.contacto_transparente_presentation.dto.EvidenceDTO;
import app.epm.com.contacto_transparente_presentation.dto.EvidenceParametersDTO;
import app.epm.com.contacto_transparente_presentation.dto.IncidentDTO;
import app.epm.com.contacto_transparente_presentation.dto.InterestGroupDTO;
import app.epm.com.contacto_transparente_presentation.dto.ItemUIDTO;
import app.epm.com.utilities.helpers.BaseMapper;

/**
 * Created by leidycarolinazuluagabastidas on 10/03/17.
 */

public class Mapper extends BaseMapper {

    public static List<GrupoInteres> convertListInterestGroupDTOToDomain(List<InterestGroupDTO> interestGroupDTOs) {
        ArrayList<GrupoInteres> list = new ArrayList<>();

        for (InterestGroupDTO interestGroup : interestGroupDTOs) {
            GrupoInteres grupoInteres = new GrupoInteres();
            grupoInteres.setId(interestGroup.getIdGrupoInteres());
            grupoInteres.setDescripcion(interestGroup.getNombreGrupoInteres());

            list.add(grupoInteres);
        }
        return list;
    }

    public static Incidente convertIncidentDTOToDomain(IncidentDTO incidentDTO) {
        Incidente incidente = new Incidente();
        incidente.setFechaCreacion(incidentDTO.getFechaCreacion());
        incidente.setIdEstado(incidentDTO.getIdEstado());
        incidente.setNombreDelEstado(incidentDTO.getNombreDelEstado());
        incidente.setNombreGrupoInteres(incidentDTO.getNombreGrupoInteres());
        incidente.setFechaDeSeguimiento(incidentDTO.getFechaDeSeguimiento());
        incidente.setInformeDeLaDenuncia(incidentDTO.getInformeDeLaDenuncia());
        incidente.setFechaDeteccion(incidentDTO.getFechaDeteccion());
        incidente.setPersonasInvolucradas(incidentDTO.getPersonasInvolucradas());
        incidente.setPersonasInvolucradasEnLaEmpresa(incidentDTO.getPersonasInvolucradasEnLaEmpresa());
        incidente.setAnonimo(incidentDTO.isAnonimo());
        incidente.setNombreContacto(incidentDTO.getNombreContacto());
        incidente.setTelefonoContacto(incidentDTO.getTelefonoContacto());
        incidente.setCorreoElectronicoContacto(incidentDTO.getCorreoElectronicoContacto());
        incidente.setDescripcion(incidentDTO.getDescripcion());
        incidente.setLugarEnDondeSucedio(incidentDTO.getLugarEnDondeSucedio());


        return incidente;

    }

    public static IncidentDTO convertIncidentDomainToDTO(Incidente incident) {
        IncidentDTO incidentDTO = new IncidentDTO();
        incidentDTO.setIdGrupoInteres(incident.getIdGrupoInteres());
        incidentDTO.setDescripcion(incident.getDescripcion());
        incidentDTO.setLugarEnDondeSucedio(incident.getLugarEnDondeSucedio());
        incidentDTO.setPersonasInvolucradas(incident.getPersonasInvolucradas());
        incidentDTO.setPersonasInvolucradasEnLaEmpresa(incident.getPersonasInvolucradasEnLaEmpresa());
        incidentDTO.setAnonimo(incident.isAnonimo());
        incidentDTO.setNombreContacto(incident.getNombreContacto());
        incidentDTO.setTelefonoContacto(incident.getTelefonoContacto());
        incidentDTO.setCorreoElectronicoContacto(incident.getCorreoElectronicoContacto());
        incidentDTO.setFechaDeteccion(incident.getFechaDeteccion());
        return incidentDTO;
    }

    public static ItemUI convertItemUIDTOToDomain(ItemUIDTO itemUIDTO) {
        ItemUI itemUI = new ItemUI();
        itemUI.setLlave(itemUIDTO.getLlave());
        itemUI.setValor(itemUIDTO.getValor());
        return itemUI;
    }

    public static EvidenceParametersDTO convertEvidenceParameterDomainToDTO(ParametrosEvidencia evidenceParameter) {
        EvidenceParametersDTO evidenceParametersDTO = new EvidenceParametersDTO();

        ArrayList<EvidenceDTO> evidenceDTOList = new ArrayList<>();

        for (Evidencia evidence : evidenceParameter.getEvidenciasDelActoIndebido()) {
            EvidenceDTO evidenceDTO = new EvidenceDTO();
            evidenceDTO.setArchivo(evidence.getArchivo());
            evidenceDTO.setNombreDelArchivoConExtension(evidence.getNombreDelArchivo());
            evidenceDTOList.add(evidenceDTO);
        }
        evidenceParametersDTO.setCodigoDelActoIndebido(evidenceParameter.getCodigoDelActoIndebido());
        evidenceParametersDTO.setEvidenciasDelActoIndebido(evidenceDTOList);
        return evidenceParametersDTO;
    }

    public static Respuesta convertRespuestaDTOToDomain(AnswerDTO answerDTO) {
        Respuesta answer = new Respuesta();
        answer.setEstado(answerDTO.isEstado());
        return answer;
    }
}
