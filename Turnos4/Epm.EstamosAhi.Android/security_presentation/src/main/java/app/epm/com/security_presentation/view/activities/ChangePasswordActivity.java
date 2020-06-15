package app.epm.com.security_presentation.view.activities;

import android.content.DialogInterface;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.text.method.PasswordTransformationMethod;
import android.view.Menu;
import android.view.View;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.EditText;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.menu_presentation.view.fragments.MenuFragment;

import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.dependency_injection.DomainModule;
import app.epm.com.security_presentation.presenters.ChangePasswordPresenter;

import app.epm.com.security_presentation.view.views_activities.IChangePasswordView;
import app.epm.com.utilities.controls.CustomTextViewNormal;
import app.epm.com.utilities.custom_controls.CustomAlertDialog;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by ocadavid on 5/12/2016.
 */

public class ChangePasswordActivity extends BaseActivity<ChangePasswordPresenter> implements IChangePasswordView, TextWatcher {


    private Validations validation;
    private Toolbar toolbarApp;
    private Button changePassword_btnChangePasword;
    private EditText changePassword_edtNewPassword;
    private EditText changePassword_edtCurrentPassword;
    private CheckBox changePassword_cbShowPassword;
    private MenuFragment menuFragment;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_change_password);
        loadDrawerLayout(R.id.generalDrawerLayout);
        setPresenter(new ChangePasswordPresenter(DomainModule.getSecurityBLInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_HIDDEN);
        validation = new Validations();
        loadViews();
        createProgressDialog();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        return false;
    }

    @Override
    public void beforeTextChanged(CharSequence s, int start, int count, int after) {
        //The method is not used.
    }

    @Override
    public void onTextChanged(CharSequence s, int start, int before, int count) {
        changeColorBackgroundButtonChangePassword();
    }

    @Override
    public void afterTextChanged(Editable s) {
        //The method is not used.
    }

    /**
     * Permite cargar las vistas del xml.
     */
    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        changePassword_btnChangePasword = (Button) findViewById(R.id.changePassword_btnChangePasword);
        changePassword_edtNewPassword = (EditText) findViewById(R.id.changePassword_edtNewPassword);
        changePassword_edtCurrentPassword = (EditText) findViewById(R.id.changePassword_edtCurrentPassword);
        changePassword_cbShowPassword = (CheckBox) findViewById(R.id.changePassword_cbShowPassword);
        menuFragment = (MenuFragment) getSupportFragmentManager().findFragmentById(R.id.menu_landing_small);
        loadToolbar();
        loadListenerToTheControl();
    }

    /**
     * Permite cargar el toolbar con el título y el botón de navegación.
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

    /**
     * Cargar los eventos a los controles.
     */
    private void loadListenerToTheControl() {
        changePassword_btnChangePasword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                saveChangePassword();
            }
        });

        changePassword_cbShowPassword.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton compoundButton, boolean isChecked) {
                changePassword_cbShowPassword.setButtonDrawable(isChecked ? R.mipmap.checkbox_on : R.mipmap.checkbox_off);
                changePassword_edtNewPassword.setTransformationMethod(isChecked ? null : new PasswordTransformationMethod());
                changePassword_edtNewPassword.setSelection(changePassword_edtNewPassword.length());
            }
        });

        changePassword_edtCurrentPassword.addTextChangedListener(this);
        changePassword_edtNewPassword.addTextChangedListener(this);
    }

    /**
     * Cambia diseño y habilita el botón de guardar.
     */
    private void changeColorBackgroundButtonChangePassword() {
        changePassword_btnChangePasword.setBackgroundResource(validateEditText() ? R.color.login_now : R.color.status_bar);
        changePassword_btnChangePasword.setEnabled(validateEditText());
    }

    /**
     * Valida si se ha ingresado valor en los campos de contraseña nueva y actual.
     * @return True si contiene información o false en caso contrario.
     */
    private boolean validateEditText() {
        return !(changePassword_edtNewPassword.getText().toString().isEmpty() ||
                changePassword_edtCurrentPassword.getText().toString().isEmpty());
    }


    /**
     * Ejecutar la funcionalidad de guardar el cambio de contraseña.
     */
    private void saveChangePassword() {
        Usuario usuario = getUsuario();
        UsuarioRequest usuarioRequest = new UsuarioRequest();
        usuarioRequest.setCorreoElectronico(usuario.getCorreoElectronico());
        usuarioRequest.setContrasenia(changePassword_edtCurrentPassword.getText().toString().trim());
        usuarioRequest.setContraseniaNueva(changePassword_edtNewPassword.getText().toString().trim());
        getPresenter().validateFieldsToChangePassword(usuarioRequest);
    }

    /**
     * Actualiza el nuevo token.
     * @param token token.
     */
    @Override
    public void saveToken(String token) {
        getCustomSharedPreferences().addString(Constants.TOKEN, token);
    }

    /**
     * Limpia los campos del layout.
     */
    @Override
    public void cleanFieldsTheChangePassword() {
        runOnUiThread(() -> {
            changePassword_edtNewPassword.setText(Constants.EMPTY_STRING);
            changePassword_edtCurrentPassword.setText(Constants.EMPTY_STRING);
            changePassword_cbShowPassword.setChecked(false);
        });
    }

    /**
     * Muestra el mensaje de exitoso en change password.
     * @param name
     * @param message
     */
    @Override
    public void showAlertDialogChangePasswordInformationOnUiThread(final String name, final int message) {
        runOnUiThread(() -> showAlertDialogChangePasswordInformation(name, message));
    }

    private void showAlertDialogChangePasswordInformation(String name, int message) {
        getCustomAlertDialog().showAlertDialog(name, message, false, R.string.text_aceptar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                finish();
            }
        }, null);
    }

}
