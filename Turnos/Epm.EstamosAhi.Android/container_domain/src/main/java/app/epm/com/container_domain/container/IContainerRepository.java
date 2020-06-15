package app.epm.com.container_domain.container;


import com.epm.app.business_models.business_models.ListasGenerales;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.container_domain.business_models.EmailUsuarioRequest;
import app.epm.com.container_domain.business_models.InformacionEspacioPromocional;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public interface IContainerRepository {

    ListasGenerales getGeneralList() throws RepositoryError;

    Authoken getGuestLogin() throws RepositoryError;

    Authoken getAutoLogin(String token) throws RepositoryError;

    InformacionEspacioPromocional getEspacioPromocional() throws RepositoryError;
}
