package app.epm.com.contacto_transparente_presentation.view.activities;


import android.content.Intent;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import java.io.Serializable;
import java.util.List;

import app.epm.com.contacto_transparente_domain.business_models.GrupoInteres;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.contacto_transparente_presentation.dependency_injection.DomainModule;
import app.epm.com.contacto_transparente_presentation.presenters.HomeContactoTransparentePresenter;
import app.epm.com.contacto_transparente_presentation.view.views_activities.IHomeContactoTransparenteView;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

public class HomeContactoTransparenteActivity extends BaseActivity<HomeContactoTransparentePresenter>
        implements IHomeContactoTransparenteView {

    private Toolbar toolbarApp;
    private ImageView home_report_ivInsident;
    private ImageView home_consult_ivInsident;
    private Boolean isAnonymous;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home_contacto_transparente);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        setPresenter(new HomeContactoTransparentePresenter
                (DomainModule.getContactoTransparenteBusinessLogicInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        createProgressDialog();
        loadViews();
        showAlertDialogWithInformation();
        openAlertRateApp();
    }

    private void openAlertRateApp() {
        int resultCode = 0;
        resultCode = getIntent().getIntExtra(Constants.SHOW_ALERT_RATE, resultCode);
        if (resultCode == Constants.RATE_APP) {
            validateShowAlertQualifyApp();
        }
    }

    private void loadViews() {
        toolbarApp = findViewById(R.id.toolbar);
        home_report_ivInsident =  findViewById(R.id.home_report_ivInsident);
        home_consult_ivInsident =  findViewById(R.id.home_consult_ivInsident);
        loadToolbar();
        loadListener();
    }

    private void loadListener() {
        loadListenerToTheControlHomeReportIvInsident();
        loadListenerToTheControlHomeConsultIvInsident();
    }

    private void loadListenerToTheControlHomeConsultIvInsident() {
        home_consult_ivInsident.setOnClickListener(v -> {
            Intent intent = new Intent(HomeContactoTransparenteActivity.this, GetDenunciaActivity.class);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        });
    }

    private void loadListenerToTheControlHomeReportIvInsident() {
        home_report_ivInsident.setOnClickListener(v -> showAlertAnonymous(R.string.title_report_annonymous, R.string.message_report_how_annonymous));
    }

    public void showAlertDialogWithInformation() {
        LayoutInflater inflater = this.getLayoutInflater();
        View view = inflater.inflate(R.layout.alert_dialog_home_contacto_transparente_description, null);
        TextView alert_tvNameUser =  view.findViewById(R.id.alert_tvNameUser);
        alert_tvNameUser.setText(getName());
        getCustomAlertDialog().showAlertDialog(null, null, false, R.string.btn_acept, (dialog, which) -> dialog.dismiss(), view);
    }

    @Override
    public void showAlertDialogLoadAgainOnUiThread(final String name, final int message) {
        showAlertDialogLoadAgainOnUiThread(name, getString(message));
    }

    @Override
    public void showAlertDialogLoadAgainOnUiThread(final int title, final String message) {
        showAlertDialogLoadAgainOnUiThread(getString(title), message);
    }

    private void showAlertDialogLoadAgainOnUiThread(final String title, final String message) {
        runOnUiThread(() -> showAlertDialogLoadAgain(title, message));
    }

    private void showAlertDialogLoadAgain(String name, String message) {
        getCustomAlertDialog().showAlertDialog(name, message, true, R.string.text_aceptar, (dialog, which) -> dialog.dismiss(), null);
    }

    @Override
    public void callActivityDescribesAndSentDataForIntent(List<GrupoInteres> grupoInteresList) {
        Intent intent = new Intent(HomeContactoTransparenteActivity.this, RegisterIncidentActivity.class);
        intent.putExtra(Constants.STATE_ANONYMOUS, isAnonymous);
        intent.putExtra(Constants.LISTA_GRUPO_INTERES, (Serializable) grupoInteresList);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar( toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        this.toolbarApp.setNavigationOnClickListener(view -> onBackPressed());
    }

    public void showAlertAnonymous(int title, int message) {
        getCustomAlertDialog().showAlertDialog(title, message, false, R.string.button_yes, (dialog, which) -> {
            isAnonymous = true;
            getPresenter().validateInternetToGetListInterestGroup();
        }, R.string.button_no, (dialog, which) -> {
            isAnonymous = false;
            getPresenter().validateInternetToGetListInterestGroup();
        }, null);
    }
}