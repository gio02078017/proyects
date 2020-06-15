package app.epm.com.reporte_danios_presentation.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import androidx.appcompat.widget.Toolbar;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.engine.DiskCacheStrategy;
import com.esri.arcgisruntime.mapping.ArcGISMap;
import com.esri.arcgisruntime.mapping.Basemap;
import com.esri.arcgisruntime.mapping.view.MapView;

import java.util.ArrayList;

import app.epm.com.reporte_danios_domain.danios.business_models.ReportDanio;
import app.epm.com.reporte_danios_presentation.R;
import app.epm.com.reporte_danios_presentation.dependency_injection.DomainModule;
import app.epm.com.reporte_danios_presentation.presenters.ReporteDanioPresenter;
import app.epm.com.reporte_danios_presentation.view.views_activities.IReporteDanioView;
import app.epm.com.utilities.helpers.FileManager;
import app.epm.com.utilities.helpers.IFileManager;
import app.epm.com.utilities.services.ServicesArcGIS;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

public class VerResumenDanioActivity extends BaseActivity<ReporteDanioPresenter> implements IReporteDanioView {
    private Toolbar toolbarApp;
    private ReportDanio reportDanio;
    private TextView see_resume_etTipoDanio;
    private TextView see_resume_etDescripcion;
    private TextView see_resume_etLugar;
    private TextView see_resume_etTelefono;
    private TextView see_resume_etNombre;
    private ArrayList<String> listFilesAttach;
    private LinearLayout see_resume_ivPhoto;
    private LinearLayout see_resume_danio_llPhoto;
    private ServicesArcGIS servicesArcGIS;
    private IFileManager fileManager;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ver_resumen_danio);
        loadDrawerLayout(R.id.generalDrawerLayout);
        setPresenter(new ReporteDanioPresenter(DomainModule.getDaniosBLInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        this.fileManager = new FileManager(this);
        createProgressDialog();
        getDataIntent();
        loadViews();
        setViewData();
        loadServicesArcGIS(reportDanio.getUrlServicio());
    }

    private void getDataIntent() {
        Intent intent = getIntent();
        reportDanio = (ReportDanio) intent.getSerializableExtra(Constants.REPORT_DANIOS);
        setListFilesAttach(reportDanio.getArrayFiles());
    }

    private void loadViews() {
        toolbarApp = findViewById(R.id.toolbar);
        see_resume_etTipoDanio = findViewById(R.id.see_resume_etTipoDanio);
        see_resume_etDescripcion =  findViewById(R.id.see_resume_etDescripcion);
        see_resume_etLugar =  findViewById(R.id.see_resume_etLugar);
        see_resume_etTelefono = findViewById(R.id.see_resume_etTelefono);
        see_resume_etNombre =  findViewById(R.id.see_resume_etNombre);
        see_resume_ivPhoto =  findViewById(R.id.see_resume_ivPhoto);
        see_resume_danio_llPhoto =  findViewById(R.id.see_resume_danio_llPhoto);
        Button see_resume_danio_sendReport = findViewById(R.id.see_resume_btnSendMyReport);
        see_resume_danio_sendReport.setOnClickListener(v -> sendReportDanio());
        loadToolbar();
        loadInfo();
    }

    private void sendReportDanio() {
        if (fileManager.verifyMaxMegaBytesArrayFiles(10, getListFilesAttach())) {
            getPresenter().validateInternetToSendReportDanio(this.reportDanio, this.servicesArcGIS, this);
        } else {
            getCustomAlertDialog().showAlertDialog(getName(), getResources().getString(app.epm.com.utilities.R.string.not_attach_more_imagenes_size)
                    + 10 + " MB.", false, app.epm.com.utilities.R.string.text_aceptar, (dialogInterface, i) -> dialogInterface.dismiss(), null);
        }
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(view -> onBackPressed());

    }

    private void loadInfo() {
        see_resume_etTipoDanio.setText(reportDanio.getTypeReporte());
        see_resume_etDescripcion.setText(reportDanio.getDescribeReport());
        see_resume_etLugar.setText(reportDanio.getLugarReferencia());
        see_resume_etTelefono.setText(reportDanio.getTelephoneUserReport());
        see_resume_etNombre.setText(reportDanio.getUserName());
    }

    private void setViewData() {
        if (getListFilesAttach() != null && getListFilesAttach().size() > 0) {
            see_resume_danio_llPhoto.setVisibility(View.VISIBLE);
            for (String fileName : getListFilesAttach()) {
                ImageView imageView = fileManager.getImageViewInstance();
                if (fileName.contains(Constants.SUFFIX_FILE_AUDIO)) {
                    imageView.setAdjustViewBounds(true);
                    Glide.with(this).load(fileName).thumbnail(1f).fitCenter().
                            placeholder(R.mipmap.icon_file_audio_attach).diskCacheStrategy(DiskCacheStrategy.ALL).into(imageView);
                } else {
                    Glide.with(this).load(fileName).thumbnail(1f).fitCenter().diskCacheStrategy(DiskCacheStrategy.ALL).into(imageView);
                }
                see_resume_ivPhoto.addView(imageView);
            }
        }
    }


    private void loadServicesArcGIS(String url) {
        MapView base_mapView = new MapView(this);
        ArcGISMap map = new ArcGISMap(Basemap.Type.OPEN_STREET_MAP, 6.234703, -75.5514745, 12);
        servicesArcGIS = new ServicesArcGIS(url,map,this);
        base_mapView.setMap(map);
    }

    @Override
    public void showAlertDialogSendReportDanioSuccessful(String numberFiled) {
        final String message = String.format("%s %s. %s", getString(R.string.message_send_danios_sucessful_1),
                numberFiled, getString(R.string.message_send_danios_sucessful_2));
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(R.string.title_send_danios_successful, message, false, R.string.text_aceptar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                getCustomAlertDialog().showAlertDialog(getName(), R.string.message_send_danios_continue_report, false,
                        R.string.button_yes, (dialogInterface1, i1) -> {
                            setResult(Constants.SERVICES_DANIO);
                            dialogInterface1.dismiss();
                            finish();
                        }, R.string.button_no, (dialogInterface12, i12) -> {
                            try {
                                Class clazz = Class.forName(getCustomSharedPreferences().getString(Constants.LANDING_CLASS));
                                Intent intent = new Intent(VerResumenDanioActivity.this, clazz);
                                intent.putExtra(Constants.CALLED_FROM_ANOTHER_MODULE, true);
                                intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                startActivityWithOutDoubleClick(intent);
                            } catch (ClassNotFoundException e) {
                                Log.e("Exception", e.toString());
                            }
                        }, null);
            }
        }, null));
    }


    public ArrayList<String> getListFilesAttach() {
        return listFilesAttach;
    }

    public void setListFilesAttach(ArrayList<String> listFilesAttach) {
        this.listFilesAttach = listFilesAttach;
    }
}
