package com.epm.app.turns;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import androidx.lifecycle.MutableLiveData;
import android.content.res.Resources;
import android.graphics.drawable.Drawable;
import android.location.Location;

import com.epm.app.R;
import com.epm.app.mvvm.turn.network.request.RequestGetNearbyOffices;
import com.epm.app.mvvm.turn.network.response.nearbyOffices.NearbyOfficesItem;
import com.epm.app.mvvm.turn.network.response.nearbyOffices.NearbyOfficesResponse;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.mvvm.turn.viewModel.NearbyOfficesViewModel;
import com.epm.app.mvvm.turn.viewModel.iViewModel.INearbyOfficesViewModel;
import com.epm.app.services.ServiceWithControlErrorGPS;

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

import java.util.ArrayList;
import java.util.List;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.ConvertUtilities;
import io.reactivex.Observable;
import io.reactivex.android.plugins.RxAndroidPlugins;
import io.reactivex.schedulers.Schedulers;

import static org.junit.Assert.*;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

public class NearbyOfficesViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @ClassRule
    public static final RxImmediateSchedulerRule schedulers = new RxImmediateSchedulerRule();

    @Mock
    INearbyOfficesViewModel iNearbyOfficesViewModel;

    @Mock
    TurnServicesRepository turnServicesRepository;

    @InjectMocks
    NearbyOfficesResponse nearbyOfficesResponse;

    List<NearbyOfficesItem> list;

    @InjectMocks
    NearbyOfficesItem nearbyOfficesItem;

    @Mock
    public CustomSharedPreferences customSharedPreferences;

    @Mock
    ConvertUtilities convertUtilities;

    @InjectMocks
    RequestGetNearbyOffices solicitudObtenerOficinasCercanas;

    @Mock
    Location location;

    @Mock
    Resources resources;

    @Mock
    Drawable drawable;

    @Mock
    public ServiceWithControlErrorGPS serviceWithControlErrorGPS;

    @Mock
    ValidateInternet validateInternet;

    private NearbyOfficesViewModel nearbyOfficesViewModel;

    @InjectMocks
    private String informationOffice;

    private String officeName ="";
    public ErrorMessage errorMessage;

    @Before
    public void setUp() throws Exception {
        list = new ArrayList<>();
        errorMessage = new ErrorMessage(0,0);
        convertUtilities = mock(ConvertUtilities.class);
        resources = mock(Resources.class);
        turnServicesRepository = mock(TurnServicesRepository.class);
        serviceWithControlErrorGPS = mock(ServiceWithControlErrorGPS.class);
        validateInternet = mock(ValidateInternet.class);
        location = mock(Location.class);
        drawable = mock(Drawable.class);
        customSharedPreferences = mock(CustomSharedPreferences.class);
        MockitoAnnotations.initMocks(this);
        RxAndroidPlugins.setInitMainThreadSchedulerHandler(__
                -> Schedulers.trampoline());
        nearbyOfficesViewModel = Mockito.spy(new NearbyOfficesViewModel(turnServicesRepository, customSharedPreferences,serviceWithControlErrorGPS, validateInternet));
    }

    @After
    public void tearDown() throws Exception {
    }

    @Test
    public void getLocation() {
    }

    @Test
    public void testGetSuccessLocationNotNull() {
        MutableLiveData<Location> responseLocation = new MutableLiveData<>();
        when(serviceWithControlErrorGPS.getResponseLocation()).thenReturn(responseLocation);
        nearbyOfficesViewModel.getLocation();
        assertNotNull(nearbyOfficesViewModel.currentLocation);
    }

   @Test
    public void testGetSuccessNearbyOfficesNotNull() {
        loadDates();
        loadResponseTrue();
        //nearbyOfficesViewModel.setNearbyOfficesResponse(nearbyOfficesResponse);
        String token = "token";
        nearbyOfficesViewModel.setSolicitudObtenerOficinasCercanas(solicitudObtenerOficinasCercanas);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(validateInternet.isConnected()).thenReturn(true);
        when(turnServicesRepository.getNearbyOfficesResponse(customSharedPreferences.getString(Constants.TOKEN), solicitudObtenerOficinasCercanas )).thenReturn(Observable.just(nearbyOfficesResponse));
        nearbyOfficesViewModel.getNearbyOffices(location);
        assertNotNull(nearbyOfficesViewModel.getListNearbyOffices());
    }

    @Test
    public void testGetSuccessNearbyOfficesNull() {
        String token = "token";
        nearbyOfficesViewModel.setSolicitudObtenerOficinasCercanas(solicitudObtenerOficinasCercanas);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(turnServicesRepository.getNearbyOfficesResponse(customSharedPreferences.getString(Constants.TOKEN), solicitudObtenerOficinasCercanas )).thenReturn(null);
        nearbyOfficesViewModel.getNearbyOffices(location);
        assertNull(nearbyOfficesViewModel.getListNearbyOffices());
    }

    @Test
    public void testGetNearbyOfficesWithTurn() {
        boolean turn;
        loadResponseWithTurn();
        when(customSharedPreferences.getString(Constants.ASSIGNED_TRUN)).thenReturn("noSoyNulo");
        turn = nearbyOfficesViewModel.searchTurn();
        nearbyOfficesViewModel.validateTurn();
        verify(nearbyOfficesViewModel).deleteTurn(turn);
    }

    @Test
    public void testGetNearbyOfficesWithTurnIsNull() {
        boolean turn;
        loadResponseWithTurn();
        when(customSharedPreferences.getString(Constants.ASSIGNED_TRUN)).thenReturn(null);
        turn = nearbyOfficesViewModel.searchTurn();
        nearbyOfficesViewModel.validateTurn();
        verify(nearbyOfficesViewModel,times(0)).deleteTurn(turn);
    }

    @Test
    public void testGetNearbyOfficesSearchTurnTrue() {
        loadResponseWithTurn();
        assertTrue(nearbyOfficesViewModel.searchTurn());
    }

    @Test
    public void testGetNearbyOfficesSearchTurnFalse() {
        loadResponseWithOutTurn();
        assertFalse(nearbyOfficesViewModel.searchTurn());
    }

    private void loadDates(){
        solicitudObtenerOficinasCercanas.setLatitud(String.valueOf(location.getLatitude()));
        solicitudObtenerOficinasCercanas.setLongitud(String.valueOf(location.getLongitude()));
        solicitudObtenerOficinasCercanas.setIdDispositivo(IdDispositive.getIdDispositive());
    }

    private void loadResponseTrue(){
        nearbyOfficesResponse.setNearbyOfficesItems(list);
        nearbyOfficesItem.setNombreOficina("Edificio Inteligente");
        list.add(nearbyOfficesItem);
    }

    private void loadResponseWithTurn(){
        nearbyOfficesItem.setNombreOficina("Edificio Inteligente");
        nearbyOfficesItem.setTurnoAsignado("Ap20");
        list.add(nearbyOfficesItem);
        nearbyOfficesResponse.setNearbyOfficesItems(list);
        nearbyOfficesViewModel.setNearbyOfficesResponse(nearbyOfficesResponse);
    }

    private void loadResponseWithOutTurn(){
        nearbyOfficesItem.setNombreOficina("Edificio Inteligente");
        list.add(nearbyOfficesItem);
        nearbyOfficesResponse.setNearbyOfficesItems(list);
        nearbyOfficesViewModel.setNearbyOfficesResponse(nearbyOfficesResponse);
    }

    @Test
    public void testValidateShowGenerallyError(){
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        nearbyOfficesViewModel.validateShowError(errorMessage);
        assertEquals( nearbyOfficesViewModel.getError().getValue().getMessage(), errorMessage.getMessage());
    }

    @Test
    public void testValidateShowError401(){
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        nearbyOfficesViewModel.validateShowError(errorMessage);
        assertEquals(nearbyOfficesViewModel.getErrorUnauthorized(),(int) errorMessage.getMessage());
    }

    @Test
    public void testDrawInformation() {
        nearbyOfficesViewModel = new NearbyOfficesViewModel(resources);
        loadDatesDraw();
        when(resources.getDrawable(R.drawable.ic_unselectedlocatenearbyoffices)).thenReturn(drawable);
        nearbyOfficesViewModel.drawInformation();
        assertEquals(nearbyOfficesViewModel.textOfficeNameNearbyOffices.getValue(),officeName);
        assertEquals(nearbyOfficesViewModel.imageNearbyOffice.getValue(),drawable);
        //// assertEquals(nearbyOfficesViewModel.textInformationNearbyOffices.getValue(),nearbyOfficesViewModel.getInformationOffice());
    }

    private void loadDatesDraw(){
        nearbyOfficesViewModel.setNearbyOfficesItem(nearbyOfficesItem);
        float distancia = 0;
        String unidad ="";
        int turnosEnEspera = 0;
        String image ="";
        this.nearbyOfficesItem.setDistancia(distancia);
        this.nearbyOfficesItem.setUnidad(unidad);
        this.nearbyOfficesItem.setTurnosEnEspera(turnosEnEspera);
        this.nearbyOfficesItem.setNombreOficina(officeName);
    }
}