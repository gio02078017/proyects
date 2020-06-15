package app.epm.com.security_presentation.security;

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

import app.epm.com.security_domain.business_models.ChangePasswordResponse;
import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_domain.security.ISecurityRepository;
import app.epm.com.security_domain.security.SecurityBusinessLogic;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by josetabaresramirez on 17/11/16.
 */
@RunWith(MockitoJUnitRunner.class)
public class SecurityBusinessLogicTest {

    @Rule
    public ExpectedException expectedException = ExpectedException.none();

    SecurityBusinessLogic securityBusinessLogic;

    @Mock
    ISecurityRepository securityRepository;

    @Before
    public void setUp() {
        securityBusinessLogic = new SecurityBusinessLogic(securityRepository);
    }

    private UsuarioRequest getUsuarioResponseInstance() {
        UsuarioRequest usuarioRequest = new UsuarioRequest();
        usuarioRequest.setCorreoElectronico("lcz97@live.com");
        usuarioRequest.setContrasenia("test");
        usuarioRequest.setContraseniaNueva("test2");
        return usuarioRequest;
    }

    private Usuario getRegisterUsuarioRequest() {
        Usuario registerUsuarioRequest = new Usuario();
        registerUsuarioRequest.setCorreoElectronico("quiceno1127@gmail.com");
        registerUsuarioRequest.setNombres("Mateo");
        registerUsuarioRequest.setApellido("Quiceno Sosa");
        registerUsuarioRequest.setIdTipoIdentificacion(1);
        registerUsuarioRequest.setNumeroIdentificacion("1017253093");
        registerUsuarioRequest.setContrasenia("Mqs1017253093.");
        return registerUsuarioRequest;
    }

    private EmailUsuarioRequest getEmailUsuarioRequest() {
        EmailUsuarioRequest resetPasswordRequest = new EmailUsuarioRequest();
        resetPasswordRequest.setCorreoElectronico("quiceno1127@gmail.com");
        return resetPasswordRequest;
    }

    /**
     * Start Login.
     *
     * @throws RepositoryError
     */
    @Test
    public void methodLoginWithResponseNullShouldReturnAnException() throws RepositoryError {
        String idOneSignal = "1212-131334-3435";
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        securityBusinessLogic.login(null, idOneSignal);
    }

    @Test
    public void methodLoginWithEmailNullShouldReturnAnException() throws RepositoryError {
        String idOneSignal = "1212-131334-3435";
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setCorreoElectronico(null);
        securityBusinessLogic.login(usuarioRequest, idOneSignal);
    }

    @Test
    public void methodLoginWithPasswordNullShouldReturnAnException() throws RepositoryError {
        String idOneSignal = "1212-131334-3435";
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setContrasenia(null);
        securityBusinessLogic.login(usuarioRequest, idOneSignal);
    }

    @Test
    public void methodLoginWithEmptyEmailShouldReturnAnException() throws RepositoryError {
        String idOneSignal = "1212-131334-3435";
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setCorreoElectronico(Constants.EMPTY_STRING);
        securityBusinessLogic.login(usuarioRequest, idOneSignal);
    }

    @Test
    public void methodLoginWithEmptyPasswordShouldReturnAnException() throws RepositoryError {
        String idOneSignal = "1212-131334-3435";
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setContrasenia(Constants.EMPTY_STRING);
        securityBusinessLogic.login(usuarioRequest, idOneSignal);
    }

    @Test
    public void methodLoginWithCorrectParametersShouldCallMethodLoginInRepository() throws RepositoryError {
        String idOneSignal = "1212-131334-3435";
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        securityBusinessLogic.login(usuarioRequest, idOneSignal);
        verify(securityRepository).login(usuarioRequest, idOneSignal);
    }

    @Test
    public void methodLoginShouldReturnAnUsuarioWhenCallLoginInRepository() throws RepositoryError {
        String idOneSignal = "1212-131334-3435";
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        Usuario usuario = new Usuario();
        usuario.setToken("sgg65456");
        when(securityRepository.login(usuarioRequest, idOneSignal)).thenReturn(usuario);
        Usuario result = securityBusinessLogic.login(usuarioRequest, idOneSignal);
        Assert.assertEquals(usuario, result);
    }
    /**
     * End Login.
     */

