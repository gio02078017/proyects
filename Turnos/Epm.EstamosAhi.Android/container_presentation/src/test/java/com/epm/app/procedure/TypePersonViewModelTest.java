package com.epm.app.procedure;

import com.epm.app.BaseTest;
import com.epm.app.R;
import com.epm.app.TestObserver;
import com.epm.app.mvvm.procedure.network.request.TypePersonRequest;
import com.epm.app.mvvm.procedure.network.response.ProcedureServiceMessage;
import com.epm.app.mvvm.procedure.network.response.TypePerson.MasterProcess;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonItem;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonResponse;
import com.epm.app.mvvm.procedure.repository.ProcedureServicesRepository;
import com.epm.app.mvvm.procedure.viewModel.TypePersonViewModel;
import com.google.common.truth.Truth;

import org.junit.Before;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.ConvertUtilities;
import io.reactivex.Observable;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotEquals;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class TypePersonViewModelTest extends BaseTest {

    @Mock
    ProcedureServicesRepository procedureServicesRepository;

    @InjectMocks
    TypePersonResponse typePersonResponse;

    @InjectMocks
    TypePersonRequest typePersonRequest;

    @InjectMocks
    ProcedureServiceMessage procedureServiceMessage;

    @InjectMocks
    MasterProcess masterProcess;

    private List<TypePersonItem> list;

    @Mock
    TypePersonItem typePersonItem;

    @Mock
    public ConvertUtilities convertUtilities;

    public ErrorMessage errorMessage;

    private TypePersonViewModel typePersonViewModel;

    String serviceId = "SERV1";
    String procedureId = "TRAM1";

    @Before
    public void setUp() throws Exception {
        super.setUp();
        list = new ArrayList<>();
        errorMessage = new ErrorMessage(0,0);
        convertUtilities =  mock(ConvertUtilities.class);
        procedureServicesRepository = mock(ProcedureServicesRepository.class);
        typePersonViewModel = Mockito.spy(new TypePersonViewModel(procedureServicesRepository ,validateInternet, customSharedPreferences));
        setMockTypePersonResponse();
        loadDatesRequest();
    }

    @Test
    public void fetchTypePerson_getTheDataTypePersonSuccessfully() {
        when(procedureServicesRepository.getTypePerson(typePersonRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(typePersonResponse));
        typePersonViewModel.fetchTypePerson(typePersonRequest);
        TestObserver.test(typePersonViewModel.getListTypePerson())
                .assertHasValue()
                .assertValue(it->it.size() > 0)
                .assertValue(it->it.size() == 2)
                .assertValue(it-> it.get(1).getTypePersonName().equals("Empresa"))
                .assertNever(it-> it.size() == 0);
    }

    @Test
    public void fetchTypePerson_whenGetTheResponseFromTheListOfTypePersonReturnsNull() {
        loadDatesRequest();
        when(procedureServicesRepository.getTypePerson(typePersonRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(null);
        typePersonViewModel.fetchTypePerson(typePersonRequest);
        TestObserver.test(typePersonViewModel.getListTypePerson())
                .assertNoValue();
    }

    @Test
    public void fetchTypePerson_whenTheListOfTypePersonIsObtainedItReturnsNull() {
        typePersonResponse.setMasterProcess(null);
        when(procedureServicesRepository.getTypePerson(typePersonRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(typePersonResponse));
        typePersonViewModel.fetchTypePerson(typePersonRequest);
        TestObserver.test(typePersonViewModel.getListTypePerson())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheTypePersonListIsEmpty(){
        masterProcess.setTypePersonItem(new ArrayList<>());
        typePersonViewModel.validateResponse(typePersonResponse);
        TestObserver.test(typePersonViewModel.getListTypePerson())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheTypePersonListIsNotEmpty(){
        masterProcess.setTypePersonItem(setMockListTypePerson(new ArrayList<>(),mockTypePerson()));
        typePersonViewModel.validateResponse(typePersonResponse);
        TestObserver.test(typePersonViewModel.getListTypePerson())
                .assertHasValue()
                .assertValue(it->it.size() > 0);
    }

    @Test
    public void validateResponse_whenTheTypePersonListIsNull(){
        typePersonResponse.setMasterProcess(null);
        typePersonViewModel.validateResponse(typePersonResponse);
        TestObserver.test(typePersonViewModel.getListTypePerson())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheTypePersonListIsNotNull(){
        masterProcess.setTypePersonItem(setMockListTypePerson(new ArrayList<>(),mockTypePerson()));
        typePersonViewModel.validateResponse(typePersonResponse);
        TestObserver.test(typePersonViewModel.getListTypePerson())
                .assertHasValue()
                .assertValue(it->it.size() > 0);
    }

    @Test
    public void getFirstTypePerson_whenReturnTheFirstServiceInTheListIsSuccess(){
        int position = 0;
        List<String> listService = new ArrayList<>();
        listService.add("Hogar");
        List<TypePersonItem> listTypePerson = setMockListTypePerson(new ArrayList<>(),listService);
        masterProcess.setTypePersonItem(listTypePerson);
        typePersonResponse.setMasterProcess(masterProcess);
        when(procedureServicesRepository.getTypePerson(typePersonRequest, customSharedPreferences.getString(Constants.TOKEN)))
                .thenReturn(Observable.just(typePersonResponse));
        typePersonViewModel.validateResponse(typePersonResponse);
        String service = typePersonViewModel.getTypePersonItem(position).getTypePersonName();
        Truth.assert_()
                .that(service)
                .isNotEmpty();
    }

    @Test
    public void getIconStatusButton_verifyThatTheGrayArrowIconReturnsWhenTheItemStatusIsInactive() {
        int icon = typePersonViewModel.getIconStatusButton(false);
        assertEquals(icon, R.drawable.ic_arrow_right_turns_gray);
    }

    @Test
    public void getIconStatusButton_verifyThatTheGrayArrowIconReturnsWhenTheItemStatusIsNotInactive() {
        int icon = typePersonViewModel.getIconStatusButton(true);
        assertNotEquals(icon, R.drawable.ic_arrow_right_turns_gray);
    }


    @Test
    public void getIconStatusButton_verifyThatTheGreenArrowIconReturnsWhenTheItemStatusIsInactive() {
        int icon = typePersonViewModel.getIconStatusButton(true);
        assertEquals(icon, R.drawable.ic_arrow_right_turns_green);
    }

    @Test
    public void getIconStatusButton_verifyThatTheGreenArrowIconReturnsWhenTheItemStatusIsNotInactive() {
        int icon = typePersonViewModel.getIconStatusButton(false);
        assertNotEquals(icon, R.drawable.ic_arrow_right_turns_green);
    }

    private void setMockTypePersonResponse(){
        List<TypePersonItem> list = new ArrayList<>();
        masterProcess.setTypePersonItem(
                setMockListTypePerson(list, mockTypePerson())
        );
        typePersonResponse.setMasterProcess(masterProcess);
        typePersonResponse.setMessage(setMockMessage());
    }

    private List<TypePersonItem> setMockListTypePerson(List<TypePersonItem> list, List<String> typePerson){
        for (int index = 0; index < 2; index++) {
            list.add(mockTypePerson(index, mockTypePerson().get(index)));
        }
        return list;
    }

    private TypePersonItem mockTypePerson(int identification, String typePerson){
        return new TypePersonItem(
                "M1DM"+identification,
                typePerson,
                true
        );
    }

    private List<String> mockTypePerson() {
        return Arrays.asList("Hogar","Empresa","Constructor");
    }

    private ProcedureServiceMessage setMockMessage(){
        procedureServiceMessage.setContent("Éxito genérico.");
        procedureServiceMessage.setIdentificator(1L);
        procedureServiceMessage.setTitle("");
        return procedureServiceMessage;
    }

    private void loadDatesRequest() {
        typePersonRequest.setProcessId(procedureId);
        typePersonRequest.setServiceId(serviceId);
    }

}