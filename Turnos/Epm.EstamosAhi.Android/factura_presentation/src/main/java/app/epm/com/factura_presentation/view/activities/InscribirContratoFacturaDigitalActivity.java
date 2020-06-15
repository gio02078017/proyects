package app.epm.com.factura_presentation.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import androidx.appcompat.widget.Toolbar;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;

import com.epm.app.business_models.business_models.Usuario;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.DataContratos;
import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.business_models.GestionContrato;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.dependency_injection.DomainModule;
import app.epm.com.factura_presentation.presenters.InscribirContratoFacturaDigitalPresenter;
import app.epm.com.factura_presentation.view.views_activities.IInscribirContratoFacturaDigitalView;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 20/12/16.
 */

public class InscribirContratoFacturaDigitalActivity extends BaseActivity<InscribirContratoFacturaDigitalPresenter>
        implements IInscribirContratoFacturaDigitalView {

    private Toolbar toolbarApp;
    private CheckBox inscribirContratoFacturaDigital_cbFacturaDigital;
    private Button inscribirContratoFacturaDigital_btnInscribir;
    private String nameContrato;
    private String numberContrato;
    private boolean isCheckFacturaDigital;
    private Usuario user = new Usuario();
    private List<FacturaResponse> facturaResponsesInscritas;
    private List<FacturaResponse> facturaResponses;
    private int position;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_inscribir_contrato_factura_digital);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        setPresenter(new InscribirContratoFacturaDigitalPresenter(DomainModule.getFacturaBLInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        createProgressDialog();
        loadViews();
        nameContrato = getIntent().getStringExtra(Constants.NAME_CONTRATO);
        numberContrato = getIntent().getStringExtra(Constants.NUMBER_FACTURA);
        user = (Usuario) getIntent().getSerializableExtra(Constants.USUARIO);
        facturaResponsesInscritas = (List<FacturaResponse>) getIntent().getSerializableExtra(Constants.LISTA_MIS_FACTURAS);
        facturaResponses = (List<FacturaResponse>) getIntent().getSerializableExtra(Constants.LISTA_MIS_FACTURAS);
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
        inscribirContratoFacturaDigital_cbFacturaDigital = (CheckBox) findViewById(R.id.inscribirContratoFacturaDigital_cbFacturaDigital);
        inscribirContratoFacturaDigital_btnInscribir = (Button) findViewById(R.id.inscribirContratoFacturaDigital_btnInscribir);
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
        loadOnCheckBoxChangedFacturaDigital();
        loadOnClickListenerToTheControlFacturaDigitalBtnInscribir();
    }

    /**
     * Carga OnCheck al control inscribirContratoFacturaDigital_cbFacturaDigital.
     */
    private void loadOnCheckBoxChangedFacturaDigital() {
        inscribirContratoFacturaDigital_cbFacturaDigital.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton compoundButton, boolean isCheck) {
                isCheckFacturaDigital = isCheck;
            }
        });
    }

    /**
     * Carga OnClick al control inscribirContratoFacturaDigital_btnInscribir.
     */
    private void loadOnClickListenerToTheControlFacturaDigitalBtnInscribir() {
        inscribirContratoFacturaDigital_btnInscribir.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                prepareDateToInscribirContrato();
            }
        });
    }

    private List<FacturaResponse> sendFacturasInscritas() {
        position = 0;
        List<FacturaResponse> facturaResponseList = new ArrayList<>();
        for (int i = 0; i < facturaResponsesInscritas.size(); i++) {
            if (facturaResponsesInscritas.get(i).getNumeroContrato().equals(numberContrato)) {
                facturaResponsesInscritas.get(i).setDescripcionContrato(nameContrato);
                facturaResponsesInscritas.get(i).setEstaInscrita(FacturaResponse.EstaInscrita.INSCRITA);
                position = facturaResponsesInscritas.indexOf(facturaResponsesInscritas.get(i));
                facturaResponseList.add(facturaResponsesInscritas.get(i));
            }
        }
        return facturaResponseList;
    }

    /**
     * Prepara los datos para inscribir contrato.
     */
    private void prepareDateToInscribirContrato() {
        ArrayList<DataContratos> dataContratosList = new ArrayList<>();
        GestionContrato inscribirContrato = new GestionContrato();
        DataContratos dataContratos = new DataContratos();
        dataContratos.setDescripcion(nameContrato);
        dataContratos.setRecibirFacturaDigital(isCheckFacturaDigital);
        dataContratos.setNumero(numberContrato);
        dataContratos.setOperacion(Constants.CODIGO_OPERACION);
        dataContratosList.add(dataContratos);
        inscribirContrato.setCorreoElectronico(user.getCorreoElectronico());
        inscribirContrato.setContratos(dataContratosList);
        getPresenter().validateInternetInscribirContratoFacturaDigital(inscribirContrato);
    }

    @Override
    public void showAlertDialogToShowMessageFacturaInscritaSuccesOnUiThread(final int title) {
        runOnUiThread(() -> showAlertDialogToShowMessageFacturaInscritaSucces(title));
    }

    /**
     * Abre alerta cuando el contrato se inscribe correctamente
     *
     * @param title title.
     * @param text  text.
     */
    private void showAlertDialogToShowMessageFacturaInscritaSucces(int title) {
        getCustomAlertDialog().showAlertDialog(getUsuario().getNombres(), R.string.text_contrato_inscrito,
                false, R.string.text_ir_a_mis_facturas, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                startFacturasConsultadas();
            }
        }, null);

    }

    /**
     * Envia a la actividad de Facturas consultadas.
     */
    private void startFacturasConsultadas() {
        Intent intent = new Intent(this, FacturasConsultadasActivity.class);
        intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) sendFacturasInscritas());
        intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) facturaResponses);
        intent.putExtra(Constants.POSICION_LISTA_MIS_FACTURAS, position);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivityWithOutDoubleClick(intent);
    }
}