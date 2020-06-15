package app.epm.com.utilities.view.fragments;

import android.os.Bundle;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.view.activities.BaseActivity;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 18/11/16.
 */

public class BaseFragment<T extends BasePresenter> extends Fragment implements IBaseView {

    private BaseActivity baseActivity;
    private IValidateInternet validateInternet;
    private T presenter;

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

    public T getPresenter() {
        return presenter;
    }

    public void setPresenter(T presenter) {
        this.presenter = presenter;
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
