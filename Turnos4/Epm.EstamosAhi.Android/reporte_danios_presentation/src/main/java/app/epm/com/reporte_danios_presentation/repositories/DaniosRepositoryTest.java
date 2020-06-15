package app.epm.com.reporte_danios_presentation.repositories;

import android.content.Context;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;

import java.util.ArrayList;
import java.util.List;

import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import app.epm.com.reporte_danios_domain.danios.business_models.ParametrosEnviarCorreo;
import app.epm.com.reporte_danios_domain.danios.business_models.ReportDanio;
import app.epm.com.reporte_danios_domain.danios.danios.IDaniosRepository;
import app.epm.com.utilities.services.ServicesArcGIS;

/**
 * Created by josetabaresramirez on 2/02/17.
 */

public class DaniosRepositoryTest implements IDaniosRepository {

    @Override
    public ServiciosMapa getServicesKML() throws RepositoryError {
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

    @Override
    public List<String> getMunicipios(ParametrosCobertura parametrosCobertura) throws RepositoryError {
        return new ArrayList<>();
    }

    @Override
    public Mensaje sendEmail(ParametrosEnviarCorreo parametrosEnviarCorreo) throws RepositoryError {
        return null;
    }

    @Override
    public String sendReportDanioArcgis(ReportDanio reportDanio, ServicesArcGIS servicesArcGIS, Context context)  {
        return null;
    }
}
