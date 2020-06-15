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

import app.epm.com.facturadomain.business_models.HistoricoFacturaResponse;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.presenters.HistoricoPresenter;
import app.epm.com.factura_presentation.view.views_activities.IHistoricoView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by ocadavid on 7/02/2017.
 */

@RunWith(MockitoJUnitRunner.Silent.class)
public class HistoricoPresenterTest {

    private HistoricoPresenter historicoPresenter;
    private FacturaBL facturaBL;
    private String numberContrato;

    @Mock
    IFacturaRepository facturaRepository;

    @Mock
    IHistoricoView historicoView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    ICustomSharedPreferences customSharedPreferences;


    @Before
    public void setUp() {
        facturaBL = Mockito.spy(new FacturaBL(facturaRepository));
        historicoPresenter = Mockito.spy(new HistoricoPresenter(facturaBL));
        historicoPresenter.inject(historicoView, validateInternet);
        numberContrato = "123";
    }


    @Test
    public void methodConsulHistoricoCallValidateInternetWithOutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        historicoPresenter.consultHistorico(numberContrato);
        verify(historicoView).showAlertDialogtryAgain(historicoView.getName(), R.string.text_validate_internet, R.string.text_intentar, R.string.text_cancelar);
    }

    @Test
    public void methodConsultaHistoricoCallCreateThreadToConsultHistorico(){
        when(validateInternet.isConnected()).thenReturn(true);
        historicoPresenter.consultHistorico(numberContrato);
        verify(historicoPresenter).createThreadToConsultHistorico(numberContrato);
    }

    @Test
    public void methodCreateThreadToConsultHistoricoShouldShowProgressDialog() {
        historicoPresenter.createThreadToConsultHistorico(numberContrato);
        verify(historicoView).showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
    }

    @Test
    public void  methodConsultHistoricoCallConsultHistoricoBL() throws RepositoryError {
        when(validateInternet.isConnected()).thenReturn(true);
        List<HistoricoFacturaResponse> historicoFacturaResponses =  new ArrayList<>();
        historicoFacturaResponses.add(new HistoricoFacturaResponse());
        when(facturaBL.consulHistorico(numberContrato)).thenReturn(historicoFacturaResponses);
        historicoPresenter.consultHistorico(numberContrato);
        verify(historicoPresenter).createThreadToConsultHistorico(numberContrato);
    }

    @Test
    public void methodConsultHistoricoShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        Mockito.lenient().when(validateInternet.isConnected()).thenReturn(true);
        RepositoryError repositoryError = new RepositoryError(Constants.HISTORICO_MESSAGE);
        repositoryError.setIdError(404);
        when(facturaRepository.consultHistorico(numberContrato)).thenThrow(repositoryError);
        historicoPresenter.consultHistoricoBL(numberContrato);
        verify(historicoView).showAlertDialogtryAgain(R.string.title_error, repositoryError.getMessage(), R.string.text_intentar, R.string.text_cancelar);
        verify(historicoView).dismissProgressDialog();
    }

    @Test
    public void methodConsultHistoricoShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(facturaRepository.consultHistorico(numberContrato)).thenThrow(repositoryError);
        historicoPresenter.consultHistoricoBL(numberContrato);
        Mockito.lenient().when(validateInternet.isConnected()).thenReturn(true);
        verify(historicoView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(historicoView).dismissProgressDialog();
    }

    @Test
    public void methodConsultHistoricoShouldShowAnAlertDialogWhenReturnAnExceptionGeneral() throws RepositoryError {
        facturaBL = Mockito.spy(new FacturaBL(facturaRepository));
        historicoPresenter = Mockito.spy(new HistoricoPresenter(facturaBL));
        historicoPresenter.inject(historicoView, validateInternet);
        numberContrato = "123";
        Mockito.lenient().when(validateInternet.isConnected()).thenReturn(true);
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.consultHistorico(numberContrato)).thenThrow(repositoryError);
        historicoPresenter.consultHistoricoBL(numberContrato);
        verify(historicoView).showAlertDialogtryAgain(R.string.title_error, repositoryError.getMessage(), R.string.text_intentar, R.string.text_cancelar);
        verify(historicoView).dismissProgressDialog();
    }

}
