package com.epm.app.mvvm.comunidad.views.activities;

import androidx.lifecycle.ViewModelProviders;
import androidx.databinding.DataBindingUtil;
import android.net.http.SslError;
import android.os.Bundle;
import androidx.appcompat.widget.Toolbar;
import android.view.View;
import android.webkit.SslErrorHandler;
import android.webkit.WebResourceError;
import android.webkit.WebResourceRequest;
import android.webkit.WebResourceResponse;
import android.webkit.WebView;
import android.webkit.WebViewClient;

import com.epm.app.R;
import com.epm.app.databinding.ActivityPrivacyPolicyBinding;
import com.epm.app.mvvm.comunidad.viewModel.PrivacyPolicyViewModel;

import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class PrivacyPolicyActivity extends BaseActivityWithDI {


    private ActivityPrivacyPolicyBinding binding;
    private PrivacyPolicyViewModel privacyPolicyViewModel;
    private Toolbar toolbarApp;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_privacy_policy);
        this.configureDagger();
        privacyPolicyViewModel = ViewModelProviders.of(this,viewModelFactory).get(PrivacyPolicyViewModel.class);
        binding.setPrivacyViewModel((PrivacyPolicyViewModel) privacyPolicyViewModel);
        toolbarApp = (Toolbar) binding.toolbarApp;
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadToolbar();
        loadBinding();
        privacyPolicyViewModel.loadUrl();
    }

    private void loadBinding(){
        privacyPolicyViewModel.showError();
        privacyPolicyViewModel.getError().observe(this, errorMessage -> {
            if(errorMessage != null) {
                showAlertDialogTryAgain(errorMessage.getTitle(),errorMessage.getMessage(), R.string.text_intentar, R.string.text_cancelar);
            }
        });
        privacyPolicyViewModel.getExpiredToken().observe(this, errorMessage -> showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage()) );
        privacyPolicyViewModel.getSuccessful().observe(this, aBoolean -> {
            if(aBoolean != null && aBoolean){
                loadWebView(privacyPolicyViewModel.getUrl());
            }
        });
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }


    private void loadWebView(String url){
        if(!url.isEmpty()){
            binding.privacyPolicy.getSettings().setBuiltInZoomControls(true);
            binding.privacyPolicy.getSettings().setJavaScriptEnabled(true);
            binding.privacyPolicy.getSettings().setLoadWithOverviewMode(true);
            binding.privacyPolicy.getSettings().setUseWideViewPort(true);
            binding.privacyPolicy.loadUrl(url);
            binding.privacyPolicy.setWebViewClient(new WebViewClient() {
                @Override
                public void onPageFinished(WebView view, String url) {
                    super.onPageFinished(view, url);
                    binding.progressTerms.setVisibility(View.GONE);
                }
                @Override
                public void onReceivedError(WebView view, WebResourceRequest request, WebResourceError error){
                   errorWebview();
                }

                @Override
                public void onReceivedHttpError(WebView view, WebResourceRequest request, WebResourceResponse errorResponse) {
                    super.onReceivedHttpError(view, request, errorResponse);
                    errorWebview();
                }

                @Override
                public void onReceivedSslError(WebView view, SslErrorHandler handler, SslError error) {
                    super.onReceivedSslError(view, handler, error);
                    errorWebview();
                }
            });
        }
    }

    private void errorWebview(){
        binding.progressTerms.setVisibility(View.GONE);
        binding.notFound.setVisibility(View.VISIBLE);
        binding.privacyPolicy.loadUrl("about:blank");
    }



    private void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> {
            if (!getUsuario().isInvitado() && title == R.string.title_appreciated_user) {
                getCustomAlertDialog().showAlertDialog(getUsuario().getNombres(), message, false, positive, (dialogInterface, i) -> {
                    privacyPolicyViewModel.loadUrl();
                }, negative, (dialogInterface, i) -> onBackPressed(), null);
            } else {
                getCustomAlertDialog().showAlertDialog(title, message, false, positive, (dialogInterface, i) -> {
                    privacyPolicyViewModel.loadUrl();
                }, negative, (dialogInterface, i) -> onBackPressed(), null);
            }
        });
    }


}
