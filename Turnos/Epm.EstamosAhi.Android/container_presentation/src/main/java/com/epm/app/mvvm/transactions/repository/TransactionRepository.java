package com.epm.app.mvvm.transactions.repository;

import com.epm.app.mvvm.transactions.bussinesslogic.ITransactionsBL;
import com.epm.app.mvvm.transactions.network.TransactionServices;
import com.epm.app.mvvm.transactions.network.request.TransactionRequest;
import com.epm.app.mvvm.transactions.network.response.TransactionListResponse;

import javax.inject.Inject;

import io.reactivex.Observable;

public class TransactionRepository implements ITransactionsBL {

    private TransactionServices webServices;

    @Inject
    public TransactionRepository(TransactionServices transactionServices) {
        this.webServices = transactionServices;
    }

    @Override
    public Observable<TransactionListResponse> getFastTransactions(String token) {
        return webServices.GetFastTransaction(token);
    }
}
