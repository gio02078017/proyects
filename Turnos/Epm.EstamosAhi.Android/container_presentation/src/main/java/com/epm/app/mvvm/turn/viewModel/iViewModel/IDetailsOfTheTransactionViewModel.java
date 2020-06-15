package com.epm.app.mvvm.turn.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.viewModel.IBaseViewModel;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
import com.epm.app.mvvm.procedure.network.response.DetailOfTheTransactionResponse;
import com.epm.app.mvvm.turn.models.DetailOfFormalitiesGroup;

import java.util.List;

public interface IDetailsOfTheTransactionViewModel extends IBaseViewModel {

    void loadDetailOfTheTransaction(ProcedureInformation procedureInformation,boolean validateButton);
    MutableLiveData<List<DetailOfFormalitiesGroup>> getDetailOfFormalitiesGroups();
    MutableLiveData<DetailOfTheTransactionResponse> getPushBottom();

}
