package app.epm.com.reporte_fraudes_domain.reporte_fraudes;

import android.content.Context;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;

import java.util.List;

import app.epm.com.app_utilities_domain.ubicacion_danios_fraudes.IBaseUbicacionDaniosOrFraudesRepository;
import app.epm.com.reporte_fraudes_domain.business_models.ParametrossReporteDeFraudes;
import app.epm.com.reporte_fraudes_domain.business_models.ReporteDeFraude;
import app.epm.com.utilities.services.ServicesArcGIS;

/**
 * Created by mateoquicenososa on 10/04/17.
 */

public interface IReporteDeFraudesRepository extends IBaseUbicacionDaniosOrFraudesRepository {

    List<String> getMunicipalitiesWithCoverage(ParametrosCobertura typeService) throws RepositoryError;

    Mensaje sendEmailTheRegister(ParametrossReporteDeFraudes parametrossReporteDeFraudes) throws RepositoryError;

    String sendReportFraudeArcgis(ReporteDeFraude reporteDeFraude, ServicesArcGIS servicesArcGIS, Context context);

    ServiciosMapa getServicioKML() throws RepositoryError;
}
