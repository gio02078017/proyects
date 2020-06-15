package com.epm.app.mvvm.procedure.viewModel;

import androidx.lifecycle.MutableLiveData;
import android.view.View;

import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.procedure.network.response.ChannelTypesResponse;
import com.epm.app.mvvm.procedure.network.response.DetailOfTheTransactionResponse;
import com.epm.app.mvvm.procedure.viewModel.iViewModel.IFormalitiesCareChannelsViewModel;

import java.util.List;

import app.epm.com.utilities.utils.Constants;

public class FormalitiesCareChannelsViewModel extends BaseViewModel implements IFormalitiesCareChannelsViewModel {

    private  MutableLiveData<Integer> lineAttentionVisibility;
    private  MutableLiveData<Integer> visitVisibility;

    public FormalitiesCareChannelsViewModel() {
        lineAttentionVisibility = new MutableLiveData<>();
        visitVisibility = new MutableLiveData<>();
        lineAttentionVisibility.setValue(View.GONE);
        visitVisibility.setValue(View.GONE);
    }

    public void loadFormalities(DetailOfTheTransactionResponse transactionResponse){
       if(transactionResponse != null && transactionResponse.getDetailTransactionResponse() != null){
           validateListFormalities(transactionResponse.getDetailTransactionResponse().getChannelTypes());
       }
    }

    public void validateListFormalities(List<ChannelTypesResponse> channelTypesResponses){
        if(channelTypesResponses != null && !channelTypesResponses.isEmpty()){
            loadListFormalities(channelTypesResponses);
        }
    }

    public void loadListFormalities(List<ChannelTypesResponse> channelTypesResponses){
        for (ChannelTypesResponse channelTypesResponse: channelTypesResponses) {
            validateFormalities(channelTypesResponse);
        }
    }

    public void validateFormalities(ChannelTypesResponse channelTypesResponse){
        if(channelTypesResponse.getId().equals(Constants.ID_LINE_ATTENTION)){
            lineAttentionVisibility.setValue(View.VISIBLE);
        }
        if(channelTypesResponse.getId().equals(Constants.ID_VISIT_OFFICE)){
            visitVisibility.setValue(View.VISIBLE);
        }
    }

    @Override
    public MutableLiveData<Integer> getLineAttentionVisibility() {
        return lineAttentionVisibility;
    }

    @Override
    public MutableLiveData<Integer> getVisitVisibility() {
        return visitVisibility;
    }
}
