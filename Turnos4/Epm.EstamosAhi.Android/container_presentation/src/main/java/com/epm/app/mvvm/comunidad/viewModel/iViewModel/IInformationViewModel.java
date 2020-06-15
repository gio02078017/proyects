package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import android.arch.lifecycle.MutableLiveData;


public interface IInformationViewModel  {

    MutableLiveData<String> getUrl();
    void getUrlInformation();
    MutableLiveData<Boolean> getInternet();
    int getIntTitleError();
    int getIntError();
}
