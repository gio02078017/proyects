package com.epm.app.turns;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import android.content.Context;
import android.content.res.Resources;

import com.epm.app.mvvm.turn.models.ChannelsOfAttentionMenu;
import com.epm.app.mvvm.turn.models.ChannelsOfAttentionMenuItem;
import com.epm.app.mvvm.turn.repository.JsonStringRepository;
import com.epm.app.mvvm.turn.viewModel.ChannelsOfAttentionViewModel;
import com.epm.app.mvvm.util.CallChannelsOfAttentionMenu;
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

public class ChannelsOfAttentionViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @InjectMocks
    CallChannelsOfAttentionMenu callChannelsOfAttentionMenu;

    @Mock
    JsonStringRepository jsonStringRepository;

    @Mock
    ChannelsOfAttentionMenu channelsOfAttentionMenu;

    @Mock
    ChannelsOfAttentionMenuItem channelsOfAttentionMenuItem;

    @InjectMocks
    ChannelsOfAttentionViewModel channelsOfAttentionViewModel;

    @Mock
    public Resources resources;

    @Mock
    public Context context;


    @Before
    public void setUp() throws Exception {
        jsonStringRepository = mock(JsonStringRepository.class);
        channelsOfAttentionViewModel.jsonStringRepository = jsonStringRepository;
        this.channelsOfAttentionMenu = mock(ChannelsOfAttentionMenu.class);
        this.channelsOfAttentionMenuItem = mock(ChannelsOfAttentionMenuItem.class);
    }

    @After
    public void tearDown() throws Exception {
    }

    @Test
    public void onCleared() {
    }

    @Test
    public void getChannelsOfAttentionMenuNull() {
        channelsOfAttentionMenu = new Gson().fromJson("", ChannelsOfAttentionMenu.class);
        when(jsonStringRepository.getDataChannelsOfAttentionMenu()).thenReturn(channelsOfAttentionMenu);
        channelsOfAttentionViewModel.getChannelsOfAttentionMenu();
        assertNull(channelsOfAttentionViewModel.getListChannelsOfAttention().getValue());
    }

    @Test
    public void getChannelsOfAttentionMenuNoNull() {
        channelsOfAttentionMenu = new Gson().fromJson("{\"Menu\" :\n" +
                "[\n" +
                "  {\n" +
                "    \"TextItemChannelsOfAttentionDescription\":\"Conoce todas nuestras líneas de atención y contáctanos si requieres atención.\",\n" +
                "    \"ImageItemChannelsOfAttention\":\"ic_call_us_channels_of_attention\",\n" +
                "    \"TextButtonChannelsOfAttention\": \"Llámanos\"\n" +
                "  },\n" +
                "  {\n" +
                "    \"TextItemChannelsOfAttentionDescription\":\"Conoce las oficinas más cercanas a tu ubicación y solicita un turno si lo requieres.\",\n" +
                "    \"ImageItemChannelsOfAttention\":\"ic_visit_us_channels_of_attention\",\n" +
                "    \"TextButtonChannelsOfAttention\": \"Visítanos\"\n" +
                "  }\n" +
                "]\n" +
                "}", ChannelsOfAttentionMenu.class);
        when(jsonStringRepository.getDataChannelsOfAttentionMenu()).thenReturn(channelsOfAttentionMenu);
        channelsOfAttentionViewModel.getChannelsOfAttentionMenu();
        assertNotNull(channelsOfAttentionViewModel.getListChannelsOfAttention().getValue());
    }
}