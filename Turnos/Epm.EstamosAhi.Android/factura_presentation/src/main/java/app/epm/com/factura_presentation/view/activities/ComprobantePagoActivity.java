package app.epm.com.factura_presentation.view.activities;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import androidx.annotation.Nullable;
import androidx.appcompat.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;

import com.epm.app.business_models.business_models.Mensaje;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.ComprobantePago;
import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.business_models.FacturasPorTransaccion;
import app.epm.com.facturadomain.business_models.InformacionPSE;
import app.epm.com.facturadomain.business_models.ProcesarInformacionPSE;
import app.epm.com.facturadomain.business_models.TransaccionPSEResponse;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.dependency_injection.DomainModule;
import app.epm.com.factura_presentation.presenters.ComprobantePagoPresenter;
import app.epm.com.factura_presentation.view.views_activities.IComprobantePagoView;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.FormatDateFactura;
import app.epm.com.utilities.helpers.HelperIP;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 11/01/17.
 */

public class ComprobantePagoActivity extends BaseActivity<ComprobantePagoPresenter> implements IComprobantePagoView {

    private Toolbar toolbarApp;
    private TextView comprobantePago_tvPagoExitoso;
    private TextView comprobantePago_tvNumeroComprobante;
    private TextView comprobantePago_tvEnvioCorreo;
    private TextView comprobantePago_tvFechaTransaccion;
    private TextView comprobantePago_tvEntidadBancaria;
    private TextView comprobantePago_tvDireccionIP;
    private TextView comprobantePago_tvValor;
    private TextView comprobantePago_tvEstadoTransaccion;
    private TextView comprobantePago_tvComprobanteCorreo;
    private ImageView comprobantePago_ivComprobanteCorreo;
    private Button comprobantePago_btnOtraConsulta;

