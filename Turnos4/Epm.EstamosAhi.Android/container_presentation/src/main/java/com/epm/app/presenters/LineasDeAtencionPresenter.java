package com.epm.app.presenters;

import com.epm.app.R;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.view.views_activities.ILineasDeAtencionView;

import java.util.List;

import app.epm.com.container_domain.business_models.LineaDeAtencion;
import app.epm.com.container_domain.lineas_de_atencion.LineasDeAtencionBusinessLogic;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by root on 28/03/17.
 */

public class LineasDeAtencionPresenter extends BasePresenter<ILineasDeAtencionView> {

    private LineasDeAtencionBusinessLogic lineasDeAtencionBusinessLogic;

    public LineasDeAtencionPresenter(LineasDeAtencionBusinessLogic lineasDeAtencionBusinessLogic) {
        this.lineasDeAtencionBusinessLogic = lineasDeAtencionBusinessLogic;
    }

    public void getValidateInternetGetLineasDeAtencion() {
        if (getValidateInternet().isConnected()) {
            createThreadToGetLineasDeAtencion();
        } else {
            getView().showAlertDialogToLoadAgainOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    public void createThreadToGetLineasDeAtencion() {
        getView().showProgressDIalog(R.string.text_please_wait);
        new Thread(this::getLineasDeAtencion).start();
    }

    public void getLineasDeAtencion() {
        try {
            List<LineaDeAtencion> lineaDeAtencions = lineasDeAtencionBusinessLogic.getLineasDeAtencion();
            getView().consultLineasDeAtencion(lineaDeAtencions);
        } catch (RepositoryError repositoryError) {

            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogToLoadAgainOnUiThread(R.string.title_error, repositoryError.getMessage());

            }
        } finally {
            getView().dismissProgressDialog();
        }


    }


}
