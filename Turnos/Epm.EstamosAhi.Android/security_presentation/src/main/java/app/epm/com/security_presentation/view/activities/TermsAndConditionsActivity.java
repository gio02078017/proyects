package app.epm.com.security_presentation.view.activities;

import android.os.Bundle;
import androidx.appcompat.widget.Toolbar;
import android.view.Menu;
import android.view.View;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.Button;
import android.widget.ProgressBar;

import app.epm.com.security_presentation.R;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 23/11/16.
 */

public class TermsAndConditionsActivity extends BaseActivity {

    private Toolbar toolbarApp;
    private WebView termsAndConditions_wvText;
    private ProgressBar termsAndConditions_pbLoad;
    private Button termsAndConditions_btnContinue;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_terms_conditions);
        loadViews();
    }

    /**
     * Carga los controles de la vista.
     */
    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        termsAndConditions_wvText = (WebView) findViewById(R.id.termsAndConditions_wvText);
        termsAndConditions_pbLoad = (ProgressBar) findViewById(R.id.termsAndConditions_pbLoad);
        termsAndConditions_btnContinue = (Button) findViewById(R.id.termsAndConditions_btnContinue);
        loadListenerToTheControls();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        return false;
    }

    /**
     * Carga los Listener de los controles.
     */
    private void loadListenerToTheControls() {
        loadWebViewTermsAndConditions();
        loadOnClickListenerToTheControlBtnContinue();
    }

    /**
     * Carga la WebView de Terminos y condiciones.
     */
    private void loadWebViewTermsAndConditions() {
        loadToolbar();
        termsAndConditions_wvText.getSettings().setBuiltInZoomControls(true);
        termsAndConditions_wvText.getSettings().setJavaScriptEnabled(true);
        termsAndConditions_wvText.getSettings().setLoadWithOverviewMode(true);
        termsAndConditions_wvText.getSettings().setUseWideViewPort(true);
        termsAndConditions_wvText.loadUrl(Constants.URL_TERMS_AND_CONDITIONS);
        termsAndConditions_wvText.setWebViewClient(new WebViewClient() {
            @Override
            public void onPageFinished(WebView view, String url) {
                super.onPageFinished(view, url);
                termsAndConditions_pbLoad.setVisibility(View.GONE);
            }
        });
    }

    /**
     * Carga el toolbar.
     */
    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onBackPressed();
            }
        });
    }

    /**
     * Carga OnClick al control termsAndConditions_btnContinue.
     */
    private void loadOnClickListenerToTheControlBtnContinue(){
        termsAndConditions_btnContinue.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onBackPressed();
            }
        });
    }
}
