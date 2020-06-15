package com.epm.app.alertasItuango;



import android.arch.core.executor.testing.InstantTaskExecutorRule;
import android.arch.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.epm.app.mvvm.comunidad.network.response.places.Municipio;
import com.epm.app.mvvm.comunidad.network.response.places.ObtenerMunicipios;
import com.epm.app.mvvm.comunidad.network.response.places.ObtenerSectores;
import com.epm.app.mvvm.comunidad.network.response.places.Sectores;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.SolicitudSuscripcionNotificacionPush;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.Subscription;
import com.epm.app.mvvm.comunidad.repository.PlacesRepository;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import com.epm.app.mvvm.comunidad.viewModel.AlertasViewModel;
import com.epm.app.mvvm.util.Utils;

import static org.junit.Assert.*;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import org.junit.Assert;
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
import app.epm.com.utilities.utils.Constants;

public class AlertasViewModelTest {


    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();


    AlertasViewModel alertasViewModel;

    @Mock
    public PlacesRepository placesRepository;

    @Mock
    public CustomSharedPreferences customSharedPreferences;

    @Mock
    public Municipio municipio;

    @Mock
    public Sectores sectores;

    Mensaje mensaje;


    MutableLiveData<Subscription> saveSuscriptionMutableLiveData;
    @InjectMocks
    Subscription subscription;
    @Mock
    public List<Municipio> listMunicipio;

    @InjectMocks
    private SolicitudSuscripcionNotificacionPush solicitudSuscripcion;

    @Mock
    public List<Sectores> listSectores;

    @InjectMocks
    public SolicitudSuscripcionNotificacionPush solicitudSuscripcionNotificacionPush;

    @Mock
    public SubscriptionRepository subscriptionRepository;



   @Before
    public void setUp() throws Exception {
       placesRepository = mock(PlacesRepository.class);
       subscriptionRepository = mock(SubscriptionRepository.class);
       municipio = new Municipio();
       sectores = new Sectores();
       listMunicipio = new ArrayList<>();
       listSectores = new ArrayList<>();
       saveSuscriptionMutableLiveData = new MutableLiveData<>();
       customSharedPreferences = mock(CustomSharedPreferences.class);
       alertasViewModel =  Mockito.spy(new AlertasViewModel(placesRepository,customSharedPreferences,solicitudSuscripcionNotificacionPush,subscriptionRepository));
    }


    @Test
    public void testEmailisEmpty(){
        alertasViewModel.name.setValue("nombre");
        alertasViewModel.lastname.setValue("apellido");
        alertasViewModel.telephone.setValue("1234567890");
        alertasViewModel.email.setValue("");
        alertasViewModel.validateDates();
        int error = alertasViewModel.getError().getValue().getMessage();
        Assert.assertEquals(error,R.string.text_empty_email);
    }

    @Test
    public void testNameisEmpty(){
        alertasViewModel.name.setValue("");
        alertasViewModel.validateDates();
        int error = alertasViewModel.getError().getValue().getMessage();
        Assert.assertEquals(error,R.string.text_empty_name);
    }

    @Test
    public void testLastNameisEmpty(){
        alertasViewModel.name.setValue("nombre");
        alertasViewModel.lastname.setValue("");
        alertasViewModel.validateDates();
        int error = alertasViewModel.getError().getValue().getMessage();
        Assert.assertEquals(error,R.string.text_empty_lastname);
    }

    @Test
    public void testTelephoneisEmpty(){
        alertasViewModel.name.setValue("nombre");
        alertasViewModel.lastname.setValue("apellido");
        alertasViewModel.telephone.setValue("");
        alertasViewModel.validateDates();
        int error = alertasViewModel.getError().getValue().getMessage();
        Assert.assertEquals(error,R.string.text_empty_telephone);
    }

    @Test
    public void testMunicipioisEmpty(){
        alertasViewModel.name.setValue("nombre");
        alertasViewModel.lastname.setValue("apellido");
        alertasViewModel.telephone.setValue("1234567890");
        alertasViewModel.email.setValue("correo@correo.com");
        alertasViewModel.municipio.setValue("");
        alertasViewModel.validateDates();
        int error = alertasViewModel.getError().getValue().getMessage();
        Assert.assertEquals(error,R.string.text_empty_municipio);
    }
    @Test
    public void testSectorisEmpty(){
        alertasViewModel.name.setValue("nombre");
        alertasViewModel.lastname.setValue("apellido");
        alertasViewModel.telephone.setValue("1234567890");
        alertasViewModel.email.setValue("correo@correo.com");
        alertasViewModel.municipio.setValue("municipio");
        alertasViewModel.sector.setValue("");
        alertasViewModel.validateDates();
        int error = alertasViewModel.getError().getValue().getMessage();
        Assert.assertEquals(error,R.string.text_empty_sector);
    }

