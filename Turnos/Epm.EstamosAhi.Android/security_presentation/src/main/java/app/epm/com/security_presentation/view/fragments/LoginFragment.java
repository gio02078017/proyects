package app.epm.com.security_presentation.view.fragments;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;

import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.dependency_injection.DomainModule;
import app.epm.com.security_presentation.presenters.LoginPresenter;
import app.epm.com.security_presentation.view.activities.RegisterLoginActivity;
import app.epm.com.security_presentation.view.views_activities.ILoginView;
import app.epm.com.utilities.custom_controls.CustomAlertDialog;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.fragments.BaseFragment;

public class LoginFragment extends BaseFragment<LoginPresenter> implements ILoginView {

    private EditText login_etCorreoElectronico;
    private EditText login_etContrasenia;
    private Button login_btnOlvidasteContrasenia;
    private Button login_btnIngresarAhora;
    private LayoutInflater layoutInflater;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_login, container, false);
        this.layoutInflater = inflater;
        setPresenter(new LoginPresenter(DomainModule.getSecurityBLInstance(getBaseActivity().getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet(), getBaseActivity().getCustomSharedPreferences());
        loadViews(view);
        createProgressDialog();
        ((RegisterLoginActivity) getActivity()).injectView(this);
        return view;
    }

    /**
     * Carga los controles de la vista.
     *
     * @param view view.
     */
    private void loadViews(View view) {
        login_etCorreoElectronico = (EditText) view.findViewById(R.id.login_etCorreoElectronico);
        login_etContrasenia = (EditText) view.findViewById(R.id.login_etContrasenia);
        login_btnOlvidasteContrasenia = (Button) view.findViewById(R.id.login_btnOlvidasteContrasenia);
        login_btnIngresarAhora = (Button) view.findViewById(R.id.login_btnIngresarAhora);
        loadListenerToTheControls();
    }

    /**
     * Carga los Listener de los controles.
     */
    private void loadListenerToTheControls() {
        loadOnClickListenerToTheControlLoginBtnIngresarAhora();
        loadOnClickListenerToTheControlLoginBtnOlvidasteContrasenia();
    }

    /**
     * Carga OnClick al control login_btnOlvidasteContrasenia.
     */
    private void loadOnClickListenerToTheControlLoginBtnOlvidasteContrasenia() {
        login_btnOlvidasteContrasenia.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showAlertDialogToResetPassword();
            }
        });
    }

    /**
     * Abre alerta para resetear la contraseña.
     */
    private void showAlertDialogToResetPassword() {
        View view = layoutInflater.inflate(R.layout.template_reset_password, null);
        DialogInterface.OnClickListener onClickListenerPositiveButton = getPositiveButtonOnClickListenerResetPassword(view);
        DialogInterface.OnClickListener onClickListenerNegativeButtonCancel = getNegativeButtonCancel();
        CustomAlertDialog customAlertDialog = new CustomAlertDialog(getBaseActivity());
        customAlertDialog.showAlertDialog(R.string.text_olvidaste_contrasenia, R.string.text_resetear_contrasenia, true,
                R.string.text_aceptar, onClickListenerPositiveButton, R.string.text_cancelar,
                onClickListenerNegativeButtonCancel, view);
    }

    /**
     * Crea boton para cancelar alerta.
     *
     * @return boton cancelar.
     */
    private DialogInterface.OnClickListener getNegativeButtonCancel() {
        return new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                dialogInterface.dismiss();
            }
        };
    }

    /**
     * Crea boton aceptar para enviar correo y resetear la contraseña.
     *
     * @param view view
     * @return boton aceptar.
     */
    private DialogInterface.OnClickListener getPositiveButtonOnClickListenerResetPassword(View view) {
        final EditText templateResetPassword_etEmail = (EditText) view.findViewById(R.id.templateResetPassword_etEmail);
        templateResetPassword_etEmail.setText(login_etCorreoElectronico.getText().toString().trim());
        templateResetPassword_etEmail.setSelection(login_etCorreoElectronico.length());
        return new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                EmailUsuarioRequest resetPasswordRequest = new EmailUsuarioRequest();
                resetPasswordRequest.setCorreoElectronico(templateResetPassword_etEmail.getText().toString());
                getPresenter().validateFieldToResetPassword(resetPasswordRequest);
            }
        };
    }

    /**
     * Carga OnClick al control login_btnIngresarAhora.
     */
    private void loadOnClickListenerToTheControlLoginBtnIngresarAhora() {
        login_btnIngresarAhora.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                prepareDataToLogin();
            }
        });
    }


    /**
     * Prepara los datos para iniciar sesión.
     */
    private void prepareDataToLogin() {
        UsuarioRequest usuarioRequest = new UsuarioRequest();
        usuarioRequest.setCorreoElectronico(login_etCorreoElectronico.getText().toString().trim());
        usuarioRequest.setContrasenia(login_etContrasenia.getText().toString().trim());
        getPresenter().validateFieldsToLogin(usuarioRequest);
    }

    @Override
    public void saveToken(String token) {
        getBaseActivity().getCustomSharedPreferences().addString(Constants.TOKEN, token);
    }

    @Override
    public void startLanding(Usuario usuario) {
        getBaseActivity().setResult(Constants.LOGIN);
        getBaseActivity().finish();
        try {
            Class landingClass = Class.forName(getBaseActivity().getCustomSharedPreferences().getString(Constants.LANDING_CLASS));
            Intent intent = new Intent(getActivity(), landingClass);
            intent.putExtra(Constants.CALLED_FROM_ANOTHER_MODULE, true);
            getBaseActivity().setUsuario(usuario);
            getBaseActivity().startActivity(intent);
        } catch (ClassNotFoundException e) {
            Log.e("Exception", e.toString());
        }
    }

    @Override
    public void cleanFields() {
        login_etCorreoElectronico.setText(Constants.EMPTY_STRING);
        login_etContrasenia.setText(Constants.EMPTY_STRING);
    }


}
