package app.epm.com.reporte_fraudes_presentation.view.views_activities;

import com.epm.app.app_utilities_presentation.views.views_activities.IBaseUbicacionDeFraudeOrDanioView;

import java.util.List;

import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by leidycarolinazuluagabastidas on 19/04/17.
 */

public interface IUbicacionDeFraudeView extends IBaseUbicacionDeFraudeOrDanioView {
    void setInformationToIntent(List<String> listMunicipialities);
}
