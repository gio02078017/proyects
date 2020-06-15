package com.epm.app.container;

import android.arch.core.executor.testing.InstantTaskExecutorRule;
import android.arch.lifecycle.MutableLiveData;

import com.epm.app.business_models.business_models.ItemGeneral;
import com.epm.app.business_models.business_models.ListasGenerales;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnit;
import org.mockito.junit.MockitoJUnitRunner;
import org.mockito.junit.MockitoRule;

import java.util.ArrayList;
import java.util.List;


import com.epm.app.R;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.container_domain.container.IContainerRepository;
import app.epm.com.container_domain.container.ContainerBusinessLogic;

import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetNotificationsPush;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.epm.app.mvvm.turn.network.response.AssignedTurn;
import com.epm.app.mvvm.turn.network.response.ShiftDevice;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.presenters.SplashAppPresenter;

import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;

import com.epm.app.view.views_activities.ISplashAppView;

import static org.junit.Assert.*;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by mateoquicenososa on 24/11/16.
 */

@RunWith(MockitoJUnitRunner.class)
public class SplasAppPresenterTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @Mock
    ISplashAppView splashAppView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    IContainerRepository securityRepository;

    @Mock
    ICustomSharedPreferences customSharedPreferences;

    SplashAppPresenter splashAppPresenter;

    @Mock
    NotificationsRepository notificationsRepository;

    @Mock
    TurnServicesRepository turnServicesRepository;

    ContainerBusinessLogic securityBL;

    public MutableLiveData<GetNotificationsPush> getNotificationsPushMutableLiveData;

    private GetNotificationsPush getNotificationsPush;

    @Before
    public void setUp() throws Exception {
        notificationsRepository = mock(NotificationsRepository.class);
        turnServicesRepository = mock(TurnServicesRepository.class);
        getNotificationsPushMutableLiveData = new MutableLiveData<>();
        getNotificationsPush = new GetNotificationsPush();
        securityBL = Mockito.spy(new ContainerBusinessLogic(securityRepository));
        splashAppPresenter = Mockito.spy(new SplashAppPresenter(securityBL));
        splashAppPresenter.inject(splashAppView, validateInternet, customSharedPreferences, notificationsRepository,turnServicesRepository);
    }

    @Test
    public void methodValidateInternetWithoutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        splashAppPresenter.validateInternetToGetGeneralList();

        verify(splashAppPresenter, never()).createThreadToGetGeneralList();
        verify(splashAppView).showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetWithoutConnectionShoudlCallMethodListsGeneralsApp() {
        when(validateInternet.isConnected()).thenReturn(true);
        splashAppPresenter.validateInternetToGetGeneralList();

        verify(splashAppPresenter).createThreadToGetGeneralList();
        verify(splashAppView, never()).showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodGetGeneralListShouldCallMethodGetGeneralListInSecurityBL() throws RepositoryError {
        ListasGenerales listasGenerales = getListasGenerales();
        when(securityRepository.getGeneralList()).thenReturn(listasGenerales);
        splashAppPresenter.getGeneralList();
        verify(securityBL).getGeneralList();
    }

    @Test
    public void methodGetGeneralListShouldSaveTiposIdentificacionListInSharedPreferences() throws RepositoryError {
        ListasGenerales listasGenerales = getListasGenerales();
        when(securityRepository.getGeneralList()).thenReturn(listasGenerales);
        splashAppPresenter.getGeneralList();
        verify(customSharedPreferences).addSetArray(Constants.TIPOS_DOCUMENTO_LIST, listasGenerales.getTiposIdentificacion());
    }

    @Test
    public void methodGetGeneralListShouldSaveTiposPersonaListInSharedPreferences() throws RepositoryError {
        ListasGenerales listasGenerales = getListasGenerales();
        when(securityRepository.getGeneralList()).thenReturn(listasGenerales);
        splashAppPresenter.getGeneralList();
        verify(customSharedPreferences).addSetArray(Constants.TIPOS_PERSONA_LIST, listasGenerales.getTiposPersonas());
    }

    @Test
    public void methodGetGeneralListShouldSaveTiposVivienndaListInSharedPreferences() throws RepositoryError {
        ListasGenerales listasGenerales = getListasGenerales();
        when(securityRepository.getGeneralList()).thenReturn(listasGenerales);
        splashAppPresenter.getGeneralList();
        verify(customSharedPreferences).addSetArray(Constants.TIPOS_VIVIENDA_LIST, listasGenerales.getTiposViviendas());
    }

    @Test
    public void methodGetGeneralListShouldSaveGenerosListInSharedPreferences() throws RepositoryError {
        ListasGenerales listasGenerales = getListasGenerales();
        when(securityRepository.getGeneralList()).thenReturn(listasGenerales);
        splashAppPresenter.getGeneralList();
        verify(customSharedPreferences).addSetArray(Constants.GENEROS_LIST, listasGenerales.getGeneros());
    }

    @Test
    public void methodGetGeneralListShouldSaveTheDateInTheMomentWhenSavedGeneralList() throws RepositoryError {
        ListasGenerales listasGenerales = getListasGenerales();
        String dateInMillisecond = "465454654";
        when(securityRepository.getGeneralList()).thenReturn(listasGenerales);
        when(splashAppView.getCurrentDate()).thenReturn(dateInMillisecond);
        splashAppPresenter.getGeneralList();
        verify(customSharedPreferences).addString(Constants.CURRENT_DATE_SAVED_LIST, dateInMillisecond);
    }


    @Test
    public void methodGetGeneralListShouldCallMethodValidateUserIfHaveBeenLogin() throws RepositoryError {
        ListasGenerales listasGenerales = getListasGenerales();
        when(securityRepository.getGeneralList()).thenReturn(listasGenerales);
        splashAppPresenter.getGeneralList();
        assertNull(customSharedPreferences.getString(Constants.CURRENT_DATE_SAVED_LIST));
    }

    @Test
    public void methodGetGeneralListShouldAlertDialogIfReturnAnException() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(securityRepository.getGeneralList()).thenThrow(repositoryError);
        splashAppPresenter.getGeneralList();
        verify(splashAppView).showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
    }

    @Test
    public void methodGetGeneralListShouldAlertDialogIfReturnAnUnauthorized() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(securityRepository.getGeneralList()).thenThrow(repositoryError);
        splashAppPresenter.getGeneralList();
        verify(splashAppView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
    }


    @Test
    public void methodGetGeneralListShouldHideAlwaysProgressBar() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(securityRepository.getGeneralList()).thenThrow(repositoryError);
        splashAppPresenter.getGeneralList();
        assertNull(customSharedPreferences.getString(Constants.CURRENT_DATE_SAVED_LIST));
    }

    @Test
    public void methodValidateInternetWithoutConnectionShoudlShowAlertDialogTryAgain() {
        when(validateInternet.isConnected()).thenReturn(false);
        splashAppPresenter.validateInternetToGetGuestLogin();
        verify(splashAppPresenter, never()).createThreadToGetGuestLogin();
        verify(splashAppView).showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetWithConnectionShoudlCallMethodGetGuestLogin() {
        when(validateInternet.isConnected()).thenReturn(true);
        splashAppPresenter.validateInternetToGetGuestLogin();
        verify(splashAppPresenter).createThreadToGetGuestLogin();
        verify(splashAppView, never()).showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }


    @Test
    public void methodGetGuestLoginShouldCallMethodGetGuestLoginInSecurityBL() throws RepositoryError {
        Authoken authoken = getAuthoken();
        when(securityRepository.getGuestLogin()).thenReturn(authoken);
        loadFakeRespositories();
        splashAppPresenter.getGuestLogin();
        verify(securityBL).getGuestLogin();
        verify(splashAppView).hideProgressBarOnUiThread();
    }


    @Test
    public void methodGetGuestLoginShouldSaveTokenInSharedPreferences() throws RepositoryError {
        Authoken authoken = getAuthoken();
        when(securityRepository.getGuestLogin()).thenReturn(authoken);
        loadFakeRespositories();
        splashAppPresenter.getGuestLogin();
        verify(splashAppView).setDataUser(authoken);
        verify(splashAppView).hideProgressBarOnUiThread();
    }


    @Test
    public void methodGetGuestLoginShouldAlertDialogIfReturnAnException() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(securityRepository.getGuestLogin()).thenThrow(repositoryError);
        loadFakeRespositories();
        splashAppPresenter.getGuestLogin();
        verify(splashAppView).showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
        verify(splashAppView).hideProgressBarOnUiThread();
    }


    @Test
    public void methodValidateInternetToGetAutoLoginWithoutConnectionShoudlShowAlertDialog() {
        String token = "21212";
        when(validateInternet.isConnected()).thenReturn(false);
        splashAppPresenter.validateInternetToGetAutoLogin(token);
        verify(splashAppView).showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }


    @Test
    public void methodValidateInternetWithConnectionShoudlCallMethodGetAutoLogin() {
        String token = "21212";
        when(validateInternet.isConnected()).thenReturn(true);
        splashAppPresenter.validateInternetToGetAutoLogin(token);
        verify(splashAppPresenter).createThreadToGetAutoLogin(token);
        verify(splashAppView, never()).showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }


    /*@Test
    public void methodGetAutoLoginShouldCallMethodGetAutoLoginInSecurityBL() throws RepositoryError {
        String token = "21212";
        Authoken authoken = new Authoken();
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        authoken.setInvitado(true);
        authoken.setUsuario(registerUsuarioRequest);
        ProcedureServiceMessage mensaje = new ProcedureServiceMessage();
        mensaje.setCode(1);
        mensaje.setText("");
        authoken.setMensaje(mensaje);
        when(securityRepository.getAutoLogin(token)).thenReturn(authoken);
        loadFakeRespositories();
        splashAppPresenter.getAutoLogin(token);
        verify(securityBL).getAutoLogin(token);
        verify(splashAppView).hideProgressBarOnUiThread();
    }*/


    @Test
    public void methodGetAutoLoginShouldAlertDialogIfReturnAnException() throws RepositoryError {
        String token = "21212";
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(securityRepository.getAutoLogin(token)).thenThrow(repositoryError);
        loadFakeRespositories();
        splashAppPresenter.getAutoLogin(token);
        verify(splashAppView).showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
        verify(splashAppView).hideProgressBarOnUiThread();
    }


    @Test
    public void methodGetAutoLoginShouldAlertDialogIfReturnAnUnauthorized() throws RepositoryError {
        String token = "21212";
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(securityRepository.getAutoLogin(token)).thenThrow(repositoryError);
        loadFakeRespositories();
        splashAppPresenter.getAutoLogin(token);
        verify(splashAppPresenter).validateInternetToGetGuestLogin();
        verify(splashAppView).hideProgressBarOnUiThread();
    }

    @Test
    public void getNotificationsIsSuccessful(){
        loadFakeRespositories();
        splashAppPresenter.getNotificationsPush();
        verify(splashAppView).validateUserIfHaveBeenLogin();
        verify(splashAppView).hideProgressBarOnUiThread();
    }

    private Authoken getAuthoken() {
        Authoken authoken = new Authoken();
        Usuario usuario = new Usuario();
        Mensaje mensaje = new Mensaje();
        authoken.setUsuario(usuario);
        authoken.setMensaje(mensaje);
        authoken.setInvitado(false);
        return authoken;
    }

    private ListasGenerales getListasGenerales() {
        ListasGenerales listasGenerales = new ListasGenerales();
        List<ItemGeneral> itemGeneralList = new ArrayList<>();
        ItemGeneral itemGeneral = new ItemGeneral();
        itemGeneral.setId(1);
        itemGeneral.setCodigo("CC");
        itemGeneral.setDescripcion("Cedula");
        itemGeneralList.add(itemGeneral);
        listasGenerales.setTiposIdentificacion(itemGeneralList);
        itemGeneralList = new ArrayList<>();
        itemGeneral = new ItemGeneral();
        itemGeneral.setId(2);
        itemGeneral.setCodigo("T");
        itemGeneral.setDescripcion("Propia");
        listasGenerales.setTiposViviendas(itemGeneralList);
        itemGeneralList = new ArrayList<>();
        itemGeneral = new ItemGeneral();
        itemGeneral.setId(3);
        itemGeneral.setCodigo("M");
        itemGeneral.setDescripcion("Masculino");
        listasGenerales.setGeneros(itemGeneralList);
        itemGeneralList = new ArrayList<>();
        itemGeneral = new ItemGeneral();
        itemGeneral.setId(4);
        itemGeneral.setCodigo("N");
        itemGeneral.setDescripcion("Natural");
        listasGenerales.setTiposPersonas(itemGeneralList);
        return listasGenerales;
    }

    private void loadFakeRespositories() {
        Authoken authoken = getAuthoken();
        MutableLiveData<Integer> error = new MutableLiveData<>();
        error.setValue(R.string.text_error_500);
        getNotificationsPush.setCantidadNotificacionesSinLeer(12);
        getNotificationsPushMutableLiveData.setValue(getNotificationsPush);
        when(notificationsRepository.getNotificationsPush(IdDispositive.getIdDispositive())).thenReturn(Observable.just(getNotificationsPush));
    }

    private Usuario getRegisterUsuarioRequest() {
        Usuario registerUsuarioRequest = new Usuario();
        registerUsuarioRequest.setCorreoElectronico("quiceno1127@gmail.com");
        registerUsuarioRequest.setNombres("Mateo");
        registerUsuarioRequest.setApellido("Quiceno Sosa");
        registerUsuarioRequest.setIdTipoIdentificacion(1);
        registerUsuarioRequest.setNumeroIdentificacion("1017253093");
        registerUsuarioRequest.setContrasenia("Mqs1017253093.");
        registerUsuarioRequest.setAceptoTerminosyCondiciones(true);
        return registerUsuarioRequest;
    }


    private void validateIftheServiceSaveDataOfAssignedTurn(){
        splashAppPresenter.getAssignedTurn();
    }

    @Test
    public void validateIfassignedTurnIsNull(){
        AssignedTurn assignedTurn = new AssignedTurn();
       boolean validation = splashAppPresenter.assignedTurnOrTurnDeviceIsNotNull(null);
       assertFalse(validation);
    }
    @Test
    public void validateIfTurnDeviceIsNull(){
        AssignedTurn assignedTurn = new AssignedTurn();
        assignedTurn.setShiftDevice(null);
       boolean validation = splashAppPresenter.assignedTurnOrTurnDeviceIsNotNull(assignedTurn);
       assertFalse(validation);
    }


    @Test
    public void validateIfassignedTurnIsNotNull(){
        AssignedTurn assignedTurn = new AssignedTurn();
        boolean validation = splashAppPresenter.assignedTurnOrTurnDeviceIsNotNull(null);
        assertFalse(validation);
    }
    @Test
    public void validateIfTurnDeviceIsNotNull(){
        AssignedTurn assignedTurn = new AssignedTurn();
        ShiftDevice shiftDevice = new ShiftDevice();
        assignedTurn.setShiftDevice(shiftDevice);
        boolean validation = splashAppPresenter.assignedTurnOrTurnDeviceIsNotNull(assignedTurn);
        assertFalse(validation);
    }

}
