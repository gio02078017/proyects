package com.epm.app.mvvm.comunidad.views.activities;

import android.arch.lifecycle.ViewModelProviders;
import android.databinding.DataBindingUtil;
import android.net.http.SslError;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.webkit.SslErrorHandler;
import android.webkit.WebResourceError;
import android.webkit.WebResourceRequest;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.ProgressBar;

import com.epm.app.R;
import com.epm.app.databinding.ActivityInformationOfInterestBinding;
import com.epm.app.mvvm.comunidad.viewModel.InformationViewModel;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.IInformationViewModel;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class InformationOfInterestActivity extends BaseActivityWithDI {


    private Toolbar toolbarApp;
    private ProgressBar termsAndConditions_pbLoad;
    private WebView termsAndConditions_wvText;
    private InformationViewModel informationViewModel;
    private ActivityInformationOfInterestBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this,R.layout.activity_information_of_interest);
        this.configureDagger();
        informationViewModel = ViewModelProviders.of(this,viewModelFactory).get(InformationViewModel.class);
        binding.setInformationViewModel((InformationViewModel) informationViewModel);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadViews();
        loadToolbar();
        loadWebViewInformationInterest();
        loadBinding();
    }


    private void loadBinding(){
        informationViewModel.showError();
        informationViewModel.getExpiredToken().observe(this, aBoolean -> {
            if(aBoolean!= null && aBoolean){
                showAlertDialogUnauthorized(informationViewModel.getIntTitleError(),informationViewModel.getIntError());
            }
        });
        informationViewModel.getInternet().observe(this, internet -> {
            if(internet != null && internet){
                showAlertDialogTryAgain(informationViewModel.getIntTitleError(),informationViewModel.getIntError(),R.string.text_intentar, R.string.text_cancelar);
            }
        });
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }

    private void loadViews() {
        toolbarApp = (Toolbar) binding.toolbarInformationInterest;
        termsAndConditions_wvText = binding.informationInterest;
        termsAndConditions_pbLoad = binding.informationInterestLoad;
    }

    private void loadWebViewInformationInterest(){
        informationViewModel.getUrlInformation();
        informationViewModel.getUrl().observe(this, s -> {
            if(s != null){
                loadPage(s);
            }
        });
    }

    private void loadPage(String url){
        termsAndConditions_wvText.getSettings().setBuiltInZoomControls(true);
        termsAndConditions_wvText.getSettings().setJavaScriptEnabled(true);
        termsAndConditions_wvText.getSettings().setLoadWithOverviewMode(true);
        termsAndConditions_wvText.getSettings().setUseWideViewPort(true);
        termsAndConditions_wvText.loadUrl(url);
        termsAndConditions_wvText.setWebViewClient(new WebViewClient() {
            @Override
            public void onPageFinished(WebView view, String url) {
                super.onPageFinished(view, url);
                termsAndConditions_pbLoad.setVisibility(View.GONE);
            }
            @Override
            public void onReceivedError(WebView view, WebResourceRequest request, WebResourceError error){
                errorWebview();
            }
            @Override
            public void onReceivedSslError(WebView view, SslErrorHandler handler, SslError error) {
                super.onReceivedSslError(view, handler, error);
                errorWebview();
            }
        });
    }

    private void errorWebview(){
        binding.informationInterestLoad.setVisibility(View.GONE);
        binding.notFound.setVisibility(View.VISIBLE);
        binding.informationInterest.loadUrl("about:blank");
    }

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }



    private void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> {
            if (!getUsuario().isInvitado() && title == R.string.title_appreciated_user) {
                getCustomAlertDialog().showAlertDialog(getUsuario().getNombres(), message, false, positive, (dialogInterface, i) -> {
                    informationViewModel.getUrlInformation();
                }, negative, (dialogInterface, i) -> onBackPressed(), null);
            } else {
                getCustomAlertDialog().showAlertDialog(title, message, false, positive, (dialogInterface, i) -> {
                    informationViewModel.getUrlInformation();
                }, negative, (dialogInterface, i) -> onBackPressed(), null);
            }
        });
    }


}
