package com.epm.app.mvvm.turn.viewModel.iViewModel;

import android.arch.lifecycle.MutableLiveData;

import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.turn.network.response.askTurn.TurnResponse;
import com.epm.app.mvvm.turn.network.response.officeDetail.Oficina;

public interface IAskTurnViewModel {

    void askTurn(String nameClient, Oficina oficina, Usuario user);
    MutableLiveData<Boolean> getProgressDialog();
    MutableLiveData<Boolean> getSuccessAskTurn();
    MutableLiveData<Boolean> getProblemsAskTurn();
    TurnResponse getTurnResponse();

}
