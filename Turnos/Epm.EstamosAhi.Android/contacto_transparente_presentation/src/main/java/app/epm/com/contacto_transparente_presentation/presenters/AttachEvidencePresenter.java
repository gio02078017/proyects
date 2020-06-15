package app.epm.com.contacto_transparente_presentation.presenters;

import android.util.Log;

import com.epm.app.business_models.business_models.RepositoryError;

import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import app.epm.com.contacto_transparente_domain.business_models.Evidencia;
import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_domain.business_models.ItemUI;
import app.epm.com.contacto_transparente_domain.business_models.ParametrosEvidencia;
import app.epm.com.contacto_transparente_domain.contacto_transparente.ContactoTransparenteBusinessLogic;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.contacto_transparente_presentation.view.views_activities.IAttachEvidenceView;
import app.epm.com.utilities.helpers.IFileManager;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 15/03/17.
 */

public class AttachEvidencePresenter extends BasePresenter<IAttachEvidenceView> {

    private ContactoTransparenteBusinessLogic contactoTransparenteBusinessLogic;
    private IFileManager fileManager;

    public AttachEvidencePresenter(ContactoTransparenteBusinessLogic contactoTransparenteBusinessLogic) {
        this.contactoTransparenteBusinessLogic = contactoTransparenteBusinessLogic;
    }

    public void inject(IAttachEvidenceView attachEvidenceView, IValidateInternet validateInternet, IFileManager fileManager) {
        setView(attachEvidenceView);
        setValidateInternet(validateInternet);
        this.fileManager = fileManager;
    }

    public void validateInternetToSendIncident(Incidente incident) {
        if (getValidateInternet().isConnected()) {
            createThreadToSendIncident(incident);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    public void createThreadToSendIncident(final Incidente incident) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> sendIncident(incident));
        thread.start();
    }

    public void sendIncident(Incidente incident) {

        try {
            ItemUI itemUI = contactoTransparenteBusinessLogic.sendIncident(incident);
            getView().sendAttach(itemUI);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
            }
            getView().dismissProgressDialog();
        }
    }


    public void changeNameFilesToSend(ArrayList<String> files, ItemUI itemUI) {
        ParametrosEvidencia evidenceParameters = new ParametrosEvidencia();
        evidenceParameters.setCodigoDelActoIndebido(itemUI.getLlave());
        ArrayList<Evidencia> evidenceList = new ArrayList<>();

        for (String filePath : files) {

            Evidencia evidence = new Evidencia();
            String timeStamp = new SimpleDateFormat(Constants.FORMAT_DATE_ATTACH).format(new Date());
            String file = fileManager.getExtensionFile(filePath);
            evidence.setNombreDelArchivo(changeFileName(file, timeStamp));
            try {
                evidence.setArchivo(fileManager.fileToBytesBase64(filePath));

            } catch (IOException e) {
                Log.e("Exception", e.toString());
            } catch (Exception e) {
                Log.e("Exception", e.toString());
            }
            evidenceList.add(evidence);
        }
        evidenceParameters.setEvidenciasDelActoIndebido(evidenceList);
        validateInternetToSendEvidenceFiles(evidenceParameters, itemUI);
    }

    public String changeFileName(String extension, String timeStamp) {

        String fileNameChanged;

        if (extension.equals(Constants.MP3)) {
            extension = Constants.AUDIO.concat(timeStamp).concat(".").concat(extension);
        } else {
            extension = Constants.IMAGE.concat(timeStamp).concat(".").concat(extension);
        }
        return extension;
    }

    public void validateInternetToSendEvidenceFiles(ParametrosEvidencia evidenceParameter, ItemUI itemUI) {
        if (getValidateInternet().isConnected()) {
            createThreadToSendEvidenceFiles(evidenceParameter, itemUI);
        }
    }

    public void createThreadToSendEvidenceFiles(final ParametrosEvidencia evidenceParameter, final ItemUI itemUI) {
        Thread thread = new Thread(() -> sendEvidenceFiles(evidenceParameter, itemUI));
        thread.start();
    }

    public void sendEvidenceFiles(ParametrosEvidencia evidenceParameter, ItemUI itemUI) {
        try {
            getView().showAlertDialogToGoHome(R.string.title_register_successful, itemUI);
            contactoTransparenteBusinessLogic.sendEvidenceFiles(evidenceParameter);
        } catch (RepositoryError repositoryError) {

        } finally {
            getView().dismissProgressDialog();
        }
    }

}
