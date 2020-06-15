package com.epm.app.notifications;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import androidx.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.network.request.UpdateStatusSendNotificationRequest;
import com.epm.app.mvvm.comunidad.network.response.notifications.UpdateStatusSendNotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.places.ListSubscriptions;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPush;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import com.epm.app.mvvm.comunidad.viewModel.ConfigurationNotificationViewModel;

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
import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;

import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.mockito.internal.verification.VerificationModeFactory.times;

public class ConfigurationNotificationViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    private ConfigurationNotificationViewModel configurationNotificationViewModel;

    private MutableLiveData<GetSubscriptionNotificationsPush> subscriptions;

    private boolean checked;

    private List<ListSubscriptions> listSubscriptions;

    @Mock
    ValidateInternet validateInternet;

    private MutableLiveData<UpdateStatusSendNotificationResponse> updateStatusSendNotificationLiveData;
    @InjectMocks
    private UpdateStatusSendNotificationResponse statusSendNotificationResponse;

    @InjectMocks
    private ListSubscriptions subscription;

    @InjectMocks
    public GetSubscriptionNotificationsPush getSubscriptionNotificationsPush;

    @Mock
    SubscriptionRepository subscriptionRepository;

    @Mock
    CustomSharedPreferences customSharedPreferences;

    @Mock
    NotificationsRepository notificationsRepository;

    @InjectMocks
    UpdateStatusSendNotificationRequest updateStatusSendNotificationRequest;

    @Before
    public void setUp()  {
        listSubscriptions = new ArrayList<>();
        subscriptions = new MutableLiveData<>();
        updateStatusSendNotificationLiveData = new MutableLiveData<>();
        subscriptionRepository = mock(SubscriptionRepository.class);
        customSharedPreferences = mock(CustomSharedPreferences.class);
        notificationsRepository = mock(NotificationsRepository.class);
        configurationNotificationViewModel = Mockito.spy(new ConfigurationNotificationViewModel(subscriptionRepository,
                customSharedPreferences,notificationsRepository,updateStatusSendNotificationRequest,validateInternet));
    }

    @Test
    public void testSubscriptionToDataIsNull(){
        fakeRepositories();
        configurationNotificationViewModel.verifyNotification();
        verify(configurationNotificationViewModel,times(0)).validateListIsEmptyOrNull(listSubscriptions);
    }

    @Test
    public void testSubscriptionToDataIsNotNull(){
        loadData();
        fakeRepositories();
        configurationNotificationViewModel.verifyNotification();
        verify(configurationNotificationViewModel).validateListIsEmptyOrNull(listSubscriptions);
    }

    @Test
    public void testListOfSubscriptionIsEmpty(){
        configurationNotificationViewModel.validateListIsEmptyOrNull(listSubscriptions);
        verify(configurationNotificationViewModel,times(0)).validateSubscription(listSubscriptions);
    }

    @Test
    public void testListOfSubscriptionIsNull(){
        listSubscriptions = null;
        configurationNotificationViewModel.validateListIsEmptyOrNull(listSubscriptions);
        verify(configurationNotificationViewModel,times(0)).validateSubscription(listSubscriptions);
    }

    @Test
    public void testListOfSubscriptionIsNotEmpty(){
        loadData();
        configurationNotificationViewModel.validateListIsEmptyOrNull(listSubscriptions);
        verify(configurationNotificationViewModel).validateSubscription(listSubscriptions);
    }

    @Test
    public void testValidateSubscriptionIsSuccessful(){
        subscription.setIdTipoSuscripcionNotificacion(1);
        subscription.setEnvioNotificacion(true);
        listSubscriptions.add(subscription);
        configurationNotificationViewModel.validateSubscription(listSubscriptions);
        verify(configurationNotificationViewModel).validateTypeNotification(subscription);
    }

    @Test
    public void testValidateSubscriptionIsNotSuccessful(){
        configurationNotificationViewModel.validateSubscription(listSubscriptions);
        verify(configurationNotificationViewModel,times(0)).loadDataToNotificationAlerts(true);
    }

    @Test
    public void testLoadDataNotification(){
        boolean sendNotification = true;
        configurationNotificationViewModel.loadDataToNotificationAlerts(sendNotification);
        assertTrue(configurationNotificationViewModel.getCheckedAlerts().getValue());
        assertTrue(configurationNotificationViewModel.getEnabledAlerts().getValue());
    }

    @Test
    public void testUpdateStatusNotificationResponseIsNull(){
        checked = true;
        fakeRepositories();
        when(validateInternet.isConnected()).thenReturn(true);
        configurationNotificationViewModel.updateStatusNotification(Constants.TYPE_SUSCRIPTION_ALERTAS,checked);
        verify(configurationNotificationViewModel,times(0)).verifyNotification();
    }

    @Test
    public void testUpdateStatusNotificationResponseIsNotNull(){
        checked = true;
        updateStatusSendNotificationLiveData.setValue(statusSendNotificationResponse);
        fakeRepositories();
        when(validateInternet.isConnected()).thenReturn(true);
        configurationNotificationViewModel.updateStatusNotification(Constants.TYPE_SUSCRIPTION_ALERTAS,checked);
        verify(configurationNotificationViewModel).loadDataToUpdate(Constants.TYPE_SUSCRIPTION_ALERTAS,checked);
    }

    @Test
    public void testValidateErrorIs401(){
        int titleError= R.string.title_error;
        Integer error= R.string.text_unauthorized;
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        configurationNotificationViewModel.validateError(new ErrorMessage(titleError,error));
        assertEquals(titleError,configurationNotificationViewModel.getExpiredToken().getValue().getTitle());
        assertEquals(error,configurationNotificationViewModel.getExpiredToken().getValue().getMessage());
    }

    @Test
    public void testValidateErrorIsOtherCode(){
        int titleError= R.string.title_error;
        Integer error= R.string.text_unauthorized;
        configurationNotificationViewModel.getCheckedAlerts().setValue(true);
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        configurationNotificationViewModel.validateError(new ErrorMessage(titleError,error));
        assertEquals(titleError,configurationNotificationViewModel.getIsError().getValue().getTitle());
        assertEquals(error,configurationNotificationViewModel.getIsError().getValue().getMessage());
    }

    private void fakeRepositories(){
        String token = "token";
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(subscriptionRepository.getListSubscriptionAlertas(customSharedPreferences.getString(Constants.TOKEN),
                IdDispositive.getIdDispositive(),Constants.TYPE_SUSCRIPTION_ALERTAS,customSharedPreferences.getString(Constants.ONE_SIGNAL_ID))).thenReturn(subscriptions);
        when( notificationsRepository.updateStatusSendNotification(updateStatusSendNotificationRequest,
                customSharedPreferences.getString(Constants.TOKEN))).thenReturn(Observable.just(statusSendNotificationResponse));
    }

    private void loadData(){
        subscriptions.setValue(getSubscriptionNotificationsPush);
        getSubscriptionNotificationsPush.setSuscripciones(listSubscriptions);
        for (int i = 0 ; i < 3; i++){
            subscription.setIdTipoSuscripcionNotificacion(i);
            subscription.setNombreSuscripcionNotificacion("alertas");
            subscription.setAceptaTerminosCondiciones(true);
            subscription.setEnvioNotificacion(true);
            subscription.setCorreoElectronico("correo");
            listSubscriptions.add(subscription);
        }
    }



}
