package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;

import app.epm.com.utilities.helpers.ErrorMessage;


public interface IConfigurationViewModel  {

    void verifyNotification();
    MutableLiveData<Boolean> getCheckedAlerts();
    MutableLiveData<ErrorMessage> getIsError();
    MutableLiveData<Boolean> getEnabledAlerts();
    MutableLiveData<Boolean> getEnabledService();
    MutableLiveData<Boolean> getCheckedService();
    void updateStatusNotification(int typeSubscription, boolean checked);
}