    @Test
    public void testPrivacyCheckedisFalse(){
        alertasViewModel.name.setValue("nombre");
        alertasViewModel.lastname.setValue("apellido");
        alertasViewModel.telephone.setValue("1234567890");
        alertasViewModel.email.setValue("correo@correo.com");
        alertasViewModel.municipio.setValue("municipio");
        alertasViewModel.sector.setValue("sector");
        alertasViewModel.information.setValue(true);
        alertasViewModel.privacity.setValue(false);
        alertasViewModel.validateDates();
        int error = alertasViewModel.getError().getValue().getMessage();
        Assert.assertEquals(error,R.string.text_empty_privacy);
    }

    @Test
    public void testReceivInformationCheckedisFalse(){
        alertasViewModel.name.setValue("nombre");
        alertasViewModel.lastname.setValue("apellido");
        alertasViewModel.telephone.setValue("1234567890");
        alertasViewModel.email.setValue("correo@correo.com");
        alertasViewModel.municipio.setValue("municipio");
        alertasViewModel.sector.setValue("sector");
        alertasViewModel.information.setValue(false);
        alertasViewModel.validateDates();
        int error = alertasViewModel.getError().getValue().getMessage();
        Assert.assertEquals(error,R.string.text_empty_information);
    }

    @Test
    public void testInvalidEmail(){
        alertasViewModel.name.setValue("nombre");
        alertasViewModel.lastname.setValue("apellido");
        alertasViewModel.telephone.setValue("1234567890");
        alertasViewModel.email.setValue("correo");
        alertasViewModel.municipio.setValue("municipio");
        alertasViewModel.sector.setValue("sector");
        alertasViewModel.information.setValue(true);
        alertasViewModel.privacity.setValue(true);
        alertasViewModel.validateDates();
        int error = alertasViewModel.getError().getValue().getMessage();
        Assert.assertEquals(error,R.string.text_incorrect_email);
    }

    @Test
    public void testValidateSizeFieldTelephoneNumberNotIsEqualsIs10(){
        alertasViewModel.name.setValue("nombre");
        alertasViewModel.lastname.setValue("apellido");
        alertasViewModel.telephone.setValue("315412");
        alertasViewModel.validateDates();
        int error = alertasViewModel.getError().getValue().getMessage();
        Assert.assertEquals(error,R.string.text_error_telephone_number);
    }

   @Test
    public void testValidateUserisGuestisTrue(){
       Usuario usuario = new Usuario();
       usuario.setInvitado(true);
       usuario.setNombres("nombres");
       alertasViewModel.validateUserisguest(usuario);
       assertTrue(alertasViewModel.name.getValue() == null );
   }

    @Test
    public void testValidateUserisGuestisFalse(){
        Usuario usuario = new Usuario();
        usuario.setInvitado(false);
        usuario.setNombres("nombres");
        alertasViewModel.validateUserisguest(usuario);
        assertTrue(alertasViewModel.name.getValue() != null );
    }

    @Test
    public void testonBackPressedArrow(){
       alertasViewModel.onBackPressedArrow();
       assertTrue(alertasViewModel.backPressed.getValue().booleanValue());
    }


    @Test
    public void testOnClickMunicipioIsEmpty(){
       alertasViewModel.onClickMunicipio();
       assertFalse(alertasViewModel.pushMunicipio.getValue().booleanValue());
    }

    @Test
    public void testOnClickMunicipioIsNotEmpty(){
       Municipio municipio = new Municipio();
       municipio.setEstado(true);
       municipio.setIdMunicipio(4);
       municipio.setDescripcion("municipio");
       List<Municipio> list = new ArrayList<>();
       list.add(municipio);
       alertasViewModel.setListMunicipio(list);
       alertasViewModel.onClickMunicipio();
       assertTrue(alertasViewModel.pushMunicipio.getValue().booleanValue());
    }

    @Test
    public void testOnClickSectoresIsEmpty(){
        alertasViewModel.onClickSectores();
        assertFalse(alertasViewModel.pushSector.getValue().booleanValue());
    }

