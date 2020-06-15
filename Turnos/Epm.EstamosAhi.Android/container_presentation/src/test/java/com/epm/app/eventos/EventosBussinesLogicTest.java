package com.epm.app.eventos;

import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.container_domain.eventos.EventosBussinesLogic;
import app.epm.com.container_domain.eventos.IEventosRepository;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class EventosBussinesLogicTest {

    EventosBussinesLogic eventosBussinesLogic;

    @Mock
    IEventosRepository eventosRepository;

    @Before
    public void setUp(){
        eventosBussinesLogic = new EventosBussinesLogic(eventosRepository);
    }

    @Test
    public void methodWithCorrectsParamtersShoudlCallMethodGetEventsInRepository() throws RepositoryError {
        eventosBussinesLogic.getEventos();
        verify(eventosRepository).getEventos();
    }

    @Test
    public void methodGetNewsShouldReturnAnListNotciasEventosWhenCallgetNewsInRepository() throws RepositoryError {
        List<NoticiasEventos> noticiasEventos = new ArrayList<>();
        when(eventosRepository.getEventos()).thenReturn(noticiasEventos);
        List<NoticiasEventos> noticiasEventosList = eventosBussinesLogic.getEventos();
        Assert.assertEquals(noticiasEventos, noticiasEventosList);
    }

}
