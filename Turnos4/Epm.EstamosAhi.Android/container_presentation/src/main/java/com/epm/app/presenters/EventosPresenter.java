package com.epm.app.presenters;

import com.epm.app.R;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.view.views_activities.IEventosView;

import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.container_domain.eventos.EventosBussinesLogic;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */


public class EventosPresenter extends BasePresenter<IEventosView> {

    private EventosBussinesLogic eventosBussinesLogic;

    public EventosPresenter(EventosBussinesLogic eventosBussinesLogic) {
        this.eventosBussinesLogic = eventosBussinesLogic;
    }

    public void validateInternetToGetEventos() {
        if (getValidateInternet().isConnected()) {
            createThreadToGetEventos();
        } else {
            getView().showAlertDialogToLoadAgain(getView().getName(), R.string.text_validate_internet);
        }
    }

    public void createThreadToGetEventos() {
        Thread thread = new Thread(this::getEventos);
        thread.start();
    }

    public void getEventos() {

        try {
            List<NoticiasEventos> noticiasEventos = eventosBussinesLogic.getEventos();
            getView().showInformationEventos(noticiasEventos);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogToLoadAgain(R.string.title_error, repositoryError.getMessage());
            }
        }
    }
}
