package app.epm.com.utilities.helpers;


/**
 * Created by josetabaresramirez on 13/03/17.
 */

public interface ICustomApplicationContext {

    void setCustomNotificationHelper(ICustomNotificationHelper customNotificationHelper);

    void sendReport(String report);
}
