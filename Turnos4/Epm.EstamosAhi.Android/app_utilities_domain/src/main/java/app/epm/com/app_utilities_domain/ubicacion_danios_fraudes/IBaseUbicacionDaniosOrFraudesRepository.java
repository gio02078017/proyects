package app.epm.com.app_utilities_domain.ubicacion_danios_fraudes;

import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import com.epm.app.business_models.business_models.RepositoryError;

/**
 * Created by mateoquicenososa on 20/04/17.
 */

public interface IBaseUbicacionDaniosOrFraudesRepository {

    InformacionDeUbicacion getInformacionDeUbicacionWithAddress(String lat, String lon) throws RepositoryError;

    InformacionDeUbicacion getBasicInformacionDeUbicacion(String lat, String lon) throws RepositoryError;
}
