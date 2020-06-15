package com.epm.app.mvvm.transactions.bussinesslogic;


import com.epm.app.mvvm.transactions.network.response.TransactionListResponse;

import io.reactivex.Observable;

public interface ITransactionsBL {
    Observable<TransactionListResponse> getFastTransactions( String token);

}
