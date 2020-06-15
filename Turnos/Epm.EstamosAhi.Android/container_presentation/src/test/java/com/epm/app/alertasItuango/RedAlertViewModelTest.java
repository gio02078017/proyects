package com.epm.app.alertasItuango;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.network.response.subscriptions.DetailRedAlert;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetDetailRedAlertResponse;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import com.epm.app.mvvm.comunidad.viewModel.RedAlertViewModel;

import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnit;
import org.mockito.junit.MockitoRule;
import static org.junit.Assert.*;


import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

public class RedAlertViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @Mock
    public SubscriptionRepository subscriptionRepository;

    @Mock
    public CustomSharedPreferences customSharedPreferences;

    MutableLiveData<GetDetailRedAlertResponse> getDetailRedAlert;

    @InjectMocks
    GetDetailRedAlertResponse getDetailRedAlertResponse;

    @InjectMocks
    DetailRedAlert detailRedAlert;

    private RedAlertViewModel redAlertViewModel;


    @Before
    public void setUp() throws Exception {
        getDetailRedAlert = new MutableLiveData<>();
        subscriptionRepository = mock(SubscriptionRepository.class);
        customSharedPreferences = mock(CustomSharedPreferences.class);
        redAlertViewModel = Mockito.spy(new RedAlertViewModel(customSharedPreferences, subscriptionRepository));
    }

    @Test
    public void testLoadLocationIsNull() {
        falseRepository();
        redAlertViewModel.validateSubscription();
        assertNull(redAlertViewModel.getGetDetailRedAlertResponse().getValue());

    }

    @Test
    public void testLoadLocationIsSuccessful() {
        loadDates();
        falseRepository();
        redAlertViewModel.validateSubscription();
        assertEquals(redAlertViewModel.getGetDetailRedAlertResponse().getValue(),getDetailRedAlertResponse);
    }

    private void falseRepository() {
        String token = "token";
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(customSharedPreferences.getString(Constants.SUSCRIPTION_ALERTAS)).thenReturn(Constants.TRUE);
        when(subscriptionRepository.getDetailRedAlert(IdDispositive.getIdDispositive(),customSharedPreferences.getString(Constants.TOKEN))).thenReturn(getDetailRedAlert);
    }

    private void loadDates() {
        detailRedAlert.setLatitud("1512");
        detailRedAlert.setLongitud("2112");
        detailRedAlert.setNombre("nombre");
        detailRedAlert.setReferenciaDescripcion("descripcion");
        getDetailRedAlertResponse.setDetalleNotificacionPush(detailRedAlert);
        getDetailRedAlert.setValue(getDetailRedAlertResponse);
    }


}
