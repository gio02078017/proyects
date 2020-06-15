package com.epm.app.notifications;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import androidx.lifecycle.MutableLiveData;
import android.view.View;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationList;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.ReceivePushNotification;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetNotificationsPush;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.epm.app.mvvm.comunidad.viewModel.NotificationCenterViewModel;
import com.epm.app.turns.RxImmediateSchedulerRule;

import org.junit.Before;
import org.junit.ClassRule;
import org.junit.Rule;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.MockitoAnnotations;
import org.mockito.junit.MockitoJUnit;
import org.mockito.junit.MockitoRule;

import java.util.ArrayList;
import java.util.List;


import static org.junit.Assert.*;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;


import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;
import io.reactivex.android.plugins.RxAndroidPlugins;
import io.reactivex.schedulers.Schedulers;


public class NotificationCenterViewModelTest {


    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();


    @ClassRule
    public static final RxImmediateSchedulerRule schedulers = new RxImmediateSchedulerRule();

    @Mock
    public CustomSharedPreferences customSharedPreferences;

    @Mock
    public NotificationsRepository notificationsRepository;

    @Mock
    ValidateInternet validateInternet;

    private int quantityNotifications= 4;
    public ErrorMessage errorMessage;

    private GetNotificationsPush getNotificationsPush;
    private MutableLiveData<GetNotificationsPush> getNotificationsPushMutableLiveData;
    private NotificationResponse notificationResponse;
    private MutableLiveData<NotificationResponse> notificationResponseMutableLiveData;
    private List<ReceivePushNotification> listNotification;
    private MutableLiveData<NotificationList> notificationListMutableLiveData;
    private ReceivePushNotification receivePushNotification;
    private MutableLiveData<ErrorMessage> error = new MutableLiveData<>();
    private MutableLiveData<Integer> titleError = new MutableLiveData<>();
    @InjectMocks
    private NotificationList notificationList;
    private NotificationCenterViewModel notificationCenterViewModel;
    private int idNotification = 1;


    @Before
    public void setUp() throws Exception {
        errorMessage = new ErrorMessage(0,0);
        notificationListMutableLiveData = new MutableLiveData<>();
        receivePushNotification = new ReceivePushNotification();
        listNotification = new ArrayList<>();
        notificationResponse = new NotificationResponse();
        notificationResponseMutableLiveData = new MutableLiveData<>();
        getNotificationsPushMutableLiveData = new MutableLiveData<>();
        getNotificationsPush = new GetNotificationsPush();
        notificationsRepository = mock(NotificationsRepository.class);
        customSharedPreferences = mock(CustomSharedPreferences.class);
        MockitoAnnotations.initMocks(this);
        RxAndroidPlugins.setInitMainThreadSchedulerHandler(__
                -> Schedulers.trampoline());
        notificationCenterViewModel = Mockito.spy(new NotificationCenterViewModel(notificationsRepository,customSharedPreferences,validateInternet));
    }

    @Test
    public void testGetNotificationPushIsSuccessful(){
        getNotificationsPush.setCantidadNotificacionesSinLeer(quantityNotifications);
        getNotificationsPushMutableLiveData.setValue(getNotificationsPush);
        loadNotification();
        fakeRepositories();
        notificationCenterViewModel.getNotificationsPush();
        verify(customSharedPreferences).addInt(Constants.NUMBER_NOTIFICATIONS, getNotificationsPush.getCantidadNotificacionesSinLeer());
    }

    @Test
    public void testGetNotificationPushIsNotSuccessful(){
        getNotificationsPushMutableLiveData.setValue(getNotificationsPush);
        loadNotification();
        fakeRepositories();
        notificationCenterViewModel.getNotificationsPush();
        verify(customSharedPreferences,times(0)).addInt(Constants.NUMBER_NOTIFICATIONS, getNotificationsPush.getCantidadNotificacionesSinLeer());
    }

    @Test
    public void testDeleteNotificationPushIsSuccessful(){
        int idNotification = 1;
        loadNotification();
        notificationResponse.setEstadoTransaccion(true);
        notificationResponseMutableLiveData.setValue(notificationResponse);
        fakeRepositories();
        notificationCenterViewModel.loadNotifications();
        notificationCenterViewModel.deleteNotificationsPush(idNotification);
        assertEquals(false, notificationCenterViewModel.getProgress().getValue());
        verify(notificationCenterViewModel).validateListNotificationIsEmpty();;
    }

