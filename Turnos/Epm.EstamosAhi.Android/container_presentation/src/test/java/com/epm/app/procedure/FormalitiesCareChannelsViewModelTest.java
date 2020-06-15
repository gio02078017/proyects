package com.epm.app.procedure;

import android.view.View;

import com.epm.app.BaseTest;
import com.epm.app.TestObserver;
import com.epm.app.mvvm.procedure.network.response.ChannelTypesResponse;
import com.epm.app.mvvm.procedure.network.response.DetailOfTheTransactionResponse;
import com.epm.app.mvvm.procedure.network.response.DetailTransactionResponse;
import com.epm.app.mvvm.procedure.viewModel.FormalitiesCareChannelsViewModel;

import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mockito;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.utilities.utils.Constants;


import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;

public class FormalitiesCareChannelsViewModelTest extends BaseTest {

    private FormalitiesCareChannelsViewModel formalitiesCareChannelsViewModel;

    @InjectMocks
    DetailOfTheTransactionResponse detailOfTheTransactionResponse;

    @InjectMocks
    DetailTransactionResponse detailTransactionResponse;

    private List<ChannelTypesResponse> listChannelTypesResponses;

    @InjectMocks
    ChannelTypesResponse channelTypesResponse;


    @Override
    public void setUp() throws Exception {
        super.setUp();
        listChannelTypesResponses = new ArrayList<>();
        formalitiesCareChannelsViewModel = Mockito.spy(new FormalitiesCareChannelsViewModel());
    }

    @Test
    public void testLoadFormalitiesDetailOfTheTransactionResponseIsNull(){
        detailOfTheTransactionResponse = null;
        formalitiesCareChannelsViewModel.loadFormalities(detailOfTheTransactionResponse);
        verify(formalitiesCareChannelsViewModel,times(0)).validateListFormalities(listChannelTypesResponses);
    }

    @Test
    public void testLoadFormalitiesDetailTransactionResponseIsNull(){
        formalitiesCareChannelsViewModel.loadFormalities(detailOfTheTransactionResponse);
        verify(formalitiesCareChannelsViewModel,times(0)).validateListFormalities(listChannelTypesResponses);
    }

    @Test
    public void testLoadFormalitiesDetailTransactionResponseIsNotNull(){
        detailTransactionResponse.setChannelTypes(listChannelTypesResponses);
        detailOfTheTransactionResponse.setDetailTransactionResponse(detailTransactionResponse);
        formalitiesCareChannelsViewModel.loadFormalities(detailOfTheTransactionResponse);
        verify(formalitiesCareChannelsViewModel).validateListFormalities(listChannelTypesResponses);
    }

    @Test
    public void testValidaListFormalitiesIsNull(){
        listChannelTypesResponses = null;
        formalitiesCareChannelsViewModel.validateListFormalities(listChannelTypesResponses);
        verify(formalitiesCareChannelsViewModel,times(0)).loadListFormalities(listChannelTypesResponses);
    }

    @Test
    public void testValidaListFormalitiesIsEmpty(){
        formalitiesCareChannelsViewModel.validateListFormalities(listChannelTypesResponses);
        verify(formalitiesCareChannelsViewModel,times(0)).loadListFormalities(listChannelTypesResponses);
    }

    @Test
    public void testValidaListFormalitiesIsNotEmptyAndNotNull(){
        loadList();
        formalitiesCareChannelsViewModel.validateListFormalities(listChannelTypesResponses);
        verify(formalitiesCareChannelsViewModel).loadListFormalities(listChannelTypesResponses);
    }

    @Test
    public void testLoadListFormalities(){
        loadList();
        formalitiesCareChannelsViewModel.loadListFormalities(listChannelTypesResponses);
        verify(formalitiesCareChannelsViewModel).validateFormalities(channelTypesResponse);
    }

    @Test
    public void testValidateFormalitiesIfIdLineAttention(){
        channelTypesResponse.setId(Constants.ID_LINE_ATTENTION);
        formalitiesCareChannelsViewModel.validateFormalities(channelTypesResponse);
        TestObserver.test(formalitiesCareChannelsViewModel.getLineAttentionVisibility()).assertValue(View.VISIBLE);
    }

    @Test
    public void testValidateFormalitiesIfIdVisitUs(){
        channelTypesResponse.setId(Constants.ID_VISIT_OFFICE);
        formalitiesCareChannelsViewModel.validateFormalities(channelTypesResponse);
        TestObserver.test(formalitiesCareChannelsViewModel.getVisitVisibility()).assertValue(View.VISIBLE);
    }

    private void loadList(){
        channelTypesResponse.setId("id");
        listChannelTypesResponses.add(channelTypesResponse);
    }

}
