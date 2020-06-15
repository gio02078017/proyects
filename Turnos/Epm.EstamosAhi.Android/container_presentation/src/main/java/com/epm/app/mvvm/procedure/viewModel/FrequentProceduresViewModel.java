package com.epm.app.mvvm.procedure.viewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.procedure.bussinessLogic.IProcedureServicesBL;
import com.epm.app.mvvm.procedure.network.request.ProcedureRequest;
import com.epm.app.mvvm.procedure.network.response.FrequentProceduresResponse;
import com.epm.app.mvvm.procedure.network.response.Procedure;
import com.epm.app.mvvm.procedure.repository.ProcedureServicesRepository;
import com.epm.app.mvvm.procedure.viewModel.iViewModel.IFrequentProceduresViewModel;

import java.util.List;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;

public class FrequentProceduresViewModel extends BaseViewModel implements IFrequentProceduresViewModel {


    private MutableLiveData<List<Procedure>> listProcedure;
    private IProcedureServicesBL procedureServicesBL;
    private IValidateInternet validateInternet;
    private ICustomSharedPreferences customSharedPreferences;

    @Inject
    public FrequentProceduresViewModel(ProcedureServicesRepository procedureServicesBL, ValidateInternet validateInternet, CustomSharedPreferences customSharedPreferences) {
        this.procedureServicesBL = procedureServicesBL;
        this.validateInternet = validateInternet;
        this.customSharedPreferences = customSharedPreferences;
        listProcedure = new MutableLiveData<>();
    }


    @Override
    public void fetchFrequentProcedures(ProcedureRequest procedureRequest) {
        Observable<FrequentProceduresResponse> result = procedureServicesBL.getFrequentProcedures(procedureRequest,customSharedPreferences.getString(Constants.TOKEN));
        fetchService(result, validateInternet);
    }

    @Override
    protected void handleResponse(Object responseService) {
        FrequentProceduresResponse result = (FrequentProceduresResponse) responseService;
        validateResponse(result);
    }

    @Override
    public void validateResponse(FrequentProceduresResponse result) {
        if (result != null && result.getTransactionState() && validateIfProceduresIsNullOrEmpty(result)) {
            listProcedure.setValue(result.getTramites());
        }
    }

    @Override
    public boolean validateIfProceduresIsNullOrEmpty(FrequentProceduresResponse result) {
        if (result.getTramites() != null){
            return !result.getTramites().isEmpty();
        }
        return false;
    }

    @Override
    public String getFirstService(int position){
        Procedure procedure = listProcedure.getValue().get(position);
        return (procedure.getServices() != null && !procedure.getServices().isEmpty()) ? procedure.getServices().get(Constants.ZERO): "";
    }

    @Override
    public String getIdProcedure(int position) {
        return (isProcedureNotNullOrEmpty()) ? listProcedure.getValue().get(position).getId(): "";
    }

    public boolean isProcedureNotNullOrEmpty() {
        return listProcedure != null && listProcedure.getValue() != null;
    }

    @Override
    public String getNameProcedure(int position) {
        return (isProcedureNotNullOrEmpty()) ? listProcedure.getValue().get(position).getName(): "";
    }

    public List<String> getServices(Procedure procedure){
        return procedure.getServices();
    }

    @Override
    public MutableLiveData<List<Procedure>> getListProcedure() {
        return listProcedure;
    }


}
