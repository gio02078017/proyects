package app.epm.com.factura_presentation.view.activities;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Button;
import android.widget.ListView;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.DataContratos;
import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.business_models.GestionContrato;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.dependency_injection.DomainModule;
import app.epm.com.factura_presentation.presenters.GestionarContratosPresenter;
import app.epm.com.factura_presentation.view.adapters.ContratoAdapter;
import app.epm.com.factura_presentation.view.views_activities.IGestionarContratosView;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 20/12/16.
 */

public class GestionarContratosActivity extends BaseActivity<GestionarContratosPresenter> implements IGestionarContratosView {

    private Toolbar toolbarApp;
    private ListView gestionarContratos_lvFacturas;
    private Button gestionarContratos_btnGuardarCambios;
    private ContratoAdapter contratoAdapter;
    private List<FacturaResponse> listaMisFacturas;
    private ICustomSharedPreferences customSharedPreferences;
    private AlertDialog.Builder alertDialogBuilder;
    private List<DataContratos> datosContratosList;
    private GestionContrato gestionContrato;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_gestionar_contratos);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        this.customSharedPreferences = new CustomSharedPreferences(this);
        setPresenter(new GestionarContratosPresenter(DomainModule.getFacturaBLInstance(this.customSharedPreferences)));
        gestionContrato = new GestionContrato();
        getPresenter().inject(this, getValidateInternet());
        alertDialogBuilder = new AlertDialog.Builder(this);
        loadViews();
        ConsultConstratosInscritos();
    }

    @Override
    public void manageActionFactura() {
        //The method is not used.
    }

    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        gestionarContratos_lvFacturas = (ListView) findViewById(R.id.gestionarContratos_lvFacturas);
        gestionarContratos_btnGuardarCambios = (Button) findViewById(R.id.gestionarContratos_btnGuardarCambios);
        loadToolbar();
        loadListener();
    }

    private void loadListener() {
        gestionarContratos_btnGuardarCambios.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                datosContratosList = contratoAdapter.getDataContratosList();
                String contrato = validateEmptyDescription(datosContratosList);
                if (!contrato.isEmpty()) {
                    String message = getResources().getString(R.string.text_empty_contract) + contrato;
                    showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, message);
                } else {
                    showAlertDialogSaveInfo();
                }
            }
        });
    }

    private void showAlertDialogSaveInfo() {
        alertDialogBuilder.setTitle(R.string.title_save);
        alertDialogBuilder.setMessage(R.string.text_save);
        alertDialogBuilder.setPositiveButton(R.string.text_aceptar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                setInfoGestionContrato();
            }
        });
        alertDialogBuilder.setNegativeButton(R.string.text_cancelar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                dialogInterface.dismiss();
            }
        });
        alertDialogBuilder.show();
    }


    private String validateEmptyDescription(List<DataContratos> dataContratosList) {
        String contrato = "";
        for (DataContratos item : dataContratosList) {
            if (item.getDescripcion().trim().isEmpty()) {
                return item.getNumero();
            }
        }
        return contrato;
    }

    private void setInfoGestionContrato() {
        gestionContrato.setContratos(datosContratosList);
        gestionContrato.setCorreoElectronico(getUsuario().getCorreoElectronico());
        getPresenter().updateContratos(gestionContrato);
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

    /**
     * Consulta los contratos inscritos del usuario.
     */
    private void ConsultConstratosInscritos() {
        getPresenter().consultContratosInscritos(getUsuario().getCorreoElectronico());
    }

    /**
     * Obtiene la Lista Mis facturas.
     */
    private void getListaMisFacturas() {
        listaMisFacturas = (List<FacturaResponse>) getIntent().getExtras().getSerializable(Constants.LISTA_MIS_FACTURAS);
    }

    /**
     * Carga los contratos inscritos del usuario.
     *
     * @param datosContratos
     */
    @Override
    public void loadContratosInscritos(final List<DataContratos> datosContratos) {
        datosContratosList = datosContratos;
        if (datosContratos.size() > 0) {
            getListaMisFacturas();
            runOnUiThread(() -> {
                contratoAdapter = new ContratoAdapter(GestionarContratosActivity.this, R.id.gestionarContratos_lvFacturas, datosContratosList, listaMisFacturas);
                gestionarContratos_lvFacturas.setAdapter(contratoAdapter);
            });
        }
    }

    @Override
    public void showAlertDialogTryAgain(String title, int message, int positive, int negative) {
        showAlertDialog(title, getString(message), positive, negative);

    }

    @Override
    public void showAlertDialogTryAgain(int title, String message, int positive, int negative) {
        showAlertDialog(getString(title), message, positive, negative);
    }

    private void showAlertDialog(final String title, final String message, final int positive, final int negative){
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false, positive, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                getPresenter().consultContratosInscritos(getUsuario().getCorreoElectronico());
            }
        }, negative, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                finish();
            }
        }, null));
    }

    /**
     * Se recorre la lista de facturas para saber cuales se deben eliminar y se vea reflejado al volver a mis facturas, también se elimina
     * de contratos para ver el cambio en el adapter
     */
    @Override
    public void sendFacturaResponse() {
        List<DataContratos> facturaUpdate = new ArrayList<>();
        List<FacturaResponse> facturaResponseList = contratoAdapter.getFacturaResponses();
        ArrayList<FacturaResponse> facturaResponsesEliminar = new ArrayList<>();
        ArrayList<DataContratos> dataContratoEliminar = new ArrayList<>();

        for (int i = 0; i < datosContratosList.size(); i++) {
            if (datosContratosList.get(i).isEliminar()) {
                dataContratoEliminar.add(datosContratosList.get(i));
            }else if (datosContratosList.get(i).getOperacion() == Constants.OPERACION_ACTUALIZAR){
                facturaUpdate.add(datosContratosList.get(i));
            }
        }
        datosContratosList.removeAll(dataContratoEliminar);

        for (int i = 0; i < facturaResponseList.size(); i++) {
            if (facturaResponseList.get(i).isEliminar()) {
                facturaResponsesEliminar.add(facturaResponseList.get(i));
            }
            for (DataContratos item: facturaUpdate) {
                if(item.getNumero().equals(facturaResponseList.get(i).getNumeroContrato())){
                    facturaResponseList.get(i).setDescripcionContrato(item.getDescripcion());
                }
            }
        }


        facturaResponseList.removeAll(facturaResponsesEliminar);
        listaMisFacturas = facturaResponseList;
    }

    @Override
    public void showAlertDialogSaveInformation(final String title, final int message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false, R.string.text_aceptar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                sendFacturaResponse();
                if (datosContratosList.size() == 0) {
                    onBackPressed();
                } else {
                    contratoAdapter.notifyDataSetChanged();
                }
            }
        }, null));

    }

    /**
     * Se envía la lista de facturas a mis facturas
     */
    @Override
    public void onBackPressed() {
        Intent intent = new Intent();
        intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) listaMisFacturas);
        setResultData(Constants.MIS_FACTURAS, intent);
        finish();
        super.onBackPressed();
    }

}
