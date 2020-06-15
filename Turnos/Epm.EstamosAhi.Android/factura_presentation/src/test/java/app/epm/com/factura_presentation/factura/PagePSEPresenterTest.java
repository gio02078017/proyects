package app.epm.com.factura_presentation.factura;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.presenters.PagePSEPresenter;
import app.epm.com.factura_presentation.view.views_activities.IPagePSEView;
import app.epm.com.utilities.helpers.IValidateInternet;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by mateoquicenososa on 10/01/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class PagePSEPresenterTest {

    PagePSEPresenter pagePSEPresenter;

    FacturaBL facturaBL;

    @Mock
    IFacturaRepository facturaRepository;

    @Mock
    IPagePSEView pagePSEView;

    @Mock
    IValidateInternet validateInternet;

    @Before
    public void setUp() throws Exception {
        facturaBL = Mockito.spy(new FacturaBL(facturaRepository));
        pagePSEPresenter = Mockito.spy(new PagePSEPresenter(facturaBL));
        pagePSEPresenter.inject(pagePSEView, validateInternet);
    }

    @Test
    public void methodValidateInternetToSendComprobanteWithoutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        pagePSEPresenter.validateInternetToPagePSE();
        verify(pagePSEView).showAlertDialogTryAgainIternet();
    }

    @Test
    public void methodValidateInternetToPagePSEWithoutConnectionShoudlCalllMethodCreateThreadToPagePSE() {
        when(validateInternet.isConnected()).thenReturn(true);
        pagePSEPresenter.validateInternetToPagePSE();
        verify(pagePSEPresenter).createThreadToPagePSE();
    }

    @Test
    public void methodCreateThreadToPagePSEShouldShowProgressDialog() {
        pagePSEPresenter.createThreadToPagePSE();
        verify(pagePSEView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodStartPagePSEShouldCallMethodOpenPagePSE() {
        pagePSEPresenter.startPagePSE();
        verify(pagePSEView).openPagePSE();
    }
}
