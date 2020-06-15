package com.epm.app.noticias;

import com.epm.app.R;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.presenters.NoticiasPresenter;
import com.epm.app.view.views_activities.INoticiasView;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.container_domain.noticias.INoticiasRepository;
import app.epm.com.container_domain.noticias.NoticiasBussinesLogic;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */


@RunWith(MockitoJUnitRunner.class)
public class NoticiasPresenterTest {

    NoticiasPresenter noticiasPresenter;
    NoticiasBussinesLogic noticiasBussinesLogic;

    @Mock
    INoticiasView noticiasView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    INoticiasRepository noticiasRepository;


    @Before
    public void setUp() throws Exception {
        noticiasBussinesLogic = Mockito.spy(new NoticiasBussinesLogic(noticiasRepository));
        noticiasPresenter = Mockito.spy(new NoticiasPresenter(noticiasBussinesLogic));
        noticiasPresenter.inject(noticiasView, validateInternet);
    }

    @Test
    public void methodValidateinternetWithConnectionShouldCallMethodCreateThreadGetNoticias(){
        when(validateInternet.isConnected()).thenReturn(true);
        noticiasPresenter.validateInternetToGetNoticias();
        verify(noticiasPresenter).createThreadToGetNoticias();
        verify(noticiasView, never()).showAlertDialogToLoadAgain(noticiasView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetWithOutConnectionShouldShowAlertDialog(){
        when(validateInternet.isConnected()).thenReturn(false);
        noticiasPresenter.validateInternetToGetNoticias();
        verify(noticiasView).showAlertDialogToLoadAgain(noticiasView.getName(), R.string.text_validate_internet);
        verify(noticiasPresenter, never()).createThreadToGetNoticias();
    }

    @Test
    public void methodCreateThreadToGetNewsShouldCallMethodGetNews() throws RepositoryError {
        List<NoticiasEventos> noticiasEventos = new ArrayList<>();
        when(noticiasRepository.getNoticias()).thenReturn(noticiasEventos);
        noticiasPresenter.getNoticias();
        verify(noticiasBussinesLogic).getNoticias();
        verify(noticiasView).showInformationNoticias(noticiasEventos);
    }

    @Test
    public void methodGetNewsShouldShowAnAlertDialogWhenRetunrAnException() throws RepositoryError {
        List<NoticiasEventos> noticiasEventos = new ArrayList<>();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(noticiasRepository.getNoticias()).thenThrow(repositoryError);
        noticiasPresenter.getNoticias();
        verify(noticiasView).showAlertDialogToLoadAgain(R.string.title_error, repositoryError.getMessage());
        verify(noticiasView, never()).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);

    }

    @Test
    public void methodGetNewsShouldShowAnAlertdialogWhenReturnAnUnauthorized() throws RepositoryError {
        List<NoticiasEventos> noticiasEventos = new ArrayList<>();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(401);
        when(noticiasRepository.getNoticias()).thenThrow(repositoryError);
        noticiasPresenter.getNoticias();
        verify(noticiasView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(noticiasView, never()).showAlertDialogToLoadAgain(R.string.title_error, repositoryError.getMessage());
    }


}
