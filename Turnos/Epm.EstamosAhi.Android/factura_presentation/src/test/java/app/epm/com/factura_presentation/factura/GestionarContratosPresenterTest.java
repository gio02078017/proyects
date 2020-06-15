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

import app.epm.com.facturadomain.business_models.DataContratos;
import app.epm.com.facturadomain.business_models.GestionContrato;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.presenters.GestionarContratosPresenter;
import app.epm.com.factura_presentation.view.views_activities.IGestionarContratosView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by ocadavid on 25/01/2017.
 */

@RunWith(MockitoJUnitRunner.Silent.class)
public class GestionarContratosPresenterTest {
    GestionarContratosPresenter gestionarContratosPresenter;
    FacturaBL facturaBL;
    String email = "juanito@gmail.com";

    @Mock
    IFacturaRepository facturaRepository;

    @Mock
    IGestionarContratosView gestionarContratosView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    ICustomSharedPreferences customSharedPreferences;


    @Before
    public void setUp() {
        facturaBL = Mockito.spy(new FacturaBL(facturaRepository));
        gestionarContratosPresenter = Mockito.spy(new GestionarContratosPresenter(facturaBL));
        gestionarContratosPresenter.inject(gestionarContratosView, validateInternet);
    }

    private GestionContrato getGestionContratoInstance() {
        GestionContrato gestionContrato = new GestionContrato();
        gestionContrato.setCorreoElectronico("lcz97@live.com");
        List<DataContratos> dataContratos =  new ArrayList<DataContratos>();
        DataContratos data = new DataContratos();
        data.setNumero("123");
        data.setDescripcion("Descripción de contrato");
        data.setOperacion(1);
        data.setRecibirFacturaDigital(true);
        dataContratos.add(data);
        gestionContrato.setContratos(dataContratos);
        return gestionContrato;
    }

    /**
     * Start UpdateContratos.
     */

    @Test
    public void methodValidateInternetToUpdateContratosShouldShowAlertDialog() {
        GestionContrato gestionContrato = getGestionContratoInstance();
        gestionarContratosPresenter.getValidateInternet();
        gestionarContratosPresenter.createThreadToUpdateContratos(gestionContrato);
        verify(gestionarContratosView, never()).showAlertDialogTryAgain(gestionarContratosView.getName(),R.string.text_validate_internet,R.string.text_intentar,R.string.text_cancelar);
    }


    @Test
    public void methodValidateInternetWithOutConnectionShoudlShowAlertDialog() {
        GestionContrato gestionContrato = new GestionContrato();
        when(validateInternet.isConnected()).thenReturn(false);
        gestionarContratosPresenter.updateContratos(gestionContrato);
        verify(gestionarContratosView).showAlertDialogTryAgain(gestionarContratosView.getName(),R.string.text_validate_internet,R.string.text_intentar,R.string.text_cancelar);
    }

    @Test
    public void methodUpdateContratosCallCreateThreadToUpdateContratos(){
        GestionContrato infoContrato = getGestionContratoInstance();
        when(validateInternet.isConnected()).thenReturn(true);
        gestionarContratosPresenter.updateContratos(infoContrato);
        verify(gestionarContratosPresenter).createThreadToUpdateContratos(infoContrato);
    }

    @Test
    public void methodCreateThreadToUpdateContratosShouldShowProgressDialog() {
        gestionarContratosPresenter.createThreadToUpdateContratos(getGestionContratoInstance());
        verify(gestionarContratosView).showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
    }

    @Test
    public void  methodUpdateContratosCallUpdateContratosBL() throws RepositoryError {
        GestionContrato infoContrato = getGestionContratoInstance();
        //when(validateInternet.isConnected()).thenReturn(true);
        Mensaje mensaje = new Mensaje();
        mensaje.setCode(1);
        when(facturaRepository.updateContratos(infoContrato)).thenReturn(mensaje);
        gestionarContratosPresenter.updateContratosBL(infoContrato);
        verify(facturaBL).updateContratos(infoContrato);
    }

    @Test
    public void methodVerifyUpdateShoulShowAnAlertDialogWhenIsCorrect() throws RepositoryError {
        GestionContrato infoContrato = getGestionContratoInstance();
        Mensaje mensaje = new Mensaje();
        mensaje.setCode(1);
        mensaje.setText("Correcto!");
        when(facturaRepository.updateContratos(infoContrato)).thenReturn(mensaje);
        gestionarContratosPresenter.updateContratosBL(infoContrato);
        verify(gestionarContratosView).showAlertDialogGeneralInformationOnUiThread(gestionarContratosView.getName(), mensaje.getText());
    }

