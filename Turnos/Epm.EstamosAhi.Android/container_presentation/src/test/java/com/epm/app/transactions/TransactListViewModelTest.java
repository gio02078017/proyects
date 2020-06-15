package com.epm.app.transactions;

import com.epm.app.BaseTest;
import com.epm.app.TestObserver;
import com.epm.app.mvvm.transactions.models.Transaction;
import com.epm.app.mvvm.transactions.models.TransactionServiceMessage;
import com.epm.app.mvvm.transactions.network.request.TransactionRequest;
import com.epm.app.mvvm.transactions.network.response.TransactionListResponse;
import com.epm.app.mvvm.transactions.repository.TransactionRepository;
import com.epm.app.mvvm.transactions.viewModel.TransactionListViewModel;
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

public class TransactListViewModelTest extends BaseTest {

    @InjectMocks
    public TransactionListViewModel transactionListViewModel;

    @Mock
    TransactionRepository transactionRepository;

    @InjectMocks
    TransactionListResponse TransactionListResponse;

    @InjectMocks
    TransactionServiceMessage TransactionServiceMessage;

    @Mock
    TransactionRequest TransactionRequest;


    @Override
    public void setUp() throws Exception {
        super.setUp();
        transactionRepository = mock(TransactionRepository.class);
        transactionListViewModel = Mockito.spy(new TransactionListViewModel(transactionRepository, validateInternet, customSharedPreferences ));
        //Cargar parametros de entrada y salida
        setMockFrequentTransactionsResponse();
    }


