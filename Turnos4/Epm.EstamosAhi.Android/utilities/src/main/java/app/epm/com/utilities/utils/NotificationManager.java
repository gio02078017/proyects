package app.epm.com.utilities.utils;

public class NotificationManager {

    private static NotificationManager INSTANCE = new NotificationManager();
    private NotificationSubject notificationSubject = new NotificationSubject();


    private NotificationManager() {
    }

    public static NotificationManager getInstance() {
        return INSTANCE;
    }

    public NotificationSubject getNotificationSubject() {
        return notificationSubject;
    }
}
