package com.epm.app.mvvm.comunidad.views.activities;


import android.arch.lifecycle.ViewModelProviders;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.support.v7.widget.helper.ItemTouchHelper;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;

import com.epm.app.R;
import com.epm.app.databinding.ActivityNotificationCenterBinding;
import com.epm.app.mvvm.comunidad.adapter.CustomScrollListener;
import com.epm.app.mvvm.comunidad.adapter.NotificationsRecyclerAdapter;
import com.epm.app.mvvm.comunidad.adapter.swipeHelperTest;
import com.epm.app.mvvm.comunidad.network.response.notifications.ReceivePushNotification;
import com.epm.app.mvvm.comunidad.viewModel.ConfigurationNotificationViewModel;
import com.epm.app.mvvm.comunidad.viewModel.NotificationCenterViewModel;
import com.epm.app.mvvm.turn.views.activities.ChannelsOfAttentionActivity;
import com.epm.app.mvvm.turn.views.activities.CustomerServiceMenuOptionActivity;
import com.epm.app.mvvm.comunidad.views.fragments.ConfigurationFragment;
import com.epm.app.mvvm.turn.views.activities.ShiftInformationActivity;
import app.epm.com.utilities.helpers.InformationOffice;
import com.epm.app.view.activities.EstacionesDeGasActivity;
import com.epm.app.view.activities.EventosActivity;
import com.epm.app.view.activities.LandingActivity;
import com.epm.app.view.activities.LineasDeAtencionActivity;
import com.epm.app.view.activities.NoticiasActivity;
import com.google.gson.Gson;

import java.util.List;


import app.epm.com.contacto_transparente_presentation.view.activities.HomeContactoTransparenteActivity;
import app.epm.com.factura_presentation.view.activities.ConsultFacturaActivity;
import app.epm.com.factura_presentation.view.activities.FacturasConsultadasActivity;
import app.epm.com.reporte_danios_presentation.view.activities.ServiciosDanioActivity;
import app.epm.com.reporte_fraudes_presentation.view.activities.ServicesDeFraudesActivity;
import app.epm.com.utilities.helpers.UtilitiesDate;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.INotificationObserver;
import app.epm.com.utilities.utils.NotificationManager;


