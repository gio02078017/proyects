package app.epm.com.utilities.view.fragments;

import android.os.Bundle;
import android.support.annotation.Nullable;

import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.view.activities.BaseActivity;
import app.epm.com.utilities.view.views_acitvities.IBaseView;
import dagger.android.DaggerFragment;

public class BaseFragmentDi extends DaggerFragment implements IBaseView {
    private BaseActivity baseActivity;
    private IValidateInternet validateInternet;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        baseActivity = (BaseActivity) getActivity();
        validateInternet = baseActivity.getValidateInternet();

    }

    @Override
    public void showAlertDialogGeneralInformationOnUiThread(int title, int message) {
        baseActivity.showAlertDialogGeneralInformationOnUiThread(title, message);
    }

    @Override
    public void showAlertDialogGeneralInformationOnUiThread(int title, String message) {
        baseActivity.showAlertDialogGeneralInformationOnUiThread(title, message);
    }

    @Override
    public void showAlertDialogGeneralInformationOnUiThread(String title, String message) {
        baseActivity.showAlertDialogGeneralInformationOnUiThread(title, message);
    }

    @Override
    public void showAlertDialogGeneralInformationOnUiThread(String title, int message) {
        baseActivity.showAlertDialogGeneralInformationOnUiThread(title, message);
    }

    @Override
    public void showProgressDIalog(int text) {
        baseActivity.showProgressDIalog(text);
    }

    @Override
    public void dismissProgressDialog() {
        baseActivity.dismissProgressDialog();
    }

    @Override
    public void showAlertDialogUnauthorized(int title, int message) {
        baseActivity.showAlertDialogUnauthorized(title, message);
    }

    @Override
    public String getName() {
        return baseActivity.getName();
    }

    @Override
    public String getEmail() {
        return baseActivity.getEmail();
    }

    @Override
    public void finishActivity() {
        baseActivity.finishActivity();
    }

    @Override
    public void validateShowAlertQualifyApp() {
        baseActivity.validateShowAlertQualifyApp();
    }

    public void createProgressDialog() {
        baseActivity.createProgressDialog();
    }

    public IValidateInternet getValidateInternet() {
        return validateInternet;
    }

    public void setValidateInternet(IValidateInternet validateInternet) {
        this.validateInternet = validateInternet;
    }

    public BaseActivity getBaseActivity() {
        return baseActivity;
    }

    public Usuario userIsAuthenticated() {
        return baseActivity.getUsuario();
    }

    public void setBaseActivity(BaseActivity baseActivity) {
        this.baseActivity = baseActivity;
    }

    public void setUsuario(Usuario usuario) {
        baseActivity.setUsuario(usuario);
    }
}
