package com.epm.app.turns;

import android.arch.core.executor.testing.InstantTaskExecutorRule;

import com.epm.app.R;
import com.epm.app.mvvm.turn.network.request.OfficeDetailParameters;
import com.epm.app.mvvm.turn.network.response.Mensaje;
import com.epm.app.mvvm.turn.network.response.officeDetail.OfficeDetailResponse;
import com.epm.app.mvvm.turn.network.response.officeDetail.Oficina;
import com.epm.app.mvvm.turn.network.response.officeDetail.Turno;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.mvvm.turn.viewModel.OfficeDetailViewModel;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IOfficeDetailViewModel;
import app.epm.com.utilities.helpers.InformationOffice;

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
import io.reactivex.android.plugins.RxAndroidPlugins;
import io.reactivex.schedulers.Schedulers;
import retrofit2.Response;

import static org.junit.Assert.*;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class OfficeDetailViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @ClassRule
    public static final RxImmediateSchedulerRule schedulers = new RxImmediateSchedulerRule();

    @Mock
    IOfficeDetailViewModel iOfficeDetailViewModel;

    @Mock
    TurnServicesRepository turnServicesRepository;

    @InjectMocks
    OfficeDetailResponse officeDetailResponse;

    @InjectMocks
    InformationOffice informationOffice;

    @InjectMocks
    OfficeDetailParameters officeDetailParameters;

    public OfficeDetailViewModel officeDetailViewModel;

    @Mock
    public CustomSharedPreferences customSharedPreferences;

    @InjectMocks
    Oficina oficina;

    @Mock
    Mensaje mensaje;

    @Mock
    Turno turno;

    @Mock
    ValidateInternet validateInternet;

    @InjectMocks
    Response<OfficeDetailResponse> response;

    public ErrorMessage errorMessage;

    @Before
    public void setUp() throws Exception {
        errorMessage = new ErrorMessage(0,0);
        turnServicesRepository = mock(TurnServicesRepository.class);
        customSharedPreferences = mock(CustomSharedPreferences.class);
        validateInternet = mock(ValidateInternet.class);
        informationOffice = mock(InformationOffice.class);
        mensaje = mock(Mensaje.class);
        turno = mock(Turno.class);
        MockitoAnnotations.initMocks(this);
        RxAndroidPlugins.setInitMainThreadSchedulerHandler(__
                -> Schedulers.trampoline());
        officeDetailViewModel = Mockito.spy(new OfficeDetailViewModel(turnServicesRepository, customSharedPreferences,validateInternet));
    }

    @After
    public void tearDown() throws Exception {
    }

   /* @Test
    public void testGetOfficeDetailNotNull() {
        loadDates();
        loadResponseTrue();
        String token = "token";
        officeDetailViewModel.setOfficeDetailParameters(officeDetailParameters);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(validateInternet.isConnected()).thenReturn(true);
        when(procedureServicesRepository.getOfficeDetailResponse(customSharedPreferences.getString(Constants.TOKEN), officeDetailParameters )).thenReturn(Observable.just(officeDetailResponse));
        officeDetailViewModel.getOfficeDetail(informationOffice,false);
        assertNotNull(officeDetailViewModel.getOfficeDetailResponse());
    }*/

    @Test
    public void testGetOfficeDetailNull() {
        loadDates();
        String token = "token";
        loadResponseNull();
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(turnServicesRepository.getOfficeDetailResponse(customSharedPreferences.getString(Constants.TOKEN), officeDetailParameters )).thenReturn(null);
        officeDetailViewModel.getOfficeDetail(informationOffice,false);
        assertNull(officeDetailViewModel.getOfficeDetailResponse());
    }

   /* @Test
    public void testOfficeClose() {
        loadDates();
        loadResponseOfficeClose();
        String token = "token";
        officeDetailViewModel.setOfficeDetailParameters(officeDetailParameters);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(validateInternet.isConnected()).thenReturn(true);
        when(procedureServicesRepository.getOfficeDetailResponse(customSharedPreferences.getString(Constants.TOKEN), officeDetailParameters )).thenReturn(Observable.just(officeDetailResponse));
        officeDetailViewModel.getOfficeDetail(informationOffice,false);
        assertEquals(officeDetailViewModel.getOfficeDetailResponse().getOficina().getEstadoOficina(), Constants.OFFICE_STATED_CERRADA);
    }*/

  /*  @Test
    public void testAppliedTurn() {
        loadDates();
        loadResponseAppliedTurn();
        String token = "token";
        officeDetailViewModel.setOfficeDetailParameters(officeDetailParameters);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(validateInternet.isConnected()).thenReturn(true);
        when(procedureServicesRepository.getOfficeDetailResponse(customSharedPreferences.getString(Constants.TOKEN), officeDetailParameters )).thenReturn(Observable.just(officeDetailResponse));
        officeDetailViewModel.getOfficeDetail(informationOffice,false);
        assertTrue(officeDetailViewModel.getOfficeDetailResponse().getAplicaSolicitudTurno());
    }*/

   /* @Test
    public void testNotAppliedTurn() {
        loadDates();
        loadResponseTrue();
        String token = "token";
        officeDetailViewModel.setOfficeDetailParameters(officeDetailParameters);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(validateInternet.isConnected()).thenReturn(true);
        when(procedureServicesRepository.getOfficeDetailResponse(customSharedPreferences.getString(Constants.TOKEN), officeDetailParameters )).thenReturn(Observable.just(officeDetailResponse));
        officeDetailViewModel.getOfficeDetail(informationOffice,false);
        assertFalse(officeDetailViewModel.getOfficeDetailResponse().getAplicaSolicitudTurno());
    }*/

    @Test
    public void testValidateShowErrorGuideProceduresAndRequirementsCategoryError(){
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        officeDetailViewModel.validateShowError(errorMessage);
        assertEquals( officeDetailViewModel.getError().getValue().getMessage(), errorMessage.getMessage());
    }

    @Test
    public void testValidateShowErrorGuideProceduresAndRequirementsCategoryError401(){
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        officeDetailViewModel.validateShowError(errorMessage);
        assertEquals(officeDetailViewModel.getErrorUnauthorized(),(int) errorMessage.getMessage());
    }


    private void loadDates(){
        officeDetailParameters.setSistemaOperativo("");
        officeDetailParameters.setIdDispositivo("");
        officeDetailParameters.setIdOficinaSentry("");

    }

    private void loadResponseTrue(){
        officeDetailResponse.setDispositivoTieneTurnoAsignado(false);
        officeDetailResponse.setAplicaSolicitudTurno(false);
        oficina.setEstadoOficina("");
        oficina.setDireccion("");
        oficina.setHorario("");
        oficina.setNombre("");
        oficina.setTiempoPromedio("");
        oficina.setTurnosEnEspera(0);
        officeDetailResponse.setOficina(oficina);
        officeDetailResponse.setMensaje(mensaje);
    }

    private void loadResponseNull(){
        officeDetailResponse = null;
    }

    private void loadResponseOfficeClose(){
        officeDetailResponse.setDispositivoTieneTurnoAsignado(false);
        officeDetailResponse.setAplicaSolicitudTurno(false);
        oficina.setEstadoOficina(Constants.OFFICE_STATED_CERRADA);
        oficina.setDireccion("");
        oficina.setHorario("");
        oficina.setNombre("");
        oficina.setTiempoPromedio("");
        oficina.setTurnosEnEspera(0);
        officeDetailResponse.setOficina(oficina);
        officeDetailResponse.setMensaje(mensaje);
    }

    private void loadResponseAppliedTurn(){
        officeDetailResponse.setDispositivoTieneTurnoAsignado(false);
        officeDetailResponse.setAplicaSolicitudTurno(true);
        oficina.setEstadoOficina("");
        oficina.setDireccion("");
        oficina.setHorario("");
        oficina.setNombre("");
        oficina.setTiempoPromedio("");
        oficina.setTurnosEnEspera(0);
        officeDetailResponse.setOficina(oficina);
        officeDetailResponse.setMensaje(mensaje);
    }
}