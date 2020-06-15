package app.epm.com.factura_presentation.view.activities;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import androidx.appcompat.widget.Toolbar;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;

import java.io.Serializable;
import java.util.List;

import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.dependency_injection.DomainModule;
import app.epm.com.factura_presentation.presenters.FacturasConsultadasPresenter;
import app.epm.com.factura_presentation.view.adapters.FacturaAdapter;
import app.epm.com.factura_presentation.view.views_activities.IFacturasConsultadasView;
import app.epm.com.utilities.custom_controls.CustomAlertDialog;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.FormatDateFactura;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 13/12/16.
 */

public class FacturasConsultadasActivity extends BaseActivity<FacturasConsultadasPresenter> implements IFacturasConsultadasView {

    private Toolbar toolbarApp;
    private LinearLayout facturasConsultadas_llFacturasConsultadas;
    private LinearLayout facturasConsultadas_llSinContratos;
    private LinearLayout facturasConsultadas_llTextViewsTotal;
    private TextView facturasConsultadas_tvTotal;
    private TextView facturasConsultadas_tvTotalPagar;
    private Button facturasConsultadas_btnPagar;
    private LinearLayout facturasConsultads_llListaFacturas;
    private Button facturasConsultadas_btnConsultar;
    private ListView facturasConsultadas_lvFacturas;
    private TextView facturasConsultad_tvLine;
    private ImageView facturasConsultadas_ivPagar;
    private CustomAlertDialog customAlertDialog;

