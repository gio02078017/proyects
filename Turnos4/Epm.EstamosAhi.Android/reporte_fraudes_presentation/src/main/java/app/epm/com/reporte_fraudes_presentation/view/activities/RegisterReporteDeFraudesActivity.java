package app.epm.com.reporte_fraudes_presentation.view.activities;

import android.app.AlertDialog;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.engine.DiskCacheStrategy;
import com.epm.app.app_utilities_presentation.utils.LoadIconServices;
import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import com.esri.arcgisruntime.mapping.ArcGISMap;
import com.esri.arcgisruntime.mapping.Basemap;
import com.esri.arcgisruntime.mapping.view.MapView;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.reporte_fraudes_domain.business_models.ReporteDeFraude;
import app.epm.com.reporte_fraudes_presentation.R;
import app.epm.com.reporte_fraudes_presentation.presenters.RegisterReporteDeFraudesPresenter;
import app.epm.com.reporte_fraudes_presentation.view.views_activities.IRegisterReporteDeFraudesView;
import app.epm.com.reporte_fraudes_presentation.dependency_injection.DomainModule;
import app.epm.com.utilities.helpers.FileManager;
import app.epm.com.utilities.helpers.IFileManager;
import app.epm.com.utilities.helpers.TypeDanioOrFraude;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.services.ServicesArcGIS;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 4/04/17.
 */

public class RegisterReporteDeFraudesActivity extends BaseActivity<RegisterReporteDeFraudesPresenter> implements IRegisterReporteDeFraudesView, TextWatcher {

    private Toolbar toolbarApp;
    private CheckBox registerReporteDeFraude_checkBox_anonymous;
    private ImageView registerReporteDeFraude_imageView_service;
    private TextView registerReporteDeFraude_textView_evidence;
    private LinearLayout registerReporteDeFraude_linearLayout_evidenceImages;
    private TextView registerReporteDeFraude_textView_type;
    private TextView registerReporteDeFraude_textView_municipality;
    private EditText registerReporteDeFraude_editText_sector;
    private EditText registerReporteDeFraude_editText_address;
    private EditText registerReporteDeFraude_editText_description;
    private EditText registerReporteDeFraude_editText_horary;
    private LinearLayout registerReporteDeFraude_linearLayout_fieldsAnonymous;
    private EditText registerReporteDeFraude_editText_phone;
    private EditText registerReporteDeFraude_editText_email;
    private EditText registerReporteDeFraude_editText_name;
    private Button registerReporteDeFraude_button_sendRegister;

