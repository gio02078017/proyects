package app.epm.com.reporte_fraudes_presentation.dependency_injection;

import app.epm.com.reporte_fraudes_domain.reporte_fraudes.ReporteDeFraudesBusinessLogic;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;

/**
 * Created by mateoquicenososa on 10/04/17.
 */

public class DomainModule {

    public static ReporteDeFraudesBusinessLogic getReporteDeFraudesBusinessLogicInstance(ICustomSharedPreferences customSharedPreferences) {
        return new ReporteDeFraudesBusinessLogic(RepositoryLocator.getReporteDeFraudesRepositoryInstance(customSharedPreferences));
    }
}