    @Test
    public void fetchFrequentTransactions_getTheDataFrequentTransactionsSuccessfully() {
        //Falsear respuesta del servicio
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(TransactionListResponse));
        //Se llama el metodo a testear
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        // Validacion de la respuesta
        TestObserver.test(transactionListViewModel.getListTransaction())
                .assertHasValue()
                .assertValue(it->it.size() > 0)
                .assertValue(it->it.size() == 2)
                .assertNever(it-> it.size() == 0);
    }


    @Test
    public void fetchFrequentTransactions_whenGetTheResponseFromTheListOfFrequentTransactionsReturnsNull() {
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(null);
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        TestObserver.test(transactionListViewModel.getListTransaction())
                .assertNoValue();
    }


    @Test
    public void fetchFrequentTransactions_whenTheListOfFrequentTransactionsIsObtainedItReturnsNull() {
        TransactionListResponse.setTransaction(null);
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(TransactionListResponse));
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        TestObserver.test(transactionListViewModel.getListTransaction())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheStatusOfTheTransactionIsTrue(){
        TransactionListResponse.setTransactionState(true);
        transactionListViewModel.validateResponse(TransactionListResponse);
        TestObserver.test(transactionListViewModel.getListTransaction())
                .assertHasValue()
                .assertValue(it->it.size() > 0);
    }

    @Test
    public void validateResponse_whenTheStatusOfTheTransactionIsFalse(){
        TransactionListResponse.setTransactionState(false);
        transactionListViewModel.validateResponse(TransactionListResponse);
        TestObserver.test(transactionListViewModel.getListTransaction())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheTransactionListIsEmpty(){
        TransactionListResponse.setTransactionState(true);
        TransactionListResponse.setTransaction(new ArrayList<>());
        transactionListViewModel.validateResponse(TransactionListResponse);
        TestObserver.test(transactionListViewModel.getListTransaction())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheTransactionListIsNotEmpty(){
        TransactionListResponse.setTransaction(setMockListTransaction(new ArrayList<>(),mockServices()));
        transactionListViewModel.validateResponse(TransactionListResponse);
        TestObserver.test(transactionListViewModel.getListTransaction())
                .assertHasValue()
                .assertValue(it->it.size() > 0);
    }

    @Test
    public void validateResponse_whenTheTransactionListIsNull(){
        TransactionListResponse.setTransactionState(true);
        TransactionListResponse.setTransaction(null);
        transactionListViewModel.validateResponse(TransactionListResponse);
        TestObserver.test(transactionListViewModel.getListTransaction())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheTransactionListIsNotNull(){
        TransactionListResponse.setTransaction(setMockListTransaction(new ArrayList<>(), mockServices()));
        transactionListViewModel.validateResponse(TransactionListResponse);
        TestObserver.test(transactionListViewModel.getListTransaction())
                .assertHasValue()
                .assertValue(it->it.size() > 0);
    }

    @Test
    public void getFirstService_whenReturnTheFirstServiceInTheListIsSuccess(){
        int position = 0;
        List<String> listService = new ArrayList<>();
        listService.add("SERV1");
        List<Transaction> listTransaction = setMockListTransaction(new ArrayList<>(),listService);
        TransactionListResponse.setTransaction(listTransaction);
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(TransactionListResponse));
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        String service = transactionListViewModel.getFirstService(position);
        Truth.assert_()
                .that(service)
                .isNotEmpty();
    }

    @Test
    public void getFirstService_whenTheFirstServiceOnTheListDoesNotExist(){
        int position = 0;
        List<Transaction> listTransaction = setMockListTransaction(new ArrayList<>(),new ArrayList<>());
        TransactionListResponse.setTransaction(listTransaction);
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(TransactionListResponse));
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        String service = transactionListViewModel.getFirstService(position);
        Truth.assert_()
                .that(service)
                .isEmpty();
    }

    @Test
    public void getFirstService_whenTheListOfServicesIsNull(){
        int position = 0;
        List<Transaction> listTransaction = setMockListTransaction(new ArrayList<>(),null);
        TransactionListResponse.setTransaction(listTransaction);
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(TransactionListResponse));
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        String service = transactionListViewModel.getFirstService(position);
        Truth.assert_()
                .that(service)
                .isEmpty();
    }


    @Test
    public void getServices_whenTheListOfServices(){
        List<String> list = transactionListViewModel.getServices(mockTransaction(0,Arrays.asList("serv1","serv2")));
        Truth.assert_()
                .that(list)
                .isNotNull();
    }

    @Test
    public void getServices_whenTheListOfServicesIsNull(){
        List<String> list = transactionListViewModel.getServices(mockTransaction(0,null));
        Truth.assert_()
                .that(list)
                .isNull();
    }

    @Test
    public void getTransaction_getTheIdOfAProcessInstance(){
        int position = 0;
        List<Transaction> listTransaction = setMockListTransaction(new ArrayList<>(),new ArrayList<>());
        TransactionListResponse.setTransaction(listTransaction);
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(TransactionListResponse));
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        String id = transactionListViewModel.getIdTransaction(position);
        Truth.assert_()
                .that(id)
                .isNotEmpty();

    }

    @Test
    public void getTransaction_getTheIdOfAProcessInstanceIsEmpty(){
        TransactionListResponse.setTransaction(null);
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(TransactionListResponse));
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        String name = transactionListViewModel.getNameTransaction(0);
        Truth.assert_()
                .that(name)
                .isEmpty();

    }

    @Test
    public void getTransaction_getTheNameOfAProcessInstance(){
        int position = 0;
        List<Transaction> listTransaction = setMockListTransaction(new ArrayList<>(),new ArrayList<>());
        TransactionListResponse.setTransaction(listTransaction);
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(TransactionListResponse));
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        String name = transactionListViewModel.getNameTransaction(position);
        Truth.assert_()
                .that(name)
                .isNotEmpty();
    }

    @Test
    public void getTransaction_getTheNameOfAProcessInstanceIsEmpty(){
        TransactionListResponse.setTransaction(null);
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(TransactionListResponse));
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        String id = transactionListViewModel.getIdTransaction(0);
        Truth.assert_()
                .that(id)
                .isEmpty();

    }

    @Test
    public void isTransactionNotNullOrEmpty_getTheNameOfAProcessInstance(){

        List<Transaction> listTransaction = setMockListTransaction(new ArrayList<>(),new ArrayList<>());
        TransactionListResponse.setTransaction(listTransaction);
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(TransactionListResponse));
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        boolean resul = transactionListViewModel.isTransactionNotNullOrEmpty();
        Truth.assert_()
                .that(resul)
                .isTrue();
    }

    @Test
    public void isTransactionNotNullOrEmpty_getTheNameOfAProcessInstanceIsEmpty(){
        TransactionListResponse.setTransaction(null);
        when(transactionRepository.getFastTransactions(TransactionRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(TransactionListResponse));
        transactionListViewModel.fetchFastTransactions(TransactionRequest);
        boolean resul = transactionListViewModel.isTransactionNotNullOrEmpty();
        Truth.assert_()
                .that(resul)
                .isFalse();

    }





    private void setMockFrequentTransactionsResponse(){
        List<Transaction> list = new ArrayList<>();
        TransactionListResponse.setTransactionState(true);
        TransactionListResponse.setTransaction(
                setMockListTransaction(list, mockServices())
        );
        TransactionListResponse.setTransactionServiceMessage(setMockMessage());
    }

    private List<Transaction> setMockListTransaction(List<Transaction> list, List<String> services){
        for (int index = 0; index < 2; index++) {
            list.add(mockTransaction(index,services));
        }
        return list;
    }

    private Transaction mockTransaction(int identification, List<String> services){
        return new Transaction(
                ""+identification,
                "TRAM"+identification,
                "",
                true,
                services

        );
    }

    private List<String> mockServices() {
        return Arrays.asList("SERV1");
    }

    private TransactionServiceMessage setMockMessage(){
        TransactionServiceMessage.setContent("Éxito genérico.");
        TransactionServiceMessage.setIdentificator(1L);
        TransactionServiceMessage.setTitle("");
        return TransactionServiceMessage;
    }


}
