package com.epm.app.container;

import com.epm.app.R;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPush;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import com.epm.app.presenters.LandingPresenter;
import com.epm.app.view.views_activities.ILandingView;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnit;
import org.mockito.junit.MockitoJUnitRunner;
import org.mockito.junit.MockitoRule;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.container_domain.business_models.InformacionEspacioPromocional;
import app.epm.com.container_domain.container.ContainerBusinessLogic;
import app.epm.com.container_domain.container.IContainerRepository;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by mateoquicenososa on 7/12/16.
 */

@RunWith(MockitoJUnitRunner.class)
public class LandingPresenterTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();


    @Mock
    ILandingView landingView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    IContainerRepository containerRepository;

    @Mock
    ICustomSharedPreferences customSharedPreferences;

    @Mock
    NotificationsRepository notificationsRepository;


    @InjectMocks
    SubscriptionRepository subscriptionRepository;

    LandingPresenter landingPresenter;


    ContainerBusinessLogic containerBusinessLogic;

    private String dateOpenActivity = "test";



    private Authoken getAuthoken() {
        Authoken authoken = new Authoken();
        Usuario usuario = new Usuario();
        Mensaje mensaje = new Mensaje();
        authoken.setUsuario(usuario);
        authoken.setMensaje(mensaje);
        authoken.setInvitado(false);
        return authoken;
    }
    @Before
    public void setUp() throws Exception {
        notificationsRepository = mock(NotificationsRepository.class);
        containerBusinessLogic = Mockito.spy(new ContainerBusinessLogic(containerRepository));
        landingPresenter = Mockito.spy(new LandingPresenter(containerBusinessLogic,subscriptionRepository));
        landingPresenter.inject(landingView, validateInternet, customSharedPreferences,notificationsRepository);


    }

    @Test
    public void methodValidateInternetWithoutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        landingPresenter.validateInternetToGetGuestLogin();
        verify(landingPresenter, never()).createThreadToGetGuestLogin();
        verify(landingView).showAlertDialogTryAgain(R.string.title_appreciated_user, R.string.text_validate_internet, R.string.text_intentar, R.string.text_abandonar, true);
    }

    @Test
    public void methodValidateInternetWithConnectionShoudlCallMethodGetGuestLogin() {
        when(validateInternet.isConnected()).thenReturn(true);
        landingPresenter.validateInternetToGetGuestLogin();
        verify(landingPresenter).createThreadToGetGuestLogin();
        verify(landingView, never()).showAlertDialogTryAgain(R.string.title_appreciated_user, R.string.text_validate_internet, R.string.text_intentar, R.string.text_abandonar, true);
    }

    @Test
    public void methodGetGuestLoginShouldCallMethodGetGuestLoginInSecurityBL() throws RepositoryError {
        Authoken authoken = getAuthoken();
        when(containerRepository.getGuestLogin()).thenReturn(authoken);
        landingPresenter.getGuestLogin();
        verify(containerBusinessLogic).getGuestLogin();
        verify(landingView).dismissProgressDialog();
    }

    @Test
    public void methodGetGuestLoginShouldSaveTokenInSharedPreferences() throws RepositoryError {
        Authoken authoken = getAuthoken();
        when(containerRepository.getGuestLogin()).thenReturn(authoken);
        landingPresenter.getGuestLogin();
        verify(landingView).setDataUser(authoken);
        verify(landingView).dismissProgressDialog();
    }

    @Test
    public void methodGetGuestLoginShouldAlertDialogIfReturnAnException() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(containerRepository.getGuestLogin()).thenThrow(repositoryError);
        landingPresenter.getGuestLogin();
        verify(landingView).showAlertDialogTryAgain(R.string.title_appreciated_user, repositoryError.getMessage(), R.string.text_intentar, R.string.text_abandonar, true);
        verify(landingView).dismissProgressDialog();
    }


    private InformacionEspacioPromocional getInformacionEspacioPromocional() {
        InformacionEspacioPromocional informacionEspacioPromocional = new InformacionEspacioPromocional();
        informacionEspacioPromocional.setModulo(1);
        informacionEspacioPromocional.setImagen("test");
        Date date = new Date();
        informacionEspacioPromocional.setFechaCreacion(date);
        return informacionEspacioPromocional;
    }

    @Test
    public void methodValidateInternetToGetEspacioPromocionalWithoutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        landingPresenter.validateInternetToGetEspacioPromocional();
        verify(landingView).showAlertDialogGeneralInformationOnUiThread(landingView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetToGetEspacioPromocionalWithConnectionShoudlCallMethodCreateThreadToGetEspacioPromocional() {
        when(validateInternet.isConnected()).thenReturn(true);
        landingPresenter.validateInternetToGetEspacioPromocional();
        verify(landingPresenter).createThreadToGetEspacioPromocional();
    }

    @Test
    public void methodCreateThreadToGetEspacioPromocionalShouldCallShowProgressDialog() {
        landingPresenter.createThreadToGetEspacioPromocional();
        verify(landingView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodGetEspacioPromocionalShouldCallGetEspacioPromocionalInBusinessLogic() throws RepositoryError {
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        when(containerRepository.getEspacioPromocional()).thenReturn(informacionEspacioPromocional);
        landingPresenter.getEspacioPromocional();
        verify(containerBusinessLogic).getEspacioPromocional();
        verify(landingView).dismissProgressDialog();
    }

    @Test
    public void methoGetEspacioPromocionalShouldAlertDialogIfReturnAnException() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(containerRepository.getEspacioPromocional()).thenThrow(repositoryError);
        landingPresenter.getEspacioPromocional();
        verify(landingView).showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
        verify(landingView).dismissProgressDialog();
    }

    @Test
    public void methodGetEspacioPromocionalShouldAlertDialogIfReturnAnUnauthorized() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(containerRepository.getEspacioPromocional()).thenThrow(repositoryError);
        landingPresenter.getEspacioPromocional();
        //verify(landingPresenter).validateInternetToGetGuestLogin();
        verify(landingView).dismissProgressDialog();
    }

    @Test
    public void methodvalidateDateToStartEspacioPromocionalShouldCallMethodCompareCurrentDate() {
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        landingPresenter.validateDateToStartEspacioPromocional(informacionEspacioPromocional);
        equals(informacionEspacioPromocional);
    }

    @Test
    public void methodValidateDateToStartEspacioPromocionalWithDatePromotionalSpaceNullShouldCallMethodAddValuesToTheSharedPreferences() {
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        DateFormat dateFormat = new SimpleDateFormat(Constants.FORMAT_DATE_YMDHMS);
        Date dateService = informacionEspacioPromocional.getFechaCreacion();
        String date = dateFormat.format(dateService);
        when(customSharedPreferences.getString(Constants.DATE_PROMOTIONAL_SPACE)).thenReturn(null);
        landingPresenter.validateDateToStartEspacioPromocional(informacionEspacioPromocional);
        verify(landingPresenter).addValuesToTheSharedPreferences(date, informacionEspacioPromocional);
    }

    @Test
    public void methodValidateDateToStartEspacioPromocionalWithDatePromotionalSpaceEqualsToDateShouldCallMethodAddValuesToTheSharedPreferences() {
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        DateFormat dateFormat = new SimpleDateFormat(Constants.FORMAT_DATE_YMDHMS);
        Date dateService = informacionEspacioPromocional.getFechaCreacion();
        String date = dateFormat.format(dateService);
        when(customSharedPreferences.getString(Constants.DATE_PROMOTIONAL_SPACE)).thenReturn("");
        landingPresenter.validateDateToStartEspacioPromocional(informacionEspacioPromocional);
        verify(landingPresenter).addValuesToTheSharedPreferences(date, informacionEspacioPromocional);
    }

    @Test
    public void methodValidateDateToStartEspacioPromocionalMethodValidateDateToOpenPromotionalSpaceForTheSecondTime() {
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        DateFormat dateFormat = new SimpleDateFormat(Constants.FORMAT_DATE_YMDHMS);
        Date dateService = informacionEspacioPromocional.getFechaCreacion();
        String date = dateFormat.format(dateService);
        when(customSharedPreferences.getString(Constants.DATE_PROMOTIONAL_SPACE)).thenReturn(date);
        when(landingPresenter.compareCurrentDate()).thenReturn("");
        landingPresenter.validateDateToStartEspacioPromocional(informacionEspacioPromocional);
        verify(landingPresenter).validateDateToOpenPromotionalSpaceForTheSecondTime(date, informacionEspacioPromocional);
    }

    @Test
    public void methodValidateDateToStartEspacioPromocionalWithDatePromotionalSpaceNoNullShouldCallMethodAddValuesToTheSharedPreferences() {
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        String date = "tests";
        landingPresenter.validateDateToStartEspacioPromocional(informacionEspacioPromocional);
        Assert.assertNotEquals(date, informacionEspacioPromocional.getFechaCreacion());
    }

    @Test
    public void methodCompareCurrentDateWithDateWithHoursNoNullShouldReplaceCharactersOfDateOpenActivity() {
        String dateWithoutHours = "test-test";
        when(customSharedPreferences.getString(Constants.DATE_WITHOUT_HOURS)).thenReturn(dateWithoutHours);
        landingPresenter.compareCurrentDate();
        equals(customSharedPreferences.getString(Constants.DATE_WITHOUT_HOURS));
    }

    @Test
    public void methodAddValuesToTheSharedPreferencesShouldCallMethodStartEspacioPromocionalInTheView() {
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        String date = "test";
        landingPresenter.addValuesToTheSharedPreferences(date, informacionEspacioPromocional);
        verify(landingView).startEspacioPromocional(informacionEspacioPromocional);
    }

    @Test
    public void validateDateToOpenPromotionalSpaceForTheSecondTimeShouldCallMethodAddValuesToTheSharedPreferences() {
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        DateFormat dateFormat = new SimpleDateFormat(Constants.FORMAT_DATE_YMDHMS);
        Date dateService = informacionEspacioPromocional.getFechaCreacion();
        String date = dateFormat.format(dateService);
        landingPresenter.validateDateToOpenPromotionalSpaceForTheSecondTime(date, informacionEspacioPromocional);
        Assert.assertNotEquals(date, informacionEspacioPromocional.getFechaCreacion());
    }

    @Test
    public void validateDateToOpenPromotionalSpaceForTheSecondTimeWithOpenPromotionalSpaceOneAndCurrentDateMajorToDateShouldCallMethodAddValuesToTheSharedPreferences() {
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        String date = "test";
        when(customSharedPreferences.getInt(Constants.OPEN_PROMOTIONAL_SPACE)).thenReturn(1);
        when(landingPresenter.compareCurrentDate()).thenReturn("20171127");
        landingPresenter.validateDateToOpenPromotionalSpaceForTheSecondTime(date, informacionEspacioPromocional);
        verify(landingPresenter).addValuesToTheSharedPreferences(date, informacionEspacioPromocional);
    }

    @Test
    public void validateDateToOpenPromotionalSpaceForTheSecondTimeWithOpenPromotionalSpaceOneShouldCallMethodAddValuesToTheSharedPreferences() {
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        String date = "test";
        when(customSharedPreferences.getInt(Constants.OPEN_PROMOTIONAL_SPACE)).thenReturn(1);
        when(landingPresenter.compareCurrentDate()).thenReturn("20401127");
        landingPresenter.validateDateToOpenPromotionalSpaceForTheSecondTime(date, informacionEspacioPromocional);
        verify(landingView).startEspacioPromocional(informacionEspacioPromocional);
    }

    @Test
    public void validateDateToOpenPromotionalSpaceForTheSecondTimeWithOpenPromotionalSpaceTwoShouldCallMethodValidateDateAfterToSevenDays() {
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        String date = "test";
        when(customSharedPreferences.getInt(Constants.OPEN_PROMOTIONAL_SPACE)).thenReturn(2);
        when(customSharedPreferences.getString(Constants.DATE_ACTIVITY_PROMOTIONAL_SPACE)).thenReturn("2017-11-27-02-20-50");
        landingPresenter.validateDateToOpenPromotionalSpaceForTheSecondTime(date, informacionEspacioPromocional);
        verify(landingPresenter).validateDateAfterToSevenDays(date, informacionEspacioPromocional);
    }

    @Test
    public void validateDateAfterToSevenDaysShouldTryValuesToTheSharedPreferences() {
        String date = "testDate";
        InformacionEspacioPromocional informacionEspacioPromocional = getInformacionEspacioPromocional();
        when(customSharedPreferences.getString(Constants.DATE_ACTIVITY_PROMOTIONAL_SPACE)).thenReturn("2017-11-27-02-20-50");
        landingPresenter.validateDateAfterToSevenDays(date, informacionEspacioPromocional);
        verify(landingPresenter).addValuesToTheSharedPreferences(date, informacionEspacioPromocional);
    }

    @Test
    public void testGetSubscriptionValidateIfEmailIsNull(){
        Usuario usuario = null;
        String token = "123";
        String idDispositive = "123";
        String correoElectronico = "jlopezc@gmail.com";
        GetSubscriptionNotificationsPush subscription = new GetSubscriptionNotificationsPush();
        subscription.setIdDispositivo("");
        subscription.setIdUsuario("");
        subscription.setIdSuscripcionOneSignal("");
        subscription.setMensaje(new com.epm.app.mvvm.comunidad.network.response.Mensaje());
        subscription.setSuscripciones(new ArrayList<>());
        landingPresenter.getSubscription(usuario);
        verify(landingPresenter,times(0)).updateSubscription(subscription,0);
       // subscriptionRepository.getListSubscriptionAlertas(token,idDispositive,Constants.TYPE_SUSCRIPTION_ALERTAS,correoElectronico).observeForever(subscription->{
       // });

    }

    @Test
    public void testGetNotifications(){

    }

}