package app.epm.com.reporte_fraudes_presentation.services;

import com.epm.app.business_models.dto.AddressFromArcgisDTO;
import com.epm.app.business_models.dto.CityFromArcgisDTO;
import com.epm.app.business_models.dto.ParametrosCoberturaDTO;
import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.MensajeDTO;
import com.epm.app.business_models.dto.ServicesMapDTO;

import java.util.List;

import retrofit.http.Body;
import retrofit.http.GET;
import retrofit.http.POST;
import retrofit.http.Query;

/**
 * Created by mateoquicenososa on 10/04/17.
 */

public interface IReporteDeFraudesServices {

    @POST("/ReporteFraude/EnvioCorreoRegistroFraude")
    MensajeDTO sendEmailTheRegister(@Body DataDTO dataDTO);

    @POST("/Consultas/ConsultarLosMunicipiosConCoberturaPorServicioPublicoYPorDepartamento")
    List<String> getMunicipalitiesWithCoverage(@Body ParametrosCoberturaDTO parametrosEnviarCorreoDTO);

    @GET("/ReporteFraude/ObtenerInformacionKml")
    ServicesMapDTO getServicioKML();

    @GET("/GeocodeServer/reverseGeocode")
    AddressFromArcgisDTO getInformacionDeUbicacionWithAddress(@Query("location") String location, @Query("f") String format);

    @GET("/InfoMunicipio/FeatureServer/0/query")
    CityFromArcgisDTO getBasicInformacionDeUbicacion(@Query(value = "geometry", encodeValue = false) String location, @Query("f") String f,
                                                     @Query("geometryType") String geometryType, @Query("inSR") String inSR,
                                                     @Query("outFields") String outFields, @Query("returnGeometry") boolean returnGeometry);


}
