package com.epm.app.turns;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import androidx.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.mvvm.turn.network.request.CancelTurnParameters;
import com.epm.app.mvvm.turn.network.response.Mensaje;
import com.epm.app.mvvm.turn.network.response.cancelTurn.CancelTurnResponse;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.mvvm.turn.viewModel.ShiftInformationViewModel;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IShiftInformationViewModel;

import org.junit.After;
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

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;
import io.reactivex.android.plugins.RxAndroidPlugins;
import io.reactivex.schedulers.Schedulers;

import static junit.framework.TestCase.assertTrue;
import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotNull;
import static org.junit.Assert.assertNull;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class ShiftInformationViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @ClassRule
    public static final RxImmediateSchedulerRule schedulers = new RxImmediateSchedulerRule();

    @Mock
    TurnServicesRepository turnServicesRepository;

    @Mock
    CustomSharedPreferences customSharedPreferences;

    @Mock
    IShiftInformationViewModel iShiftInformationViewModel;

    @InjectMocks
    Mensaje mensaje;

    @InjectMocks
    CancelTurnParameters cancelTurnParameters;

    @InjectMocks
    CancelTurnResponse cancelTurnResponse;

    @Mock
    ValidateInternet validateInternet;

    public ShiftInformationViewModel shiftInformationViewModel;
    public ErrorMessage errorMessage;

    @Before
    public void setUp() throws Exception {
        errorMessage = new ErrorMessage(0,0);
        turnServicesRepository = mock(TurnServicesRepository.class);
        validateInternet = mock(ValidateInternet.class);
        customSharedPreferences = mock(CustomSharedPreferences.class);
        MockitoAnnotations.initMocks(this);
        RxAndroidPlugins.setInitMainThreadSchedulerHandler(__
                -> Schedulers.trampoline());
        shiftInformationViewModel = Mockito.spy(new ShiftInformationViewModel(turnServicesRepository, customSharedPreferences,validateInternet));
    }

    @After
    public void tearDown() throws Exception {
    }

    @Test
    public void testCancelTurnNotNull() {
        loadData();
        loadResponseTrue();
        String token = "token";
        shiftInformationViewModel.setCancelTurnParameters(cancelTurnParameters);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(turnServicesRepository.cancelTurnResponse(customSharedPreferences.getString(Constants.TOKEN), cancelTurnParameters )).thenReturn(Observable.just(cancelTurnResponse));
        when(validateInternet.isConnected()).thenReturn(true);
        shiftInformationViewModel.cancelTurn(1);
        assertNotNull(shiftInformationViewModel.getCancelTurnResponse());
    }

    @Test
    public void testCancelTurnNull() {
        loadData();
        String token = "token";
        shiftInformationViewModel.setCancelTurnParameters(cancelTurnParameters);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(turnServicesRepository.cancelTurnResponse(customSharedPreferences.getString(Constants.TOKEN), cancelTurnParameters )).thenReturn(null);
        shiftInformationViewModel.cancelTurn(1);
        assertNull(shiftInformationViewModel.getCancelTurnResponse());
    }

    @Test
    public void testCancelTurnTrue() {
        loadData();
        loadResponseTrue();
        String token = "token";
        shiftInformationViewModel.setCancelTurnParameters(cancelTurnParameters);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(turnServicesRepository.cancelTurnResponse(customSharedPreferences.getString(Constants.TOKEN), cancelTurnParameters )).thenReturn(Observable.just(cancelTurnResponse));
        when(validateInternet.isConnected()).thenReturn(true);
        shiftInformationViewModel.cancelTurn(1);
        assertNotNull(shiftInformationViewModel.getCancelTurnResponse());
        assertEquals(shiftInformationViewModel.getCancelTurnResponse().getEstadoTransaccion(), true);
    }

    @Test
    public void testCancelTurnFalse() {
        loadData();
        loadResponseAnotherId();
        String token = "token";
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(turnServicesRepository.cancelTurnResponse(customSharedPreferences.getString(Constants.TOKEN), cancelTurnParameters )).thenReturn(Observable.just(cancelTurnResponse));
        shiftInformationViewModel.cancelTurn(1);
        assertNotNull(shiftInformationViewModel.getCancelTurnResponse());
        assertEquals(shiftInformationViewModel.getCancelTurnResponse().getEstadoTransaccion(), false);
    }

    @Test
    public void testIdentificateWrongCauseId8() {
        loadResponseId8();
        shiftInformationViewModel.identificateWrongCause();
        assertTrue(shiftInformationViewModel.getFailedWithActionCancelTurn().getValue());
    }

    @Test
    public void testIdentificateWrongCauseAnotherId() {
        loadResponseAnotherId();
        shiftInformationViewModel.identificateWrongCause();
        assertTrue(shiftInformationViewModel.getFailedCancelTurn().getValue());
    }

    @Test
    public void testValidateShowError(){
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        shiftInformationViewModel.validateShowError(errorMessage);
        assertEquals( shiftInformationViewModel.getError().getValue().getMessage(), errorMessage.getMessage());
    }

    @Test
    public void testValidateShow401(){
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        shiftInformationViewModel.validateShowError(errorMessage);
        assertEquals(shiftInformationViewModel.getErrorUnauthorized(),(int) errorMessage.getMessage());
    }

    private void loadData(){
        cancelTurnParameters.setSistemaOperativo("");
        cancelTurnParameters.setIdDispositivo("");
        cancelTurnParameters.setIdTurno(1);

    }

    private void loadResponseTrue(){
        cancelTurnResponse.setEstadoTransaccion(true);
    }

    private void loadResponseFalse(){
        cancelTurnResponse.setEstadoTransaccion(false);
    }

    private void loadResponseId8(){
        cancelTurnResponse.setEstadoTransaccion(false);
        mensaje.setIdentificador(8);
        cancelTurnResponse.setMensaje(mensaje);
        shiftInformationViewModel.setCancelTurnResponse(cancelTurnResponse);
    }

    private void loadResponseAnotherId(){
        cancelTurnResponse.setEstadoTransaccion(false);
        mensaje.setIdentificador(11);
        cancelTurnResponse.setMensaje(mensaje);
        shiftInformationViewModel.setCancelTurnResponse(cancelTurnResponse);
    }
}