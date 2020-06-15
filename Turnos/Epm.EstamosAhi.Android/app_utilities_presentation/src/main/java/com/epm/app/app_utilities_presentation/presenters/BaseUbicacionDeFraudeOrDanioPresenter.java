package com.epm.app.app_utilities_presentation.presenters;

import com.epm.app.app_utilities_presentation.R;
import com.epm.app.app_utilities_presentation.views.views_activities.IBaseUbicacionDeFraudeOrDanioView;
import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import com.epm.app.business_models.business_models.RepositoryError;

import app.epm.com.app_utilities_domain.ubicacion_danios_fraudes.BaseUbicacionDaniosOrFraudesBusinessLogic;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 19/04/17.
 */

public class BaseUbicacionDeFraudeOrDanioPresenter<T extends IBaseUbicacionDeFraudeOrDanioView> extends BasePresenter<T> {

    private BaseUbicacionDaniosOrFraudesBusinessLogic ubicacionDaniosOrFraudesBusinessLogic;

    public BaseUbicacionDeFraudeOrDanioPresenter(BaseUbicacionDaniosOrFraudesBusinessLogic ubicacionDaniosOrFraudesBusinessLogic) {
        this.ubicacionDaniosOrFraudesBusinessLogic = ubicacionDaniosOrFraudesBusinessLogic;
    }

    public BaseUbicacionDaniosOrFraudesBusinessLogic getUbicacionDaniosOrFraudesBusinessLogic() {
        return ubicacionDaniosOrFraudesBusinessLogic;
    }

    public void validateInternetToExecuteAnAction(Runnable runnable) {
        if (getValidateInternet().isConnected()) {
            createThreadToExecuteAnAction(runnable);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }
    public void createThreadToExecuteAnAction(Runnable runnable) {
        getView().showProgressDIalog(R.string.text_please_wait);
        new Thread(runnable).start();
    }

    public void getInformacionDeUbicacion(String lat, String lon) {
        try {
            validateInformacionDeUbicacion(ubicacionDaniosOrFraudesBusinessLogic.getInformacionDeUbicacion(lat, lon));
        } catch (RepositoryError repositoryError) {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), repositoryError.getMessage());
        } finally {
            getView().dismissProgressDialog();
        }
    }

    public void validateInformacionDeUbicacion(InformacionDeUbicacion informacionDeUbicacion) {
        if (informacionDeUbicacion == null || !informacionDeUbicacion.getPais().equals(Constants.COUNTRYCODE)) {
            getView().showAlertWithOutAddress(getView().getName(), R.string.text_ubicacion_no_encontrada);
        } else {
            getView().loadUbicationOnUiThread(informacionDeUbicacion);
        }
    }
}
