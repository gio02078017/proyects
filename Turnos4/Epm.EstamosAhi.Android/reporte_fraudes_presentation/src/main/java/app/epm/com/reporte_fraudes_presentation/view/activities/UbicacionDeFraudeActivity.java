package app.epm.com.reporte_fraudes_presentation.view.activities;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.CompoundButton;
import android.widget.Toast;

import com.epm.app.app_utilities_presentation.views.activities.BaseUbicacionDeFraudeOrDanioActivity;
import com.epm.app.business_models.business_models.ParametrosCobertura;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

import app.epm.com.reporte_fraudes_domain.business_models.ReporteDeFraude;
import app.epm.com.reporte_fraudes_presentation.dependency_injection.DomainModule;
import app.epm.com.reporte_fraudes_presentation.presenters.UbicacionDeFraudePresenter;
import app.epm.com.reporte_fraudes_presentation.view.views_activities.IUbicacionDeFraudeView;
import app.epm.com.utilities.controls.CustomTextViewNormal;
import app.epm.com.utilities.helpers.TypeDanioOrFraude;
import app.epm.com.utilities.helpers.UbicationHelper;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.R;

/**
 * Created by leidycarolinazuluagabastidas on 19/04/17.
 */

public class UbicacionDeFraudeActivity extends BaseUbicacionDeFraudeOrDanioActivity<UbicacionDeFraudePresenter> implements IUbicacionDeFraudeView {

    private boolean isCheckAnonymous;
    private String address;
    private ReporteDeFraude reporteDeFraude;
    private ArrayList<String> attachList;
    private Toolbar toolbarApp;

    @Override
    protected String getTitleCustom() {
        return getResources().getString(R.string.text_direccion_fraude);
    }

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        setPresenter(new UbicacionDeFraudePresenter(DomainModule.getReporteDeFraudesBusinessLogicInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        super.onCreate(savedInstanceState);
        getDataIntent();
        loadToolbar();
        loadListenersToTheControls();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == Constants.REPORTE_FRAUDE) {
            isCheckAnonymous = data.getBooleanExtra(Constants.STATE_ANONYMOUS, isCheckAnonymous);
            address = data.getStringExtra(Constants.ADDRESS_FRAUDE);
            reporteDeFraude = (ReporteDeFraude) data.getSerializableExtra(Constants.REPORT_FRAUDE);
            loadCheckToTheContolUbicacionCheckBoxAnonymous();
            loadOnCheckBoxChangedToTheControlUbicacionCheckBoxAnonymous();
            loadTextToTheControlUbicacionEditTextAddress();
        }else if(resultCode == Constants.SERVICIOS_FRAUDE){
            setResult(Constants.SERVICIOS_FRAUDE);
            finish();
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    private void getDataIntent() {
        isCheckAnonymous = getIntent().getBooleanExtra(Constants.STATE_ANONYMOUS, isCheckAnonymous);
        reporteDeFraude = (ReporteDeFraude) getIntent().getSerializableExtra(Constants.REPORT_FRAUDE);
        address = getIntent().getStringExtra(Constants.ADDRESS_FRAUDE);
        attachList = getIntent().getStringArrayListExtra(Constants.ATTACHLIST);
        if (reporteDeFraude == null) {
            reporteDeFraude = new ReporteDeFraude();
        }
    }

    private void loadToolbar() {
        toolbarApp = (Toolbar) findViewById(app.epm.com.reporte_fraudes_presentation.R.id.toolbar);
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

    private void loadListenersToTheControls() {
        loadVisibilityToTheControlUbicacionRelativeLayoutAnonymous();
        loadImageToTheControlUbicacionImageViewProgress();
        loadCheckToTheContolUbicacionCheckBoxAnonymous();
        loadOnCheckBoxChangedToTheControlUbicacionCheckBoxAnonymous();
        loadTextToTheControlUbicacionTextViewTitle();
        loadTextToTheControlUbicacionEditTextAddress();
    }

    private void loadVisibilityToTheControlUbicacionRelativeLayoutAnonymous() {
        ubicacion_rlAnonymous.setVisibility(View.VISIBLE);
    }

    private void loadImageToTheControlUbicacionImageViewProgress() {
        ubicacion_ivProgress.setImageResource(R.mipmap.step_two);
    }

    private void loadCheckToTheContolUbicacionCheckBoxAnonymous() {
        ubicacion_cbAnonymous.setButtonDrawable(isCheckAnonymous ? R.mipmap.checkbox_on : R.mipmap.checkbox_off);
    }

    private void loadOnCheckBoxChangedToTheControlUbicacionCheckBoxAnonymous() {
        ubicacion_cbAnonymous.setChecked(isCheckAnonymous);
        ubicacion_cbAnonymous.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton compoundButton, boolean isChecked) {
                ubicacion_cbAnonymous.setButtonDrawable(isChecked ? R.mipmap.checkbox_on : R.mipmap.checkbox_off);
                isCheckAnonymous = isChecked;
            }
        });
    }

