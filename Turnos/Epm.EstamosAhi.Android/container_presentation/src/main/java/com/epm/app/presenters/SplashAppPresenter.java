package com.epm.app.presenters;


import androidx.lifecycle.LifecycleOwner;
import androidx.lifecycle.Observer;
import androidx.annotation.Nullable;
import android.util.Log;

import com.epm.app.business_models.business_models.ListasGenerales;
import com.epm.app.business_models.business_models.RepositoryError;

import com.epm.app.R;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.container_domain.container.ContainerBusinessLogic;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;
import io.reactivex.Observable;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.disposables.Disposable;
import io.reactivex.observers.DisposableObserver;
import io.reactivex.schedulers.Schedulers;

import com.epm.app.mvvm.comunidad.bussinesslogic.INotificationsBL;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetNotificationsPush;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.epm.app.mvvm.turn.bussinesslogic.ITurnServicesBL;
import com.epm.app.mvvm.turn.network.response.AssignedTurn;
import com.epm.app.mvvm.turn.network.response.ShiftDevice;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.view.views_activities.ISplashAppView;

/**
 * Created by mateoquicenososa on 24/11/16.
 */

public class SplashAppPresenter extends BasePresenter<ISplashAppView> {

    private ContainerBusinessLogic containerBusinessLogic;
    private ICustomSharedPreferences customSharedPreferences;
    private INotificationsBL notificationsBL;
    private ITurnServicesBL turnServicesRepository;
    private final CompositeDisposable disposables = new CompositeDisposable();

    /**
     * Constructor
     *
     * @param containerBusinessLogic Clase con lógica de negocio.
     */
    public SplashAppPresenter(ContainerBusinessLogic containerBusinessLogic) {
        this.containerBusinessLogic = containerBusinessLogic;

    }

    public void inject(ISplashAppView splashAppView, IValidateInternet validateInternet, ICustomSharedPreferences customSharedPreferences, NotificationsRepository notificationsRepository, TurnServicesRepository turnServicesRepository) {
        setView(splashAppView);
        setValidateInternet(validateInternet);
        this.customSharedPreferences = customSharedPreferences;
        this.notificationsBL = notificationsRepository;
        this.turnServicesRepository = turnServicesRepository;
    }

