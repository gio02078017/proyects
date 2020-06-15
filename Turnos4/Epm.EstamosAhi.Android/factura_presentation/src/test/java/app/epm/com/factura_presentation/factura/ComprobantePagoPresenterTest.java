package app.epm.com.factura_presentation.factura;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.ComprobantePago;
import app.epm.com.facturadomain.business_models.FacturasPorTransaccion;
import app.epm.com.facturadomain.business_models.ProcesarInformacionPSE;
import app.epm.com.facturadomain.business_models.TransaccionPSEResponse;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.presenters.ComprobantePagoPresenter;
import app.epm.com.factura_presentation.view.views_activities.IComprobantePagoView;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by mateoquicenososa on 11/01/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class ComprobantePagoPresenterTest {

    @Mock
    IFacturaRepository facturaRepository;

    @Mock
    IComprobantePagoView comprobantePagoView;

    @Mock
    IValidateInternet validateInternet;

    FacturaBL facturaBL;

    ComprobantePagoPresenter comprobantePagoPresenter;

    @Before
    public void setUp() throws Exception {
        facturaBL = Mockito.spy(new FacturaBL(facturaRepository));
        comprobantePagoPresenter = Mockito.spy(new ComprobantePagoPresenter(facturaBL));
        comprobantePagoPresenter.inject(comprobantePagoView, validateInternet);
    }

    private ComprobantePago getComprobantePago() {
        ComprobantePago comprobantePago = new ComprobantePago();
        List<FacturasPorTransaccion> itemsFacturasPorTransaccion = new ArrayList<>();
        FacturasPorTransaccion facturasPorTransaccion = new FacturasPorTransaccion();
        facturasPorTransaccion.setDocumentoReferencia("test");
        facturasPorTransaccion.setFechaCreacion("test");
        facturasPorTransaccion.setIdFactura(1);
        facturasPorTransaccion.setIdFacturaPorTransaccion(1);
        itemsFacturasPorTransaccion.add(facturasPorTransaccion);
        comprobantePago.setCorreos("test");
        comprobantePago.setValorTotalPago(1);
        comprobantePago.setIdTransaccion(1);
        comprobantePago.setCodigoTrazabilidad("test");
        comprobantePago.setEstadoTransaccion(1);
        comprobantePago.setNombreEntidadFinanciera("test");
        comprobantePago.setDireccionIp("test");
        comprobantePago.setFacturasPorTransaccion(itemsFacturasPorTransaccion);
        return comprobantePago;
    }

    @Test
    public void methodValidateEmailToSendComprobanteWithEmptyEmailShouldShowAlertDialog() {
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePago.setCorreos(Constants.EMPTY_STRING);
        comprobantePagoPresenter.validateEmailToSendComprobante(comprobantePago);
        verify(comprobantePagoView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
    }

    @Test
    public void methodValidateEmailToSendComprobanteWithCorrectParametersShouldValidateEmail() {
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePagoPresenter.validateEmailToSendComprobante(comprobantePago);
        verify(comprobantePagoPresenter).validateParametersToSendComprobante(comprobantePago);
        verify(comprobantePagoView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
    }

    @Test
    public void methodValidateParametersToSendComprobanteWithCorrectEmailShoulCallMethodvalidateInternetToSendComprobante(){
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePago.setCorreos("lcz97@live.com");
        comprobantePagoPresenter.validateParametersToSendComprobante(comprobantePago);
        Assert.assertTrue(comprobantePago.getCorreos().matches(Constants.REGULAR_EXPRESSION_CORRECT_EMAIL));
        verify(comprobantePagoPresenter).validateInternetToSendComprobante(comprobantePago);
    }

    @Test
    public void methodValidateEmailToSendComprobanteWithoutTheRegularExpressionShouldShowAlertDialog() {
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePago.setCorreos("llll");
        comprobantePagoPresenter.validateParametersToSendComprobante(comprobantePago);
        verify(comprobantePagoView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
    }

    @Test
    public void methodValidateInternetToSendComprobanteWithoutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePagoPresenter.validateInternetToSendComprobante(comprobantePago);
        verify(comprobantePagoView).showAlertDialogGeneralInformationOnUiThread(comprobantePagoView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetToSendComprobanteWithoutConnectionShoudlCalllMethodCreateThreadTToSendComprobante() {
        when(validateInternet.isConnected()).thenReturn(true);
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePagoPresenter.validateInternetToSendComprobante(comprobantePago);
        verify(comprobantePagoPresenter).createThreadToSendComprobante(comprobantePago);
    }

    @Test
    public void methodCreateThreadToSendComprobanteShouldShowProgressDialog() {
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePagoPresenter.createThreadToSendComprobante(comprobantePago);
        verify(comprobantePagoView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodComprobantePagoShouldCallMethodComprobantePagoInSecurityBL() throws RepositoryError {
        ComprobantePago comprobantePago = getComprobantePago();
        Mensaje mensaje = new Mensaje();
        when(facturaRepository.comprobantePago(comprobantePago)).thenReturn(mensaje);
        comprobantePagoPresenter.comprobantePago(comprobantePago);
        verify(facturaBL).comprobantePago(comprobantePago);
        verify(comprobantePagoView).dismissProgressDialog();
    }

    @Test
    public void methodComprobantePagoShouldCallMethodShowAlertDialogSendComprobantePago() throws RepositoryError {
        ComprobantePago comprobantePago = getComprobantePago();
        Mensaje mensaje = new Mensaje();
        when(facturaRepository.comprobantePago(comprobantePago)).thenReturn(mensaje);
        comprobantePagoPresenter.comprobantePago(comprobantePago);
        verify(comprobantePagoView).showAlertDialogSendComprobantePago(mensaje);
        verify(comprobantePagoView).dismissProgressDialog();


    }

    @Test
    public void methodComprobantePagoShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        ComprobantePago comprobantePago = getComprobantePago();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.comprobantePago(comprobantePago)).thenThrow(repositoryError);
        comprobantePagoPresenter.comprobantePago(comprobantePago);
        verify(comprobantePagoView).showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
        verify(comprobantePagoView).dismissProgressDialog();
    }

    @Test
    public void methodComprobantePagoShouldShowAnAlertDialogWhenReturnAnUnAuthorized() throws RepositoryError {
        ComprobantePago comprobantePago = getComprobantePago();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(facturaRepository.comprobantePago(comprobantePago)).thenThrow(repositoryError);
        comprobantePagoPresenter.comprobantePago(comprobantePago);
        verify(comprobantePagoView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(comprobantePagoView).dismissProgressDialog();
    }

    @Test
    public void methodComprobantePagoShouldCallMethodHideProgressDialog() throws RepositoryError {
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePagoPresenter.comprobantePago(comprobantePago);
        verify(comprobantePagoView).dismissProgressDialog();
    }


    private ProcesarInformacionPSE getProcesarInformacionPSE(){
        ProcesarInformacionPSE procesarInformacionPSE = new ProcesarInformacionPSE();
        ArrayList<FacturasPorTransaccion> itemsFacturasTransaccion = new ArrayList<>();
        procesarInformacionPSE.setEstadoTransaccion(1);
        procesarInformacionPSE.setCodigoTrazabilidad("test");
        procesarInformacionPSE.setEntidadFinanciera(1);
        procesarInformacionPSE.setIdTransaccion(1);
        procesarInformacionPSE.setFacturasPorTransaccion(itemsFacturasTransaccion);
        return procesarInformacionPSE;
    }

    @Test
    public void methodValidateInternetProcesarInformacionPSEWithoutConnectionShouldShowAlertDialog() {
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        when(validateInternet.isConnected()).thenReturn(false);
        comprobantePagoPresenter.validateInternetProcesarInformacionPSE(procesarInformacionPSE);
        verify(comprobantePagoView).showAlertDialogGeneralLoadAgain(comprobantePagoView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetProcesarInformacionPSEWithConnectionShouldShowCallMethodCreateThreadProcesarInformacionPSE() {
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        when(validateInternet.isConnected()).thenReturn(true);
        comprobantePagoPresenter.validateInternetProcesarInformacionPSE(procesarInformacionPSE);
        verify(comprobantePagoPresenter).createThreadProcesarInformacionPSE(procesarInformacionPSE);
    }

    @Test
    public void methodCreateThreadIProcesarInformacionPSEShouldShowProgressDialog() {
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        comprobantePagoPresenter.createThreadProcesarInformacionPSE(procesarInformacionPSE);
        verify(comprobantePagoView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodProcesarInformacionPSEShouldCallMethodProcesarInformacionPSEInSecurityBL() throws RepositoryError {
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        TransaccionPSEResponse transaccionPSEResponse = new TransaccionPSEResponse();
        when(facturaRepository.procesarInformacionPSE(procesarInformacionPSE)).thenReturn(transaccionPSEResponse);
        comprobantePagoPresenter.procesarInformacionPSE(procesarInformacionPSE);
        verify(facturaBL).procesarInformacionPSE(procesarInformacionPSE);
        verify(comprobantePagoView).dismissProgressDialog();
    }

    @Test
    public void methodProcesarInformacionPSEShouldCallMethodSaveTransaccionPSEResponse() throws RepositoryError {
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        TransaccionPSEResponse transaccionPSEResponse = new TransaccionPSEResponse();
        when(facturaRepository.procesarInformacionPSE(procesarInformacionPSE)).thenReturn(transaccionPSEResponse);
        comprobantePagoPresenter.procesarInformacionPSE(procesarInformacionPSE);
        verify(comprobantePagoView).saveTransaccionPSEResponse(transaccionPSEResponse);
        verify(comprobantePagoView).dismissProgressDialog();
    }

    @Test
    public void methodProcesarInformacionPSEShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.procesarInformacionPSE(procesarInformacionPSE)).thenThrow(repositoryError);
        comprobantePagoPresenter.procesarInformacionPSE(procesarInformacionPSE);
        verify(comprobantePagoView).showAlertDialogGeneralLoadAgain(R.string.title_error, repositoryError.getMessage());
        verify(comprobantePagoView).dismissProgressDialog();
    }

    @Test
    public void methodProcesarInformacionShouldShowAnAlertDialogWhenReturnAnUnAuthorized() throws RepositoryError {
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(facturaRepository.procesarInformacionPSE(procesarInformacionPSE)).thenThrow(repositoryError);
        comprobantePagoPresenter.procesarInformacionPSE(procesarInformacionPSE);
        verify(comprobantePagoView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(comprobantePagoView).dismissProgressDialog();
    }

    @Test
    public void methodProcesarInformacionPSEShouldCallMethodHideProgressDialog() throws RepositoryError {
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        comprobantePagoPresenter.procesarInformacionPSE(procesarInformacionPSE);
        verify(comprobantePagoView).dismissProgressDialog();
    }


}
