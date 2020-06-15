package app.epm.com.reporte_danios_presentation.services;

import com.epm.app.business_models.dto.ParametrosCoberturaDTO;
import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.MensajeDTO;

import java.util.List;

import com.epm.app.business_models.dto.AddressFromArcgisDTO;
import com.epm.app.business_models.dto.CityFromArcgisDTO;

import com.epm.app.business_models.dto.ServicesMapDTO;
import retrofit.http.Body;
import retrofit.http.GET;
import retrofit.http.POST;
import retrofit.http.Query;


/**
 * Created by josetabaresramirez on 2/02/17.
 */

public interface IDaniosServices {


    @GET("/ReporteDanio/ObtenerInformacionKml")
    ServicesMapDTO getServicesKML();


    @GET("/GeocodeServer/reverseGeocode")
    AddressFromArcgisDTO getInformacionDeUbicacionWithAddress(@Query("location") String location, @Query("f") String format);

    @GET("/InfoMunicipio/FeatureServer/0/query")
    CityFromArcgisDTO getBasicInformacionDeUbicacion(@Query(value = "geometry", encodeValue = false) String location, @Query("f") String f,
                                                     @Query("geometryType") String geometryType, @Query("inSR") String inSR,
                                                     @Query("outFields") String outFields, @Query("returnGeometry") boolean returnGeometry);

    @POST("/Consultas/ConsultarLosMunicipiosConCoberturaPorServicioPublicoYPorDepartamento")
    List<String> getMunicipios(@Body ParametrosCoberturaDTO parametrosCoberturaDTO);

    @POST("/ReporteDanio/EnvioCorreoRegistroDanio")
    MensajeDTO sendEmail(@Body DataDTO dataDTO);

}
