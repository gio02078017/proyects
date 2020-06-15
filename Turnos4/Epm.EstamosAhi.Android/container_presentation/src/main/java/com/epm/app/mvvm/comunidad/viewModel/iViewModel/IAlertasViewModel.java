package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import android.arch.lifecycle.MutableLiveData;

import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.comunidad.network.response.places.Municipio;
import com.epm.app.mvvm.comunidad.network.response.places.Sectores;
import com.epm.app.mvvm.comunidad.viewModel.IBaseViewModel;

import java.util.List;

public interface IAlertasViewModel extends IBaseViewModel {
    void loadInfoProfile(Usuario usuario);
    List getListMunicipio();
    List<Sectores> getListSector();
    void loadMunicipio(List<Municipio> list);
    void loadSector(int id);
    MutableLiveData<Boolean> getProgressDialog();
    boolean isStatusSubscription();
    MutableLiveData<String> getResponseService();
    MutableLiveData<String> getTitleResponseService();

}
