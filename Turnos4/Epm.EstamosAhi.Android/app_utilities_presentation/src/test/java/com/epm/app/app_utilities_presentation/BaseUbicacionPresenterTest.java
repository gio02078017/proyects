package com.epm.app.app_utilities_presentation;

import com.epm.app.app_utilities_presentation.presenters.BaseUbicacionDeFraudeOrDanioPresenter;
import com.epm.app.app_utilities_presentation.views.views_activities.IBaseUbicacionDeFraudeOrDanioView;
import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.runners.MockitoJUnitRunner;

import app.epm.com.app_utilities_domain.ubicacion_danios_fraudes.BaseUbicacionDaniosOrFraudesBusinessLogic;
import app.epm.com.app_utilities_domain.ubicacion_danios_fraudes.IBaseUbicacionDaniosOrFraudesRepository;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by mateoquicenososa on 12/1/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class BaseUbicacionPresenterTest {

    @Mock
    IBaseUbicacionDeFraudeOrDanioView baseUbicacionDeFraudeOrDanioView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    IBaseUbicacionDaniosOrFraudesRepository baseUbicacionDaniosOrFraudesRepository;

    BaseUbicacionDeFraudeOrDanioPresenter baseUbicacionDeFraudeOrDanioPresenter;

    BaseUbicacionDaniosOrFraudesBusinessLogic baseUbicacionDaniosOrFraudesBusinessLogic;

    @Before
    public void setUp() throws Exception {
        baseUbicacionDaniosOrFraudesBusinessLogic = Mockito.spy(new BaseUbicacionDaniosOrFraudesBusinessLogic(baseUbicacionDaniosOrFraudesRepository));
        baseUbicacionDeFraudeOrDanioPresenter = Mockito.spy(new BaseUbicacionDeFraudeOrDanioPresenter(baseUbicacionDaniosOrFraudesBusinessLogic));
        baseUbicacionDeFraudeOrDanioPresenter.inject(baseUbicacionDeFraudeOrDanioView, validateInternet);
    }

    private InformacionDeUbicacion getInformacionDeUbicacion() {
        InformacionDeUbicacion informacionDeUbicacion = new InformacionDeUbicacion();
        informacionDeUbicacion.setDeparatamento("test");
        informacionDeUbicacion.setDireccion("test");
        informacionDeUbicacion.setMunicipio("test");
        informacionDeUbicacion.setPais("test");
        return informacionDeUbicacion;
    }

    @Test
    public void methodValidateInternetToExecuteAnActionWithoutConnectionShouldShowAlertDialog() {
        Runnable runnable = null;
        when(validateInternet.isConnected()).thenReturn(false);
        baseUbicacionDeFraudeOrDanioPresenter.validateInternetToExecuteAnAction(runnable);
        verify(baseUbicacionDeFraudeOrDanioView).showAlertDialogGeneralInformationOnUiThread(baseUbicacionDeFraudeOrDanioView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetToExecuteAnActionWithoutConnectionShouldCallMethodCreateThreadToExecuteAnAction() {
        Runnable runnable = null;
        when(validateInternet.isConnected()).thenReturn(true);
        baseUbicacionDeFraudeOrDanioPresenter.validateInternetToExecuteAnAction(runnable);
        verify(baseUbicacionDeFraudeOrDanioPresenter).createThreadToExecuteAnAction(runnable);
    }

    @Test
    public void methodCreateThreadToExecuteAnActionShouldShowProgressDialog() {
        Runnable runnable = null;
        baseUbicacionDeFraudeOrDanioPresenter.createThreadToExecuteAnAction(runnable);
        verify(baseUbicacionDeFraudeOrDanioView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodGetInformacionDeUbicacionShouldCallMethodGetInformacionDeUbicacionInTheBusinessLogic() throws RepositoryError {
        String lat = "111111";
        String lon = "521542";
        InformacionDeUbicacion informacionDeUbicacion = getInformacionDeUbicacion();
        when(baseUbicacionDaniosOrFraudesBusinessLogic.getInformacionDeUbicacion(lat, lon)).thenReturn(informacionDeUbicacion);
        baseUbicacionDeFraudeOrDanioPresenter.getInformacionDeUbicacion(lat, lon);
        verify(baseUbicacionDeFraudeOrDanioPresenter).getInformacionDeUbicacion(lat, lon);
        verify(baseUbicacionDeFraudeOrDanioView).dismissProgressDialog();
    }

    @Test
    public void methodConsultarConfiguracionSomosShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        String lat = "111111";
        String lon = "521542";
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.DEFAUL_ERROR_CODE);
        when(baseUbicacionDaniosOrFraudesBusinessLogic.getInformacionDeUbicacion(lat, lon)).thenThrow(repositoryError);
        baseUbicacionDeFraudeOrDanioPresenter.getInformacionDeUbicacion(lat, lon);
        verify(baseUbicacionDeFraudeOrDanioView).showAlertDialogGeneralInformationOnUiThread(baseUbicacionDeFraudeOrDanioView.getName(), repositoryError.getMessage());
        verify(baseUbicacionDeFraudeOrDanioView).dismissProgressDialog();
    }

    @Test
    public void methodvalidateInformacionDeUbicacionShouldCallShowAlertWithOutAddress() {
        InformacionDeUbicacion informacionDeUbicacion = getInformacionDeUbicacion();
        baseUbicacionDeFraudeOrDanioPresenter.validateInformacionDeUbicacion(informacionDeUbicacion);
        verify(baseUbicacionDeFraudeOrDanioView).showAlertWithOutAddress(baseUbicacionDeFraudeOrDanioView.getName(), R.string.text_ubicacion_no_encontrada);
    }
}
