package com.epm.app.view.activities;

import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import androidx.annotation.Nullable;
import android.util.Base64;
import android.widget.ImageView;
import android.widget.TextView;

import com.epm.app.R;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

import app.epm.com.contacto_transparente_presentation.view.activities.HomeContactoTransparenteActivity;
import app.epm.com.factura_presentation.view.activities.ConsultFacturaActivity;
import app.epm.com.factura_presentation.view.activities.FacturasConsultadasActivity;
import app.epm.com.reporte_danios_presentation.view.activities.ServiciosDanioActivity;
import app.epm.com.reporte_fraudes_presentation.view.activities.ServicesDeFraudesActivity;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by mateoquicenososa on 3/5/18.
 */

public class EspacioPromocionalActivity extends BaseActivity {

    private ImageView espacioPromocional_imageView;
    private TextView espacioPromocional_textViewOmitir;

    private String date;
    private String dateWithoutHours;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_espacio_promocional);
        loadViews();
    }

    private void loadViews() {
        espacioPromocional_imageView =  findViewById(R.id.espacioPromocional_imageView);
        espacioPromocional_textViewOmitir =  findViewById(R.id.espacioPromocional_textViewOmitir);
        loadListenersToTheControls();
        loadDate();
    }

    private void loadListenersToTheControls() {
        loadImageToEspacioPromocional();
        loadOnClickListenerToTheControlEspacioPromocionalImageView();
        loadOnClickListenerToTheControlEspacioPromocionalTextViewOmitir();
    }

    private void loadDate() {
        date = new SimpleDateFormat(Constants.FORMAT_DATE_YMDHMS).format(new Date());
        dateWithoutHours = new SimpleDateFormat(Constants.FORMATYMD).format(new Date());
        getCustomSharedPreferences().addString(Constants.DATE_WITHOUT_HOURS, dateWithoutHours);

        if (getCustomSharedPreferences().getInt(Constants.OPEN_PROMOTIONAL_SPACE) == Constants.TWO) {
            SimpleDateFormat dateFormat = new SimpleDateFormat(Constants.FORMAT_DATE_YMDHMS);
            Calendar calendar = Calendar.getInstance();
            calendar.add(Calendar.DATE, Constants.SEVEN);
            date = dateFormat.format(calendar.getTime());
            getCustomSharedPreferences().addString(Constants.DATE_ACTIVITY_PROMOTIONAL_SPACE, date);
        }
    }

    private void loadImageToEspacioPromocional() {
        String imagen = getCustomSharedPreferences().getString(Constants.IMAGE_PROMOTIONAL_SPACE);
        byte[] decodedString = Base64.decode(imagen, Base64.DEFAULT);
        Bitmap decodedByte = BitmapFactory.decodeByteArray(decodedString, Constants.ZERO, decodedString.length);
        Drawable drawable = new BitmapDrawable(decodedByte);
        espacioPromocional_imageView.setBackground(drawable);
    }

    private void loadOnClickListenerToTheControlEspacioPromocionalImageView() {
        espacioPromocional_imageView.setOnClickListener(v -> openActivityFromEspacioPromocional());
    }

    private void loadOnClickListenerToTheControlEspacioPromocionalTextViewOmitir() {
        espacioPromocional_textViewOmitir.setOnClickListener(v -> loadOmitir());
    }

    private void openActivityFromEspacioPromocional() {
        int nameModule = getCustomSharedPreferences().getInt(Constants.NAME_MODULE_PROMOTIONAL_SPACE);
        Intent intent;
        switch (nameModule) {
            case Constants.NOT_MODULE:
                break;
            case Constants.MODULE_CODE_NOTICIAS:
                intent = new Intent(this, NoticiasActivity.class);
                startActivityFromEspacioPromocional(intent);
                break;
            case Constants.MODULE_CODE_LINEAS_DE_ATENCION:
                intent = new Intent(this, LineasDeAtencionActivity.class);
                startActivityFromEspacioPromocional(intent);
                break;
            case Constants.MODULE_CODE_OFICINAS:
                intent = new Intent(this, OficinasDeAtencionActivity.class);
                startActivityFromEspacioPromocional(intent);
                break;
            case Constants.MODULE_CODE_REPORTE_FRAUDES:
                intent = new Intent(this, ServicesDeFraudesActivity.class);
                startActivityFromEspacioPromocional(intent);
                break;
            case Constants.MODULE_CODE_FACTURA:
                if (getUsuario().isInvitado() == false) {
                    sendReportToGoogleAnalytics(Constants.FACTURA_REGISTRADO);
                    intent = new Intent(this, FacturasConsultadasActivity.class);
                } else {
                    sendReportToGoogleAnalytics(Constants.FACTURA_INVITADO);
                    intent = new Intent(this, ConsultFacturaActivity.class);
                    intent.putExtra(Constants.TRUE, true);
                }
                startActivityFromEspacioPromocional(intent);
                break;
            case Constants.MODULE_CODE_CONTACTO_TRANSPARENTE:
                if (getUsuario().isInvitado() == false) {
                    sendReportToGoogleAnalytics(Constants.CONTACTO_TRASPARENTE_REGISTRADO);
                } else {
                    sendReportToGoogleAnalytics(Constants.CONTACTO_TRASPARENTE_INVITADO);
                }
                intent = new Intent(this, HomeContactoTransparenteActivity.class);
                startActivityFromEspacioPromocional(intent);
                break;
            case Constants.MODULE_CODE_REPORTE_DANIOS:
                if (getUsuario().isInvitado() == false) {
                    sendReportToGoogleAnalytics(Constants.DANIOS_REGISTRADO);
                } else {
                    sendReportToGoogleAnalytics(Constants.DANIOS_INVITADO);
                }
                intent = new Intent(this, ServiciosDanioActivity.class);
                startActivityFromEspacioPromocional(intent);
                break;
            case Constants.MODULE_CODE_EVENTOS:
                intent = new Intent(this, EventosActivity.class);
                startActivityFromEspacioPromocional(intent);
                break;
            case Constants.MODULE_CODE_ESTACIONES_DE_GAS:
                intent = new Intent(this, EstacionesDeGasActivity.class);
                startActivityFromEspacioPromocional(intent);
                break;
            default:
                break;
        }
    }

    private void startActivityFromEspacioPromocional(Intent intent) {
        startActivityWithOutDoubleClick(intent);
        loadOmitir();
    }

    private void loadOmitir() {
        finish();
    }

    @Override
    public void onBackPressed() {
        loadOmitir();
    }

    @Override
    protected void onDestroy() {
        loadOmitir();
        super.onDestroy();
    }
}