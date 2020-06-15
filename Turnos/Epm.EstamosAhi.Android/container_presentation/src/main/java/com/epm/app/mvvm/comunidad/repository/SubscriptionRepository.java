package com.epm.app.mvvm.comunidad.repository;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.bussinesslogic.ISubscriptionBL;
import com.epm.app.mvvm.comunidad.network.SuscriptionServices;
import com.epm.app.mvvm.comunidad.network.request.CancelSubscriptionRequest;
import com.epm.app.mvvm.comunidad.network.request.UpdateSubscriptionByMailRequest;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.CancelSubscriptionResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetDetailRedAlertResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPush;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPushAlertasItuango;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.RecoverySubscriptionResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.RequestUpdateSubscription;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.Subscription;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.SolicitudActualizarEstadoSuscripcion;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.SolicitudSuscripcionNotificacionPush;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.UpdateSubscription;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateServiceCode;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.DisposableManager;
import io.reactivex.Observable;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.Disposable;
import io.reactivex.schedulers.Schedulers;

public class SubscriptionRepository implements ISubscriptionBL {

    private SuscriptionServices webService;
    private IValidateInternet validateInternet;
    private MutableLiveData<ErrorMessage> error;

    @Inject
    public SubscriptionRepository(SuscriptionServices webService, ValidateInternet validateInternet) {
        this.webService = webService;
        this.validateInternet = validateInternet;
        error = new MutableLiveData<>();
    }

    @Override
    public MutableLiveData<GetSubscriptionNotificationsPushAlertasItuango> getSubscriptionAlertasItuango(String token, String idDispositive) {
        MutableLiveData<GetSubscriptionNotificationsPushAlertasItuango> subscriptionNotificationsMutableLiveData = new MutableLiveData<>();
        if (validateInternet.isConnected()) {
            GetSubscriptionNotificationsPushAlertasItuango subscriptionNotifications = new GetSubscriptionNotificationsPushAlertasItuango();
            Disposable disposable = webService.getSuscriptionNotificationsPushAlertasItuango(idDispositive, token)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(response -> {
                        ValidateServiceCode.setTitleError404(R.string.title_error);
                        ValidateServiceCode.setError404(R.string.text_error_500);
                        if (ValidateServiceCode.captureServiceErrorCode(response.code())) {
                            if (response.isSuccessful()) {
                                if (response.body() != null) {
                                    subscriptionNotifications.setSuscripcionNotificacionesPushComunidad(response.body().getSuscripcionNotificacionesPushComunidad());
                                    subscriptionNotifications.setMensaje(response.body().getMensaje());
                                    subscriptionNotificationsMutableLiveData.postValue(subscriptionNotifications);
                                }
                            }

                        } else {
                            setError(ValidateServiceCode.getTitleError(),ValidateServiceCode.getError());
                        }

                    }, throwable -> {
                        setError(R.string.title_error,R.string.text_error_500);
                    });
            DisposableManager.add(disposable);
        } else {
            errorInternet();
        }
        return subscriptionNotificationsMutableLiveData;
    }

    private void setError(int title, int message) {
        error.setValue(new ErrorMessage(title,message));
    }


    @Override
    public MutableLiveData<GetSubscriptionNotificationsPush> getListSubscriptionAlertas(String token, String idDispositive, int idAplication,String idSuscriptionOneSignal) {
        MutableLiveData<GetSubscriptionNotificationsPush> subscriptionNotificationsMutableLiveData = new MutableLiveData<>();
        if (validateInternet.isConnected()) {
            Disposable disposable = webService.getSuscriptionNotificationsPush(idDispositive, idAplication,idSuscriptionOneSignal, token)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(response -> {
                        if (ValidateServiceCode.captureServiceErrorCode(response.code())) {
                            if (response.isSuccessful()) {
                                subscriptionNotificationsMutableLiveData.postValue(response.body());
                            }
                        } else {
                            setError(ValidateServiceCode.getTitleError(),ValidateServiceCode.getError());
                        }
                    }, throwable -> {
                        setError(R.string.title_error,R.string.text_error_500);
                    });
            DisposableManager.add(disposable);
        } else {
            errorInternet();
        }
        return subscriptionNotificationsMutableLiveData;
    }

    @Override
    public MutableLiveData<Subscription> saveSubscription(String token, SolicitudSuscripcionNotificacionPush solicitudSuscripcionNotificacionPush) {
        final MutableLiveData<Subscription> saveSuscriptionMutableLiveData = new MutableLiveData<>();
        if (validateInternet.isConnected()) {
            Subscription subscription = new Subscription();
            Disposable disposable = webService.saveSuscriptionAlertas(token, solicitudSuscripcionNotificacionPush)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(response -> {
                        ValidateServiceCode.setTitleError404(R.string.title_error);
                        ValidateServiceCode.setError404(R.string.text_error_500);
                        if (ValidateServiceCode.captureServiceErrorCode(response.code())) {
                            if (response.isSuccessful()) {
                                if (response.body() != null) {
                                    subscription.setStateTransaction(response.body().stateTransaction);
                                    subscription.setMensaje(response.body().mensaje);
                                    saveSuscriptionMutableLiveData.postValue(subscription);
                                }
                            }

                        } else {
                            setError(ValidateServiceCode.getTitleError(),ValidateServiceCode.getError());
                        }
                    }, throwable -> {
                        setError(R.string.title_error,R.string.text_error_500);
                    });
            DisposableManager.add(disposable);
        } else {
            errorInternet();
        }
        return saveSuscriptionMutableLiveData;
    }

