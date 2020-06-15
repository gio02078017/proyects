package com.epm.app.mvvm.comunidad.bussinesslogic;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.network.response.places.ObtenerMunicipios;
import com.epm.app.mvvm.comunidad.network.response.places.ObtenerSectores;
import com.epm.app.mvvm.util.IError;

public interface IPlacesBL extends IError {

     LiveData<ObtenerMunicipios> getMunicipios(String token,int idSubscription);
     MutableLiveData<ObtenerSectores> getSectores(String token, int id);
}
