package app.epm.com.factura_presentation.view.adapters;

import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.Typeface;
import android.net.Uri;
import android.os.Bundle;
import androidx.fragment.app.FragmentActivity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.io.Serializable;
import java.util.List;

import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.view.activities.DetalleConsumoActivity;
import app.epm.com.factura_presentation.view.activities.FacturasConsultadasActivity;
import app.epm.com.factura_presentation.view.activities.HistoricoActivity;
import app.epm.com.factura_presentation.view.activities.InscribirContratoActivity;
import app.epm.com.factura_presentation.view.views_activities.IFacturasConsultadasView;
import app.epm.com.security_presentation.view.activities.RegisterLoginActivity;
import app.epm.com.utilities.helpers.FormatDateFactura;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 27/12/16.
 */

public class FacturaAdapter extends ArrayAdapter<FacturaResponse> {

    private FragmentActivity context;
    private List<FacturaResponse> facturaResponseList;
    private FacturaResponse facturaResponse;
    private int valor = 0;
    private FormatDateFactura formatDateFactura;
    private LinearLayout facturas_llDescipcionFactura;
    private ProgressDialog progressDialog;
    private TextView factura_tvAlias;
    private TextView factura_tvbNumeroContrato;
    private TextView factura_tvbNumeroReferente;
    private TextView factura_tvbPeriodo;
    private TextView factura_tvbPagoSinRecargo;
    private TextView factura_tvbPagoConRecargo;
    private TextView factura_tvValorPagar;
    private TextView factura_tvbValorPagar;
    private TextView facturas_tvLineaSeparador;
    private Button factura_btnInscribirContrato;
    private TextView factura_tvSumarAlPago;
    private CheckBox factura_checkIncluir;
    private ImageButton factura_imgVerPdf;
    private Button factura_btnVerPdf;
    private ImageButton factura_imgHistorico;
    private Button factura_btnVerHistorico;
    private ImageButton factura_imgVerDetalle;
    private Button factura_btnVerDetalle;
    private IFacturasConsultadasView facturasConsultadasView;
    private boolean isInvitado;
    private boolean controlDoubleTab = false;

    public FacturaAdapter(FragmentActivity context, int resource, List<FacturaResponse> facturaResponseList, boolean isInvitado, IFacturasConsultadasView facturasConsultadasView) {
        super(context, resource, facturaResponseList);
        this.context = context;
        this.facturasConsultadasView = facturasConsultadasView;
        this.facturaResponseList = facturaResponseList;
        formatDateFactura = new FormatDateFactura();
        this.isInvitado = isInvitado;
    }

    public int getCount() {
        return this.facturaResponseList.size();
    }


    @Override
    public View getView(final int position, View convertView, ViewGroup parent) {
        View customView = convertView;
        LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        customView = inflater.inflate(R.layout.list_factura_item, parent, false);
        loadViews(customView);
        loadListenerToTheControl(position);
        facturaResponse = this.facturaResponseList.get(position);
        setInfoFactura(facturaResponse);
        if (facturaResponse.getDescripcionContrato() != null) {
            facturas_llDescipcionFactura.setVisibility(View.VISIBLE);
            factura_tvAlias.setText(facturaResponse.getDescripcionContrato());
        } else {
            facturas_llDescipcionFactura.setVisibility(View.GONE);
            factura_btnInscribirContrato.setVisibility(View.GONE);
        }
        if (!facturaResponse.getEstaInscrita().equals(FacturaResponse.EstaInscrita.INSCRITA)) {
            facturas_tvLineaSeparador.setVisibility(View.VISIBLE);
            factura_btnInscribirContrato.setVisibility(View.VISIBLE);
            factura_btnInscribirContrato.setEnabled(true);
        } else {
            facturas_tvLineaSeparador.setVisibility(View.GONE);
            factura_btnInscribirContrato.setVisibility(View.GONE);
            factura_btnInscribirContrato.setEnabled(true);
        }
        if (facturaResponse.isEstadoPagoFactura()) {
            factura_tvSumarAlPago.setText(R.string.factura_pagada);
            factura_tvSumarAlPago.setTextColor(context.getResources().getColor(R.color.green_factura));
            factura_tvSumarAlPago.setTypeface(Typeface.DEFAULT_BOLD);
            factura_tvValorPagar.setText(R.string.factura_valor_pagado);
        } else {
            factura_tvSumarAlPago.setTextColor(context.getResources().getColor(R.color.grey_dark));
            factura_tvSumarAlPago.setText(R.string.factura_sumar_al_pago);
            factura_tvSumarAlPago.setTypeface(Typeface.DEFAULT);
            factura_tvValorPagar.setText(R.string.factura_valor_a_pagar);
        }
        factura_checkIncluir.setEnabled(!facturaResponse.isEstadoPagoFactura());
        factura_checkIncluir.setChecked((facturaResponse.isEstadoPagoFactura() || facturaResponse.isEstaSeleccionadaParaPago()));

        return customView;
    }

