package com.epm.app.mvvm.transactions.viewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.transactions.bussinesslogic.ITransactionsBL;
import com.epm.app.mvvm.transactions.repository.TransactionRepository;
import com.epm.app.mvvm.transactions.viewModel.IViewModel.IEnterInvoiceContractViewModel;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;

public class EnterInvoiceContractViewModel extends BaseViewModel implements IEnterInvoiceContractViewModel {

    private ITransactionsBL transactionsBL;
    private IValidateInternet validateInternet;
    public final MutableLiveData<String> textTittle;

    @Inject
    public EnterInvoiceContractViewModel(TransactionRepository transactionsBL, ValidateInternet validateInternet) {
        this.transactionsBL = transactionsBL;
        this.validateInternet = validateInternet;
        this.textTittle = new  MutableLiveData<>();
    }

}
