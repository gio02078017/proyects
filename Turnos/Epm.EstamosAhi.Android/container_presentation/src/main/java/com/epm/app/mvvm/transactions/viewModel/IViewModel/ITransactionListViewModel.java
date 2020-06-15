package com.epm.app.mvvm.transactions.viewModel.IViewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.viewModel.IBaseViewModel;
import com.epm.app.mvvm.transactions.models.Transaction;
import com.epm.app.mvvm.transactions.network.request.TransactionRequest;
import com.epm.app.mvvm.transactions.network.response.TransactionListResponse;

import java.util.List;

public interface ITransactionListViewModel extends IBaseViewModel {
    MutableLiveData<List<Transaction>> getListTransaction();
    void fetchFastTransactions();
    String getFirstService(int position);
    String getIdTransaction(int position);
    String getNameTransaction(int position);
    boolean isTransactionNotNullOrEmpty();
    List<String> getServices(Transaction transaction);
    boolean validateIfTransactionIsNullOrEmpty(TransactionListResponse result);
    void validateResponse(TransactionListResponse result);

}