    @Test
    public void testOnClickSectoresIsNotEmpty(){
       Sectores sectores = new Sectores();
       sectores.setDescripcion("sectores");
       sectores.setEstado(true);
       sectores.setIdDepartamento(4);
       sectores.setSector("sector");
       List<Sectores> list = new ArrayList<>();
       list.add(sectores);
       alertasViewModel.setListSector(list);
       alertasViewModel.onClickSectores();
       assertTrue(alertasViewModel.pushSector.getValue().booleanValue());
    }


    @Test
    public void testSubscriptionIsFailure(){
        alertasViewModel.name.setValue("pedro");
        alertasViewModel.lastname.setValue("");
        alertasViewModel.telephone.setValue("3022944355");
        alertasViewModel.email.setValue("km185n123@gmail.com");
        alertasViewModel.municipio.setValue("sabaneta");
        alertasViewModel.sector.setValue("la doctora");
        alertasViewModel.information.setValue(true);
        alertasViewModel.privacity.setValue(true);
        alertasViewModel.subscription();
        assertFalse(alertasViewModel.getError().getValue().getMessage() == R.string.title_suscription_successful);

    }

    @Test
    public void testLoadMunicipioisEmpty(){
        MutableLiveData<ObtenerMunicipios> obtenerMunicipios = new MutableLiveData<>();
        String token = "";
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(placesRepository.getMunicipios(customSharedPreferences.getString(Constants.TOKEN))).thenReturn(obtenerMunicipios);
        alertasViewModel.loadMunicipio(listMunicipio);
        assertTrue(alertasViewModel.getListMunicipio() == null);
    }

    @Test
    public void testLoadMunicipioisNotEmpty(){
        municipio.setEstado(true);
        municipio.setIdMunicipio(4);
        municipio.setDescripcion("municipio");
        listMunicipio.add(municipio);
        ObtenerMunicipios obtener = new ObtenerMunicipios();
        obtener.municipios = listMunicipio;
        MutableLiveData<ObtenerMunicipios> obtenerMunicipios = new MutableLiveData<>();
        obtenerMunicipios.setValue(obtener);
        String token = "";
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(placesRepository.getMunicipios(customSharedPreferences.getString(Constants.TOKEN))).thenReturn(obtenerMunicipios);
        alertasViewModel.loadMunicipio(listMunicipio);
        assertTrue(alertasViewModel.getListMunicipio() != null);

    }

    @Test
    public void testLoadSectorisEmpty(){
        MutableLiveData<ObtenerSectores> obtenerSectores = new MutableLiveData<>();
        String token = "";
        int id=1;
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(placesRepository.getSectores(customSharedPreferences.getString(Constants.TOKEN),id)).thenReturn(obtenerSectores);
        alertasViewModel.loadSector(id);
        assertTrue(alertasViewModel.getListMunicipio() == null);
    }

    @Test
    public void testLoadSectorisNotEmpty(){
        String token = "";
        int id = 1;
        sectores.setEstado(true);
        sectores.setIdSector(4);
        sectores.setDescripcion("municipio");
        listSectores.add(sectores);
        ObtenerSectores obtener = new ObtenerSectores();
        obtener.sectores = listSectores;
        MutableLiveData<ObtenerSectores> obtenerSectores= new MutableLiveData<>();
        obtenerSectores.setValue(obtener);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(placesRepository.getSectores(customSharedPreferences.getString(Constants.TOKEN),id)).thenReturn(obtenerSectores);
        alertasViewModel.loadSector(id);
        assertTrue(alertasViewModel.getListSector() != null);
    }

    @Test
    public void testgetMunicipiosArrayFromItemGeneralListisEmpty(){
       String[] vector;
       vector= Utils.getMunicipiosArrayFromItemGeneralList(listMunicipio);
       assertTrue(vector.length == 0);
    }


    @Test
    public void testgetMunicipiosArrayFromItemGeneralListisNotEmpty(){
        String[] vector;
        municipio.setEstado(true);
        municipio.setIdMunicipio(4);
        municipio.setDescripcion("municipio");
        listMunicipio.add(municipio);
        vector=Utils.getMunicipiosArrayFromItemGeneralList(listMunicipio);
        assertTrue(vector.length != 0);
    }

    @Test
    public void testgetSectoresArrayFromItemGeneralListisEmpty(){
        String[] vector;
        vector=Utils.getSectoresArrayFromItemGeneralList(listSectores);
        assertTrue(vector.length == 0);
    }


    @Test
    public void testgetSectoresArrayFromItemGeneralListisNotEmpty(){
        String[] vector;
        sectores.setEstado(true);
        sectores.setIdSector(4);
        sectores.setDescripcion("municipio");
        listSectores.add(sectores);
        vector=Utils.getSectoresArrayFromItemGeneralList(listSectores);
        assertTrue(vector.length != 0);
    }

