package app.epm.com.contacto_transparente_presentation.view.activities;

import android.content.Intent;
import android.os.Bundle;
import androidx.viewpager.widget.ViewPager;
import androidx.appcompat.widget.Toolbar;
import android.widget.TextView;

import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.contacto_transparente_presentation.view.Adapters.SwipeConsultDescription;
import app.epm.com.utilities.controls.SlidingTabLayout;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by leidycarolinazuluagabastidas on 17/03/17.
 */

public class ConsultRegisterIncidentActivity extends BaseActivity {

    private Toolbar toolbarApp;
    private TextView status_tvCodeIncident;
    private SlidingTabLayout status_stl;
    private ViewPager status_vpFragments;
    private String incidentCode;
    private Incidente incident;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_describe_consult);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadViews();
        getdataIntent();
        setincidentCode();
        showIncident(incident);
        status_stl.setCustomTabView(R.layout.template_text_view_tab, R.id.templateTextView_Tab);
        status_stl.setSelectedIndicatorColors(getResources().getColor(R.color.button_green_factura));
    }

    private void showIncident(final Incidente incident) {
        runOnUiThread(() -> {
            SwipeConsultDescription swipeConsultDescription = new SwipeConsultDescription(getSupportFragmentManager(), incident);
            status_vpFragments.setAdapter(swipeConsultDescription);
            status_stl.setDistributeEvenly(true);
            status_stl.setViewPager(status_vpFragments);
        });
    }

    private void getdataIntent() {
        Intent intent = getIntent();
        incidentCode = intent.getStringExtra(Constants.CODE_INCIDENT);
        incident = (Incidente) intent.getSerializableExtra(Constants.INCIDENT);
    }

    private void setincidentCode() {
        status_tvCodeIncident.setText(incidentCode);
    }

    private void loadViews() {
        toolbarApp = findViewById(R.id.toolbar);
        status_tvCodeIncident =  findViewById(R.id.status_tvCodeIncident);
        status_stl =  findViewById(R.id.status_stl);
        status_vpFragments =  findViewById(R.id.status_vpFragments);
        loadToolbar();
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar( toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(view -> onBackPressed());
    }
}
