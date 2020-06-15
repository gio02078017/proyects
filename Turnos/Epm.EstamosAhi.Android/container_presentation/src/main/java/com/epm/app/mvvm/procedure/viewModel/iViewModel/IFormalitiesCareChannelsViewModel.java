package com.epm.app.mvvm.procedure.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.procedure.network.response.DetailOfTheTransactionResponse;

public interface IFormalitiesCareChannelsViewModel {

    void loadFormalities(DetailOfTheTransactionResponse transactionResponse);
    MutableLiveData<Integer> getLineAttentionVisibility();
    MutableLiveData<Integer> getVisitVisibility();


}
