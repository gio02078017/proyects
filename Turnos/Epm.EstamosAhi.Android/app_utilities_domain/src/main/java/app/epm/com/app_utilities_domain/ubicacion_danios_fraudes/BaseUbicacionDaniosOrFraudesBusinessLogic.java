package app.epm.com.app_utilities_domain.ubicacion_danios_fraudes;

import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import com.epm.app.business_models.business_models.RepositoryError;

import app.epm.com.utilities.helpers.Validations;

/**
 * Created by mateoquicenososa on 20/04/17.
 */

public class BaseUbicacionDaniosOrFraudesBusinessLogic {

    private IBaseUbicacionDaniosOrFraudesRepository ubicacionDaniosOrFraudesRepository;

    public BaseUbicacionDaniosOrFraudesBusinessLogic(IBaseUbicacionDaniosOrFraudesRepository ubicacionDaniosOrFraudesRepository) {
        this.ubicacionDaniosOrFraudesRepository = ubicacionDaniosOrFraudesRepository;
    }

    public InformacionDeUbicacion getInformacionDeUbicacion(String lat, String lon) throws RepositoryError {
        Validations.validateNullParameter(lat, lon);
        Validations.validateEmptyParameter(lat, lon);

        InformacionDeUbicacion informacionDeUbicacion = ubicacionDaniosOrFraudesRepository.getInformacionDeUbicacionWithAddress(lat, lon);
        if (informacionDeUbicacion != null) {
            return informacionDeUbicacion;
        } else {
            return ubicacionDaniosOrFraudesRepository.getBasicInformacionDeUbicacion(lat, lon);
        }
    }
}
