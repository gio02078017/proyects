package app.epm.com.contacto_transparente_presentation.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.widget.ImageView;

import com.epm.app.app_utilities_presentation.views.activities.AttachActivity;

import java.util.ArrayList;

import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_domain.business_models.ItemUI;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.contacto_transparente_presentation.dependency_injection.DomainModule;
import app.epm.com.contacto_transparente_presentation.presenters.AttachEvidencePresenter;
import app.epm.com.contacto_transparente_presentation.view.views_activities.IAttachEvidenceView;
import app.epm.com.utilities.helpers.FileManager;
import app.epm.com.utilities.helpers.IFileManager;
import app.epm.com.utilities.utils.Constants;


/**
 * Created by leidycarolinazuluagabastidas on 15/03/17.
 */

public class AttachEvidenceActivity extends AttachActivity<AttachEvidencePresenter> implements IAttachEvidenceView {

    private Incidente incident;
    private ImageView describe_ivServicio;
    private ImageView attach_ivLine;
    private ArrayList<String> files;
    private IFileManager fileManager;

    public AttachEvidenceActivity() {
        super(Constants.CONTACTO, 5, 10);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        IFileManager fileManager = new FileManager(this);
        setPresenter(new AttachEvidencePresenter(DomainModule.getContactoTransparenteBusinessLogicInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet(), fileManager);
        createProgressDialog();
        loadViewsAttach();
        incident = (Incidente) getIntent().getSerializableExtra(Constants.INCIDENT);
        getAttachPrint();
        this.fileManager = new FileManager(this);
    }

    private void getAttachPrint() {
        ArrayList<String> temp = getIntent().getStringArrayListExtra(Constants.ATTACHLIST);
        if (temp != null) {
            setArrayFiles(temp);
        }
    }

    private void loadViewsAttach() {
        describe_ivServicio = (ImageView) findViewById(R.id.attach_ivServicio);
        attach_ivLine = (ImageView) findViewById(R.id.attach_ivLine);
        describe_ivServicio.setImageResource(R.mipmap.icon_transparent_contact_internal);
        attach_ivLine.setImageResource(R.mipmap.image_step_02);
    }

    @Override
    public void showResume(ArrayList<String> filesNames) {
        //The method is not used.
    }

    @Override
    public void clickButton(ArrayList<String> filesNames) {
        if (fileManager.verifyMaxMegaBytesArrayFiles(10, filesNames)) {
            files = filesNames;
            getPresenter().validateInternetToSendIncident(setParameterEmptyToAnonymous(incident));
        } else {
            getCustomAlertDialog().showAlertDialog(getName(), getResources().getString(app.epm.com.utilities.R.string.not_attach_more_imagenes_size) + 10 + " MB.", false, app.epm.com.utilities.R.string.text_aceptar, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialogInterface, int i) {
                    dialogInterface.dismiss();
                }
            }, null);
        }
    }

    private Incidente setParameterEmptyToAnonymous(Incidente incident) {
        if (incident.isAnonimo()) {
            incident.setNombreContacto(Constants.EMPTY_STRING);
            incident.setTelefonoContacto(Constants.EMPTY_STRING);
            incident.setCorreoElectronicoContacto(Constants.EMPTY_STRING);
            incident.setNombreGrupoInteres(null);
        }
        return incident;
    }

    @Override
    public void onBackPressed(ArrayList<String> filesNames) {
        Intent intent = new Intent();
        intent.putStringArrayListExtra(Constants.ATTACHLIST, filesNames);
        setResultData(Constants.ATTACH, intent);
        finish();
    }

    @Override
    public String getToolBarNameModule() {
        return getString(R.string.title_contacto_transparente);
    }

    @Override
    public String getTextViewTitleAttach() {
        return getString(R.string.title_attach);
    }

    @Override
    public String getTextViewDescribeAttach() {
        return getString(R.string.messagje_image_attach_contacto);
    }

    @Override
    public String getButtonTextAttach() {
        return getString(R.string.text_button_attach_contacto);
    }

    @Override
    public int getImageViewLine() {
        return 0;
    }

    @Override
    public void sendAttach(final ItemUI itemUI) {
        if (files.size() > 0) {
            getPresenter().changeNameFilesToSend(files, itemUI);
        } else {
            runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(R.string.title_register_successful, itemUI.getValor(), false, R.string.text_aceptar, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialogInterface, int i) {
                    startHomeOfContactoTransparente();
                }
            }, null));
        }
    }

    @Override
    public void showAlertDialogToGoHome(final int title, final ItemUI itemUI) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, itemUI.getValor(), false, R.string.text_aceptar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                startHomeOfContactoTransparente();
            }
        }, null));
    }

    private void startHomeOfContactoTransparente() {
        Intent intent = new Intent(AttachEvidenceActivity.this, HomeContactoTransparenteActivity.class);
        intent.putExtra(Constants.SHOW_ALERT_RATE, Constants.RATE_APP);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        intent.putExtra(Constants.ALERT_HOME, true);
        startActivityWithOutDoubleClick(intent);
    }
}