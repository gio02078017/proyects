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

import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.presenters.ConsultFacturaPresenter;
import app.epm.com.factura_presentation.view.views_activities.IConsultFacturaView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.ArgumentMatchers.nullable;
import static org.mockito.Matchers.anyInt;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by ocadavid on 15/12/2016.
 */
@RunWith(MockitoJUnitRunner.Silent.class)
public class ConsultFacturaPresenterTest {
    ConsultFacturaPresenter consultFacturaPresenter;
    FacturaBL facturaBL;
    String number = "123";

    @Mock
    IFacturaRepository facturaRepository;

    @Mock
    IConsultFacturaView consultFacturaView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    ICustomSharedPreferences customSharedPreferences;


    @Before
    public void setUp() {
        facturaBL = Mockito.spy(new FacturaBL(facturaRepository));
        consultFacturaPresenter = Mockito.spy(new ConsultFacturaPresenter(facturaBL));
        consultFacturaPresenter.inject(consultFacturaView, validateInternet);
    }

    /**
     * Start Consultar factura por referente de pago.
     */

    @Test
    public void methodValidateFieldsWithEmptyNumberShouldShowAlertDialog() {
        String number = Constants.EMPTY_STRING;
        consultFacturaPresenter.validateFieldsToConsultFactura(number);
        verify(consultFacturaView).showAlertDialogGeneralInformationOnUiThread(app.epm.com.security_presentation.R.string.title_empty_fields, app.epm.com.security_presentation.R.string.text_digitar_numero);
    }

    @Test
    public void methodValidateInternetToConsultFacturaShouldShowAlertDialog() {
        consultFacturaPresenter.validateInternetToConsultFactura();
        verify(consultFacturaView).showAlertDialogGeneralInformationOnUiThread(consultFacturaView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodConsulFacturaPorReferenteDePagoCallValidateFieldsToConsultFactura() {
        consultFacturaPresenter.consulFacturaPorReferenteDePago(number);
        verify(consultFacturaPresenter).validateFieldsToConsultFactura(number);
    }

    @Test
    public void methodConsulFacturaPorReferenteDePagoCallValidateInternetToConsultFactura() {
        consultFacturaPresenter.consulFacturaPorReferenteDePago(number);
        verify(consultFacturaPresenter).validateFieldsToConsultFactura(number);
        verify(consultFacturaPresenter).validateInternetToConsultFactura();
    }

    @Test
    public void methodConsulFacturaPorReferenteDePagoCallValidateFieldsWithEmptyNumberShouldShowAlertDialog() {
        number = Constants.EMPTY_STRING;;
        consultFacturaPresenter.consulFacturaPorReferenteDePago(number);
        verify(consultFacturaView).showAlertDialogGeneralInformationOnUiThread(app.epm.com.security_presentation.R.string.title_empty_fields, app.epm.com.security_presentation.R.string.text_digitar_numero);
        verify(consultFacturaPresenter, never()).validateInternetToConsultFactura();
        verify(consultFacturaPresenter, never()).createThreadToConsulFacturaPorReferenteDePago(number);
    }

    @Test
    public void methodConsulFacturaPorReferenteDePagoCallValidateInternetWithOutConnectionShoudlShowAlertDialog() {
        //when(validateInternet.isConnected()).thenReturn(false);
        consultFacturaPresenter.consulFacturaPorReferenteDePago(number);
        verify(consultFacturaPresenter).validateFieldsToConsultFactura(number);
        verify(consultFacturaPresenter).validateInternetToConsultFactura();
        verify(consultFacturaView).showAlertDialogGeneralInformationOnUiThread(consultFacturaView.getName(), R.string.text_validate_internet);
        verify(consultFacturaPresenter, never()).createThreadToConsulFacturaPorReferenteDePago(number);
    }

    @Test
    public void methodConsultaFacturaPorReferenteDePagoCallCreateThreadToConsulFacturaPorReferenteDePago(){
        //when(validateInternet.isConnected()).thenReturn(true);
        consultFacturaPresenter.consulFacturaPorReferenteDePago(number);
        verify(consultFacturaPresenter).validateFieldsToConsultFactura(number);
        verify(consultFacturaPresenter).validateInternetToConsultFactura();
        //verify(consultFacturaPresenter).createThreadToConsulFacturaPorReferenteDePago(number);
    }

    @Test
    public void methodCreateThreadToConsulFacturaPorReferenteDePagoShouldShowProgressDialog() {
        consultFacturaPresenter.createThreadToConsulFacturaPorReferenteDePago(number);
        verify(consultFacturaView).showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
    }

    @Test
    public void  methodConsultaFacturaPorReferenteDePagoCallConsulFacturaPorReferenteDePagoBL() throws RepositoryError {
        //when(validateInternet.isConnected()).thenReturn(true);
        List<FacturaResponse> facturaResponse =  new ArrayList<FacturaResponse>();
        when(facturaRepository.consultFacturaPorReferenteDePago(number)).thenReturn(facturaResponse);
        consultFacturaPresenter.consulFacturaPorReferenteDePagoBL(number);
        verify(facturaBL).consulFacturaPorReferenteDePago(number);
        verify(consultFacturaView).dismissProgressDialog();
    }


    @Test
    public void methodConsultaFacturaPorReferenteDePagoShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError("401");
        repositoryError.setIdError(401);
        when(facturaRepository.consultFacturaPorReferenteDePago(number)).thenThrow(repositoryError);
        consultFacturaPresenter.consulFacturaPorReferenteDePagoBL(number);
        verify(consultFacturaView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(consultFacturaView).dismissProgressDialog();
    }

    @Test
    public void methodConsultaFacturaPorReferenteDePagoShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.consultFacturaPorReferenteDePago(number)).thenThrow(repositoryError);
        consultFacturaPresenter.consulFacturaPorReferenteDePagoBL(number);
        verify(consultFacturaView).showAlertDialogGeneralInformationOnUiThread(R.string.title_error,Constants.DEFAUL_ERROR);
        verify(consultFacturaView).dismissProgressDialog();
    }

