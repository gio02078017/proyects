package app.epm.com.factura_presentation.view.activities;

import android.content.Intent;
import android.os.Bundle;
import androidx.appcompat.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import java.io.Serializable;
import java.util.List;

import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.factura_presentation.R;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 19/12/16.
 */

public class InscribirContratoActivity extends BaseActivity implements TextWatcher {

    private Toolbar toolbarApp;
    private TextView inscribirContrato_tvNumero;
    private EditText inscribirContrato_etNombre;
    private Button inscribirContrato_btnContinuar;
    private String numberContrato;
    private List<FacturaResponse> facturaResponses;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_inscribir_contrato);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        numberContrato = getIntent().getStringExtra(Constants.NUMBER_FACTURA);
        facturaResponses = (List<FacturaResponse>) getIntent().getSerializableExtra(Constants.LISTA_MIS_FACTURAS);
        loadViews();
    }

    @Override
    public void manageActionFactura() {
        //The method is not used.
    }

    /**
     * Carga los controles de la vista.
     */
    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        inscribirContrato_tvNumero = (TextView) findViewById(R.id.inscribirContrato_tvNumero);
        inscribirContrato_etNombre = (EditText) findViewById(R.id.inscribirContrato_etNombre);
        inscribirContrato_etNombre.addTextChangedListener(this);
        inscribirContrato_btnContinuar = (Button) findViewById(R.id.inscribirContrato_btnContinuar);
        inscribirContrato_btnContinuar.addTextChangedListener(this);
        loadToolbar();
        loadListenerToTheControl();
    }

    /**
     * Carga el Toolbar.
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
     * Carga los Listener de los controles.
     */
    private void loadListenerToTheControl() {
        loadOnClickListenerToTheControlInscribirContratoBtnContinuar();
        loadNumberContratoTvNumero();
    }

    /**
     * Carga el número del contrato.
     */
    private void loadNumberContratoTvNumero() {
        inscribirContrato_tvNumero.setText(numberContrato);
    }

    /**
     * Cambia el estado del boton inscribirContrato_btnContinuar.
     */
    private void changeStateButtonInscribirContrato() {
        if (inscribirContrato_etNombre.getText().toString().trim().isEmpty()) {
            inscribirContrato_btnContinuar.setEnabled(false);
            inscribirContrato_btnContinuar.setBackgroundResource(R.color.status_bar);
        } else {
            inscribirContrato_btnContinuar.setEnabled(true);
            inscribirContrato_btnContinuar.setBackgroundResource(R.color.button_green_factura);
        }
    }

    /**
     * Carga OnClick al control inscribirContrato_btnContinuar.
     */
    private void loadOnClickListenerToTheControlInscribirContratoBtnContinuar() {
        inscribirContrato_btnContinuar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startInscribirContratoFacturaDigital();
            }
        });
    }

    private void startInscribirContratoFacturaDigital() {
        Intent intent = new Intent(this, InscribirContratoFacturaDigitalActivity.class);
        intent.putExtra(Constants.NAME_CONTRATO, inscribirContrato_etNombre.getText().toString().trim());
        intent.putExtra(Constants.NUMBER_FACTURA, numberContrato);
        intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) facturaResponses);
        startActivityWithOutDoubleClick(intent);
    }

    @Override
    public void beforeTextChanged(CharSequence s, int start, int count, int after) {
        //The method is not used.
    }

    @Override
    public void onTextChanged(CharSequence s, int start, int before, int count) {
        changeStateButtonInscribirContrato();
    }

    @Override
    public void afterTextChanged(Editable editable) {
        //The method is not used.
    }
}