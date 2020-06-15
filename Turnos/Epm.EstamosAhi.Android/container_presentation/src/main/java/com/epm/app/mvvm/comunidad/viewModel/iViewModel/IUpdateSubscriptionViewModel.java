package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.network.response.places.Sectores;

import java.util.List;

public interface IUpdateSubscriptionViewModel {

    void getSuscription();
    MutableLiveData<Boolean> getProgressDialog();
    List getListMunicipio();
    List<Sectores> getListSector();
    void loadSector(int id);
    MutableLiveData<String> getTitleMessage();
    MutableLiveData<String> getMessageSuccesful();
    boolean isValidateMessage();
    void cancelSubscription();


}
