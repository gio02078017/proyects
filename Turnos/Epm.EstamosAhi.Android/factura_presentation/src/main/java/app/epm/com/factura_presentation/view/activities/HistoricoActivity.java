package app.epm.com.factura_presentation.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import androidx.annotation.Nullable;
import androidx.viewpager.widget.ViewPager;
import androidx.appcompat.widget.Toolbar;
import android.view.View;
import android.widget.RelativeLayout;
import android.widget.TextView;

import java.util.List;

import app.epm.com.facturadomain.business_models.HistoricoFacturaResponse;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.dependency_injection.DomainModule;
import app.epm.com.factura_presentation.presenters.HistoricoPresenter;
import app.epm.com.factura_presentation.view.adapters.HistoricoAdapter;
import app.epm.com.factura_presentation.view.views_activities.IHistoricoView;
import app.epm.com.utilities.helpers.FormatDateFactura;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 19/12/16.
 */

public class HistoricoActivity extends BaseActivity<HistoricoPresenter> implements IHistoricoView {

    private TextView historico_tvNroContrato;
    private ViewPager historico_vpMes;

    private List<HistoricoFacturaResponse> listaHistoricoFacturas;
    private String numberContrato;
    private int cantidadDeHistoricos;
    private String[] mesesDeLasFacturas;
    private String[] numerosDeFacturas;
    private double[] valoresPagadosDescendente;
    private String[] valoresPagadosDescendenteString;
    private TextView[] textViewsBarra = null;
    private RelativeLayout[] relativeLayoutsCirculos;
    private TextView[] textViewsCirculos;
    private TextView tvSeleccionado;
    private boolean openBrowser;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_historico);
        loadDrawerLayout(R.id.generalDrawerLayout);
        setPresenter(new HistoricoPresenter(DomainModule.getFacturaBLInstance
                (getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        createProgressDialog();
        loadViews();
        this.numberContrato = getIntent().getStringExtra(Constants.NUMBER_CONTRATO);
        getPresenter().consultHistorico(numberContrato);
    }

    @Override
    public void manageActionFactura() {
        //The method is not used.
    }

    /**
     * Carga la información historica que devuelve el servicio.
     *
     * @param listaHistoricoFacturas lista de facturas
     */
    @Override
    public void loadHistorico(List<HistoricoFacturaResponse> listaHistoricoFacturas) {
        this.listaHistoricoFacturas = listaHistoricoFacturas;

        runOnUiThread(() -> showComponents());
    }

    /**
     * Permite cargar las vistas del xml.
     */
    private void loadViews() {
        loadTextViews();
        loadTexTViewsBarra();
        loadRelativeLayouts();
        loadTextViewsDeLosCirculos();
        loadToolbar();
        loadListenerToTheControl();
    }

    /**
     * Permite cargar los controles.
     */
    private void loadTextViews() {
        this.historico_tvNroContrato = (TextView) findViewById(R.id.historico_tvNroContrato);
        this.historico_vpMes = (ViewPager) findViewById(R.id.historico_vpMes);
    }

    /**
     * Permite cargar los TextViews de la barra que contiene las fechas de los históricos.
     */
    private void loadTexTViewsBarra() {
        this.textViewsBarra = new TextView[6];
        this.textViewsBarra[0] = (TextView) findViewById(R.id.historico_tvFecha1);
        this.textViewsBarra[1] = (TextView) findViewById(R.id.historico_tvFecha2);
        this.textViewsBarra[2] = (TextView) findViewById(R.id.historico_tvFecha3);
        this.textViewsBarra[3] = (TextView) findViewById(R.id.historico_tvFecha4);
        this.textViewsBarra[4] = (TextView) findViewById(R.id.historico_tvFecha5);
        this.textViewsBarra[5] = (TextView) findViewById(R.id.historico_tvFecha6);
        this.tvSeleccionado = this.textViewsBarra[0];
    }

    /**
     * Permite cargar los Circulos verdes que contiene las fechas de los históricos.
     */
    private void loadRelativeLayouts() {
        this.relativeLayoutsCirculos = new RelativeLayout[6];
        this.relativeLayoutsCirculos[0] = (RelativeLayout) findViewById(R.id.historico_rlCirculo1);
        this.relativeLayoutsCirculos[1] = (RelativeLayout) findViewById(R.id.historico_rlCirculo2);
        this.relativeLayoutsCirculos[2] = (RelativeLayout) findViewById(R.id.historico_rlCirculo3);
        this.relativeLayoutsCirculos[3] = (RelativeLayout) findViewById(R.id.historico_rlCirculo4);
        this.relativeLayoutsCirculos[4] = (RelativeLayout) findViewById(R.id.historico_rlCirculo5);
        this.relativeLayoutsCirculos[5] = (RelativeLayout) findViewById(R.id.historico_rlCirculo6);
    }

    /**
     * Permite cargar los TextViews dentro de los circulos verdes para mostrar las fechas.
     */
    private void loadTextViewsDeLosCirculos() {
        this.textViewsCirculos = new TextView[6];
        this.textViewsCirculos[0] = (TextView) findViewById(R.id.historico_tvCirculo1);
        this.textViewsCirculos[1] = (TextView) findViewById(R.id.historico_tvCirculo2);
        this.textViewsCirculos[2] = (TextView) findViewById(R.id.historico_tvCirculo3);
        this.textViewsCirculos[3] = (TextView) findViewById(R.id.historico_tvCirculo4);
        this.textViewsCirculos[4] = (TextView) findViewById(R.id.historico_tvCirculo5);
        this.textViewsCirculos[5] = (TextView) findViewById(R.id.historico_tvCirculo6);
    }

    /**
     * Carga los eventos a los controles.
     */
    private void loadListenerToTheControl() {
        loadListenerToTheControlViewPager();
        loadListenerToTheControlTextViewsBarra();
        loadListenerToTheControlTextViewsCirculos();
    }

    /**
     * Carga OnPageChange al control historico_vpMes.
     */
    private void loadListenerToTheControlViewPager() {
        this.historico_vpMes.setOnPageChangeListener(new ViewPager.OnPageChangeListener() {
            @Override
            public void onPageScrolled(int position, float positionOffset, int positionOffsetPixels) {
                //The method is not used.
            }

            @Override
            public void onPageSelected(int position) {
                if (tvSeleccionado != textViewsBarra[position]) {
                    changeStyleTextTextViewClickAnother();
                    changeStyleTextViewSelected(position);
                }
            }

            @Override
            public void onPageScrollStateChanged(int state) {
                //The method is not used.
            }
        });
    }

    /**
     * Carga OnClick a los text view barra.
     */
    private void loadListenerToTheControlTextViewsBarra() {
        this.textViewsBarra[0].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewBarra(0);
            }
        });

        this.textViewsBarra[1].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewBarra(1);
            }
        });

        this.textViewsBarra[2].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewBarra(2);
            }
        });

        this.textViewsBarra[3].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewBarra(3);
            }
        });

        this.textViewsBarra[4].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewBarra(4);
            }
        });

        this.textViewsBarra[5].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewBarra(5);
            }
        });
    }

    /**
     * Carga OnClick a los text view circulos.
     */
    private void loadListenerToTheControlTextViewsCirculos() {
        this.textViewsCirculos[0].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewsCirculos(0);
            }
        });

        this.textViewsCirculos[1].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewsCirculos(1);
            }
        });

        this.textViewsCirculos[2].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewsCirculos(2);
            }
        });

        this.textViewsCirculos[3].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewsCirculos(3);
            }
        });

        this.textViewsCirculos[4].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewsCirculos(4);
            }
        });

        this.textViewsCirculos[5].setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickTextViewsCirculos(5);
            }
        });
    }

    /**
     * Funcionalidad que ejecuta en el OnClick de los circulos.
     *
     * @param position
     */
    private void onClickTextViewsCirculos(int position) {
        startDetalleConsumoActivity(position);
    }

    /**
     * Inicializa la Activity etalleConsumoActivity.
     *
     * @param position
     */
    private void startDetalleConsumoActivity(int position) {
        Intent intent = new Intent(this, DetalleConsumoActivity.class);
        intent.putExtra(Constants.NUMBER_FACTURA, this.numerosDeFacturas[position]);
        intent.putExtra(Constants.VALOR_TOTAL_PAGAR, this.valoresPagadosDescendenteString[position]);
        startActivityWithOutDoubleClick(intent);
    }

    /**
     * Funcionalidad que ejecuta en el OnClick de la barra.
     *
     * @param position
     */
    private void onClickTextViewBarra(int position) {
        this.historico_vpMes.setCurrentItem(position);
        return;
    }

    /**
     * Permite cargar el toolbar con el título y el botón de navegación.
     */
    private void loadToolbar() {
        Toolbar toolbarApp;
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
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
     * Permite mostrar los items de la barra dependiendo de la cantidad de items de la lista.
     */
    private void showComponents() {
        this.cantidadDeHistoricos = this.listaHistoricoFacturas.size();
        if (this.cantidadDeHistoricos > 6) {
            this.cantidadDeHistoricos = 6;
        }

        ((TextView) findViewById(R.id.historico_tvLabelNroContrato)).setText(R.string.text_numero_contrato);
        ((TextView) findViewById(R.id.historico_tvLabelValoresFacturas)).setText(R.string.text_valores_facturas_ultimos_meses);
        (findViewById(R.id.historico_LayoutFechas)).setVisibility(View.VISIBLE);
        (findViewById(R.id.Historico_LayoutBordeHistorico)).setVisibility(View.VISIBLE);
        this.historico_tvNroContrato.setText(this.numberContrato);
        HistoricoAdapter historicoAdapter = new HistoricoAdapter(getSupportFragmentManager(),
                this.listaHistoricoFacturas, this);
        this.historico_vpMes.setAdapter(historicoAdapter);

        orderHigherToLessValuesMonthsPaid();
        String mesBarra = Constants.EMPTY_STRING;
        String anioBarra = Constants.EMPTY_STRING;
        String fechaCompletaBarra = Constants.EMPTY_STRING;
        String fechaCompletaCirculo = Constants.EMPTY_STRING;
        String valorFactura = Constants.EMPTY_STRING;

        FormatDateFactura formatDateFactura = new FormatDateFactura();

        this.valoresPagadosDescendenteString = new String[this.cantidadDeHistoricos];

        for (int i = 0; i < this.cantidadDeHistoricos; i++) {

            this.textViewsBarra[i].setVisibility(View.VISIBLE);
            this.relativeLayoutsCirculos[i].setVisibility(View.VISIBLE);

            mesBarra = this.listaHistoricoFacturas.get(i).getNombreMes();
            mesBarra = mesBarra.trim().substring(0, 1).toUpperCase() +
                    mesBarra.trim().substring(1, 3).toLowerCase();
            anioBarra = Integer.toString(this.listaHistoricoFacturas.get(i).getAnio());
            anioBarra = anioBarra.trim().substring(2, 4);
            fechaCompletaBarra = mesBarra + "." + anioBarra;
            this.textViewsBarra[i].setText(fechaCompletaBarra);

            valorFactura = formatDateFactura.moneda(this.valoresPagadosDescendente[i]);
            this.valoresPagadosDescendenteString[i] = valorFactura;
            fechaCompletaCirculo = this.mesesDeLasFacturas[i];
            fechaCompletaCirculo = fechaCompletaCirculo.trim().substring(0, 1).toUpperCase() +
                    fechaCompletaCirculo.trim().substring(1, 6).toLowerCase();
            fechaCompletaCirculo = fechaCompletaCirculo + "\n" + valorFactura;
            this.textViewsCirculos[i].setText(fechaCompletaCirculo);
        }
    }

    /**
     * Permite cambiar el color del fondo y la letra al TextView antes seleccionado a su estado por defecto.
     */
    private void changeStyleTextTextViewClickAnother() {
        this.tvSeleccionado.setBackground(null);
        this.tvSeleccionado.setTextColor(getResources().getColor(R.color.text_gray_barra_historico));
    }

    /**
     * Permite cambiar el color del fondo y la letra al TextView seleccionado.
     *
     * @param position Posición del item seleccionado.
     */
    private void changeStyleTextViewSelected(int position) {
        this.textViewsBarra[position].setBackgroundResource(R.drawable.border_textview_fecha_historico);
        this.textViewsBarra[position].setTextColor(getResources().getColor(R.color.black));
        this.tvSeleccionado = this.textViewsBarra[position];
    }

    /**
     * Permite ordenar de mayor a menor los valores de los meses pagados.
     */
    private void orderHigherToLessValuesMonthsPaid() {
        this.valoresPagadosDescendente = new double[this.cantidadDeHistoricos];
        this.mesesDeLasFacturas = new String[this.cantidadDeHistoricos];
        this.numerosDeFacturas = new String[this.cantidadDeHistoricos];
        for (int i = 0; i < this.cantidadDeHistoricos; i++) {
            this.valoresPagadosDescendente[i] = this.listaHistoricoFacturas.get(i).getValorFactura();
            this.mesesDeLasFacturas[i] = this.listaHistoricoFacturas.get(i).getNombreMes().substring(0, 3) +
                    "." + Integer.toString(this.listaHistoricoFacturas.get(i).getAnio()).substring(2, 4);
            this.numerosDeFacturas[i] = this.listaHistoricoFacturas.get(i).getNumeroFactura();
        }

        orderDescValues();
    }

    /**
     * Permite ordenar de manera descendente un arreglo de enteros.
     */
    private void orderDescValues() {
        for (int i = 0; i < this.valoresPagadosDescendente.length - 1; i++) {
            int max = i;

            //buscamos el mayor número
            for (int j = i + 1; j < this.valoresPagadosDescendente.length; j++) {
                if (this.valoresPagadosDescendente[j] > this.valoresPagadosDescendente[max]) {
                    max = j;    //encontramos el mayor número
                }
            }

            if (i != max) {
                //permutamos los valores
                double auxValor = this.valoresPagadosDescendente[i];
                this.valoresPagadosDescendente[i] = this.valoresPagadosDescendente[max];
                this.valoresPagadosDescendente[max] = auxValor;

                String auxMes = mesesDeLasFacturas[i];
                this.mesesDeLasFacturas[i] = this.mesesDeLasFacturas[max];
                this.mesesDeLasFacturas[max] = auxMes;

                String auxNumeroFactura = this.numerosDeFacturas[i];
                this.numerosDeFacturas[i] = this.numerosDeFacturas[max];
                this.numerosDeFacturas[max] = auxNumeroFactura;
            }
        }
    }

    @Override
    public void openBrowser(boolean openBrowser) {
        this.openBrowser = openBrowser;
    }

    @Override
    public void showAlertDialogtryAgain(String title, int message, int positive, int negative) {
        showAlertDialog(title, getString(message), positive, negative);
    }

    @Override
    public void showAlertDialogtryAgain(int title, String message, int positive, int negative) {
        showAlertDialog(getString(title), message, positive, negative);
    }

    private void showAlertDialog(final String title, final String message, final int positive, final int negative) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false, positive, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                getPresenter().consultHistorico(numberContrato);
            }
        }, negative, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                finish();
            }
        }, null));
    }

    @Override
    public Intent getDefaultIntent(Intent intent) {
        if (this.openBrowser) {
            return intent;
        } else {
            return super.getDefaultIntent(intent);
        }
    }

}