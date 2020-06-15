package app.epm.com.security_domain.profile;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.security_domain.profile.IProfileRepository;
import app.epm.com.utilities.helpers.Validations;

/**
 * Created by leidycarolinazuluagabastidas on 25/11/16.
 */

public class ProfileBL {

    private IProfileRepository profileRepository;

    public ProfileBL(IProfileRepository profileRepository){
        this.profileRepository = profileRepository;
    }

    /**
     * Permite validar la informacion e ir al repositorio
     * @param usuario informacion del usuario
     * @return
     * @throws RepositoryError
     */
    public Mensaje updateProfile(Usuario usuario)  throws RepositoryError{
        Validations.validateNullParameter(usuario);
        Validations.validateNullParameter(usuario.getNombres(),usuario.getApellido(),usuario.getIdTipoIdentificacion(),
                usuario.getNumeroIdentificacion());
        Validations.validateEmptyParameter(usuario.getNombres(),usuario.getApellido(),usuario.getNumeroIdentificacion());

        return profileRepository.updateProfile(usuario);
    }
}
