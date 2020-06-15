package app.epm.com.security_presentation.view.fragments;

import android.content.Intent;
import android.os.Bundle;
import androidx.appcompat.app.AlertDialog;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.TextView;

import com.epm.app.business_models.business_models.ItemGeneral;
import com.epm.app.business_models.business_models.Usuario;

import java.util.List;

import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.dependency_injection.DomainModule;
import app.epm.com.security_presentation.presenters.RegisterPresenter;
import app.epm.com.security_presentation.view.activities.RegisterLoginActivity;
import app.epm.com.security_presentation.view.activities.TermsAndConditionsActivity;
import app.epm.com.security_presentation.view.views_activities.IRegisterView;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.fragments.BaseFragment;

/**
 * Created by mateoquicenososa on 21/11/16.
 */
public class RegisterFragment extends BaseFragment<RegisterPresenter> implements IRegisterView {

    private EditText register_etCorreoElectronico;
    private EditText register_etNombres;
    private EditText register_etApellidos;
    private TextView register_tvTipoDocumento;
    private EditText register_etNumeroDocumento;
    private EditText register_etContrasenia;
    private CheckBox register_cbTerminosYCondiciones;
    private Button register_btnRegistrarme;
    private TextView register_tvTerminosYCondiciones;

    private ItemGeneral itemGeneralSelected;
    private boolean isCheckTerminosYCondiciones;
    private ICustomSharedPreferences customSharedPreferences;
    private AlertDialog alertDialog;


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_register, container, false);
        this.customSharedPreferences = new CustomSharedPreferences(getBaseActivity());
        setPresenter(new RegisterPresenter(DomainModule.getSecurityBLInstance(this.customSharedPreferences)));
        getPresenter().inject(this, getValidateInternet());
        loadViews(view);
        createProgressDialog();
        loadTiposDocumentoList();
        itemGeneralSelected = new ItemGeneral();
        ((RegisterLoginActivity) getActivity()).injectView(this);
        return view;
    }

    /**
     * Permite cargar la lista tipos de documentos.
     */
    private void loadTiposDocumentoList() {
        final List<ItemGeneral> tiposDocumentoList = customSharedPreferences.getItemGeneralList(Constants.TIPOS_DOCUMENTO_LIST);
        String[] documentos = getDocumentosArrayFromItemGeneralList(tiposDocumentoList);
        AlertDialog.Builder builder = new AlertDialog.Builder(getBaseActivity());
        builder.setTitle(R.string.text_tipo_documento);
        builder.setItems(documentos, (dialog, which) -> {
            itemGeneralSelected = tiposDocumentoList.get(which);
            loadTextItemSelected();
        });
        alertDialog = builder.create();
    }

    /**
     * Obtiene la descripción de cada uno de los documentos.
     * @param tiposDocumentoList tiposDocumentoList
     * @return La descripción de los documentos.
     */
    private String[] getDocumentosArrayFromItemGeneralList(List<ItemGeneral> tiposDocumentoList) {
        String[] documentos = new String[tiposDocumentoList.size()];
        for (int i = 0; i < tiposDocumentoList.size(); i++) {
            documentos[i] = tiposDocumentoList.get(i).getDescripcion();
        }
        return documentos;
    }

    /**
     * Carga los controles de la vista.
     * @param view view
     */
    private void loadViews(View view) {
        register_etCorreoElectronico =  view.findViewById(R.id.register_etCorreoElectronico);
        register_etNombres =  view.findViewById(R.id.register_etNombres);
        register_etApellidos =  view.findViewById(R.id.register_etApellidos);
        register_tvTipoDocumento =  view.findViewById(R.id.register_tvTipoDcumento);
        register_etNumeroDocumento =  view.findViewById(R.id.register_etNumeroDocumento);
        register_etContrasenia =  view.findViewById(R.id.register_etContrasenia);
        register_cbTerminosYCondiciones =  view.findViewById(R.id.register_cbTerminosYCondiciones);
        register_btnRegistrarme =  view.findViewById(R.id.register_btnRegistrarme);
        register_tvTerminosYCondiciones = view.findViewById(R.id.register_tvTerminosYCondiciones);
        loadListenerToTheControl();
    }

    /**
     * Carga los Listener de los controles.
     */
    private void loadListenerToTheControl() {
        loadOnClickListenerToTheControlRegisterBtnRegistrarme();
        loadOnClickListenerToTheControlRegisterTvTerminosYCondiciones();
        loadOnClickListenerToTheControlRegisterTvTipoDeDocumentos();
        loadOnCheckBoxChanged();
    }

    /**
     * Carga OnClick al control register_tvTipoDocumento.
     */
    private void loadOnClickListenerToTheControlRegisterTvTipoDeDocumentos() {
        register_tvTipoDocumento.setOnClickListener(view -> alertDialog.show());
    }

    /**
     * Carga los Items con la descripción de la lista tipos documentos.
     */
    private void loadTextItemSelected() {
        register_tvTipoDocumento.setText(itemGeneralSelected.getDescripcion());
    }

    /**
     * Carga OnCheck al control register_cbTerminosYCondiciones.
     */
    private void loadOnCheckBoxChanged() {
        register_cbTerminosYCondiciones.setOnCheckedChangeListener((compoundButton, isChecked) -> {
            register_cbTerminosYCondiciones.setButtonDrawable(isChecked ? R.mipmap.checkbox_on : R.mipmap.checkbox_off);
            isCheckTerminosYCondiciones = isChecked;
        });
    }

    /**
     * Carga OnClick al control register_btnRegistrarme.
     */
    private void loadOnClickListenerToTheControlRegisterBtnRegistrarme() {
        register_btnRegistrarme.setOnClickListener(view -> prepareDateToRegister());
    }

    /**
     * Prepara los datos para registrarme.
     */
    private void prepareDateToRegister() {
        Usuario usuario = new Usuario();
        usuario.setCorreoElectronico(register_etCorreoElectronico.getText().toString().trim());
        usuario.setNombres(register_etNombres.getText().toString().trim());
        usuario.setApellido(register_etApellidos.getText().toString().trim());
        usuario.setIdTipoIdentificacion(itemGeneralSelected.getId());
        usuario.setNumeroIdentificacion(register_etNumeroDocumento.getText().toString().trim());
        usuario.setContrasenia(register_etContrasenia.getText().toString().trim());
        usuario.setAceptoTerminosyCondiciones(isCheckTerminosYCondiciones);
        getPresenter().validateFieldsToRegister(usuario);
    }

    /**
     * Carga OnClick al control register_tvTerminosYCondiciones.
     */
    private void loadOnClickListenerToTheControlRegisterTvTerminosYCondiciones() {
        register_tvTerminosYCondiciones.setOnClickListener(view -> getPresenter().validateInternetToTermsAndConditionsRegister());
    }

    /**
     * Limpia los campos del registro.
     */


    @Override
    public void startTermsAndConditions() {
        Intent intent = new Intent(getActivity(), TermsAndConditionsActivity.class);
        getActivity().startActivity(intent);
    }

    @Override
    public void startFragmentIniciarSesion() {
        getBaseActivity().runOnUiThread(() -> {
            ((RegisterLoginActivity) getActivity()).showFragmentIniciarSesion();
            ((RegisterLoginActivity) getActivity()).cleanLoginFields();
            cleanFields();
        });
    }

    @Override
    public void cleanFields() {
        register_etCorreoElectronico.setText(Constants.EMPTY_STRING);
        register_etNombres.setText(Constants.EMPTY_STRING);
        register_etApellidos.setText(Constants.EMPTY_STRING);
        register_tvTipoDocumento.setText(Constants.EMPTY_STRING);
        register_etNumeroDocumento.setText(Constants.EMPTY_STRING);
        register_etContrasenia.setText(Constants.EMPTY_STRING);
        register_cbTerminosYCondiciones.setChecked(false);
    }
}
