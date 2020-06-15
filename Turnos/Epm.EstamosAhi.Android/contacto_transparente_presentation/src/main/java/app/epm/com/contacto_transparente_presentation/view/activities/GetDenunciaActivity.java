package app.epm.com.contacto_transparente_presentation.view.activities;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import androidx.appcompat.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.View;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;

import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.contacto_transparente_presentation.dependency_injection.DomainModule;
import app.epm.com.contacto_transparente_presentation.presenters.GetDenunciaPresenter;
import app.epm.com.contacto_transparente_presentation.view.views_activities.IGetDenunciaView;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

public class GetDenunciaActivity extends BaseActivity<GetDenunciaPresenter> implements IGetDenunciaView {

    private Toolbar toolbarApp;
    private Button consultaIncidente_ButtonContinue;
    private EditText consultaIncidente_EditTextNumeroIncidente;
    private String codigoIncidente = Constants.EMPTY_STRING;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_get_denuncia);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        setPresenter(new GetDenunciaPresenter(DomainModule.getContactoTransparenteBusinessLogicInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        createProgressDialog();
        hideKeyboard();
        loadViews();
    }

    private void hideKeyboard() {
        getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_HIDDEN);
    }

    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        consultaIncidente_ButtonContinue = (Button) findViewById(R.id.consultaIncidente_ButtonContinue);
        consultaIncidente_EditTextNumeroIncidente = (EditText) findViewById(R.id.consultaIncidente_EditTextNumeroIncidente);
        consultaIncidente_ButtonContinue.setEnabled(false);
        loadToolbar();
        loadListener();
    }

    private void loadListener() {
        loadListenerToTheControlButtonContinue();
        loadListenerToTheControlEditTextNumeroIncidente();
    }

    private void loadListenerToTheControlEditTextNumeroIncidente() {
        consultaIncidente_EditTextNumeroIncidente.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {
                //The method is not used.
            }

            @Override
            public void onTextChanged(CharSequence newText, int start, int before, int count) {
                validateReportNumberToDisabledButton(newText);
            }

            @Override
            public void afterTextChanged(Editable s) {
                //The method is not used.
            }
        });
    }

    private void validateReportNumberToDisabledButton(CharSequence s) {
        int colorButton;
        boolean enabledButton;
        if (s.toString().isEmpty()) {
            enabledButton = false;
            colorButton = R.color.gray_button_continue;
        } else {
            enabledButton = true;
            colorButton = R.color.button_green_factura;
        }
        consultaIncidente_ButtonContinue.setBackgroundResource(colorButton);
        consultaIncidente_ButtonContinue.setEnabled(enabledButton);
    }

    private void loadListenerToTheControlButtonContinue() {
        consultaIncidente_ButtonContinue.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                codigoIncidente = consultaIncidente_EditTextNumeroIncidente.getText().toString();
                getIncidente(codigoIncidente);
            }
        });
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        this.toolbarApp.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onBackPressed();
            }
        });
    }

    @Override
    public void getIncidente(String codigoIncidente) {
        getPresenter().validateInternetToGetIncidente(codigoIncidente);
    }

    @Override
    public void callActivityDescribesAndSentDataForIntent(Incidente incidente) {
        Intent intent = new Intent(GetDenunciaActivity.this, ConsultRegisterIncidentActivity.class);
        intent.putExtra(Constants.CODE_INCIDENT, codigoIncidente);
        intent.putExtra(Constants.INCIDENT, incidente);
        startActivityWithOutDoubleClick(intent);
    }

    @Override
    public void showAlertDialogOnUiThread(final String title, final String message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false,
                R.string.btn_acept, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        consultaIncidente_EditTextNumeroIncidente.setText("");
                        dialog.dismiss();
                    }
                }, null));
    }


}