package com.epm.app.mvvm.turn.viewModel.iViewModel;

import android.arch.lifecycle.MutableLiveData;

import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryItem;

import java.util.List;

public interface IGuideProceduresAndRequirementsViewModel {

    void getGuideProceduresAndRequirementsCategories();
    MutableLiveData<Boolean> getProgressDialog();
    MutableLiveData<List<GuideProceduresAndRequirementsCategoryItem>> getListGuideProceduresAndRequirementsCategory();
}
