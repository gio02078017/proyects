package com.epm.app.view.activities;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;

import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import androidx.appcompat.widget.Toolbar;

import android.widget.Toast;

import com.epm.app.R;
import com.epm.app.adapters.LineasDeAtencionRecyclerViewAdapter;
import com.epm.app.dependency_injection.DomainModule;
import com.epm.app.presenters.LineasDeAtencionPresenter;
import com.epm.app.view.views_activities.ILineasDeAtencionView;

import java.util.List;

import app.epm.com.container_domain.business_models.LineaDeAtencion;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

public class LineasDeAtencionActivity extends BaseActivity<LineasDeAtencionPresenter> implements ILineasDeAtencionView {

    private Toolbar toolbarApp;
    private RecyclerView lineasDeAtencionRecyclerViewList;
    private boolean openCalls;
    private boolean filterCall;
    private Bundle datosBeforeActivity;
    private boolean doubleClick = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_lineas_de_atencion);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        setPresenter(new LineasDeAtencionPresenter(DomainModule.getLineasDeAtencionBusinessLogicInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        createProgressDialog();
        getPresenter().getValidateInternetGetLineasDeAtencion();
        loadViews();
    }

    @Override
    protected void onStart() {
        super.onStart();
        filterCall = false;
        doubleClick = false;
        datosBeforeActivity = this.getIntent().getExtras();
        if (datosBeforeActivity.getBoolean(Constants.INFORMATION_ATTENTION_LINES)) {
            filterCall = datosBeforeActivity.getBoolean(Constants.INFORMATION_ATTENTION_LINES);
        }

    }

    private void loadViews() {
        toolbarApp = findViewById(R.id.toolbar);
        lineasDeAtencionRecyclerViewList = findViewById(R.id.lineasDeAtencionRecyclerViewList);
        loadToolbar();
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar( toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(view -> onBackPressed());
    }

    @Override
    public void showAlertDialogToLoadAgainOnUiThread(final String name, final int message) {
        showAlertDialogLoadAgain(name, getString(message));
    }

    @Override
    public void showAlertDialogToLoadAgainOnUiThread(final int title, final String message) {
        showAlertDialogLoadAgain(getString(title), message);
    }

    private void showAlertDialogLoadAgain(final String name, final String message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(name, message, false, R.string.text_intentar, (dialog, which) -> getPresenter().getValidateInternetGetLineasDeAtencion(), R.string.text_cancelar, (dialog, which) -> finish(), null));

    }

    @Override
    public void consultLineasDeAtencion(final List<LineaDeAtencion> lineaDeAtencion) {
        runOnUiThread(() -> loadLineasDeAtencion(lineaDeAtencion));
    }

    @Override
    public void showAlerDialogLineasDeAtencion(final LineaDeAtencion lineaDeAtencion) {
        if (lineaDeAtencion.getNumber().equalsIgnoreCase(Constants.NUMBER_OUTSIDE_MEDELLIN_CITY) || lineaDeAtencion.getNumber().equalsIgnoreCase(Constants.NUMBER_TRANSPARENT_CONTACT)) {
            doToCall(lineaDeAtencion);
        } else {
            if(!doubleClick) {
                getCustomAlertDialog().showAlertDialog(R.string.text_title_call_with_cost, getString(R.string.text__message_call_aburra_valley)
                        , false, R.string.text_aceptar,
                        (dialog, which) -> {
                            doToCall(lineaDeAtencion);
                            doubleClick = true;
                        }, R.string.text_cancelar, (dialog, which) -> {
                            dialog.dismiss();
                            doubleClick = false;
                        }, null);
            }
        }


    }

    private void doToCall(final LineaDeAtencion lineaDeAtencion) {
        Intent intent = new Intent(Intent.ACTION_DIAL);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        intent.setData(Uri.parse("tel:" + lineaDeAtencion.getNumber().replaceAll(" ", "")));
        try {
            openCalls(true);
            startActivityWithOutDoubleClick(intent);
            openCalls(false);
        } catch (Exception exc) {
            Toast.makeText(getBaseContext(), R.string.lineas_de_atencion_title_alert_dialog_show_error, Toast.LENGTH_SHORT).show();
        }
    }

    private void loadLineasDeAtencion(List<LineaDeAtencion> lineaDeAtencion) {
        getTemplateLineasDeAtencionItems(lineaDeAtencion);
    }

    private void getTemplateLineasDeAtencionItems(List<LineaDeAtencion> lineaDeAtencion) {
        LineasDeAtencionRecyclerViewAdapter lineasDeAtencionRecyclerViewAdapter = new LineasDeAtencionRecyclerViewAdapter(this, this, filterCall ? lineaDeAtencion.subList(0, 2) : lineaDeAtencion);
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this);
        lineasDeAtencionRecyclerViewList.setLayoutManager(linearLayoutManager);
        lineasDeAtencionRecyclerViewList.setAdapter(lineasDeAtencionRecyclerViewAdapter);
    }

    public void openCalls(boolean openCalls) {
        this.openCalls = openCalls;
    }

    public Intent getDefaultIntent(Intent intent) {
        if (openCalls) {
            return intent;
        } else {
            return super.getDefaultIntent(intent);
        }
    }
}
