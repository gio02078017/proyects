package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import android.arch.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.network.response.notifications.ReceivePushNotification;

import java.util.List;

public interface INotificationCenterViewModel {

    void getNotificationsPush();
    void loadNotifications();
    List<ReceivePushNotification> getListNotification();
    MutableLiveData<Boolean> getLoadNotifications();
    void deleteNotificationsPush(int notification);
    MutableLiveData<Boolean> getProgress();
    MutableLiveData<Boolean> getDelete();
    boolean isTryAgain();
    void updateNotification(int position);
    void newNotification(String template);
    Integer getIntTitleError();
    MutableLiveData<Boolean> getIsError();
    Integer getMessageError();


}
