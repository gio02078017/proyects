package app.epm.com.security_presentation.repositories.profile;


import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.MensajeDTO;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import app.epm.com.security_domain.profile.IProfileRepository;
import app.epm.com.security_presentation.dto.UsuarioDTO;
import app.epm.com.security_presentation.helpers.Mapper;
import app.epm.com.security_presentation.services.ISecurityServices;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.services.ServicesFactory;
import retrofit.RetrofitError;

/**
 * Created by leidycarolinazuluagabastidas on 29/11/16.
 */

public class ProfileRepository implements IProfileRepository {

    private ISecurityServices securityServices;
    private Gson gson;

    public ProfileRepository(ICustomSharedPreferences customSharedPreferences) {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences);
        securityServices = (ISecurityServices) servicesFactory.getInstance(ISecurityServices.class);
        gson = new GsonBuilder().disableHtmlEscaping().create();
    }

    /**
     * Permite encriptar la informacion y consumir el servicio
     * @param usuario
     * @return
     * @throws RepositoryError
     */
    @Override
    public Mensaje updateProfile(Usuario usuario) throws RepositoryError {
        try{
            UsuarioDTO usuarioDTO = Mapper.convertUsuarioDomainToDTOProfile(usuario);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(usuarioDTO));
            MensajeDTO mensajeDTO = securityServices.updateProfile(dataDTO);
            return Mapper.convertMensajeDTOToDomain(mensajeDTO);
        }catch (RetrofitError retrofitError){
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }
}