    @Test
    public void testDeleteNotificationPushIsNotSuccessful(){
        int idNotification = 1;
        loadNotification();
        fakeRepositories();
        notificationResponse.setEstadoTransaccion(false);
        notificationCenterViewModel.loadNotifications();
        notificationCenterViewModel.deleteNotificationsPush(idNotification);
        assertEquals(true,notificationCenterViewModel.getProgress().getValue());
        //verify(notificationCenterViewModel,times(0)).validateThereAreNotifications();
        verify(notificationCenterViewModel,times(0)).validateListNotificationIsEmpty();
    }



    @Test
    public void testLoadNotificationFound(){
        loadNotification();
        fakeRepositories();
        notificationCenterViewModel.loadNotifications();
        verify(notificationCenterViewModel).loadListNotification(notificationList);
    }

    @Test
    public void testValidateRecordsActualPageIsEqualToTheTotalPage(){
        int actualPage = 1;
        int totalPage = 1;
        notificationCenterViewModel.validateRecords(actualPage,totalPage);
        assertTrue(notificationCenterViewModel.isRecordsNotFound());
    }

    @Test
    public void testValidateRecordsActualPageIsNotEqualToTheTotalPage(){
        int actualPage = 1;
        int totalPage = 2;
        notificationCenterViewModel.validateRecords(actualPage,totalPage);
        assertFalse(notificationCenterViewModel.isRecordsNotFound());
    }

    @Test
    public void testUpdateNotificationIsSuccessful(){
        loadNotification();
        notificationResponse.setEstadoTransaccion(true);
        notificationResponseMutableLiveData.setValue(notificationResponse);
        fakeRepositories();
        notificationCenterViewModel.updateNotification(idNotification);
        verify(notificationCenterViewModel).getNotificationsPush();
    }

    @Test
    public void testUpdateNotificationIsNoTSuccessful(){
        loadNotification();
        notificationResponse.setEstadoTransaccion(false);
        notificationResponseMutableLiveData.setValue(notificationResponse);
        fakeRepositories();
        notificationCenterViewModel.updateNotification(idNotification);
        verify(notificationCenterViewModel,times(0)).getNotificationsPush();

    }
    
    @Test
    public void testValidateErrorCodeService401(){
        int titleError= R.string.title_error;
        Integer error= R.string.text_unauthorized;
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        notificationCenterViewModel.validateError(new ErrorMessage(titleError,error));
        assertEquals(error,notificationCenterViewModel.getExpiredToken().getValue().getMessage());
    }

    @Test
    public void testValidateErrorCodeService404(){
        int titleError= R.string.title_error;
        Integer error= R.string.text_unauthorized;
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        notificationCenterViewModel.validateError(new ErrorMessage(titleError,error));
        assertEquals((int) notificationCenterViewModel.notFound.getValue(), View.VISIBLE);
    }

    @Test
    public void testValidateErrorCodeServiceOtherError(){
        ValidateServiceCode.captureServiceErrorCode(Constants.DEFAUL_ERROR_CODE);
        notificationCenterViewModel.validateError(new ErrorMessage(0,0));
        assertNotEquals(errorMessage, notificationCenterViewModel.getError().getValue());
    }


    private void fakeRepositories(){
        int counter =1;
        String token = "token";
        when(validateInternet.isConnected()).thenReturn(true);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(notificationsRepository.getListNotificationsPush(IdDispositive.getIdDispositive(),customSharedPreferences.getString(Constants.TOKEN), Constants.ONE_THOUSAND, counter))
                .thenReturn(Observable.just(notificationList));
        when(notificationsRepository.getNotificationsPush(IdDispositive.getIdDispositive()))
                .thenReturn(Observable.just(getNotificationsPush));
        when(notificationsRepository.deleteNotificationsPush(listNotification.get(idNotification).getIdNotificationPush(),customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(notificationResponse));
        when(notificationsRepository.updateNotificationPush(listNotification.get(idNotification).getIdNotificationPush(),customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(notificationResponse));
    }


    private void loadNotification(){
        int actualPage= 1;
        int totalPage = 1;
        int totalRecords = 5;
        for(int i=0;i<4;i++) {
            receivePushNotification.setRead(true);
            receivePushNotification.setDateTimeReceived("date");
            receivePushNotification.setIdNotificationPush(i);
            receivePushNotification.setDateTimeReceived("fecha");
            listNotification.add(receivePushNotification);
        }
        notificationList.setActualPage(actualPage);
        notificationList.setTotalPages(totalPage);
        notificationList.setTotalRecords(totalRecords);
        notificationList.setReceivePushNotification(listNotification);
        notificationListMutableLiveData.setValue(notificationList);
        notificationCenterViewModel.setListNotification(listNotification);
    }


}
