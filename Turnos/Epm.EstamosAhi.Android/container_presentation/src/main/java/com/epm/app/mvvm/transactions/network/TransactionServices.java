package com.epm.app.mvvm.transactions.network;

import com.epm.app.mvvm.transactions.network.request.TransactionRequest;
import com.epm.app.mvvm.transactions.network.response.TransactionListResponse;

import io.reactivex.Observable;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface TransactionServices  {

    @GET("TransaccionRapida/ObtenerTransaccionesRapidas")
    Observable<TransactionListResponse> GetFastTransaction(
            @Header("authToken") String token);


    @POST("TransaccionRapida/ObtenerTransaccionesRapidas")
    Observable<TransactionListResponse> GetPdf(
            @Query("id")  String id,
            @Header("authToken") String token,
            @Body TransactionRequest transactionRequest);

}
