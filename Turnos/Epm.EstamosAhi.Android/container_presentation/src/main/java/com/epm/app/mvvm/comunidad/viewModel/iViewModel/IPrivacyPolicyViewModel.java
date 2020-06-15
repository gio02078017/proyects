package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;


public interface IPrivacyPolicyViewModel   {

    void loadUrl();
    String getUrl();
    MutableLiveData<Boolean> getSuccessful();

}
