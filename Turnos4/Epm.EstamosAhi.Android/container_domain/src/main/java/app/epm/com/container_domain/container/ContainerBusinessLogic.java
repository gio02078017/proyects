package app.epm.com.container_domain.container;

import com.epm.app.business_models.business_models.ListasGenerales;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.container_domain.business_models.EmailUsuarioRequest;
import app.epm.com.container_domain.business_models.InformacionEspacioPromocional;
import app.epm.com.utilities.helpers.Validations;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class ContainerBusinessLogic {

    private IContainerRepository securityRepository;

    public ContainerBusinessLogic(IContainerRepository securityRepository) {
        this.securityRepository = securityRepository;
    }

    public ListasGenerales getGeneralList() throws RepositoryError {
        return securityRepository.getGeneralList();
    }

    public Authoken getGuestLogin() throws RepositoryError {
        return securityRepository.getGuestLogin();
    }

    public Authoken getAutoLogin(String token) throws RepositoryError {
        return securityRepository.getAutoLogin(token);
    }

    public InformacionEspacioPromocional getEspacioPromocional() throws RepositoryError {
        return securityRepository.getEspacioPromocional();
    }
}
