package app.epm.com.reporte_fraudes_presentation.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.widget.ImageView;

import com.epm.app.app_utilities_presentation.utils.LoadIconServices;
import com.epm.app.app_utilities_presentation.views.activities.AttachActivity;
import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.Mapa;

import java.util.ArrayList;

import app.epm.com.reporte_fraudes_domain.business_models.ReporteDeFraude;
import app.epm.com.reporte_fraudes_presentation.R;
import app.epm.com.utilities.helpers.FileManager;
import app.epm.com.utilities.helpers.IFileManager;
import app.epm.com.utilities.utils.Constants;


public class AttachEvidenceDeFraudesActivity extends AttachActivity {

    private ETipoServicio tipoServicio;
    private Mapa map;
    private boolean isAnonymous;
    private ReporteDeFraude reporteDeFraude;
    private String address;

    public AttachEvidenceDeFraudesActivity() {
        super(Constants.REPORT_FRAUDES, 5, 10);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getDataIntent();
        loadIconServices();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == Constants.REPORTE_FRAUDE) {
            isAnonymous = data.getBooleanExtra(Constants.STATE_ANONYMOUS, isAnonymous);
            reporteDeFraude = (ReporteDeFraude) data.getSerializableExtra(Constants.REPORT_FRAUDE);
            address = data.getStringExtra(Constants.ADDRESS_FRAUDE);
            setAnonymous(isAnonymous);
        } else if (resultCode == Constants.SERVICIOS_FRAUDE) {
            setResult(Constants.SERVICIOS_FRAUDE);
            finish();
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    @Override
    public void showResume(ArrayList filesNames) {
        //The method is not used.
    }

    @Override
    public void clickButton(ArrayList filesNames) {
        IFileManager fileManager = new FileManager(this);
        if (fileManager.verifyMaxMegaBytesArrayFiles(10, filesNames)) {
            Intent intent = new Intent(this, UbicacionDeFraudeActivity.class);
            intent.putExtra(Constants.SERVICES, this.map);
            intent.putExtra(Constants.STATE_ANONYMOUS, this.getAnonymous());
            intent.putExtra(Constants.TIPO_SERVICIO, this.tipoServicio);
            intent.putExtra(Constants.REPORT_FRAUDE, reporteDeFraude);
            intent.putExtra(Constants.ADDRESS_FRAUDE, address);
            intent.putStringArrayListExtra(Constants.ATTACHLIST, filesNames);
            startActivityWithOutDoubleClick(intent);
        } else {
            getCustomAlertDialog().showAlertDialog(getName(), getResources().getString(R.string.not_attach_more_imagenes_size) + 10 + " MB.",
                    false, app.epm.com.utilities.R.string.text_aceptar, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            dialog.dismiss();
                        }
                    }, null);
        }
    }

    @Override
    public void onBackPressed(ArrayList filesNames) {
        setResultData(Constants.ALERT);
        finish();
    }

    @Override
    public String getToolBarNameModule() {
        return getString(R.string.title_fraude);
    }

    @Override
    public String getTextViewTitleAttach() {
        return getString(R.string.attach_fraudes_text_title_attach);
    }

    @Override
    public String getTextViewDescribeAttach() {
        return getString(R.string.attach_fraudes_message_describe);
    }

    @Override
    public String getButtonTextAttach() {
        return getString(R.string.attach_fraudes_text_button);
    }

    @Override
    public int getImageViewLine() {
        return R.mipmap.step_one;
    }

    private void getDataIntent() {
        this.map = (Mapa) getIntent().getSerializableExtra(Constants.SERVICES);
        this.tipoServicio = (ETipoServicio) getIntent().getSerializableExtra(Constants.TIPO_SERVICIO);
        isAnonymous = getIntent().getBooleanExtra(Constants.STATE_ANONYMOUS, isAnonymous);
        setAnonymous(isAnonymous);
    }

    private void loadIconServices() {
        ImageView describe_ivServicio = (ImageView) findViewById(R.id.attach_ivServicio);
        LoadIconServices loadIconServices = new LoadIconServices();
        describe_ivServicio.setImageResource(loadIconServices.setIdIconoServicio(this.tipoServicio.getName()));
    }
}