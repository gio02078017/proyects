package app.epm.com.reporte_danios_presentation.presenters;

import com.epm.app.app_utilities_presentation.presenters.BaseUbicacionDeFraudeOrDanioPresenter;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;

import java.text.Normalizer;
import java.util.List;

import com.epm.app.business_models.business_models.ETipoServicio;

import app.epm.com.reporte_danios_domain.danios.danios.DaniosBL;
import app.epm.com.reporte_danios_presentation.R;
import app.epm.com.reporte_danios_presentation.view.views_activities.IUbicacionDanioView;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by josetabaresramirez on 2/02/17.
 */

public class UbicacionDanioPresenter extends BaseUbicacionDeFraudeOrDanioPresenter<IUbicacionDanioView> {

    private DaniosBL daniosBL;

    public UbicacionDanioPresenter(DaniosBL daniosBL) {
        super(daniosBL);
        this.daniosBL = daniosBL;
    }

    public boolean hasCobertura(ParametrosCobertura parametrosCobertura) throws RepositoryError {
        List<String> municipios = daniosBL.getMunicipios(parametrosCobertura);
        String municipio = removeDiacriticalMarks(parametrosCobertura.getMunicipio().toUpperCase().trim());
        for (String municipioToValdiate : municipios) {
            if (municipioToValdiate.toUpperCase().contains(municipio)) {
                return true;
            }
        }
        return false;
    }

    public static String removeDiacriticalMarks(String string) {
        return Normalizer.normalize(string, Normalizer.Form.NFD)
                .replaceAll("\\p{InCombiningDiacriticalMarks}+", "");
    }


    public void validateCoberturaAndTipoServicio(ParametrosCobertura parametrosCobertura) {
        try {
            boolean hasCobertura = hasCobertura(parametrosCobertura);
            if (parametrosCobertura.getTipoServicio() == 2) {
                parametrosCobertura.setTipoServicio(0);
                validateCoberturaAndTipoServicioIluminaria(hasCobertura, parametrosCobertura);
            } else {
                validateGeneralCobertura(hasCobertura, parametrosCobertura);
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

    public void validateCoberturaAndTipoServicioIluminaria(boolean hasCoberturaEnergia, ParametrosCobertura parametrosCobertura) {
        if (parametrosCobertura.getTipoServicio() != 0) {
            throw new IllegalArgumentException(Constants.EMPTY_PARAMETERS);
        }
        try {
            boolean hasCoberturaIluminaria = hasCobertura(parametrosCobertura);
            validateCoberturasEnergiaAndIluminaria(hasCoberturaEnergia, hasCoberturaIluminaria, parametrosCobertura);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), repositoryError.getMessage());
            }
        }

    }

    public void validateGeneralCobertura(boolean hasCobertura, ParametrosCobertura parametrosCobertura) {
        if (hasCobertura) {
            getView().getListTypeDanio(false, false);
        } else {
            String message;
            String departamento = removeDiacriticalMarks(parametrosCobertura.getDepartamento().toUpperCase().trim());
            if (departamento.equals(Constants.ANTIOQUIA)) {
                message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_AND_ON_ANTIOQUIA,
                        ETipoServicio.getNameService(parametrosCobertura.getTipoServicio()),
                        parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
            } else {
                message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_AND_NOT_ON_ANTIOQUIA,
                        parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
            }
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), message);
        }
    }

    public void validateCoberturasEnergiaAndIluminaria(boolean hasCoberturEnergia, boolean hasCoberturaIluminaria,
                                                       ParametrosCobertura parametrosCobertura) {
        String departamento = removeDiacriticalMarks(parametrosCobertura.getDepartamento().toUpperCase().trim());
        if (hasCoberturEnergia && hasCoberturaIluminaria) {
            getView().getListTypeDanio(hasCoberturEnergia, hasCoberturaIluminaria);
            return;
        }
        if (hasCoberturEnergia && departamento.equals(Constants.ANTIOQUIA)) {
            String message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_ILUMINARIA_AND_ON_ANTIOQUIA,
                    parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
            getView().showAlertDialogToStartDescribeDanioActivityOnUiThread(getView().getName(),
                    message, hasCoberturEnergia, hasCoberturaIluminaria);
            return;
        }

        if (hasCoberturEnergia && !departamento.equals(Constants.ANTIOQUIA)) {
            String message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_ILUMINARIA_AND_NOT_ON_ANTIOQUIA,
                    parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), message);
            return;
        }

        if (!hasCoberturEnergia && hasCoberturaIluminaria) {
            String message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_ENERGIA,
                    parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
            getView().showAlertDialogToStartDescribeDanioActivityOnUiThread(getView().getName(),
                    message, hasCoberturEnergia, hasCoberturaIluminaria);
            return;
        }
        if (!hasCoberturEnergia) {
            String message;
            if (departamento.equals(Constants.ANTIOQUIA)) {
                message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_AND_ON_ANTIOQUIA,
                        ETipoServicio.Energia.getName(), parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
            } else {
                message = String.format(Constants.FORMAT_MESSAGE_WITHOUT_COBERTURA_AND_NOT_ON_ANTIOQUIA,
                        parametrosCobertura.getMunicipio(), parametrosCobertura.getDepartamento());
            }
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), message);
        }
    }
}
