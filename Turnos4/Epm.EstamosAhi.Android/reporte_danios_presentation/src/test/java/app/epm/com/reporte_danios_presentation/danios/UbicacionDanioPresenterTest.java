package app.epm.com.reporte_danios_presentation.danios;

import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.ExpectedException;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.InformacionDeUbicacion;

import app.epm.com.reporte_danios_domain.danios.danios.DaniosBL;
import app.epm.com.reporte_danios_domain.danios.danios.IDaniosRepository;
import app.epm.com.reporte_danios_presentation.R;
import app.epm.com.reporte_danios_presentation.presenters.UbicacionDanioPresenter;
import app.epm.com.reporte_danios_presentation.view.views_activities.IUbicacionDanioView;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Matchers.anyObject;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by josetabaresramirez on 14/02/17.
 */
@RunWith(MockitoJUnitRunner.class)
public class UbicacionDanioPresenterTest {

    @Rule
    public ExpectedException expectedException = ExpectedException.none();

    @Mock
    IDaniosRepository daniosRepository;
    @Mock
    IUbicacionDanioView ubicacionDanioView;
    @Mock
    IValidateInternet validateInternet;
    DaniosBL daniosBL;
    UbicacionDanioPresenter ubicacionDanioPresenter;

    @Before
    public void setUp() throws Exception {
        daniosBL = Mockito.spy(new DaniosBL(daniosRepository));
        ubicacionDanioPresenter = Mockito.spy(new UbicacionDanioPresenter(daniosBL));
        ubicacionDanioPresenter.inject(ubicacionDanioView, validateInternet);
    }

    @Test
    public void methodValidateInternetToExecuteAnActionShouldShowAlertDialogWhenThereIsNotConnection() {
        when(validateInternet.isConnected()).thenReturn(false);

        ubicacionDanioPresenter.validateInternetToExecuteAnAction((Runnable) anyObject());

        verify(ubicacionDanioView).showAlertDialogGeneralInformationOnUiThread(ubicacionDanioView.getName(), R.string.text_validate_internet);
        verify(ubicacionDanioPresenter, never()).createThreadToExecuteAnAction((Runnable) anyObject());
    }

