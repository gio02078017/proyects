package com.epm.app.mvvm.transactions.viewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.transactions.bussinesslogic.ITransactionsBL;
import com.epm.app.mvvm.transactions.models.Transaction;
import com.epm.app.mvvm.transactions.network.request.TransactionRequest;
import com.epm.app.mvvm.transactions.network.response.TransactionListResponse;
import com.epm.app.mvvm.transactions.repository.TransactionRepository;
import com.epm.app.mvvm.transactions.viewModel.IViewModel.ITransactionListViewModel;

import java.util.List;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;

public class TransactionListViewModel extends BaseViewModel implements ITransactionListViewModel {

    private MutableLiveData<List<Transaction>> listTransactions;
    private ITransactionsBL fastTransitionServicesBL;
    private IValidateInternet validateInternet;
    private ICustomSharedPreferences customSharedPreferences;

    @Inject
    public TransactionListViewModel(TransactionRepository fastTransitionServicesBL, ValidateInternet validateInternet, CustomSharedPreferences customSharedPreferences) {
        this.fastTransitionServicesBL = fastTransitionServicesBL;
        this.validateInternet = validateInternet;
        this.customSharedPreferences = customSharedPreferences;
        listTransactions = new MutableLiveData<>();
    }



    @Override
    public void fetchFastTransactions() {
        Observable<TransactionListResponse> result = fastTransitionServicesBL.getFastTransactions(customSharedPreferences.getString(Constants.TOKEN));
        fetchService(result, validateInternet);
    }

    @Override
    protected void handleResponse(Object responseService) {
        TransactionListResponse result = (TransactionListResponse) responseService;
        validateResponse(result);
    }

    @Override
    public void validateResponse(TransactionListResponse result) {
        if (result != null && result.getTransactionState() && validateIfTransactionIsNullOrEmpty(result)) {
            listTransactions.setValue(result.getTransaction());
        }
    }

    @Override
    public String getFirstService(int position){
        Transaction transaction = listTransactions.getValue().get(position);
        return (transaction.getServices() != null && !transaction.getServices().isEmpty()) ? transaction.getServices().get(Constants.ZERO): "";
    }

    @Override
    public boolean validateIfTransactionIsNullOrEmpty(TransactionListResponse result) {
        if (result.getTransaction() != null){
            return !result.getTransaction().isEmpty();
        }
        return false;
    }

    @Override
    public String getNameTransaction(int position) {
        return (isTransactionNotNullOrEmpty()) ? listTransactions.getValue().get(position).getName(): "";
    }

    @Override
    public boolean isTransactionNotNullOrEmpty() {
        return listTransactions != null && listTransactions.getValue() != null;
    }

    @Override
    public List<String> getServices(Transaction transaction){
        return transaction.getServices();
    }

    @Override
    public String getIdTransaction(int position) {
        return (isTransactionNotNullOrEmpty()) ? listTransactions.getValue().get(position).getId(): "";
    }

    @Override
    public MutableLiveData<List<Transaction>> getListTransaction() {
       return listTransactions;
    }


}