    private ICustomSharedPreferences customSharedPreferences;
    private ArrayList<FacturasPorTransaccion> facturasPorTransaccionList;
    private List<FacturaResponse> facturaResponse;
    private LayoutInflater layoutInflater;
    private FormatDateFactura formatDateFactura;
    private String codigoTrazabilidad;
    private String fechaTransaccion;
    private String nombreEntidadFinanciera;
    private String correo;
    private int estadoTransaccion;
    private int valorPagar;
    private int idTransaccion;
    private int entidadFinanciera;
    private InformacionPSE informacionPSE;
    private TransaccionPSEResponse transaccionPSEResponse;
    private boolean statusTransaction;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_comprobante_pago);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        this.layoutInflater = new LayoutInflater(this) {
            @Override
            public LayoutInflater cloneInContext(Context context) {
                return null;
            }
        };
        this.customSharedPreferences = new CustomSharedPreferences(this);
        setPresenter(new ComprobantePagoPresenter(DomainModule.getFacturaBLInstance(this.customSharedPreferences)));
        getPresenter().inject(this, getValidateInternet());
        this.informacionPSE = (InformacionPSE) getIntent().getSerializableExtra(Constants.INFORMACION_PSE);
        this.transaccionPSEResponse = (TransaccionPSEResponse) getIntent().getSerializableExtra(Constants.TRANSACCION_PSE_RESPONSE);
        facturaResponse = (List<FacturaResponse>) getIntent().getSerializableExtra(Constants.LISTA_MIS_FACTURAS_POR_PAGO);
        nombreEntidadFinanciera = getIntent().getStringExtra(Constants.NOMBRE_ENTIDAD_FINANCIERA);
        entidadFinanciera = getIntent().getIntExtra(Constants.ID_ENTIDAD_FINANCIERA, entidadFinanciera);
        estadoTransaccion = informacionPSE.getEstadoTransaccion();
        codigoTrazabilidad = informacionPSE.getCodigoTrazabilidad();
        idTransaccion = informacionPSE.getIdTransaccion();
        facturasPorTransaccionList = informacionPSE.getFacturasPorTransaccion();
        fechaTransaccion = informacionPSE.getFechaTransaccion();
        createProgressDialog();
        loadViews();
        sendDataProcesarInformacionPSE();
    }

    @Override
    public void manageActionFactura() {
        //The method is not used.
    }

    /**
     * Envia datos para procesar información de PSE.
     */
    private void sendDataProcesarInformacionPSE() {
        ProcesarInformacionPSE procesarInformacionPSE = new ProcesarInformacionPSE();
        procesarInformacionPSE.setEstadoTransaccion(estadoTransaccion);
        procesarInformacionPSE.setCodigoTrazabilidad(codigoTrazabilidad);
        procesarInformacionPSE.setEntidadFinanciera(entidadFinanciera);
        procesarInformacionPSE.setIdTransaccion(idTransaccion);
        procesarInformacionPSE.setFacturasPorTransaccion(facturasPorTransaccionList);
        getPresenter().validateInternetProcesarInformacionPSE(procesarInformacionPSE);
    }

    /**
     * Valida si el usuario esta logueado para obtener el correo electronico.
     */
    private void validateUserLogueado() {
        if (getUsuario().isInvitado() == false) {
            correo = getUsuario().getCorreoElectronico();
            sendDataComprobantePago();
        }
    }

    /**
     * Carga los controles de la vista.
     */
    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        comprobantePago_tvPagoExitoso = (TextView) findViewById(R.id.comprobantePago_tvPagoExitoso);
        comprobantePago_tvNumeroComprobante = (TextView) findViewById(R.id.comprobantePago_tvNumeroComprobante);
        comprobantePago_tvEnvioCorreo = (TextView) findViewById(R.id.comprobantePago_tvEnvioCorreo);
        comprobantePago_tvFechaTransaccion = (TextView) findViewById(R.id.comprobantePago_tvFechaTransaccion);
        comprobantePago_tvEntidadBancaria = (TextView) findViewById(R.id.comprobantePago_tvEntidadBancaria);
        comprobantePago_tvDireccionIP = (TextView) findViewById(R.id.comprobantePago_tvDireccionIP);
        comprobantePago_tvValor = (TextView) findViewById(R.id.comprobantePago_tvValor);
        comprobantePago_tvEstadoTransaccion = (TextView) findViewById(R.id.comprobantePago_tvEstadoTransaccion);
        comprobantePago_tvComprobanteCorreo = (TextView) findViewById(R.id.comprobantePago_tvComprobanteCorreo);
        comprobantePago_ivComprobanteCorreo = (ImageView) findViewById(R.id.comprobantePago_ivComprobanteCorreo);
        comprobantePago_btnOtraConsulta = (Button) findViewById(R.id.comprobantePago_btnOtraConsulta);
        loadToolbar();
        loadListenerToTheControls();
    }

    /**
     * Carga los Listener de los controles.
     */
    private void loadListenerToTheControls() {
        loadOnClickListenerToTheControlComprobantePagoTvComprobanteCorreo();
        loadOnClickListenerToTheControlComprobantePagoIvComprobanteCorreo();
        loadOnClickListenerToTheControlComprobantePagoBtnOtraConsulta();
    }

    /**
     * Carga los datos del Comprobante de pago.
     */
    private void loadDataComprobantePago() {
        valorPagar = customSharedPreferences.getInt(Constants.VALOR_TOTAL_PAGAR);
        estadoTransaccion = transaccionPSEResponse.getEstadoTransaccion();
        loadTextsToComprobantePago();
        loadComprobantePagoTvPagoExitoso();
    }

    /**
     * Carga mensaje Somos dependiendo del estado de la transacción.
     */
    private void loadComprobantePagoTvPagoExitoso() {
        if (estadoTransaccion == 4 || estadoTransaccion == 5) {
            statusTransaction = true;
            comprobantePago_tvPagoExitoso.setVisibility(View.VISIBLE);
            comprobantePago_tvPagoExitoso.setText(getResources().getString(R.string.text_pago_exitoso));
        }
    }

    /**
     * Carga OnClick al control comprobantePago_btnOtraConsulta.
     */
    private void loadOnClickListenerToTheControlComprobantePagoBtnOtraConsulta() {
        comprobantePago_btnOtraConsulta.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startConsulta();
            }
        });
    }

    /**
     * Inicia otra consulta validando si esta logueado o no.
     */
    private void startConsulta() {
        if (getUsuario().isInvitado() == false) {
            Intent intent = new Intent(this, FacturasConsultadasActivity.class);
            intent.putExtra(Constants.SHOW_ALERT_RATE, statusTransaction);
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
            startActivityWithOutDoubleClick(intent);
        } else {
            Intent intent = new Intent(this, ConsultFacturaActivity.class);
            intent.putExtra(Constants.TRUE, true);
            intent.putExtra(Constants.SHOW_ALERT_RATE, statusTransaction);
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
            startActivityWithOutDoubleClick(intent);
            finish();
        }
    }

    /**
     * Carga OnClick al control comprobantePago_ivComprobanteCorreo.
     */
    private void loadOnClickListenerToTheControlComprobantePagoIvComprobanteCorreo() {
        comprobantePago_ivComprobanteCorreo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showAlertDialogSendEmailComprobante();
            }
        });
    }

    /**
     * Carga OnClick al control comprobantePago_tvComprobanteCorreo.
     */
    private void loadOnClickListenerToTheControlComprobantePagoTvComprobanteCorreo() {
        comprobantePago_tvComprobanteCorreo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showAlertDialogSendEmailComprobante();
            }
        });
    }

    /**
     * Abre alerta para enviar comprobante de pago al correo.
     */
    private void showAlertDialogSendEmailComprobante() {
        View view = layoutInflater.inflate(R.layout.template_reset_password, null);
        DialogInterface.OnClickListener onClickListenerPositiveButton = getPositiveButtonOnClickListenerResetPassword(view);
        DialogInterface.OnClickListener onClickListenerNegativeButtonCancel = getNegativeButtonCancel();
        getCustomAlertDialog().showAlertDialog(R.string.title_enviar_comprobante, null, false,
                R.string.text_aceptar, onClickListenerPositiveButton, R.string.text_cancelar,
                onClickListenerNegativeButtonCancel, view);
    }

    /**
     * Boton para aceptar el envio del comprobante al correo.
     *
     * @param view view.
     * @return dialogInterface.
     */
    private DialogInterface.OnClickListener getPositiveButtonOnClickListenerResetPassword(View view) {
        final EditText templateSendEmailComprobante = (EditText) view.findViewById(R.id.templateResetPassword_etEmail);
        return new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                correo = templateSendEmailComprobante.getText().toString().trim();
                sendDataComprobantePago();
            }
        };
    }

    /**
     * Envia datos al comprobante de pago.
     */
    private void sendDataComprobantePago() {
        ComprobantePago comprobantePago = new ComprobantePago();
        comprobantePago.setCorreos(correo);
        comprobantePago.setEstadoTransaccion(estadoTransaccion);
        comprobantePago.setValorTotalPago(valorPagar);
        comprobantePago.setIdTransaccion(idTransaccion);
        comprobantePago.setCodigoTrazabilidad(codigoTrazabilidad);
        comprobantePago.setNombreEntidadFinanciera(nombreEntidadFinanciera);
        comprobantePago.setDireccionIp(HelperIP.getIPAddress(true));
        comprobantePago.setFacturasPorTransaccion(facturasPorTransaccionList);
        comprobantePago.setFechaTransaccion(fechaTransaccion);
        getPresenter().validateEmailToSendComprobante(comprobantePago);
    }

    /**
     * Boton para cancelar el envio del comprobante al correo.
     *
     * @return dialogInterface.
     */
    private DialogInterface.OnClickListener getNegativeButtonCancel() {
        return new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                dialogInterface.dismiss();
            }
        };
    }

    /**
     * Carga el Toolbar.
     */
    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar(toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onBackPressed();
            }
        });
    }

    /**
     * Carga los campos que se muestran en el comprobante de pago.
     */
    private void loadTextsToComprobantePago() {
        formatDateFactura = new FormatDateFactura();
        comprobantePago_tvNumeroComprobante.setText("No. (CUS) " + codigoTrazabilidad);
        comprobantePago_tvFechaTransaccion.setText(formatDateFactura.FormatDateddMMyyyyHHmmss(fechaTransaccion, Constants.FORMAT_YMD_HMS, Constants.FORMAT_DMY_HMS));
        comprobantePago_tvEntidadBancaria.setText(nombreEntidadFinanciera);
        comprobantePago_tvDireccionIP.setText(HelperIP.getIPAddress(true));
        comprobantePago_tvValor.setText(formatDateFactura.moneda(valorPagar));
        comprobantePago_tvEstadoTransaccion.setText(transaccionPSEResponse.getNombreEstadoTransaccion());
    }

    /**
     * Carga texto cuando se envía comprobante de pago al correo.
     */
    private void loadTextSendComprobantePago() {
        comprobantePago_tvEnvioCorreo.setText(R.string.text_correo_comprobante_enviado);
    }

    /**
     * Alerta que se muestra cuando el comprobante de pago se ha enviado al correo.
     *
     * @param mensaje mensaje.
     */
    private void alertDialogSendComprobante(final Mensaje mensaje) {
        runOnUiThread(() -> {
            DialogInterface.OnClickListener onClickListenerNegativeButtonCancel = getNegativeButtonCancel();
            getCustomAlertDialog().showAlertDialog(getUsuario().getNombres(), mensaje.getText(), false, R.string.text_aceptar,
                    onClickListenerNegativeButtonCancel, null);
        });
    }

    @Override
    public void showAlertDialogSendComprobantePago(final Mensaje mensaje) {
        runOnUiThread(() -> {
            alertDialogSendComprobante(mensaje);
            loadTextSendComprobantePago();
        });
    }

    @Override
    public void saveTransaccionPSEResponse(TransaccionPSEResponse transaccionPSEResponse) {
        this.transaccionPSEResponse = transaccionPSEResponse;
        runOnUiThread(() -> {
            loadDataComprobantePago();
            validateUserLogueado();
        });
    }

    @Override
    public void showAlertDialogGeneralLoadAgain(final int titleError, final String message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(titleError, message, false, R.string.text_intentar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                sendDataProcesarInformacionPSE();
            }
        }, null));
    }

    @Override
    public void showAlertDialogGeneralLoadAgain(final String name, final int text_validate_internet) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(name, text_validate_internet, false, R.string.text_intentar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                sendDataProcesarInformacionPSE();
            }
        }, null));
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        startConsulta();
    }
}