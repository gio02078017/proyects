package app.epm.com.factura_presentation.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.Menu;
import android.view.View;
import android.webkit.ConsoleMessage;
import android.webkit.WebChromeClient;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.ProgressBar;

import java.io.Serializable;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.business_models.InformacionPSE;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.dependency_injection.DomainModule;
import app.epm.com.factura_presentation.presenters.PagePSEPresenter;
import app.epm.com.factura_presentation.view.views_activities.IPagePSEView;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 6/01/17.
 */

public class PagePSEActivity extends BaseActivity<PagePSEPresenter> implements IPagePSEView {

    private Toolbar toolbarApp;
    private WebView pagePSE_wvText;
    private ProgressBar pagePSE_pbLoad;

    private ICustomSharedPreferences customSharedPreferences;
    private List<FacturaResponse> facturaResponse;
    private List<FacturaResponse> myFacturaResponse;
    private String urlPSE;
    private String nombreEntidadFinanciera;
    private InformacionPSE informacionPSE;
    private int entidadFinanciera;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_page_pse);
        loadSwipeDrawerLayout();
        createProgressDialog();
        this.customSharedPreferences = new CustomSharedPreferences(this);
        setPresenter(new PagePSEPresenter(DomainModule.getFacturaBLInstance(this.customSharedPreferences)));
        getPresenter().inject(this, getValidateInternet());
        getDataIntent();
        loadViews();
    }

    private void getDataIntent() {
        this.informacionPSE = (InformacionPSE) getIntent().getSerializableExtra(Constants.INFORMACION_PSE);
        facturaResponse = (List<FacturaResponse>) getIntent().getSerializableExtra(Constants.LISTA_MIS_FACTURAS_POR_PAGO);
        myFacturaResponse = (List<FacturaResponse>) getIntent().getSerializableExtra(Constants.LISTA_MIS_FACTURAS);
        entidadFinanciera = getIntent().getIntExtra(Constants.ID_ENTIDAD_FINANCIERA, entidadFinanciera);
        nombreEntidadFinanciera = getIntent().getStringExtra(Constants.NOMBRE_ENTIDAD_FINANCIERA);
    }

    @Override
    public void manageActionFactura() {
        //The method is not used.
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        return false;
    }

    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        pagePSE_wvText = (WebView) findViewById(R.id.pagePSE_wvText);
        pagePSE_pbLoad = (ProgressBar) findViewById(R.id.pagePSE_pbLoad);
        loadToolbar();
        getPresenter().validateInternetToPagePSE();
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar(toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showAlertDialogClosePagePSEonUiThread();
            }
        });
    }

    private void showAlertDialogClosePagePSEonUiThread() {
        runOnUiThread(() -> showAlertDialogClosePagePSE());
    }

    private void showAlertDialogClosePagePSE() {
        getCustomAlertDialog().showAlertDialog(R.string.title_advertencia, R.string.text_alert_salir_pse,
                false, R.string.text_abandonar, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        startBackToActivityInPagePSE();
                    }
                }, R.string.text_cancelar, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        dialogInterface.dismiss();
                    }
                }, null);
    }

    /**
     * Cambia el estado de la factura cuando se da Back.
     *
     * @return facturaResponseList.
     */
    private List<FacturaResponse> changeStateFactura() {
        List<FacturaResponse> facturaResponseList = new ArrayList<>();
        for (int i = 0; i < facturaResponse.size(); i++) {
            if (facturaResponse.get(i).isEstaSeleccionadaParaPago()) {
                facturaResponse.get(i).setEstaSeleccionadaParaPago(false);
                facturaResponseList.add(facturaResponse.get(i));
            }
        }
        return facturaResponseList;
    }

    private void startBackToActivityInPagePSE() {
        changeStateFactura();
        Intent intent = new Intent(this, FacturasConsultadasActivity.class);
        intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) facturaResponse);
        intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) myFacturaResponse);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        finish();
    }

    @Override
    public void openPagePSE() {
        runOnUiThread(() -> {
            urlPSE = informacionPSE.getUrlEntidadFinanciera();
            creacionWebView(pagePSE_wvText, urlPSE);
        });
    }

    @Override
    public void showAlertDialogTryAgainIternet() {
        validateIternetAgain();
    }

    /**
     * Crea el webView de PSE.
     *
     * @param webView webView.
     * @param urlPSE  urlPSE.
     */
    private void creacionWebView(WebView webView, String urlPSE) {
        final WebView pagePSE_wvText = webView;
        pagePSE_wvText.loadUrl(urlPSE);
        pagePSE_wvText.getSettings().setBuiltInZoomControls(true);
        pagePSE_wvText.getSettings().setDisplayZoomControls(false);
        pagePSE_wvText.getSettings().setSupportZoom(true);
        pagePSE_wvText.getSettings().setJavaScriptEnabled(true);
        pagePSE_wvText.setInitialScale(50);
        pagePSE_wvText.getSettings().setLoadWithOverviewMode(true);
        pagePSE_wvText.getSettings().setJavaScriptCanOpenWindowsAutomatically(true);
        pagePSE_wvText.getSettings().setUseWideViewPort(true);
        pagePSE_wvText.getSettings().setUserAgentString("Android");
        pagePSE_wvText.setWebChromeClient(new WebChromeClient() {
            @Override
            public boolean onConsoleMessage(ConsoleMessage cm) {
                Log.v("ChromeClient", cm.message() + " -- From line "
                        + cm.lineNumber() + " of "
                        + cm.sourceId());
                return true;
            }

        });
        pagePSE_wvText.setWebViewClient(new WebViewClient() {


            @Override
            public void onPageFinished(WebView view, String url) {

                pagePSE_pbLoad.setVisibility(View.GONE);
                dismissProgressDialog();
                try {
                    URI urlRetorno = new URI(url);
                    String Scheme = urlRetorno.getScheme();
                    if (Scheme.equals(Constants.EMP_FACTURA)) {
                        pagePSE_wvText.setWebViewClient(null);
                        startComprobantePago();
                    }
                } catch (URISyntaxException e) {
                    Log.e("Exception", e.toString());
                }
                super.onPageFinished(view, url);
            }

        });
        validateIternetAgain();
    }

    private void validateIternetAgain() {
        runOnUiThread(() -> {
            if (!getValidateInternet().isConnected()) {
                showAlertDialogValidateInternetAgain();
            }
        });
    }

    private void showAlertDialogValidateInternetAgain() {
        getCustomAlertDialog().showAlertDialog(getName(), R.string.text_validate_internet, false, R.string.text_intentar,
                new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        dialog.dismiss();
                        getPresenter().createThreadToPagePSE();
                    }
                }, R.string.text_exit_transaction, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        startBackToActivityInPagePSE();
                    }
                }, null);
    }

    private void startComprobantePago() {
        Intent intent = new Intent(this, ComprobantePagoActivity.class);
        intent.putExtra(Constants.INFORMACION_PSE, this.informacionPSE);
        intent.putExtra(Constants.LISTA_MIS_FACTURAS_POR_PAGO, (Serializable) facturaResponse);
        intent.putExtra(Constants.ID_ENTIDAD_FINANCIERA, entidadFinanciera);
        intent.putExtra(Constants.NOMBRE_ENTIDAD_FINANCIERA, nombreEntidadFinanciera);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    @Override
    public void onBackPressed() {
        showAlertDialogClosePagePSEonUiThread();
    }
}