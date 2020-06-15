package com.epm.app.mvvm.comunidad.views.activities;


import androidx.lifecycle.ViewModelProviders;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.os.Bundle;
import android.os.Parcelable;
import android.view.WindowManager;
import com.epm.app.R;
import com.epm.app.databinding.ActivitySplashBinding;
import com.epm.app.mvvm.comunidad.viewModel.SplashViewModel;
import com.epm.app.view.activities.LandingActivity;
import java.util.ArrayList;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class SplashActivity extends BaseActivityWithDI {


    SplashViewModel splashViewModel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        ActivitySplashBinding binding = DataBindingUtil.setContentView(this, R.layout.activity_splash);
        this.configureDagger();
        splashViewModel = ViewModelProviders.of(this,viewModelFactory).get(SplashViewModel.class);
        binding.setViewModel((SplashViewModel) splashViewModel);
        this.getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        getCustomSharedPreferences().addString(Constants.EXIST_NOTIFICATION, Constants.EMPTY_STRING);
        splashViewModel.loadUser(getUsuario());
        callSplashTime();
    }


    private void startTutorialActivity(){
        Intent intent=new Intent(SplashActivity.this,TutorialActivity.class);
        intent.putParcelableArrayListExtra(Constants.MUNICIPALITIES, (ArrayList<? extends Parcelable>) splashViewModel.getListMunicipio());
        intent.putExtra(Constants.SUSCRIPTION_ALERTAS,splashViewModel.getSuccess().getValue());
        startActivityWithOutDoubleClick(intent);
    }

    private void startAlertasActivity(){
       Intent intent=new Intent(SplashActivity.this,AlertasActivity.class);
       intent.putParcelableArrayListExtra(Constants.MUNICIPALITIES, (ArrayList<? extends Parcelable>) splashViewModel.getListMunicipio());
       startActivityWithOutDoubleClick(intent);
    }

    private void startDashboardAlertas(){
        Intent intent = new Intent(SplashActivity.this,DashboardComunityAlertActivity.class);
        startActivityWithOutDoubleClick(intent);
    }


    private void callSplashTime(){
        splashViewModel.captureError();
        splashViewModel.getSuccess().observe(this, s -> {
            if(splashViewModel.validateViewTutorial(getApplicationContext())){
                gotoAlertasActivity(s);
            }else {
                startTutorialActivity();
            }
        });
        splashViewModel.getExpiredToken().observe(this, errorMessage -> showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage()) );
        splashViewModel.getError().observe(this, errorMessage -> showAlertDialogTryAgain(errorMessage.getTitle(),errorMessage.getMessage(),R.string.text_intentar, R.string.text_cancelar));
        splashViewModel.loadViewWithSplash();
    }

    private void gotoAlertasActivity(String validate){
        if(Constants.TRUE.equals(validate)){
            startDashboardAlertas();
        }else {
            startAlertasActivity();
        }

    }

    private void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> {
            if (!getUsuario().isInvitado() && title == R.string.title_appreciated_user) {
                getCustomAlertDialog().showAlertDialog(getUsuario().getNombres(), message, false, positive, (dialogInterface, i) -> {
                    splashViewModel.loadViewWithSplash();
                }, negative, (dialogInterface, i) -> startActivityLanding(), null);
            } else {
                getCustomAlertDialog().showAlertDialog(title, message, false, positive, (dialogInterface, i) -> {
                    splashViewModel.loadViewWithSplash();
                }, negative, (dialogInterface, i) -> startActivityLanding(), null);
            }
        });
    }

    private void startActivityLanding(){
        Intent intent= new Intent(SplashActivity.this,LandingActivity.class);
        startActivityWithOutDoubleClick(intent);
    }

    @Override
    public void onBackPressed() {
    }


}
