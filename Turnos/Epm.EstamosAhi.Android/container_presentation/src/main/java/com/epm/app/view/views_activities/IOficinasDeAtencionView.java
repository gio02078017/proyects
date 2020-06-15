package com.epm.app.view.views_activities;

import com.epm.app.mvvm.turn.network.response.AssignedTurn;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by root on 28/03/17.
 */

public interface IOficinasDeAtencionView extends IBaseView {

    void successAssignedTurn(AssignedTurn assignedTurn);

    void errorOrDoesNotTurn();

    void showErrorService(int titleError, int error);

    void showErrorUnauhthorized(int titleError, int error);

}
