package com.epm.app.mvvm.turn.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.turn.network.response.cancelTurn.CancelTurnResponse;

public interface IShiftInformationViewModel   {
    void cancelTurn(Integer numberTurn);
    MutableLiveData<Boolean> getProgressDialog();
    MutableLiveData<Boolean> getSuccessCancelTurn();
    MutableLiveData<Boolean> getFailedCancelTurn();
    MutableLiveData<Boolean> getFailedWithActionCancelTurn();
    CancelTurnResponse getCancelTurnResponse();
}
