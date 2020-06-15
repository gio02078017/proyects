package com.epm.app.mvvm.turn.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.viewModel.IBaseViewModel;
import com.epm.app.mvvm.turn.network.response.AssignedTurn;

public interface IOficinasDeAtencionViewModel extends IBaseViewModel {

    void getAssignedTurn();
    MutableLiveData<AssignedTurn> getResponseAssignedTurn();
    MutableLiveData<Boolean> getWithOutAssignedTurn();

}
