package app.epm.com.reporte_fraudes_presentation.repositories;

import android.content.Context;

import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;

import java.util.List;

import app.epm.com.reporte_fraudes_domain.business_models.ParametrossReporteDeFraudes;
import app.epm.com.reporte_fraudes_domain.business_models.ReporteDeFraude;
import app.epm.com.reporte_fraudes_domain.reporte_fraudes.IReporteDeFraudesRepository;
import app.epm.com.utilities.services.ServicesArcGIS;

/**
 * Created by mateoquicenososa on 10/04/17.
 */

public class ReporteDeFraudesRepositoryTest implements IReporteDeFraudesRepository {

    @Override
    public List<String> getMunicipalitiesWithCoverage(ParametrosCobertura typeService) {
        return null;
    }

    @Override
    public Mensaje sendEmailTheRegister(ParametrossReporteDeFraudes parametrossReporteDeFraudes) throws RepositoryError {
        return null;
    }

    @Override
    public String sendReportFraudeArcgis(ReporteDeFraude reporteDeFraude, ServicesArcGIS servicesArcGIS, Context context) {
        return null;
    }

    @Override
    public ServiciosMapa getServicioKML() throws RepositoryError {
        return null;
    }

    @Override
    public InformacionDeUbicacion getInformacionDeUbicacionWithAddress(String lat, String lon) throws RepositoryError {
        return null;
    }

    @Override
    public InformacionDeUbicacion getBasicInformacionDeUbicacion(String lat, String lon) throws RepositoryError {
        return null;
    }
}
