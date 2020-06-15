package app.epm.com.security_presentation.view.activities;

import android.os.Bundle;
import androidx.viewpager.widget.ViewPager;
import androidx.appcompat.widget.Toolbar;
import android.view.Menu;
import android.view.View;
import android.widget.Button;

import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.view.adapters.RegisterLoginTabsAdapter;
import app.epm.com.security_presentation.view.views_activities.ILoginView;
import app.epm.com.security_presentation.view.views_activities.IRegisterView;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

public class RegisterLoginActivity extends BaseActivity {

    private Toolbar toolbarApp;
    private ViewPager registroLogin_viewPager;
    private Button registroLogin_btnRegistrarme;
    private Button registroLogin_btnIniciarSesion;
    private RegisterLoginTabsAdapter swipeRegisterLoginAdapter;
    private ILoginView loginView;
    private IRegisterView registerView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_registro_login);
        loadViews();
        swipeRegisterLoginAdapter = new RegisterLoginTabsAdapter(getSupportFragmentManager());
        registroLogin_viewPager.setAdapter(swipeRegisterLoginAdapter);
        fragmentInitiatedForTheMenu();
    }

    /**
     * Inicia fragmento iniciado por el menu.
     */
    private void fragmentInitiatedForTheMenu() {
        Validations validations = new Validations();
        Bundle bundle = getIntent().getExtras();
        boolean showLogin = bundle.getBoolean(Constants.SHOW_LOGIN);
        if (validations.validateNullParameterBoolean(showLogin) && showLogin) {
            showFragmentIniciarSesion();
        }
    }

    /**
     * Carga los controles de la vista.
     */
    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        registroLogin_viewPager = (ViewPager) findViewById(R.id.registroLogin_viewPager);
        registroLogin_btnRegistrarme = (Button) findViewById(R.id.registroLogin_btnRegistrarme);
        registroLogin_btnIniciarSesion = (Button) findViewById(R.id.registroLogin_btnIniciarSesion);
        loadListenerToTheControl();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        return false;
    }

    /**
     * Carga los Listener de los controles.
     */
    private void loadListenerToTheControl() {
        loadOnClickListenerToTheControlRegisterLoginBtnRegistrarme();
        loadOnClickListenerToTheControlRegisterLoginBtnIniciarSesion();
        loadaddOnPageChangeListenerRegisterLoginRegistroLoginViewPager();
        loadToolbar();
    }

    private void loadaddOnPageChangeListenerRegisterLoginRegistroLoginViewPager() {
        registroLogin_viewPager.addOnPageChangeListener(new ViewPager.OnPageChangeListener() {
            @Override
            public void onPageScrolled(int position, float positionOffset, int positionOffsetPixels) {
                //The method is not used.
            }

            @Override
            public void onPageSelected(int position) {
                if (position == 0) {
                    cleanLoginFields();
                } else {
                    cleanRegisterFields();
                }
            }

            @Override
            public void onPageScrollStateChanged(int state) {
                //The method is not used.
            }
        });
    }

    /**
     * Carga el Toolbar.
     */
    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onBackPressed();
            }
        });
    }

    @Override
    public void onBackPressed() {
        setResultData(Constants.REGISTER_LOGIN_BACK);
        finish();
    }

    /**
     * Carga OnClick al control registroLogin_btnRegistrarme.
     */
    private void loadOnClickListenerToTheControlRegisterLoginBtnRegistrarme() {
        registroLogin_btnRegistrarme.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showFragmentRegistrarme();
            }
        });
    }

    /**
     * Carga OnClick al control registroLogin_btnIniciarSesion.
     */
    private void loadOnClickListenerToTheControlRegisterLoginBtnIniciarSesion() {
        registroLogin_btnIniciarSesion.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showFragmentIniciarSesion();
            }
        });
    }

    /**
     * Permite mostrar los fragmentos.
     *
     * @param fragment fragment.
     */
    public void showFragment(int fragment) {
        registroLogin_viewPager.setCurrentItem(fragment);
    }

    /**
     * Muestra el fragmento de registrarme.
     */
    public void showFragmentRegistrarme() {
        showFragment(0);
    }

    /**
     * Muestra el fragmento de iniciar sesión.
     */
    public void showFragmentIniciarSesion() {
        showFragment(1);

    }

    private void cleanRegisterFields() {
        if (registerView != null) {
            registerView.cleanFields();

        }
    }
    public void cleanLoginFields() {
        if (loginView != null) {
            loginView.cleanFields();
        }

    }

    public void injectView(ILoginView loginView) {
        this.loginView = loginView;
    }

    public void injectView(IRegisterView registerView) {
        this.registerView = registerView;
    }
}
