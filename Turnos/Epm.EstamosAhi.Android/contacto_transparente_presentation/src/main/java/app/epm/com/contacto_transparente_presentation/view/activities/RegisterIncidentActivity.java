package app.epm.com.contacto_transparente_presentation.view.activities;

import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.content.Intent;
import android.os.Bundle;
import androidx.appcompat.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TextView;

import java.text.DateFormatSymbols;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Locale;

import app.epm.com.contacto_transparente_domain.business_models.GrupoInteres;
import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.utilities.custom_controls.CustomAlertDialog;
import app.epm.com.utilities.helpers.CustomAlertDialogInputData;
import app.epm.com.utilities.helpers.FormatDateFactura;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by leidycarolinazuluagabastidas on 10/03/17.
 */

public class RegisterIncidentActivity extends BaseActivity implements DatePickerDialog.OnDateSetListener,
        TextWatcher, CompoundButton.OnCheckedChangeListener,
        CustomAlertDialogInputData.OnChangeDoubleTab {

    private Toolbar toolbarApp;
    private CheckBox describe_cbAnonymous;
    private CheckBox describe_cbPeople;
    private CheckBox describe_cbCompany;
    private EditText describe_etPlace;
    private EditText describe_etDescription;
    private EditText describe_etPeople;
    private EditText describe_etCompany;
    private EditText describe_etCalendar;
    private EditText describe_etName;
    private EditText describe_etEmail;
    private EditText describe_etPhone;
    private EditText describe_etGroup;
    private View describe_viewLinePeople;
    private View describe_viewLineCompany;
    private TextView describe_tvName;
    private TextView describe_tvEmail;
    private TextView describe_tvPhone;
    private TextView describe_tvGroup;
    private Button describe_btnContinue;
    private CustomAlertDialog customAlertDialog;
    private CustomAlertDialogInputData customAlertDialogInputData;
    private DatePickerDialog datePickerDialog;
    private Calendar calendar;
    private int currentYear;
    private int currentMonth;
    private int currentDay;
    private String dateFormat;
    private AlertDialog alertDialog;
    private GrupoInteres itemGeneralSelected;
    private List<GrupoInteres> groupInterestList;
    private boolean stateAnonymous;
    private Validations validations;
    private String idInterestGroup;
    private ArrayList<String> arrayAttach;
    private boolean controlDoubleTab = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_describe);
        loadDrawerLayout(R.id.generalDrawerLayout);
        this.customAlertDialog = new CustomAlertDialog(this);
        this.customAlertDialogInputData = new CustomAlertDialogInputData();
        calendar = Calendar.getInstance();
        calendar = Calendar.getInstance();
        currentYear = calendar.get(Calendar.YEAR);
        currentMonth = calendar.get(Calendar.MONTH);
        currentDay = calendar.get(Calendar.DAY_OF_MONTH);
        getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_HIDDEN);
        datePickerDialog = new DatePickerDialog(RegisterIncidentActivity.this, RegisterIncidentActivity.this, currentYear, currentMonth, currentDay);
        stateAnonymous = getIntent().getBooleanExtra(Constants.STATE_ANONYMOUS, false);
        groupInterestList = (List<GrupoInteres>) getIntent().getSerializableExtra(Constants.LISTA_GRUPO_INTERES);
        hideKeyboard();
        loadViews();
        validateUser();
        createProgressDialog();
    }

    @Override
    protected void onResume() {
        super.onResume();
        controlDoubleTab = false;
    }

    private void validateUser() {
        Log.e(RegisterIncidentActivity.class.getName(), stateAnonymous + "antes del check");
        describe_cbAnonymous.setChecked(stateAnonymous);
        Log.e(RegisterIncidentActivity.class.getName(), stateAnonymous + "luego del check");
        if (getUsuario() != null) {
            Log.e(RegisterIncidentActivity.class.getName(), getUsuario() + "usuario");
            if (getUsuario().isInvitado() == false) {

                describe_etName.setText(getUsuario().getNombres());
                describe_etEmail.setText(getUsuario().getCorreoElectronico());
                if (!getUsuario().getCelular().isEmpty()) {
                    describe_etPhone.setText(getUsuario().getCelular());
                } else {
                    describe_etPhone.setText(getUsuario().getTelefono());
                }
            }
        }
    }

    private void hideKeyboard() {
        getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_HIDDEN);
    }

    private void loadViews() {
        toolbarApp = findViewById(R.id.toolbar);
        describe_cbAnonymous =  findViewById(R.id.describe_cbAnonymous);
        describe_viewLinePeople = findViewById(R.id.describe_viewLinePeople);
        describe_viewLineCompany = findViewById(R.id.describe_viewLineCompany);
        describe_cbAnonymous =  findViewById(R.id.describe_cbAnonymous);
        describe_cbAnonymous.setOnCheckedChangeListener(this);
        describe_cbPeople = findViewById(R.id.describe_cbPeople);
        describe_cbPeople.setOnCheckedChangeListener(this);
        describe_cbCompany =  findViewById(R.id.describe_cbCompany);
        describe_cbCompany.setOnCheckedChangeListener(this);
        describe_etPlace =  findViewById(R.id.describe_etPlace);
        describe_etPlace.addTextChangedListener(this);
        describe_etDescription =  findViewById(R.id.describe_etDescription);
        describe_etDescription.addTextChangedListener(this);
        describe_etPeople = findViewById(R.id.describe_etPeople);
        describe_etPeople.addTextChangedListener(this);
        describe_etCompany =  findViewById(R.id.describe_etCompany);
        describe_etCompany.addTextChangedListener(this);
        describe_etCalendar =  findViewById(R.id.describe_etCalendar);
        describe_etCalendar.addTextChangedListener(this);
        describe_etName =  findViewById(R.id.describe_etName);
        describe_etName.addTextChangedListener(this);
        describe_etEmail =  findViewById(R.id.describe_etEmail);
        describe_etEmail.addTextChangedListener(this);
        describe_etPhone = findViewById(R.id.describe_etPhone);
        describe_etPhone.addTextChangedListener(this);
        describe_etGroup =  findViewById(R.id.describe_etGroup);
        describe_etGroup.addTextChangedListener(this);
        describe_tvName = findViewById(R.id.describe_tvName);
        describe_tvEmail =  findViewById(R.id.describe_tvEmail);
        describe_tvPhone =  findViewById(R.id.describe_tvPhone);
        describe_tvGroup =  findViewById(R.id.describe_tvGroup);
        describe_btnContinue =  findViewById(R.id.describe_btnContinue);
        loadToolbar();
        loadListener();
    }

    private void loadListener() {
        final LayoutInflater inflater = this.getLayoutInflater();
        final Window window = this.getWindow();

        describe_etPlace.setOnClickListener(view -> customAlertDialogInputData.showAlertDialogInputData(window, inflater, RegisterIncidentActivity.this, R.string.text_tv_location, describe_etPlace, describe_etPlace.getText().toString(), 200, controlDoubleTab));
        describe_etDescription.setOnClickListener(view -> customAlertDialogInputData.showAlertDialogInputData(window, inflater, RegisterIncidentActivity.this, R.string.text_tv_description, describe_etDescription, describe_etDescription.getText().toString(), 5000,controlDoubleTab));
        describe_etPeople.setOnClickListener(view -> customAlertDialogInputData.showAlertDialogInputData(window, inflater, RegisterIncidentActivity.this, R.string.text_tv_name_people_involved, describe_etPeople, describe_etPeople.getText().toString(), 500, controlDoubleTab));
        describe_etCompany.setOnClickListener(view -> customAlertDialogInputData.showAlertDialogInputData(window, inflater, RegisterIncidentActivity.this, R.string.text_tv_incident_company, describe_etCompany, describe_etCompany.getText().toString(), 500,controlDoubleTab));
        describe_etCalendar.setOnClickListener(view -> showAlertDialogWithCalendar());
        describe_etGroup.setOnClickListener(view -> loadTiposList(groupInterestList, R.string.title_interest_group, describe_etGroup));
        describe_btnContinue.setOnClickListener(view -> validateFieldsToIntent());
    }

    private void validateFieldsToIntent() {
        if (stateAnonymous) {
            intentToAttachActivity();
        } else {
            if (validatePhoneAndEmail()) {
                intentToAttachActivity();
            }
        }
    }

    private void intentToAttachActivity() {
        Incidente incidente = new Incidente();
        incidente.setAnonimo(stateAnonymous);
        incidente.setCorreoElectronicoContacto(describe_etEmail.getText().toString().trim());
        incidente.setNombreContacto(describe_etName.getText().toString().trim());
        incidente.setTelefonoContacto(describe_etPhone.getText().toString().trim());
        incidente.setDescripcion(describe_etDescription.getText().toString().trim());
        incidente.setPersonasInvolucradas(describe_etPeople.getText().toString().trim());
        incidente.setPersonasInvolucradasEnLaEmpresa(describe_etCompany.getText().toString().trim());
        incidente.setFechaDeteccion(dateFormat);
        incidente.setIdGrupoInteres(itemGeneralSelected != null ? itemGeneralSelected.getId() : null);
        incidente.setLugarEnDondeSucedio(describe_etPlace.getText().toString().trim());
        Intent intent = new Intent(this, AttachEvidenceActivity.class);
        intent.putExtra(Constants.INCIDENT, incidente);
        intent.putStringArrayListExtra(Constants.ATTACHLIST, arrayAttach);
        startActivityWithOutDoubleClick(intent);
    }

    private boolean validatePhoneAndEmail() {
        if (validations.validatePhone(describe_etPhone.getText().toString(), Constants.MAX_LENGTH_PHONE,
                Constants.MIN_LENGTH_PHONE)) {
            showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.number_phone_invalid);
            return false;
        }
        if (!describe_etEmail.getText().toString().matches(Constants.REGULAR_EXPRESSION_CORRECT_EMAIL)) {
            showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
            return false;
        }
        return true;
    }

    private void showAlertDialogWithCalendar() {
        datePickerDialog.setButton(DatePickerDialog.BUTTON_POSITIVE,
                getResources().getString(R.string.text_button_positive_datepicker), datePickerDialog);
        datePickerDialog.setButton(DatePickerDialog.BUTTON_NEGATIVE,
                getResources().getString(R.string.text_cancelar), datePickerDialog);
        datePickerDialog.getDatePicker().setMaxDate(new Date().getTime());
        datePickerDialog.getDatePicker().setMinDate((setMinDateFromPicker()).getTime());
        datePickerDialog.show();
    }

    private Date setMinDateFromPicker() {
        Date convertedDate = new Date();
        SimpleDateFormat dateFormat = new SimpleDateFormat(Constants.FORMAT_DATE);
        try {
            convertedDate = dateFormat.parse(Constants.MINIMUM_DATE);
        } catch (ParseException e) {
            Log.e("Exception", e.toString());
        }
        return convertedDate;
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar( toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        this.toolbarApp.setNavigationOnClickListener(view -> onBackPressed());
    }

    @Override
    public void onDateSet(DatePicker view, int year, int month, int day) {
        Calendar calendarDatePicker = Calendar.getInstance();
        calendarDatePicker.set(Calendar.YEAR, year);
        calendarDatePicker.set(Calendar.DAY_OF_MONTH, day);
        calendarDatePicker.set(Calendar.MONTH, month);
        Calendar calendarToday = Calendar.getInstance();
        calendarToday.set(Calendar.DAY_OF_MONTH, (calendarToday.get(Calendar.DAY_OF_MONTH) + 1));
        if (calendarDatePicker.getTime().before(calendarToday.getTime())) {
            String date = String.valueOf(day) + " " + FormatDateFactura.monthsOfYears(setMonthDateToNameOfMonth(month + 1),
                    Locale.getDefault().getDisplayLanguage()) + " " + String.valueOf(year);
            dateFormat = String.valueOf(month + 1) + "/" + String.valueOf(day) + "/" + String.valueOf(year);
            describe_etCalendar.setText(date);
        }
    }

    private String setMonthDateToNameOfMonth(int month) {
        return new DateFormatSymbols().getMonths()[month - 1];
    }

    private void loadTiposList(final List<GrupoInteres> listType, int title, final EditText editText) {
        String[] items = getItemsArrayFromItemGeneralList(listType);
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle(title);
        builder.setItems(items, (dialog, which) -> {
            itemGeneralSelected = listType.get(which);
            loadTextItemSelected(editText);
        });
        alertDialog = builder.create();
        alertDialog.show();
    }

    private String[] getItemsArrayFromItemGeneralList(List<GrupoInteres> tiposItemsList) {
        String[] items = new String[tiposItemsList.size()];
        for (int i = 0; i < tiposItemsList.size(); i++) {
            items[i] = tiposItemsList.get(i).getDescripcion();
        }
        return items;
    }

    private void loadTextItemSelected(EditText editText) {
        editText.setText(itemGeneralSelected.getDescripcion());
    }

    @Override
    public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {
        //The method is not used.
    }

    @Override
    public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
        changeBackgroudButtonSend();
    }

    private void changeBackgroudButtonSend() {
        describe_btnContinue.setBackgroundResource(validateFields() ? R.color.button_green_factura : R.color.status_bar);
        describe_btnContinue.setEnabled(validateFields());
    }

    @Override
    public void afterTextChanged(Editable editable) {
        //The method is not used.
    }

    private boolean validateFields() {
        boolean validateField;
        if (stateAnonymous) {
            validateField = validations.validateEmptyParameterBoolean(describe_etPlace.getText().toString().trim(),
                    describe_etDescription.getText().toString().trim(), describe_etCalendar.getText().toString().trim());
        } else {
            validateField = validations.validateEmptyParameterBoolean(describe_etPlace.getText().toString().trim(),
                    describe_etDescription.getText().toString().trim(), describe_etCalendar.getText().toString().trim(),
                    describe_etName.getText().toString().trim(), describe_etEmail.getText().toString().trim(),
                    describe_etPhone.getText().toString().trim(), describe_etGroup.getText().toString().trim());
        }
        if (validateField) {
            validateField = validateOptionalFields(validateField);
        }
        return validateField;
    }

    private boolean validateOptionalFields(boolean validateField) {
        if (validateField && describe_cbCompany.isChecked() && describe_cbPeople.isChecked()) {
            validateField = validations.validateEmptyParameterBoolean(describe_etCompany.getText().toString().trim(),
                    describe_etPeople.getText().toString().trim());
        } else if (validateField && !describe_cbCompany.isChecked() && describe_cbPeople.isChecked()) {
            validateField = validations.validateEmptyParameterBoolean(describe_etPeople.getText().toString().trim());
        } else if (validateField && describe_cbCompany.isChecked() && !describe_cbPeople.isChecked()) {
            validateField = validations.validateEmptyParameterBoolean(describe_etCompany.getText().toString().trim());
        }
        return validateField;
    }

    @Override
    public void onCheckedChanged(CompoundButton compoundButton, boolean isChecked) {
        if (compoundButton.getId() == describe_cbAnonymous.getId()) {
            stateAnonymous = isChecked;
            describe_cbAnonymous.setButtonDrawable(getImageStateAnonymous(isChecked));
            hideComponentsWithStateAnonymous(isChecked);
            changeBackgroudButtonSend();
        } else if (compoundButton.getId() == describe_cbPeople.getId()) {
            describe_cbPeople.setButtonDrawable(getImageStateAnonymous(isChecked));
            showEditText(isChecked, describe_cbPeople.getId());
            changeBackgroudButtonSend();
        } else if (compoundButton.getId() == describe_cbCompany.getId()) {
            describe_cbCompany.setButtonDrawable(getImageStateAnonymous(isChecked));
            showEditText(isChecked, describe_cbCompany.getId());
            changeBackgroudButtonSend();
        }
    }

    private void showEditText(boolean isChecked, int id) {
        if (id == describe_cbPeople.getId()) {
            describe_etPeople.setVisibility(isChecked ? View.VISIBLE : View.GONE);
            describe_viewLinePeople.setVisibility(isChecked ? View.GONE : View.VISIBLE);
            if (!isChecked) {
                describe_etPeople.setText("");
            }
        } else if (id == describe_cbCompany.getId()) {
            describe_etCompany.setVisibility(isChecked ? View.VISIBLE : View.GONE);
            describe_viewLineCompany.setVisibility(isChecked ? View.GONE : View.VISIBLE);
            if (!isChecked) {
                describe_etCompany.setText("");
            }
        }
    }

    private void hideComponentsWithStateAnonymous(boolean isAnonymous) {
        describe_etName.setVisibility(isAnonymous ? View.GONE : View.VISIBLE);
        describe_tvName.setVisibility(isAnonymous ? View.GONE : View.VISIBLE);
        describe_etEmail.setVisibility(isAnonymous ? View.GONE : View.VISIBLE);
        describe_tvEmail.setVisibility(isAnonymous ? View.GONE : View.VISIBLE);
        describe_etPhone.setVisibility(isAnonymous ? View.GONE : View.VISIBLE);
        describe_tvPhone.setVisibility(isAnonymous ? View.GONE : View.VISIBLE);
        describe_etGroup.setVisibility(isAnonymous ? View.GONE : View.VISIBLE);
        describe_tvGroup.setVisibility(isAnonymous ? View.GONE : View.VISIBLE);
    }

    private int getImageStateAnonymous(boolean stateAnonymous) {
        int mipmapImage;
        mipmapImage = stateAnonymous ? R.mipmap.checkbox_on : R.mipmap.checkbox_off;
        return mipmapImage;
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == Constants.ATTACHACTIVITY && resultCode == Constants.FINISH) {
            finish();
        } else if (requestCode == Constants.ATTACHACTIVITY && resultCode == Constants.ATTACH) {
            arrayAttach = data.getStringArrayListExtra(Constants.ATTACHLIST);
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    @Override
    public void onSendToChange(boolean controlDoubleTab) {
        this.controlDoubleTab = controlDoubleTab;
    }
}