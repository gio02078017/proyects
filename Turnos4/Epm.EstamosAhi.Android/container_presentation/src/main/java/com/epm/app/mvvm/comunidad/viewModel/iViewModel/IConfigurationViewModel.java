package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import android.arch.lifecycle.MutableLiveData;


public interface IConfigurationViewModel  {

    void verifyNotification();
    MutableLiveData<Boolean> getCheckedAlerts();
    MutableLiveData<Boolean> getIsError();
    MutableLiveData<Boolean> getEnabledAlerts();
    MutableLiveData<Boolean> getEnabledService();
    MutableLiveData<Boolean> getCheckedService();
    void updateStatusNotification(int typeSubscription, boolean checked);
    int getIntTitleError();
    int getMessageError();
}