    private FacturaAdapter adapter;
    private FormatDateFactura formatDateFactura;
    private List<FacturaResponse> facturaResponses;
    private int positionFactura;
    private AlertDialog.Builder alertDialogBuilder;
    private ICustomSharedPreferences customSharedPreferences;
    private boolean openBrowser;
    private Menu menuFacturas;
    private MenuItem item;
    private boolean stateItem;
    private boolean controlDoubleTab = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_facturas_consultadas);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        this.customSharedPreferences = new CustomSharedPreferences(this);
        setPresenter(new FacturasConsultadasPresenter(DomainModule.getFacturaBLInstance(this.customSharedPreferences)));
        getPresenter().inject(this, getValidateInternet(), customSharedPreferences);
        this.customAlertDialog = new CustomAlertDialog(this);
        loadViews();
        createProgressDialog();
        getIntentListFacturaResponse();
        validateConsultFactura();
        validateFacturaResponseEmpty();
        alertDialogBuilder = new AlertDialog.Builder(this);
        formatDateFactura = new FormatDateFactura();
        if (getFacturaResponses() != null) {
            valorTotalAPagar(getFacturaResponses());
        }
        openAlertRateApp();
        stateItem = true;
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

    @Override
    public Intent getDefaultIntent(Intent intent) {
        if (openBrowser) {
            return intent;
        } else {
            return super.getDefaultIntent(intent);
        }
    }

    /**
     * Valida si la lista de facturas está vacia para mostrar otra vista
     */
    private void validateFacturaResponseEmpty() {
        if (facturaResponses != null && facturaResponses.isEmpty()) {
            showViewWithOutBill(facturaResponses);
        }
    }

    /**
     * Valida si debe ir a consultar el servicio de facturas por usuario
     */
    private void validateConsultFactura() {
        if (facturaResponses == null && getUsuario().isInvitado() == false) {
            validateUser();
        }
    }

    /**
     * Valida si le han pasado la lista de facturas
     */
    private void getIntentListFacturaResponse() {
        Intent intent = getIntent();

        if (intent.getSerializableExtra(Constants.LISTA_MIS_FACTURAS) != null) {
            facturaResponses = (List<FacturaResponse>) intent.getSerializableExtra(Constants.LISTA_MIS_FACTURAS);
            positionFactura = intent.getIntExtra(Constants.POSICION_LISTA_MIS_FACTURAS, 0);
            setFacturaResponses(facturaResponses);
            adapter = new FacturaAdapter(FacturasConsultadasActivity.this, R.id.facturasConsultadas_lvFacturas, getFacturaResponses(), getUsuario().isInvitado(), this);
            facturasConsultadas_lvFacturas.setAdapter(adapter);
            facturasConsultadas_lvFacturas.setSelection(positionFactura);
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (getFacturaResponses() != null) {
            valorTotalAPagar(getFacturaResponses());
        }
        if(adapter != null){
            adapter.setControlDoubleTab(false);
        }
        controlDoubleTab = false;
    }

    @Override
    protected void onRestart() {
        super.onRestart();
        if (getFacturaResponses() != null) {
            valorTotalAPagar(getFacturaResponses());
        }
    }

    private void loadListener() {
        facturasConsultadas_btnConsultar.setOnClickListener(view -> {
            Intent intent = new Intent(FacturasConsultadasActivity.this, ConsultFacturaActivity.class);
            intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) facturaResponses);
            intent.addFlags(Intent.FLAG_ACTIVITY_EXCLUDE_FROM_RECENTS);
            startActivityWithOutDoubleClick(intent);
        });

        facturasConsultadas_btnPagar.setOnClickListener(view -> {
            String message = String.format("%s %s", FacturasConsultadasActivity.this.getResources().getString(R.string.factura_message_to_pse), facturasConsultadas_tvTotalPagar.getText().toString().trim() + "?");
            alertDialogBuilder.setTitle(R.string.factura_title_go_to_pay);
            alertDialogBuilder.setMessage(message);
            alertDialogBuilder.setPositiveButton(R.string.text_aceptar, (dialogInterface, i) -> {
                if (validateFacturaVencida(getFacturaResponses())) {
                    if (getUsuario().isInvitado() == false) {
                        showAlertDialogGoToPse(getUsuario().getNombres(), R.string.factura_text_vencida);
                    } else {
                        showAlertDialogGoToPse(getResources().getString(R.string.title_appreciated_user), R.string.factura_text_vencida);
                    }
                } else {
                    getPresenter().validateInternetListEntityFinancial();
                }
            });
            alertDialogBuilder.setNegativeButton(R.string.text_cancelar, (dialogInterface, i) -> dialogInterface.dismiss());
            alertDialogBuilder.show();
        });
    }

    /**
     * Permite ir a la actividad de pago
     */
    @Override
    public void goToPagoActivity() {
        Intent intent = new Intent(this, IngresarDatosPSEActivity.class);
        intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) getFacturaResponses());
        startActivityWithOutDoubleClick(intent);
    }

    @Override
    public void openBrowser(boolean openBrowser) {
        this.openBrowser = openBrowser;
    }

    private void showAlertDialogGoToPse(String title, int message) {
        alertDialogBuilder.setTitle(title);
        alertDialogBuilder.setMessage(message);
        alertDialogBuilder.setPositiveButton(R.string.text_aceptar, (dialogInterface, i) -> getPresenter().validateInternetListEntityFinancial());
        alertDialogBuilder.setNegativeButton(null, (dialogInterface, i) -> dialogInterface.dismiss());
        alertDialogBuilder.setCancelable(false);
        alertDialogBuilder.show();
    }

    /**
     * Permite validar si una factura está vencida
     *
     * @param facturaResponses lista de facturas
     * @return estado de la factura
     */
    private boolean validateFacturaVencida(List<FacturaResponse> facturaResponses) {
        boolean status = false;
        for (FacturaResponse facturaResponse : facturaResponses) {
            if (facturaResponse.isEstaSeleccionadaParaPago() && facturaResponse.isFacturaVencida()) {
                status = true;
            }
        }
        return status;
    }

    /**
     * Permite validar si el usuario tiene facturas asociadas
     */
    private void validateUser() {
        getPresenter().validateInternetToConsultBill(getUsuario().getCorreoElectronico());
    }

    /**
     * Permite cargar las vistas de la actividad
     */
    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        facturasConsultadas_llFacturasConsultadas = (LinearLayout) findViewById(R.id.facturasConsultadas_llFacturasConsultadas);
        facturasConsultadas_lvFacturas = (ListView) findViewById(R.id.facturasConsultadas_lvFacturas);
        facturasConsultads_llListaFacturas = (LinearLayout) findViewById(R.id.facturasConsultads_llListaFacturas);
        facturasConsultadas_llSinContratos = (LinearLayout) findViewById(R.id.facturasConsultadas_llSinContratos);
        facturasConsultadas_llTextViewsTotal = (LinearLayout) findViewById(R.id.facturasConsultadas_llTextViewsTotal);
        facturasConsultadas_tvTotal = (TextView) findViewById(R.id.facturasConsultadas_tvTotal);
        facturasConsultadas_tvTotalPagar = (TextView) findViewById(R.id.facturasConsultadas_tvTotalPagar);
        facturasConsultadas_btnPagar = (Button) findViewById(R.id.facturasConsultadas_btnPagar);
        facturasConsultadas_btnConsultar = (Button) findViewById(R.id.facturasConsultadas_btnConsultar);
        facturasConsultad_tvLine = (TextView) findViewById(R.id.facturasConsultad_tvLine);
        facturasConsultadas_ivPagar = (ImageView) findViewById(R.id.facturasConsultadas_ivPagar);
        loadToolbar();
        loadListener();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        menuFacturas = menu;
        if (getUsuario().isInvitado() == false) {
            getMenuInflater().inflate(R.menu.menu_mis_facturas, menu);
            item = menu.findItem(R.id.menuGeneral_gestionarMisFacturas);
            item.setVisible(stateItem);
        }
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        if (id == R.id.menuGeneral_gestionarMisFacturas) {
            startGestionarContratos();
        }
        return super.onOptionsItemSelected(item);
    }

    private void startGestionarContratos() {
        Intent intent = new Intent(this, GestionarContratosActivity.class);
        intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) getFacturaResponses());
        startActivityWithOutDoubleClick(intent);
    }

    /**
     * Permite cargar el toolbar
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
     * Permite guardar la lista de facturas
     *
     * @param facturaResponseList
     */
    @Override
    public void setFacturaResponse(List<FacturaResponse> facturaResponseList) {
        for (FacturaResponse facturaResponse : facturaResponseList) {
            facturaResponse.setEstaInscrita(FacturaResponse.EstaInscrita.INSCRITA);
        }
        setFacturaResponses(facturaResponseList);
        valorTotalAPagar(getFacturaResponses());
    }

    /**
     * Permite valida el estado de la factura
     *
     * @param estadoPendiente boolean de estado de la factura
     * @param position        posicion de la factura
     */
    @Override
    public void setValueBill(boolean estadoPendiente, final int position) {
        if (estadoPendiente) {
            showAlertDialogGeneralInformationOnUiThread(R.string.factura_title_pending, R.string.factura_message_pending);
            getFacturaResponses().get(position).setEstaSeleccionadaParaPago(false);
        } else {
            valorTotalAPagar(getFacturaResponses());
        }
        getFacturaResponses().get(position).setEstaPendiente(estadoPendiente);
        runOnUiThread(() -> {
            adapter.notifyDataSetChanged();
            facturasConsultadas_lvFacturas.setSelection(position);
        });
    }


    public void validateInternet(int position) {
        if (!getValidateInternet().isConnected()) {
            getFacturaResponses().get(position).setEstaSeleccionadaParaPago(false);
            showAlertDialogGeneralInformationOnUiThread(getName(), R.string.text_validate_internet);
            adapter.notifyDataSetChanged();
            facturasConsultadas_lvFacturas.setSelection(position);
        }
    }

    @Override
    public void showAlertDialogError(final int position, final int title, final String message) {
        runOnUiThread(() -> {
            adapter.notifyDataSetChanged();
            facturasConsultadas_lvFacturas.setSelection(position);
            getFacturaResponses().get(position).setEstaSeleccionadaParaPago(false);
            showAlertDialogGeneralInformationOnUiThread(title, message);
        });
    }

    @Override
    public void showViewWithOutBill(final List<FacturaResponse> facturaResponseList) {
        runOnUiThread(() -> {
            facturasConsultadas_llFacturasConsultadas.setBackgroundResource(R.color.gray_sin_contratos);
            facturasConsultadas_llSinContratos.setVisibility(View.VISIBLE);
            facturasConsultadas_btnConsultar.setVisibility(View.VISIBLE);
            facturasConsultadas_llTextViewsTotal.setVisibility(View.GONE);
            facturasConsultads_llListaFacturas.setVisibility(View.GONE);
            facturasConsultadas_btnConsultar.setText(R.string.btn_ir_a_consultar);
            stateItem = false;
            item.setVisible(false);
        });
    }

    @Override
    public void showAlertDialogTryAgain(String title, int message, int positive, int negative) {
        showAlert(title, getString(message), positive, negative);
    }

    @Override
    public void showAlertDialogTryAgain(int title, String message, int positive, int negative) {
        showAlert(getString(title), message, positive, negative);
    }

    private void showAlert(final String title, final String message, final int positive, final int negative) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false, positive, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                getPresenter().validateInternetToConsultBill(getUsuario().getCorreoElectronico());
            }
        }, negative, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                finish();
            }
        }, null));
    }

    /**
     * Permite poner viisble los componentes del xml de facturasConsultadas
     */
    private void showViews() {
        facturasConsultadas_llFacturasConsultadas.setBackgroundResource(R.color.white);
        facturasConsultads_llListaFacturas.setVisibility(View.VISIBLE);
        facturasConsultadas_lvFacturas.setVisibility(View.VISIBLE);
        facturasConsultadas_llTextViewsTotal.setVisibility(View.VISIBLE);
        facturasConsultadas_llTextViewsTotal.setBackgroundResource(R.color.gray_barra_historico);
        facturasConsultadas_btnPagar.setVisibility(View.VISIBLE);
        facturasConsultadas_btnPagar.setEnabled(false);
        facturasConsultadas_btnPagar.setBackgroundResource(R.color.gray_barra_historico);
        facturasConsultad_tvLine.setVisibility(View.VISIBLE);
        facturasConsultadas_tvTotal.setVisibility(View.VISIBLE);
        facturasConsultadas_btnConsultar.setVisibility(View.VISIBLE);
        facturasConsultadas_btnConsultar.setText(R.string.btn_adicionar_factura);
        facturasConsultadas_tvTotalPagar.setVisibility(View.VISIBLE);
        facturasConsultadas_ivPagar.setVisibility(View.VISIBLE);
        facturasConsultadas_llSinContratos.setVisibility(View.GONE);
    }

    public List<FacturaResponse> getFacturaResponses() {
        return facturaResponses;
    }

    public void setFacturaResponses(final List<FacturaResponse> facturaResponses) {
        this.facturaResponses = facturaResponses;
        runOnUiThread(() -> {
            showViews();
            adapter = new FacturaAdapter(FacturasConsultadasActivity.this, R.id.facturasConsultadas_lvFacturas, facturaResponses, getUsuario().isInvitado(), FacturasConsultadasActivity.this);
            adapter.notifyDataSetChanged();
            facturasConsultadas_lvFacturas.setAdapter(adapter);
            facturasConsultadas_lvFacturas.setSelection(positionFactura);
        });
    }

    /**
     * Permite calcular el valor a pagar
     *
     * @param facturaResponseList
     */
    public void valorTotalAPagar(final List<FacturaResponse> facturaResponseList) {
        runOnUiThread(() -> {
            int valorPagar = 0;
            for (FacturaResponse factura : facturaResponseList) {
                if (factura != null && factura.isEstaSeleccionadaParaPago()) {
                    valorPagar = valorPagar + factura.getValorFactura();
                }
            }
            facturasConsultadas_tvTotalPagar.setText(formatDateFactura.moneda(valorPagar));
            if (valorPagar == 0) {
                toDisableButtonPay();
            } else {
                enableButtonPay();
            }
            customSharedPreferences.addInt(Constants.VALOR_TOTAL_PAGAR, valorPagar);
        });
    }

    /**
     * Habiliar botón para pagar
     */
    private void toDisableButtonPay() {
        facturasConsultadas_llTextViewsTotal.setBackgroundResource(R.color.gray_barra_historico);
        facturasConsultadas_btnPagar.setBackgroundResource(R.color.gray_barra_historico);
        facturasConsultadas_btnPagar.setEnabled(false);
    }

    /**
     * Deshabilitar botón para pagar
     */
    private void enableButtonPay() {
        facturasConsultadas_llTextViewsTotal.setBackgroundResource(R.color.green_factura);
        facturasConsultadas_btnPagar.setBackgroundResource(R.color.green_factura);
        facturasConsultadas_btnPagar.setEnabled(true);
    }

    /**
     * Permite validar si la factura esta pendiente
     *
     * @param referencia numero de la factura
     */
    public void consultStatusPendingBill(int position, String referencia) {
        getPresenter().consultStatusPendingBill(position, referencia);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == Constants.FACTURA) {
            positionFactura = data.getIntExtra(Constants.POSICION_LISTA_MIS_FACTURAS, 0);
            setFacturaResponses((List<FacturaResponse>) data.getSerializableExtra(Constants.LISTA_MIS_FACTURAS));
        }
        if (resultCode == Constants.MIS_FACTURAS) {
            setFacturaResponses((List<FacturaResponse>) data.getSerializableExtra(Constants.LISTA_MIS_FACTURAS));
            if (getFacturaResponses().size() == 0) {
                showViewWithOutBill(getFacturaResponses());
            }
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        if(!controlDoubleTab ) {
            Class landingClass = null;
            try {
                landingClass = Class.forName(customSharedPreferences.getString(Constants.LANDING_CLASS));
            } catch (ClassNotFoundException e) {
                Log.e("Exception", e.toString());
            }
            Intent intent = new Intent(FacturasConsultadasActivity.this, landingClass);
            intent.putExtra(Constants.CALLED_FROM_ANOTHER_MODULE, true);
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
            startActivity(intent);
            controlDoubleTab = true;
        }
    }
}