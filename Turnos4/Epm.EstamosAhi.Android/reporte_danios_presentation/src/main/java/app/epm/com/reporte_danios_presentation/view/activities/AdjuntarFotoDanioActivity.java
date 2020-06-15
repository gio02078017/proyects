package app.epm.com.reporte_danios_presentation.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.ImageView;

import java.util.ArrayList;

import com.epm.app.app_utilities_presentation.utils.LoadIconServices;
import com.epm.app.app_utilities_presentation.views.activities.AttachActivity;
import com.epm.app.business_models.business_models.ETipoServicio;
import com.esri.arcgisruntime.mapping.ArcGISMap;
import com.esri.arcgisruntime.mapping.Basemap;
import com.esri.arcgisruntime.mapping.view.MapView;

import app.epm.com.reporte_danios_domain.danios.business_models.ReportDanio;
import app.epm.com.reporte_danios_presentation.R;
import app.epm.com.reporte_danios_presentation.dependency_injection.DomainModule;
import app.epm.com.reporte_danios_presentation.presenters.ReporteDanioPresenter;
import app.epm.com.reporte_danios_presentation.view.views_activities.IReporteDanioView;
import app.epm.com.utilities.helpers.FileManager;
import app.epm.com.utilities.helpers.IFileManager;
import app.epm.com.utilities.services.ServicesArcGIS;
import app.epm.com.utilities.utils.Constants;

public class AdjuntarFotoDanioActivity extends AttachActivity<ReporteDanioPresenter> implements IReporteDanioView {
    private ReportDanio reportDanio;
    private ETipoServicio tipoServicio;
    private ServicesArcGIS servicesArcGIS;
    private IFileManager fileManager;

    public AdjuntarFotoDanioActivity() {
        super(Constants.REPORT_DANIOS, 5, 10);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setPresenter(new ReporteDanioPresenter(DomainModule.getDaniosBLInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        createProgressDialog();
        getDataIntent();
        loadIconServices();
        loadServicesArcGIS(reportDanio.getUrlServicio());
        this.fileManager = new FileManager(this);
    }

    private void loadIconServices() {
        ImageView attach_ivServicio = (ImageView) findViewById(R.id.attach_ivServicio);
        LoadIconServices loadIconServices = new LoadIconServices();
        attach_ivServicio.setImageResource(loadIconServices.setIdIconoServicio(this.tipoServicio.getName()));
    }

    @Override
    public void showResume(ArrayList<String> filesNames) {
        Intent intent = new Intent(this, VerResumenDanioActivity.class);
        this.reportDanio.setArrayFiles(filesNames);
        intent.putExtra(Constants.REPORT_DANIOS, this.reportDanio);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    @Override
    public void clickButton(ArrayList<String> filesNames) {
        if (fileManager.verifyMaxMegaBytesArrayFiles(10, filesNames)) {
            this.reportDanio.setArrayFiles(filesNames);
            getPresenter().validateInternetToSendReportDanio(this.reportDanio, this.servicesArcGIS, this);
        } else {
            getCustomAlertDialog().showAlertDialog(getName(), getResources().getString(app.epm.com.utilities.R.string.not_attach_more_imagenes_size) + 10 + " MB.",
                    false, app.epm.com.utilities.R.string.text_aceptar, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            dialog.dismiss();
                        }
                    }, null);
        }
    }

    @Override
    public void onBackPressed(ArrayList<String> filesNames) {
        this.reportDanio.setArrayFiles(filesNames);
        Intent intent = new Intent();
        intent.putExtra(Constants.REPORT_DANIOS, this.reportDanio);
        setResultData(Constants.ATTACH, intent);
        finish();
    }

    @Override
    public String getToolBarNameModule() {
        return getString(R.string.title_reporte_de_danios);
    }

    @Override
    public String getTextViewTitleAttach() {
        return getString(R.string.text_describe_adjuntos_danios);
    }

    @Override
    public String getTextViewDescribeAttach() {
        return getString(R.string.message_image_attach_danios);
    }

    @Override
    public String getButtonTextAttach() {
        return getString(R.string.text_button_adjuntos_danios);
    }

    @Override
    public int getImageViewLine() {
        return R.mipmap.step_three;
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == Constants.SERVICES_DANIO) {
            setResult(Constants.SERVICES_DANIO);
            finish();
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    @Override
    public void showAlertDialogSendReportDanioSuccessful(String numberFiled) {
        final String message = String.format("%s %s. %s", getString(R.string.message_send_danios_sucessful_1),
                numberFiled, getString(R.string.message_send_danios_sucessful_2));
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(R.string.title_send_danios_successful, message,
                false, R.string.text_aceptar, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        getCustomAlertDialog().showAlertDialog(getName(), R.string.message_send_danios_continue_report, false,
                                R.string.button_yes, new DialogInterface.OnClickListener() {
                                    @Override
                                    public void onClick(DialogInterface dialogInterface, int i) {
                                        startServiciosDanioActivity();
                                        dialogInterface.dismiss();
                                    }
                                }, R.string.button_no, new DialogInterface.OnClickListener() {
                                    @Override
                                    public void onClick(DialogInterface dialogInterface, int i) {
                                        try {
                                            Class clazz = Class.forName(getCustomSharedPreferences().getString(Constants.LANDING_CLASS));
                                            Intent intent = new Intent(AdjuntarFotoDanioActivity.this, clazz);
                                            intent.putExtra(Constants.CALLED_FROM_ANOTHER_MODULE, true);
                                            intent.putExtra(Constants.SHOW_ALERT_RATE, Constants.RATE_APP);
                                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
                                        } catch (ClassNotFoundException e) {
                                            Log.e("Exception", e.toString());
                                        }
                                    }
                                }, null);
                    }
                }, null));
    }

    private void startServiciosDanioActivity() {
        Intent intent = new Intent(this, ServiciosDanioActivity.class);
        intent.putExtra(Constants.SHOW_ALERT_RATE, Constants.RATE_APP);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void getDataIntent() {
        this.reportDanio = (ReportDanio) getIntent().getSerializableExtra(Constants.REPORT_DANIOS);
        this.tipoServicio = reportDanio.getTipoServicio();
        if (reportDanio.getArrayFiles() != null) {
            this.setArrayFiles(reportDanio.getArrayFiles());
        }
    }

    private void loadServicesArcGIS(String url) {
        MapView base_mapView = new MapView(this);
        ArcGISMap map = new ArcGISMap(Basemap.Type.OPEN_STREET_MAP, 6.234703, -75.5514745, 12);
        servicesArcGIS = new ServicesArcGIS(url,map,this);
        base_mapView.setMap(map);
    }
}