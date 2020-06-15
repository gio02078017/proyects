package com.epm.app.noticias;

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
import app.epm.com.container_domain.noticias.INoticiasRepository;
import app.epm.com.container_domain.noticias.NoticiasBussinesLogic;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class NoticiasBussinesLogicTest {

    NoticiasBussinesLogic noticiasBussinesLogic;

    @Mock
    INoticiasRepository noticiasRepository;

    @Before
    public void setUp(){
        noticiasBussinesLogic = new NoticiasBussinesLogic(noticiasRepository);
    }

    @Test
    public void methodWithCorrectsParamtersShoudlCallMethodGetNewsInRepository() throws RepositoryError {
        noticiasBussinesLogic.getNoticias();
        verify(noticiasRepository).getNoticias();
    }

    @Test
    public void methodGetNewsShouldReturnAnListNotciasEventosWhenCallgetNewsInRepository() throws RepositoryError {
        List<NoticiasEventos> noticiasEventos = new ArrayList<>();
        when(noticiasRepository.getNoticias()).thenReturn(noticiasEventos);
        List<NoticiasEventos> noticiasEventosList = noticiasBussinesLogic.getNoticias();
        Assert.assertEquals(noticiasEventos, noticiasEventosList);
    }

}
