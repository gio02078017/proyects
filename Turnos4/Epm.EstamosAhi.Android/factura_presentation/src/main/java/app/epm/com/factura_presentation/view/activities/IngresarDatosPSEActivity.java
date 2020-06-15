package app.epm.com.factura_presentation.view.activities;

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
import android.widget.TextView;

import com.epm.app.business_models.business_models.ItemGeneral;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.DataPagar;
import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.business_models.InformacionPSE;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.dependency_injection.DomainModule;
import app.epm.com.factura_presentation.presenters.IngresarDatosPSEPresenter;
import app.epm.com.factura_presentation.view.views_activities.IIngresarDatosPSEView;
import app.epm.com.utilities.helpers.HelperIP;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 6/01/17.
 */

public class IngresarDatosPSEActivity extends BaseActivity<IngresarDatosPSEPresenter>
        implements IIngresarDatosPSEView, TextWatcher {

    private Toolbar toolbarApp;
    private TextView ingresarDatosPse_tvEntidadBancaria;
    private TextView ingresarDatosPse_tvTipoCliente;
    private TextView ingresarDatosPse_tvTipoDocumento;
    private EditText ingresarDatosPse_etNumeroDocumento;
    private Button ingresarDatosPse_btnPagar;
    private ItemGeneral itemGeneralSelected;
    private List<ItemGeneral> personTypeList;
    private List<ItemGeneral> documentTypeList;
    private List<ItemGeneral> entityFinancialList;
    private List<FacturaResponse> facturaResponses;
    private InformacionPSE informacionPSE;
    private String nombreEntidadFinanciera;
    private int entidadFinanciera;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ingresar_datos_pse);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        setPresenter(new IngresarDatosPSEPresenter(DomainModule.getFacturaBLInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        facturaResponses = (List<FacturaResponse>) getIntent().getSerializableExtra(Constants.LISTA_MIS_FACTURAS);
        createProgressDialog();
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
        ingresarDatosPse_tvEntidadBancaria = (TextView) findViewById(R.id.ingresarDatosPse_tvEntidadBancaria);
        ingresarDatosPse_tvEntidadBancaria.addTextChangedListener(this);
        ingresarDatosPse_tvTipoCliente = (TextView) findViewById(R.id.ingresarDatosPse_tvTipoCliente);
        ingresarDatosPse_tvTipoCliente.addTextChangedListener(this);
        ingresarDatosPse_tvTipoDocumento = (TextView) findViewById(R.id.ingresarDatosPse_tvTipoDocumento);
        ingresarDatosPse_tvTipoDocumento.addTextChangedListener(this);
        ingresarDatosPse_etNumeroDocumento = (EditText) findViewById(R.id.ingresarDatosPse_etNumeroDocumento);
        ingresarDatosPse_etNumeroDocumento.addTextChangedListener(this);
        ingresarDatosPse_btnPagar = (Button) findViewById(R.id.ingresarDatosPse_btnPagar);
        loadToolbar();
        loadListenerToTheControl();
        loadList();
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
     * Permite cargar la lista
     *
     * @param listType tipo de lista
     * @param title    titulo de la lista
     * @param textView textview donde va a cargar la lista
     */
    private void loadTiposList(String listType, int title, final TextView textView) {
        final List<ItemGeneral> list = getCustomSharedPreferences().getItemGeneralList(listType);
        String[] items = getItemsArrayFromItemGeneralList(list);
        AlertDialog alertDialog;
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle(title);
        builder.setItems(items, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                itemGeneralSelected = list.get(which);
                loadTextItemSelected(textView);
            }
        });
        alertDialog = builder.create();
        alertDialog.show();
    }

    /**
     * Carga los Items con la descripción de la lista seleccionada.
     */
    private void loadTextItemSelected(TextView textView) {
        textView.setText(itemGeneralSelected.getDescripcion());
    }

    /**
     * Obtiene la descripción de cada uno de los documentos.
     *
     * @param tiposItemsList tipo de items de la lista
     * @return La descripción de los documentos.
     */
    private String[] getItemsArrayFromItemGeneralList(List<ItemGeneral> tiposItemsList) {
        String[] items = new String[tiposItemsList.size()];
        for (int i = 0; i < tiposItemsList.size(); i++) {
            items[i] = tiposItemsList.get(i).getDescripcion();
        }
        return items;
    }

    /**
     * Permite obtener el id a traves de la descripcion
     *
     * @param text String de la lista
     * @param list tipo de lista
     * @return id asociado a la descripcion
     */
    private int getIdFromListByString(String text, List<ItemGeneral> list) {
        int id = 0;
        for (ItemGeneral item : list) {
            if (item.getDescripcion().equals(text)) {
                return item.getId();
            }
        }
        return id;
    }

    /**
     * Obtiene las facturas para pagar.
     *
     * @return facturaResponseList.
     */
    private List<FacturaResponse> getFacturasToPagar() {
        final List<FacturaResponse> facturaResponseList = new ArrayList<>();
        for (int i = 0; i < facturaResponses.size(); i++) {
            if (facturaResponses.get(i).isEstaSeleccionadaParaPago()) {
                facturaResponseList.add(facturaResponses.get(i));
            }
        }
        return facturaResponseList;
    }

    /**
     * Carga las listas de la actividad.
     */
    private void loadList() {
        documentTypeList = getCustomSharedPreferences().getItemGeneralList(Constants.TIPOS_DOCUMENTO_LIST);
        personTypeList = getCustomSharedPreferences().getItemGeneralList(Constants.TIPOS_PERSONA_LIST);
        entityFinancialList = getCustomSharedPreferences().getItemGeneralList(Constants.ENTITYFINANCIAL);
    }

    /**
     * Carga los Listener de los controles.
     */
    private void loadListenerToTheControl() {
        loadOnClickListenerToTheControlIngresarDatosPseEntidadBancaria();
        loadOnClickListenerToTheControlIngresarDatosPseTipoCliente();
        loadOnClickListenerToTheControlIngresarDatosPseTipoDocumento();
        loadOnClickListenerToTheControlIngresarDatosPsePagar();
    }

    /**
     * Carga OnClick al control ingresarDatosPse_tvEntidadBancaria.
     */
    private void loadOnClickListenerToTheControlIngresarDatosPseEntidadBancaria() {
        ingresarDatosPse_tvEntidadBancaria.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                loadTiposList(Constants.ENTITYFINANCIAL, R.string.title_entity_financial, ingresarDatosPse_tvEntidadBancaria);
            }
        });
    }

    /**
     * Carga OnClick al control ingresarDatosPse_tvTipoCliente.
     */
    private void loadOnClickListenerToTheControlIngresarDatosPseTipoCliente() {
        ingresarDatosPse_tvTipoCliente.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                loadTiposList(Constants.TIPOS_PERSONA_LIST, R.string.text_tipo_de_persona_list, ingresarDatosPse_tvTipoCliente);
            }
        });
    }

    /**
     * Carga OnClick al control ingresarDatosPse_tvTipoDocumento.
     */
    private void loadOnClickListenerToTheControlIngresarDatosPseTipoDocumento() {
        ingresarDatosPse_tvTipoDocumento.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                loadTiposList(Constants.TIPOS_DOCUMENTO_LIST, R.string.text_tipo_documento_list, ingresarDatosPse_tvTipoDocumento);
            }
        });
    }

    /**
     * Carga OnCLick al control ingresarDatosPse_btnPagar.
     */
    private void loadOnClickListenerToTheControlIngresarDatosPsePagar() {
        ingresarDatosPse_btnPagar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showAlertDialogMessagePSEOnUiThread();
            }
        });
    }

    /**
     * Valida si los campos tienen texto y cambia el estado del boton ingresarDatosPse_btnPagar.
     */
    private void changeStatusButtonPagar() {
        if (ingresarDatosPse_tvEntidadBancaria.getText().toString().trim().isEmpty() ||
                ingresarDatosPse_tvTipoCliente.getText().toString().trim().isEmpty() ||
                ingresarDatosPse_tvTipoDocumento.getText().toString().trim().isEmpty() ||
                ingresarDatosPse_etNumeroDocumento.getText().toString().trim().isEmpty()) {
            ingresarDatosPse_btnPagar.setEnabled(false);
            ingresarDatosPse_btnPagar.setBackgroundResource(R.color.status_bar);
        } else {
            ingresarDatosPse_btnPagar.setEnabled(true);
            ingresarDatosPse_btnPagar.setBackgroundResource(R.color.button_green_factura);
        }
    }

    /**
     * Prepara los datos para ir a PSE.
     */
    private void prepareDateToPSE() {
        DataPagar dataPagar = new DataPagar();
        entidadFinanciera = getIdFromListByString(ingresarDatosPse_tvEntidadBancaria.getText().toString(), entityFinancialList);
        dataPagar.setEntidadFinanciera(entidadFinanciera);
        dataPagar.setIdTipoPersona(getIdFromListByString(ingresarDatosPse_tvTipoCliente.getText().toString(), personTypeList));
        dataPagar.setIdTipoDocumento(getIdFromListByString(ingresarDatosPse_tvTipoDocumento.getText().toString(), documentTypeList));
        dataPagar.setNumeroDocumento(ingresarDatosPse_etNumeroDocumento.getText().toString().trim());
        dataPagar.setFacturasPagar(getFacturasToPagar());
        dataPagar.setDireccionIp(HelperIP.getIPAddress(true));
        getPresenter().validateInternetDatosPagar(dataPagar);
    }

    /**
     * Crea alerta que se muestra antes de ir a PSE en un nuevo hilo.
     */
    public void showAlertDialogMessagePSEOnUiThread() {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(R.string.title_alert_ir_a_pse, R.string.text_alert_ir_a_pse,
                false, R.string.text_aceptar, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        prepareDateToPSE();
                    }
                }, R.string.text_cancelar, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        dialogInterface.dismiss();
                    }
                }, null));

    }

    @Override
    public void saveInformacionPSE(InformacionPSE informacionPSE) {
        this.informacionPSE = informacionPSE;
        nombreEntidadFinanciera = ingresarDatosPse_tvEntidadBancaria.getText().toString();
    }

    @Override
    public void startPagePSE(List<FacturaResponse> facturasPagar) {
        Intent intent = new Intent(this, PagePSEActivity.class);
        intent.putExtra(Constants.LISTA_MIS_FACTURAS_POR_PAGO, (Serializable) facturasPagar);
        intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) facturaResponses);
        intent.putExtra(Constants.INFORMACION_PSE, this.informacionPSE);
        intent.putExtra(Constants.ID_ENTIDAD_FINANCIERA, entidadFinanciera);
        intent.putExtra(Constants.NOMBRE_ENTIDAD_FINANCIERA, nombreEntidadFinanciera);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    @Override
    public void beforeTextChanged(CharSequence s, int start, int count, int after) {
        //The method is not used.
    }

    @Override
    public void onTextChanged(CharSequence s, int start, int before, int count) {
        changeStatusButtonPagar();
    }

    @Override
    public void afterTextChanged(Editable editable) {
        //The method is not used.
    }
}