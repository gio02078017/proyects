package com.epm.app.mvvm.procedure.network;

import com.epm.app.mvvm.procedure.network.request.DetailOfTheTransactionRequest;
import com.epm.app.mvvm.procedure.network.request.ProcedureRequest;

import com.epm.app.mvvm.procedure.network.response.DetailOfTheTransactionResponse;

import com.epm.app.mvvm.procedure.network.request.TypePersonRequest;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryResponse;
import com.epm.app.mvvm.procedure.network.response.FrequentProceduresResponse;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonResponse;


import io.reactivex.Observable;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;

public interface ProcedureApiServices {

    @GET("Transaction/ObtenerCategorias")
    Observable<GuideProceduresAndRequirementsCategoryResponse> getGuideProceduresAndRequirementsCategory(
            @Header("authToken") String token);


    @POST("Transaction/ObtenerTramites")

    Observable<FrequentProceduresResponse> getProcedures(
            @Body ProcedureRequest procedureRequest,
            @Header("authToken") String token);

    @POST("Transaction/ObtenerMaestroTramite")
    Observable<TypePersonResponse> getTypePerson(
            @Body TypePersonRequest typePersonRequest,
            @Header("authToken") String token);


    @POST("Transaction/ObtenerDetalleTramite")
    Observable<DetailOfTheTransactionResponse> GetDetailProcedures(
            @Header("authToken") String token,
            @Body DetailOfTheTransactionRequest detailOfTheTransactionRequest);




}
