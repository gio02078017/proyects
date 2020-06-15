package com.epm.app.mvvm.procedure.repository;

import com.epm.app.mvvm.procedure.bussinessLogic.IProcedureServicesBL;
import com.epm.app.mvvm.procedure.network.ProcedureApiServices;

import com.epm.app.mvvm.procedure.network.request.DetailOfTheTransactionRequest;
import com.epm.app.mvvm.procedure.network.response.DetailOfTheTransactionResponse;

import com.epm.app.mvvm.procedure.network.request.ProcedureRequest;
import com.epm.app.mvvm.procedure.network.request.TypePersonRequest;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryResponse;
import com.epm.app.mvvm.procedure.network.response.FrequentProceduresResponse;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonResponse;


import javax.inject.Inject;

import io.reactivex.Observable;

public class ProcedureServicesRepository implements IProcedureServicesBL {

    private ProcedureApiServices procedureApiServices;

    @Inject
    public ProcedureServicesRepository(ProcedureApiServices procedureApiServices) {
        this.procedureApiServices = procedureApiServices;
    }

    @Override
    public Observable<GuideProceduresAndRequirementsCategoryResponse> getGuideProceduresAndRequirementsCategory(String token) {
        return procedureApiServices.getGuideProceduresAndRequirementsCategory(token);
    }

    @Override
    public Observable<DetailOfTheTransactionResponse> getDetailOfTheTransaction(String token, DetailOfTheTransactionRequest detailOfTheTransactionRequest) {
        return procedureApiServices.GetDetailProcedures(token, detailOfTheTransactionRequest);
    }

    @Override
    public Observable<FrequentProceduresResponse> getFrequentProcedures(ProcedureRequest procedureRequest,String token) {
        return procedureApiServices.getProcedures(procedureRequest,token);
    }

    @Override
    public Observable<TypePersonResponse> getTypePerson(TypePersonRequest typePersonRequest, String token) {
        return procedureApiServices.getTypePerson(typePersonRequest,token);
    }

}
