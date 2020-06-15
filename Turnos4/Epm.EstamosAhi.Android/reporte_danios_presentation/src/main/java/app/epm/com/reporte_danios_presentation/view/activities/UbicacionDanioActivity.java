package app.epm.com.reporte_danios_presentation.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.os.Parcelable;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Toast;

import com.epm.app.app_utilities_presentation.views.activities.BaseUbicacionDeFraudeOrDanioActivity;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.esri.arcgisruntime.io.JsonSerializable;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

import app.epm.com.utilities.helpers.TypeDanioOrFraude;
import app.epm.com.reporte_danios_domain.danios.business_models.ReportDanio;
import app.epm.com.reporte_danios_presentation.dependency_injection.DomainModule;
import app.epm.com.reporte_danios_presentation.presenters.UbicacionDanioPresenter;
import app.epm.com.reporte_danios_presentation.view.views_activities.IUbicacionDanioView;
import app.epm.com.utilities.controls.CustomTextViewNormal;
import app.epm.com.utilities.helpers.UbicationHelper;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.R;


public class UbicacionDanioActivity extends BaseUbicacionDeFraudeOrDanioActivity<UbicacionDanioPresenter>
        implements IUbicacionDanioView {


    private List<TypeDanioOrFraude> dataList;
    private ReportDanio reportDanio;
    private Toolbar toolbarApp;

    @Override
    protected String getTitleCustom() {
        return getResources().getString(R.string.title_direccion);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        setPresenter(new UbicacionDanioPresenter(DomainModule.getDaniosBLInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        super.onCreate(savedInstanceState);
        loadToolbar();
    }

    private void loadToolbar() {
        toolbarApp = (Toolbar) findViewById(app.epm.com.reporte_danios_presentation.R.id.toolbar);
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
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == Constants.DESCRIBE_DANIO) {
            this.reportDanio = (ReportDanio) data.getSerializableExtra(Constants.REPORT_DANIOS);

            if (reportDanio.getAddress() != null) {
                ubicacion_etAddress.setText(reportDanio.getLugarReferencia());
            }
        }

        if (resultCode == Constants.SERVICES_DANIO) {
            finish();
        }

        super.onActivityResult(requestCode, resultCode, data);
    }

    private void startDescribeDanioActivity() {
        Intent intent = new Intent(this, DescribeDanioActivity.class);
        intent.putExtra(Constants.LIST_TYPE_DANIO, (Serializable) dataList);
        intent.putExtra(Constants.TIPO_SERVICIO, tipoServicio);
        intent.putExtra(Constants.INFORMACION_UBICACION, informacionDeUbicacion);
        intent.putExtra(Constants.ADDRESS, ubicacion_etAddress.getText().toString());

        if (reportDanio == null) {
            reportDanio = new ReportDanio();
        }

        reportDanio.setPointSelectedSerializable(pointSelected.toJson());
        reportDanio.setUrlServicio(mapa.getUrlServicio());
        intent.putExtra(Constants.REPORT_DANIOS, reportDanio);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    @Override
    public void showAlertDialogToStartDescribeDanioActivityOnUiThread(final String title, final String message,
                                                                      final boolean hasCoberturEnergia,
                                                                      final boolean hasCoberturaIluminaria) {
        runOnUiThread(() -> showAlertDialogToStartDescribeDanioActivity(title, message, hasCoberturEnergia, hasCoberturaIluminaria));
    }

    @Override
    public void getListTypeDanio(final boolean hasCoberturEnergia, final boolean hasCoberturaIluminaria) {
        final ArrayList<TypeDanioOrFraude> templateList = servicesArcGIS.getListNameTypeDanio(hasCoberturEnergia, hasCoberturaIluminaria);
        runOnUiThread(() -> {
            if (templateList.size() > 0) {
                dataList = templateList;
                startDescribeDanioActivity();
            } else {
                Toast.makeText(UbicacionDanioActivity.this, Constants.DATA_ERROR_MESSAGE, Toast.LENGTH_LONG).show();
            }
        });
    }

    private void showAlertDialogToStartDescribeDanioActivity(String title, String message,
                                                             final boolean hasCoberturEnergia,
                                                             final boolean hasCoberturaIluminaria) {
        getCustomAlertDialog().showAlertDialog(title, message, true, R.string.text_aceptar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                getListTypeDanio(hasCoberturEnergia, hasCoberturaIluminaria);
            }
        }, null);
    }

    @Override
    public void onClickBtnContinuar() {
        if (informacionDeUbicacion != null) {
            final ParametrosCobertura parametrosCobertura = new ParametrosCobertura();
            parametrosCobertura.setDepartamento(informacionDeUbicacion.getDeparatamento());
            parametrosCobertura.setMunicipio(informacionDeUbicacion.getMunicipio());
            parametrosCobertura.setTipoServicio(tipoServicio.getValue());
            getPresenter().validateInternetToExecuteAnAction(() -> getPresenter().validateCoberturaAndTipoServicio(parametrosCobertura));
        } else {
            showAlertDialogGeneralInformationOnUiThread(getName(), R.string.text_empty_address);
        }
    }

    @Override
    public void customOnBackPressed() {
        getCustomAlertDialog().showAlertDialog(getName(), R.string.text_exit_ubicacion_danio, false,
                R.string.text_aceptar, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        UbicacionDanioActivity.super.customOnBackPressed();
                    }
                }, R.string.text_cancelar, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        dialogInterface.dismiss();
                    }
                }, null);
    }
}