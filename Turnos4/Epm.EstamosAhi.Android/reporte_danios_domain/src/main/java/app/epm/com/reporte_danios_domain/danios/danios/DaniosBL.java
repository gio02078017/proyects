package app.epm.com.reporte_danios_domain.danios.danios;

import android.content.Context;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;

import java.util.List;


import app.epm.com.app_utilities_domain.ubicacion_danios_fraudes.BaseUbicacionDaniosOrFraudesBusinessLogic;
import app.epm.com.reporte_danios_domain.danios.business_models.ParametrosEnviarCorreo;
import app.epm.com.reporte_danios_domain.danios.business_models.ReportDanio;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.services.ServicesArcGIS;

/**
 * Created by leidycarolinazuluagabastidas on 2/02/17.
 */

public class DaniosBL extends BaseUbicacionDaniosOrFraudesBusinessLogic {
    private IDaniosRepository daniosRepository;

    public DaniosBL(IDaniosRepository daniosRepository) {
        super(daniosRepository);
        this.daniosRepository = daniosRepository;
    }

    public ServiciosMapa getServicesKML() throws RepositoryError {
        return daniosRepository.getServicesKML();
    }

    public List<String> getMunicipios(ParametrosCobertura parametrosCobertura) throws RepositoryError {
        Validations.validateNullParameter(parametrosCobertura);
        Validations.validateNullParameter(parametrosCobertura.getDepartamento());
        Validations.validateEmptyParameter(parametrosCobertura.getDepartamento());

        return daniosRepository.getMunicipios(parametrosCobertura);
    }

    public Mensaje sendEmail(ParametrosEnviarCorreo parametrosEnviarCorreo) throws RepositoryError{
        Validations.validateNullParameter(parametrosEnviarCorreo);
        Validations.validateNullParameter(parametrosEnviarCorreo.getNombreServicio());
        Validations.validateNullParameter(parametrosEnviarCorreo.getNumeroRadicado());
        Validations.validateEmptyParameter(parametrosEnviarCorreo.getNombreServicio());
        Validations.validateEmptyParameter(parametrosEnviarCorreo.getNumeroRadicado());

        return daniosRepository.sendEmail(parametrosEnviarCorreo);
    }

    public String sendReportDanioArcgis(ReportDanio reportDanio, ServicesArcGIS servicesArcGIS, Context context) {
            return daniosRepository.sendReportDanioArcgis(reportDanio, servicesArcGIS, context);
    }
}
