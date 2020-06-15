package com.epm.app.mvvm.comunidad.viewModel;

import android.arch.lifecycle.MutableLiveData;
import android.util.Log;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.bussinesslogic.INotificationsBL;
import com.epm.app.mvvm.comunidad.bussinesslogic.ISubscriptionBL;
import com.epm.app.mvvm.comunidad.network.request.UpdateStatusSendNotificationRequest;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.UpdateStatusSendNotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.places.ListSubscriptions;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.IConfigurationViewModel;
import com.epm.app.mvvm.turn.network.response.nearbyOffices.NearbyOfficesResponse;

import java.util.List;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;
import io.reactivex.Observable;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.Disposable;
import io.reactivex.observers.DisposableObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

public class ConfigurationNotificationViewModel extends BaseViewModel implements IConfigurationViewModel {

    private ISubscriptionBL subscriptionBL;
    private INotificationsBL notificationsBL;
    private ICustomSharedPreferences customSharedPreferences;
    private int messageError;
    private int titleError;
    private MutableLiveData<Boolean> error;
    private UpdateStatusSendNotificationRequest updateStatusSendNotification;
    private MutableLiveData<Boolean> checkedAlerts;
    private MutableLiveData<Boolean> checkedService;
    private MutableLiveData<Boolean> enabledAlerts;
    private MutableLiveData<Boolean> enabledService;
    private int typeNotification;
    private IValidateInternet validateInternet;

    @Inject
    public ConfigurationNotificationViewModel(SubscriptionRepository subscriptionRepository, CustomSharedPreferences customSharedPreferences,
                                              NotificationsRepository notificationsRepository, UpdateStatusSendNotificationRequest updateStatusSendNotificationRequest, ValidateInternet validateInternet) {
        this.error = new MutableLiveData<>();
        this.enabledAlerts = new MutableLiveData<>();
        this.enabledService = new MutableLiveData<>();
        this.checkedService = new MutableLiveData<>();
        this.checkedAlerts = new MutableLiveData<>();
        this.subscriptionBL = subscriptionRepository;
        this.customSharedPreferences = customSharedPreferences;
        this.notificationsBL = notificationsRepository;
        this.updateStatusSendNotification = updateStatusSendNotificationRequest;
        this.validateInternet = validateInternet;
        enabledAlerts.setValue(false);
        enabledService.setValue(false);
        checkedService.setValue(false);
        checkedAlerts.setValue(false);
    }

    public void verifyNotification() {
        subscriptionBL.getListSubscriptionAlertas(customSharedPreferences.getString(Constants.TOKEN),
                IdDispositive.getIdDispositive(), Constants.ID_APPLICATION,customSharedPreferences.getString(Constants.ONE_SIGNAL_ID)).observeForever(getSubscriptionNotificationsPush -> {
            if (getSubscriptionNotificationsPush != null) {
                validateListIsEmptyOrNull(getSubscriptionNotificationsPush.getSuscripciones());
            }
        });
    }

    public void validateListIsEmptyOrNull(List<ListSubscriptions> subscriptions) {
        if (subscriptions != null && !subscriptions.isEmpty()) {
            validateSubscription(subscriptions);
        }
    }

    public void validateSubscription(List<ListSubscriptions> subscriptions) {
        for (ListSubscriptions getSubscriptionNotificationsPush : subscriptions) {
            validateTypeNotification(getSubscriptionNotificationsPush);
        }
    }

    public void validateTypeNotification(ListSubscriptions subscriptions) {
        switch (subscriptions.getIdTipoSuscripcionNotificacion()) {
            case Constants.TYPE_SUSCRIPTION_ALERTAS:
                loadDataToNotificationAlerts(subscriptions.getEnvioNotificacion());
                break;
            case Constants.TYPE_SUBSCRIPTION_CUSTOMER_SERVICE:
                loadDataToNotificationService(subscriptions.getEnvioNotificacion());
                break;
            default:
                break;
        }
    }

    public void loadDataToNotificationAlerts(boolean sendNotification) {
        enabledAlerts.setValue(true);
        checkedAlerts.setValue(sendNotification);
    }

    public void loadDataToNotificationService(boolean sendNotification) {
        enabledService.setValue(true);
        checkedService.setValue(sendNotification);
    }

    public void updateStatusNotification(int typeSubscription,boolean checked) {
        loadDataToUpdate(typeSubscription,checked);
        validateInternetUpdateStatusNotification();
    }

    public void validateInternetUpdateStatusNotification(){
        if (validateInternet.isConnected()) {
            updateNotification();
        } else {
            validateError(R.string.title_appreciated_user, R.string.text_validate_internet);
        }
    }


    public void updateNotification() {
        Observable<UpdateStatusSendNotificationResponse> result =  notificationsBL.updateStatusSendNotification(updateStatusSendNotification, customSharedPreferences.getString(Constants.TOKEN));
        Disposable disposable = result.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<UpdateStatusSendNotificationResponse>() {
                    @Override
                    public void onNext(UpdateStatusSendNotificationResponse updateStatusSendNotificationResponse) {
                        //verifyNotification();
                        Log.d("complete","completeUpdate");
                    }

                    @Override
                    public void onError(Throwable e) {
                        ValidateServiceCode.captureServiceErrorCode(((HttpException) e).code());
                        validateError(ValidateServiceCode.getTitleError(), ValidateServiceCode.getError());
                    }

                    @Override
                    public void onComplete() {

                    }
                });
        disposables.add(disposable);

    }


    public void loadDataToUpdate(int typeSubscription,boolean checked) {
        updateStatusSendNotification.setEnvioNotificacion(checked);
        updateStatusSendNotification.setIdDispositivo(IdDispositive.getIdDispositive());
        updateStatusSendNotification.setIdTipoSuscripcionNotificacion(typeSubscription);
        this.typeNotification = typeSubscription;
    }


    public void validateError(int title, int error) {
        this.titleError = title;
        this.messageError = error;
        if (ValidateServiceCode.getErrorCode() == Constants.UNAUTHORIZED_ERROR_CODE) {
            expiredToken.setValue(true);
        } else {
            validateErrorTypeNotification();
            this.error.setValue(true);
        }
    }

    private void validateErrorTypeNotification(){
        switch (typeNotification) {
            case Constants.TYPE_SUSCRIPTION_ALERTAS:
                checkedAlerts.setValue(!checkedAlerts.getValue());
                break;
            case Constants.TYPE_SUBSCRIPTION_CUSTOMER_SERVICE:
                checkedService.setValue(!checkedService.getValue());
                break;
            default:
                break;
        }
    }

    @Override
    protected void onCleared() {
        super.onCleared();
        DisposableManager.dispose();
    }

    @Override
    public MutableLiveData<Boolean> getEnabledAlerts() {
        return enabledAlerts;
    }

    @Override
    public MutableLiveData<Boolean> getEnabledService() {
        return enabledService;
    }

    @Override
    public MutableLiveData<Boolean> getCheckedAlerts() {
        return checkedAlerts;
    }

    @Override
    public MutableLiveData<Boolean> getCheckedService() {
        return checkedService;
    }

    @Override
    public MutableLiveData<Boolean> getIsError() {
        return error;
    }

    @Override
    public int getMessageError() {
        return messageError;
    }

    @Override
    public int getIntTitleError() {
        return titleError;
    }
}
