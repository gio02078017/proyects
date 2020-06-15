package app.epm.com.factura_presentation.view.activities;

import android.content.DialogInterface;
import android.os.Bundle;
import androidx.annotation.Nullable;
import androidx.appcompat.widget.Toolbar;
import android.view.View;
import android.widget.TextView;

import java.util.List;

import app.epm.com.facturadomain.business_models.ServicioFacturaResponse;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.dependency_injection.DomainModule;
import app.epm.com.factura_presentation.presenters.DetalleConsumoPresenter;
import app.epm.com.factura_presentation.view.adapters.DetalleConsumoAdapter;
import app.epm.com.factura_presentation.view.views_activities.IDetalleConsumoView;
import app.epm.com.utilities.controls.ListViewExpandible;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 14/12/16.
 */

public class DetalleConsumoActivity extends BaseActivity<DetalleConsumoPresenter> implements IDetalleConsumoView {

    private Toolbar toolbarApp;
    private ListViewExpandible detalleConsumo_lvExpandible;
    private TextView detalleConsumo_tvValorAPagar;
    private String valorTotalPagar;
    private String numberFactura;
    private List<ServicioFacturaResponse> listaServicioFacturas;
    DetalleConsumoAdapter detalleConsumoAdapter;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_detalle_consumo);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        ICustomSharedPreferences customSharedPreferences = new CustomSharedPreferences(this);
        setPresenter(new DetalleConsumoPresenter(DomainModule.getFacturaBLInstance(customSharedPreferences)));
        getPresenter().inject(this, getValidateInternet());
        createProgressDialog();
        loadViews();
        consultDetalleFacturas();
    }

    @Override
    public void manageActionFactura() {
        //The method is not used.
    }

    /**
     * Consulta el detalle de una factura
     */
    private void consultDetalleFacturas() {
        this.numberFactura = getIntent().getStringExtra(Constants.NUMBER_FACTURA);
        this.valorTotalPagar = getIntent().getStringExtra(Constants.VALOR_TOTAL_PAGAR);
        getPresenter().validateInternetToGetDetailFactura(this.numberFactura);
    }

    /**
     * Carga la información detalle de facturas que devuelve el servicio.
     *
     * @param listaServicioFacturas
     */
    @Override
    public void loadDetalleFacturas(List<ServicioFacturaResponse> listaServicioFacturas) {
        this.listaServicioFacturas = listaServicioFacturas;
        runOnUiThread(() -> {

            loadValorAPagar();
            loadListaDeplegable();
        });
    }

    /**
     * Carga el valor total a pagar en el textview
     */
    private void loadValorAPagar() {
        (findViewById(R.id.LayoutValorAPagar)).setVisibility(View.VISIBLE);
        (findViewById(R.id.detalle_viewLine)).setVisibility(View.VISIBLE);
        this.detalleConsumo_tvValorAPagar.setText(this.valorTotalPagar);
    }

    /**
     * Carga el detalle de consumo en el ListView y el adapter.
     */
    private void loadListaDeplegable() {
        this.detalleConsumo_lvExpandible = (ListViewExpandible) findViewById(R.id.detalleConsumo_lvExpandible);
        this.detalleConsumoAdapter = new DetalleConsumoAdapter(DetalleConsumoActivity.this, detalleConsumo_lvExpandible, this.listaServicioFacturas);
        this.detalleConsumo_lvExpandible.setAdapter(detalleConsumoAdapter);
    }

    /**
     * Carga la vista.
     */
    private void loadViews() {
        this.toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        this.detalleConsumo_lvExpandible = (ListViewExpandible) findViewById(R.id.detalleConsumo_lvExpandible);
        this.detalleConsumo_tvValorAPagar = (TextView) findViewById(R.id.detalleConsumo_tvValorAPagar);
        loadToolbar();
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

    @Override
    public void showAlertDialogToLoadAgain(String title, int message) {
        showAlertDialogToLoadAgain(title, getString(message));
    }

    @Override
    public void showAlertDialogToLoadAgain(int title, String message) {
        showAlertDialogToLoadAgain(getString(title), message);
    }

    private void showAlertDialogToLoadAgain(final String title, final String message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false, R.string.text_intentar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                getPresenter().validateInternetToGetDetailFactura(numberFactura);
            }
        }, R.string.text_cancelar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                finishActivity();
            }
        }, null));

    }
}
