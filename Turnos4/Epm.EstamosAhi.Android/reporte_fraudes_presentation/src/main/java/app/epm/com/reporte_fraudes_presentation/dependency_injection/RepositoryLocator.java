package app.epm.com.reporte_fraudes_presentation.dependency_injection;

import app.epm.com.reporte_fraudes_domain.reporte_fraudes.IReporteDeFraudesRepository;
import app.epm.com.reporte_fraudes_presentation.repositories.ReporteDeFraudesRepository;
import app.epm.com.reporte_fraudes_presentation.repositories.ReporteDeFraudesRepositoryTest;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 10/04/17.
 */

class RepositoryLocator {

    static IReporteDeFraudesRepository getReporteDeFraudesRepositoryInstance(ICustomSharedPreferences customSharedPreferences) {

        if (Constants.IS_DEBUG) {
            return new ReporteDeFraudesRepositoryTest();
        } else {
            return new ReporteDeFraudesRepository(customSharedPreferences);
        }
    }
}