    /**
     * Start Register
     *
     * @throws RepositoryError
     */
    @Test
    public void methodRegisterWithUsuarioResponseNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        securityBusinessLogic.register(null);
    }

    @Test
    public void methodRegisterWithEmailNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setCorreoElectronico(null);
        securityBusinessLogic.register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterWithNameNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setNombres(null);
        securityBusinessLogic.register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterWithLastNameNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setApellido(null);
        securityBusinessLogic.register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterWithTypeDocumentNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setIdTipoIdentificacion(null);
        securityBusinessLogic.register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterWithNumberDocumentNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setNumeroIdentificacion(null);
        securityBusinessLogic.register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterWithPasswordNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setContrasenia(null);
        securityBusinessLogic.register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterWithEmptyEmailShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setCorreoElectronico(Constants.EMPTY_STRING);
        securityBusinessLogic.register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterWithEmptyNameShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setNombres(Constants.EMPTY_STRING);
        securityBusinessLogic.register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterWithEmptyLastNameShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setApellido(Constants.EMPTY_STRING);
        securityBusinessLogic.register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterWithEmptyNumberDocumentShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setNumeroIdentificacion(Constants.EMPTY_STRING);
        securityBusinessLogic.register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterWithEmptyPasswordShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setContrasenia(Constants.EMPTY_STRING);
        securityBusinessLogic.register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterWithCorrectParametersShouldCallMethodRegisterInRepository() throws RepositoryError {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        securityBusinessLogic.register(registerUsuarioRequest);
        verify(securityRepository).register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterShouldReturnAnMessageWhenCallRegisterInRepository() throws RepositoryError {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        Mensaje mensaje = new Mensaje();
        when(securityRepository.register(registerUsuarioRequest)).thenReturn(mensaje);
        Mensaje result = securityBusinessLogic.register(registerUsuarioRequest);
        Assert.assertEquals(mensaje, result);
    }

    /**
     * End Register.
     */

     /**
     * Start ResetPassword.
     *
     * @throws RepositoryError
     */

    @Test
    public void methodResetPasswordWithUsuarioResponseNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        securityBusinessLogic.resetPassword(null);
    }

    @Test
    public void methodResetPasswordWithEmailNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        EmailUsuarioRequest emailUsuarioRequest = getEmailUsuarioRequest();
        emailUsuarioRequest.setCorreoElectronico(null);
        securityBusinessLogic.resetPassword(emailUsuarioRequest);
    }

    @Test
    public void methodResetPasswordWithEmptyEmailShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        EmailUsuarioRequest resetPasswordRequest = getEmailUsuarioRequest();
        resetPasswordRequest.setCorreoElectronico(Constants.EMPTY_STRING);
        securityBusinessLogic.resetPassword(resetPasswordRequest);
    }

    @Test
    public void methodResetPasswordWithCorrectParametersShouldCallMethodResetPasswordInRepository() throws RepositoryError {
        EmailUsuarioRequest resetPasswordRequest = getEmailUsuarioRequest();
        securityBusinessLogic.resetPassword(resetPasswordRequest);
        verify(securityRepository).resetPassword(resetPasswordRequest);
    }
    /**
     * End ResetPassword.
     */

    /**
     * Start ChangePassword.
     *
     * @throws RepositoryError
     */
    @Test
    public void methodChangePasswordWithUsuarioResponseNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        securityBusinessLogic.changePassword(null);
    }

    @Test
    public void methodChangePasswordWithPaswordCurrentNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setContrasenia(null);
        securityBusinessLogic.changePassword(usuarioRequest);
    }

    @Test
    public void methodChangePasswordWithPaswordNewNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setContraseniaNueva(null);
        securityBusinessLogic.changePassword(usuarioRequest);
    }

    @Test
    public void methodWithCorrectParametersShouldCallMethodChangePasswordInRepository() throws RepositoryError {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        securityBusinessLogic.changePassword(usuarioRequest);
        verify(securityRepository).changePassword(usuarioRequest);
    }

    @Test
    public void methodChangePasswordShouldReturnAnMessageWhenCallChangePasswordInRepository() throws RepositoryError {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse();
        Mensaje mensaje = new Mensaje();
        changePasswordResponse.setMensaje(mensaje);
        when(securityRepository.changePassword(usuarioRequest)).thenReturn(changePasswordResponse);
        Mensaje result = securityBusinessLogic.changePassword(usuarioRequest).getMensaje();
        Assert.assertEquals(mensaje, result);
    }

    /**
     * End ChangePassword.
     */


    /**
     * Start LogOut.
     * @throws RepositoryError
     */
    @Test
    public void methodLogOutWithCorrectShouldCallMethodLogOutInRepository() throws RepositoryError {
        String idOneSignal = "1212-131334-3435";
        EmailUsuarioRequest emailUsuarioRequest = getEmailUsuarioRequest();
        securityBusinessLogic.logOut(emailUsuarioRequest, idOneSignal);
        verify(securityRepository).logOut(emailUsuarioRequest, idOneSignal);
    }

    @Test
    public void methodLogOutWithUsuarioResponseNullShouldReturnAnException() throws RepositoryError {
        String idOneSignal = "1212-131334-3435";
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        securityBusinessLogic.logOut(null, idOneSignal);
    }

    @Test
    public void methodLogOutShouldReturnAnMessageWhenCallLogOutInRepository() throws RepositoryError {
        String idOneSignal = "1212-131334-3435";
        EmailUsuarioRequest emailUsuarioRequest = getEmailUsuarioRequest();
        Mensaje mensaje = new Mensaje();
        when(securityRepository.logOut(emailUsuarioRequest, idOneSignal)).thenReturn(mensaje);
        Mensaje result = securityBusinessLogic.logOut(emailUsuarioRequest, idOneSignal);
        Assert.assertEquals(mensaje, result);
    }
    /**
     * End LogOut.
     */
}
