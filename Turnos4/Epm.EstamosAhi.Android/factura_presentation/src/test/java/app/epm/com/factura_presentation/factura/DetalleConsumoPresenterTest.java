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

import app.epm.com.facturadomain.business_models.ServicioFacturaResponse;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.presenters.DetalleConsumoPresenter;
import app.epm.com.factura_presentation.view.views_activities.IDetalleConsumoView;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 14/02/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class DetalleConsumoPresenterTest {

    @Mock
    IFacturaRepository facturaRepository;

    @Mock
    IDetalleConsumoView detalleConsumoView;

    @Mock
    IValidateInternet validateInternet;

    DetalleConsumoPresenter detalleConsumoPresenter;

    FacturaBL facturaBL;

    String numberFactura;

    @Before
    public void setUp() throws Exception {
        facturaBL = Mockito.spy(new FacturaBL(facturaRepository));
        detalleConsumoPresenter = Mockito.spy(new DetalleConsumoPresenter(facturaBL));
        detalleConsumoPresenter.inject(detalleConsumoView, validateInternet);
        numberFactura = "12345";
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallCreateThreadToGetDetail(){
        when(validateInternet.isConnected()).thenReturn(true);
        detalleConsumoPresenter.validateInternetToGetDetailFactura(numberFactura);
        verify(detalleConsumoPresenter).createThreadToGetDetailFactura(numberFactura);
    }

    @Test
    public void methodValidateInternetWithOutConnectionShouldShowAlertDialog(){
        when(validateInternet.isConnected()).thenReturn(false);
        detalleConsumoPresenter.validateInternetToGetDetailFactura(numberFactura);
        verify(detalleConsumoView).showAlertDialogToLoadAgain(detalleConsumoView.getName(),R.string.text_validate_internet);
    }

    @Test
    public void methodCreateThreadToGetDetailFacturaShouldShowProgressDialog(){
        detalleConsumoPresenter.createThreadToGetDetailFactura(numberFactura);
        verify(detalleConsumoView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodDetailFacturaCallDetailFacturaBL() throws RepositoryError {
        List<ServicioFacturaResponse> servicioFacturaResponses = new ArrayList<>();
        servicioFacturaResponses.add(new ServicioFacturaResponse());
        when(facturaRepository.detailFactura(numberFactura)).thenReturn(servicioFacturaResponses);
        detalleConsumoPresenter.getDetailFactura(numberFactura);
        verify(facturaBL).getDetailFactura(numberFactura);
        verify(detalleConsumoView).dismissProgressDialog();
    }

    @Test
    public void methodDetailFacturaShouldShowAlertDialogWhenReturnAnException() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(facturaRepository.detailFactura(numberFactura)).thenThrow(repositoryError);
        detalleConsumoPresenter.getDetailFactura(numberFactura);
        verify(detalleConsumoView).showAlertDialogToLoadAgain(R.string.title_error,repositoryError.getMessage());
        verify(detalleConsumoView).dismissProgressDialog();
    }

    @Test
    public void methodDetailFacturaShouldShowAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(facturaRepository.detailFactura(numberFactura)).thenThrow(repositoryError);
        detalleConsumoPresenter.getDetailFactura(numberFactura);
        verify(detalleConsumoView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(detalleConsumoView).dismissProgressDialog();
    }

    @Test
    public void methodDetailFacturaShouldShowAlertDialogWhenReturn404() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.NOT_FOUND_ERROR_CODE);
        when(facturaRepository.detailFactura(numberFactura)).thenThrow(repositoryError);
        detalleConsumoPresenter.getDetailFactura(numberFactura);
        verify(detalleConsumoView).showAlertDialogToLoadAgain(R.string.title_error, Constants.DETALLE_MESSAGE);
        verify(detalleConsumoView).dismissProgressDialog();
    }


}
