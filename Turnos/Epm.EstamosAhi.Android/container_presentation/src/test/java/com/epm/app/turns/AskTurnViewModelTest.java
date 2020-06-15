package com.epm.app.turns;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;

import com.epm.app.R;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
import com.epm.app.mvvm.turn.network.request.AskTurnParameters;
import com.epm.app.mvvm.turn.network.response.Mensaje;
import com.epm.app.mvvm.turn.network.response.askTurn.TurnResponse;
import com.epm.app.mvvm.turn.network.response.officeDetail.Oficina;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.mvvm.turn.viewModel.AskTurnViewModel;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IAskTurnViewModel;

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
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;
import io.reactivex.android.plugins.RxAndroidPlugins;
import io.reactivex.schedulers.Schedulers;

import static org.junit.Assert.*;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

public class AskTurnViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @ClassRule
    public static final RxImmediateSchedulerRule schedulers = new RxImmediateSchedulerRule();

    @Mock
    IAskTurnViewModel iAskTurnViewModel;

    @Mock
    TurnServicesRepository turnServicesRepository;

    @InjectMocks
    AskTurnParameters askTurnParameters;

    @InjectMocks
    TurnResponse turnResponse;

    @InjectMocks
    ProcedureInformation procedureInformation;

    @InjectMocks
    Usuario user;

    @Mock
    Oficina oficina;

    @InjectMocks
    Mensaje mensaje;

    @Mock
    ValidateInternet validateInternet;

    @Mock
    CustomSharedPreferences customSharedPreferences;

    private AskTurnViewModel askTurnViewModel;
    public ErrorMessage errorMessage;

    @Before
    public void setUp() throws Exception {
        turnServicesRepository = mock(TurnServicesRepository.class);
        validateInternet = mock(ValidateInternet.class);
        oficina = mock(Oficina.class);
        errorMessage = new ErrorMessage(0,0);
        //mensaje = mock(ProcedureServiceMessage.class);
        customSharedPreferences = mock(CustomSharedPreferences.class);
        MockitoAnnotations.initMocks(this);
        RxAndroidPlugins.setInitMainThreadSchedulerHandler(__
                -> Schedulers.trampoline());
        askTurnViewModel = Mockito.spy(new AskTurnViewModel(turnServicesRepository, customSharedPreferences,validateInternet));
    }

    @After
    public void tearDown() throws Exception {

    }

    @Test
    public void testAskTurnNull() {
        loadData();
        String token = "token";
        String nombre = "";
        String nombres = "";
        askTurnViewModel.setAskTurnParameters(askTurnParameters);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(turnServicesRepository.askTurnResponse(customSharedPreferences.getString(Constants.TOKEN), askTurnParameters )).thenReturn(Observable.just(turnResponse));
        askTurnViewModel.askTurn(nombre, oficina, user, procedureInformation);
        askTurnViewModel.setTurnResponse(null);
        assertNull(askTurnViewModel.getTurnResponse());
    }

    @Test
    public void testAskTurnProcedureNull() {
        loadData();
        loadResponseTrue();
        String token = "token";
        String nombre = "";
        askTurnViewModel.setAskTurnParameters(askTurnParameters);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(turnServicesRepository.askTurnResponse(customSharedPreferences.getString(Constants.TOKEN), askTurnParameters )).thenReturn(Observable.just(turnResponse));
        when(validateInternet.isConnected()).thenReturn(true);
        askTurnViewModel.askTurn(nombre, oficina, user, procedureInformation);
        assertNotNull(askTurnViewModel.getTurnResponse());
    }

    @Test
    public void testAskTurnProcedureNotNull() {
        loadDataProcedureNotNull();
        loadResponseTrue();
        String token = "token";
        String nombre = "";
        askTurnViewModel.setAskTurnParameters(askTurnParameters);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(turnServicesRepository.askTurnResponse(customSharedPreferences.getString(Constants.TOKEN), askTurnParameters )).thenReturn(Observable.just(turnResponse));
        when(validateInternet.isConnected()).thenReturn(true);
        askTurnViewModel.askTurn(nombre, oficina, user, procedureInformation);
        assertNotNull(askTurnViewModel.getTurnResponse());
    }

    @Test
    public void testValidateShowErrorGuideProceduresAndRequirementsCategoryError(){
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        askTurnViewModel.validateShowError(errorMessage);
        assertEquals( askTurnViewModel.getError().getValue().getMessage(), errorMessage.getMessage());
    }

    @Test
    public void testValidateShowErrorGuideProceduresAndRequirementsCategoryError401(){
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        askTurnViewModel.validateShowError(errorMessage);
        assertEquals(askTurnViewModel.getErrorUnauthorized(),(int) errorMessage.getMessage());
    }

    @Test
    public void testValidateUserIsGuest(){
        String name = "nombre";
        assertEquals(askTurnViewModel.validateUserGuest(name,true),Constants.ID_USER);
    }

    @Test
    public void testValidateUserIsNotGuest(){
        String name = "nombre";
        assertEquals(askTurnViewModel.validateUserGuest(name,false),name);
    }

    @Test
    public void testValidateResponseTurnIsNull(){
        turnResponse = null;
        askTurnViewModel.validateResponseTurn(turnResponse);
        verify(askTurnViewModel,times(0)).validateMessageResponse();
    }

    @Test
    public void testValidateResponseTurnIsNotNull(){
        turnResponse = new TurnResponse();
        askTurnViewModel.validateResponseTurn(turnResponse);
        verify(askTurnViewModel).validateMessageResponse();
    }

    @Test
    public void testValidateMessageResponseMessageIsNull(){
        mensaje = null;
        turnResponse.setMensaje(mensaje);
        askTurnViewModel.setTurnResponse(turnResponse);
        askTurnViewModel.validateMessageResponse();
        assertTrue(askTurnViewModel.getProblemsAskTurn().getValue());
    }

    @Test
    public void testValidateMessageResponseMessageIsNotNullAndIdentifierOne(){
        mensaje.setContenido("mensaje");
        mensaje.setIdentificador(Constants.ONE);
        turnResponse.setMensaje(mensaje);
        askTurnViewModel.setTurnResponse(turnResponse);
        askTurnViewModel.validateMessageResponse();
        assertTrue(askTurnViewModel.getSuccessAskTurn().getValue());
    }



    private void loadData(){
        askTurnParameters.setIdDispositivo(IdDispositive.getIdDispositive());
        askTurnParameters.setIdOficinaSentry(oficina.getIdOficinaSentry());
        askTurnParameters.setNombreSolicitante("");
        askTurnParameters.setIdTramite(null);
        askTurnParameters.setIdServicio(null);
        askTurnParameters.setDetalleMaestros(null);
        askTurnParameters.setIdOneSignal(customSharedPreferences.getString(Constants.ONE_SIGNAL_ID));
        askTurnParameters.setUsuarioAutenticado("");
        askTurnParameters.setSistemaOperativo(Constants.SYSTEM_OPERATIVE);
    }

    private void loadDataProcedureNotNull(){
        askTurnParameters.setIdDispositivo(IdDispositive.getIdDispositive());
        askTurnParameters.setIdOficinaSentry(oficina.getIdOficinaSentry());
        askTurnParameters.setNombreSolicitante("");
        askTurnParameters.setIdTramite(procedureInformation.getIdProcedure());
        askTurnParameters.setIdServicio(procedureInformation.getIdService());
        askTurnParameters.setDetalleMaestros(procedureInformation.getIdMasterDetails());
        askTurnParameters.setIdOneSignal(customSharedPreferences.getString(Constants.ONE_SIGNAL_ID));
        askTurnParameters.setUsuarioAutenticado("");
        askTurnParameters.setSistemaOperativo(Constants.SYSTEM_OPERATIVE);
    }

    private void loadResponseTrue(){
        mensaje.setIdentificador(1);
        turnResponse.setMensaje(mensaje);
    }
}