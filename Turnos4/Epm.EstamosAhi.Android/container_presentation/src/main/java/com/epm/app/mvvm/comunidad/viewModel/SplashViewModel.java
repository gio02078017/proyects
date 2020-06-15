package com.epm.app.mvvm.comunidad.viewModel;


import android.arch.lifecycle.MutableLiveData;
import android.content.Context;

import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.comunidad.bussinesslogic.IPlacesBL;
import com.epm.app.mvvm.comunidad.bussinesslogic.ISharedBL;
import com.epm.app.mvvm.comunidad.bussinesslogic.ISubscriptionBL;
import com.epm.app.mvvm.comunidad.network.response.places.ListSubscriptions;
import com.epm.app.mvvm.comunidad.network.response.places.Municipio;
import com.epm.app.mvvm.comunidad.repository.PlacesRepository;
import com.epm.app.mvvm.comunidad.repository.RepositoryShared;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.ISplashViewModel;
import app.epm.com.utilities.helpers.ErrorMessage;

import java.util.List;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;

public class SplashViewModel extends BaseViewModel implements ISplashViewModel {

    private MutableLiveData<String> success;
    private ISharedBL repositoryShared;
    private ICustomSharedPreferences customSharedPreferences;
    private IPlacesBL placesBL;
    private ISubscriptionBL subscriptionBL;
    private int titleError, errorUnauhthorized;
    private String validate;
    private Usuario usuario;
    private List<Municipio> listMunicipio;

    @Inject
    public SplashViewModel(RepositoryShared repositoryShared, CustomSharedPreferences customSharedPreferences, SubscriptionRepository subscriptionBL, PlacesRepository placesBL) {
        this.repositoryShared = repositoryShared;
        success = new MutableLiveData<>();
        this.customSharedPreferences = customSharedPreferences;
        this.subscriptionBL = subscriptionBL;
        this.placesBL = placesBL;
    }

    public void loadUser(Usuario usuario) {
        if (usuario != null) {
            this.usuario = usuario;
        }
    }


    public void loadViewWithSplash() {
        customSharedPreferences.addString(Constants.SUSCRIPTION_ALERTAS, Constants.FALSE);
        validate = Constants.FALSE;
        getSubscriptions();
    }

    public Boolean validateViewTutorial(Context context) {
        return repositoryShared.getValidate(context);
    }




    public void getSubscriptions() {
        String oneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID);
        if(oneSignal == null){
            oneSignal = Constants.ONE_SIGNAL_IS_EMPTY;
        }
        subscriptionBL.getListSubscriptionAlertas(customSharedPreferences.getString(Constants.TOKEN), IdDispositive.getIdDispositive(),
                Constants.TYPE_SUSCRIPTION_ALERTAS,oneSignal).observeForever(subscription -> {
            if (subscription.getSuscripciones() != null && subscription.getSuscripciones().size() > 0) {
                 captureSubscription(subscription.getSuscripciones());
            } else {
                customSharedPreferences.addString(Constants.SUSCRIPTION_ALERTAS, Constants.FALSE);
                validate = Constants.FALSE;
            }
            loadMunicipio();
        });
    }

    @Override
    public void loadMunicipio() {
        placesBL.getMunicipios(customSharedPreferences.getString(Constants.TOKEN)).observeForever(obtenerMunicipios -> {
            if (obtenerMunicipios != null) {
                if (obtenerMunicipios.municipios != null && obtenerMunicipios.municipios.size() != 0) {
                    listMunicipio = obtenerMunicipios.getMunicipios();
                    success.setValue(validate);
                }
            }
        });
    }

    public void captureSubscription(List<ListSubscriptions> subscription){
        for (ListSubscriptions getSubscriptionNotificationsPush : subscription) {
            if (getSubscriptionNotificationsPush.getIdTipoSuscripcionNotificacion() == Constants.TYPE_SUSCRIPTION_ALERTAS) {
                customSharedPreferences.addString(Constants.SUSCRIPTION_ALERTAS, Constants.TRUE);
                validate = Constants.TRUE;
                break;
            }
        }
    }

    @Override
    public int getIntTitleError() {
        return titleError;
    }

    @Override
    public void captureError() {
        subscriptionBL.showError().observeForever(this::validateShowErrorSubscription);
        placesBL.showError().observeForever(errorMessage -> validateShowErrorPlaces(errorMessage));
    }

    public void validateShowErrorPlaces(ErrorMessage errorMessage) {
        if (ValidateServiceCode.getErrorCode() == Constants.UNAUTHORIZED_ERROR_CODE) {
            this.errorUnauhthorized = errorMessage.getMessage();
            expiredToken.setValue(true);
        } else {
            showError(errorMessage);
        }
    }

    public void validateShowErrorSubscription(ErrorMessage errorMessage) {
        if (ValidateServiceCode.getErrorCode() == Constants.UNAUTHORIZED_ERROR_CODE) {
            this.titleError = errorMessage.getTitle();
            this.errorUnauhthorized = errorMessage.getMessage();
            expiredToken.setValue(true);
            return;
        }
        if (ValidateServiceCode.getErrorCode() == Constants.NOT_FOUND_ERROR_CODE) {
            validate = Constants.FALSE;
            loadMunicipio();
        }
        else {
            showError(errorMessage);
        }

    }

    public void showError(ErrorMessage errorMessage) {
        this.error.setValue(errorMessage);
    }

    @Override
    protected void onCleared() {
        super.onCleared();
        DisposableManager.dispose();
    }



    @Override
    public int getErrorUnauhthorized() {
        return errorUnauhthorized;
    }

    @Override
    public MutableLiveData<String> getSuccess() {
        return success;
    }

    @Override
    public List<Municipio> getListMunicipio() {
        return listMunicipio;
    }

}
