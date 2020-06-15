package com.epm.app.presenters;

import android.content.DialogInterface;
import android.os.Build;

import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.R;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.container_domain.business_models.InformacionEspacioPromocional;
import app.epm.com.container_domain.container.ContainerBusinessLogic;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.comunidad.bussinesslogic.INotificationsBL;
import com.epm.app.mvvm.comunidad.bussinesslogic.ISubscriptionBL;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPush;
import com.epm.app.mvvm.comunidad.network.response.places.ListSubscriptions;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.SolicitudActualizarEstadoSuscripcion;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.view.views_activities.ILandingView;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created by mateoquicenososa on 30/11/16.
 */

public class LandingPresenter extends BasePresenter<ILandingView> {

    private ContainerBusinessLogic containerBusinessLogic;
    private ICustomSharedPreferences customSharedPreferences;
    private ISubscriptionBL subscriptionBL;
    private INotificationsBL notificationsBL;

    /**
     * Constructor
     *
     * @param containerBusinessLogic Clase con lógica de negocio.
     */
    public LandingPresenter(ContainerBusinessLogic containerBusinessLogic, SubscriptionRepository subscriptionRepository) {
        this.containerBusinessLogic = containerBusinessLogic;
        this.subscriptionBL = subscriptionRepository;
    }


    public void inject(ILandingView iLandingView, IValidateInternet validateInternet, ICustomSharedPreferences customSharedPreferences, NotificationsRepository notificationsRepository) {
        setView(iLandingView);
        setValidateInternet(validateInternet);
        this.customSharedPreferences = customSharedPreferences;
        this.notificationsBL = notificationsRepository;

    }

    /**
     * Valida conexión a internet Ingresar como invitado.
     */

    public void validateInternetToGetGuestLogin() {
        if (getValidateInternet().isConnected()) {
            createThreadToGetGuestLogin();
        } else {
            getView().showAlertDialogTryAgain(R.string.title_appreciated_user, R.string.text_validate_internet, R.string.text_intentar, R.string.text_abandonar, true);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio.
     */
    public void createThreadToGetGuestLogin() {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {
                getGuestLogin();
            }
        });
        thread.start();
    }

    /**
     * LLama a la clase del negocio para comunicarse con el servicio Ingresar como invitado.
     */
    public void getGuestLogin() {
        try {
            Authoken authoken = containerBusinessLogic.getGuestLogin();
            getView().setDataUser(authoken);

        } catch (RepositoryError repositoryError) {
            getView().showAlertDialogTryAgain(R.string.title_appreciated_user, repositoryError.getMessage(), R.string.text_intentar, R.string.text_abandonar, true);
        } finally {
            getView().dismissProgressDialog();
        }
    }



   /* *//**
     * Valida conexión a internet AutoLogin.
     *
     * @param token token.
     *//*
    public void validateInternetToGetAutoLogin(final String token) {
        if (getValidateInternet().isConnected()) {
            createThreadToGetAutoLogin(token);
        } else {
            getView().showAlertDialogTryAgain(R.string.title_appreciated_user, R.string.text_validate_internet, R.string.text_intentar, R.string.text_abandonar, false);
        }
    }

    *//**
     * Crea hilo para realizar petición al servicio AutoLogin
     *
     * @param token token.
     *//*
    public void createThreadToGetAutoLogin(final String token) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {
                getAutoLogin(token);
            }
        });
        thread.start();
    }

    *//**
     * Llama a la clase del negocio para comunicarse con el servicio AutoLogin.
     *
     //* @param token token.
     *//*
    public void getAutoLogin(String token) {
        try {
            Authoken authoken = containerBusinessLogic.getAutoLogin(token);
            getView().setDataUser(authoken);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                validateInternetToGetGuestLogin();
            } else {
                getView().showAlertDialogTryAgain(R.string.title_appreciated_user, repositoryError.getMessage(), R.string.text_intentar, R.string.text_abandonar, false);
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }*/

