package com.epm.app.mvvm.turn.viewModel;

import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IDetailsOfTheTransactionViewModel;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;

public class DetailsOfTheTransactionViewModel extends BaseViewModel implements IDetailsOfTheTransactionViewModel {

   private IValidateInternet validateInternet;

    @Inject
    public DetailsOfTheTransactionViewModel(ValidateInternet validateInternet) {
        this.validateInternet = validateInternet;
    }
}
