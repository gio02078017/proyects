package com.epm.app.procedure;

import android.content.res.Resources;

import com.epm.app.BaseTest;
import com.epm.app.TestObserver;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
import com.epm.app.mvvm.procedure.network.request.DetailOfTheTransactionRequest;
import com.epm.app.mvvm.procedure.network.response.DetailOfTheTransactionResponse;
import com.epm.app.mvvm.procedure.network.response.DetailTransactionResponse;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryItem;
import com.epm.app.mvvm.procedure.network.response.Procedure;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonItem;
import com.epm.app.mvvm.procedure.repository.ProcedureServicesRepository;
import com.epm.app.mvvm.procedure.viewModel.DetailsOfTheTransactionViewModel;

import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;

import static junit.framework.TestCase.assertEquals;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

public class DetailsOfTheTransactionViewModelTest extends BaseTest {


    private DetailsOfTheTransactionViewModel detailsOfTheTransactionViewModel;

    @Mock
    ProcedureServicesRepository procedureServicesRepository;

    @Mock
    Resources resources;

    @InjectMocks
    ProcedureInformation procedureInformation;

    boolean redirect;



    @InjectMocks
    GuideProceduresAndRequirementsCategoryItem guideProceduresAndRequirementsCategoryItem;

    @InjectMocks
    TypePersonItem typePersonItem;

    String listMasterDetails;

    @InjectMocks
    Procedure procedure;

    @InjectMocks
    DetailOfTheTransactionResponse detailOfTheTransactionResponse;

    @InjectMocks
    DetailTransactionResponse detailTransactionResponse;

    @InjectMocks
    DetailOfTheTransactionRequest detailOfTheTransactionRequest;




    @Override
    public void setUp() throws Exception {
        super.setUp();
        resources = mock(Resources.class);
        procedureServicesRepository = mock(ProcedureServicesRepository.class);
        detailsOfTheTransactionViewModel = Mockito.spy(new DetailsOfTheTransactionViewModel(validateInternet,
                procedureServicesRepository,customSharedPreferences,resources));
    }

    @Test
    public void testLoadDetailOfTheTransaction(){
        loadProcedureInformation();
        when(procedureServicesRepository.getDetailOfTheTransaction(customSharedPreferences.getString(Constants.TOKEN),detailOfTheTransactionRequest)).thenReturn(Observable.just(detailOfTheTransactionResponse));
        detailsOfTheTransactionViewModel.loadDetailOfTheTransaction(procedureInformation,redirect);
        verify(detailsOfTheTransactionViewModel).loadRequest(procedureInformation);
    }

    @Test
    public void testHandleResponseIsNotNull(){
        loadProcedureInformation();
        detailsOfTheTransactionViewModel.handleResponse(detailOfTheTransactionResponse);
        verify(detailsOfTheTransactionViewModel).loadData(detailOfTheTransactionResponse);
    }

    @Test
    public void testHandleResponseIsNull(){
        detailOfTheTransactionResponse = null;
        loadProcedureInformation();
        detailsOfTheTransactionViewModel.handleResponse(detailOfTheTransactionResponse);
        verify(detailsOfTheTransactionViewModel,times(0)).loadData(detailOfTheTransactionResponse);
    }

    @Test
    public void testLoadDataIfDetailTransactionIsNull(){
        detailsOfTheTransactionViewModel.loadData(detailOfTheTransactionResponse);
        TestObserver.test(detailsOfTheTransactionViewModel.getDetailOfFormalitiesGroups())
                .assertNoValue();
    }

    @Test
    public void testLoadDataIfDetailTransactionIsNotNull(){
        detailTransactionResponse.setQueEs("que es");
        detailTransactionResponse.setQueNecesito("que necesito");
        detailOfTheTransactionResponse.setDetailTransactionResponse(detailTransactionResponse);
        detailsOfTheTransactionViewModel.loadData(detailOfTheTransactionResponse);
        TestObserver.test(detailsOfTheTransactionViewModel.getDetailOfFormalitiesGroups())
                .assertHasValue();
    }

    @Test
    public void testLoadRequest(){
        loadProcedureInformation();
        detailsOfTheTransactionViewModel.loadRequest(procedureInformation);
        assertEquals(detailsOfTheTransactionViewModel.getDetailOfTheTransactionRequest().getIdProcedure()
        ,procedureInformation.getIdProcedure());
        assertEquals(detailsOfTheTransactionViewModel.getDetailOfTheTransactionRequest().getIdService(),procedureInformation.getIdService());
        assertEquals(detailsOfTheTransactionViewModel.getDetailOfTheTransactionRequest().getMasterDetails(),procedureInformation.getIdMasterDetails());
    }

    @Test
    public void testPushButtonProcess(){
        detailOfTheTransactionResponse.setDetailTransactionResponse(detailTransactionResponse);
        detailsOfTheTransactionViewModel.loadData(detailOfTheTransactionResponse);
        detailsOfTheTransactionViewModel.pushButtonStartProcess();
        assertEquals(detailsOfTheTransactionViewModel.getPushBottom().getValue(),detailOfTheTransactionResponse);
    }

    private void loadProcedureInformation(){
        procedureInformation.setIdService("idService");
        procedureInformation.setIdProcedure("idProcedure");
        procedureInformation.setIdMasterDetails("idMaster");
    }


}