    private void loadTextToTheControlUbicacionTextViewTitle() {
        ubicacion_tvTitle.setText(R.string.text_ubica_mapa_fraude);
    }

    private void loadTextToTheControlUbicacionEditTextAddress() {
        if (address != null) {
            ubicacion_etAddress.setText(address);
            ubicacion_etAddress.setSelection(address.length());
        }
    }

    @Override
    public void onClickBtnContinuar() {
        if (informacionDeUbicacion != null) {
            final ParametrosCobertura parametrosCobertura = new ParametrosCobertura();
            parametrosCobertura.setDepartamento(informacionDeUbicacion.getDeparatamento());
            parametrosCobertura.setMunicipio(informacionDeUbicacion.getMunicipio());
            parametrosCobertura.setTipoServicio(tipoServicio.getValue());
            getPresenter().validateInternetToExecuteAnAction(() -> getPresenter().validateInternetGetListMunicipalities(parametrosCobertura));
        } else {
            showAlertDialogGeneralInformationOnUiThread(getName(), R.string.text_empty_address_fraude);
        }
    }

    @Override
    public void setInformationToIntent(final List<String> listMunicipialities) {
        runOnUiThread(() -> startActivityRegisterReporteDeFraude(listMunicipialities));
    }

    private void startActivityRegisterReporteDeFraude(List<String> listMunicipialities) {
        final ArrayList<TypeDanioOrFraude> templateList = servicesArcGIS.getListNameTypeFraude();
        if (templateList.size() > 0) {
            Intent intent = new Intent(this, RegisterReporteDeFraudesActivity.class);
            intent.putExtra(Constants.TIPO_SERVICIO, tipoServicio);
            intent.putExtra(Constants.INFORMACION_UBICACION, informacionDeUbicacion);
            intent.putExtra(Constants.STATE_ANONYMOUS, isCheckAnonymous);
            intent.putExtra(Constants.LIST_MUNICIPIES, (Serializable) listMunicipialities);
            intent.putExtra(Constants.ADDRESS, ubicacion_etAddress.getText().toString());
            intent.putExtra(Constants.LIST_TYPE_FRAUDE, templateList);
            intent.putStringArrayListExtra(Constants.ATTACHLIST, attachList);
            reporteDeFraude.setPointSelectedSerializable(pointSelected.toJson());
            reporteDeFraude.setUrlServices(mapa.getUrlServicio());
            intent.putExtra(Constants.REPORT_FRAUDE, reporteDeFraude);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        } else {
            Toast.makeText(this, Constants.DATA_ERROR_MESSAGE_FRAUDE, Toast.LENGTH_LONG).show();
        }
    }

    @Override
    protected void customOnBackPressed() {
        Intent intent = new Intent();
        intent.putExtra(Constants.STATE_ANONYMOUS, isCheckAnonymous);
        intent.putExtra(Constants.REPORT_FRAUDE, reporteDeFraude);
        intent.putExtra(Constants.ADDRESS_FRAUDE, ubicacion_etAddress.getText().toString());
        setResultData(Constants.REPORTE_FRAUDE, intent);
        finish();
    }
}