package com.epm.app.procedure;


import com.epm.app.BaseTest;
import com.epm.app.TestObserver;
import com.epm.app.mvvm.procedure.network.request.ProcedureRequest;
import com.epm.app.mvvm.procedure.network.response.FrequentProceduresResponse;
import com.epm.app.mvvm.procedure.network.response.Procedure;
import com.epm.app.mvvm.procedure.network.response.ProcedureServiceMessage;
import com.epm.app.mvvm.procedure.repository.ProcedureServicesRepository;
import com.epm.app.mvvm.procedure.viewModel.FrequentProceduresViewModel;
import com.google.common.truth.Truth;

import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;

import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class FrequentProceduresViewModelTest extends BaseTest {


    @InjectMocks
    public FrequentProceduresViewModel frequentProceduresViewModel;

    @Mock
    ProcedureServicesRepository procedureServicesRepository;

    @InjectMocks
    FrequentProceduresResponse frequentProceduresResponse;

    @InjectMocks
    ProcedureServiceMessage procedureServiceMessage;

    @Mock
    ProcedureRequest procedureRequest;

    String serviceId = "SERV1";

    @Override
    public void setUp() throws Exception {
        super.setUp();
        procedureServicesRepository = mock(ProcedureServicesRepository.class);
        frequentProceduresViewModel = Mockito.spy(new FrequentProceduresViewModel(procedureServicesRepository, validateInternet, customSharedPreferences ));
        //Cargar parametros de entrada y salida
        setMockFrequentProceduresResponse();
        loadDatesRequest();
    }


    @Test
    public void fetchFrequentProcedures_getTheDataFrequentProceduresSuccessfully() {
        //Falsear respuesta del servicio
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(frequentProceduresResponse));
        //Se llama el metodo a testear
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
        // Validacion de la respuesta
        TestObserver.test(frequentProceduresViewModel.getListProcedure())
                .assertHasValue()
                .assertValue(it->it.size() > 0)
                .assertValue(it->it.size() == 2)
                .assertValue(it-> it.get(1).getName().equals("Tramite1"))
                .assertNever(it-> it.size() == 0);
    }


    @Test
    public void fetchFrequentProcedures_whenGetTheResponseFromTheListOfFrequentProceduresReturnsNull() {
        loadDatesRequest();
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(null);
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
        TestObserver.test(frequentProceduresViewModel.getListProcedure())
                .assertNoValue();
    }


    @Test
    public void fetchFrequentProcedures_whenTheListOfFrequentProceduresIsObtainedItReturnsNull() {
        frequentProceduresResponse.setTramites(null);
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(frequentProceduresResponse));
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
        TestObserver.test(frequentProceduresViewModel.getListProcedure())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheStatusOfTheTransactionIsTrue(){
        frequentProceduresResponse.setTransactionState(true);
        frequentProceduresViewModel.validateResponse(frequentProceduresResponse);
        TestObserver.test(frequentProceduresViewModel.getListProcedure())
                .assertHasValue()
                .assertValue(it->it.size() > 0);
    }

    @Test
    public void validateResponse_whenTheStatusOfTheTransactionIsFalse(){
        frequentProceduresResponse.setTransactionState(false);
        frequentProceduresViewModel.validateResponse(frequentProceduresResponse);
        TestObserver.test(frequentProceduresViewModel.getListProcedure())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheProcedureListIsEmpty(){
        frequentProceduresResponse.setTransactionState(true);
        frequentProceduresResponse.setTramites(new ArrayList<>());
        frequentProceduresViewModel.validateResponse(frequentProceduresResponse);
        TestObserver.test(frequentProceduresViewModel.getListProcedure())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheProcedureListIsNotEmpty(){
        frequentProceduresResponse.setTramites(setMockListProcedure(new ArrayList<>(),mockServices()));
        frequentProceduresViewModel.validateResponse(frequentProceduresResponse);
        TestObserver.test(frequentProceduresViewModel.getListProcedure())
                .assertHasValue()
                .assertValue(it->it.size() > 0);
    }

    @Test
    public void validateResponse_whenTheProcedureListIsNull(){
        frequentProceduresResponse.setTransactionState(true);
        frequentProceduresResponse.setTramites(null);
        frequentProceduresViewModel.validateResponse(frequentProceduresResponse);
        TestObserver.test(frequentProceduresViewModel.getListProcedure())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheProcedureListIsNotNull(){
        frequentProceduresResponse.setTramites(setMockListProcedure(new ArrayList<>(), mockServices()));
        frequentProceduresViewModel.validateResponse(frequentProceduresResponse);
        TestObserver.test(frequentProceduresViewModel.getListProcedure())
                .assertHasValue()
                .assertValue(it->it.size() > 0);
    }

    @Test
    public void getFirstService_whenReturnTheFirstServiceInTheListIsSuccess(){
        int position = 0;
        List<String> listService = new ArrayList<>();
        listService.add("SERV1");
        List<Procedure> listprocedure = setMockListProcedure(new ArrayList<>(),listService);
        frequentProceduresResponse.setTramites(listprocedure);
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(frequentProceduresResponse));
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
       String service = frequentProceduresViewModel.getFirstService(position);
        Truth.assert_()
                .that(service)
                .isNotEmpty();
    }

    @Test
    public void getFirstService_whenTheFirstServiceOnTheListDoesNotExist(){
        int position = 0;
        List<Procedure> listprocedure = setMockListProcedure(new ArrayList<>(),new ArrayList<>());
        frequentProceduresResponse.setTramites(listprocedure);
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(frequentProceduresResponse));
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
        String service = frequentProceduresViewModel.getFirstService(position);
        Truth.assert_()
                .that(service)
                .isEmpty();
    }

    @Test
    public void getFirstService_whenTheListOfServicesIsNull(){
        int position = 0;
        List<Procedure> listprocedure = setMockListProcedure(new ArrayList<>(),null);
        frequentProceduresResponse.setTramites(listprocedure);
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(frequentProceduresResponse));
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
        String service = frequentProceduresViewModel.getFirstService(position);
        Truth.assert_()
                .that(service)
                .isEmpty();
    }


    @Test
    public void getServices_whenTheListOfServices(){
        List<String> list = frequentProceduresViewModel.getServices(mockProcedure(0,Arrays.asList("serv1","serv2")));
        Truth.assert_()
                .that(list)
                .isNotNull();
    }

    @Test
    public void getServices_whenTheListOfServicesIsNull(){
        List<String> list = frequentProceduresViewModel.getServices(mockProcedure(0,null));
        Truth.assert_()
                .that(list)
                .isNull();
    }

    @Test
    public void getProcedure_getTheIdOfAProcessInstance(){
        int position = 0;
        List<Procedure> listprocedure = setMockListProcedure(new ArrayList<>(),new ArrayList<>());
        frequentProceduresResponse.setTramites(listprocedure);
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(frequentProceduresResponse));
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
        String id = frequentProceduresViewModel.getIdProcedure(position);
        Truth.assert_()
                .that(id)
                .isNotEmpty();

    }

    @Test
    public void getProcedure_getTheIdOfAProcessInstanceIsEmpty(){
        frequentProceduresResponse.setTramites(null);
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(frequentProceduresResponse));
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
        String name = frequentProceduresViewModel.getNameProcedure(0);
        Truth.assert_()
                .that(name)
                .isEmpty();

    }

    @Test
    public void getProcedure_getTheNameOfAProcessInstance(){
        int position = 0;
        List<Procedure> listprocedure = setMockListProcedure(new ArrayList<>(),new ArrayList<>());
        frequentProceduresResponse.setTramites(listprocedure);
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(frequentProceduresResponse));
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
        String name = frequentProceduresViewModel.getNameProcedure(position);
        Truth.assert_()
                .that(name)
                .isNotEmpty();
    }

    @Test
    public void getProcedure_getTheNameOfAProcessInstanceIsEmpty(){
        frequentProceduresResponse.setTramites(null);
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(frequentProceduresResponse));
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
        String id = frequentProceduresViewModel.getIdProcedure(0);
        Truth.assert_()
                .that(id)
                .isEmpty();

    }

    @Test
    public void isProcedureNotNullOrEmpty_getTheNameOfAProcessInstance(){

        List<Procedure> listprocedure = setMockListProcedure(new ArrayList<>(),new ArrayList<>());
        frequentProceduresResponse.setTramites(listprocedure);
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(frequentProceduresResponse));
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
        boolean resul = frequentProceduresViewModel.isProcedureNotNullOrEmpty();
        Truth.assert_()
                .that(resul)
                .isTrue();
    }

    @Test
    public void isProcedureNotNullOrEmpty_getTheNameOfAProcessInstanceIsEmpty(){
        frequentProceduresResponse.setTramites(null);
        when(procedureServicesRepository.getFrequentProcedures(procedureRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(frequentProceduresResponse));
        frequentProceduresViewModel.fetchFrequentProcedures(procedureRequest);
        boolean resul = frequentProceduresViewModel.isProcedureNotNullOrEmpty();
        Truth.assert_()
                .that(resul)
                .isFalse();

    }





    private void setMockFrequentProceduresResponse(){
        List<Procedure> list = new ArrayList<>();
        frequentProceduresResponse.setTransactionState(true);
        frequentProceduresResponse.setTramites(
                setMockListProcedure(list, mockServices())
        );
        frequentProceduresResponse.setMensaje(setMockMessage());
    }

    private List<Procedure> setMockListProcedure(List<Procedure> list, List<String> services){
        for (int index = 0; index < 2; index++) {
            list.add(mockProcedure(index,services));
        }
        return list;
    }

    private Procedure mockProcedure(int identification, List<String> services){
        return new Procedure(
                true,
                "TRAM"+identification,
                services,
                "Transaction"+identification
        );
    }

    private List<String> mockServices() {
       return Arrays.asList("SERV1");
    }

    private ProcedureServiceMessage setMockMessage(){
        procedureServiceMessage.setContent("Éxito genérico.");
        procedureServiceMessage.setIdentificator(1L);
        procedureServiceMessage.setTitle("");
        return procedureServiceMessage;
    }

    private void loadDatesRequest() {
        procedureRequest.setServiceId(serviceId);
    }



}
