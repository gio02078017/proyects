package com.epm.app.menu_presentation.view.fragments;

import android.content.Intent;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.Bundle;
import androidx.fragment.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.epm.app.menu_presentation.R;
import com.epm.app.menu_presentation.dependecy_injection.DomainModule;
import com.epm.app.menu_presentation.presenters.MenuFragmentPresenter;
import com.epm.app.menu_presentation.view.view_activity.IMenuFragmentView;

import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.utilities.helpers.CustomApplicationContext;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IMenuFragmentListener;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.INotificationObserver;
import app.epm.com.utilities.utils.NotificationManager;
import app.epm.com.utilities.view.fragments.BaseFragment;

/**
 * A simple {@link Fragment} subclass.
 */
public class MenuFragment extends BaseFragment<MenuFragmentPresenter> implements IMenuFragmentView, IMenuFragmentListener, INotificationObserver {

    private TextView menu_tvVersion;
    private TextView menu_tvLine1;
    private TextView menu_tvLine3;
    private TextView menu_tvLineNotification;
    private TextView numberNotifications;
    private LinearLayout menu_llStart;
    private LinearLayout menu_llMyProfile;
    private LinearLayout menu_llChangePassword;
    private LinearLayout menu_llSignIn;
    private LinearLayout menu_llRegister;
    private LinearLayout menu_llShareApp;
    private LinearLayout menu_llQualifyApp;
    private LinearLayout menu_llNotification;
    private Button menu_llLogOff;
    private boolean controlDobleClick = false;

    private ICustomSharedPreferences customSharedPreferences;

