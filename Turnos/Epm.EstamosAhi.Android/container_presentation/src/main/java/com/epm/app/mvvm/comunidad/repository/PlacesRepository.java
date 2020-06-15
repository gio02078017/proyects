package com.epm.app.mvvm.comunidad.repository;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import android.util.Log;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.bussinesslogic.IPlacesBL;
import com.epm.app.mvvm.comunidad.network.SuscriptionServices;
import com.epm.app.mvvm.comunidad.network.response.places.ObtenerMunicipios;
import com.epm.app.mvvm.comunidad.network.response.places.ObtenerSectores;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateServiceCode;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.DisposableManager;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.Disposable;
import io.reactivex.schedulers.Schedulers;


public class PlacesRepository implements IPlacesBL {

    private SuscriptionServices webService;
    private MutableLiveData<ErrorMessage> error;
    private IValidateInternet validateInternet;


    @Inject
    public PlacesRepository(SuscriptionServices webService, ValidateInternet validateInternet) {
        this.webService = webService;
        this.validateInternet = validateInternet;
        error = new MutableLiveData<>();

    }

    @Override
    public LiveData<ObtenerMunicipios> getMunicipios(String token,int idSubscription) {
         MutableLiveData<ObtenerMunicipios> obtenerMunicipiosMutableLiveData = new MutableLiveData<>();
        if (validateInternet.isConnected()) {
            ObtenerMunicipios obtenerMunicipios = new ObtenerMunicipios();
            Disposable disposable = webService.getObtenerMunicipios(token,idSubscription)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(response -> {
                        ValidateServiceCode.setTitleError404(R.string.title_error_municipio);
                        ValidateServiceCode.setError404(R.string.text_error_municipio);
                        if(ValidateServiceCode.captureServiceErrorCode(response.code())){
                            obtenerMunicipios.setCode(response.code());
                            obtenerMunicipios.setMessage(response.message());
                            if (response.isSuccessful()) {
                                if (response.body() != null) {
                                    Log.e("repository","lmunicipios "+response.body().getMunicipios().size());
                                    obtenerMunicipios.setMunicipios(response.body().municipios);
                                    obtenerMunicipios.setMensaje(response.body().mensaje);
                                    obtenerMunicipiosMutableLiveData.postValue(obtenerMunicipios);

                                }
                            }
                        }else {
                            setError(ValidateServiceCode.getTitleError(),ValidateServiceCode.getError());
                        }

                    }, throwable -> {
                        setError(R.string.title_error,R.string.text_error_500);
                        obtenerMunicipios.setMessage(throwable.getMessage());
                    });
            DisposableManager.add(disposable);

        } else {
            setError(R.string.title_appreciated_user,R.string.text_validate_internet);
        }
        return obtenerMunicipiosMutableLiveData;
    }

    private void setError(int titleError, int message) {
        error.setValue(new ErrorMessage(titleError,message));
    }

    @Override
    public MutableLiveData<ObtenerSectores> getSectores(String token, int id) {
        Log.e("repository","sectores "+id);
        MutableLiveData<ObtenerSectores> obtenerSectoresMutableLiveData = new MutableLiveData<>();
        if (validateInternet.isConnected()) {
            ObtenerSectores obtenerSectores = new ObtenerSectores();
            Disposable disposable =  webService.getObtenerSectores(id, token)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(response -> {
                        ValidateServiceCode.setTitleError404(R.string.title_error_sector404);
                        ValidateServiceCode.setError404(R.string.text_error_sector404);
                        if(ValidateServiceCode.captureServiceErrorCode(response.code())) {
                            if (response.isSuccessful()) {
                                if (response.body() != null) {
                                    Log.e("repository","sectores "+response.body().getSectores().size());
                                    obtenerSectores.setSectores(response.body().sectores);
                                    obtenerSectores.setMensaje(response.body().mensaje);
                                    obtenerSectoresMutableLiveData.setValue(obtenerSectores);
                                }
                            }
                        }else {
                            setError(ValidateServiceCode.getTitleError(),ValidateServiceCode.getError());
                        }

                    }, throwable -> {
                        setError(R.string.title_error,R.string.text_error_500);
                        obtenerSectores.setThrowable(throwable);

                    });
            DisposableManager.add(disposable);
        } else {
            setError(R.string.title_appreciated_user,R.string.text_validate_internet);
        }
        return obtenerSectoresMutableLiveData;
    }



    @Override
    public MutableLiveData<ErrorMessage> showError() {
        return error;
    }





}
