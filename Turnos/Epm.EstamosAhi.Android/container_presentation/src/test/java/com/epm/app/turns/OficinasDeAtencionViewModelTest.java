package com.epm.app.turns;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;


import com.epm.app.mvvm.turn.network.response.AssignedTurn;
import com.epm.app.mvvm.turn.network.response.Mensaje;
import com.epm.app.mvvm.turn.network.response.ShiftDevice;
import com.epm.app.mvvm.turn.network.response.officeDetail.Oficina;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.mvvm.turn.viewModel.OficinasDeAtencionViewModel;

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
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;
import io.reactivex.android.plugins.RxAndroidPlugins;
import io.reactivex.schedulers.Schedulers;

import static junit.framework.TestCase.assertFalse;
import static junit.framework.TestCase.assertTrue;
import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotEquals;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

public class OficinasDeAtencionViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @ClassRule
    public static final RxImmediateSchedulerRule schedulers = new RxImmediateSchedulerRule();

    @Mock
    TurnServicesRepository turnServicesRepository;

    @Mock
    public CustomSharedPreferences customSharedPreferences;

    @Mock
    ValidateInternet validateInternet;

    private OficinasDeAtencionViewModel oficinasDeAtencionViewModel;

    private MutableLiveData<AssignedTurn> assignedTurnMutableLiveData;

    @InjectMocks
    ShiftDevice shiftDevice;

    @InjectMocks
    AssignedTurn assignedTurn;

    @InjectMocks
    Mensaje mensaje;

    @InjectMocks
    Oficina oficina;

    @Before
    public void setUp() throws Exception {
        assignedTurnMutableLiveData = new MutableLiveData<>();
        turnServicesRepository = mock(TurnServicesRepository.class);
        customSharedPreferences = mock(CustomSharedPreferences.class);
        validateInternet = mock(ValidateInternet.class);
        MockitoAnnotations.initMocks(this);
        RxAndroidPlugins.setInitMainThreadSchedulerHandler(__
                -> Schedulers.trampoline());
        oficinasDeAtencionViewModel = Mockito.spy(new OficinasDeAtencionViewModel(turnServicesRepository, customSharedPreferences, validateInternet));
    }

    @Test
    public void testValidateTurnisTrue(){
        shiftDevice.setTurnoAsignado("turno");
        assignedTurn.setShiftDevice(shiftDevice);
        loadDates();
        fakeRepositories();
        oficinasDeAtencionViewModel.validateTurn(assignedTurn);
        assertEquals(assignedTurn,oficinasDeAtencionViewModel.getResponseAssignedTurn().getValue());
    }

    @Test
    public void testValidateTurnisFalse(){
        assignedTurn.setShiftDevice(shiftDevice);
        loadDates();
        fakeRepositories();
        oficinasDeAtencionViewModel.validateTurn(assignedTurn);
        assertTrue(oficinasDeAtencionViewModel.getWithOutAssignedTurn().getValue());
    }

    @Test
    public void testValidateTurnAssignedIsTrue(){
        shiftDevice.setTurnoAsignado("turno");
        assignedTurn.setShiftDevice(shiftDevice);
        loadDates();
        fakeRepositories();
        assertTrue(oficinasDeAtencionViewModel.assignedTurnOrTurnDeviceIsNotNull(assignedTurn));
    }

    @Test
    public void testValidateTurnAssignedIsFalse(){
        loadDates();
        fakeRepositories();
        assertFalse(oficinasDeAtencionViewModel.assignedTurnOrTurnDeviceIsNotNull(assignedTurn));
    }

    @Test
    public void testGetAssignedTurnIsNullOrEmptyIsTrue(){
        shiftDevice.setTurnoAsignado("turno");
        loadDates();
        fakeRepositories();
        assertFalse(oficinasDeAtencionViewModel.getAssignedTurnIsNullOrEmpty(shiftDevice));
    }

    @Test
    public void testGetAssignedTurnIsNullOrEmptyIsFalse(){
        loadDates();
        fakeRepositories();
        assertTrue(oficinasDeAtencionViewModel.getAssignedTurnIsNullOrEmpty(shiftDevice));
    }


    private void loadDates() {
        mensaje.setContenido("contenido");
        mensaje.setIdentificador(1);
        mensaje.setTitulo("titulo");
        assignedTurn.setMessage(mensaje);
        assignedTurnMutableLiveData.setValue(assignedTurn);
    }

    private void fakeRepositories(){
        String token = "token";
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(validateInternet.isConnected()).thenReturn(true);
        when(turnServicesRepository.getAssignedTurn(customSharedPreferences.getString(Constants.TOKEN), IdDispositive.getIdDispositive())).thenReturn(Observable.just(assignedTurn));
    }


}
