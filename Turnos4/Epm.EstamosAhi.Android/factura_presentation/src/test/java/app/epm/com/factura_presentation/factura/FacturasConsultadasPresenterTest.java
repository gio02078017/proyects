package app.epm.com.factura_presentation.factura;

import com.epm.app.business_models.business_models.ItemGeneral;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.EstadoFacturaResponse;
import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.presenters.FacturasConsultadasPresenter;
import app.epm.com.factura_presentation.view.views_activities.IFacturasConsultadasView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 21/12/16.
 */

@RunWith(MockitoJUnitRunner.class)
public class FacturasConsultadasPresenterTest {

    FacturasConsultadasPresenter consultadasPresenter;
    FacturaBL facturaBL;
    String name;
    String numberContrato;

    @Mock
    IFacturaRepository facturaRepository;

    @Mock
    IFacturasConsultadasView facturasConsultadasView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    ICustomSharedPreferences customSharedPreferences;

    @Before
    public void setUp() throws Exception {
        facturaBL = Mockito.spy(new FacturaBL(facturaRepository));
        consultadasPresenter = Mockito.spy(new FacturasConsultadasPresenter(facturaBL));
        consultadasPresenter.inject(facturasConsultadasView, validateInternet, customSharedPreferences);
        name = "Juan";
        numberContrato = "123";
    }

    private Usuario getUsuarioResponse() {
        Usuario usuario = new Usuario();
        usuario.setCorreoElectronico("leidyzulu9@gmail.com");
        usuario.setNombres("Leidy");
        return usuario;
    }