    public void validateInternetToGetEspacioPromocional() {
        if (getValidateInternet().isConnected()) {
            createThreadToGetEspacioPromocional();
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    public void createThreadToGetEspacioPromocional() {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {
                getEspacioPromocional();
            }
        });
        thread.start();
    }

    public void getEspacioPromocional() {
        try {
            validateDateToStartEspacioPromocional(containerBusinessLogic.getEspacioPromocional());
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                validateInternetToGetGuestLogin();
            } else if (repositoryError.getIdError() != Constants.NOT_FOUND_ERROR_CODE) {
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }

    public void validateDateToStartEspacioPromocional(InformacionEspacioPromocional espacioPromocional) {
        DateFormat dateFormat = new SimpleDateFormat(Constants.FORMAT_DATE_YMDHMS);
        Date dateService = espacioPromocional.getFechaCreacion();
        String date = dateFormat.format(dateService);
        if (customSharedPreferences.getString(Constants.DATE_PROMOTIONAL_SPACE) == null) {
            addValuesToTheSharedPreferences(date, espacioPromocional);
        } else if (!customSharedPreferences.getString(Constants.DATE_PROMOTIONAL_SPACE).equals(date)) {
            addValuesToTheSharedPreferences(date, espacioPromocional);
        } else if (compareCurrentDate() != null) {
            validateDateToOpenPromotionalSpaceForTheSecondTime(date, espacioPromocional);
        }
    }

    public String compareCurrentDate() {
        if (customSharedPreferences.getString(Constants.DATE_WITHOUT_HOURS) != null) {
            String dateOpenActivity = customSharedPreferences.getString(Constants.DATE_WITHOUT_HOURS);
            dateOpenActivity = dateOpenActivity.replace("-", Constants.EMPTY_STRING);
            return dateOpenActivity;
        }
        return null;
    }

    public void addValuesToTheSharedPreferences(String date, InformacionEspacioPromocional informacionEspacioPromocional) {
        customSharedPreferences.addString(Constants.DATE_PROMOTIONAL_SPACE, date);
        customSharedPreferences.addInt(Constants.OPEN_PROMOTIONAL_SPACE, Constants.ONE);
        getView().startEspacioPromocional(informacionEspacioPromocional);
    }

    public void validateDateToOpenPromotionalSpaceForTheSecondTime(String date, InformacionEspacioPromocional informacionEspacioPromocional) {
        String currentDate = new SimpleDateFormat(Constants.FORMATYMD).format(new Date());
        currentDate = currentDate.replace("-", Constants.EMPTY_STRING);
        if (customSharedPreferences.getInt(Constants.OPEN_PROMOTIONAL_SPACE) == Constants.ONE && Long.valueOf(currentDate) > Long.valueOf(compareCurrentDate())) {
            addValuesToTheSharedPreferences(date, informacionEspacioPromocional);
        } else if (customSharedPreferences.getInt(Constants.OPEN_PROMOTIONAL_SPACE) == Constants.ONE) {
            customSharedPreferences.addInt(Constants.OPEN_PROMOTIONAL_SPACE, Constants.TWO);
            getView().startEspacioPromocional(informacionEspacioPromocional);
        } else if (customSharedPreferences.getInt(Constants.OPEN_PROMOTIONAL_SPACE) == Constants.TWO) {
            validateDateAfterToSevenDays(date, informacionEspacioPromocional);
        }
    }

    public void validateDateAfterToSevenDays(String date, InformacionEspacioPromocional informacionEspacioPromocional) {
        String currentDate = new SimpleDateFormat(Constants.FORMAT_DATE_YMDHMS).format(new Date());
        currentDate = currentDate.replace("-", Constants.EMPTY_STRING);
        String dateActivityPromotionalSpace = customSharedPreferences.getString(Constants.DATE_ACTIVITY_PROMOTIONAL_SPACE);
        if (dateActivityPromotionalSpace != null) {
            dateActivityPromotionalSpace = dateActivityPromotionalSpace.replace("-", Constants.EMPTY_STRING);
            if (Long.valueOf(currentDate) > Long.valueOf(dateActivityPromotionalSpace)) {
                addValuesToTheSharedPreferences(date, informacionEspacioPromocional);
            }
        }
    }

    public void getSubscription(Usuario usuario){
        if(usuario != null){
            showErrorSubscription();
            subscriptionBL.getListSubscriptionAlertas(customSharedPreferences.getString(Constants.TOKEN), IdDispositive.getIdDispositive(),
                    Constants.TYPE_SUSCRIPTION_ALERTAS,customSharedPreferences.getString(Constants.ONE_SIGNAL_ID)).observeForever(subscription -> {
                if(subscription.getSuscripciones() != null && subscription.getSuscripciones().size() > 0){
                    int contListSubscription = 0;
                    for (ListSubscriptions getSubscriptionNotificationsPush : subscription.getSuscripciones()) {
                        if(getSubscriptionNotificationsPush.getIdTipoSuscripcionNotificacion() ==  Constants.TYPE_SUSCRIPTION_ALERTAS){
                            customSharedPreferences.addString(Constants.SUSCRIPTION_ALERTAS, Constants.TRUE);
                            updateSubscription(subscription,contListSubscription);
                            break;
                        }
                        contListSubscription = contListSubscription + 1;
                    }
                }else {
                    customSharedPreferences.addString(Constants.SUSCRIPTION_ALERTAS,Constants.FALSE);
                }
            });
        }
    }

    public void updateSubscription(GetSubscriptionNotificationsPush getSubscriptionNotifications,int numList){
        SolicitudActualizarEstadoSuscripcion solicitudActualizar = new SolicitudActualizarEstadoSuscripcion();
        System.out.println("customSharedPreferences "+customSharedPreferences);
        System.out.println("customSharedPreferences.getString(Constants.ONE_SIGNAL_ID) "+customSharedPreferences.getString(Constants.ONE_SIGNAL_ID));
        System.out.println("getSubscriptionNotifications.getIdSuscripcionOneSignal() "+getSubscriptionNotifications.getIdSuscripcionOneSignal());
         if(!customSharedPreferences.getString(Constants.ONE_SIGNAL_ID).equals(getSubscriptionNotifications.getIdSuscripcionOneSignal())){
            solicitudActualizar.setEnvioNotificacion(getSubscriptionNotifications.getSuscripciones().get(numList).getEnvioNotificacion());
            solicitudActualizar.setCorreoElectronico(getSubscriptionNotifications.getSuscripciones().get(numList).getCorreoElectronico());
            solicitudActualizar.setIdDispositivo(IdDispositive.getIdDispositive());
            solicitudActualizar.setIdSuscripcionOneSignal(customSharedPreferences.getString(Constants.ONE_SIGNAL_ID));
            solicitudActualizar.setIdTipoSuscripcionNotificacion(getSubscriptionNotifications.getSuscripciones().get(numList).getIdTipoSuscripcionNotificacion());
            solicitudActualizar.setCambioIdDispositivo(getSubscriptionNotifications.isCambioIdDispositivo());
            solicitudActualizar.setCambioIdOneSignal(getSubscriptionNotifications.isCambioIdOneSignal());
            subscriptionBL.updateSubscriptionStatus(customSharedPreferences.getString(Constants.TOKEN),solicitudActualizar).observeForever(subscription -> getView().dismissProgressDialog());
        }


    }

    private void showErrorSubscription(){
        subscriptionBL.showError().observeForever(integer -> {
            getView().dismissProgressDialog();
        });
    }


    public boolean checkExecutionInAndroidApiActualVersion(){
        int apiLevelVersion = Build.VERSION.SDK_INT;
        boolean execute = true;
        if (apiLevelVersion < 21) {
            execute = false;
        }
        return execute;
    }

    public void showDialogToUserWithApiLevelToLower(){
        getView().showAlertDialogGeneralInformationOnUiThread(Constants.TITTLE_MESSAGE_APILEVEL_TOOLOW,Constants.BODY_MESSAGE_APILEVEL_TOOLOW);
    }
}