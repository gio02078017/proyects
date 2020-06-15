package com.epm.app.presenters;

import com.epm.app.R;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.view.views_activities.INoticiasView;

import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.container_domain.noticias.NoticiasBussinesLogic;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public class NoticiasPresenter extends BasePresenter<INoticiasView> {


    private NoticiasBussinesLogic noticiasBussinesLogic;

    public NoticiasPresenter(NoticiasBussinesLogic noticiasBussinesLogic) {
        this.noticiasBussinesLogic = noticiasBussinesLogic;
    }

    public void validateInternetToGetNoticias() {
        if (getValidateInternet().isConnected()) {
            createThreadToGetNoticias();
        } else {
            getView().showAlertDialogToLoadAgain(getView().getName(), R.string.text_validate_internet);
        }
    }

    public void createThreadToGetNoticias() {
        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {
                getNoticias();
            }
        });
        thread.start();
    }

    public void getNoticias() {

        try {
            List<NoticiasEventos> noticiasEventos = noticiasBussinesLogic.getNoticias();
            getView().showInformationNoticias(noticiasEventos);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogToLoadAgain(R.string.title_error, repositoryError.getMessage());
            }
        }
    }
}
