package com.epm.app.alertasItuango;


import android.arch.core.executor.testing.InstantTaskExecutorRule;
import android.arch.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPush;
import com.epm.app.mvvm.comunidad.network.response.places.ListSubscriptions;
import com.epm.app.mvvm.comunidad.network.response.places.Municipio;
import com.epm.app.mvvm.comunidad.network.response.places.ObtenerMunicipios;
import com.epm.app.mvvm.comunidad.repository.PlacesRepository;
import com.epm.app.mvvm.comunidad.repository.RepositoryShared;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import com.epm.app.mvvm.comunidad.viewModel.SplashViewModel;
import com.epm.app.mvvm.comunidad.views.activities.SplashActivity;

import static org.junit.Assert.*;

import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnit;
import org.mockito.junit.MockitoRule;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;


public class SplashViewModelTest {

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();
    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @InjectMocks
    SplashActivity splashActivity;

    @InjectMocks
    MutableLiveData<ObtenerMunicipios> municipiosMutableLiveData;
    @InjectMocks
    ObtenerMunicipios obtenerMunicipios;
    @InjectMocks
    Municipio municipio;

    private List<Municipio> municipioList;
    private SplashViewModel splashViewModel;
    @Mock
    RepositoryShared repositoryShared;
    @Mock
    SubscriptionRepository subscriptionRepository;
    @Mock
    CustomSharedPreferences customSharedPreferences;
    @Mock
    PlacesRepository placesRepository;

    private MutableLiveData<GetSubscriptionNotificationsPush> subscriptionNotificationsMutableLiveData;
    private GetSubscriptionNotificationsPush subscriptionNotifications;

    private Usuario usuario;
    public ErrorMessage errorMessage;

    @Before
    public void setUp() throws Exception {
        errorMessage = new ErrorMessage(0,0);
        subscriptionRepository = mock(SubscriptionRepository.class);
        repositoryShared = mock(RepositoryShared.class);
        customSharedPreferences = mock(CustomSharedPreferences.class);
        placesRepository = mock(PlacesRepository.class);
        splashViewModel = Mockito.spy(new SplashViewModel(repositoryShared, customSharedPreferences, subscriptionRepository, placesRepository));
        usuario = new Usuario();
        municipioList = new ArrayList<>();
        municipiosMutableLiveData = new MutableLiveData<>();
        subscriptionNotifications = new GetSubscriptionNotificationsPush();
        subscriptionNotificationsMutableLiveData = new MutableLiveData<>();
    }

    @Test
    public void testIfSplashViewModelDelays3Seconds() {
        falseRepositories();
        splashViewModel.loadViewWithSplash();
        verify(splashViewModel).loadViewWithSplash();
    }

    @Test
    public void testValidateViewTutorialisFalse() {
        when(repositoryShared.getValidate(null)).thenReturn(false);
        assertFalse(splashViewModel.validateViewTutorial(null));
    }

    @Test
    public void testValidateViewTutorialisTrue() {
        when(repositoryShared.getValidate(null)).thenReturn(true);
        assertTrue(splashViewModel.validateViewTutorial(null));
    }

    @Test
    public void testValidateSusbcriptionisNull() {
        when(customSharedPreferences.getString(Constants.SUSCRIPTION_ALERTAS)).thenReturn(null);
        falseRepositories();
        splashViewModel.loadViewWithSplash();
        verify(splashViewModel).getSubscriptions();
    }


    @Test
    public void testValidateSusbcriptionisTrue() {
        ListSubscriptions listSubscriptions = new ListSubscriptions();
        List<ListSubscriptions> listSubscriptions1 = new ArrayList<>();
        listSubscriptions1.add(listSubscriptions);
        listSubscriptions.setIdTipoSuscripcionNotificacion(1);
        subscriptionNotifications.setSuscripciones(listSubscriptions1);
        when(customSharedPreferences.getString(Constants.SUSCRIPTION_ALERTAS)).thenReturn(Constants.TRUE);
        loadMunicipality();
        falseRepositories();
        splashViewModel.loadViewWithSplash();
        assertSame(splashViewModel.getSuccess().getValue(), Constants.TRUE);
    }


    @Test
    public void testValidateSusbcriptionisFalse() {
        when(customSharedPreferences.getString(Constants.SUSCRIPTION_ALERTAS)).thenReturn(Constants.FALSE);
        loadMunicipality();
        falseRepositories();
        splashViewModel.loadViewWithSplash();
        assertSame(splashViewModel.getSuccess().getValue(), Constants.FALSE);
    }

    @Test
    public void testGetSubscriptionsisNull() {
        falseRepositories();
        loadMunicipality();
        splashViewModel.getSubscriptions();
        assertSame(splashViewModel.getSuccess().getValue(), Constants.FALSE);
    }

