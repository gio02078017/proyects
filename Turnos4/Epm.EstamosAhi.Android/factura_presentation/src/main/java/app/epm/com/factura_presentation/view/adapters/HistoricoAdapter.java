package app.epm.com.factura_presentation.view.adapters;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentStatePagerAdapter;

import java.util.List;

import app.epm.com.facturadomain.business_models.HistoricoFacturaResponse;

import app.epm.com.factura_presentation.view.fragments.ResumenPagoHistoricoFragment;
import app.epm.com.factura_presentation.view.views_activities.IHistoricoView;
import app.epm.com.utilities.helpers.FormatDateFactura;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by ocadavid on 5/01/2017.
 */
public class HistoricoAdapter  extends FragmentStatePagerAdapter {
    private List<HistoricoFacturaResponse> listaHistoricoFacturas;
    private int cantidadItems;
    private IHistoricoView historicoView;
    private FormatDateFactura formatDateFactura = null;

    public HistoricoAdapter(FragmentManager fragmentManager, List<HistoricoFacturaResponse> listaHistoricoFacturas, IHistoricoView historicoView) {
        super(fragmentManager);
        this.historicoView = historicoView;
        this.listaHistoricoFacturas = listaHistoricoFacturas;
        this.cantidadItems = listaHistoricoFacturas.size();
        this.formatDateFactura = new FormatDateFactura();
    }

    @Override
    public Fragment getItem(int position) {

        Fragment fragment = new ResumenPagoHistoricoFragment().newInstance(historicoView);
        Bundle bundle = new Bundle();

        String mes = this.listaHistoricoFacturas.get(position).getNombreMes().toLowerCase();
        String mesFormat = Character.toUpperCase(mes.charAt(0)) + mes.substring(1);
        bundle.putString(Constants.ARG_MES, mesFormat);
        bundle.putString(Constants.ARG_ANIO, Integer.toString(this.listaHistoricoFacturas.get(position).getAnio()));
        if (this.listaHistoricoFacturas.get(position).isEstadoPagoFactura()){
            bundle.putString(Constants.ARG_FECHA_DE_PAGO_RESULTADO, this.formatDateFactura.formatDate(this.listaHistoricoFacturas.get(position).getFechaPagoFactura(), Constants.FORMATYMD, Constants.FORMATDMY));
            bundle.putString(Constants.ARG_PAGADA_EN_RESULTADO, this.listaHistoricoFacturas.get(position).getBancoPagoFactura());
        }
        else {
            bundle.putString(Constants.ARG_FECHA_DE_PAGO_RESULTADO, this.formatDateFactura.formatDate(this.listaHistoricoFacturas.get(position).getFechaVencimiento(), Constants.FORMATYMD, Constants.FORMATDMY));
            bundle.putString(Constants.ARG_PAGADA_EN_RESULTADO, this.formatDateFactura.formatDate(this.listaHistoricoFacturas.get(position).getFechaRecargo(), Constants.FORMATYMD, Constants.FORMATDMY));
        }
        String valorPagado = this.formatDateFactura.moneda(this.listaHistoricoFacturas.get(position).getValorFactura());
        bundle.putString(Constants.ARG_VALOR_PAGADO_RESULTADO, valorPagado);
        bundle.putBoolean(Constants.ARG_FACTURA_PAGADA, this.listaHistoricoFacturas.get(position).isEstadoPagoFactura());
        bundle.putString(Constants.ARG_URL_PDF, this.listaHistoricoFacturas.get(position).getUrlFacturaDigital());

        fragment.setArguments(bundle);
        return fragment;
    }

    @Override
    public int getCount() {
        return this.cantidadItems;
    }
}
