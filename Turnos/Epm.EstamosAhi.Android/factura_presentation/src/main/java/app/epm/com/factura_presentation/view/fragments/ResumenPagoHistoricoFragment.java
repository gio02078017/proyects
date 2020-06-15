package app.epm.com.factura_presentation.view.fragments;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.presenters.HistoricoPresenter;
import app.epm.com.factura_presentation.view.views_activities.IHistoricoView;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.fragments.BaseFragment;

/**
 * Created by mateoquicenososa on 19/12/16.
 */

public class ResumenPagoHistoricoFragment extends BaseFragment<HistoricoPresenter>{

    private TextView resumenPago_tvResultadoAnio;
    private TextView resumenPago_tvResultadoMes;
    private TextView resumenPago_tvResultadoFechaDePago;
    private TextView resumenPago_tvResultadoPagadaEn;
    private TextView resumenPago_tvResultadoValorPagado;
    private Button resumenPago_btnVerFacturaPDF;
    private LayoutInflater layoutInflater;
    private IHistoricoView historicoView;
    private String urlPDF;

    public static ResumenPagoHistoricoFragment newInstance(IHistoricoView historicoView) {
        ResumenPagoHistoricoFragment fragment = new ResumenPagoHistoricoFragment();
        fragment.setHistoricoView(historicoView);
        return fragment;
    }

    public  void setHistoricoView(IHistoricoView historicoView)
    {
        this.historicoView = historicoView;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_resumen_pago_historico, container, false);
        this.layoutInflater = inflater;
        loadViews(view);
        loadListenerToTheControl();
        loadInformacion(view);
        return view;
    }

    private void loadViews(View view) {
        this.resumenPago_tvResultadoAnio =  view.findViewById(R.id.resumenPago_tvResultadoAnio);
        this.resumenPago_tvResultadoMes =  view.findViewById(R.id.resumenPago_tvResultadoMes);
        this.resumenPago_tvResultadoFechaDePago =  view.findViewById(R.id.resumenPago_tvResultadoFechaDePago);
        this.resumenPago_tvResultadoPagadaEn =  view.findViewById(R.id.resumenPago_tvResultadoPagadaEn);
        this.resumenPago_tvResultadoValorPagado =  view.findViewById(R.id.resumenPago_tvResultadoValorPagado);
        this.resumenPago_btnVerFacturaPDF =  view.findViewById(R.id.resumenPago_btnVerFacturaPDF);
    }

    private void loadInformacion(View view) {
        Bundle arg = getArguments();
        if (!arg.getBoolean(Constants.ARG_FACTURA_PAGADA)) {
            ((TextView) view.findViewById(R.id.resumenPago_tvTitulo)).setText(R.string.title_resumen_de_factura);
            ((TextView) view.findViewById(R.id.resumenPago_tvFechaDePago)).setText(R.string.label_resumen_pago_FechaVencimiento);
            ((TextView) view.findViewById(R.id.resumenPago_tvPagadaEn)).setText(R.string.label_resumen_pago_FechaRecargo);
            ((TextView) view.findViewById(R.id.resumenPago_tvValorPagado)).setText(R.string.label_resumen_pago_ValorTotal);
        }

        this.urlPDF = arg.getString(Constants.ARG_URL_PDF);
        this.resumenPago_tvResultadoAnio.setText(arg.getString(Constants.ARG_ANIO));
        this.resumenPago_tvResultadoMes.setText(arg.getString(Constants.ARG_MES));
        this.resumenPago_tvResultadoFechaDePago.setText(arg.getString(Constants.ARG_FECHA_DE_PAGO_RESULTADO));
        this.resumenPago_tvResultadoPagadaEn.setText(arg.getString(Constants.ARG_PAGADA_EN_RESULTADO));
        this.resumenPago_tvResultadoValorPagado.setText(arg.getString(Constants.ARG_VALOR_PAGADO_RESULTADO));
    }

    private void loadListenerToTheControl() {
        this.resumenPago_btnVerFacturaPDF.setOnClickListener(view -> startVerFacturaPDFActivity());
    }

    /**
     * Visualiza la factura en pdf en el navegador.
     */
    private void startVerFacturaPDFActivity() {
        if (!urlPDF.isEmpty()) {
            Intent intent = new Intent(Intent.ACTION_VIEW);
            intent.setData(Uri.parse(urlPDF));
            this.historicoView.openBrowser(true);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
            this.historicoView.openBrowser(false);
        }
    }
}
