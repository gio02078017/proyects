package app.epm.com.reporte_fraudes_presentation.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.ImageView;

import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.Mapa;
import com.epm.app.business_models.business_models.ServiciosMapa;

import app.epm.com.reporte_fraudes_presentation.R;
import app.epm.com.reporte_fraudes_presentation.dependency_injection.DomainModule;
import app.epm.com.reporte_fraudes_presentation.presenters.ServicesDeFraudesPresenter;
import app.epm.com.reporte_fraudes_presentation.view.views_activities.IServicesDeFraudesView;
import app.epm.com.utilities.controls.CustomTextViewNormal;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

public class ServicesDeFraudesActivity extends BaseActivity<ServicesDeFraudesPresenter> implements IServicesDeFraudesView {
    private Toolbar services_ToolbarApp;
    private ImageView services_ImageViewEnergia, services_ImageViewAgua, services_ImageViewGas, services_ImageViewSelected;
    private ServiciosMapa servicioMap;
    private int icon;
    private Boolean isAnonymous;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_services_fraude);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        setPresenter(new ServicesDeFraudesPresenter(DomainModule.getReporteDeFraudesBusinessLogicInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        createProgressDialog();
        loadView();
        getPresenter().validateInternetToGetServicesKML();
        servicioMap = new ServiciosMapa();
        openAlertRateApp();
    }

    private void openAlertRateApp() {
        int resultCode = 0;
        resultCode = getIntent().getIntExtra(Constants.SHOW_ALERT_RATE, resultCode);
        if (resultCode == Constants.RATE_APP) {
            validateShowAlertQualifyApp();
        }
    }

    private void showAlertAnonymous(final int service, final ETipoServicio eTipoServicio) {
        getCustomAlertDialog().showAlertDialog(R.string.title_report_annonymous, R.string.servicios_fraudes_message_report_how_annonymous
                , false, R.string.button_yes, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        isAnonymous = true;
                        intentToAttachEvidence(service, eTipoServicio);
                    }
                }, R.string.button_no, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        isAnonymous = false;
                        intentToAttachEvidence(service, eTipoServicio);
                    }
                }, null);
    }

    private void loadView() {
        services_ToolbarApp = (Toolbar) findViewById(R.id.toolbar);
        services_ImageViewEnergia = (ImageView) findViewById(R.id.reporte_fraudes_ivEnergia);
        services_ImageViewEnergia.setEnabled(false);
        services_ImageViewAgua = (ImageView) findViewById(R.id.reporte_fraudes_ivAgua);
        services_ImageViewAgua.setEnabled(false);
        services_ImageViewGas = (ImageView) findViewById(R.id.reporte_fraudes_ivGas);
        services_ImageViewGas.setEnabled(false);
        loadToolbar();
        loadListenerToTheControls();
    }

    private void loadToolbar() {
        services_ToolbarApp = (Toolbar) findViewById(R.id.toolbar);
        services_ToolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) services_ToolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        services_ToolbarApp.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onBackPressed();
            }
        });
    }

    private void loadListenerToTheControls() {

        services_ImageViewEnergia.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showAlertAnonymous(1, ETipoServicio.Energia);

            }
        });

        services_ImageViewAgua.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showAlertAnonymous(0, ETipoServicio.Agua);
            }
        });

        services_ImageViewGas.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showAlertAnonymous(2, ETipoServicio.Gas);
            }
        });

    }

    private void intentToAttachEvidence(int position, ETipoServicio services) {
        Intent intent = new Intent(this, AttachEvidenceDeFraudesActivity.class);
        intent.putExtra(Constants.SERVICES, this.servicioMap.getMapa().get(position));
        intent.putExtra(Constants.STATE_ANONYMOUS, isAnonymous);
        intent.putExtra(Constants.TIPO_SERVICIO, services);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    @Override
    public void enableServices(ServiciosMapa servicesMap) {
        this.servicioMap = servicesMap;
        int numServices = servicesMap.getMapa().size();
        for (int i = 0; i <= numServices - 1; i++) {
            setEnableServices(servicesMap.getMapa().get(i));
        }
    }

    @Override
    public void showAlertDialogTryAgain(String title, int message) {
        showAlertDialog(title, getString(message));
    }

    @Override
    public void showAlertDialogTryAgain(int title, String message) {
        showAlertDialog(getString(title), message);
    }

    private void showAlertDialog(final String title, final String message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false, R.string.text_intentar,
                new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        getPresenter().validateInternetToGetServicesKML();
                    }
                }, R.string.text_cancelar, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        finish();
                    }
                }, null));

    }

    private void setEnableServices(final Mapa map) {
        runOnUiThread(() -> {
            switch (map.getNombreServcio()) {
                case Constants.AGUA:
                    services_ImageViewSelected = services_ImageViewAgua;
                    icon = R.mipmap.icon_water_green;
                    break;
                case Constants.ENERGIA:
                    services_ImageViewSelected = services_ImageViewEnergia;
                    icon = R.mipmap.icon_energy_green;
                    break;
                case Constants.GAS:
                    services_ImageViewSelected = services_ImageViewGas;
                    icon = R.mipmap.icon_gas_green;
                    break;
                default:
                    break;
            }

            setImageResource(services_ImageViewSelected, icon);
        });
    }

    private void setImageResource(ImageView imageView, int icon) {
        imageView.setEnabled(true);
        imageView.setImageResource(icon);
    }
}