    /**
     * Actualiza el one signal.
     */

    @Override
    public MutableLiveData<Subscription> updateSubscriptionStatus(String token, SolicitudActualizarEstadoSuscripcion solicitudActualizarEstadoSuscripcion) {
        MutableLiveData<Subscription> subscriptionMutableLiveData = new MutableLiveData<>();
        if (validateInternet.isConnected()) {
            Subscription subscription = new Subscription();
            Disposable disposable = webService.updateSuscriptionAlertasState(token, solicitudActualizarEstadoSuscripcion)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(response -> {
                        if (ValidateServiceCode.captureServiceErrorCode(response.code())) {
                            if (response.isSuccessful()) {
                                if (response.body() != null) {
                                    subscription.setStateTransaction(response.body().stateTransaction);
                                    subscription.setMensaje(response.body().mensaje);
                                    subscriptionMutableLiveData.postValue(subscription);
                                }
                            }
                        }
                    }, throwable -> {
                        setError(R.string.title_error,R.string.text_error_500);
                    });
            DisposableManager.add(disposable);
        } else {
            errorInternet();
        }
        return subscriptionMutableLiveData;
    }

    /**
     * Actualiza la suscripcion de alertas ituango.
     */

    @Override
    public Observable<UpdateSubscription> updateSubscription(String token, RequestUpdateSubscription requestUpdateSubscription) {
        return webService.updateSuscriptionAlertas(token,requestUpdateSubscription);
    }

    @Override
    public Observable<CancelSubscriptionResponse> cancelSubscription(String token, CancelSubscriptionRequest cancelSubscriptionRequest) {
        return webService.cancelSubscription(token,cancelSubscriptionRequest);
    }

    @Override
    public MutableLiveData<RecoverySubscriptionResponse> getSubscriptionDeviceAlertsByEmail(String email, String token) {
        MutableLiveData<RecoverySubscriptionResponse> recoverySubscriptionResponseMutableLiveData = new MutableLiveData<>();
        if (validateInternet.isConnected()) {
            Disposable disposable = webService.getSubscriptionDeviceAlertsByEmail(email, token)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(response -> {
                        if (ValidateServiceCode.captureServiceErrorCode(response.code())) {
                            if (response.isSuccessful()) {
                                if (response.body() != null) {
                                    recoverySubscriptionResponseMutableLiveData.postValue(response.body());
                                }
                            }
                        } else {
                            setError(ValidateServiceCode.getTitleError(),ValidateServiceCode.getError());
                        }
                    }, throwable -> {
                        setError(R.string.title_error,R.string.text_error_500);
                    });
            DisposableManager.add(disposable);
        }else {
            errorInternet();
        }
        return recoverySubscriptionResponseMutableLiveData;
    }

    @Override
    public MutableLiveData<NotificationResponse> updateSubscriptionDeviceAlertsByEmail(UpdateSubscriptionByMailRequest updateSubscriptionByMailRequest, String token) {
        MutableLiveData<NotificationResponse> updateSubscriptionDeviceAlertsByEmailMutableLiveData = new MutableLiveData<>();
        if (validateInternet.isConnected()) {
            Disposable disposable = webService.updateSubscriptionByMail(token,updateSubscriptionByMailRequest)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(response -> {
                        if (ValidateServiceCode.captureServiceErrorCode(response.code())) {
                            if (response.isSuccessful()) {
                                if (response.body() != null) {
                                    updateSubscriptionDeviceAlertsByEmailMutableLiveData.postValue(response.body());
                                }
                            }
                        } else {
                            setError(ValidateServiceCode.getTitleError(),ValidateServiceCode.getError());
                        }
                    }, throwable -> {
                        setError(R.string.title_error,R.string.text_error_500);
                    });
            DisposableManager.add(disposable);
        }else {
            errorInternet();
        }
        return updateSubscriptionDeviceAlertsByEmailMutableLiveData;
    }

    @Override
    public MutableLiveData<GetDetailRedAlertResponse> getDetailRedAlert(String idDevice, String token) {
        MutableLiveData<GetDetailRedAlertResponse> updateSubscriptionDeviceAlertsByEmailMutableLiveData = new MutableLiveData<>();
        if (validateInternet.isConnected()) {
            Disposable disposable = webService.getDetailRedAlert(idDevice,token)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(response -> {
                        if (ValidateServiceCode.captureServiceErrorCode(response.code())) {
                            if (response.isSuccessful()) {
                                if (response.body() != null) {
                                    updateSubscriptionDeviceAlertsByEmailMutableLiveData.postValue(response.body());
                                }
                            }
                        } else {
                            setError(ValidateServiceCode.getTitleError(),ValidateServiceCode.getError());
                        }
                    }, throwable -> {
                        setError(R.string.title_error,R.string.text_error_500);
                    });
            DisposableManager.add(disposable);
        }else {
            errorInternet();
        }
        return updateSubscriptionDeviceAlertsByEmailMutableLiveData;
    }



    private void errorInternet() {
        setError(R.string.title_appreciated_user,R.string.text_validate_internet);
    }

    @Override
    public MutableLiveData<ErrorMessage> showError() {
        return error;
    }




}
