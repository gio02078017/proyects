package com.epm.app.mvvm.turn.viewModel.iViewModel;

import android.arch.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.viewModel.IBaseViewModel;
import com.epm.app.mvvm.turn.models.StateMessage;
import com.epm.app.mvvm.turn.network.response.officeDetail.OfficeDetailResponse;

import app.epm.com.utilities.helpers.InformationOffice;

import java.io.Serializable;

public interface IOfficeDetailViewModel extends IBaseViewModel {

    void getOfficeDetail(InformationOffice informationOffice, boolean controlShiftInformation);
    MutableLiveData<Boolean> getProgressDialog();
    MutableLiveData<Boolean> getSuccessOfficeDetail();
    MutableLiveData<Boolean> getProblemsOfficeDetail();
    MutableLiveData<Boolean> getChangeButtonAskTurn();
    MutableLiveData<Boolean> getShiftInSameoffice();
    OfficeDetailResponse getOfficeDetailResponse();
    MutableLiveData<StateMessage<Serializable>> getTurnAbandonedOrAttended();
    MutableLiveData<StateMessage<Serializable>> getTurnCanceled();
}
