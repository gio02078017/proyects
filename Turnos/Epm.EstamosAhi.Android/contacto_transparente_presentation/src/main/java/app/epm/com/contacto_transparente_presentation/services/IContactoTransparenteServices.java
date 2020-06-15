package app.epm.com.contacto_transparente_presentation.services;

import java.util.List;

import app.epm.com.contacto_transparente_presentation.dto.IncidentDTO;
import app.epm.com.contacto_transparente_presentation.dto.AnswerDTO;
import app.epm.com.contacto_transparente_presentation.dto.InterestGroupDTO;

import com.epm.app.business_models.dto.DataDTO;

import app.epm.com.contacto_transparente_presentation.dto.ItemUIDTO;
import retrofit.http.Body;
import retrofit.http.GET;
import retrofit.http.Query;
import retrofit.http.POST;

/**
 * Created by leidycarolinazuluagabastidas on 10/03/17.
 */

public interface IContactoTransparenteServices {


    @GET("/ContactoTransparente/ConsultarGruposDeInteres")
    List<InterestGroupDTO> getListaGrupoInteres();

    @GET("/ContactoTransparente/ConsultarIncidente")
    List<IncidentDTO> getConsultaIncidente(@Query("codigoIncidente") String incidentCode);

    @POST("/ContactoTransparente/CrearIncidente")
    ItemUIDTO sendIncident(@Body DataDTO dataDTO);

    @POST("/ContactoTransparente/AdjuntarLasEvidenciasDelActoIndebido")
    AnswerDTO sendEvidenceFiles(@Body DataDTO dataDTO);
}