    private ReporteDeFraude reporteDeFraude;
    private AlertDialog alertDialog;
    private boolean isCheckAnonymous;
    private ETipoServicio tipoServicio;
    private InformacionDeUbicacion informacionDeUbicacion;
    private String addressUbication;
    private List<String> listMunicipialities;
    private String itemSelected;
    private TypeDanioOrFraude itemSelectedType;
    private List<TypeDanioOrFraude> listTypeFraude;
    private ArrayList<String> attachList;
    private IFileManager fileManager;
    private ServicesArcGIS servicesArcGIS;
    private Validations validations;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register_reporte_fraude);
        loadDrawerLayout(R.id.generalDrawerLayout);
        setPresenter(new RegisterReporteDeFraudesPresenter(DomainModule.getReporteDeFraudesBusinessLogicInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        fileManager = new FileManager(this);
        createProgressDialog();
        loadViews();
        getDataIntent();
        loadInfo();
        setViewData();
        loadServicesArcGIS(reporteDeFraude.getUrlServices());
        validations = new Validations();
    }

    private void setViewData() {
        if (attachList != null && attachList.size() > 0) {
            registerReporteDeFraude_textView_evidence.setVisibility(View.GONE);
            for (String fileName : attachList) {
                ImageView imageView = fileManager.getImageViewInstance();
                if (fileName.contains("mp3")) {
                    imageView.setAdjustViewBounds(true);
                    Glide.with(this).load(fileName).thumbnail(1f).fitCenter().placeholder(R.mipmap.icon_file_audio_attach).diskCacheStrategy(DiskCacheStrategy.ALL).into(imageView);
                } else {
                    Glide.with(this).load(fileName).thumbnail(1f).fitCenter().diskCacheStrategy(DiskCacheStrategy.ALL).into(imageView);
                }

                registerReporteDeFraude_linearLayout_evidenceImages.addView(imageView);
            }
        }
    }

    private void getDataIntent() {
        isCheckAnonymous = getIntent().getBooleanExtra(Constants.STATE_ANONYMOUS, isCheckAnonymous);
        tipoServicio = (ETipoServicio) getIntent().getSerializableExtra(Constants.TIPO_SERVICIO);
        informacionDeUbicacion = (InformacionDeUbicacion) getIntent().getSerializableExtra(Constants.INFORMACION_UBICACION);
        listMunicipialities = getIntent().getStringArrayListExtra(Constants.LIST_MUNICIPIES);
        addressUbication = getIntent().getStringExtra(Constants.ADDRESS);
        listTypeFraude = (List<TypeDanioOrFraude>) getIntent().getSerializableExtra(Constants.LIST_TYPE_FRAUDE);
        reporteDeFraude = (ReporteDeFraude) getIntent().getSerializableExtra(Constants.REPORT_FRAUDE);
        attachList = getIntent().getStringArrayListExtra(Constants.ATTACHLIST);
        loadIconServices();
        loadCheckToTheContolRegisterReporteDeFraudeCheckBoxAnonymous();
    }

    private void loadViews() {
        toolbarApp = findViewById(R.id.toolbar);
        registerReporteDeFraude_checkBox_anonymous = findViewById(R.id.registerReporteDeFraude_checkBox_anonymous);
        registerReporteDeFraude_imageView_service = findViewById(R.id.registerReporteDeFraude_imageView_service);
        registerReporteDeFraude_textView_evidence = findViewById(R.id.registerReporteDeFraude_textView_evidence);
        registerReporteDeFraude_linearLayout_evidenceImages = findViewById(R.id.registerReporteDeFraude_linearLayout_evidenceImages);
        registerReporteDeFraude_textView_type = findViewById(R.id.registerReporteDeFraude_textView_type);
        registerReporteDeFraude_textView_type.addTextChangedListener(this);
        registerReporteDeFraude_textView_municipality = findViewById(R.id.registerReporteDeFraude_textView_municipality);
        registerReporteDeFraude_textView_municipality.addTextChangedListener(this);
        registerReporteDeFraude_editText_sector = findViewById(R.id.registerReporteDeFraude_editText_sector);
        registerReporteDeFraude_editText_sector.addTextChangedListener(this);
        registerReporteDeFraude_editText_address = findViewById(R.id.registerReporteDeFraude_editText_address);
        registerReporteDeFraude_editText_address.addTextChangedListener(this);
        registerReporteDeFraude_editText_description = findViewById(R.id.registerReporteDeFraude_editText_description);
        registerReporteDeFraude_editText_description.addTextChangedListener(this);
        registerReporteDeFraude_editText_horary = findViewById(R.id.registerReporteDeFraude_editText_horary);
        registerReporteDeFraude_editText_horary.addTextChangedListener(this);
        registerReporteDeFraude_linearLayout_fieldsAnonymous = findViewById(R.id.registerReporteDeFraude_linearLayout_fieldsAnonymous);
        registerReporteDeFraude_editText_phone = findViewById(R.id.registerReporteDeFraude_editText_phone);
        registerReporteDeFraude_editText_phone.addTextChangedListener(this);
        registerReporteDeFraude_editText_email = findViewById(R.id.registerReporteDeFraude_editText_email);
        registerReporteDeFraude_editText_email.addTextChangedListener(this);
        registerReporteDeFraude_editText_name = findViewById(R.id.registerReporteDeFraude_editText_name);
        registerReporteDeFraude_editText_name.addTextChangedListener(this);
        registerReporteDeFraude_button_sendRegister = findViewById(R.id.registerReporteDeFraude_button_sendRegister);
        loadToolbar();
        loadListenersToTheControls();
    }

    private void loadIconServices() {
        LoadIconServices loadIconServices = new LoadIconServices();
        registerReporteDeFraude_imageView_service.setImageResource(loadIconServices.setIdIconoServicio(this.tipoServicio.getName()));
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar(toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(view -> onBackPressed());
    }

    private void loadListenersToTheControls() {
        loadOnCheckBoxChangedToTheControlRegisterReporteDeFraudeCheckBoxAnonymous();
        loadOnClickListenerToTheControlRegisterReporteDeFraudeTextViewType();
        loadOnClickListenerToTheControlRegisterReporteDeFraudeTextViewMunicipality();
        loadOnClickListenerToTheControlRegisterReporteDeFraudeButtonSendRegister();
    }

    private void loadInfo() {
        registerReporteDeFraude_editText_address.setText(addressUbication);
        registerReporteDeFraude_textView_municipality.setText(informacionDeUbicacion.getMunicipio());
        if (reporteDeFraude != null) {
            registerReporteDeFraude_editText_sector.setText(reporteDeFraude.getSector());
            registerReporteDeFraude_editText_description.setText(reporteDeFraude.getDescribeReport());
            registerReporteDeFraude_editText_horary.setText(reporteDeFraude.getHorary());
            registerReporteDeFraude_editText_phone.setText(reporteDeFraude.getTelephone());
            registerReporteDeFraude_editText_email.setText(reporteDeFraude.getUserEmail());
            registerReporteDeFraude_editText_name.setText(reporteDeFraude.getUserName());
        }
        if (getUsuario() != null) {
            if (!getUsuario().isInvitado()) {
                registerReporteDeFraude_editText_name.setText(getUsuario().getNombres() + " " + getUsuario().getApellido());
                registerReporteDeFraude_editText_email.setText(getUsuario().getCorreoElectronico());
                if (!getUsuario().getCelular().isEmpty()) {
                    registerReporteDeFraude_editText_phone.setText(getUsuario().getCelular());
                } else {
                    registerReporteDeFraude_editText_phone.setText(getUsuario().getTelefono());
                }
            }
        }
        setFocusControls();
    }

    private void setFocusControls() {
        registerReporteDeFraude_editText_sector.setSelection(registerReporteDeFraude_editText_sector.getText().length());
        registerReporteDeFraude_editText_address.setSelection(registerReporteDeFraude_editText_address.getText().length());
        registerReporteDeFraude_editText_description.setSelection(registerReporteDeFraude_editText_description.getText().length());
        registerReporteDeFraude_editText_horary.setSelection(registerReporteDeFraude_editText_horary.getText().length());
        registerReporteDeFraude_editText_phone.setSelection(registerReporteDeFraude_editText_phone.getText().length());
        registerReporteDeFraude_editText_email.setSelection(registerReporteDeFraude_editText_email.getText().length());
        registerReporteDeFraude_editText_name.setSelection(registerReporteDeFraude_editText_name.getText().length());
    }

    private void loadCheckToTheContolRegisterReporteDeFraudeCheckBoxAnonymous() {
        registerReporteDeFraude_checkBox_anonymous.setButtonDrawable(isCheckAnonymous ? R.mipmap.checkbox_on : R.mipmap.checkbox_off);
        validateStateAnonymous();
    }

    private void loadOnCheckBoxChangedToTheControlRegisterReporteDeFraudeCheckBoxAnonymous() {
        registerReporteDeFraude_checkBox_anonymous.setChecked(isCheckAnonymous);
        registerReporteDeFraude_checkBox_anonymous.setOnCheckedChangeListener((compoundButton, isChecked) -> {
            registerReporteDeFraude_checkBox_anonymous.setButtonDrawable(isChecked ? R.mipmap.checkbox_on : R.mipmap.checkbox_off);
            isCheckAnonymous = isChecked;
            validateStateAnonymous();
        });
    }

    private void loadOnClickListenerToTheControlRegisterReporteDeFraudeTextViewType() {
        registerReporteDeFraude_textView_type.setOnClickListener(v -> loadListsTheReporteFraudes(R.string.text_tipo_fraude, listTypeFraude, registerReporteDeFraude_textView_type));
    }

    private void loadOnClickListenerToTheControlRegisterReporteDeFraudeTextViewMunicipality() {
        registerReporteDeFraude_textView_municipality.setOnClickListener(v -> loadListMunicipialities(R.string.text_municipios_fraude, listMunicipialities, registerReporteDeFraude_textView_municipality));
    }

    private void loadOnClickListenerToTheControlRegisterReporteDeFraudeButtonSendRegister() {
        registerReporteDeFraude_button_sendRegister.setOnClickListener(v -> sendRegisterTheReporteDeFraude());
    }

    private void validateStateAnonymous() {
        if (isCheckAnonymous) {
            registerReporteDeFraude_linearLayout_fieldsAnonymous.setVisibility(View.GONE);
            changeStatusToButtonSendRegister();
        } else {
            registerReporteDeFraude_linearLayout_fieldsAnonymous.setVisibility(View.VISIBLE);
            changeStatusToButtonSendRegister();
        }
    }

    private void loadListMunicipialities(int title, List<String> listMunicipialities,
                                         final TextView textView) {
        final List<String> list = listMunicipialities;
        String[] items = getItemsArrayFromItemList(list);

        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle(title);
        builder.setItems(items, (dialog, which) -> {
            itemSelected = list.get(which);
            loadTextItemSelected(textView);
        });
        alertDialog = builder.create();
        alertDialog.show();
    }

    private void loadTextItemSelected(TextView textView) {
        textView.setText(itemSelected);
    }

    private String[] getItemsArrayFromItemList(List<String> tiposItemsList) {
        String[] items = new String[tiposItemsList.size()];
        for (int i = 0; i < tiposItemsList.size(); i++) {
            items[i] = tiposItemsList.get(i);
        }
        return items;
    }

    private void loadListsTheReporteFraudes(int title, List<TypeDanioOrFraude> listMunicipialities,
                                            final TextView textView) {
        final List<TypeDanioOrFraude> list = listMunicipialities;
        String[] items = getItemsArrayFromItemListTypeFraude(list);

        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle(title);
        builder.setItems(items, (dialog, which) -> {
            itemSelectedType = list.get(which);
            loadTextItemSelectedTypeFraude(textView);
        });
        alertDialog = builder.create();
        alertDialog.show();
    }

    private void loadTextItemSelectedTypeFraude(TextView textView) {
        textView.setText(itemSelectedType.getNameType());
    }

    private String[] getItemsArrayFromItemListTypeFraude(List<TypeDanioOrFraude> tiposItemsList) {
        String[] items = new String[tiposItemsList.size()];
        for (int i = 0; i < tiposItemsList.size(); i++) {
            items[i] = tiposItemsList.get(i).getNameType();
        }
        return items;
    }

    private void sendRegisterTheReporteDeFraude() {
        if (isCheckAnonymous) {
            getPresenter().validateInternetSendrReporteDeFraude(setParametersEmptyToAnonymous(setDataReportFraude()), servicesArcGIS, this);
        } else {
            if (validatePhoneAndEmail()) {
                getPresenter().validateInternetSendrReporteDeFraude(setParametersEmptyToAnonymous(setDataReportFraude()), servicesArcGIS, this);
            }
        }
    }

    private void loadServicesArcGIS(String url) {
        MapView base_mapView = new MapView(this);
        ArcGISMap map = new ArcGISMap(Basemap.Type.OPEN_STREET_MAP, 6.234703, -75.5514745, 12);
        servicesArcGIS = new ServicesArcGIS(url, map, this);
        base_mapView.setMap(map);
    }

    @Override
    protected void customOnBackPressed() {
        Intent intent = new Intent();
        intent.putExtra(Constants.STATE_ANONYMOUS, isCheckAnonymous);
        intent.putExtra(Constants.ADDRESS_FRAUDE, registerReporteDeFraude_editText_address.getText().toString());
        intent.putExtra(Constants.REPORT_FRAUDE, setDataReportFraude());
        setResultData(Constants.REPORTE_FRAUDE, intent);
        finish();
    }

    private ReporteDeFraude setDataReportFraude() {
        reporteDeFraude.setArrayFiles(attachList);
        reporteDeFraude.setTypeReport(registerReporteDeFraude_textView_type.getText().toString());
        reporteDeFraude.setMunicipality(registerReporteDeFraude_textView_municipality.getText().toString());
        reporteDeFraude.setSector(registerReporteDeFraude_editText_sector.getText().toString());
        reporteDeFraude.setAddress(informacionDeUbicacion.getDireccion());
        reporteDeFraude.setDescribeReport(registerReporteDeFraude_editText_description.getText().toString());
        reporteDeFraude.setHorary(registerReporteDeFraude_editText_horary.getText().toString());
        reporteDeFraude.setTelephone(registerReporteDeFraude_editText_phone.getText().toString());
        reporteDeFraude.setUserEmail(registerReporteDeFraude_editText_email.getText().toString());
        reporteDeFraude.setUserName(registerReporteDeFraude_editText_name.getText().toString());
        reporteDeFraude.setReferencePlace(registerReporteDeFraude_editText_address.getText().toString());
        reporteDeFraude.setTypeService(tipoServicio);
        reporteDeFraude.setIdType(validations.getIdFromListByString(registerReporteDeFraude_textView_type.getText().toString(), listTypeFraude));
        reporteDeFraude = setParameterEmptyToAnonymous();
        return reporteDeFraude;
    }

    private ReporteDeFraude setParameterEmptyToAnonymous() {
        if (isCheckAnonymous) {
            reporteDeFraude.setTelephone(Constants.EMPTY_STRING);
            reporteDeFraude.setUserEmail(Constants.EMPTY_STRING);
            reporteDeFraude.setUserName(Constants.EMPTY_STRING);
        }
        return reporteDeFraude;
    }

    private void changeStatusToButtonSendRegister() {
        if (isCheckAnonymous) {
            validateFieldsWhenIsAnonymous();
        } else {
            validateFieldsWhenIsNotAnonymous();
        }
    }

    private void validateFieldsWhenIsAnonymous() {
        if (registerReporteDeFraude_textView_type.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_textView_municipality.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_editText_sector.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_editText_address.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_editText_description.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_editText_horary.getText().toString().trim().isEmpty()) {
            registerReporteDeFraude_button_sendRegister.setEnabled(false);
            registerReporteDeFraude_button_sendRegister.setBackgroundResource(R.color.gray_button_continue);
        } else {
            registerReporteDeFraude_button_sendRegister.setEnabled(true);
            registerReporteDeFraude_button_sendRegister.setBackgroundResource(R.color.button_green_factura);
        }
    }

    private void validateFieldsWhenIsNotAnonymous() {
        if (registerReporteDeFraude_textView_type.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_textView_municipality.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_editText_sector.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_editText_address.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_editText_description.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_editText_horary.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_editText_phone.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_editText_email.getText().toString().trim().isEmpty() ||
                registerReporteDeFraude_editText_name.getText().toString().trim().isEmpty()) {
            registerReporteDeFraude_button_sendRegister.setEnabled(false);
            registerReporteDeFraude_button_sendRegister.setBackgroundResource(R.color.gray_button_continue);
        } else {
            registerReporteDeFraude_button_sendRegister.setEnabled(true);
            registerReporteDeFraude_button_sendRegister.setBackgroundResource(R.color.button_green_factura);
        }
    }

    @Override
    public void beforeTextChanged(CharSequence s, int start, int count, int after) {
        //The method is not used.
    }

    @Override
    public void onTextChanged(CharSequence s, int start, int before, int count) {
        changeStatusToButtonSendRegister();
    }

    @Override
    public void afterTextChanged(Editable s) {
        //The method is not used.
    }

    @Override
    public void showAlertDialogSendReportFraude(final String numberReport, final String service) {
        runOnUiThread(() -> {
            final String message = String.format("%s %s. %s %s. %s", getString(R.string.message_report_one), service,
                    getString(R.string.message_report_two), numberReport, getString(R.string.message_report_three));
            getCustomAlertDialog().showAlertDialog(R.string.title_send_danios_successful, message, false, R.string.text_aceptar, (dialogInterface, i) -> getCustomAlertDialog().showAlertDialog(getName(), R.string.message_send_fraude_continue_report, false,
                    R.string.button_yes, (dialogInterface1, i1) -> {
                        startServicesDeFraudesActivity();
                        dialogInterface1.dismiss();
                    }, R.string.button_no, (dialogInterface12, i12) -> {
                        try {
                            Class clazz = Class.forName(getCustomSharedPreferences().getString(Constants.LANDING_CLASS));
                            Intent intent = new Intent(RegisterReporteDeFraudesActivity.this, clazz);
                            intent.putExtra(Constants.CALLED_FROM_ANOTHER_MODULE, true);
                            intent.putExtra(Constants.SHOW_ALERT_RATE, Constants.RATE_APP);
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
                        } catch (ClassNotFoundException e) {
                            Log.e("Exception", e.toString());
                        }
                    }, null), null);
        });
    }

    private void startServicesDeFraudesActivity() {
        Intent intent = new Intent(this, ServicesDeFraudesActivity.class);
        intent.putExtra(Constants.SHOW_ALERT_RATE, Constants.RATE_APP);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private boolean validatePhoneAndEmail() {
        if (validations.validatePhone(registerReporteDeFraude_editText_phone.getText().toString(),
                Constants.MAX_LENGTH_PHONE_FRAUDE, Constants.MIN_LENGTH_PHONE)) {
            showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.number_phone_invalid_report);
            return false;
        }
        if (!registerReporteDeFraude_editText_email.getText().toString()
                .matches(Constants.REGULAR_EXPRESSION_CORRECT_EMAIL)) {
            showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
            return false;
        }
        return true;
    }

    private ReporteDeFraude setParametersEmptyToAnonymous(ReporteDeFraude reporteDeFraude) {
        if (isCheckAnonymous) {
            reporteDeFraude.setTelephone(Constants.EMPTY_STRING);
            reporteDeFraude.setUserEmail(Constants.EMPTY_STRING);
            reporteDeFraude.setUserName(Constants.EMPTY_STRING);
        }
        return reporteDeFraude;
    }
}