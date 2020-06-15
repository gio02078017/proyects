package com.epm.app.alertasItuango;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import androidx.lifecycle.MutableLiveData;
import com.epm.app.R;
import com.epm.app.mvvm.comunidad.network.request.CancelSubscriptionRequest;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.CancelSubscriptionResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotifications;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPushAlertasItuango;
import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.epm.app.mvvm.comunidad.network.response.places.Municipio;
import com.epm.app.mvvm.comunidad.network.response.places.ObtenerMunicipios;
import com.epm.app.mvvm.comunidad.network.response.places.ObtenerSectores;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.RequestUpdateSubscription;
import com.epm.app.mvvm.comunidad.network.response.places.Sectores;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.UpdateSubscription;
import com.epm.app.mvvm.comunidad.repository.PlacesRepository;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import com.epm.app.mvvm.comunidad.viewModel.UpdateSubscriptionViewModel;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.IUpdateSubscriptionViewModel;
import com.epm.app.turns.RxImmediateSchedulerRule;

import org.junit.Assert;
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
import io.reactivex.Observable;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.android.plugins.RxAndroidPlugins;
import io.reactivex.schedulers.Schedulers;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNull;
import static org.junit.Assert.assertTrue;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.when;


public class UpdateSubscriptionViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @ClassRule
    public static final RxImmediateSchedulerRule schedulers = new RxImmediateSchedulerRule();

    @Mock
    public IUpdateSubscriptionViewModel iUpdateSubscriptionViewModel;

    @Mock
    public SubscriptionRepository subscriptionRepository;

    @Mock
    public PlacesRepository placesRepository;

    @InjectMocks
    public UpdateSubscription updateSubscription;


    public List<Municipio> listMunicipio;
    public List<Sectores> listSector;

    private int id = 2;


    public CancelSubscriptionResponse cancelSubscriptionResponse;


    public CancelSubscriptionRequest cancelSubscriptionRequest;


    @InjectMocks
    MutableLiveData<ObtenerMunicipios> municipiosMutableLiveData;
    @InjectMocks
    ObtenerMunicipios obtenerMunicipios;
    @InjectMocks
    Municipio municipio;

    @Mock
    ValidateInternet validateInternet;

    MutableLiveData<ObtenerSectores> obtenerSectoresLiveData = new MutableLiveData<>();

    ObtenerSectores obtenerSectores= new ObtenerSectores();

    @Mock
    public CustomSharedPreferences customSharedPreferences;

    @InjectMocks
    public GetSubscriptionNotificationsPushAlertasItuango subscription;

    @Mock
    MutableLiveData<GetSubscriptionNotificationsPushAlertasItuango> getSubscriptionMutableLiveData;


    public UpdateSubscriptionViewModel updateSubscriptionViewModel;

    @Mock
    RequestUpdateSubscription solicitudActualizar;

    @Mock
    public Mensaje mensaje;

    @Mock
    public GetSubscriptionNotifications getSubscriptionNotifications;


    public String token;
    public String idDispositivo;
    public ErrorMessage errorMessage;

    @Before
    public void setUp() {

       // placesRepository = mock(PlacesRepository.class);
        listMunicipio = new ArrayList<>();
        errorMessage = new ErrorMessage(0,0);
        listSector = new ArrayList<>();
        mensaje = new Mensaje();
        getSubscriptionMutableLiveData = new MutableLiveData<>();
        getSubscriptionNotifications = new GetSubscriptionNotifications();
        solicitudActualizar = new RequestUpdateSubscription();
        cancelSubscriptionResponse = new CancelSubscriptionResponse();
        cancelSubscriptionRequest = new CancelSubscriptionRequest();
        MockitoAnnotations.initMocks(this);
        RxAndroidPlugins.setInitMainThreadSchedulerHandler(__
                -> Schedulers.trampoline());
        customSharedPreferences = mock(CustomSharedPreferences.class);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(validateInternet.isConnected()).thenReturn(true);
        updateSubscriptionViewModel = Mockito.spy(new UpdateSubscriptionViewModel(subscriptionRepository,customSharedPreferences,placesRepository,validateInternet));
        loadData();

    }



    public void loadData() {
        updateSubscriptionViewModel.nameRecivied.setValue("julian");
        updateSubscriptionViewModel.lastNameRecivied.setValue("lopez");
        updateSubscriptionViewModel.telephoneRecivied.setValue("3022944355");
        updateSubscriptionViewModel.emailRecivied.setValue("km185n123@gmail.com");
        updateSubscriptionViewModel.municipality.setValue("abc");
        updateSubscriptionViewModel.sector.setValue("acb");

        token = "1234";

        mensaje.setContenido("content...");
        mensaje.setIdentificador(1);
        mensaje.setTitulo("title");

        subscription.setMensaje(mensaje);

        getSubscriptionNotifications.setAcceptTermsConditions(true);
        getSubscriptionNotifications.setEmail(updateSubscriptionViewModel.emailRecivied.getValue());
        getSubscriptionNotifications.setIdDispositive("abc");
        getSubscriptionNotifications.setIdOneSignal("abc");
        getSubscriptionNotifications.setIdSuscription(1);
        getSubscriptionNotifications.setLastName(updateSubscriptionViewModel.lastNameRecivied.getValue());
        getSubscriptionNotifications.setMunicipality(1);
        getSubscriptionNotifications.setName(updateSubscriptionViewModel.nameRecivied.getValue());
        getSubscriptionNotifications.setNotification(true);
        getSubscriptionNotifications.setSector(1);
        getSubscriptionNotifications.setTelephone(updateSubscriptionViewModel.telephoneRecivied.getValue());

        solicitudActualizar.setAceptaTerminosCondiciones(getSubscriptionNotifications.acceptTermsConditions);

        cancelSubscriptionRequest.setIdDispositivo("123");
        cancelSubscriptionRequest.setIdSuscripcion(1);
        cancelSubscriptionResponse.setEstadoTransaccion(true);
        cancelSubscriptionResponse.setMessage(mensaje);
    }

    @Test
    public void testNameisEmpty() {
        updateSubscriptionViewModel.nameRecivied.setValue("");
        updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue().getMessage() == R.string.text_empty_name);
    }

    @Test
    public void testNameisNotEmpty() {
        updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue() == null);
    }

    @Test
    public void testLastNameReciviedisEmpty() {
        updateSubscriptionViewModel.lastNameRecivied.setValue("");
        boolean validation = updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue().equals(R.string.text_empty_lastname) || validation == false);
    }

    @Test
    public void testLastNameReciviedisNotEmpty() {
        updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue() == null);
    }

    @Test
    public void testTelephoneReciviedIsCorrect() {
        updateSubscriptionViewModel.telephoneRecivied.setValue("3022944355");
        boolean validation = updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.telephoneRecivied.getValue().length() == 10);
    }

    @Test
    public void testTelephoneReciviedIsNotCorrect() {
        updateSubscriptionViewModel.telephoneRecivied.setValue("302294435");
        boolean validation = updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.telephoneRecivied.getValue().length() != 10);
    }

    @Test
    public void testTelephoneReciviedisEmpty() {
        updateSubscriptionViewModel.telephoneRecivied.setValue("");
        boolean validation = updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue().equals(R.string.text_empty_telephone) || validation == false);
    }

    @Test
    public void testTelephoneReciviedisNotEmpty() {
        boolean validation = updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue() == null);
    }

    @Test
    public void testEmailReciviedisEmpty() {
        updateSubscriptionViewModel.emailRecivied.setValue("");
        boolean validation = updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue().equals(R.string.text_empty_email) || validation == false);
    }

    @Test
    public void testEmailReciviedisNotEmpty() {
        updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue() == null);
    }

    @Test
    public void testEmailReciviedisCorrect() {
        updateSubscriptionViewModel.emailRecivied.setValue("km185n123@gmail.com");
        boolean validation = updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue() == null);
    }

    @Test
    public void testEmailReciviedisNotCorrect() {
        updateSubscriptionViewModel.emailRecivied.setValue("km185n123@gmailcom");
        boolean validation = updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue().equals(R.string.text_incorrect_email) || validation == false);
    }

    @Test
    public void testMucipalityisEmpty() {
        updateSubscriptionViewModel.municipality.setValue("");
        boolean validation = updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue().equals(R.string.text_empty_municipio) || validation == false);
    }

    @Test
    public void testMucipalityisNotEmpty() {
        updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue() == null);
    }

    @Test
    public void testSectorisEmpty() {
        updateSubscriptionViewModel.sector.setValue("");
        boolean validation = updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue().equals(R.string.text_empty_sector) || validation == false);
    }

    @Test
    public void testSectorisNotEmpty() {
        updateSubscriptionViewModel.validateData();
        Assert.assertTrue(updateSubscriptionViewModel.getError().getValue() == null);
    }

    @Test
    public void testUpdateSubscriptionWhenInformationFormIsCorrect(){
        MutableLiveData<UpdateSubscription> updateSubscriptionLiveData = new MutableLiveData<>();
        updateSubscriptionViewModel.setSubscription(subscription);
        subscription.setSuscripcionNotificacionesPushComunidad(getSubscriptionNotifications);
        updateSubscriptionViewModel.setSolicitudActualizar(solicitudActualizar);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(subscriptionRepository.updateSubscription(token,solicitudActualizar)).thenReturn(Observable.just(updateSubscription));
        updateSubscriptionViewModel.updateSubscription();
        MutableLiveData<Boolean> showDialog = updateSubscriptionViewModel.getProgressDialog();
        //Assert.assertTrue(showDialog.getValue());
    }

    @Test
    public void testUpdateSubscriptionWhenInformationFormIsNotCorrect(){
        MutableLiveData<UpdateSubscription> updateSubscriptionLiveData = new MutableLiveData<>();
        updateSubscriptionViewModel.emailRecivied.setValue("km185n123@gmail.com");
        updateSubscriptionViewModel.setSubscription(subscription);
        subscription.setSuscripcionNotificacionesPushComunidad(getSubscriptionNotifications);
        updateSubscriptionViewModel.setSolicitudActualizar(solicitudActualizar);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(subscriptionRepository.updateSubscription(token,solicitudActualizar)).thenReturn(Observable.just(updateSubscription));
        updateSubscriptionViewModel.updateSubscription();
        MutableLiveData<Boolean> showDialog = updateSubscriptionViewModel.getProgressDialog();
        Assert.assertFalse(showDialog.getValue() == null);
    }

    @Test
    public void test_onClickMunicipio_validate_if_listMunicipio_is_empty(){
        listMunicipio.add(new Municipio());
       updateSubscriptionViewModel.setListMunicipio(listMunicipio);
       updateSubscriptionViewModel.onClickMunicipio();
        Assert.assertTrue(updateSubscriptionViewModel.pushMunicipio.getValue());
    }

    @Test
    public void test_onClickMunicipio_validate_if_listMunicipio_is_not_empty(){
        updateSubscriptionViewModel.setListMunicipio(listMunicipio);
        updateSubscriptionViewModel.onClickMunicipio();
        Assert.assertTrue(updateSubscriptionViewModel.pushMunicipio.getValue() == null);

    }

    @Test
    public void test_onClickSectores_validate_if_listSectores_is_not_empty(){
        listSector.add(new Sectores());
        updateSubscriptionViewModel.setListSector(listSector);
        updateSubscriptionViewModel.onClickSectores();
        Assert.assertTrue(updateSubscriptionViewModel.pushSector.getValue());
    }

    @Test
    public void test_onClickSectores_validate_if_listSectores_is_empty(){
        updateSubscriptionViewModel.setListSector(listSector);
        updateSubscriptionViewModel.onClickSectores();
        Assert.assertTrue(updateSubscriptionViewModel.pushSector.getValue() == null);

    }

    @Test
    public void test_getIdmuncipio_should_have_a_idmunicipio(){
        updateSubscriptionViewModel.setIdmuncipio(1);
        int id = updateSubscriptionViewModel.getIdmuncipio();
        Assert.assertEquals(1,id);
    }

    @Test
    public void test_getIdmuncipio_should_do_not_have_a_idmunicipio(){
        int id = updateSubscriptionViewModel.getIdmuncipio();
        Assert.assertEquals(0,id);
    }

    @Test
    public void test_loadSector_validate_if_listSector_is_empty(){
        MutableLiveData<ObtenerSectores> obtenerSectoresLiveData = new MutableLiveData<>();
        ObtenerSectores obtenerSectores= new ObtenerSectores();
        int numberOfItem = 0;
        int numberOfItemObtained = 0;
        listSector = getMockListSector(numberOfItem);
        obtenerSectores.setMensaje(mensaje);
        obtenerSectores.setSectores(listSector);
        obtenerSectores.setThrowable(new Throwable());
        obtenerSectoresLiveData.setValue(obtenerSectores);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(placesRepository.getSectores(token,2)).thenReturn(obtenerSectoresLiveData);
        updateSubscriptionViewModel.loadSector(2);
        Assert.assertTrue(updateSubscriptionViewModel.getListSector() == null);

    }

    @Test
    public void test_loadSector_validate_if_listSector_is_not_empty(){
        MutableLiveData<ObtenerSectores> obtenerSectoresLiveData = new MutableLiveData<>();
        ObtenerSectores obtenerSectores= new ObtenerSectores();
        int numberOfItem = 2;
        int numberOfItemObtained = 0;
        listSector = getMockListSector(numberOfItem);
        obtenerSectores.setMensaje(mensaje);
        obtenerSectores.setSectores(listSector);
        obtenerSectores.setThrowable(new Throwable());
        obtenerSectoresLiveData.setValue(obtenerSectores);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(placesRepository.getSectores(token,2)).thenReturn(obtenerSectoresLiveData);

        updateSubscriptionViewModel.loadSector(2);
        numberOfItemObtained = updateSubscriptionViewModel.getListSector().size();
        Assert.assertEquals(numberOfItem,numberOfItemObtained);
    }

    @Test
    public void testGetSubscription(){
        falseRepositories();
        loadMunicipality();
        loadSector();
        when(placesRepository.getMunicipios(customSharedPreferences.getString(Constants.TOKEN),Constants.TYPE_SUSCRIPTION_ALERTAS)).thenReturn(municipiosMutableLiveData);
        when(placesRepository.getSectores(customSharedPreferences.getString(Constants.TOKEN),id)).thenReturn(obtenerSectoresLiveData);
        when(subscriptionRepository.getSubscriptionAlertasItuango(customSharedPreferences.getString(Constants.TOKEN), IdDispositive.getIdDispositive())).thenReturn(getSubscriptionMutableLiveData);
        updateSubscriptionViewModel.getSuscription();
//        verify(updateSubscriptionViewModel).loadSubscription(subscription);
    }

   /* @Test
    public void testNotHasMunicipalities(){
        falseRepositories();
        loadSector();
        when(placesRepository.getMunicipios(customSharedPreferences.getString(Constants.TOKEN))).thenReturn(municipiosMutableLiveData);
        when(placesRepository.getSectores(customSharedPreferences.getString(Constants.TOKEN),2)).thenReturn(obtenerSectoresLiveData);
        when(subscriptionRepository.getSubscriptionAlertasItuango(customSharedPreferences.getString(Constants.TOKEN), IdDispositive.getIdDispositive())).thenReturn(getSubscriptionMutableLiveData);
        updateSubscriptionViewModel.loadPlaces();
        assertNull(updateSubscriptionViewModel.enableMunicipio.getValue());
    }*/

    @Test
    public void testItHasMunicipalities(){
        falseRepositories();
        loadSector();
        loadMunicipality();
        when(placesRepository.getMunicipios(customSharedPreferences.getString(Constants.TOKEN),Constants.TYPE_SUSCRIPTION_ALERTAS)).thenReturn(municipiosMutableLiveData);
        when(placesRepository.getSectores(customSharedPreferences.getString(Constants.TOKEN),2)).thenReturn(obtenerSectoresLiveData);
        getSubscriptionNotifications.setMunicipality(id);
        getSubscriptionNotifications.setSector(id);
        subscription.setSuscripcionNotificacionesPushComunidad(getSubscriptionNotifications);
        updateSubscriptionViewModel.setSubscription(subscription);
        updateSubscriptionViewModel.loadPlaces();
        assertTrue(updateSubscriptionViewModel.enableMunicipio.getValue());
    }

    @Test
    public void testValidateShowErrorPlacesError401(){
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        updateSubscriptionViewModel.validateShowError(errorMessage);
        assertEquals(updateSubscriptionViewModel.getErrorUnauthorized(),(int) errorMessage.getMessage());
    }

    @Test
    public void testValidateShowErrorPlacesError(){
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        updateSubscriptionViewModel.validateShowError(errorMessage);
        assertEquals( updateSubscriptionViewModel.getError().getValue().getMessage(), errorMessage.getMessage());
    }


    private void falseRepositories(){
        getSubscriptionMutableLiveData.setValue(subscription);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
    }

    private void loadMunicipality(){
        municipio.setDescripcion("municipio");
        municipio.setIdMunicipio(2);
        listMunicipio.add(municipio);
        obtenerMunicipios.setMunicipios(listMunicipio);
        municipiosMutableLiveData.setValue(obtenerMunicipios);
    }

    private void loadSector(){
        int numberOfItem = 0;
        int numberOfItemObtained = 0;
        listSector = getMockListSector(numberOfItem);
        obtenerSectores.setMensaje(mensaje);
        obtenerSectores.setSectores(listSector);
        obtenerSectores.setThrowable(new Throwable());
        obtenerSectoresLiveData.setValue(obtenerSectores);
    }

    private List<Sectores> getMockListSector(int numItems) {
        List<Sectores> sectores = new ArrayList<>();
        for (int i = 0 ; i < numItems; i++){
            Sectores sector = new Sectores();
            sector.setDescripcion("description"+i);
            sector.setEstado(true);
            sector.setIdDepartamento(i);
            sector.setIdSector(i);
            sector.setOrdenGeograficoMunicipio(i);
            sector.setSector("sector"+i);
            sectores.add(sector);
        }

        return sectores;
    }





    @Test
    public void test_cancel_should_can_not_cancel_subscription(){
        when(subscriptionRepository.cancelSubscription(token,cancelSubscriptionRequest)).thenReturn(Observable.just(cancelSubscriptionResponse));
        updateSubscriptionViewModel.cancel(token,null);
        verify(subscriptionRepository,times(0)).cancelSubscription(token,cancelSubscriptionRequest);

    }

    @Test
    public void test_cancel_should_can_cancel_subscription(){
        when(subscriptionRepository.cancelSubscription(customSharedPreferences.getString(Constants.TOKEN),cancelSubscriptionRequest)).thenReturn(Observable.just(cancelSubscriptionResponse));
        updateSubscriptionViewModel.cancel(customSharedPreferences.getString(Constants.TOKEN),cancelSubscriptionRequest);
        verify(updateSubscriptionViewModel).validateCancelTurn(cancelSubscriptionResponse);

    }

    @Test
    public void test_setParameterCancelSubscriptionRequest_should_can_insert_data_in_CancelSubscriptionRequest(){
        updateSubscriptionViewModel.setParameterCancelSubscriptionRequest(123,"123");
        String idDispositivo = updateSubscriptionViewModel.getCancelSubscriptionRequest().getIdDispositivo();
        Assert.assertEquals("123",idDispositivo);

    }

    @Test
    public void test_setParameterCancelSubscriptionRequest_data_is_not_equals(){
        updateSubscriptionViewModel.setParameterCancelSubscriptionRequest(123,"123");
        String idDispositivo = updateSubscriptionViewModel.getCancelSubscriptionRequest().getIdDispositivo();
        Assert.assertNotEquals("12",idDispositivo);
    }

    @Test
    public void test_getCancelSubscriptionRequest_is_not_null(){
        CancelSubscriptionRequest  cancelSubscriptionRequest = updateSubscriptionViewModel.getCancelSubscriptionRequest();
        Assert.assertNotNull(cancelSubscriptionRequest);
    }

    @Test
    public void test_getCancelSubscriptionRequest_is_null(){
        updateSubscriptionViewModel.setCancelSubscriptionRequest(null);
        CancelSubscriptionRequest  cancelSubscriptionRequest = updateSubscriptionViewModel.getCancelSubscriptionRequest();
        Assert.assertNull(cancelSubscriptionRequest);
    }




}

















