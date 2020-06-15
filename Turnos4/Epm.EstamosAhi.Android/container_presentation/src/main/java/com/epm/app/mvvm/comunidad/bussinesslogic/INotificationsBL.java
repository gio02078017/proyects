package com.epm.app.mvvm.comunidad.bussinesslogic;

import android.arch.lifecycle.LiveData;
import android.arch.lifecycle.MutableLiveData;


import com.epm.app.mvvm.comunidad.network.request.NotificationSaveRequest;
import com.epm.app.mvvm.comunidad.network.request.ShowNotificationPushRequest;
import com.epm.app.mvvm.comunidad.network.request.UpdateNotificationStatusOneSignalRequest;
import com.epm.app.mvvm.comunidad.network.request.UpdateStatusSendNotificationRequest;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationList;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationSaveResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.ShowNotificationPushResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.UpdateStatusSendNotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetNotificationsPush;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;
import com.epm.app.mvvm.util.IError;

import io.reactivex.Observable;

public interface INotificationsBL{


    Observable<GetNotificationsPush> getNotificationsPush(String idDispositive);
    Observable<NotificationList> getListNotificationsPush(String idDispositive, String token, int recordsPage, int pageNumber);
    Observable<NotificationResponse> deleteNotificationsPush(int idNotification, String token);
    Observable<NotificationResponse> updateNotificationPush(int idNotification, String token);
    Observable<NotificationSaveResponse> saveNotificationPush(NotificationSaveRequest notificationSaveRequest);
    Observable<ShowNotificationPushResponse> isShowNotificationPush(ShowNotificationPushRequest showNotificationPushRequest, String token);
    Observable<NotificationResponse> updateStatusNotification(UpdateNotificationStatusOneSignalRequest updateNotificationStatusOneSignal);
    Observable<UpdateStatusSendNotificationResponse> updateStatusSendNotification(UpdateStatusSendNotificationRequest updateStatusSendNotificationRequest, String token);

}
