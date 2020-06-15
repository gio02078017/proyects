package com.epm.app.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.support.v4.app.Fragment;
import android.support.v7.widget.CardView;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.View;
import android.widget.TextView;

import com.epm.app.business_models.business_models.Usuario;

import com.epm.app.R;

import app.epm.com.contacto_transparente_presentation.view.activities.HomeContactoTransparenteActivity;
import app.epm.com.container_domain.business_models.Authoken;

import com.epm.app.dependency_injection.DomainModule;

import app.epm.com.container_domain.business_models.InformacionEspacioPromocional;
import app.epm.com.factura_presentation.view.activities.ConsultFacturaActivity;
import app.epm.com.factura_presentation.view.activities.FacturasConsultadasActivity;

import com.epm.app.menu_presentation.view.fragments.MenuFragment;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import com.epm.app.mvvm.comunidad.views.activities.NotificationCenterActivity;
import com.epm.app.mvvm.comunidad.views.activities.RedAlertInformationActivity;
import com.epm.app.mvvm.comunidad.views.activities.SplashActivity;
import com.epm.app.mvvm.turn.views.activities.CustomerServiceMenuOptionActivity;
import com.epm.app.mvvm.turn.views.activities.ShiftInformationActivity;
import app.epm.com.utilities.helpers.InformationOffice;
import com.epm.app.presenters.LandingPresenter;

import app.epm.com.reporte_danios_presentation.view.activities.ServiciosDanioActivity;
import app.epm.com.reporte_fraudes_presentation.view.activities.ServicesDeFraudesActivity;
import app.epm.com.security_presentation.view.activities.RegisterLoginActivity;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.INotificationObserver;
import app.epm.com.utilities.utils.NotificationManager;
import app.epm.com.utilities.view.activities.BaseActivity;
import dagger.android.AndroidInjection;
import dagger.android.AndroidInjector;
import dagger.android.DispatchingAndroidInjector;
import dagger.android.support.HasSupportFragmentInjector;

import com.epm.app.view.views_activities.ILandingView;
import com.google.gson.Gson;

import javax.inject.Inject;

public class LandingActivity extends BaseActivity<LandingPresenter> implements ILandingView, HasSupportFragmentInjector, INotificationObserver.IShift {

