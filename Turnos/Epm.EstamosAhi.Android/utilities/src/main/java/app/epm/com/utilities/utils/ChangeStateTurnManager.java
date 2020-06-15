package app.epm.com.utilities.utils;

public class ChangeStateTurnManager {

    private static ChangeStateTurnManager INSTANCE = new ChangeStateTurnManager();
    private ChangeStateTurnSubject notificationSubject = new ChangeStateTurnSubject();


    private ChangeStateTurnManager() {
    }

    public static ChangeStateTurnManager getInstance() {
        return INSTANCE;
    }

    public ChangeStateTurnSubject getNotificationSubject() {
        return notificationSubject;
    }

}
