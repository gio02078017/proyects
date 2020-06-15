package app.epm.com.reporte_danios_domain.danios.danios;

import android.content.Context;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;

import java.util.List;

import app.epm.com.app_utilities_domain.ubicacion_danios_fraudes.IBaseUbicacionDaniosOrFraudesRepository;
import app.epm.com.reporte_danios_domain.danios.business_models.ParametrosEnviarCorreo;
import app.epm.com.reporte_danios_domain.danios.business_models.ReportDanio;
import app.epm.com.utilities.services.ServicesArcGIS;

public interface IDaniosRepository extends IBaseUbicacionDaniosOrFraudesRepository {

    ServiciosMapa getServicesKML() throws RepositoryError;

    List<String> getMunicipios(ParametrosCobertura parametrosCobertura) throws RepositoryError;

    Mensaje sendEmail(ParametrosEnviarCorreo parametrosEnviarCorreo) throws RepositoryError;

    String sendReportDanioArcgis(ReportDanio reportDanio, ServicesArcGIS servicesArcGIS, Context context);
}
