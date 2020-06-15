package app.epm.com.factura_presentation.view.activities;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.InputFilter;
import android.text.TextWatcher;
import android.view.View;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.dependency_injection.DomainModule;
import app.epm.com.factura_presentation.presenters.ConsultFacturaPresenter;
import app.epm.com.factura_presentation.view.views_activities.IConsultFacturaView;

import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 12/12/16.
 */

public class ConsultFacturaActivity extends BaseActivity<ConsultFacturaPresenter> implements IConsultFacturaView, TextWatcher {

    private Validations validation;
    private Toolbar toolbarApp;
    private CheckBox consultFactura_cbNumeroContrato;
    private CheckBox consultFactura_cbReferentePago;
    private EditText consultFactura_etNumeroFactura;
    private Button consultFactura_btnConsultarFactura;
    private List<FacturaResponse> listaMisFacturas;
    private boolean landing;
    private float sizeEditText;
    private float sizeEditTextBig;
    private int position;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_consult_factura);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        setPresenter(new ConsultFacturaPresenter(DomainModule.getFacturaBLInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_HIDDEN);
        validation = new Validations();
        getIntentLAnding();
        loadViews();
        createProgressDialog();
        openAlertRateApp();
    }

    private void openAlertRateApp() {
        boolean resultCode;
        resultCode = getIntent().getBooleanExtra(Constants.SHOW_ALERT_RATE, false);
        if (resultCode) {
            validateShowAlertQualifyApp();
        }
    }

    @Override
    public void manageActionFactura() {
        //The method is not used.
    }

    private void getIntentLAnding() {
        Intent intent = getIntent();
        landing = intent.getBooleanExtra(Constants.TRUE, false);
    }

    @Override
    public void beforeTextChanged(CharSequence s, int start, int count, int after) {
        //The method is not used.
    }

    @Override
    public void onTextChanged(CharSequence s, int start, int before, int count) {
        changeStateButtonConsultFactura();
    }

    @Override
    public void afterTextChanged(Editable s) {
        //The method is not used.
    }

    @Override
    protected void onRestart() {
        super.onRestart();
    }

    /**
     * Habilitar o deshabilitar el botón que consulta factura.
     */
    private void changeStateButtonConsultFactura() {
        if (consultFactura_etNumeroFactura.getText().toString().isEmpty()) {
            consultFactura_btnConsultarFactura.setEnabled(false);
            consultFactura_btnConsultarFactura.setBackgroundResource(R.color.status_bar);
            setSizeFontNumeroFactura(false);
        } else {
            consultFactura_btnConsultarFactura.setEnabled(true);
            consultFactura_btnConsultarFactura.setBackgroundResource(R.color.button_green_factura);
            setSizeFontNumeroFactura(true);
        }
    }

    /**
     * Permite cargar las vistas del xml.
     */
    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        consultFactura_cbNumeroContrato = (CheckBox) findViewById(R.id.consultFactura_cbNumeroContrato);
        consultFactura_cbReferentePago = (CheckBox) findViewById(R.id.consultFactura_cbReferentePago);
        consultFactura_etNumeroFactura = (EditText) findViewById(R.id.consultFactura_etNumeroFactura);
        consultFactura_btnConsultarFactura = (Button) findViewById(R.id.consultFactura_btnConsultarAhora);
        setMaxLengNumeroFactura(8);
        calculateSizeEdiText();
        loadToolbar();
        loadListenerToTheControl();
    }

    /**
     * Calcula el tamaño de la fuente del editext en SP.
     */
    private void calculateSizeEdiText() {
        final float scaledDensity = getResources().getDisplayMetrics().scaledDensity;
        final float sizeSP = consultFactura_etNumeroFactura.getTextSize() / scaledDensity;
        sizeEditText = sizeSP;
        sizeEditTextBig = sizeSP + Constants.THREE;
    }

    /**
     * Cargar los eventos a los controles.
     */
    private void loadListenerToTheControl() {
        consultFactura_cbNumeroContrato.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                verifyOptionsQueryContrato();
            }
        });

        consultFactura_cbReferentePago.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                verifyOptionsQueryReferentePago();
            }
        });

        consultFactura_btnConsultarFactura.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                consultFactura();
            }
        });

        consultFactura_etNumeroFactura.addTextChangedListener(this);
    }

    /**
     * Consulta la factura por referente de pago o por contrato.
     */
    private void consultFactura() {
        String number = consultFactura_etNumeroFactura.getText().toString().trim();
        if (consultFactura_cbReferentePago.isChecked()) {
            getPresenter().consulFacturaPorReferenteDePago(number);
        } else {
            getPresenter().consultFacturaPorContrato(number);
        }
    }

    /**
     * Verifica si la factura ya existe en la lista Mis Facturas en caso de no existir la agrega.
     *
     * @param facturas
     */
    private void verifyExistFacturaListaMisFacturas(List<FacturaResponse> facturas) {
        position = 0;
        boolean exist = false;
        if (listaMisFacturas == null) {
            listaMisFacturas = new ArrayList<>();
        }
        for (FacturaResponse fact : facturas) {
            exist = false;
            for (FacturaResponse misFact : listaMisFacturas)
                if (fact.getDocumentoReferencia().equals(misFact.getDocumentoReferencia())) {
                    position = listaMisFacturas.indexOf(misFact);
                    exist = true;
                }
            if (!exist) {
                fact.setEstaInscrita(FacturaResponse.EstaInscrita.PARAINSCRIBIR);
                listaMisFacturas.add(0, fact);
            }
        }
    }

    /**
     * Asigna comportamiento cuando selecciona el contrato.
     */
    private void verifyOptionsQueryContrato() {
        if (consultFactura_cbNumeroContrato.isChecked()) {
            consultFactura_cbReferentePago.setChecked(false);
        } else {
            consultFactura_cbReferentePago.setChecked(true);
        }
        consultFactura_etNumeroFactura.setText(Constants.EMPTY_STRING);
        setSizeFontNumeroFactura(false);
        setMaxLengNumeroFactura(8);
    }

    /**
     * Asigna comportamiento cuando selecciona referente de pago.
     */
    private void verifyOptionsQueryReferentePago() {
        if (consultFactura_cbReferentePago.isChecked()) {
            consultFactura_cbNumeroContrato.setChecked(false);
        } else {
            consultFactura_cbNumeroContrato.setChecked(true);
        }
        consultFactura_etNumeroFactura.setText(Constants.EMPTY_STRING);
        setSizeFontNumeroFactura(false);
        setMaxLengNumeroFactura(12);
    }

    /**
     * Asigna la longitud al campo Numero Factura.
     *
     * @param maxLength
     */
    private void setMaxLengNumeroFactura(int maxLength) {
        InputFilter[] filterArray = new InputFilter[1];
        filterArray[0] = new InputFilter.LengthFilter(maxLength);
        consultFactura_etNumeroFactura.setFilters(filterArray);
    }

    /**
     * Cambia el tamaño de la fuente del editext.
     *
     * @param increase
     */
    private void setSizeFontNumeroFactura(boolean increase) {
        if (increase) {
            consultFactura_etNumeroFactura.setTextSize(sizeEditTextBig);
        } else {
            consultFactura_etNumeroFactura.setTextSize(sizeEditText);
        }
    }

    /**
     * Permite cargar el toolbar con el título y el botón de navegación.
     */
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

    /**
     * Limpia los campos del layout.
     */
    @Override
    public void cleanFieldsTheConsulFactura() {
        runOnUiThread(() -> startControls());
    }

    /**
     * Concatena el titulo del mensaje cuando la factura no existe.
     *
     * @param textoUno
     * @param number
     * @param textoDos
     * @return
     */
    @Override
    public String getTitleFacturaNotExist(int textoUno, String number, int textoDos) {
        return String.format("%s %s %s", getString(textoUno), number, getString(textoDos));
    }

    /**
     * Inicializa la Activity FacturasConsultadasActivity.
     *
     * @param facturaResponse
     */
    @Override
    public void startFacturasConsultadasActivity(List<FacturaResponse> facturaResponse) {
        getListaMisFacturas();
        verifyExistFacturaListaMisFacturas(facturaResponse);
        if (landing) {
            Intent intent = new Intent(this, FacturasConsultadasActivity.class);
            intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) listaMisFacturas);
            intent.putExtra(Constants.POSICION_LISTA_MIS_FACTURAS, position);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        } else {
            goToFactura();
        }
    }

    /**
     * Inicializa  el EditText y el button.
     */
    private void startControls() {
        consultFactura_etNumeroFactura.setText(Constants.EMPTY_STRING);
        consultFactura_btnConsultarFactura.setEnabled(false);
        consultFactura_btnConsultarFactura.setBackgroundResource(R.color.status_bar);
    }

    /**
     * Obtiene la Lista Mis facturas.
     */
    private void getListaMisFacturas() {
        listaMisFacturas = (List<FacturaResponse>) getIntent().getExtras().getSerializable(Constants.LISTA_MIS_FACTURAS);
    }

    /**
     * Permite ir a
     */
    private void goToFactura() {
        Intent intent = new Intent();
        intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) listaMisFacturas);
        intent.putExtra(Constants.POSICION_LISTA_MIS_FACTURAS, position);
        intent.putExtra(Constants.USUARIO, getUsuario());
        setResult(Constants.FACTURA, intent);
        finish();
    }
}