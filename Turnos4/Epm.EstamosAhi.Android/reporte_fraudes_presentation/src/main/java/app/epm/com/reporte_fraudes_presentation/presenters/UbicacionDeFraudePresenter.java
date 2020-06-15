package app.epm.com.reporte_fraudes_presentation.presenters;

import com.epm.app.app_utilities_presentation.presenters.BaseUbicacionDeFraudeOrDanioPresenter;
import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;

import java.text.Normalizer;
import java.util.List;

import app.epm.com.reporte_fraudes_domain.reporte_fraudes.ReporteDeFraudesBusinessLogic;
import app.epm.com.reporte_fraudes_presentation.R;
import app.epm.com.reporte_fraudes_presentation.view.views_activities.IUbicacionDeFraudeView;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 19/04/17.
 */

public class UbicacionDeFraudePresenter extends BaseUbicacionDeFraudeOrDanioPresenter<IUbicacionDeFraudeView> {

    ReporteDeFraudesBusinessLogic reporteDeFraudesBusinessLogic;
    private List<String> listMunicipialities;

    public UbicacionDeFraudePresenter(ReporteDeFraudesBusinessLogic reporteDeFraudesBusinessLogic) {
        super(reporteDeFraudesBusinessLogic);
        this.reporteDeFraudesBusinessLogic = reporteDeFraudesBusinessLogic;
    }

    public void validateInternetGetListMunicipalities(ParametrosCobertura parametrosCobertura) {
        if (getValidateInternet().isConnected()) {
            createThreadGetListMunicipalities(parametrosCobertura);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    public void createThreadGetListMunicipalities(final ParametrosCobertura parametrosCobertura) {
        getView().showProgressDIalog(R.string.text_wait_please);
        Thread thread = new Thread(() -> getMunicipalitiesWithCoverage(parametrosCobertura));
        thread.start();
    }

    public void getMunicipalitiesWithCoverage(ParametrosCobertura parametrosCobertura) {
        try {
            boolean cobertura = getListMunicipalities(parametrosCobertura);
            if (cobertura) {
                getView().setInformationToIntent(listMunicipialities);
            } else {
                String message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_AND_NOT_ON_ANTIOQUIA,
                        parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());

                getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), message);
            }
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }

    public boolean getListMunicipalities(ParametrosCobertura parametrosCobertura) throws RepositoryError {
        listMunicipialities = reporteDeFraudesBusinessLogic.getMunicipalitiesWithCoverage(parametrosCobertura);
        String municipality = removeDiacriticalMarks(parametrosCobertura.getMunicipio().toUpperCase().trim());
        for (String municipalityToValidate : listMunicipialities) {
            if (municipalityToValidate.toUpperCase().contains(municipality)) {
                boolean ensucasa = true;
                return true;
            }
        }
        return false;
    }

    public static String removeDiacriticalMarks(String string) {
        return Normalizer.normalize(string, Normalizer.Form.NFD)
                .replaceAll("\\p{InCombiningDiacriticalMarks}+", "");
    }

}
