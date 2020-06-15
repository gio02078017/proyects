package com.epm.app.mvvm.comunidad.viewModel;

import android.arch.lifecycle.MutableLiveData;
import android.arch.lifecycle.ViewModel;
import android.view.View;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.bussinesslogic.INotificationsBL;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationList;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.ReceivePushNotification;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetNotificationsPush;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.helpers.ValidateServiceCode;

import com.epm.app.mvvm.comunidad.viewModel.iViewModel.INotificationCenterViewModel;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashSet;
import java.util.List;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;
import app.epm.com.utilities.utils.NotificationManager;
import io.reactivex.Observable;
import io.reactivex.Scheduler;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.Disposable;
import io.reactivex.observers.DisposableObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

public class NotificationCenterViewModel extends BaseViewModel implements INotificationCenterViewModel {

    private INotificationsBL notificationsBL;
    private ICustomSharedPreferences customSharedPreferences;
    private int numberPages = 1;
    private List<ReceivePushNotification> listNotification;
    private MutableLiveData<Boolean> loadNotifications;
    private MutableLiveData<Boolean> error;
    private Integer titleError, messageError;
    public final MutableLiveData<Integer> notFound;
    private MutableLiveData<Boolean> progress;
    private MutableLiveData<Boolean> delete;
    private int totalRecords;
    private boolean tryAgain = true;
    private boolean recordsNotFound;
    private IValidateInternet validateInternet;

    @Inject
    public NotificationCenterViewModel(NotificationsRepository notificationsRepository, CustomSharedPreferences customSharedPreferences, ValidateInternet validateInternet) {
        this.notificationsBL = notificationsRepository;
        this.customSharedPreferences = customSharedPreferences;
        this.listNotification = new ArrayList<>();
        this.loadNotifications = new MutableLiveData<>();
        this.error = new MutableLiveData<>();
        this.expiredToken = new MutableLiveData<>();
        this.notFound = new MutableLiveData<>();
        this.progress = new MutableLiveData<>();
        this.delete = new MutableLiveData<>();
        this.validateInternet = validateInternet;
    }

    @Override
    public void getNotificationsPush() {
        if(validateInternet.isConnected()) {
            serviceNotificationsPush();
        }else{
            validateShowError(new ErrorMessage(R.string.title_appreciated_user, R.string.text_validate_internet));
        }
    }