    /**
     * Se pone la información del contrato
     *
     * @param facturaResponse entidad de factura
     */
    private void setInfoFactura(FacturaResponse facturaResponse) {
        factura_tvbNumeroReferente.setText(facturaResponse.getDocumentoReferencia());
        factura_tvbNumeroContrato.setText(facturaResponse.getNumeroContrato());
        factura_tvbPeriodo.setText(formatDateFactura.convertStrinToDate(facturaResponse.getFechaVencimiento(), Constants.FORMATYMD, Constants.FORMATMY));
        factura_tvbPagoSinRecargo.setText(formatDateFactura.convertStrinToDate(facturaResponse.getFechaVencimiento(), Constants.FORMATYMD, Constants.FORMATMD));
        factura_tvbPagoConRecargo.setText(formatDateFactura.convertStrinToDate(facturaResponse.getFechaRecargo(), Constants.FORMATYMD, Constants.FORMATMD));
        factura_tvbValorPagar.setText(formatDateFactura.moneda(facturaResponse.getValorFactura()));
        factura_checkIncluir.setChecked(facturaResponse.isEstadoPagoFactura());
    }

    /**
     * Carga las vistas del adapter
     *
     * @param view
     */
    private void loadViews(View view) {
        factura_tvAlias =  view.findViewById(R.id.factura_tvAlias);
        factura_tvbNumeroContrato =  view.findViewById(R.id.factura_tvbNumeroContrato);
        factura_tvbNumeroReferente =  view.findViewById(R.id.factura_tvbNumeroReferente);
        factura_tvbPeriodo =  view.findViewById(R.id.factura_tvbPeriodo);
        factura_tvbPagoSinRecargo =  view.findViewById(R.id.factura_tvbPagoSinRecargo);
        factura_tvbPagoConRecargo =  view.findViewById(R.id.factura_tvbPagoConRecargo);
        factura_tvValorPagar = view.findViewById(R.id.factura_tvValorPagar);
        factura_tvbValorPagar = view.findViewById(R.id.factura_tvbValorPagar);
        factura_btnInscribirContrato = view.findViewById(R.id.factura_btnInscribirContrato);
        factura_tvSumarAlPago =  view.findViewById(R.id.factura_tvSumarAlPago);
        facturas_tvLineaSeparador = view.findViewById(R.id.facturas_tvLineaSeparador);
        factura_checkIncluir =  view.findViewById(R.id.factura_checkIncluir);
        factura_imgVerPdf =  view.findViewById(R.id.factura_imgPdf);
        factura_btnVerPdf = view.findViewById(R.id.factura_btnVerPdf);
        factura_imgHistorico = view.findViewById(R.id.factura_imgHistorico);
        factura_btnVerHistorico =  view.findViewById(R.id.factura_btnVerHistorico);
        facturas_llDescipcionFactura =  view.findViewById(R.id.facturas_llDescipcionFactura);
        factura_imgVerDetalle = view.findViewById(R.id.factura_imgVerDetalle);
        factura_btnVerDetalle =  view.findViewById(R.id.factura_btnVerDetalle);
    }

    /**
     * Carga los listener
     */

    private void loadListenerToTheControl(final int position) {
        loadListenerToTheControlBtnInscribirContrato(position);
        loadListenerToTheControlImgVerDetalle(position);
        loadListenerToTheControlBtnVerDetalle(position);
        loadListenerToTheControlImgVerPdf(position);
        loadListenerToTheControlBtnVerPdf(position);
        loadListenerToTheControlImgHistorico(position);
        loadListenerToTheControlBtnVerHistorico(position);
        loadListenerToTheControlCheckIncluir(position);
    }

    private void loadListenerToTheControlCheckIncluir(final int position) {
        factura_checkIncluir.setOnClickListener(view -> {
            facturaResponseList.get(position).setEstaSeleccionadaParaPago(!facturaResponseList.get(position).isEstaSeleccionadaParaPago());
            ((FacturasConsultadasActivity) getContext()).validateInternet(position);
            if (facturaResponseList.get(position).isEstaSeleccionadaParaPago()) {
                factura_checkIncluir.setEnabled(false);
                ((FacturasConsultadasActivity) getContext()).consultStatusPendingBill(position, facturaResponseList.get(position).getDocumentoReferencia());
            } else {
                calcularTotalPagar(facturaResponseList);
            }
        });
    }

    private void loadListenerToTheControlBtnVerHistorico(final int position) {
        factura_btnVerHistorico.setOnClickListener(view -> {
            if (!isInvitado) {
                viewHistorico(facturaResponseList.get(position).getNumeroContrato());
            } else {
                showAlertDialogUserWhitOutLogin(context.getResources().getString(R.string.factura_title_login), context.getResources().getString(R.string.factura_ver));
            }
        });
    }

    private void loadListenerToTheControlImgHistorico(final int position) {
        factura_imgHistorico.setOnClickListener(view -> {
            if (!isInvitado) {
                viewHistorico(facturaResponseList.get(position).getNumeroContrato());
            } else {
                showAlertDialogUserWhitOutLogin(context.getResources().getString(R.string.factura_title_login), context.getResources().getString(R.string.factura_ver));
            }
        });
    }