    /**
     * Valida conexión a internet Ingresar como invitado.
     */
    public void validateInternetToGetGuestLogin() {
        if (getValidateInternet().isConnected()) {
            createThreadToGetGuestLogin();
        } else {
            getView().hideProgressBarOnUiThread();
            getView().showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio.
     */
    public void createThreadToGetGuestLogin() {
        Thread thread = new Thread(this::getGuestLogin);
        thread.start();
    }

    /**
     * LLama a la clase del negocio para comunicarse con el servicio Ingresar como invitado.
     */
    public void getGuestLogin() {
        try {
            Authoken authoken = containerBusinessLogic.getGuestLogin();
            getView().setDataUser(authoken);
            getAssignedTurn();
        } catch (RepositoryError repositoryError) {
            getView().hideProgressBarOnUiThread();
            getView().showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
        }
    }


    /**
     * Valida conexión a internet AutoLogin.
     *
     * @param token token.
     */
    public void validateInternetToGetAutoLogin(final String token) {
        if (getValidateInternet().isConnected()) {
            createThreadToGetAutoLogin(token);
        } else {
            getView().showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio AutoLogin
     *
     * @param token token.
     */
    public void createThreadToGetAutoLogin(final String token) {
        Thread thread = new Thread(() -> getAutoLogin(token));
        thread.start();
    }

    /**
     * Llama a la clase del negocio para comunicarse con el servicio AutoLogin.
     *
     * @param token token.
     */
    public void getAutoLogin(String token) {
        try {
            Authoken authoken = containerBusinessLogic.getAutoLogin(token);
            getView().setDataUser(authoken);
            getAssignedTurn();
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                validateInternetToGetGuestLogin();
            } else {
                getView().hideProgressBarOnUiThread();
                getView().showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
            }
        }
    }

    public void getAssignedTurn() {
        if(getValidateInternet().isConnected()) {
            Observable<AssignedTurn> result = turnServicesRepository.getAssignedTurn(customSharedPreferences.getString(Constants.TOKEN), IdDispositive.getIdDispositive());
            Disposable subscription = result.subscribeOn(Schedulers.io())
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableObserver<AssignedTurn>() {
                                       @Override
                                       public void onNext(AssignedTurn assignedTurn) {
                                           if (assignedTurnOrTurnDeviceIsNotNull(assignedTurn)) {
                                               customSharedPreferences.addString(Constants.ASSIGNED_TRUN, assignedTurn.getShiftDevice().getTurnoAsignado());
                                           }

                                       }

                                       @Override
                                       public void onError(Throwable e) {
                                           Log.e("errorgetAssignedTurn",e.getMessage());
                                           tryAgain();
                                       }

                                       @Override
                                       public void onComplete() {
                                           getNotificationsPush();
                                           Log.e("complete ", "complete");
                                       }
                                   });
            disposables.add(subscription);
        }
    }

    private void setError(Throwable e) {
        tryAgain();
        customSharedPreferences.deleteValue(Constants.ASSIGNED_TRUN);
    }

    public boolean assignedTurnOrTurnDeviceIsNotNull(AssignedTurn assignedTurn) {
        if (assignedTurn != null && assignedTurn.getShiftDevice() != null) {
            return !getAssignedTurnIsNullOrEmpty(assignedTurn.getShiftDevice());
        }else{
            customSharedPreferences.deleteValue(Constants.ASSIGNED_TRUN);
        }
        return false;
    }


    public boolean getAssignedTurnIsNullOrEmpty(ShiftDevice shiftDevice) {
        return shiftDevice.getTurnoAsignado() == null || shiftDevice.getTurnoAsignado().isEmpty();

    }


    /**
     * Valida la conexión a internet.
     */
    public void validateInternetToGetGeneralList() {
        if (getValidateInternet().isConnected()) {
            createThreadToGetGeneralList();
        } else {
            getView().showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio.
     */
    public void createThreadToGetGeneralList() {
        Thread thread = new Thread(this::getGeneralList);
        thread.start();
    }

    /**
     * Llama a la clase del negocio para comunicarse con el servicio.
     */
    public void getGeneralList() {
        try {
            ListasGenerales listasGenerales = containerBusinessLogic.getGeneralList();
            customSharedPreferences.addSetArray(Constants.TIPOS_DOCUMENTO_LIST, listasGenerales.getTiposIdentificacion());
            customSharedPreferences.addSetArray(Constants.TIPOS_PERSONA_LIST, listasGenerales.getTiposPersonas());
            customSharedPreferences.addSetArray(Constants.TIPOS_VIVIENDA_LIST, listasGenerales.getTiposViviendas());
            customSharedPreferences.addSetArray(Constants.GENEROS_LIST, listasGenerales.getGeneros());
            customSharedPreferences.addString(Constants.CURRENT_DATE_SAVED_LIST, getView().getCurrentDate());
            getView().validateUser();
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogToLoadAgainOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
            }
        }
    }

    public void getNotificationsPush() {
        Observable<GetNotificationsPush> result = notificationsBL.getNotificationsPush(IdDispositive.getIdDispositive());
        Disposable disposable = result.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<GetNotificationsPush>() {
                    @Override
                    public void onNext(GetNotificationsPush getNotificationsPush) {
                        validateResponseGetNotifications(getNotificationsPush);
                    }

                    @Override
                    public void onError(Throwable e) {
                        Log.e("errorNotificationsPush",e.getMessage());
                        tryAgain();
                    }

                    @Override
                    public void onComplete() {

                    }
                });
        DisposableManager.add(disposable);
    }

    public void validateResponseGetNotifications(GetNotificationsPush getNotificationsPush){
        if (getNotificationsPush != null && getNotificationsPush.getCantidadNotificacionesSinLeer() != null) {
            customSharedPreferences.addInt(Constants.NUMBER_NOTIFICATIONS, getNotificationsPush.getCantidadNotificacionesSinLeer());
            getView().validateUserIfHaveBeenLogin();
            getView().hideProgressBarOnUiThread();
        }
    }



    public void tryAgain() {
        getView().validateUserIfHaveBeenLogin();
        getView().hideProgressBarOnUiThread();
    }
}