    private Toolbar toolbarApp;
    private CardView landing_ivNoticias;
    private TextView badgeAssignedTurn;
    private CardView landing_ivLineasDeAtencion;
    private CardView landing_ivServicioAlCliente;
    private CardView landing_ivReporteDeFraudes;
    private CardView landing_ivFacturaWeb;
    private CardView landing_ivContactoTransparente;
    private CardView landing_ivReporteDeDanios;
    private CardView landing_ivEventosEpm;
    private CardView landing_ivEstacionesDeGas;
    private CardView landing_ivAlertasItuango;
    private boolean calledFromAnotherModule;
    private MenuFragment menuFragment;
    @Inject
    DispatchingAndroidInjector<Fragment> dispatchingAndroidInjector;
    @Inject
    public SubscriptionRepository subscriptionRepository;
    @Inject
    public NotificationsRepository notificationsRepository;
    private boolean validateDoubleClick;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_landing);
        this.configureDagger();
        setPresenter(new LandingPresenter(DomainModule.getSecurityBLInstance(getCustomSharedPreferences()), subscriptionRepository));
        getPresenter().inject(this, getValidateInternet(), getCustomSharedPreferences(), notificationsRepository);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        createProgressDialog();
        loadViews();
        sendReportGoogleAnalyticsLanding();
        calledFromAnotherModule = getIntent().getBooleanExtra(Constants.CALLED_FROM_ANOTHER_MODULE, false);
        Bundle bundle = getIntent().getExtras();
        openLogin(bundle);
        rateApp();
        getPresenter().getSubscription(getUsuario());
        startProcessToOpenEspacioPromocional();
        setDataActivity();
        NotificationManager.getInstance().getNotificationSubject().attachShiftStatusObservers(this);
    }


    private void startProcessToOpenEspacioPromocional() {
        if (getCustomSharedPreferences().getString(Constants.EXIST_NOTIFICATION) == null) {
            new Handler().postDelayed(() -> {
                if (!calledFromAnotherModule) {
                    getPresenter().validateInternetToGetEspacioPromocional();
                    getCustomSharedPreferences().addString(Constants.EXIST_NOTIFICATION, null);
                }
            }, 1000);
        }
    }

    @Override
    protected void onStart() {
        super.onStart();
        validateDoubleClick = false;
    }

    @Override
    protected void onResume() {
        super.onResume();
        customSharedPreferences.addBoolean(Constants.SHOW_BELL,true);
        menuFragment.showViews();
        showBadgeAssignedTurn();
    }

    @Override
    protected void onRestart() {
        if (getCustomSharedPreferences().getString(Constants.EXIST_NOTIFICATION) != null) {
            ////// getPresenter().validateInternetToGetEspacioPromocional();
            getCustomSharedPreferences().addString(Constants.EXIST_NOTIFICATION, null);
        }
        super.onRestart();
    }

    private void openLogin(Bundle bundle) {
        boolean showLogin = bundle.getBoolean(Constants.SHOW_LOGIN);
        if (showLogin) {
            startLoginWhenToCloseSessionOnUiThread();
        }
    }


    private void rateApp() {
        int resultCode = 0;
        resultCode = getIntent().getIntExtra(Constants.SHOW_ALERT_RATE, resultCode);
        if (resultCode == Constants.RATE_APP) {
            validateShowAlertQualifyApp();
        }
    }

    private void sendReportGoogleAnalyticsLanding() {
        if (getCustomSharedPreferences().getString(Constants.INVITADO).equals(Constants.FALSE)) {
            sendReportToGoogleAnalytics(Constants.LANDING_REGISTRADO);
        } else {
            sendReportToGoogleAnalytics(Constants.LANDING_INVITADO);
        }
    }

    /**
     * Permita validar el usuario
     */
    private void validarUsuario() {
        String invitado = getCustomSharedPreferences().getString(Constants.INVITADO);

        if (invitado.equals(Constants.FALSE)) {
            //validateToken();
            getCustomSharedPreferences().addString(Constants.INVITADO, Constants.TRUE);
        }
    }


    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == Constants.DEFAUL_REQUEST_CODE) {
            if (data != null && (Usuario) data.getSerializableExtra(Constants.USUARIO) != null) {
                setUsuario((Usuario) data.getSerializableExtra(Constants.USUARIO));
            }
            switch (resultCode) {
                case Constants.REGISTER_LOGIN_BACK:
                    if (getCustomSharedPreferences().getString(Constants.TOKEN) == null || getCustomSharedPreferences().getString(Constants.TOKEN).isEmpty() || getUsuario() == null) {
                        getPresenter().validateInternetToGetGuestLogin();
                    }
                    break;
                case Constants.LOGIN:
                    finish();
                    break;
                case Constants.ACTION_LOG_OUT:
                    loadViewsInMenuAgain();
                    break;
                case Constants.SESSION:
                    loginGuest();
                    break;
                case Constants.UNAUTHORIZED:
                    loginGuest();
                    break;
                case Constants.ACTION_TO_EXECUTE_FROM_ONE_SIGNAL:
                    onClickListenerToTheControlLandingIvFacturaWeb();
                    break;
                case Constants.ACTION_TO_EXECUTE_NOTICIAS:
                    onClickListenerToTheControlLandingIvNoticias();
                    break;
                case Constants.ACTION_TO_EXECUTE_LINEAS:
                    onClickListenerToTheControlLandingIvLineasDeAtencion();
                    break;
                case Constants.ACTION_TO_EXECUTE_SERVICIO_AL_CLIENTE:
                    onClickListenerToTheControlLandingIvServicioAlCliente();
                    break;
                case Constants.ACTION_TO_EXECUTE_REPORTE_FRAUDES:
                    onClickListenerToTheControlLandingIvReporteFraudes();
                    break;
                case Constants.ACTION_TO_EXECUTE_CONTACTO_TRANSPARENTE:
                    startActivityContacto();
                    break;
                case Constants.ACTION_TO_EXECUTE_DANIOS:
                    startActivityReporte();
                    break;
                case Constants.ACTION_TO_EVENTOS:
                    onClickListenerToTheControlLandingIvEventos();
                    break;
                case Constants.ACTION_TO_ESTACIONES_DE_GAS:
                    onClickListenerToTheControlLandingIvEstacionesDeGas();
                    break;
                case Constants.ACTION_TO_ALERTAS_HIDROITUANGO:
                    onClickListenerToTheControlLandingIvAlertasHidroituango();
                    break;
                case Constants.ACTION_TO_RED_ALERTAS_HIDROITUANGO:
                    onClickListenerToRedAlertasHidroituango();
                    break;
                case Constants.ACTION_TO_ATTENDED_SHIFT:
                    onClickListenerToAttendedShift();
                    break;
                case Constants.ACTION_TO_TURN_ADVANCE:
                    onClickListenerToTurnAdvance();
                    break;
                default:
                    break;
            }
        }
    }


    @Override
    public void onBackPressed() {
        moveTaskToBack(true);
    }

    /**
     * Actualiza vistas del menu.
     */
    private void loadViewsInMenuAgain() {
        setUsuario(null);
        menuFragment.showViews();
        loginGuest();
    }

    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        landing_ivNoticias = (CardView) findViewById(R.id.landing_ivNoticias);
        badgeAssignedTurn = (TextView) findViewById(R.id.badgeAssignedTurn);
        landing_ivLineasDeAtencion = (CardView) findViewById(R.id.landing_ivLineasDeAtencion);
        landing_ivServicioAlCliente = (CardView) findViewById(R.id.landing_ivServicioAlCliente);
        landing_ivReporteDeFraudes = (CardView) findViewById(R.id.landing_ivReporteDeFraudes);
        landing_ivFacturaWeb = (CardView) findViewById(R.id.landing_ivFacturaWeb);
        landing_ivContactoTransparente = (CardView) findViewById(R.id.landing_ivContactoTransparente);
        landing_ivReporteDeDanios = (CardView) findViewById(R.id.landing_ivReporteDeDanios);
        landing_ivEventosEpm = (CardView) findViewById(R.id.landing_ivEventosEpm);
        landing_ivEstacionesDeGas = (CardView) findViewById(R.id.landing_ivEstacionesDeGas);
        landing_ivAlertasItuango = (CardView) findViewById(R.id.landing_ivalertashidroituango);
        menuFragment = (MenuFragment) getSupportFragmentManager().findFragmentById(R.id.menu_landing_small);
        loadToolbar();
        loadListenerToTheControls();
        showBadgeAssignedTurn();
    }

    private void showBadgeAssignedTurn() {
        String assignedTurn = customSharedPreferences.getString(Constants.ASSIGNED_TRUN);
        if (assignedTurn != null) {
            badgeAssignedTurn.setVisibility(View.VISIBLE);
            String textTurnDashboard = assignedTurn;
            String[] parts = assignedTurn.split("-");
            if (parts.length > 1) {
                textTurnDashboard = parts[1].length() > 1 ? String.format(getString(R.string.text_turn_dashboard), parts[0], parts[1]) : assignedTurn;
            }
            badgeAssignedTurn.setText(textTurnDashboard);
        } else {
            badgeAssignedTurn.setVisibility(View.GONE);
        }
    }


    private void loadToolbar() {
        //CustomTextViewNormal customTextViewNormal = (CustomTextViewNormal) toolbarApp.findViewById(R.id.toolbar_title);
        //customTextViewNormal.setText(R.string.title_estamos_ahi);
        setSupportActionBar(toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }

    private void loadListenerToTheControls() {
        loadOnClickListenerToTheControlLandingIvFacturaWeb();
        loadOnClickListenerToTheControllandingIvReporteDeDanios();
        loadClickListenerToTheControlLandingIvContactoTransparente();
        loadClickListenerToTheControlLandingIvNoticias();
        loadClickListenerToTheControlLandingIvEventos();
        loadClickListenerToTheControlLandingImageViewLineasDeAtencion();
        loadClickListenerToTheControlLandingImageViewReporteDeFraudes();
        loadClickListenerToTheControlLandingImageViewServicioAlCliente();
        loadClickListenerToTheControlLandingImageViewEstacionesDeGas();
        loadClickListenerToTheControlLandingImageViewAlertasHidroItuango();
    }


    private void loadClickListenerToTheControlLandingImageViewEstacionesDeGas() {
        landing_ivEstacionesDeGas.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onClickListenerToTheControlLandingIvEstacionesDeGas();
            }
        });
    }

    private void loadClickListenerToTheControlLandingImageViewAlertasHidroItuango() {
        landing_ivAlertasItuango.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickListenerToTheControlLandingIvAlertasHidroituango();
            }
        });
    }

    private void loadClickListenerToTheControlLandingImageViewServicioAlCliente() {
        landing_ivServicioAlCliente.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onClickListenerToTheControlLandingIvServicioAlCliente();
            }
        });
    }

    private void loadClickListenerToTheControlLandingImageViewReporteDeFraudes() {
        landing_ivReporteDeFraudes.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClickListenerToTheControlLandingIvReporteFraudes();
            }
        });
    }

    private void loadClickListenerToTheControlLandingImageViewLineasDeAtencion() {
        landing_ivLineasDeAtencion.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onClickListenerToTheControlLandingIvLineasDeAtencion();
            }
        });
    }

    private void loadClickListenerToTheControlLandingIvEventos() {
        landing_ivEventosEpm.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onClickListenerToTheControlLandingIvEventos();
            }
        });
    }

    private void loadClickListenerToTheControlLandingIvNoticias() {
        landing_ivNoticias.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onClickListenerToTheControlLandingIvNoticias();
            }
        });
    }

    private void loadClickListenerToTheControlLandingIvContactoTransparente() {
        landing_ivContactoTransparente.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startActivityContacto();
            }
        });
    }

    private void startActivityContacto() {
        if (getUsuario().isInvitado() == false) {
            sendReportToGoogleAnalytics(Constants.CONTACTO_TRASPARENTE_REGISTRADO);
        } else {
            sendReportToGoogleAnalytics(Constants.CONTACTO_TRASPARENTE_INVITADO);
        }

        Intent intent = new Intent(this, HomeContactoTransparenteActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void loadOnClickListenerToTheControllandingIvReporteDeDanios() {
        landing_ivReporteDeDanios.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startActivityReporte();
            }
        });
    }

    private void startActivityReporte() {
        if (getPresenter().checkExecutionInAndroidApiActualVersion()) {
            if (getUsuario().isInvitado() == false) {
                sendReportToGoogleAnalytics(Constants.DANIOS_REGISTRADO);
            } else {
                sendReportToGoogleAnalytics(Constants.DANIOS_INVITADO);
            }

            Intent intent = new Intent(this, ServiciosDanioActivity.class);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        } else {
            // TODO : Adds message without permission
            getPresenter().showDialogToUserWithApiLevelToLower();
        }
    }

    private void loadOnClickListenerToTheControlLandingIvFacturaWeb() {
        landing_ivFacturaWeb.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onClickListenerToTheControlLandingIvFacturaWeb();
            }
        });
    }

    public void onClickListenerToTheControlLandingIvFacturaWeb() {
        Intent intent;
        if (getUsuario() != null && getUsuario().isInvitado() == false) {
            sendReportToGoogleAnalytics(Constants.FACTURA_REGISTRADO);
            intent = new Intent(this, FacturasConsultadasActivity.class);
        } else {
            sendReportToGoogleAnalytics(Constants.FACTURA_INVITADO);
            intent = new Intent(this, ConsultFacturaActivity.class);
            intent.putExtra(Constants.TRUE, true);
        }
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    @Override
    public void manageActionFactura() {
        onClickListenerToTheControlLandingIvFacturaWeb();
    }

    @Override
    public void manageActionNoticias() {
        onClickListenerToTheControlLandingIvNoticias();
    }

    @Override
    public void manageActionLineas() {
        onClickListenerToTheControlLandingIvLineasDeAtencion();
    }

    @Override
    public void manageActionServicioAlCliente() {
        onClickListenerToTheControlLandingIvServicioAlCliente();
    }

    @Override
    public void manageActionReporteFraudes() {
        onClickListenerToTheControlLandingIvReporteFraudes();
    }

    @Override
    public void manageActionContactoTransparente() {
        startActivityContacto();
    }

    @Override
    public void manageActionReporteDanios() {
        startActivityReporte();
    }

    @Override
    public void manageActionEventos() {
        onClickListenerToTheControlLandingIvEventos();
    }

    @Override
    public void manageActionEstacionesDeGas() {
        onClickListenerToTheControlLandingIvEstacionesDeGas();
    }


    @Override
    public void manageActionAlertasHidroitunago() {
        onClickListenerToRedAlertasHidroituango();
    }

    @Override
    public void manageActionAttendedShift() {
        onClickListenerToAttendedShift();
    }

    /*@Override
    public void manageActionTurnAdvance() {
        onClickListenerToTurnAdvance();
    }*/


    private void onClickListenerToTheControlLandingIvEstacionesDeGas() {
        if (getPresenter().checkExecutionInAndroidApiActualVersion()) {
            Intent intent = new Intent(LandingActivity.this, EstacionesDeGasActivity.class);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        } else {
            // TODO : Adds message without permission
            getPresenter().showDialogToUserWithApiLevelToLower();
        }
    }

    private void onClickListenerToTheControlLandingIvAlertasHidroituango() {
        Intent intent = new Intent(LandingActivity.this, SplashActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }


    private void onClickListenerToRedAlertasHidroituango() {
        Intent intent = new Intent(getApplication(), RedAlertInformationActivity.class);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void onClickListenerToAttendedShift() {
        Intent intent = new Intent(getApplication(), CustomerServiceMenuOptionActivity.class);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void onClickListenerToTurnAdvance() {
        InformationOffice informationOffice = new Gson().fromJson(getCustomSharedPreferences().getString(Constants.INFORMATION_OFFICE_JSON),InformationOffice.class);
        Intent intent;
        if (informationOffice != null){
            intent = new Intent(this, ShiftInformationActivity.class);
            intent.putExtra(Constants.INFORMATION_OFFICE, informationOffice);
        }else{
            intent = new Intent(this, NotificationCenterActivity.class);
        }
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void onClickListenerToTheControlLandingIvEventos() {
        if (getPresenter().checkExecutionInAndroidApiActualVersion()) {
            Intent intent = new Intent(LandingActivity.this, EventosActivity.class);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        } else {
            // TODO : Adds message without permission
            getPresenter().showDialogToUserWithApiLevelToLower();
        }
    }

    private void onClickListenerToTheControlLandingIvReporteFraudes() {
        if (getPresenter().checkExecutionInAndroidApiActualVersion()) {
            Intent intent = new Intent(LandingActivity.this, ServicesDeFraudesActivity.class);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        } else {
            // TODO : Adds message without permission
            getPresenter().showDialogToUserWithApiLevelToLower();
        }
    }

    private void onClickListenerToTheControlLandingIvServicioAlCliente() {
        if(!validateDoubleClick){
            Intent intent = new Intent(LandingActivity.this, CustomerServiceMenuOptionActivity.class);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
            validateDoubleClick = true;
        }
    }

    private void onClickListenerToTheControlLandingIvNoticias() {
        if (getPresenter().checkExecutionInAndroidApiActualVersion()) {
            Intent intent = new Intent(LandingActivity.this, NoticiasActivity.class);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        } else {
            // TODO : Adds message without permission
            getPresenter().showDialogToUserWithApiLevelToLower();
        }
    }

    private void onClickListenerToTheControlLandingIvLineasDeAtencion() {
        Intent intent = new Intent(LandingActivity.this, LineasDeAtencionActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    @Override
    public void setDataUser(Authoken authoken) {
        setUsuario(authoken.getUsuario());
        getUsuario().setInvitado(authoken.isInvitado());
        getCustomSharedPreferences().addString(Constants.TOKEN, authoken.getUsuario().getToken());
        runOnUiThread(() -> {
            menuFragment.showViews();
            openModuleOfNotification();
        });
    }

    public void setDataActivity() {
        runOnUiThread(() -> {
            menuFragment.showViews();
            openModuleOfNotification();
        });
    }

    private void openModuleOfNotification() {
        if (getCustomSharedPreferences().getString(Constants.ACTION_TO_EXECUTE) != null) {
            //customSharedPreferences.addBoolean(Constants.SHOW_BELL,false);
            String actionToExecute = getCustomSharedPreferences().getString(Constants.ACTION_TO_EXECUTE);
            switch (actionToExecute) {
                case Constants.TYPE_FACTURA_NOTIFICATION:
                    onClickListenerToTheControlLandingIvFacturaWeb();
                    break;
                case Constants.MODULE_NOTICIAS:
                    onClickListenerToTheControlLandingIvNoticias();
                    break;
                case Constants.MODULE_LINEAS_DE_ATENCION:
                    onClickListenerToTheControlLandingIvLineasDeAtencion();
                    break;
                case Constants.MODULE_SERVICIO_AL_CLIENTE:
                    onClickListenerToTheControlLandingIvServicioAlCliente();
                    break;
                case Constants.MODULE_REPORTE_FRAUDES:
                    onClickListenerToTheControlLandingIvReporteFraudes();
                    break;
                case Constants.MODULE_FACTURA:
                    onClickListenerToTheControlLandingIvFacturaWeb();
                    break;
                case Constants.MODULE_CONTACTO_TRANSPARENTE:
                    startActivityContacto();
                    break;
                case Constants.MODULE_REPORTE_DANIOS:
                    startActivityReporte();
                    break;
                case Constants.MODULE_EVENTOS:
                    onClickListenerToTheControlLandingIvEventos();
                    break;
                case Constants.MODULE_ESTACIONES_DE_GAS:
                    onClickListenerToTheControlLandingIvEstacionesDeGas();
                    break;
                case Constants.MODULE_DE_ALERTASHIDROITUANGO:
                    onClickListenerToRedAlertasHidroituango();
                    break;
                case Constants.MODULE_TURNO_ATENDIDO:
                    onClickListenerToAttendedShift();
                    break;
                case Constants.MODULE_TURNO_AVANCE:
                    onClickListenerToTurnAdvance();
                    break;
                default:
                    break;
            }
            getCustomSharedPreferences().addString(Constants.ACTION_TO_EXECUTE, null);
        }
    }

    @Override
    public void showAlertDialogTryAgain(final int title, final String message, final int positive, final int negative, final boolean loginGuest) {
        showAlertDialog(title, message, positive, negative, loginGuest);
    }

    @Override
    public void showAlertDialogTryAgain(int title, int message, int positive, int negative, boolean loginGest) {
        showAlertDialog(title, getString(message), positive, negative, loginGest);
    }

    @Override
    public void startEspacioPromocional(InformacionEspacioPromocional informacionEspacioPromocional) {
        Intent intent = new Intent(this, EspacioPromocionalActivity.class);
        getCustomSharedPreferences().addString(Constants.IMAGE_PROMOTIONAL_SPACE, informacionEspacioPromocional.getImagen());
        getCustomSharedPreferences().addInt(Constants.NAME_MODULE_PROMOTIONAL_SPACE, informacionEspacioPromocional.getModulo());
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void showAlertDialog(final int title, final String message, final int positive, final int negative, final boolean loginGuest) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false, positive, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                if (loginGuest) {
                    //getPresenter().validateInternetToGetGuestLogin();
                } else {
                    //getPresenter().validateInternetToGetAutoLogin(getCustomSharedPreferences().getString(Constants.TOKEN));
                }
            }
        }, negative, (dialogInterface, i) -> finish(), null));
    }

    @Override
    public void startLoginWhenToCloseSessionOnUiThread() {
        Intent intent = new Intent(this, RegisterLoginActivity.class);
        Bundle bundle = new Bundle();
        bundle.putBoolean(Constants.SHOW_LOGIN, true);
        intent.putExtras(bundle);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    public void loginGuest() {
        getPresenter().validateInternetToGetGuestLogin();
    }

    private void configureDagger() {
        AndroidInjection.inject(this);
    }

    @Override
    public AndroidInjector<Fragment> supportFragmentInjector() {
        return dispatchingAndroidInjector;
    }


    @Override
    public void updateState(String template) {
        runOnUiThread(() -> showBadgeAssignedTurn());

    }


}