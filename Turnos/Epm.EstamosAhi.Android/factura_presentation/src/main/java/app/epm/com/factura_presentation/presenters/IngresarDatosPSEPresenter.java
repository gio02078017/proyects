package app.epm.com.factura_presentation.presenters;

import com.epm.app.business_models.business_models.RepositoryError;

import app.epm.com.facturadomain.business_models.DataPagar;
import app.epm.com.facturadomain.business_models.InformacionPSE;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.view.views_activities.IIngresarDatosPSEView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 6/01/17.
 */

public class IngresarDatosPSEPresenter extends BasePresenter<IIngresarDatosPSEView> {

    private FacturaBL facturaBL;

    private ICustomSharedPreferences customSharedPreferences;

    /**
     * Constructor
     *
     * @param facturaBL Clase con lógica de negocio.
     */
    public IngresarDatosPSEPresenter(FacturaBL facturaBL) {
        this.facturaBL = facturaBL;
    }

    /**
     * Valida la conexión a internet.
     *
     * @param dataPagar dataPagar.
     */
    public void validateInternetDatosPagar(DataPagar dataPagar) {
        if (getValidateInternet().isConnected()) {
            createThreadToDatosPagar(dataPagar);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio.
     *
     * @param dataPagar dataPagar.
     */
    public void createThreadToDatosPagar(final DataPagar dataPagar) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> datosPagar(dataPagar));
        thread.start();
    }

    /**
     * Llama a la clase del negocio para comunicarse con el servicio.
     *
     * @param dataPagar dataPagar.
     */
    public void datosPagar(DataPagar dataPagar) {
        try {
            InformacionPSE informacionPSE = facturaBL.datosPagar(dataPagar);
            getView().saveInformacionPSE(informacionPSE);
            getView().startPagePSE(dataPagar.getFacturasPagar());
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }
}