    @Test
    public void methodValidateInternetToExecuteAnActionShouldCallCreateThreadWhenThereIsConnection() {
        when(validateInternet.isConnected()).thenReturn(true);

        Runnable runnable = anyObject();

        ubicacionDanioPresenter.validateInternetToExecuteAnAction(runnable);

        verify(ubicacionDanioPresenter).createThreadToExecuteAnAction(runnable);
        verify(ubicacionDanioView, never()).showAlertDialogGeneralInformationOnUiThread(ubicacionDanioView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodCreateThreadToExecuteAnActionShouldCallShowProgressDialog() {
        ubicacionDanioPresenter.createThreadToExecuteAnAction((Runnable) anyObject());
        verify(ubicacionDanioView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodGetInformacionDeUbicacionhouldCallMethodGetInformacionDeUbicacionFromDaniosBL() throws RepositoryError {
        String lat = "75.55";
        String lon = "-6.566";
        ubicacionDanioPresenter.getInformacionDeUbicacion(lat, lon);
        verify(daniosBL).getInformacionDeUbicacion(lat, lon);
        verify(ubicacionDanioView).dismissProgressDialog();
    }

    @Test
    public void methodGetInformacionDeUbicacionShouldCallMethodValidateInformacionDeUbicacion() throws RepositoryError {
        String lat = "75.55";
        String lon = "-6.566";
        ubicacionDanioPresenter.getInformacionDeUbicacion(lat, lon);
        verify(ubicacionDanioPresenter).validateInformacionDeUbicacion(null);
        verify(ubicacionDanioView).dismissProgressDialog();
    }

    @Test
    public void methodGetInformacionDeUbicacionShouldShowAlertDialogWhenRepositoryThrowsAnError() throws Exception {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        String lat = "75.55";
        String lon = "-6.566";
        when(daniosRepository.getInformacionDeUbicacionWithAddress(lat, lon)).thenThrow(repositoryError);

        ubicacionDanioPresenter.getInformacionDeUbicacion(lat, lon);

        verify(ubicacionDanioView).showAlertDialogGeneralInformationOnUiThread(ubicacionDanioView.getName(), repositoryError.getMessage());
        verify(ubicacionDanioView).dismissProgressDialog();
    }

    @Test
    public void methodValidateInformacionDeUbicacionShouldCallMethodLoadUbicationInViewWhenDataIsInstanciated() {
        InformacionDeUbicacion informacionDeUbicacion = new InformacionDeUbicacion();
        informacionDeUbicacion.setPais(Constants.COUNTRYCODE);
        ubicacionDanioPresenter.validateInformacionDeUbicacion(informacionDeUbicacion);

        verify(ubicacionDanioView).loadUbicationOnUiThread(informacionDeUbicacion);
        verify(ubicacionDanioView, never()).showAlertWithOutAddress(ubicacionDanioView.getName(), R.string.text_ubicacion_no_encontrada);
    }

    @Test
    public void methodValidateInformacionDeUbicacionShouldShowAlertDialogIfDataIsNull() {
        ubicacionDanioPresenter.validateInformacionDeUbicacion(null);

        verify(ubicacionDanioView).showAlertWithOutAddress(ubicacionDanioView.getName(), R.string.text_ubicacion_no_encontrada);
        verify(ubicacionDanioView, never()).loadUbicationOnUiThread(null);
    }

    @Test
    public void methodHasCoberturaShouldReturnTrueIfBelloIsInMunicipiosList() throws RepositoryError {

        List<String> municipiosExpected = new ArrayList<>();
        municipiosExpected.add("BELLO");
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(daniosRepository.getMunicipios(parametrosCobertura)).thenReturn(municipiosExpected);

        Assert.assertTrue(ubicacionDanioPresenter.hasCobertura(parametrosCobertura));
    }

    @Test
    public void methodHasCoberturaShouldReturnFalseIfBelloIsNotInMunicipiosList() throws RepositoryError {

        List<String> municipiosExpected = new ArrayList<>();
        municipiosExpected.add("Sabaneta");
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(daniosRepository.getMunicipios(parametrosCobertura)).thenReturn(municipiosExpected);

        Assert.assertFalse(ubicacionDanioPresenter.hasCobertura(parametrosCobertura));
    }

    @Test
    public void methodHasCoberturaShouldThrowsWhenRepositoryThrowsAnError() throws RepositoryError {

        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(daniosRepository.getMunicipios(parametrosCobertura)).thenThrow(repositoryError);

        expectedException.expect(RepositoryError.class);
        expectedException.expectMessage(Constants.DEFAUL_ERROR);

        ubicacionDanioPresenter.hasCobertura(parametrosCobertura);
    }

    @Test
    public void methodValidateCoberturaAndTipoServicioStartProccessToCompleteDanageReport() throws RepositoryError {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();

        ubicacionDanioPresenter.validateCoberturaAndTipoServicio(parametrosCobertura);

        verify(ubicacionDanioPresenter).hasCobertura(parametrosCobertura);
    }

    @Test
    public void methodValidateCoberturaAndTipoServicioShouldCallMethodValidateCoberturaAndTipoServicioIluminariaIfTipoServicioIsEnergia() throws RepositoryError {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        parametrosCobertura.setTipoServicio(2);

        ubicacionDanioPresenter.validateCoberturaAndTipoServicio(parametrosCobertura);

        verify(ubicacionDanioPresenter).validateCoberturaAndTipoServicioIluminaria(false, parametrosCobertura);
    }

    @Test
    public void methodValidateCoberturaAndTipoServicioShouldCallMethodValidateGeneralCoberturaIfTipoServicioIsNotEnergia() {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        parametrosCobertura.setTipoServicio(1);

        ubicacionDanioPresenter.validateCoberturaAndTipoServicio(parametrosCobertura);

        verify(ubicacionDanioPresenter).validateGeneralCobertura(false, parametrosCobertura);
    }

    @Test
    public void methodvalidateCoberturaAndTipoServicioShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.DEFAUL_ERROR_CODE);
        when(ubicacionDanioPresenter.hasCobertura(parametrosCobertura)).thenThrow(repositoryError);
        ubicacionDanioPresenter.validateCoberturaAndTipoServicio(parametrosCobertura);
        verify(ubicacionDanioView).showAlertDialogGeneralInformationOnUiThread(ubicacionDanioView.getName(), repositoryError.getMessage());
    }

    @Test
    public void methodvalidateCoberturaAndTipoServicioShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        RepositoryError repositoryError = new RepositoryError("");
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(ubicacionDanioPresenter.hasCobertura(parametrosCobertura)).thenThrow(repositoryError);
        ubicacionDanioPresenter.validateCoberturaAndTipoServicio(parametrosCobertura);
        verify(ubicacionDanioView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
    }

    @Test
    public void methodValidateCoberturaAndTipoServicioIluminariaShouldReturnAnErrorIfTipoServicioIsNotIluminaria() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        ubicacionDanioPresenter.validateCoberturaAndTipoServicioIluminaria(false, parametrosCobertura);
    }

    @Test
    public void methodValidateCoberturaAndTipoServicioIluminariaShouldCallMethodValidateCoberturasEnergiaAndIluminaria() throws RepositoryError {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        parametrosCobertura.setTipoServicio(0);
        ubicacionDanioPresenter.validateCoberturaAndTipoServicioIluminaria(true, parametrosCobertura);
        verify(ubicacionDanioPresenter).validateCoberturasEnergiaAndIluminaria(true, false, parametrosCobertura);
    }

    @Test
    public void methodValidateGeneralCoberturaShouldCallMethodStartDescribeDanioAcivityIfHasCobertura() {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        ubicacionDanioPresenter.validateGeneralCobertura(true, parametrosCobertura);
        verify(ubicacionDanioView).getListTypeDanio(false, false);
    }

    @Test
    public void methodValidateGeneralCoberturaShouldCallAlertDialogIfHasNotCoberturaInAntioquia() {

        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        ubicacionDanioPresenter.validateGeneralCobertura(false, parametrosCobertura);

        String message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_AND_ON_ANTIOQUIA, ETipoServicio.Agua.getName(), parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
        verify(ubicacionDanioView).showAlertDialogGeneralInformationOnUiThread(null, message);
    }

    @Test
    public void methodValidateGeneralCoberturaShouldCallAlertDialogIfHasNotCoberturaInNotAntioquia() {

        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        parametrosCobertura.setDepartamento("CALDAS");
        ubicacionDanioPresenter.validateGeneralCobertura(false, parametrosCobertura);

        String message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_AND_NOT_ON_ANTIOQUIA, parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
        verify(ubicacionDanioView).showAlertDialogGeneralInformationOnUiThread(null, message);
    }

    @Test
    public void methodValidateCoberturasEnergiaAndIluminariaWithCoberturaShouldStartDescribeDanioActivity() {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        ubicacionDanioPresenter.validateCoberturasEnergiaAndIluminaria(true, true, parametrosCobertura);

        verify(ubicacionDanioView).getListTypeDanio(true, true);
    }

    @Test
    public void methodValidateCoberturasEnergiaAndIluminariaWithOnlyCoberturaEnergiaAndAntioquiaShouldShouldALertDialogToStartDescribeDanioActivity() {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        ubicacionDanioPresenter.validateCoberturasEnergiaAndIluminaria(true, false, parametrosCobertura);


        String message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_ILUMINARIA_AND_ON_ANTIOQUIA, parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
        verify(ubicacionDanioView).showAlertDialogToStartDescribeDanioActivityOnUiThread(null, message, true, false);
    }

    @Test
    public void methodValidateCoberturasEnergiaAndIluminariaWithOnlyCoberturaEnergiaAndNotAntioquiaShouldShouldALertDialogToStartDescribeDanioActivity() {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        parametrosCobertura.setDepartamento("CALDAS");
        ubicacionDanioPresenter.validateCoberturasEnergiaAndIluminaria(true, false, parametrosCobertura);


        String message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_ILUMINARIA_AND_NOT_ON_ANTIOQUIA, parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
        verify(ubicacionDanioView).showAlertDialogGeneralInformationOnUiThread(null, message);
    }

    @Test
    public void methodValidateCoberturasEnergiaAndIluminariaWithOnlyCoberturaIluminariaShouldShouldALertDialogToStartDescribeDanioActivity() {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        ubicacionDanioPresenter.validateCoberturasEnergiaAndIluminaria(false, true, parametrosCobertura);


        String message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_ENERGIA, parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
        verify(ubicacionDanioView).showAlertDialogToStartDescribeDanioActivityOnUiThread(null, message, false, true);
    }

    @Test
    public void methodValidateCoberturasEnergiaAndIluminariaWithoutCoberturaOnAntioquiaShouldShowALertDialog() {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        ubicacionDanioPresenter.validateCoberturasEnergiaAndIluminaria(false, false, parametrosCobertura);


        String message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_AND_ON_ANTIOQUIA, "Energía", parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
        verify(ubicacionDanioView).showAlertDialogGeneralInformationOnUiThread(null, message);
    }

    @Test
    public void methodValidateCoberturasEnergiaAndIluminariaWithoutCoberturaOnNotAntioquiaShouldShowALertDialog() {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        parametrosCobertura.setDepartamento("CALDAS");
        ubicacionDanioPresenter.validateCoberturasEnergiaAndIluminaria(false, false, parametrosCobertura);


        String message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_AND_NOT_ON_ANTIOQUIA, parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
        verify(ubicacionDanioView).showAlertDialogGeneralInformationOnUiThread(null, message);
    }

    private ParametrosCobertura getParametrosCobertura() {
        ParametrosCobertura parametrosCobertura = new ParametrosCobertura();
        parametrosCobertura.setTipoServicio(ETipoServicio.Agua.getValue());
        parametrosCobertura.setMunicipio("Bello");
        parametrosCobertura.setDepartamento("ANTIOQUIA");
        return parametrosCobertura;
    }
}
