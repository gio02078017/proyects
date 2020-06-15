package com.epm.app.mvvm.procedure.viewModel.iViewModel;

import android.arch.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.viewModel.IBaseViewModel;
import com.epm.app.mvvm.procedure.network.request.ProcedureRequest;
import com.epm.app.mvvm.procedure.network.response.FrequentProceduresResponse;
import com.epm.app.mvvm.procedure.network.response.Procedure;

import java.util.List;

public interface IFrequentProceduresViewModel extends IBaseViewModel {

    void fetchFrequentProcedures(ProcedureRequest procedureRequest);
    MutableLiveData<List<Procedure>> getListProcedure();
    String getFirstService(int position);
    Procedure getProcedure(int position);
    boolean validateIfProceduresIsNullOrEmpty(FrequentProceduresResponse result);
    void validateResponse(FrequentProceduresResponse result);

}
