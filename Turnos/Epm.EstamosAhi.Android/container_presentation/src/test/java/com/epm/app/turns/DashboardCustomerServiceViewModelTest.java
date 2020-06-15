package com.epm.app.turns;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import android.content.Context;
import android.content.res.Resources;

import com.epm.app.mvvm.turn.models.CustomerServiceMenu;
import com.epm.app.mvvm.turn.models.CustomerServiceMenuItem;
import com.epm.app.mvvm.turn.repository.JsonStringRepository;
import com.epm.app.mvvm.turn.viewModel.DashboardCustomerServiceViewModel;
import com.epm.app.mvvm.util.CallCustomerMenu;
import com.google.gson.Gson;

import org.junit.After;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.MockitoJUnit;
import org.mockito.junit.MockitoRule;
import static org.junit.Assert.*;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class DashboardCustomerServiceViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @InjectMocks
    CallCustomerMenu callCustomerMenu;

    @Mock
    JsonStringRepository jsonStringRepository;

    @Mock
    CustomerServiceMenu customerServiceMenu;

    @Mock
    CustomerServiceMenuItem customerServiceMenuItem;

    @InjectMocks
    DashboardCustomerServiceViewModel dashboardCustomerServiceViewModel;

    @Mock
    public Resources resources;

    @Mock
    public Context context;

    @Before
    public void setUp() throws Exception {
        jsonStringRepository = mock(JsonStringRepository.class);
        dashboardCustomerServiceViewModel.jsonStringRepository = jsonStringRepository;
        this.customerServiceMenu = mock(CustomerServiceMenu.class);
        this.customerServiceMenuItem = mock(CustomerServiceMenuItem.class);

    }

    @After
    public void tearDown() throws Exception {
    }

    @Test
    public void getCustomerServiceMenuNull() {
        customerServiceMenu = new Gson().fromJson("", CustomerServiceMenu.class);
        when(jsonStringRepository.getDataMenu()).thenReturn(customerServiceMenu);
        dashboardCustomerServiceViewModel.getCustomerServiceMenu();
        assertNull(dashboardCustomerServiceViewModel.getListCustomerServiceMenu().getValue());
    }

    @Test
    public void getCustomerServiceMenuNoNull() {
        customerServiceMenu = new Gson().fromJson("{\"Menu\" :\n" +
                "  [\n" +
                "    {\n" +
                "      \"TextItemCustomerServiceDescription\":\"¿Quieres saber cómo hacer un trámite y cuales son sus requisitos?\",\n" +
                "      \"ImageItemCustomerService\":\"ic_turn_procedures_and_requirements\",\n" +
                "      \"TextButtonCustomerService\": \"Guía de trámites y requisitos\"\n" +
                "    },\n" +
                "    {\n" +
                "      \"TextItemCustomerServiceDescription\":\"¿Deseas contactarte con nosotros,llamarnos ubicar nuestras oficinas o escribirnos?\",\n" +
                "      \"ImageItemCustomerService\":\"ic_turn_our_service_channels\",\n" +
                "      \"TextButtonCustomerService\": \"Nuestros canales de atención\"\n" +
                "    }\n" +
                "  ]\n" +
                "}", CustomerServiceMenu.class);
        when(jsonStringRepository.getDataMenu()).thenReturn(customerServiceMenu);
        dashboardCustomerServiceViewModel.getCustomerServiceMenu();
        assertNotNull(dashboardCustomerServiceViewModel.getListCustomerServiceMenu().getValue());
    }
}