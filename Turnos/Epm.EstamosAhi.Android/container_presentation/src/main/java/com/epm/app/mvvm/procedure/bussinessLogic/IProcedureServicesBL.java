package com.epm.app.mvvm.procedure.bussinessLogic;



import com.epm.app.mvvm.procedure.network.request.DetailOfTheTransactionRequest;
import com.epm.app.mvvm.procedure.network.response.DetailOfTheTransactionResponse;
import com.epm.app.mvvm.procedure.network.request.ProcedureRequest;
import com.epm.app.mvvm.procedure.network.request.TypePersonRequest;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryResponse;
import com.epm.app.mvvm.procedure.network.response.FrequentProceduresResponse;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonResponse;


import io.reactivex.Observable;

public interface IProcedureServicesBL {

    Observable<GuideProceduresAndRequirementsCategoryResponse> getGuideProceduresAndRequirementsCategory(String token);
    Observable<DetailOfTheTransactionResponse> getDetailOfTheTransaction(String token, DetailOfTheTransactionRequest detailOfTheTransactionRequest);
    Observable<FrequentProceduresResponse> getFrequentProcedures(ProcedureRequest procedureRequest,String token);
    Observable<TypePersonResponse> getTypePerson(TypePersonRequest typePersonRequest, String token);

}
