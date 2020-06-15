package com.epm.app.lineas_de_atencion;

import com.epm.app.R;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.presenters.LineasDeAtencionPresenter;
import com.epm.app.view.views_activities.ILineasDeAtencionView;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.container_domain.business_models.LineaDeAtencion;
import app.epm.com.container_domain.lineas_de_atencion.LineasDeAtencionBusinessLogic;
import app.epm.com.container_domain.lineas_de_atencion.ILineasDeAtencionRepository;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by root on 29/03/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class LineasDeAtencionPresenterTest {


    LineasDeAtencionPresenter lineasDeAtencionPresenter;
    LineasDeAtencionBusinessLogic lineasDeAtencionBusinessLogic;

    @Mock
    ILineasDeAtencionRepository lineasDeAtencionRepository;

    @Mock
    ILineasDeAtencionView lineasDeAtencionView;

    @Mock
    IValidateInternet validateInternet;


    @Mock
    ICustomSharedPreferences customSharedPreferences;


    @Before
    public void setUp() throws Exception {
        lineasDeAtencionBusinessLogic = Mockito.spy(new LineasDeAtencionBusinessLogic(lineasDeAtencionRepository));
        lineasDeAtencionPresenter = Mockito.spy(new LineasDeAtencionPresenter(lineasDeAtencionBusinessLogic));
        lineasDeAtencionPresenter.inject(lineasDeAtencionView, validateInternet);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadToGetLineasDeAtencion() {
        when(validateInternet.isConnected()).thenReturn(true);
        lineasDeAtencionPresenter.getValidateInternetGetLineasDeAtencion();
        verify(lineasDeAtencionPresenter).createThreadToGetLineasDeAtencion();
        verify(lineasDeAtencionView, never()).showAlertDialogGeneralInformationOnUiThread(lineasDeAtencionView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetWithOutConnectionShouldShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        lineasDeAtencionPresenter.getValidateInternetGetLineasDeAtencion();
        verify(lineasDeAtencionView).showAlertDialogGeneralInformationOnUiThread(lineasDeAtencionView.getName(), R.string.text_validate_internet);
        verify(lineasDeAtencionPresenter, never()).createThreadToGetLineasDeAtencion();
    }

    @Test
    public void methodCreateThreadToGetIncidenteShouldShowProgressDialog() {
        lineasDeAtencionPresenter.createThreadToGetLineasDeAtencion();
        verify(lineasDeAtencionView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodGetLineasDeAtencionShouldCallGetLineasDeAtencionBusinessLogic() throws RepositoryError {
        List<LineaDeAtencion> lineaDeAtencion = new ArrayList<>();
        when(lineasDeAtencionRepository.getLineasDeAtencion()).thenReturn(lineaDeAtencion);
        lineasDeAtencionPresenter.getLineasDeAtencion();
        verify(lineasDeAtencionBusinessLogic).getLineasDeAtencion();
        verify(lineasDeAtencionView).dismissProgressDialog();
    }

    @Test
    public void methodGetLineasDeAtencionShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(401);
        when(lineasDeAtencionRepository.getLineasDeAtencion()).thenThrow(repositoryError);
        lineasDeAtencionPresenter.getLineasDeAtencion();
        verify(lineasDeAtencionView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(lineasDeAtencionView, never()).showAlertDialogToLoadAgainOnUiThread(R.string.title_error, repositoryError.getMessage());
        verify(lineasDeAtencionView).dismissProgressDialog();
    }

    @Test
    public void methodGetLineasDeAtencionShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);

        when(lineasDeAtencionRepository.getLineasDeAtencion()).thenThrow(repositoryError);
        lineasDeAtencionPresenter.getLineasDeAtencion();
        verify(lineasDeAtencionView).showAlertDialogToLoadAgainOnUiThread(R.string.title_error, repositoryError.getMessage());
        verify(lineasDeAtencionView).dismissProgressDialog();
        verify(lineasDeAtencionView, never()).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
    }

    @Test
    public void methodGetIncidenteShouldCallMethodHideProgressDialog() throws RepositoryError {
        List<LineaDeAtencion> lineaDeAtencion = new ArrayList<>();
        when(lineasDeAtencionRepository.getLineasDeAtencion()).thenReturn(lineaDeAtencion);
        lineasDeAtencionPresenter.getLineasDeAtencion();
        verify(lineasDeAtencionBusinessLogic).getLineasDeAtencion();
        verify(lineasDeAtencionView).dismissProgressDialog();
    }

}