    @Test
    public void methodConsultaFacturaPorReferenteDePagoShouldShowAnAlertDialogWhenReturn404() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError("404");
        repositoryError.setIdError(Constants.NOT_FOUND_ERROR_CODE);
        when(facturaRepository.consultFacturaPorReferenteDePago(number)).thenThrow(repositoryError);
        consultFacturaPresenter.consulFacturaPorReferenteDePagoBL(number);
        verify(consultFacturaView).cleanFieldsTheConsulFactura();
        verify(consultFacturaView).showAlertDialogGeneralInformationOnUiThread(nullable(String.class), anyInt());
        verify(consultFacturaView).dismissProgressDialog();
    }

    /**
     * End Consultar factura por referente de pago.
     */

    /**
     * Start Consultar factura por contrato.
     */


    @Test
    public void methodConsultFacturaPorContratoCallValidateFieldsToConsultFactura() {
        consultFacturaPresenter.consultFacturaPorContrato(number);
        verify(consultFacturaPresenter).validateFieldsToConsultFactura(number);
    }

    @Test
    public void methodConsultFacturaPorContratoCallValidateInternetToConsultFactura() {
        consultFacturaPresenter.consultFacturaPorContrato(number);
        verify(consultFacturaPresenter).validateFieldsToConsultFactura(number);
        verify(consultFacturaPresenter).validateInternetToConsultFactura();
    }

    @Test
    public void methodConsultFacturaPorContratoCallValidateFieldsWithEmptyNumberShouldShowAlertDialog() {
        number = Constants.EMPTY_STRING;;
        consultFacturaPresenter.consultFacturaPorContrato(number);
        verify(consultFacturaView).showAlertDialogGeneralInformationOnUiThread(app.epm.com.security_presentation.R.string.title_empty_fields, app.epm.com.security_presentation.R.string.text_digitar_numero);
        verify(consultFacturaPresenter, never()).validateInternetToConsultFactura();
        verify(consultFacturaPresenter, never()).createThreadToConsulFacturaPorContrato(number);
    }

    @Test
    public void methodConsultFacturaPorContratoCallValidateInternetWithOutConnectionShoudlShowAlertDialog() {
        //when(validateInternet.isConnected()).thenReturn(false);
        consultFacturaPresenter.consultFacturaPorContrato(number);
        verify(consultFacturaPresenter).validateFieldsToConsultFactura(number);
        verify(consultFacturaPresenter).validateInternetToConsultFactura();
        verify(consultFacturaView).showAlertDialogGeneralInformationOnUiThread(consultFacturaView.getName(), R.string.text_validate_internet);
        verify(consultFacturaPresenter, never()).createThreadToConsulFacturaPorContrato(number);
    }

    @Test
    public void methodConsultFacturaPorContratoCallCreateThreadToConsultFacturaPorContrato(){
        //when(validateInternet.isConnected()).thenReturn(true);
        consultFacturaPresenter.consultFacturaPorContrato(number);
        verify(consultFacturaPresenter).validateFieldsToConsultFactura(number);
        verify(consultFacturaPresenter).validateInternetToConsultFactura();
        //verify(consultFacturaPresenter).createThreadToConsulFacturaPorContrato(number);
    }

    @Test
    public void methodCreateThreadToConsulFacturaPorContratoShouldShowProgressDialog() {
        consultFacturaPresenter.createThreadToConsulFacturaPorContrato(number);
        verify(consultFacturaView).showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
    }

    @Test
    public void  methodConsultFacturaPorContratoCallconsulFacturaPorContratoBL() throws RepositoryError {
        //when(validateInternet.isConnected()).thenReturn(true);
        List<FacturaResponse> facturaResponse =  new ArrayList<FacturaResponse>();
        facturaResponse.add(new FacturaResponse());
        when(facturaRepository.consultFacturaPorContrato(number)).thenReturn(facturaResponse);
        consultFacturaPresenter.consulFacturaPorContratoBL(number);
        verify(facturaBL).consulFacturaPorContrato(number);
        verify(consultFacturaView).dismissProgressDialog();
    }

    @Test
    public void methodConsultFacturaPorContratoShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError("401");
        repositoryError.setIdError(401);
        when(facturaRepository.consultFacturaPorContrato(number)).thenThrow(repositoryError);
        consultFacturaPresenter.consulFacturaPorContratoBL(number);
        verify(consultFacturaView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(consultFacturaView).dismissProgressDialog();
    }

    @Test
    public void methodConsultFacturaPorContratoShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.consultFacturaPorContrato(number)).thenThrow(repositoryError);
        consultFacturaPresenter.consulFacturaPorContratoBL(number);
        verify(consultFacturaView).showAlertDialogGeneralInformationOnUiThread(R.string.title_error,Constants.DEFAUL_ERROR);
        verify(consultFacturaView).dismissProgressDialog();
    }

    @Test
    public void methodConsultFacturaPorContratoShouldShowAnAlertDialogWhenReturn404() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError("404");
        repositoryError.setIdError(Constants.NOT_FOUND_ERROR_CODE);
        when(facturaRepository.consultFacturaPorContrato(number)).thenThrow(repositoryError);
        consultFacturaPresenter.consulFacturaPorContratoBL(number);
        verify(consultFacturaView).cleanFieldsTheConsulFactura();
        verify(consultFacturaView).showAlertDialogGeneralInformationOnUiThread(nullable(String.class), anyInt());
        verify(consultFacturaView).dismissProgressDialog();
    }

    /**
     * End Consultar factura por contrato.
     */
}
