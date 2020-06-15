package app.epm.com.security_domain.profile;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

/**
 * Created by leidycarolinazuluagabastidas on 25/11/16.
 */

public interface IProfileRepository {

    Mensaje updateProfile(Usuario usuario) throws RepositoryError;
}