import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class NotificationCenterActivity extends BaseActivityWithDI implements NotificationsRecyclerAdapter.OnNotificationRecyclerListener, ConfigurationFragment.OnConfigurationRecyclerListener , INotificationObserver.IShift{


    List<ReceivePushNotification> receivePushNotificationList;
    NotificationsRecyclerAdapter adapter;
    LinearLayoutManager linearLayoutManager;
    ConfigurationFragment configurationFragment;
    ConfigurationNotificationViewModel configurationNotificationViewModel;
    private ActivityNotificationCenterBinding binding;
    private NotificationCenterViewModel centerViewModel;
    private Toolbar toolbarApp;
    private int position;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_notification_center);
        this.configureDagger();
        loadViewModel();
        binding.setNotificationViewModel((NotificationCenterViewModel) centerViewModel);
        createProgressDialog();
        configurationFragment = ConfigurationFragment.newInstance(configurationNotificationViewModel, this, this);
        loadDrawerLayout(R.id.generalMenuConfiguration);
        toolbarApp = (Toolbar) binding.toolbarNotification;
        loadToolbar();
        loadRecycler();
        showError();
        loadNotifications();
        backActivity();
        attachNotification();
        binding.vf.setDisplayedChild(1);

        binding.generalMenuConfiguration.addDrawerListener(new DrawerLayout.DrawerListener() {
            @Override
            public void onDrawerSlide(@NonNull View view, float v) {

            }

            @Override
            public void onDrawerOpened(@NonNull View view) {

            }

            @Override
            public void onDrawerClosed(@NonNull View view) {
                binding.vf.setDisplayedChild(1);
            }

            @Override
            public void onDrawerStateChanged(int i) {

            }
        });

    }

    private void loadViewModel() {
        centerViewModel = ViewModelProviders.of(this, viewModelFactory).get(NotificationCenterViewModel.class);
        configurationNotificationViewModel = ViewModelProviders.of(this, viewModelFactory).get(ConfigurationNotificationViewModel.class);
    }

    private void attachNotification() {
        NotificationManager.getInstance().getNotificationSubject().attach(this);
        NotificationManager.getInstance().getNotificationSubject().attachShiftStatusObservers(this);

    }

    private void backActivity() {
        binding.backPressed.setOnClickListener(v -> onBackPressed());
    }


    private void startActivityUpdate(int position) {
        String type;
        customSharedPreferences.addBoolean(Constants.SHOW_BELL,false);
        if (receivePushNotificationList != null) {
            type = receivePushNotificationList.get(position).getTemplateOneSignal().getValue();
            switch (type) {
                case Constants.MODULE_NOTICIAS:
                    startActivityNotices();
                    break;
                case Constants.MODULE_LINEAS_DE_ATENCION:
                    startActivityLineAtention();
                    break;
                case Constants.MODULE_SERVICIO_AL_CLIENTE:
                    startActivityCustomerService();
                    break;
                case Constants.MODULE_REPORTE_FRAUDES:
                    startActivityReportsFrauds();
                    break;
                case Constants.MODULE_FACTURA:
                    starActivityFacture();
                    break;
                case Constants.MODULE_CONTACTO_TRANSPARENTE:
                    startActivityContact();
                    break;
                case Constants.MODULE_REPORTE_DANIOS:
                    startActivityReportsDanios();
                    break;
                case Constants.MODULE_EVENTOS:
                    startActivityEvents();
                    break;
                case Constants.MODULE_ESTACIONES_DE_GAS:
                    startActivityStationsOfGas();
                    break;
                case Constants.MODULE_DE_ALERTASHIDROITUANGO:
                    startActivityRedAlert();
                    break;
                case Constants.MODULE_TURNO_ATENDIDO:
                case Constants.MODULE_TURNO_ABANDONADO:
                    startDashboardClient();
                    break;
                case Constants.MODULE_TURNO_AVANCE:
                    startShiftInformation();
                    break;
                default:
                    break;
            }
        }

    }

    private void loadRecycler() {
        linearLayoutManager = new LinearLayoutManager(this);
        binding.notificationRecyclerView.setAdapter(adapter);
        binding.notificationRecyclerView.setLayoutManager(linearLayoutManager);
        adapter = new NotificationsRecyclerAdapter(NotificationCenterActivity.this, centerViewModel.getListNotification(), this, getResources(), binding.content);
        binding.notificationRecyclerView.addOnScrollListener(new CustomScrollListener((NotificationCenterViewModel) centerViewModel, linearLayoutManager));
        binding.notificationRecyclerView.getAdapter();
        binding.notificationRecyclerView.setAdapter(adapter);
        swipeItem();
    }


    private void loadNotifications() {
        deletePosition();
        centerViewModel.loadNotifications();
        centerViewModel.getProgress().observe(this, progress -> {
            if (progress != null && progress) {
                showProgressDIalog(R.string.text_please_wait);
            } else {
                dismissProgressDialog();
            }
        });
        centerViewModel.getLoadNotifications().observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {
                receivePushNotificationList = centerViewModel.getListNotification();
                adapter.setItems(receivePushNotificationList);
            }
        });

    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        customSharedPreferences.addBoolean(Constants.SHOW_BELL,true);
    }

    private void swipeItem() {
        ItemTouchHelper touchHelper = new ItemTouchHelper(new swipeHelperTest(0, ItemTouchHelper.LEFT, adapter, this) {
            @Override
            public void onSwiped(@NonNull RecyclerView.ViewHolder viewHolder, int i) {
                super.onSwiped(viewHolder, i);
                setPosition(viewHolder.getAdapterPosition());
                deleteNotification(viewHolder.getAdapterPosition());
            }
        });
        touchHelper.attachToRecyclerView(binding.notificationRecyclerView);
    }

    private void deletePosition() {
        centerViewModel.getDelete().observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {
                adapter.deleteItem(position);
            }
        });
    }


    private void showError() {
        centerViewModel.showError();
        centerViewModel.getExpiredToken().observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {
                showAlertDialogUnauthorized(centerViewModel.getError().getValue().getTitle(), centerViewModel.getError().getValue().getMessage());
            }
        });
        centerViewModel.getError().observe(this, errorMessage -> {
            if(errorMessage != null) {
                validateShowError(errorMessage.getTitle(),errorMessage.getMessage());
                adapter.notifyDataSetChanged();
            }
        });
    }

    public void deleteNotification(int position) {
        centerViewModel.deleteNotificationsPush(position);
    }

    private void validateShowError(int title, int message) {
        if (centerViewModel.isTryAgain()) {
            showAlertDialogTryAgain(title, message, R.string.text_intentar, R.string.text_cancelar);
        } else {
            showAlertDialog(title, message);
        }
    }

    private void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> {
            if (!getUsuario().isInvitado() && title == R.string.title_appreciated_user) {
                getCustomAlertDialog().showAlertDialog(getUsuario().getNombres(), message, false, positive, (dialogInterface, i) -> {
                    tryAgain();
                }, negative, (dialogInterface, i) -> onBackPressed(), null);
            } else {
                getCustomAlertDialog().showAlertDialog(title, message, false, positive, (dialogInterface, i) -> {
                    tryAgain();
                }, negative, (dialogInterface, i) -> onBackPressed(), null);
            }
        });
    }

    private void tryAgain() {
        configurationFragment.tryAgain();
        centerViewModel.loadNotifications();
    }

    private void showAlertDialog(final int title, final int message) {
        runOnUiThread(() -> {
            if (!getUsuario().isInvitado() && title == R.string.title_appreciated_user) {
                showAlertDialogGeneralInformationOnUiThread(getUsuario().getNombres(), message);
            } else {
                showAlertDialogGeneralInformationOnUiThread(title, message);
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_configuration_notification, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        if (id == R.id.menuConfiguration && binding.generalMenuConfiguration != null) {
            binding.vf.setDisplayedChild(2);
            binding.generalMenuConfiguration.openDrawer(GravityCompat.END);
            binding.notificationRecyclerView.setClickable(false);
        }
        if (id == R.id.menuGeneral_menuHamburguer && generalDrawerLayout != null) {
            binding.vf.setDisplayedChild(1);
            binding.generalMenuConfiguration.openDrawer(GravityCompat.END);
        }
        return super.onOptionsItemSelected(item);
    }


    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar(toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(view -> onBackPressed());
    }

    private void setPosition(int position) {
        this.position = position;
    }


    @Override
    public void onItemClick(int position) {
        if (getValidateInternet().isConnected()) {
            startActivityUpdate(position);
            adapter.updateNotification();
            centerViewModel.updateNotification(position);
        } else {
            showAlertDialog(R.string.title_appreciated_user, R.string.text_validate_internet);
        }
    }

    //llamados a las actividades de las diferentes notificaciones

    private void startActivityStationsOfGas() {
        Intent intent = new Intent(NotificationCenterActivity.this, EstacionesDeGasActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void startActivityRedAlert() {
        Intent intent = new Intent(NotificationCenterActivity.this, RedAlertInformationActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void startDashboardClient() {
        Intent intent = new Intent(NotificationCenterActivity.this, CustomerServiceMenuOptionActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void startShiftInformation() {
        InformationOffice informationOffice = new Gson().fromJson(getCustomSharedPreferences().getString(Constants.INFORMATION_OFFICE_JSON),InformationOffice.class);
        if (informationOffice != null){
            validateTurnDate(informationOffice);
        }else{
            goToLanding();
        }
    }

    private void validateTurnDate(InformationOffice informationOffice){
        if(informationOffice.getTurnDate() == null || UtilitiesDate.subtractDates(informationOffice.getTurnDate(),UtilitiesDate.getDate()) <= 0){
            Intent intent = new Intent(NotificationCenterActivity.this, ShiftInformationActivity.class);
            intent.putExtra(Constants.INFORMATION_OFFICE, informationOffice);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        }else{
            customSharedPreferences.deleteValue(Constants.ASSIGNED_TRUN);
            customSharedPreferences.deleteValue(Constants.INFORMATION_OFFICE_JSON);
            goToAtentionChannels();
        }
    }

    private void goToLanding(){
        Intent intent = new Intent(NotificationCenterActivity.this, LandingActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void goToAtentionChannels(){
        Intent intent = new Intent(NotificationCenterActivity.this, ChannelsOfAttentionActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void startActivityEvents() {
        Intent intent = new Intent(NotificationCenterActivity.this, EventosActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void startActivityReportsFrauds() {
        Intent intent = new Intent(NotificationCenterActivity.this, ServicesDeFraudesActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void startActivityCustomerService() {
        Intent intent = new Intent(NotificationCenterActivity.this, CustomerServiceMenuOptionActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void startActivityNotices() {
        Intent intent = new Intent(NotificationCenterActivity.this, NoticiasActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void startActivityLineAtention() {
        Intent intent = new Intent(NotificationCenterActivity.this, LineasDeAtencionActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void starActivityFacture() {
        Intent intent;
        if (!getUsuario().isInvitado()) {
            sendReportToGoogleAnalytics(Constants.FACTURA_REGISTRADO);
            intent = new Intent(this, FacturasConsultadasActivity.class);
        } else {
            sendReportToGoogleAnalytics(Constants.FACTURA_INVITADO);
            intent = new Intent(this, ConsultFacturaActivity.class);
            intent.putExtra(Constants.TRUE, true);
        }
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void startActivityReportsDanios() {
        Intent intent = new Intent(this, ServiciosDanioActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void startActivityContact() {
        Intent intent = new Intent(this, HomeContactoTransparenteActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    @Override
    public void onItemBack() {
        binding.generalMenuConfiguration.closeDrawer(GravityCompat.END);
    }

    @Override
    public void showError(int titleError, int error) {
        showAlertDialog(titleError,error);
    }

    @Override
    public void showErrorUnauthorized(int titleError, int error) {
        showAlertDialogUnauthorized(titleError,error);
    }

    @Override
    public void updateState(String template) {
        centerViewModel.newNotification(template);
    }


}
