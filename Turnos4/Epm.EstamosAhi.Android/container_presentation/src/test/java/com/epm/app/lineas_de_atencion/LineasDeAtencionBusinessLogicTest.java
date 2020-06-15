package com.epm.app.lineas_de_atencion;

import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.container_domain.business_models.LineaDeAtencion;
import app.epm.com.container_domain.lineas_de_atencion.ILineasDeAtencionRepository;
import app.epm.com.container_domain.lineas_de_atencion.LineasDeAtencionBusinessLogic;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by root on 29/03/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class LineasDeAtencionBusinessLogicTest {

    LineasDeAtencionBusinessLogic lineasDeAtencionBusinessLogic;

    @Mock
    ILineasDeAtencionRepository lineasDeAtencionRepository;

    @Before
    public void setUp(){
        lineasDeAtencionBusinessLogic = new LineasDeAtencionBusinessLogic(lineasDeAtencionRepository);
    }

    @Test
    public void methoWithCorrectParametersShouldMethodGetLineasDeAtencionInRepository() throws RepositoryError {
        lineasDeAtencionBusinessLogic.getLineasDeAtencion();
        verify(lineasDeAtencionRepository).getLineasDeAtencion();
    }

    @Test
    public void methodGetLineasDeAtencionShouldReturnAnListLineaDeAtencionWhenCallGetLineasDeAtencionInRepository() throws RepositoryError {
        List<LineaDeAtencion> listaLineaDeAtencion = new ArrayList<>();
        when(lineasDeAtencionRepository.getLineasDeAtencion()).thenReturn(listaLineaDeAtencion);
        List<LineaDeAtencion> listaLineaDeAtencionBusinessLogic = lineasDeAtencionBusinessLogic.getLineasDeAtencion();
        Assert.assertEquals(listaLineaDeAtencion,listaLineaDeAtencionBusinessLogic);
    }
}