    @Test
    public void testSubscriptionisNotNull(){
       loadData();
       falseRepositories();
       subscription.setStateTransaction(true);
       when(subscriptionRepository.saveSubscription(customSharedPreferences.getString(Constants.TOKEN),solicitudSuscripcion)).thenReturn(saveSuscriptionMutableLiveData);
       alertasViewModel.subscription();
       verify(alertasViewModel).validateSubscription(saveSuscriptionMutableLiveData.getValue());
    }

    @Test
    public void testSaveSubscriptionIsSuccessful(){
        loadData();
        falseRepositories();
        subscription.setStateTransaction(true);
        when(subscriptionRepository.saveSubscription(customSharedPreferences.getString(Constants.TOKEN),solicitudSuscripcion)).thenReturn(saveSuscriptionMutableLiveData);
        alertasViewModel.validateSubscription(saveSuscriptionMutableLiveData.getValue());
        assertTrue(alertasViewModel.isStatusSubscription());
    }

    @Test
    public void testSaveSubscriptionIsNotSuccesful(){
       loadData();
       falseRepositories();
       subscription.setStateTransaction(false);
       when(subscriptionRepository.saveSubscription(customSharedPreferences.getString(Constants.TOKEN),solicitudSuscripcion)).thenReturn(saveSuscriptionMutableLiveData);
       alertasViewModel.validateSubscription(saveSuscriptionMutableLiveData.getValue());
       assertFalse(alertasViewModel.isStatusSubscription());

    }

    @Test
    public void testSaveSuscriptionDataIsNotEmpty(){
        alertasViewModel.name.setValue("pedro");
        alertasViewModel.lastname.setValue("lopez");
        alertasViewModel.telephone.setValue("3022944355");
        alertasViewModel.email.setValue("km185n123@gmail.com");
        alertasViewModel.municipio.setValue("sabaneta");
        alertasViewModel.sector.setValue("parque");
        alertasViewModel.information.setValue(true);
        alertasViewModel.privacity.setValue(true);
        alertasViewModel.saveSuscriptionData();
        assertTrue(!alertasViewModel.getSolicitudSuscripcion().getEmail().isEmpty());
    }


    @Test
    public void testSaveSuscriptionDataIsEmpty(){
        alertasViewModel.name.setValue("");
        alertasViewModel.lastname.setValue("");
        alertasViewModel.telephone.setValue("");
        alertasViewModel.email.setValue("");
        alertasViewModel.municipio.setValue("");
        alertasViewModel.sector.setValue("");
        alertasViewModel.information.setValue(false);
        alertasViewModel.privacity.setValue(false);
        alertasViewModel.saveSuscriptionData();
        assertTrue(alertasViewModel.getSolicitudSuscripcion().getEmail().isEmpty());
    }

    @Test
    public void testValidateSuscriptionMessageisEmpty(){
       String dato = "";
       String titledato = "";
       alertasViewModel.validateSuscriptionMessage(titledato,dato);
       assertTrue(alertasViewModel.getResponseService().getValue() == null);
    }

    @Test
    public void testValidateSuscriptionMessageisNull(){
        String dato = null;
        String titledato = null;
        alertasViewModel.validateSuscriptionMessage(titledato,dato);
        assertNull(alertasViewModel.getResponseService().getValue());
    }

    @Test
    public void testValidateSuscriptionMessageisNotEmpty(){
        String message = "dato";
        String titledato = "titulo";
        alertasViewModel.validateSuscriptionMessage(titledato,message);
        assertSame(alertasViewModel.getResponseService().getValue(),message);
    }

    private void falseRepositories() {
        String token = "token";
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
    }

    private void loadData(){
        alertasViewModel.name.setValue("pedro");
        alertasViewModel.lastname.setValue("lopez");
        alertasViewModel.telephone.setValue("3022944355");
        alertasViewModel.email.setValue("km185n123@gmail.com");
        alertasViewModel.municipio.setValue("sabaneta");
        alertasViewModel.sector.setValue("parque");
        alertasViewModel.information.setValue(true);
        alertasViewModel.privacity.setValue(true);
        mensaje = new Mensaje();
        subscription.setMensaje(mensaje);
        saveSuscriptionMutableLiveData.setValue(subscription);
        alertasViewModel.setSolicitudSuscripcion(solicitudSuscripcion);
    }

}
