package app.epm.com.security_presentation.repositories.profile;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.security_domain.profile.IProfileRepository;


/**
 * Created by leidycarolinazuluagabastidas on 29/11/16.
 */

public class ProfileRepositoryTest implements IProfileRepository {


    @Override
    public Mensaje updateProfile(Usuario usuario) throws RepositoryError {
        return null;
    }
}
