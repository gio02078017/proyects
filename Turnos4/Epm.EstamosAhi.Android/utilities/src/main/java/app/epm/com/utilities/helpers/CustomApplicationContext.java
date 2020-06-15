package app.epm.com.utilities.helpers;


/**
 * Created by josetabaresramirez on 13/03/17.
 */

public class CustomApplicationContext {

    private static CustomApplicationContext instance = null;

    private ICustomApplicationContext customApplicationContext;
    private IMenuFragmentListener menuFragmentListener;

    public static void createInstance(ICustomApplicationContext customApplicationContext) {
        instance = new CustomApplicationContext(customApplicationContext);
    }

    public static CustomApplicationContext getInstance() {
        return instance;
    }

    public CustomApplicationContext(ICustomApplicationContext customApplicationContext) {
        this.customApplicationContext = customApplicationContext;
    }


    public void setCustomNotificationHelper(ICustomNotificationHelper customNotificationHelper) {
        customApplicationContext.setCustomNotificationHelper(customNotificationHelper);
    }

    public void sendReport(String report) {
        customApplicationContext.sendReport(report);
    }

    public void injectMenuFragmentListener(IMenuFragmentListener menuFragmentListener) {
        this.menuFragmentListener = menuFragmentListener;
    }

    public IMenuFragmentListener getMenuFragmentListener() {
        return menuFragmentListener;
    }
}
