package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetDetailRedAlertResponse;

public interface IRedAlertViewModel {

    void validateSubscription();
    MutableLiveData<Boolean> getProgress();
    int getErrorUnauhthorized();
    MutableLiveData<Boolean> getSubscription();
    MutableLiveData<GetDetailRedAlertResponse> getGetDetailRedAlertResponse();
    MutableLiveData<Boolean> getNotFound();
    int getIntTitleError();

}
