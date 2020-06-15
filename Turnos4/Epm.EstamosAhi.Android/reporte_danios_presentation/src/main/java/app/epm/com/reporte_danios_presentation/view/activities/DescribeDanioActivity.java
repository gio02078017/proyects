package app.epm.com.reporte_danios_presentation.view.activities;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import com.epm.app.app_utilities_presentation.utils.LoadIconServices;
import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import com.epm.app.business_models.business_models.ItemGeneral;

import app.epm.com.reporte_danios_domain.danios.business_models.ReportDanio;
import app.epm.com.reporte_danios_presentation.R;
import app.epm.com.utilities.controls.CustomTextViewNormal;
import app.epm.com.utilities.helpers.TypeDanioOrFraude;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by leidycarolinazuluagabastidas on 7/02/17.
 */

public class DescribeDanioActivity extends BaseActivity implements TextWatcher {

    private Toolbar toolbarApp;
    private TextView describe_etTipoDanio;
    private EditText describe_etDescripcion;
    private EditText describe_etLugar;
    private EditText describe_etTelefono;
    private EditText describe_etNombre;
    private ImageView describe_ivServicio;
    private Button describe_btnContinue;
    private ReportDanio reportDanio;
    private TypeDanioOrFraude itemGeneralSelected;
    private List<TypeDanioOrFraude> dataList = new ArrayList<>();
    private ETipoServicio tipoServicio;
    private InformacionDeUbicacion informacionDeUbicacion;
    private String address;
    private Validations validations;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_describe_danio);
        loadDrawerLayout(R.id.generalDrawerLayout);
        createProgressDialog();
        getDataIntent();
        loadViews();
        loadIconServices();
        validations = new Validations();

    }

    private void loadIconServices() {
        LoadIconServices loadIconServices = new LoadIconServices();
        describe_ivServicio.setImageResource(loadIconServices.setIdIconoServicio(this.tipoServicio.getName()));
    }


    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == Constants.ATTACH) {
            this.reportDanio = (ReportDanio) data.getSerializableExtra(Constants.REPORT_DANIOS);
        }

        if (resultCode == Constants.SERVICES_DANIO) {
            setResult(Constants.SERVICES_DANIO);
            finish();
        }

        super.onActivityResult(requestCode, resultCode, data);
    }

    private void loadViews() {
        toolbarApp =  findViewById(R.id.toolbar);
        describe_etTipoDanio =findViewById(R.id.describe_etTipoDanio);
        describe_etTipoDanio.addTextChangedListener(this);
        describe_etDescripcion =  findViewById(R.id.describe_etDescripcion);
        describe_etDescripcion.addTextChangedListener(this);
        describe_etLugar =  findViewById(R.id.describe_etLugar);
        describe_etLugar.addTextChangedListener(this);
        describe_etTelefono =  findViewById(R.id.describe_etTelefono);
        describe_etTelefono.addTextChangedListener(this);
        describe_etNombre =  findViewById(R.id.describe_etNombre);
        describe_etNombre.addTextChangedListener(this);
        describe_ivServicio =  findViewById(R.id.describe_ivServicio);
        describe_btnContinue = findViewById(R.id.describe_btnContinue);
        describe_btnContinue.addTextChangedListener(this);
        loadToolbar();
        loadInfo();
        loadListener();
    }

    private void loadListener() {
        loadListenerToTheControlDescribeEtTipoDanio();
        loadListenerToTheControlDescribeBtnContinue();

    }

    private void loadListenerToTheControlDescribeBtnContinue() {
        describe_btnContinue.setOnClickListener(v -> {
            if (validateFields()) {
                continuePhoto();
            }
        });
    }

    private void loadListenerToTheControlDescribeEtTipoDanio() {
        describe_etTipoDanio.setOnClickListener(v -> loadTiposList(dataList, R.string.title_typy_damage, describe_etTipoDanio));
    }

    private void loadInfo() {
        describe_etLugar.setText(address);

        if (this.reportDanio != null && this.reportDanio.getAddress() != null) {
            describe_etTipoDanio.setText(Constants.EMPTY_STRING);
            describe_etDescripcion.setText(reportDanio.getDescribeReport());
            describe_etTelefono.setText(reportDanio.getTelephoneUserReport());
            describe_etNombre.setText(reportDanio.getUserName());
        } else {
            if (getUsuario() != null && !getUsuario().isInvitado()) {
                describe_etNombre.setText(getUsuario().getNombres() + " " + getUsuario().getApellido());

                if (!getUsuario().getCelular().isEmpty()) {
                    describe_etTelefono.setText(getUsuario().getCelular());
                } else {
                    describe_etTelefono.setText(getUsuario().getTelefono());
                }
            }
        }

        setFocusControls();
    }

    private void setFocusControls(){
        describe_etDescripcion.setSelection(describe_etDescripcion.getText().length());
        describe_etLugar.setSelection(describe_etLugar.getText().length());
        describe_etTelefono.setSelection(describe_etTelefono.getText().length());
        describe_etNombre.setSelection(describe_etNombre.getText().length());
    }

    private void getDataIntent() {
        Intent intent = getIntent();
        tipoServicio = (ETipoServicio) intent.getSerializableExtra(Constants.TIPO_SERVICIO);
        dataList = (List<TypeDanioOrFraude>) intent.getSerializableExtra(Constants.LIST_TYPE_DANIO);
        informacionDeUbicacion = (InformacionDeUbicacion) intent.getSerializableExtra(Constants.INFORMACION_UBICACION);
        address = intent.getStringExtra(Constants.ADDRESS);
        reportDanio = (ReportDanio) intent.getSerializableExtra(Constants.REPORT_DANIOS);
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(view -> onBackPressed());
    }


    private boolean validateFields() {
        String phone = describe_etTelefono.getText().toString();
        final Pattern pattern = Pattern.compile("\\s");
        final Matcher matcher = pattern.matcher(phone);
        boolean found = matcher.find();
        boolean numeric = isNumeric(phone);
        if (found || !numeric || phone.length() < Constants.MIN_LENGTH_PHONE) {
            showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.number_phone_invalid_report);
            return false;
        } else {
            return true;
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
    }

    @Override
    public void customOnBackPressed() {
        setReportDanio();
        Intent intent = new Intent();
        intent.putExtra(Constants.REPORT_DANIOS, this.reportDanio);
        getDefaultIntent(intent);
        setResult(Constants.DESCRIBE_DANIO, intent);
        finish();
    }

    /**
     * Permite cargar la lista
     * @param listType tipo de lista
     * @param title    titulo de la lista
     * @param editText editText donde va a cargar la lista
     */
    private void loadTiposList(final List<TypeDanioOrFraude> listType, int title, final TextView editText) {
        AlertDialog alertDialog;
        final String[] items = getItemsArrayFromItemGeneralList(listType);
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle(title);
        builder.setItems(items, (dialog, which) -> {
            itemGeneralSelected = listType.get(which);
            loadTextItemSelected(editText);
        });
        alertDialog = builder.create();
        alertDialog.show();
    }


    private String[] getItemsArrayFromItemGeneralList(List<TypeDanioOrFraude> tiposItemsList) {
        String[] items = new String[tiposItemsList.size()];
        for (int i = 0; i < tiposItemsList.size(); i++) {
            items[i] = tiposItemsList.get(i).getNameType();
        }
        return items;
    }

    /**
     * Carga los Items con la descripción de la lista seleccionada.
     */
    private void loadTextItemSelected(TextView editText) {
        editText.setText(itemGeneralSelected.getNameType());
    }

    @Override
    public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {
        //The method is not used.
    }

    @Override
    public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
        if (describe_etTipoDanio.getText().toString().trim().isEmpty() || describe_etDescripcion.getText().toString().trim().isEmpty()
                || describe_etLugar.getText().toString().trim().isEmpty() || describe_etTelefono.getText().toString().trim().isEmpty()
                || describe_etNombre.getText().toString().trim().isEmpty()) {
            describe_btnContinue.setEnabled(false);
            describe_btnContinue.setBackgroundResource(R.color.status_bar);
        } else {
            describe_btnContinue.setEnabled(true);
            describe_btnContinue.setBackgroundResource(R.color.button_green_factura);

        }
    }

    @Override
    public void afterTextChanged(Editable editable) {
        //The method is not used.
    }

    private boolean isNumeric(String string) throws IllegalArgumentException {
        boolean isnumeric = false;

        if (string != null && !string.equals(Constants.EMPTY_STRING)) {
            isnumeric = true;
            char chars[] = string.toCharArray();
            for (int num = 0; num < chars.length; num++) {
                isnumeric &= Character.isDigit(chars[num]);
                if (!isnumeric) {
                    break;
                }
            }
        }
        return isnumeric;
    }

    private void continuePhoto() {
        setReportDanio();
        Intent intent = new Intent(this, AdjuntarFotoDanioActivity.class);
        intent.putExtra(Constants.REPORT_DANIOS, reportDanio);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }


    private void setReportDanio() {
        if (reportDanio == null) {
            reportDanio = new ReportDanio();
        }
        reportDanio.setAddress(informacionDeUbicacion.getDireccion());
        reportDanio.setTypeReporte(describe_etTipoDanio.getText().toString());
        reportDanio.setIdTypeReporte(validations.getIdFromListByString(describe_etTipoDanio.getText().toString(), dataList));
        reportDanio.setDescribeReport(describe_etDescripcion.getText().toString().trim());
        reportDanio.setLugarReferencia(describe_etLugar.getText().toString().trim());
        reportDanio.setTelephoneUserReport(describe_etTelefono.getText().toString().trim());
        reportDanio.setUserName(describe_etNombre.getText().toString().trim());
        if (!getUsuario().isInvitado()){
            reportDanio.setUserEmail(getUsuario().getCorreoElectronico());
        }
        reportDanio.setTipoServicio(tipoServicio);
    }
}