    @Test
    public void methodVerifyUpdateShoulShowAnAlertDialogWhenIsCorrectWithCode74() throws RepositoryError {
        GestionContrato infoContrato = getGestionContratoInstance();
        Mensaje mensaje = new Mensaje();
        mensaje.setCode(74);
        when(facturaRepository.updateContratos(infoContrato)).thenReturn(mensaje);
        gestionarContratosPresenter.updateContratosBL(infoContrato);
        Assert.assertTrue(mensaje.getCode() == 74);
        verify(gestionarContratosView).showAlertDialogSaveInformation(gestionarContratosView.getName(), R.string.factura_gestionar_contratos_mensaje_actualizacion);
    }

    @Test
    public void methodUpdateContratosShouldShowAnAlertDialogWhenReturnAnExceptionGeneral() throws RepositoryError {
        //when(validateInternet.isConnected()).thenReturn(true);
        GestionContrato infoContrato = getGestionContratoInstance();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.updateContratos(infoContrato)).thenThrow(repositoryError);
        gestionarContratosPresenter.updateContratosBL(infoContrato);
        verify(gestionarContratosView).showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
    }

    @Test
    public void methodUpdateContratosShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        //when(validateInternet.isConnected()).thenReturn(true);
        GestionContrato infoContrato = getGestionContratoInstance();
        RepositoryError repositoryError = new RepositoryError("");
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(facturaRepository.updateContratos(infoContrato)).thenThrow(repositoryError);
        gestionarContratosPresenter.updateContratosBL(infoContrato);
        verify(gestionarContratosView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
    }

    /**
     * End UpdateContratos
     */

    /**
     * Start ConsultContratosInscritos.
     */

    @Test
    public void methodConsultContratosInscritosValidateInternetWithOutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        gestionarContratosPresenter.consultContratosInscritos(email);
        verify(gestionarContratosView).showAlertDialogTryAgain(gestionarContratosView.getName(), R.string.text_validate_internet, R.string.text_intentar, R.string.text_cancelar); ;
    }

    @Test
    public void methodConsultContratosInscritosCallCreateThreadToConsultContratosInscritos(){
        when(validateInternet.isConnected()).thenReturn(true);
        gestionarContratosPresenter.consultContratosInscritos(email);
        verify(gestionarContratosPresenter).createThreadToConsultContratosInscritos(email);
        verify(gestionarContratosView, never()).showAlertDialogTryAgain(gestionarContratosView.getName(), R.string.text_validate_internet, R.string.text_intentar, R.string.text_cancelar); ;

    }

    @Test
    public void methodCreateThreadToConsultContratosInscritosShouldShowProgressDialog() {
        gestionarContratosPresenter.createThreadToConsultContratosInscritos(email);
        verify(gestionarContratosView).showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
    }

    @Test
    public void  methodConsultContratosInscritosCallConsultContratosInscritosBL() throws RepositoryError {
        //when(validateInternet.isConnected()).thenReturn(true);
        List<DataContratos> dataContratos = new ArrayList<>();
        DataContratos data = new DataContratos();
        data.setNumero("123");
        data.setRecibirFacturaDigital(true);
        data.setOperacion(1);
        data.setDescripcion("descripción");
        dataContratos.add(data);
        when(facturaRepository.consultContratosInscritos(email)).thenReturn(dataContratos);
        gestionarContratosPresenter.consultContratosInscritosBL(email);
        verify(facturaBL).consulContratosInscritos(email);
        verify(gestionarContratosView).dismissProgressDialog();
    }

    @Test
    public void methodConsultContratosShouldShowAnAlertDialogWhenReturnAnExceptionGeneral() throws RepositoryError {
        //when(validateInternet.isConnected()).thenReturn(true);
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.consultContratosInscritos(email)).thenThrow(repositoryError);
        gestionarContratosPresenter.consultContratosInscritosBL(email);
        verify(gestionarContratosView).showAlertDialogTryAgain(R.string.title_error, repositoryError.getMessage(), R.string.text_intentar, R.string.text_cancelar);
        verify(gestionarContratosView).dismissProgressDialog();
    }


    @Test
    public void methodConsultContratosShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        //when(validateInternet.isConnected()).thenReturn(true);
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(facturaRepository.consultContratosInscritos(email)).thenThrow(repositoryError);
        gestionarContratosPresenter.consultContratosInscritosBL(email);
        verify(gestionarContratosView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(gestionarContratosView).dismissProgressDialog();
    }

    /**
     * End ConsultContratosInscritos
     */
}