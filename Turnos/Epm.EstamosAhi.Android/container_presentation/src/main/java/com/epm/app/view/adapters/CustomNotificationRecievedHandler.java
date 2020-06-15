package com.epm.app.view.adapters;

import com.epm.app.App;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.onesignal.OSNotification;
import com.onesignal.OneSignal;
import org.json.JSONObject;


import app.epm.com.utilities.helpers.ICustomSharedPreferences;

public class CustomNotificationRecievedHandler implements OneSignal.NotificationReceivedHandler {

    private App app;

    public CustomNotificationRecievedHandler(App app) {
        this.app = app;

    }

    @Override
    public void notificationReceived(OSNotification notification) {
        JSONObject data = notification.payload.additionalData;
        app.notiftyFromOneSignal();
    }


}
