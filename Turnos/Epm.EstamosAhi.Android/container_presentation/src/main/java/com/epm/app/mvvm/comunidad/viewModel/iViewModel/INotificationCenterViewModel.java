package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.network.response.notifications.ReceivePushNotification;
import com.epm.app.mvvm.comunidad.viewModel.IBaseViewModel;

import java.util.List;

import app.epm.com.utilities.helpers.ErrorMessage;

public interface INotificationCenterViewModel extends IBaseViewModel {

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



}
