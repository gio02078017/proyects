package com.epm.app.mvvm.procedure.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.viewModel.IBaseViewModel;
import com.epm.app.mvvm.procedure.network.request.TypePersonRequest;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonItem;

import java.util.List;

public interface ITypePersonViewModel extends IBaseViewModel {

    void fetchTypePerson(TypePersonRequest typePersonRequest);
    MutableLiveData<List<TypePersonItem>> getListTypePerson();
    TypePersonItem getTypePersonItem(int position);

}
