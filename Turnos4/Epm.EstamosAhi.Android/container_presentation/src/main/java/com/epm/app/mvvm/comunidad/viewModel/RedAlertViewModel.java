package com.epm.app.mvvm.comunidad.viewModel;

import android.arch.lifecycle.MutableLiveData;
import com.epm.app.mvvm.comunidad.bussinesslogic.ISubscriptionBL;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetDetailRedAlertResponse;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.IRedAlertViewModel;


import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;

public class RedAlertViewModel extends BaseViewModel implements IRedAlertViewModel {

    ICustomSharedPreferences customSharedPreferences;
    public final MutableLiveData<String> location;
    private ISubscriptionBL subscriptionBL;
    private MutableLiveData<Boolean> progress;
    private MutableLiveData<Boolean> notFound;
    private MutableLiveData<Boolean> subscription;
    private MutableLiveData<GetDetailRedAlertResponse> getDetailRedAlertResponse;
    private int titleError;
    private int errorUnauthorized;


    @Inject
    public RedAlertViewModel(CustomSharedPreferences customSharedPreferences, SubscriptionRepository subscriptionRepository) {
        this.subscription = new MutableLiveData<>();
        this.location = new MutableLiveData<>();
        this.customSharedPreferences = customSharedPreferences;
        this.subscriptionBL = subscriptionRepository;
        this.progress = new MutableLiveData<>();
        this.notFound = new MutableLiveData<>();
        this.getDetailRedAlertResponse = new MutableLiveData<>();
    }

    @Override
    public void validateSubscription() {
        String subscription_ = customSharedPreferences.getString(Constants.SUSCRIPTION_ALERTAS);
        if (subscription_ == null || subscription_.equals(Constants.TRUE)) {
            loadLocation();
        }else {
            this.subscription.setValue(false);
        }
    }

    public void loadLocation() {
        progress.setValue(true);
        subscriptionBL.getDetailRedAlert(IdDispositive.getIdDispositive(),customSharedPreferences.getString(Constants.TOKEN)).observeForever(this::validateLocation);
    }

    public void validateLocation(GetDetailRedAlertResponse getDetailRedAlertResponse){
        progress.setValue(false);
        if(getDetailRedAlertResponse != null){
            this.location.setValue(getDetailRedAlertResponse.getDetalleNotificacionPush().getNombre());
            this.getDetailRedAlertResponse.setValue(getDetailRedAlertResponse);
        }
    }


    @Override
    public void showError() {
        subscriptionBL.showError().observeForever(errorMessage -> {
            if (errorMessage != null ) {
                validateError(errorMessage);
                progress.setValue(false);
            }
        });
    }

    public void validateError(ErrorMessage errorMessage) {
        if (ValidateServiceCode.getErrorCode() == Constants.UNAUTHORIZED_ERROR_CODE) {
            this.titleError = errorMessage.getTitle();
            this.errorUnauthorized = errorMessage.getMessage();
            expiredToken.setValue(true);
        } if(ValidateServiceCode.getErrorCode() == Constants.NOT_FOUND_ERROR_CODE){
            notFound.setValue(true);
        }
        else {
            showErrorService(errorMessage);
        }
    }


    @Override
    public MutableLiveData<GetDetailRedAlertResponse> getGetDetailRedAlertResponse() {
        return getDetailRedAlertResponse;
    }

    @Override
    public MutableLiveData<Boolean> getProgress() {
        return progress;
    }



    public MutableLiveData<Boolean> getNotFound() {
        return notFound;
    }


    @Override
    protected void onCleared() {
        super.onCleared();
        DisposableManager.dispose();
    }

    @Override
    public int getIntTitleError() {
        return titleError;
    }

    @Override
    public MutableLiveData<Boolean> getSubscription() {
        return subscription;
    }

    @Override
    public int getErrorUnauhthorized() {
        return errorUnauthorized;
    }
}
