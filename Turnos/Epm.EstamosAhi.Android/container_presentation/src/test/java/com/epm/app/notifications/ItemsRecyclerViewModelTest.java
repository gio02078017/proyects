package com.epm.app.notifications;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import android.content.res.Resources;
import android.graphics.drawable.Drawable;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.network.response.notifications.ReceivePushNotification;
import com.epm.app.mvvm.comunidad.network.response.notifications.TemplateOneSignal;
import com.epm.app.mvvm.comunidad.viewModel.ItemsRecyclerViewModel;

import static org.junit.Assert.*;

import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnit;
import org.mockito.junit.MockitoRule;

import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.verify;

public class ItemsRecyclerViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    private ItemsRecyclerViewModel itemsRecyclerViewModel;

    @InjectMocks
    ReceivePushNotification receivePushNotification;

    @InjectMocks
    TemplateOneSignal templateOneSignal;




    @Mock
    public Resources resources;

    @Before
    public void setUp()  {
        itemsRecyclerViewModel = Mockito.spy(new ItemsRecyclerViewModel(resources));
    }


    @Test
    public void testReceivePushNotificationShouldBeNull() {
        receivePushNotification = null;
        itemsRecyclerViewModel.setReceivePushNotification(null);
        assertNull(receivePushNotification);
    }

    @Test
    public void testReceivePushNotificationShouldNotBeNull() {

        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        assertNotNull(receivePushNotification);
    }

    @Test
    public void testTypeNotificationIsModuleNotices() {
        templateOneSignal.setValue(Constants.MODULE_NOTICIAS);
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.typeNotification();
        verify(itemsRecyclerViewModel).validateIconNotification(R.drawable.ic_notification_read_notice, R.drawable.ic_notification_notice);
    }

    @Test
    public void testTypeNotificationIsModuleLineAttention() {
        templateOneSignal.setValue(Constants.MODULE_LINEAS_DE_ATENCION);
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.typeNotification();
        verify(itemsRecyclerViewModel).validateIconNotification(R.drawable.ic_notifications_read_atention, R.drawable.ic_notification_atention);
    }

    @Test
    public void testTypeNotificationIsModuleOffices() {
        templateOneSignal.setValue(Constants.MODULE_SERVICIO_AL_CLIENTE);
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.typeNotification();
        verify(itemsRecyclerViewModel).validateIconNotification(R.drawable.ic_notification_read_customer_service, R.drawable.ic_notification_customer_service);
    }

    @Test
    public void testTypeNotificationIsModuleFraudReport() {
        templateOneSignal.setValue(Constants.MODULE_REPORTE_FRAUDES);
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.typeNotification();
        verify(itemsRecyclerViewModel).validateIconNotification(R.drawable.ic_notification_read_fraud, R.drawable.ic_notification_fraud);
    }

    @Test
    public void testTypeNotificationIsModuleBill() {
        templateOneSignal.setValue(Constants.MODULE_FACTURA);
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.typeNotification();
        verify(itemsRecyclerViewModel).validateIconNotification(R.drawable.ic_notification_read_facture, R.drawable.ic_notification_facture);
    }

    @Test
    public void testTypeNotificationIsModuleTransparentContact() {
        templateOneSignal.setValue(Constants.MODULE_CONTACTO_TRANSPARENTE);
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.typeNotification();
        verify(itemsRecyclerViewModel).validateIconNotification(R.drawable.ic_notification_read_ethical_line,R.drawable.ic_notification_ethical_line);
    }

    @Test
    public void testTypeNotificationIsModuleDaniosReport() {
        templateOneSignal.setValue(Constants.MODULE_REPORTE_DANIOS);
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.typeNotification();
        verify(itemsRecyclerViewModel).validateIconNotification(R.drawable.ic_notification_read_reports, R.drawable.ic_notification_reports);
    }

    @Test
    public void testTypeNotificationIsModuleEvents() {
        templateOneSignal.setValue(Constants.MODULE_EVENTOS);
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.typeNotification();
        verify(itemsRecyclerViewModel).validateIconNotification(R.drawable.ic_notification_read_events, R.drawable.ic_notification_events);
    }

    @Test
    public void testTypeNotificationIsModuleGasStation() {
        templateOneSignal.setValue(Constants.MODULE_ESTACIONES_DE_GAS);
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.typeNotification();
        verify(itemsRecyclerViewModel).validateIconNotification(R.drawable.ic_notification_read_station, R.drawable.ic_notification_station);
    }

    @Test
    public void testTypeNotificationIsModuleAlerts() {
        templateOneSignal.setValue(Constants.MODULE_DE_ALERTASHIDROITUANGO);
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.typeNotification();
        verify(itemsRecyclerViewModel).validateIconNotification(R.drawable.ic_notification_read_alerts, R.drawable.ic_notification_alerts);
    }

    @Test
    public void testTypeNotificationNoneOfThem() {
        templateOneSignal.setValue("");
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.typeNotification();
        verify(itemsRecyclerViewModel).validateIconNotification(R.drawable.ic_notification_read_office, R.drawable.ic_notification_office);
    }

    @Test
    public void testValidateIconNotificationIsRead(){
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        receivePushNotification.setRead(false);
        Drawable drawable = resources.getDrawable(R.drawable.icon_app);
        itemsRecyclerViewModel.validateIconNotification(R.drawable.icon_app, R.drawable.icon_app);
        assertEquals(drawable,itemsRecyclerViewModel.drawable.getValue());
    }

    @Test
    public void testShouldUpdateState(){
        templateOneSignal.setValue(Constants.MODULE_DE_ALERTASHIDROITUANGO);
        receivePushNotification.setRead(false);
        receivePushNotification.setTemplateOneSignal(templateOneSignal);
        itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
        itemsRecyclerViewModel.updateState();
        verify(itemsRecyclerViewModel).typeNotification();
    }

   @Test
   public void testValidateIsNotificationRead(){
       itemsRecyclerViewModel.setReceivePushNotification(receivePushNotification);
       receivePushNotification.setRead(true);
       receivePushNotification.setTemplateOneSignal(templateOneSignal);
       itemsRecyclerViewModel.validateReadNotification(R.drawable.color_progress);
       int color = itemsRecyclerViewModel.colorTextTitle.getValue();
       assertEquals(color,resources.getColor(R.color.text_notification));
   }



}
