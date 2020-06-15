package app.epm.com.reporte_fraudes_domain.reporte_fraudes;

import android.content.Context;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;

import java.util.List;

import app.epm.com.app_utilities_domain.ubicacion_danios_fraudes.BaseUbicacionDaniosOrFraudesBusinessLogic;
import app.epm.com.reporte_fraudes_domain.business_models.ParametrossReporteDeFraudes;
import app.epm.com.reporte_fraudes_domain.business_models.ReporteDeFraude;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.services.ServicesArcGIS;

/**
 * Created by mateoquicenososa on 10/04/17.
 */

public class ReporteDeFraudesBusinessLogic extends BaseUbicacionDaniosOrFraudesBusinessLogic{

    private IReporteDeFraudesRepository reporteDeFraudesRepository;

    public ReporteDeFraudesBusinessLogic(IReporteDeFraudesRepository reporteDeFraudesRepository) {
        super(reporteDeFraudesRepository);
        this.reporteDeFraudesRepository = reporteDeFraudesRepository;
    }

    public List<String> getMunicipalitiesWithCoverage(ParametrosCobertura parametrosCobertura) throws RepositoryError {
        Validations.validateNullParameter(parametrosCobertura);
        Validations.validateNullParameter(parametrosCobertura.getDepartamento());
        Validations.validateEmptyParameter(parametrosCobertura.getDepartamento());
        return reporteDeFraudesRepository.getMunicipalitiesWithCoverage(parametrosCobertura);
    }

    public Mensaje sendEmailTheRegister(ParametrossReporteDeFraudes parametrossReporteDeFraudes) throws RepositoryError {
        Validations.validateNullParameter(parametrossReporteDeFraudes);
        Validations.validateNullParameter(parametrossReporteDeFraudes.getCorreoElectronico(),
                parametrossReporteDeFraudes.getNombre(), parametrossReporteDeFraudes.getNumeroRadicado(),
                parametrossReporteDeFraudes.getTipoServicio());
        return reporteDeFraudesRepository.sendEmailTheRegister(parametrossReporteDeFraudes);
    }

    public String sendReportFraudeArcgis(ReporteDeFraude reporteDeFraude, ServicesArcGIS servicesArcGIS, Context context) {
        return reporteDeFraudesRepository.sendReportFraudeArcgis(reporteDeFraude, servicesArcGIS, context);
    }

    public ServiciosMapa getServicioKML() throws RepositoryError {
        return reporteDeFraudesRepository.getServicioKML();
    }
}