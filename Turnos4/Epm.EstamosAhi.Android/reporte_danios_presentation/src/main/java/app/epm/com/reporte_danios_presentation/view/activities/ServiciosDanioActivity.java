package app.epm.com.reporte_danios_presentation.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.ImageView;

import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.Mapa;
import com.epm.app.business_models.business_models.ServiciosMapa;

import java.util.List;

import app.epm.com.reporte_danios_presentation.R;
import app.epm.com.reporte_danios_presentation.dependency_injection.DomainModule;
import app.epm.com.reporte_danios_presentation.presenters.ServiciosDaniosPresenter;
import app.epm.com.reporte_danios_presentation.view.views_activities.IServiciosDanioView;
import app.epm.com.utilities.controls.CustomTextViewNormal;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

public class ServiciosDanioActivity extends BaseActivity<ServiciosDaniosPresenter> implements IServiciosDanioView {
    private ImageView reporte_danios_ivEnergia;
    private ImageView reporte_danios_ivAgua;
    private ImageView reporte_danios_ivGas;
    private Toolbar toolbarApp;
    private ServiciosMapa serviciosMapa;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_servicios_danio);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        setPresenter(new ServiciosDaniosPresenter(DomainModule.getDaniosBLInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        createProgressDialog();
        loadView();
        getPresenter().validateInternetToGetServicesKML();
        serviciosMapa = new ServiciosMapa();
        openAlertRateApp();
    }

    private void openAlertRateApp() {
        int resultCode = 0;
        resultCode = getIntent().getIntExtra(Constants.SHOW_ALERT_RATE, resultCode);
        if (resultCode == Constants.RATE_APP) {
            validateShowAlertQualifyApp();
        }
    }

    private void loadView() {

        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        reporte_danios_ivEnergia = (ImageView) findViewById(R.id.reporte_danios_ivEnergia);
        reporte_danios_ivEnergia.setEnabled(false);
        reporte_danios_ivAgua = (ImageView) findViewById(R.id.reporte_danios_ivAgua);
        reporte_danios_ivAgua.setEnabled(false);
        reporte_danios_ivGas = (ImageView) findViewById(R.id.reporte_danios_ivGas);
        reporte_danios_ivGas.setEnabled(false);
        loadToolbar();
        loadListenerToTheControls();
    }

    private void loadListenerToTheControls() {

        reporte_danios_ivEnergia.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                intentToUbicacionDanio(1, ETipoServicio.Energia);
            }
        });

        reporte_danios_ivAgua.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                intentToUbicacionDanio(0, ETipoServicio.Agua);
            }
        });

        reporte_danios_ivGas.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                intentToUbicacionDanio(2, ETipoServicio.Gas);
            }
        });

    }

    private void intentToUbicacionDanio(int position, ETipoServicio servicio) {
        Intent intent = new Intent(this, UbicacionDanioActivity.class);
        Mapa map = serviciosMapa.getMapa().get(position);
        intent.putExtra(Constants.SERVICES, serviciosMapa.getMapa().get(position));
        intent.putExtra(Constants.SERVICES, map);
        intent.putExtra(Constants.TIPO_SERVICIO, servicio);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

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

    @Override
    public void enableServices(ServiciosMapa serviciosMapa) {
        this.serviciosMapa = serviciosMapa;
        int numServices = serviciosMapa.getMapa().size();
        for (int i = 0; i <= numServices - 1; i++) {
            setEnableServices(serviciosMapa.getMapa().get(i));
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
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false, R.string.text_intentar, new DialogInterface.OnClickListener() {
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
            ImageView imageView = reporte_danios_ivAgua;
            int icono = 0;
            switch (map.getNombreServcio()) {
                case Constants.AGUA:
                    imageView = reporte_danios_ivAgua;
                    icono = R.mipmap.icono_aguas_verde;
                    break;
                case Constants.ENERGIA:
                    imageView = reporte_danios_ivEnergia;
                    icono = R.mipmap.icono_energia_verde;
                    break;
                case Constants.GAS:
                    imageView = reporte_danios_ivGas;
                    icono = R.mipmap.icono_gas_verde;
                    break;
                default:
                    break;
            }
            setImageResource(imageView, icono);
        });

    }

    private void setImageResource(ImageView imageView, int icono) {
        imageView.setEnabled(true);
        imageView.setImageResource(icono);
    }
}