    public MenuFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_menu, container, false);
        NotificationManager.getInstance().getNotificationSubject().attach(this);
        this.customSharedPreferences = new CustomSharedPreferences(getBaseActivity());
        setPresenter(new MenuFragmentPresenter(DomainModule.getSecurityBLInstance(customSharedPreferences)));
        getPresenter().inject(this, getValidateInternet(), customSharedPreferences);
        CustomApplicationContext.getInstance().injectMenuFragmentListener(this);
        loadViews(view);
        loadAppVersion();
        createProgressDialog();
        showViews();
        return view;
    }

    @Override
    public void onResume() {
        super.onResume();
        controlDobleClick = false;
    }

    /**
     * Carga versión de la app.
     */
    private void loadAppVersion() {
        int verCode = 0;
        try {
            PackageInfo pInfo = getActivity().getPackageManager().getPackageInfo(getActivity().getPackageName(), 0);
            verCode = pInfo.versionCode;
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }
        customSharedPreferences = new CustomSharedPreferences(getBaseActivity());
        menu_tvVersion.setText(customSharedPreferences.getString(Constants.APP_VERSION));
    }

    /**
     * Muestra los items del menu dependiendo si esta logueado o no.
     */
    public void showViews() {
        if (userIsAuthenticated() != null && userIsAuthenticated().isInvitado() == false) {
            loadViewsAuthenticated();
        } else {
            loadViewsNoAuthenticated();
        }
        validateIfShowOrHidenCenterNotification();
    }

    private void validateIfShowOrHidenCenterNotification(){
        if (customSharedPreferences.getBoolean(Constants.SHOW_BELL)){
            menu_tvLineNotification.setVisibility(View.VISIBLE);
            menu_llNotification.setVisibility(View.VISIBLE);
        }else {
            menu_tvLineNotification.setVisibility(View.GONE);
            menu_llNotification.setVisibility(View.GONE);
        }
    }

    /**
     * Carga los items del menu como invitado.
     */
    private void loadViewsNoAuthenticated() {
        menu_llRegister.setVisibility(View.VISIBLE);
        menu_llSignIn.setVisibility(View.VISIBLE);
        menu_tvLine3.setVisibility(View.VISIBLE);
        menu_llMyProfile.setVisibility(View.GONE);
        menu_llLogOff.setVisibility(View.GONE);
        menu_llChangePassword.setVisibility(View.GONE);
        menu_tvLine1.setVisibility(View.GONE);
    }

    /**
     * Carga los items del menu como logueado.
     */
    private void loadViewsAuthenticated() {
        menu_llMyProfile.setVisibility(View.VISIBLE);
        menu_llLogOff.setVisibility(View.VISIBLE);
        menu_llChangePassword.setVisibility(View.VISIBLE);
        menu_tvLine1.setVisibility(View.VISIBLE);
        menu_llRegister.setVisibility(View.GONE);
        menu_llSignIn.setVisibility(View.GONE);
        menu_tvLine3.setVisibility(View.GONE);
    }

    /**
     * Carga los controles de la vista.
     *
     * @param view view.
     */
    private void loadViews(View view) {
        menu_tvVersion =  view.findViewById(R.id.menu_tvVersion);
        menu_tvLine1 =  view.findViewById(R.id.menu_tvLine1);
        menu_tvLine3 =  view.findViewById(R.id.menu_tvLine3);
        menu_tvLineNotification = view.findViewById(R.id.lineNotification);
        menu_llStart =  view.findViewById(R.id.menu_llStart);
        menu_llMyProfile =  view.findViewById(R.id.menu_llMyProfile);
        menu_llChangePassword =  view.findViewById(R.id.menu_llChangePassword);
        menu_llSignIn =  view.findViewById(R.id.menu_llSignIn);
        menu_llRegister =  view.findViewById(R.id.menu_llRegister);
        menu_llShareApp = view.findViewById(R.id.menu_llShareApp);
        menu_llQualifyApp =view.findViewById(R.id.menu_llQualifyApp);
        menu_llLogOff =  view.findViewById(R.id.menu_llLogOff);
        menu_llNotification =  view.findViewById(R.id.menu_llNotification);
        numberNotifications =  view.findViewById(R.id.numberNotifications);
        loadListenerToTheControls();
    }

    private void loadListenerToTheControls() {
        loadOnClickListenerToTheControlMenuLlStart();
        loadOnClickListenerToTheControlMenuLlMyProfile();
        loadOnClickListenerToTheControlMenuLlChangePassword();
        loadOnClickListenerToTheControlMenuLlSignIn();
        loadOnClickListenerToTheControlMenuLlRegister();
        loadOnClickListenerToTheControlMenuLlShareApp();
        loadOnClickListenerToTheControlMenuLlQualifyApp();
        loadOnClickListenerToTheControlMenuLlLogOff();
        loadOnClickListenerToTheControlMenuLlNotification();
    }

    private void loadOnClickListenerToTheControlMenuLlNotification() {
        if(customSharedPreferences.getInt(Constants.NUMBER_NOTIFICATIONS) != null){
           setNumberNotifications(customSharedPreferences.getInt(Constants.NUMBER_NOTIFICATIONS));
        }
        menu_llNotification.setOnClickListener(v -> NotificationsCenter());
    }

    private void NotificationsCenter(){
        if (isActivity(Constants.NOTIFICATION_CENTER_CLASS)) {
            getBaseActivity().hideMenu();
        } else {
            try {
                Class clazz = Class.forName(customSharedPreferences.getString(Constants.NOTIFICATION_CENTER_CLASS));
                Intent intent = new Intent(getBaseActivity(), clazz);
                intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                getBaseActivity().startActivityWithOutDoubleClick(intent);
            } catch (ClassNotFoundException e) {
                Log.e(Constants.EXCEPTION_STRING, e.toString());
            }
        }
    }

    private void loadOnClickListenerToTheControlMenuLlQualifyApp() {
        menu_llQualifyApp.setOnClickListener(v -> rateApp());
    }

    @Override
    public void rateApp() {
        try {
            startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse(Constants.URL_MARKET_APP_PLAY_STORE)));
        } catch (android.content.ActivityNotFoundException anfe) {
            startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse(Constants.URL_APP_PLAY_STORE)));
        }
    }

    private void loadOnClickListenerToTheControlMenuLlLogOff() {
        menu_llLogOff.setOnClickListener(v -> onClickListenerToTheControlMenuLlLogOff());
    }

    /**
     * Carga metodo para enviar datos de cerrar sesión.
     */
    private void onClickListenerToTheControlMenuLlLogOff() {
        EmailUsuarioRequest emailUsuarioRequest = new EmailUsuarioRequest();
        emailUsuarioRequest.setCorreoElectronico(getBaseActivity().getUsuario().getCorreoElectronico());
        getPresenter().validateInternetLogOut(emailUsuarioRequest,getBaseActivity().getUsuario());
    }

    /**
     * Actualiza vistas del menu.
     */
    private void loadViewsInMenuAgain() {
        setUsuario(null);
        showViews();
    }

    private void loadOnClickListenerToTheControlMenuLlShareApp() {
        menu_llShareApp.setOnClickListener(v -> onClickListenerToTheControlMenuLlShareApp());
    }

    private void onClickListenerToTheControlMenuLlShareApp() {
        Intent intent = new Intent(Intent.ACTION_SEND);
        intent.setType("text/plain");
        intent.putExtra(Intent.EXTRA_TEXT, Constants.MESSAGE_SHARE + " \n " + Constants.URL_APP_EPM_SHARE);
        if (!controlDobleClick){
           controlDobleClick = true;
            startActivityForResult(Intent.createChooser(intent, ""), Constants.REQUEST_CODE_SHARE);
        }
    }

    private void loadOnClickListenerToTheControlMenuLlRegister() {
        menu_llRegister.setOnClickListener(v -> onClickListenerToTheControlMenuLlRegister());
    }

    private void onClickListenerToTheControlMenuLlRegister() {
        if (isActivity(Constants.REGISTER_LOGIN_CLASS)) {
            getBaseActivity().hideMenu();
        } else {
            try {
                Class clazz = Class.forName(customSharedPreferences.getString(Constants.REGISTER_LOGIN_CLASS));
                Intent intent = new Intent(getBaseActivity(), clazz);
                Bundle bundle = new Bundle();
                bundle.putBoolean(Constants.SHOW_LOGIN, false);
                intent.putExtras(bundle);
                intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                getBaseActivity().startActivityWithOutDoubleClick(intent);
            } catch (ClassNotFoundException e) {
                Log.e(Constants.EXCEPTION_STRING, e.toString());

            }
        }
    }

    private void loadOnClickListenerToTheControlMenuLlSignIn() {
        menu_llSignIn.setOnClickListener(v -> onClickListenerToTheControlMenuLlSignIn());
    }

    private void onClickListenerToTheControlMenuLlSignIn() {
        if (isActivity(Constants.REGISTER_LOGIN_CLASS)) {
            getBaseActivity().hideMenu();
        } else {
            try {
                Class clazz = Class.forName(customSharedPreferences.getString(Constants.REGISTER_LOGIN_CLASS));
                Intent intent = new Intent(getBaseActivity(), clazz);
                Bundle bundle = new Bundle();
                bundle.putBoolean(Constants.SHOW_LOGIN, true);
                intent.putExtras(bundle);
                intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                getBaseActivity().startActivityWithOutDoubleClick(intent);
            } catch (ClassNotFoundException e) {
                Log.e(Constants.EXCEPTION_STRING, e.toString());
            }
        }
    }

    private void loadOnClickListenerToTheControlMenuLlChangePassword() {
        menu_llChangePassword.setOnClickListener(v -> onClickListenerToTheControlMenuLlChangePassword());
    }

    private void onClickListenerToTheControlMenuLlChangePassword() {
        if (isActivity(Constants.CHANGE_PSWORD_CLASS)) {
            getBaseActivity().hideMenu();
        } else {
            try {
                Class clazz = Class.forName(customSharedPreferences.getString(Constants.CHANGE_PSWORD_CLASS));
                Intent intent = new Intent(getBaseActivity(), clazz);
                intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                getBaseActivity().startActivityWithOutDoubleClick(intent);
            } catch (ClassNotFoundException e) {
                Log.e(Constants.EXCEPTION_STRING, e.toString());
            }
        }
    }

    private void loadOnClickListenerToTheControlMenuLlMyProfile() {
        menu_llMyProfile.setOnClickListener(v -> onClickListenerToTheControlMenuLlMyProfile());
    }

    private void onClickListenerToTheControlMenuLlMyProfile() {
        if (isActivity(Constants.MY_PROFILE_CLASS)) {
            getBaseActivity().hideMenu();
        } else {
            try {
                Class clazz = Class.forName(customSharedPreferences.getString(Constants.MY_PROFILE_CLASS));
                Intent intent = new Intent(getBaseActivity(), clazz);
                intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                getBaseActivity().startActivityWithOutDoubleClick(intent);
            } catch (ClassNotFoundException e) {
                Log.e(Constants.EXCEPTION_STRING, e.toString());
            }
        }
    }

    private void loadOnClickListenerToTheControlMenuLlStart() {
        menu_llStart.setOnClickListener(v -> onClickListenerToTheControlMenuLlStart());
    }

    private void onClickListenerToTheControlMenuLlStart() {
        if (isActivity(Constants.LANDING_CLASS)) {
            getBaseActivity().hideMenu();
        } else {
            try {
                Class clazz = Class.forName(customSharedPreferences.getString(Constants.LANDING_CLASS));
                Intent intent = new Intent(getBaseActivity(), clazz);
                intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                getBaseActivity().startActivityWithOutDoubleClick(intent);
            } catch (ClassNotFoundException e) {
                Log.e(Constants.EXCEPTION_STRING, e.toString());
            }
        }
    }

    /**
     * Valida si el landing esta abierto.
     *
     * @return landingClassName.
     */
    private boolean isActivity(String className) {
        className = customSharedPreferences.getString(className);
        return className != null && className.equals(getActivity().getClass().getName());
    }

    @Override
    public void startLoginWhenToCloseSessionOnUiThread() {
        getBaseActivity().setResultData(Constants.SESSION);
        startLandingActivity();
    }

    /**
     * Inicia la actividad landing.
     */
    private void startLandingActivity() {
        try {
            customSharedPreferences.deleteValue(Constants.TOKEN);
            Class clazz = Class.forName(customSharedPreferences.getString(Constants.LANDING_CLASS));
            Intent intent = new Intent(getBaseActivity(), clazz);
            Bundle bundle = new Bundle();
            bundle.putBoolean(Constants.SHOW_LOGIN, true);
            intent.putExtras(bundle);
            setUsuario(null);
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
            getBaseActivity().startActivityWithOutDoubleClick(intent);
        } catch (ClassNotFoundException e) {
            Log.e(Constants.EXCEPTION_STRING, e.toString());
        }
    }

    @Override
    public void logOut() {
        onClickListenerToTheControlMenuLlLogOff();
    }

    public void showViewsSomos() {
        menu_llRegister.setVisibility(View.GONE);
        menu_llSignIn.setVisibility(View.GONE);
        menu_tvLine3.setVisibility(View.GONE);
        menu_llMyProfile.setVisibility(View.GONE);
        menu_llLogOff.setVisibility(View.GONE);
        menu_llChangePassword.setVisibility(View.GONE);
        menu_tvLine1.setVisibility(View.GONE);

    }

    public void setNumberNotifications(int numberNotifications){
        if(this.numberNotifications.getText() != null && numberNotifications > 0){
            this.numberNotifications.setVisibility(View.VISIBLE);
            this.numberNotifications.setText(String.valueOf(numberNotifications));
        }else{
            this.numberNotifications.setVisibility(View.GONE);
        }
    }

    @Override
    public void updateCounter() {
        loadOnClickListenerToTheControlMenuLlNotification();
    }


}