package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import android.arch.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.RecoverySubscriptionResponse;

public interface IRecuperationSubscriptionViewModel {

   boolean validEmail(String email);
   boolean validateIFInputIsEmpty(String email);
   void showSimpleMessage(String notFoundEmail, Mensaje mensaje);
   void getSubscriptionDeviceAlertsByEmail(String email,String token);
   void validateIfExitSubscription(RecoverySubscriptionResponse response);
   void updateSubscriptionDeviceAlertsByEmail(String email);
   void validateResponseUpdateSubscription(NotificationResponse notificationResponse);
   MutableLiveData<Boolean> getProgressDialog();



}
