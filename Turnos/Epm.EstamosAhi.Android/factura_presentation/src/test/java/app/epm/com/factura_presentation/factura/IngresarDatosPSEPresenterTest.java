package app.epm.com.factura_presentation.factura;


import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.DataPagar;
import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.business_models.InformacionPSE;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.presenters.IngresarDatosPSEPresenter;
import app.epm.com.factura_presentation.view.views_activities.IIngresarDatosPSEView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by mateoquicenososa on 6/01/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class IngresarDatosPSEPresenterTest {

    IngresarDatosPSEPresenter datosPSEPresenter;

    FacturaBL facturaBL;

    @Mock
    IFacturaRepository facturaRepository;

    @Mock
    IIngresarDatosPSEView datosPSEView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    ICustomSharedPreferences customSharedPreferences;


    @Before
    public void setUp() throws Exception {
        facturaBL = Mockito.spy(new FacturaBL(facturaRepository));
        datosPSEPresenter = Mockito.spy(new IngresarDatosPSEPresenter(facturaBL));
        datosPSEPresenter.inject(datosPSEView, validateInternet);
    }

    private DataPagar getDataPagar() {
        DataPagar dataPagar = new DataPagar();
        FacturaResponse facturaResponse = new FacturaResponse();
        List<FacturaResponse> itemsFactura = new ArrayList<>();
        facturaResponse.setDescripcionContrato("test");
        facturaResponse.setId(1);
        facturaResponse.setDocumentoReferencia("test");
        facturaResponse.setFechaCreacion("test");
        facturaResponse.setFechaRecargo("test");
        facturaResponse.setFechaVencimiento("test");
        facturaResponse.setNumeroContrato("123");
        facturaResponse.setNumeroFactura("123");
        facturaResponse.setValorFactura(1);
        facturaResponse.setUrl("test");
        facturaResponse.setEstadoPagoFactura(true);
        facturaResponse.setFacturaVencida(false);
        facturaResponse.setEstaSeleccionadaParaPago(true);
        facturaResponse.setEstaPendiente(false);
        facturaResponse.setCode(1);
        facturaResponse.setText("test");
        itemsFactura.add(facturaResponse);
        dataPagar.setEntidadFinanciera(1);
        dataPagar.setIdTipoDocumento(1);
        dataPagar.setIdTipoPersona(1);
        dataPagar.setNumeroDocumento("123");
        dataPagar.setFacturasPagar(itemsFactura);
        return dataPagar;
    }

    @Test
    public void methodValidateInternetWithoutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        DataPagar dataPagar = getDataPagar();
        datosPSEPresenter.validateInternetDatosPagar(dataPagar);
        verify(datosPSEView).showAlertDialogGeneralInformationOnUiThread(datosPSEView.getName(), R.string.text_validate_internet);
        verify(datosPSEPresenter, never()).createThreadToDatosPagar(dataPagar);
    }

    @Test
    public void methodValidateInternetWithConnectionShoudlShowCallMethodCreateThreadToDatosPagar() {
        when(validateInternet.isConnected()).thenReturn(true);
        DataPagar dataPagar = getDataPagar();
        datosPSEPresenter.validateInternetDatosPagar(dataPagar);
        verify(datosPSEView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        verify(datosPSEPresenter).createThreadToDatosPagar(dataPagar);
    }

    @Test
    public void methodCreateThreadToDatosPagarShouldShowProgressDialog() {
        DataPagar dataPagar = getDataPagar();
        datosPSEPresenter.createThreadToDatosPagar(dataPagar);
        verify(datosPSEView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodDatosPagarShouldCallMethodLoginInSecurityBL() throws RepositoryError {
        DataPagar dataPagar = getDataPagar();
        InformacionPSE informacionPSE = new InformacionPSE();
        when(facturaRepository.datosPagar(dataPagar)).thenReturn(informacionPSE);
        datosPSEPresenter.datosPagar(dataPagar);
        verify(facturaBL).datosPagar(dataPagar);
        verify(datosPSEView).dismissProgressDialog();
    }

    @Test
    public void methodDatosPagarShouldCallMessageDatosPagarResult() throws RepositoryError {
        DataPagar dataPagar = getDataPagar();
        InformacionPSE informacionPSE = new InformacionPSE();
        when(facturaRepository.datosPagar(dataPagar)).thenReturn(informacionPSE);
        datosPSEPresenter.datosPagar(dataPagar);
        verify(datosPSEView).startPagePSE(dataPagar.getFacturasPagar());
        verify(datosPSEView).dismissProgressDialog();
    }

    @Test
    public void methodDatosPagarShouldCallMethodSaveInformacionPSE() throws RepositoryError {
        DataPagar dataPagar = getDataPagar();
        InformacionPSE informacionPSE = new InformacionPSE();
        when(facturaRepository.datosPagar(dataPagar)).thenReturn(informacionPSE);
        datosPSEPresenter.datosPagar(dataPagar);
        verify(datosPSEView).saveInformacionPSE(informacionPSE);
        verify(datosPSEView).startPagePSE(dataPagar.getFacturasPagar());
        verify(datosPSEView).dismissProgressDialog();
    }

    @Test
    public void methodDatosPagarShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        DataPagar dataPagar = getDataPagar();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.datosPagar(dataPagar)).thenThrow(repositoryError);
        datosPSEPresenter.datosPagar(dataPagar);
        verify(datosPSEView).showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
        verify(datosPSEView).dismissProgressDialog();
    }

    @Test
    public void methodDatosPagarShouldShowAnAlertDialogWhenReturnAnUnAuthorized() throws RepositoryError {
        DataPagar dataPagar = getDataPagar();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(facturaRepository.datosPagar(dataPagar)).thenThrow(repositoryError);
        datosPSEPresenter.datosPagar(dataPagar);
        verify(datosPSEView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(datosPSEView).dismissProgressDialog();
    }

    @Test
    public void methodDatosPagarShouldCallMethodHideProgressDialog() throws RepositoryError {
        DataPagar dataPagar = getDataPagar();
        InformacionPSE informacionPSE = new InformacionPSE();
        when(facturaRepository.datosPagar(dataPagar)).thenReturn(informacionPSE);
        datosPSEPresenter.datosPagar(dataPagar);
        verify(datosPSEView).dismissProgressDialog();
    }
}
