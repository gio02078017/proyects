package com.epm.app.eventos;

import com.epm.app.R;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.presenters.EventosPresenter;
import com.epm.app.view.views_activities.IEventosView;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.container_domain.eventos.EventosBussinesLogic;
import app.epm.com.container_domain.eventos.IEventosRepository;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class EventosPresenterTest {


    EventosPresenter eventosPresenter;
    EventosBussinesLogic eventosBussinesLogic;

    @Mock
    IEventosView eventosView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    IEventosRepository eventosRepository;


    @Before
    public void setUp() throws Exception {
        eventosBussinesLogic = Mockito.spy(new EventosBussinesLogic(eventosRepository));
        eventosPresenter = Mockito.spy(new EventosPresenter(eventosBussinesLogic));
        eventosPresenter.inject(eventosView, validateInternet);
    }

    @Test
    public void methodValidateinternetWithConnectionShouldCallMethodCreateThreadGetEvents(){
        when(validateInternet.isConnected()).thenReturn(true);
        eventosPresenter.validateInternetToGetEventos();
        verify(eventosPresenter).createThreadToGetEventos();
        verify(eventosView, never()).showAlertDialogToLoadAgain(eventosView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetWithOutConnectionShouldShowAlertDialog(){
        when(validateInternet.isConnected()).thenReturn(false);
        eventosPresenter.validateInternetToGetEventos();
        verify(eventosView).showAlertDialogToLoadAgain(eventosView.getName(), R.string.text_validate_internet);
        verify(eventosPresenter, never()).createThreadToGetEventos();
    }

    @Test
    public void methodCreateThreadToGetEventsShouldCallMethodGetEvents() throws RepositoryError {
        List<NoticiasEventos> noticiasEventos = new ArrayList<>();
        when(eventosRepository.getEventos()).thenReturn(noticiasEventos);
        eventosPresenter.getEventos();
        verify(eventosBussinesLogic).getEventos();
    }

    @Test
    public void methodGetEventsShouldShowAnAlertDialogWhenRetunrAnException() throws RepositoryError {
        List<NoticiasEventos> noticiasEventos = new ArrayList<>();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(eventosRepository.getEventos()).thenThrow(repositoryError);
        eventosPresenter.getEventos();
        verify(eventosView).showAlertDialogToLoadAgain(R.string.title_error, repositoryError.getMessage());
        verify(eventosView, never()).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);

    }

    @Test
    public void methodGetEventsShouldShowAnAlertdialogWhenReturnAnUnauthorized() throws RepositoryError {
        List<NoticiasEventos> noticiasEventos = new ArrayList<>();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(401);
        when(eventosRepository.getEventos()).thenThrow(repositoryError);
        eventosPresenter.getEventos();
        verify(eventosView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(eventosView, never()).showAlertDialogToLoadAgain(R.string.title_error, repositoryError.getMessage());
    }

}
