package com.epm.app.procedure;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.BaseTest;
import com.epm.app.TestObserver;
import com.epm.app.mvvm.procedure.network.response.Procedure;
import com.epm.app.mvvm.procedure.viewModel.ItemFrequentProcedureViewModel;
import com.google.common.truth.Truth;

import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mockito;


import java.util.Arrays;

public class ItemFrequentProcedureViewModelTest extends BaseTest {


    @InjectMocks
    private ItemFrequentProcedureViewModel itemFrequentProcedureViewModel;


    @Override
    public void setUp() throws Exception {
        super.setUp();
        itemFrequentProcedureViewModel = Mockito.spy(new ItemFrequentProcedureViewModel());

    }

    @Test
    public void getNameProcedure_getAProcessName(){

        Procedure procedure = new Procedure(true,"1", Arrays.asList(),"nombre1");
        itemFrequentProcedureViewModel.setProcedure(procedure);
        itemFrequentProcedureViewModel.drawInformation();
        MutableLiveData<String> item = itemFrequentProcedureViewModel.getNameProcedure();
        TestObserver.test(item)
                .assertHasValue()
                .assertValue(name -> name.equals(procedure.getName()));
    }

    @Test
    public void getNameProcedure_getAProcessNameIsEmpty(){

        itemFrequentProcedureViewModel.setProcedure(null);
        itemFrequentProcedureViewModel.drawInformation();
        MutableLiveData<String> item = itemFrequentProcedureViewModel.getNameProcedure();
        TestObserver.test(item)
                .assertNoValue();
    }

    @Test
    public void getProcedure_getAnInstanceOfAProcedure(){

        Procedure procedure = new Procedure(true,"1", Arrays.asList(),"nombre1");
        itemFrequentProcedureViewModel.setProcedure(procedure);
        Procedure result = itemFrequentProcedureViewModel.getProcedure();
        Truth.assert_()
                .that(result)
                .isNotNull();

    }

    @Test
    public void getProcedure_getAnInstanceOfANullProcedure(){

        itemFrequentProcedureViewModel.setProcedure(null);
        Procedure result = itemFrequentProcedureViewModel.getProcedure();
        Truth.assert_()
                .that(result)
                .isNull();

    }






}
