package app.epm.com.security_presentation.profile;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.ExpectedException;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.junit.MockitoJUnitRunner;

import app.epm.com.security_domain.profile.IProfileRepository;
import app.epm.com.security_domain.profile.ProfileBL;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 29/11/16.
 */

@RunWith(MockitoJUnitRunner.class)
public class ProfileBLTest {

    @Rule
    public ExpectedException expectedException = ExpectedException.none();

    @Mock
    IProfileRepository profileRepository;

    ProfileBL profileBL;

    @Before
    public void setUp() {
        profileBL = new ProfileBL(profileRepository);
    }

    private Usuario getProfileUsuarioRequest(){
        Usuario  profileUsuarioRequest = new Usuario();
        profileUsuarioRequest.setNombres("eidy carolina");
        profileUsuarioRequest.setApellido("uluaga");
        profileUsuarioRequest.setIdTipoIdentificacion(1);
        profileUsuarioRequest.setNumeroIdentificacion("23242424");
        profileUsuarioRequest.setIdTipoPersona(1);
        profileUsuarioRequest.setTelefono("313131212");
        profileUsuarioRequest.setCelular("231231414");
        profileUsuarioRequest.setDireccion("dsds");
        profileUsuarioRequest.setIdTipoVivienda(1);
        profileUsuarioRequest.setPais("colombia");
        profileUsuarioRequest.setFechaNacimiento("2016-11-11");
        profileUsuarioRequest.setIdGenero(1);
        profileUsuarioRequest.setCorreoAlternativo("cz97@live.com");
        return profileUsuarioRequest;
    }

    @Test
    public void methodUpdateProfileWithMyProfileUsuarioRequestNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        profileBL.updateProfile(null);
    }

    @Test
    public void methodUpdateProfileWithNameNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setNombres(null);
        profileBL.updateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodUpdateProfileWithLastNameNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setApellido(null);
        profileBL.updateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodUpdateProfileWithTypeDocumentNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setIdTipoIdentificacion(null);
        profileBL.updateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodUpdateProfileWithNumberDocumentNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setNumeroIdentificacion(null);
        profileBL.updateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodUpdateProfileWithEmptyNameShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setNombres(Constants.EMPTY_STRING);
        profileBL.updateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodUpdateProfileWithEmptyLastNameShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setApellido(Constants.EMPTY_STRING);
        profileBL.updateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodUpdateProfileWithEmptyNumberDocumentShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setNumeroIdentificacion(Constants.EMPTY_STRING);
        profileBL.updateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodWithCorrectParametersShouldCallMethodUpdateProfileInRepository() throws RepositoryError {
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        profileBL.updateProfile(registerUsuarioRequest);
        verify(profileRepository).updateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterShouldReturnAnMessageWhenCallRegisterInRepository() throws RepositoryError {
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        Mensaje mensaje = new Mensaje();
        when(profileRepository.updateProfile(registerUsuarioRequest)).thenReturn(mensaje);
        Mensaje result = profileBL.updateProfile(registerUsuarioRequest);
        Assert.assertEquals(mensaje, result);
    }

}