    public void serviceNotificationsPush(){
        Observable<GetNotificationsPush> result = notificationsBL.getNotificationsPush(IdDispositive.getIdDispositive());
        Disposable disposable = result.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<GetNotificationsPush>() {
                    @Override
                    public void onNext(GetNotificationsPush getNotificationsPush) {
                        validateResponseGetNotifications(getNotificationsPush);
                    }

                    @Override
                    public void onError(Throwable e) {
                        errorService(e);
                    }

                    @Override
                    public void onComplete() {

                    }
                });
        disposables.add(disposable);
    }

    public void validateResponseGetNotifications(GetNotificationsPush getNotificationsPush){
        if (getNotificationsPush != null && getNotificationsPush.getCantidadNotificacionesSinLeer() != null) {
            customSharedPreferences.addInt(Constants.NUMBER_NOTIFICATIONS, getNotificationsPush.getCantidadNotificacionesSinLeer());
            NotificationManager.getInstance().getNotificationSubject().addNotification();
        }
    }

    @Override
    public void deleteNotificationsPush(int notification) {
        validateInternetDeleteNotification(notification);
    }

    public void validateInternetDeleteNotification(int notification){
        if(validateInternet.isConnected()){
            progress.setValue(true);
            serviceDeleteNotification(notification);
        }else {
            validateShowError(new ErrorMessage(R.string.title_appreciated_user, R.string.text_validate_internet));
        }
    }

    public void serviceDeleteNotification(int notification){
        Observable<NotificationResponse> result = notificationsBL.deleteNotificationsPush(listNotification.get(notification).getIdNotificationPush(), customSharedPreferences.getString(Constants.TOKEN));
        Disposable disposable = result.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<NotificationResponse>() {
                    @Override
                    public void onNext(NotificationResponse notificationResponse) {
                        validateDeleteNotification(notificationResponse);
                    }

                    @Override
                    public void onError(Throwable e) {
                        errorService(e);
                    }

                    @Override
                    public void onComplete() {

                    }
                });
        disposables.add(disposable);
    }

    public void validateDeleteNotification(NotificationResponse delete){
        if (delete != null && delete.getEstadoTransaccion()) {
            totalRecords = totalRecords - Constants.ONE;
            this.delete.setValue(true);
            validateListNotificationIsEmpty();
            progress.setValue(false);
            numberPages = Constants.ONE;
            getNotificationsPush();
        }
    }


    @Override
    public void loadNotifications() {
        if (!recordsNotFound) {
            notFound.setValue(View.INVISIBLE);
            validateInternetLoadListNotifications();
        }
    }

    public void validateInternetLoadListNotifications(){
        if(validateInternet.isConnected()){
            progress.setValue(true);
            serviceListNotification();
        }else {
            validateShowError(new ErrorMessage(R.string.title_appreciated_user, R.string.text_validate_internet));
        }
    }

    public void serviceListNotification(){
        Observable<NotificationList> result = notificationsBL.getListNotificationsPush(IdDispositive.getIdDispositive(), customSharedPreferences.getString(Constants.TOKEN),
                Constants.ONE_THOUSAND, Constants.ONE);
        Disposable disposable = result.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<NotificationList>() {
                    @Override
                    public void onNext(NotificationList notificationList) {
                        loadListNotification(notificationList);
                    }

                    @Override
                    public void onError(Throwable e) {
                        progress.setValue(false);
                        ValidateServiceCode.captureServiceErrorCode(((HttpException) e).code());
                        validateError();
                    }

                    @Override
                    public void onComplete() {

                    }
                });
        disposables.add(disposable);
    }

    @Override
    public void updateNotification(int position) {
        int idNotification = listNotification.get(position).getIdNotificationPush();
        validateInternetUpdateNotification(idNotification);
    }

    public void validateInternetUpdateNotification(int idNotification){
        if(validateInternet.isConnected()){
            serviceUpdateNotificationService(idNotification);
        }else {
            progress.setValue(false);
            validateShowError(new ErrorMessage(R.string.title_appreciated_user, R.string.text_validate_internet));
        }
    }

    public void serviceUpdateNotificationService(int idNotification){
        Observable<NotificationResponse> result = notificationsBL.updateNotificationPush(idNotification, customSharedPreferences.getString(Constants.TOKEN));
        Disposable disposable = result.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<NotificationResponse>() {
                    @Override
                    public void onNext(NotificationResponse notificationResponse) {
                        validateResponseUpdateNotification(notificationResponse);
                    }

                    @Override
                    public void onError(Throwable e) {
                       errorService(e);
                    }

                    @Override
                    public void onComplete() {

                    }
                });
        disposables.add(disposable);
    }

    public void errorService(Throwable e){
        progress.setValue(false);
        setError(e);
    }


    public void validateResponseUpdateNotification(NotificationResponse notificationResponse){
        if (notificationResponse != null && notificationResponse.getEstadoTransaccion()) {
            getNotificationsPush();
        }
    }

    @Override
    public void newNotification(String template) {
        validateShiftState(template);
    }

    private void validateShiftState(String valueTemplate) {
        switch (valueTemplate){
            case Constants.MODULE_TURNO_ATENDIDO:
            case Constants.MODULE_TURNO_ABANDONADO:
                customSharedPreferences.deleteValue(Constants.ASSIGNED_TRUN);
                customSharedPreferences.deleteValue(Constants.INFORMATION_OFFICE_JSON);
                NotificationManager.getInstance().getNotificationSubject().addNotification();
                recordsNotFound = false;
                listNotification.clear();
                loadNotifications();
                break;
            default:
             loadNewNotification();
             break;
        }
    }


    public void loadNewNotification() {
        tryAgain = true;
        notFound.setValue(View.INVISIBLE);
        validateInternetloadNewNotification();
    }

    public void validateInternetloadNewNotification(){
        if(validateInternet.isConnected()){
            serviceLoadNewNotification();
        }else {
            validateShowError(new ErrorMessage(R.string.title_appreciated_user, R.string.text_validate_internet));
        }
    }

    public void serviceLoadNewNotification(){
        Observable<NotificationList> result = notificationsBL.getListNotificationsPush(IdDispositive.getIdDispositive(), customSharedPreferences.getString(Constants.TOKEN),
                Constants.ONE, Constants.ONE);
        Disposable disposable = result.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<NotificationList>() {
                    @Override
                    public void onNext(NotificationList notificationList) {
                        loadListNewNotification(notificationList);
                    }

                    @Override
                    public void onError(Throwable e) {
                        errorService(e);
                    }

                    @Override
                    public void onComplete() {

                    }
                });
        disposables.add(disposable);
    }

    public void loadListNewNotification(NotificationList notificationList){
        if (notificationList != null) {
            listNotification.addAll(Constants.ZERO, notificationList.getReceivePushNotification());
            loadNotifications.setValue(true);
            recordsNotFound = true;
            tryAgain=false;
        }
    }

    public void loadListNotification(NotificationList notificationList) {
        if (notificationList != null && notificationList.getReceivePushNotification().size() > Constants.ZERO) {
            getNotificationsPush();
            listNotification.addAll(notificationList.getReceivePushNotification());
            numberPages = notificationList.getActualPage() + Constants.ONE;
            totalRecords = notificationList.getTotalRecords();
            progress.setValue(false);
            tryAgain = false;
            loadNotifications.setValue(true);
            validateRecords(notificationList.getActualPage(), notificationList.getTotalPages());
        }
    }




    public void validateRecords(int actualPage, int totalPage) {
        if (actualPage == totalPage) {
            recordsNotFound = true;
        }
    }


    public void validateError() {
        switch (ValidateServiceCode.getErrorCode()) {
            case Constants.UNAUTHORIZED_ERROR_CODE:
                expiredToken.setValue(true);
                break;
            case Constants.NOT_FOUND_ERROR_CODE:
                notFound.setValue(View.VISIBLE);
                break;
            default:
                error.setValue(true);
                break;
        }

    }


    public void validateListNotificationIsEmpty() {
        if (listNotification.size() == 0) {
            notFound.setValue(View.VISIBLE);
        }
    }

    @Override
    protected void onCleared() {
        super.onCleared();
        DisposableManager.dispose();
        disposables.clear();
    }



    @Override
    public boolean isTryAgain() {
        return tryAgain;
    }

    @Override
    public List<ReceivePushNotification> getListNotification() {
        return listNotification;
    }

    @Override
    public MutableLiveData<Boolean> getLoadNotifications() {
        return loadNotifications;
    }

    @Override
    public MutableLiveData<Boolean> getIsError() {
        return error;
    }

    public void setListNotification(List<ReceivePushNotification> listNotification) {
        this.listNotification = listNotification;
    }

    @Override
    public MutableLiveData<Boolean> getDelete() {
        return delete;
    }

    @Override
    public MutableLiveData<Boolean> getProgress() {
        return progress;
    }

    @Override
    public Integer getIntTitleError() {
        return titleError;
    }

    @Override
    public Integer getMessageError() {
        return messageError;
    }

    public boolean isRecordsNotFound() {
        return recordsNotFound;
    }


}