    private void loadListenerToTheControlBtnVerPdf(final int position) {
        factura_btnVerPdf.setOnClickListener(view -> {
            if (!isInvitado) {
                viewPdf(facturaResponseList.get(position).getUrl());
            } else {
                showAlertDialogUserWhitOutLogin(context.getResources().getString(R.string.factura_title_login), context.getResources().getString(R.string.factura_ver));
            }
        });
    }

    private void loadListenerToTheControlImgVerPdf(final int position) {
        factura_imgVerPdf.setOnClickListener(view -> {
            if (!isInvitado) {
                viewPdf(facturaResponseList.get(position).getUrl());
            } else {
                showAlertDialogUserWhitOutLogin(context.getResources().getString(R.string.factura_title_login), context.getResources().getString(R.string.factura_ver));
            }
        });
    }

    private void loadListenerToTheControlBtnVerDetalle(final int position) {
        factura_btnVerDetalle.setOnClickListener(view -> {
            if (!isInvitado) {
                viewDetalle(facturaResponseList.get(position).getNumeroFactura(), formatDateFactura.moneda(facturaResponseList.get(position).getValorFactura()));
            } else {
                showAlertDialogUserWhitOutLogin(context.getResources().getString(R.string.factura_title_login), context.getResources().getString(R.string.factura_ver));
            }
        });
    }

    private void loadListenerToTheControlImgVerDetalle(final int position) {
        factura_imgVerDetalle.setOnClickListener(view -> {
            if (!isInvitado) {
                viewDetalle(facturaResponseList.get(position).getNumeroFactura(), formatDateFactura.moneda(facturaResponseList.get(position).getValorFactura()));
            } else {
                showAlertDialogUserWhitOutLogin(context.getResources().getString(R.string.factura_title_login), context.getResources().getString(R.string.factura_ver));
            }
        });
    }

    private void loadListenerToTheControlBtnInscribirContrato(final int position) {
        factura_btnInscribirContrato.setOnClickListener(view -> {
            if (!isInvitado) {
                inscribirContrato(facturaResponseList.get(position).getNumeroContrato());
            } else {
                showAlertDialogUserWhitOutLogin(context.getResources().getString(R.string.factura_title_login), context.getResources().getString(R.string.factura_message_register_bill));
            }
        });

    }

    /**
     * Visualiza el historico de facturas de un contrato.
     *
     * @param numberContrato
     */
    private void viewHistorico(String numberContrato) {
        if(!controlDoubleTab) {
            Intent intent = new Intent(context, HistoricoActivity.class);
            intent.putExtra(Constants.NUMBER_CONTRATO, numberContrato);
            context.startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
            controlDoubleTab = true;
        }
    }

    /**
     * Visualiza la factura en pdf en el navegador.
     *
     * @param url
     */
    private void viewPdf(String url) {
        if (!url.isEmpty()) {
            Intent intent = new Intent(Intent.ACTION_VIEW);
            intent.setData(Uri.parse(url));
            facturasConsultadasView.openBrowser(true);
            context.startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
            facturasConsultadasView.openBrowser(false);
        }
    }

    private void viewDetalle(String numeroFactura, String valorPagar) {
        if(!controlDoubleTab) {
            Intent intent = new Intent(context, DetalleConsumoActivity.class);
            intent.putExtra(Constants.NUMBER_FACTURA, numeroFactura);
            intent.putExtra(Constants.VALOR_TOTAL_PAGAR, valorPagar);
            context.startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
            controlDoubleTab = true;
        }
    }

    private void inscribirContrato(String numeroContrato) {
        Intent intent = new Intent(context, InscribirContratoActivity.class);
        intent.putExtra(Constants.NUMBER_FACTURA, numeroContrato);
        intent.putExtra(Constants.LISTA_MIS_FACTURAS, (Serializable) facturaResponseList);
        context.startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void calcularTotalPagar(List<FacturaResponse> facturaResponseList) {
        ((FacturasConsultadasActivity) getContext()).valorTotalAPagar(facturaResponseList);
    }

    /**
     * Permite mostrar un popup cuando inscribe un contrato y no esta logueado
     *
     * @param title   titulo de la alerta
     * @param message mensaje de la alerta
     */

    private void showAlertDialogUserWhitOutLogin(String title, String message) {
        AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(context);

        alertDialogBuilder.setTitle(title);
        alertDialogBuilder.setMessage(message);
        alertDialogBuilder.setNegativeButton(context.getResources().getString(R.string.factura_btn_go_to_login), (dialogInterface, i) -> {

            Intent intent = new Intent(context, RegisterLoginActivity.class);
            Bundle bundle = new Bundle();
            bundle.putBoolean(Constants.SHOW_LOGIN, true);
            intent.putExtras(bundle);
            context.startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        });

        alertDialogBuilder.setPositiveButton(context.getResources().getString(R.string.factura_no_gracias), (dialogInterface, i) -> dialogInterface.dismiss());

        AlertDialog alertDialog = alertDialogBuilder.create();
        alertDialog.show();
    }

    public boolean isControlDoubleTab() {
        return controlDoubleTab;
    }

    public void setControlDoubleTab(boolean controlDoubleTab) {
        this.controlDoubleTab = controlDoubleTab;
    }
}