    @Test
    public void testGetSubscriptionsIsNotNull() {
        ListSubscriptions listSubscriptions = new ListSubscriptions();
        List<ListSubscriptions> listSubscriptions1 = new ArrayList<>();
        listSubscriptions1.add(listSubscriptions);
        listSubscriptions.setIdTipoSuscripcionNotificacion(1);
        subscriptionNotifications.setSuscripciones(listSubscriptions1);
        falseRepositories();
        loadMunicipality();
        splashViewModel.getSubscriptions();
        assertSame(splashViewModel.getSuccess().getValue(), Constants.TRUE);
    }

    @Test
    public void testLoadMunicipioisEmpty() {
        falseRepositories();
        splashViewModel.loadMunicipio();
        assertNull(splashViewModel.getListMunicipio());
    }

    @Test
    public void testLoadMunicipioisNotEmpty() {
        falseRepositories();
        loadMunicipality();
        splashViewModel.loadMunicipio();
        assertSame(splashViewModel.getListMunicipio(), municipioList);
    }

    @Test
    public void testShowError() {
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        MutableLiveData<ErrorMessage> compareError = new MutableLiveData<>();
        errorMessage.setMessage(R.string.text_empty_information);
        compareError.setValue(errorMessage);
        when(subscriptionRepository.showError()).thenReturn(compareError);
        splashViewModel.showError(errorMessage);
        assertEquals( splashViewModel.getError().getValue().getMessage(), errorMessage.getMessage());
    }

    @Test
    public void testValidateShowErrorSubscriptionCode401() {
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.title_error);
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        splashViewModel.validateShowErrorSubscription(errorMessage);
        assertEquals(true, splashViewModel.getExpiredToken().getValue());
    }

    @Test
    public void testValidateCaptureErrorSubscription() {
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        MutableLiveData<ErrorMessage> error = new MutableLiveData<>();
        errorMessage.setMessage(R.string.text_empty_information);
        error.setValue(errorMessage);
        when(placesRepository.showError()).thenReturn(error);
        when(subscriptionRepository.showError()).thenReturn(error);
        splashViewModel.captureError();
        verify(splashViewModel).validateShowErrorSubscription(errorMessage);
    }

    @Test
    public void testValidateShowErrorPlacesError401() {
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        splashViewModel.validateShowErrorPlaces(errorMessage);
        assertEquals(splashViewModel.getErrorUnauhthorized(),(int) errorMessage.getMessage());
    }

    @Test
    public void testValidateShowErrorPlacesError() {
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        splashViewModel.validateShowErrorPlaces(errorMessage);
        assertEquals( splashViewModel.getError().getValue().getMessage(), errorMessage.getMessage());
    }

    @Test
    public void testValidateShowErrorSubscriptionError401() {
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        splashViewModel.validateShowErrorSubscription(errorMessage);
        assertEquals(splashViewModel.getErrorUnauhthorized(),(int) errorMessage.getMessage());
    }

    @Test
    public void testValidateShowErrorSubscriptionError404() {
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        falseRepositories();
        loadMunicipality();
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        splashViewModel.validateShowErrorSubscription(errorMessage);
        verify(splashViewModel).loadMunicipio();
    }

    @Test
    public void testValidateShowErrorSubscriptionError() {
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        ValidateServiceCode.captureServiceErrorCode(Constants.FORBIDEN_ERROR_CODE);
        splashViewModel.validateShowErrorSubscription(errorMessage);
        assertEquals(splashViewModel.getError().getValue().getMessage(), errorMessage.getMessage());
    }

    private void falseRepositories() {
        String token = "token";
        String correo = "correo";
        String oneSignal = "oneSignal";
        usuario.setCorreoElectronico(correo);
        splashViewModel.loadUser(usuario);
        subscriptionNotificationsMutableLiveData.setValue(subscriptionNotifications);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(customSharedPreferences.getString(Constants.ONE_SIGNAL_ID)).thenReturn(oneSignal);
        when(subscriptionRepository.getListSubscriptionAlertas(customSharedPreferences.getString(Constants.TOKEN), IdDispositive.getIdDispositive(),
                Constants.TYPE_SUSCRIPTION_ALERTAS,customSharedPreferences.getString(Constants.ONE_SIGNAL_ID))).thenReturn(subscriptionNotificationsMutableLiveData);
        when(placesRepository.getMunicipios(customSharedPreferences.getString(Constants.TOKEN))).thenReturn(municipiosMutableLiveData);
    }

    private void loadMunicipality() {
        municipio.setDescripcion("municipio");
        municipioList.add(municipio);
        obtenerMunicipios.setMunicipios(municipioList);
        municipiosMutableLiveData.setValue(obtenerMunicipios);
    }


}
