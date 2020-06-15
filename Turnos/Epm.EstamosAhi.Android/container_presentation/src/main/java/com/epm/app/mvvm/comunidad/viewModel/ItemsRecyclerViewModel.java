package com.epm.app.mvvm.comunidad.viewModel;

import androidx.lifecycle.MutableLiveData;
import android.content.res.Resources;
import android.graphics.drawable.Drawable;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.network.response.notifications.ReceivePushNotification;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.IItemsRecyclerViewModel;

import javax.inject.Inject;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;


public class ItemsRecyclerViewModel extends BaseViewModel implements IItemsRecyclerViewModel {

    public final MutableLiveData<Drawable> drawable;
    public final MutableLiveData<String> title;
    public final MutableLiveData<String> description;
    public final MutableLiveData<String> dateTime;
    public final MutableLiveData<Integer> colorTextTitle;
    public final MutableLiveData<Integer> colorDescriptionText;
    public final MutableLiveData<Integer> colorCardView;
    private Resources resources;
    private ReceivePushNotification receivePushNotification;

    @Inject
    public ItemsRecyclerViewModel(Resources resources) {
        this.resources = resources;
        this.drawable = new MutableLiveData<>();
        this.title = new MutableLiveData<>();
        this.description = new MutableLiveData<>();
        this.dateTime = new MutableLiveData<>();
        this.colorTextTitle = new MutableLiveData<>();
        this.colorCardView = new MutableLiveData<>();
        this.colorDescriptionText = new MutableLiveData<>();

    }

    public void setReceivePushNotification(ReceivePushNotification receivePushNotification) {
        this.receivePushNotification = receivePushNotification;
    }

    @Override
    public void typeNotification() {
        switch (receivePushNotification.getTemplateOneSignal().getValue()) {
            case Constants.MODULE_NOTICIAS:
                validateIconNotification(R.drawable.ic_notification_read_notice, R.drawable.ic_notification_notice);
                validateReadNotification(R.color.title_notification_notices);
                break;
            case Constants.MODULE_LINEAS_DE_ATENCION:
                validateIconNotification(R.drawable.ic_notifications_read_atention, R.drawable.ic_notification_atention);
                validateReadNotification(R.color.title_notification_line_attention);
                break;
            case Constants.MODULE_SERVICIO_AL_CLIENTE:
            case Constants.MODULE_TURNO_ATENDIDO :
            case Constants.MODULE_TURNO_ABANDONADO:
            case Constants.MODULE_TURNO_AVANCE:
                validateIconNotification(R.drawable.ic_notification_read_customer_service, R.drawable.ic_notification_customer_service);
                validateReadNotification(R.color.title_notification_office);
                break;
            case Constants.MODULE_REPORTE_FRAUDES:
                validateIconNotification(R.drawable.ic_notification_read_fraud, R.drawable.ic_notification_fraud);
                validateReadNotification(R.color.title_notification_reports_frauds);
                break;
            case Constants.MODULE_FACTURA:
                validateIconNotification(R.drawable.ic_notification_read_facture, R.drawable.ic_notification_facture);
                validateReadNotification(R.color.title_notification_bill);
                break;
            case Constants.MODULE_CONTACTO_TRANSPARENTE:
                validateIconNotification(R.drawable.ic_notification_read_ethical_line,R.drawable.ic_notification_ethical_line);
                validateReadNotification(R.color.title_notification_contact);
                break;
            case Constants.MODULE_REPORTE_DANIOS:
                validateIconNotification(R.drawable.ic_notification_read_reports, R.drawable.ic_notification_reports);
                validateReadNotification(R.color.title_notification_reports_damage);
                break;
            case Constants.MODULE_EVENTOS:
                validateIconNotification(R.drawable.ic_notification_read_events, R.drawable.ic_notification_events);
                validateReadNotification(R.color.title_notification_events);
                break;
            case Constants.MODULE_ESTACIONES_DE_GAS:
                validateIconNotification(R.drawable.ic_notification_read_station, R.drawable.ic_notification_station);
                validateReadNotification(R.color.title_notification_stations);
                break;
            case Constants.MODULE_DE_ALERTASHIDROITUANGO:
                validateIconNotification(R.drawable.ic_notification_read_alerts, R.drawable.ic_notification_alerts);
                validateReadNotification(R.color.title_notification_alerts);
                break;
            default:
                validateIconNotification(R.drawable.ic_notification_read_office, R.drawable.ic_notification_office);
                validateReadNotification(R.color.title_notification_office);
                break;
        }
        loadDates();

    }

    public void loadDates(){
        this.title.setValue(receivePushNotification.getTitle());
        this.description.setValue(receivePushNotification.getMensaje());
        this.dateTime.setValue(receivePushNotification.getTimeCalculated());
    }


    public void validateReadNotification(int titleTextColor) {
        if (receivePushNotification.getRead()) {
            colorCardView.setValue(resources.getColor(R.color.card_read_notification));
            colorTextTitle.setValue(resources.getColor(R.color.text_read_notification));
            colorDescriptionText.setValue(resources.getColor(R.color.text_read_notification));
        } else {
            colorCardView.setValue(resources.getColor(R.color.card_notification));
            colorTextTitle.setValue(resources.getColor(titleTextColor));
            colorDescriptionText.setValue(resources.getColor(R.color.text_notification_description));
        }
    }

    public void updateState() {
        receivePushNotification.setRead(true);
        typeNotification();
    }

    public void validateIconNotification(int read, int notRead) {
        Drawable drawable;
        if (receivePushNotification.getRead()) {
            drawable = resources.getDrawable(read);
        } else {
            drawable = resources.getDrawable(notRead);
        }
        this.drawable.setValue(drawable);
    }

    @Override
    protected void onCleared() {
        super.onCleared();
        DisposableManager.dispose();
    }

}
