package com.epm.app.mvvm.procedure.viewModel;

import androidx.lifecycle.MutableLiveData;
import android.content.res.Resources;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.procedure.bussinessLogic.IProcedureServicesBL;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
import com.epm.app.mvvm.procedure.network.request.DetailOfTheTransactionRequest;
import com.epm.app.mvvm.procedure.network.response.DetailOfTheTransactionResponse;
import com.epm.app.mvvm.procedure.repository.ProcedureServicesRepository;
import com.epm.app.mvvm.turn.models.DetailOfFormalitiesGroup;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IDetailsOfTheTransactionViewModel;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;

public class DetailsOfTheTransactionViewModel extends BaseViewModel implements IDetailsOfTheTransactionViewModel {

    private IValidateInternet validateInternet;
    private IProcedureServicesBL procedureServicesBL;
    private ICustomSharedPreferences customSharedPreferences;
    private Resources resources;
    private MutableLiveData<List<DetailOfFormalitiesGroup>> detailOfFormalitiesGroups;
    private DetailOfTheTransactionRequest detailOfTheTransactionRequest;
    public final MutableLiveData<String> textButton;
    public final MutableLiveData<String> textTittle;
    private DetailOfTheTransactionResponse detailResponse;
    private MutableLiveData<DetailOfTheTransactionResponse> pushButton;

    @Inject
    public DetailsOfTheTransactionViewModel(ValidateInternet validateInternet, ProcedureServicesRepository procedureServicesRepository,
                                            CustomSharedPreferences customSharedPreferences, Resources resources) {
        this.validateInternet = validateInternet;
        this.procedureServicesBL = procedureServicesRepository;
        this.customSharedPreferences = customSharedPreferences;
        this.detailOfFormalitiesGroups = new MutableLiveData<>();
        this.detailOfTheTransactionRequest = new DetailOfTheTransactionRequest();
        this.resources = resources;
        this.pushButton = new MutableLiveData<>();
        this.textButton = new MutableLiveData<>();
        this.textTittle = new MutableLiveData<>();
    }

    @Override
    public void loadDetailOfTheTransaction(ProcedureInformation procedureInformation, boolean validateButton){
        loadRequest(procedureInformation);
        loadButton(validateButton);
        Observable<DetailOfTheTransactionResponse> result = procedureServicesBL.
                getDetailOfTheTransaction(customSharedPreferences.getString(Constants.TOKEN),detailOfTheTransactionRequest);
        fetchService(result, validateInternet);
    }

    private void loadButton(boolean validateButton) {
        textButton.setValue(resources.getString(validateButton ? R.string.text_button_formalities_back: R.string.text_button_formalities));
    }

    public void pushButtonStartProcess(){
        pushButton.setValue(detailResponse);
    }

    public void loadRequest(ProcedureInformation procedureInformation) {
        detailOfTheTransactionRequest.setIdProcedure(procedureInformation.getIdProcedure());
        detailOfTheTransactionRequest.setIdService(procedureInformation.getIdService());
        detailOfTheTransactionRequest.setMasterDetails(procedureInformation.getIdMasterDetails());
    }

    @Override
    public void handleResponse(Object responseService) {
        DetailOfTheTransactionResponse detailOfTheTransactionResponse = (DetailOfTheTransactionResponse) responseService;
        if(detailOfTheTransactionResponse != null){
            loadData(detailOfTheTransactionResponse);
        }
    }



    public void loadData(DetailOfTheTransactionResponse detailOfTheTransactionResponse){
        if(detailOfTheTransactionResponse.getDetailTransactionResponse() != null){
            detailResponse = detailOfTheTransactionResponse;
            textTittle.setValue(detailOfTheTransactionResponse.getDetailTransactionResponse().getNombre());
            DetailOfFormalitiesGroup whatIsIt = new DetailOfFormalitiesGroup(resources.getString(R.string.text_title_detail_transaction_what_is_it),
                    Arrays.asList(detailOfTheTransactionResponse.getDetailTransactionResponse().getQueEs()),resources.getDrawable(R.drawable.ic_question));
            DetailOfFormalitiesGroup whatINeed = new DetailOfFormalitiesGroup(resources.getString(R.string.text_title_detail_transaction_what_I_need),
                    Arrays.asList(detailOfTheTransactionResponse.getDetailTransactionResponse().getQueNecesito()),resources.getDrawable(R.drawable.ic_icon_need_list));
            detailOfFormalitiesGroups.setValue(Arrays.asList(whatIsIt,whatINeed));
        }

    }

    @Override
    public MutableLiveData<DetailOfTheTransactionResponse> getPushBottom() {
        return pushButton;
    }

    @Override
    public MutableLiveData<List<DetailOfFormalitiesGroup>> getDetailOfFormalitiesGroups() {
        return detailOfFormalitiesGroups;
    }

    public DetailOfTheTransactionRequest getDetailOfTheTransactionRequest() {
        return detailOfTheTransactionRequest;
    }
}
