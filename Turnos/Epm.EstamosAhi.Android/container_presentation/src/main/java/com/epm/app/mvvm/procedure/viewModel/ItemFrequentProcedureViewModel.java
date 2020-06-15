package com.epm.app.mvvm.procedure.viewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.procedure.network.response.Procedure;

public class ItemFrequentProcedureViewModel extends BaseViewModel {

    private MutableLiveData<String> nameProcedure;

    private Procedure procedure;
    public ItemFrequentProcedureViewModel() {
        nameProcedure = new MutableLiveData<>();
    }

    public MutableLiveData<String> getNameProcedure() {
        return nameProcedure;
    }

    public void drawInformation(){
        if (procedure != null) nameProcedure.setValue(procedure.getName());
    }

    public Procedure getProcedure() {
        return procedure;
    }

    public void setProcedure(Procedure procedure) {
        this.procedure = procedure;
    }
}
