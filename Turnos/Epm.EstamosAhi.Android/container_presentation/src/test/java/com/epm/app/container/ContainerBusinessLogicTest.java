package com.epm.app.container;


import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.ExpectedException;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.junit.MockitoJUnitRunner;

import app.epm.com.container_domain.business_models.EmailUsuarioRequest;
import app.epm.com.container_domain.container.ContainerBusinessLogic;
import app.epm.com.container_domain.container.IContainerRepository;

import static org.mockito.Mockito.verify;

/**
 * Created by josetabaresramirez on 17/11/16.
 */
@RunWith(MockitoJUnitRunner.class)
public class ContainerBusinessLogicTest {

    @Rule
    public ExpectedException expectedException = ExpectedException.none();

    ContainerBusinessLogic containerBusinessLogic;

    @Mock
    IContainerRepository securityRepository;

    @Before
    public void setUp() {
        containerBusinessLogic = new ContainerBusinessLogic(securityRepository);
    }

    private EmailUsuarioRequest getEmailUsuarioRequest() {
        EmailUsuarioRequest resetPasswordRequest = new EmailUsuarioRequest();
        resetPasswordRequest.setCorreoElectronico("quiceno1127@gmail.com");
        return resetPasswordRequest;
    }

     /**
     * Start lists.
     *
     * @throws RepositoryError
     */
    @Test
    public void methodGetGeneralListWithCorrectParametersShouldCallMethodGetGeneralListInRepository() throws RepositoryError {
        containerBusinessLogic.getGeneralList();
        verify(securityRepository).getGeneralList();
    }

    /**
     * Start guest login.
     *
     * @throws RepositoryError
     */
    @Test
    public void methodGetGuestLoginWithCorrectParametersShouldCallMethodGetGuestLoginInRepository() throws RepositoryError {
        containerBusinessLogic.getGuestLogin();
        verify(securityRepository).getGuestLogin();
    }

    /**
     * Start auto login.
     *
     * @throws RepositoryError
     */
    @Test
    public void methodGetAutoLoginWithCorrectParametersShouldCallMethodGetAutoLoginInRepository() throws RepositoryError {
        String token = "21212";
        containerBusinessLogic.getAutoLogin(token);
        verify(securityRepository).getAutoLogin(token);
    }

    @Test
    public void methodGetEspacioPromocionalWithCorrectParametersShouldCallMethodGetEspacioPromocionalInRepository() throws RepositoryError {
        containerBusinessLogic.getEspacioPromocional();
        verify(securityRepository).getEspacioPromocional();
    }
}