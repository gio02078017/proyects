package app.epm.com.factura_presentation.factura;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.DataContratos;
import app.epm.com.facturadomain.business_models.GestionContrato;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.presenters.InscribirContratoFacturaDigitalPresenter;
import app.epm.com.factura_presentation.view.views_activities.IInscribirContratoFacturaDigitalView;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by mateoquicenososa on 2/01/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class InscribirContratoFacturaDigitalPresenterTest {

    @Mock
    IFacturaRepository facturaRepository;

    @Mock
    IInscribirContratoFacturaDigitalView inscribirContratoFacturaDigitalView;

    @Mock
    IValidateInternet validateInternet;

    FacturaBL facturaBL;

    InscribirContratoFacturaDigitalPresenter inscribirContratoFacturaDigitalPresenter;

    @Before
    public void setUp() throws Exception {
        facturaBL = Mockito.spy(new FacturaBL(facturaRepository));
        inscribirContratoFacturaDigitalPresenter = Mockito.spy(new InscribirContratoFacturaDigitalPresenter(facturaBL));
        inscribirContratoFacturaDigitalPresenter.inject(inscribirContratoFacturaDigitalView, validateInternet);
    }


    private GestionContrato getInscribirContrato() {
        GestionContrato inscribirContrato = new GestionContrato();
        inscribirContrato.setCorreoElectronico("test@test.com");
        List<DataContratos> itemsContratos = new ArrayList<>();
        DataContratos dataContratos = new DataContratos();
        dataContratos.setDescripcion("test");
        dataContratos.setNumero("0123");
        dataContratos.setRecibirFacturaDigital(true);
        dataContratos.setOperacion(1);
        itemsContratos.add(dataContratos);
        inscribirContrato.setContratos(itemsContratos);
        return inscribirContrato;
    }

    @Test
    public void methodValidateInternetWithOutConnectionShouldShowAlertDialog() {
        GestionContrato inscribirContrato = getInscribirContrato();
        when(validateInternet.isConnected()).thenReturn(false);
        inscribirContratoFacturaDigitalPresenter.validateInternetInscribirContratoFacturaDigital(inscribirContrato);
        verify(inscribirContratoFacturaDigitalView).showAlertDialogGeneralInformationOnUiThread(inscribirContratoFacturaDigitalView.getName(), R.string.text_validate_internet);
        verify(inscribirContratoFacturaDigitalPresenter, never()).createThreadInscribirContratoFacturaDigital(inscribirContrato);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldShowActivityInscribirContratoFacturaDigital() {
        GestionContrato inscribirContrato = getInscribirContrato();
        when(validateInternet.isConnected()).thenReturn(true);
        inscribirContratoFacturaDigitalPresenter.validateInternetInscribirContratoFacturaDigital(inscribirContrato);
        verify(inscribirContratoFacturaDigitalPresenter).createThreadInscribirContratoFacturaDigital(inscribirContrato);
        verify(inscribirContratoFacturaDigitalView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodCreateThreadInscribirContratoFacturaDigitalShouldShowProgressDialog() {
        GestionContrato inscribirContrato = getInscribirContrato();
        inscribirContratoFacturaDigitalPresenter.createThreadInscribirContratoFacturaDigital(inscribirContrato);
        verify(inscribirContratoFacturaDigitalView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodInscribirContratoShouldCallMethodInscribirContratoInSecurityBL() throws RepositoryError {
        GestionContrato inscribirContrato = getInscribirContrato();
        Mensaje mensaje = new Mensaje();
        when(facturaRepository.inscribirContrato(inscribirContrato)).thenReturn(mensaje);
        inscribirContratoFacturaDigitalPresenter.inscribirContrato(inscribirContrato);
        verify(facturaBL).inscribirContrato(inscribirContrato);
        verify(inscribirContratoFacturaDigitalView).dismissProgressDialog();
    }

    @Test
    public void methodInscribirContratoShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        GestionContrato inscribirContrato = getInscribirContrato();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.inscribirContrato(inscribirContrato)).thenThrow(repositoryError);
        inscribirContratoFacturaDigitalPresenter.inscribirContrato(inscribirContrato);
        verify(inscribirContratoFacturaDigitalView).showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
        verify(inscribirContratoFacturaDigitalView).dismissProgressDialog();
    }

    @Test
    public void methodInscribirContratoShouldShowAnAlertDialogWhenReturnAnUnAuthorized() throws RepositoryError {
        GestionContrato inscribirContrato = getInscribirContrato();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(facturaRepository.inscribirContrato(inscribirContrato)).thenThrow(repositoryError);
        inscribirContratoFacturaDigitalPresenter.inscribirContrato(inscribirContrato);
        verify(inscribirContratoFacturaDigitalView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(inscribirContratoFacturaDigitalView).dismissProgressDialog();
    }

    @Test
    public void methodInscribirContratoShouldCallMethodHideProgressDialog() throws RepositoryError {
        GestionContrato inscribirContrato = getInscribirContrato();
        Mensaje mensaje = new Mensaje();
        when(facturaRepository.inscribirContrato(inscribirContrato)).thenReturn(mensaje);
        inscribirContratoFacturaDigitalPresenter.inscribirContrato(inscribirContrato);
        verify(inscribirContratoFacturaDigitalView).dismissProgressDialog();
    }

    @Test
    public void methodInscribirContratoShouldCallInscribirContratoResult() throws RepositoryError {
        GestionContrato inscribirContrato = getInscribirContrato();
        Mensaje mensaje = new Mensaje();
        mensaje.setCode(74);
        when(facturaRepository.inscribirContrato(inscribirContrato)).thenReturn(mensaje);
        inscribirContratoFacturaDigitalPresenter.inscribirContrato(inscribirContrato);
        verify(inscribirContratoFacturaDigitalView).showAlertDialogToShowMessageFacturaInscritaSuccesOnUiThread(R.string.text_contrato_inscrito);
        verify(inscribirContratoFacturaDigitalView).dismissProgressDialog();
    }

}
