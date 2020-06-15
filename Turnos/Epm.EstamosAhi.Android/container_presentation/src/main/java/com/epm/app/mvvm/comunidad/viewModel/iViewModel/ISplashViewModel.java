package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;
import android.content.Context;

import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.comunidad.network.response.places.Municipio;

import java.util.List;

public interface ISplashViewModel {

    void loadViewWithSplash();
    MutableLiveData<String> getSuccess();
    void loadUser(Usuario usuario);
    void captureError();
    Boolean validateViewTutorial(Context context);
    void loadMunicipio();
    int getErrorUnauhthorized();
    List<Municipio> getListMunicipio();
    int getIntTitleError();


}