    @Test
    public void methodValidateUserWithLoginShouldChallMethodValidateInternet() {
        Usuario usuario = getUsuarioResponse();
        consultadasPresenter.validateInternetToConsultBill(usuario.getCorreoElectronico());
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadToConsultBill() {
        Usuario usuario = getUsuarioResponse();
        when(validateInternet.isConnected()).thenReturn(true);
        consultadasPresenter.validateInternetToConsultBill(usuario.getCorreoElectronico());
        verify(consultadasPresenter).createThreadToConsultBill(usuario.getCorreoElectronico());
        verify(facturasConsultadasView, never()).showAlertDialogTryAgain(facturasConsultadasView.getName(), R.string.text_validate_internet, R.string.text_intentar, R.string.text_cancelar);
    }

    @Test
    public void methodValidateInternetWithOutConnectionShouldShowAlertDialog() {
        Usuario usuario = getUsuarioResponse();
        when(validateInternet.isConnected()).thenReturn(false);
        consultadasPresenter.validateInternetToConsultBill(usuario.getCorreoElectronico());
        verify(facturasConsultadasView).showAlertDialogTryAgain(facturasConsultadasView.getName(), R.string.text_validate_internet, R.string.text_intentar, R.string.text_cancelar);
        verify(consultadasPresenter, never()).createThreadToConsultBill(usuario.getCorreoElectronico());
    }

    @Test
    public void methodCreateThreadToConsultBillShouldShowProgressDialog() {
        Usuario usuario = getUsuarioResponse();
        consultadasPresenter.createThreadToConsultBill(usuario.getCorreoElectronico());
        verify(facturasConsultadasView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodConsultBillShouldCallMethodconsulFacturasPorUsuario() throws RepositoryError {
        Usuario usuario = getUsuarioResponse();
        List<FacturaResponse> facturaResponseList = new ArrayList<>();
        when(facturaRepository.consultFacturasPorUsuario(usuario.getCorreoElectronico())).thenReturn(facturaResponseList);
        consultadasPresenter.consultBill(usuario.getCorreoElectronico());
        facturasConsultadasView.setFacturaResponse(facturaResponseList);
        verify(facturaBL).consulFacturasPorUsuario(usuario.getCorreoElectronico());
        Assert.assertTrue(facturaResponseList.isEmpty());
        verify(facturasConsultadasView).showViewWithOutBill(facturaResponseList);
        verify(facturasConsultadasView).dismissProgressDialog();
    }


    @Test
    public void methodConsultFacturaShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        Usuario usuario = getUsuarioResponse();
        RepositoryError repositoryError = new RepositoryError("401");
        repositoryError.setIdError(0);
        when(facturaRepository.consultFacturasPorUsuario(usuario.getCorreoElectronico())).thenThrow(repositoryError);
        consultadasPresenter.consultBill(usuario.getCorreoElectronico());
        verify(facturasConsultadasView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(facturasConsultadasView).dismissProgressDialog();
    }

    @Test
    public void methodConsultFacturaShouldCallMethodHideProgressDialog() throws RepositoryError {
        Usuario usuario = getUsuarioResponse();
        List<FacturaResponse> facturaResponseList = new ArrayList<>();
        when(facturaRepository.consultFacturasPorUsuario(usuario.getCorreoElectronico())).thenReturn(facturaResponseList);
        consultadasPresenter.consultBill(usuario.getCorreoElectronico());
        verify(facturasConsultadasView).dismissProgressDialog();
    }

    @Test
    public void methodConsultFacturaShouldShowAnAlertDialogWhenReturnAnExceptionTimeOut() throws RepositoryError {
        Usuario usuario = getUsuarioResponse();
        RepositoryError repositoryError = new RepositoryError("timeout");
        repositoryError.setIdError(0);
        when(facturaRepository.consultFacturasPorUsuario(usuario.getCorreoElectronico())).thenThrow(repositoryError);
        consultadasPresenter.consultBill(usuario.getCorreoElectronico());
        Assert.assertTrue(repositoryError.getMessage().contains("timeout"));
        verify(facturasConsultadasView).showAlertDialogTryAgain(R.string.title_error, Constants.REQUEST_TIMEOUT_ERROR_MESSAGE, R.string.text_intentar, R.string.text_cancelar);
        verify(facturasConsultadasView).dismissProgressDialog();
    }


    @Test
    public void methodConsultFacturaShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        Usuario usuario = getUsuarioResponse();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.consultFacturasPorUsuario(usuario.getCorreoElectronico())).thenThrow(repositoryError);
        consultadasPresenter.consultBill(usuario.getCorreoElectronico());
        verify(facturasConsultadasView).showAlertDialogTryAgain(R.string.title_error, Constants.DEFAUL_ERROR, R.string.text_intentar, R.string.text_cancelar);
        verify(facturasConsultadasView).dismissProgressDialog();
    }

    @Test
    public void methodConsultFacturaShouldShowAnAlertDialogWhenReturnAnException404() throws RepositoryError {
        Usuario usuario = getUsuarioResponse();
        List<FacturaResponse> facturaResponseList = new ArrayList<>();
        RepositoryError repositoryError = new RepositoryError("404");
        repositoryError.setIdError(Constants.NOT_FOUND_ERROR_CODE);
        when(facturaRepository.consultFacturasPorUsuario(usuario.getCorreoElectronico())).thenThrow(repositoryError);
        consultadasPresenter.consultBill(usuario.getCorreoElectronico());
        Assert.assertTrue(repositoryError.getMessage().contains("404"));
        verify(facturasConsultadasView).showViewWithOutBill(facturaResponseList);
        verify(facturasConsultadasView).dismissProgressDialog();
    }

    @Test
    public void methodconsultStatusPendingBillhouldCallMethodValidateInternet() {
        String referencia = "12344555";
        int position = 1;
        consultadasPresenter.consultStatusPendingBill(position, referencia);
        verify(consultadasPresenter).validateInternetTOConsultStatusPendingBill(position, referencia);
    }

    @Test
    public void methodValidateInternetWithoutConnectionShouldShowAlertDialog() {
        String referencia = "12344555";
        int position = 1;
        when(validateInternet.isConnected()).thenReturn(false);
        consultadasPresenter.validateInternetTOConsultStatusPendingBill(position, referencia);
        verify(facturasConsultadasView).showAlertDialogGeneralInformationOnUiThread(facturasConsultadasView.getName(), R.string.text_validate_internet);
        verify(consultadasPresenter, never()).createThreadToConsultStatusPendingBill(position, referencia);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldShowAlertDialog() {
        String referencia = "12344555";
        int position = 1;
        when(validateInternet.isConnected()).thenReturn(true);
        consultadasPresenter.validateInternetTOConsultStatusPendingBill(position, referencia);
        verify(consultadasPresenter).createThreadToConsultStatusPendingBill(position, referencia);
        verify(facturasConsultadasView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodcreateThreadToConsultStatusPendingBillShouldShowPRogressDialog() {
        String referencia = "12344555";
        int position = 1;
        consultadasPresenter.createThreadToConsultStatusPendingBill(position, referencia);
        verify(facturasConsultadasView).showProgressDIalog(R.string.text_please_wait);

    }


    @Test
    public void methodConsultStatusPendingShouldCallMethodvalidateFacturasPendientesInFacturaBL() throws RepositoryError {
        String referencia = "12344555";
        int position = 1;
        EstadoFacturaResponse estadoFacturaResponse = new EstadoFacturaResponse();
        estadoFacturaResponse.setEstado(true);
        when(facturaRepository.validateFacturasPendientes(referencia)).thenReturn(estadoFacturaResponse);
        consultadasPresenter.consultStatusPending(position, referencia);
        verify(facturaBL).validateFacturasPendientes(referencia);
        verify(facturasConsultadasView).dismissProgressDialog();
    }

    @Test
    public void methodConsultStatusPendingShouldShowAlertDialogWhenReturnAnException() throws RepositoryError {
        String referencia = "12344555";
        int position = 1;
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.validateFacturasPendientes(referencia)).thenThrow(repositoryError);
        consultadasPresenter.consultStatusPending(position, referencia);
        verify(facturasConsultadasView).showAlertDialogError(position, R.string.title_error, repositoryError.getMessage());
    }

    @Test
    public void methodConsultStatusPendingShouldCallMethodHideProgressDialog() throws RepositoryError {
        String referencia = "12344555";
        int position = 1;
        EstadoFacturaResponse estadoFacturaResponse = new EstadoFacturaResponse();
        estadoFacturaResponse.setEstado(true);
        when(facturaRepository.validateFacturasPendientes(referencia)).thenReturn(estadoFacturaResponse);
        consultadasPresenter.consultStatusPending(position, referencia);
        verify(facturasConsultadasView).dismissProgressDialog();
    }

    @Test
    public void methodValidateInternetListEntityFinancialWithoutConnectionShouldShowAlertDialog() {
        Usuario usuario = getUsuarioResponse();
        when(validateInternet.isConnected()).thenReturn(false);
        consultadasPresenter.validateInternetListEntityFinancial();

        verify(facturasConsultadasView).showAlertDialogGeneralInformationOnUiThread(facturasConsultadasView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetListEntityFinancialWithConnectionShouldCallMethodCreateThreadToGetListEntityFinancial() {
        Usuario usuario = getUsuarioResponse();
        when(validateInternet.isConnected()).thenReturn(true);
        consultadasPresenter.validateInternetListEntityFinancial();

        verify(consultadasPresenter).createThreadTogetListEntityFinancial();
        verify(facturasConsultadasView, never()).showAlertDialogGeneralInformationOnUiThread(usuario.getNombres().toString(), R.string.text_validate_internet);

    }

    @Test
    public void methodCreateThreadToGetListEntityFinancialShouldShowProgressDialog() {
        consultadasPresenter.createThreadTogetListEntityFinancial();

        verify(facturasConsultadasView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodGetListEntityFinancialShouldCallMethodConsulEntidadesFinancierasInFacturaBL() throws RepositoryError {
        List<ItemGeneral> itemGeneralResponseList = new ArrayList<>();
        itemGeneralResponseList.add(new ItemGeneral());
        when(facturaRepository.consultEntidadesFinancieras()).thenReturn(itemGeneralResponseList);
        consultadasPresenter.getListEntityFinancial();
        verify(facturaBL).consulEntidadesFinancieras();
        verify(facturasConsultadasView).dismissProgressDialog();
    }

    @Test
    public void methodGetListEntityFinancialShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.consultEntidadesFinancieras()).thenThrow(repositoryError);
        consultadasPresenter.getListEntityFinancial();
        verify(facturasConsultadasView).showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
        verify(facturasConsultadasView).dismissProgressDialog();
    }

    @Test
    public void methodGetListEntityFinancialShouldCallMethodHidePRogressDialog() throws RepositoryError {
        List<ItemGeneral> itemGeneralResponseList = new ArrayList<>();
        itemGeneralResponseList.add(new ItemGeneral());
        when(facturaRepository.consultEntidadesFinancieras()).thenReturn(itemGeneralResponseList);
        consultadasPresenter.getListEntityFinancial();
        verify(facturasConsultadasView).dismissProgressDialog();
    }

    @Test
    public void methodGetListEntityFinancialShouldSaveEntityFinancialListInSharedPreferences() throws RepositoryError {

        List<ItemGeneral> itemGeneralResponseList = new ArrayList<>();
        itemGeneralResponseList.add(new ItemGeneral());
        when(facturaRepository.consultEntidadesFinancieras()).thenReturn(itemGeneralResponseList);
        consultadasPresenter.getListEntityFinancial();
        verify(customSharedPreferences).addSetArray(Constants.ENTITYFINANCIAL, itemGeneralResponseList);
        verify(facturasConsultadasView).dismissProgressDialog();

    }

    @Test
    public void methodGetListEntityFinancialShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        Usuario usuario = getUsuarioResponse();
        RepositoryError repositoryError = new RepositoryError("401");
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(facturaRepository.consultEntidadesFinancieras()).thenThrow(repositoryError);
        consultadasPresenter.getListEntityFinancial();
        verify(facturasConsultadasView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(facturasConsultadasView).dismissProgressDialog();
    }
}