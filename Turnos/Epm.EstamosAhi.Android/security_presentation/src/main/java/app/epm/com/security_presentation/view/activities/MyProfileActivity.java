package app.epm.com.security_presentation.view.activities;

import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.content.Context;
import android.graphics.Rect;
import android.os.Bundle;
import android.os.Handler;
import androidx.appcompat.widget.Toolbar;
import android.text.Editable;
import android.view.Menu;
import android.view.View;
import android.view.WindowManager;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import app.epm.com.security_presentation.dependency_injection.DomainModule;
import app.epm.com.security_presentation.presenters.MyProfilePresenter;
import app.epm.com.security_presentation.view.views_activities.IMyProfileView;

import com.epm.app.business_models.business_models.ItemGeneral;
import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.security_presentation.R;

import java.util.Calendar;
import java.util.List;

import app.epm.com.utilities.helpers.TextChangedListener;
import app.epm.com.utilities.helpers.TextChangedListenerView;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by leidycarolinazuluagabastidas on 25/11/16.
 */
public class MyProfileActivity extends BaseActivity<MyProfilePresenter> implements IMyProfileView,
        DatePickerDialog.OnDateSetListener {

    private Toolbar toolbarApp;
    private DatePickerDialog datePickerDialog;
    private Validations validation;
    private Usuario usuario = new Usuario();
    private ItemGeneral itemGeneralSelected;
    private List<ItemGeneral> documentsTypesList;
    private List<ItemGeneral> personTypeList;
    private List<ItemGeneral> housingTypeList;
    private List<ItemGeneral> femaleMaleTypeList;
    private LinearLayout myProfile_llInfo;
    private Button myProfile_btnSaveChanges;
    private TextView myProfile_tvName;
    private TextView myProfile_tvAddress;
    private TextView myProfile_tvCountry;
    private TextView myProfile_tvEmail;
    private EditText myProfile_etName;
    private EditText myProfile_etLastName;
    private TextView myProfile_tvDocumentType;
    private EditText myProfile_etDocumentNumber;
    private TextView myProfile_tvPersonType;
    private EditText myProfile_etTel;
    private EditText myProfile_etCel;
    private EditText myProfile_etAddress;
    private TextView myProfile_tvHousingType;
    private EditText myProfile_etCountry;
    private TextView myProfile_tvDate;
    private TextView myProfile_tvFemaleMaleType;
    private EditText myProfile_etAlternateEmail;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_my_profile);
        setPresenter(new MyProfilePresenter(DomainModule.getProfileBLInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_HIDDEN);
        itemGeneralSelected = new ItemGeneral();
        validation = new Validations();
        loadViews();
        loadDateToDay();
        loadInfoProfile();
        buildLinearLayoutSizeOfTheScrollToShowOrHideSaveChangesButton();
        createProgressDialog();

    }

    /**
     * Permite cargar las vistas del xml
     */
    private void loadViews() {
        toolbarApp =  findViewById(R.id.toolbar);
        myProfile_llInfo = findViewById(R.id.myProfile_llInfo);
        myProfile_btnSaveChanges =  findViewById(R.id.myProfile_btnSaveChanges);
        myProfile_tvName =  findViewById(R.id.myProfile_tvName);
        myProfile_tvAddress =  findViewById(R.id.myProfile_tvAddress);
        myProfile_tvCountry =  findViewById(R.id.myProfile_tvCountry);
        myProfile_tvEmail =  findViewById(R.id.myProfile_tvEmail);
        myProfile_etName =  findViewById(R.id.myProfile_etName);
        myProfile_etLastName =  findViewById(R.id.myProfile_etLastName);
        myProfile_tvDocumentType =  findViewById(R.id.myProfile_tvDocumentType);
        myProfile_etDocumentNumber =  findViewById(R.id.myProfile_etDocumentNumber);
        myProfile_tvPersonType =  findViewById(R.id.myProfile_tvPersonType);
        myProfile_etTel =  findViewById(R.id.myProfile_etTel);
        myProfile_etCel =  findViewById(R.id.myProfile_etCel);
        myProfile_etAddress =  findViewById(R.id.myProfile_etAddress);
        myProfile_tvHousingType = findViewById(R.id.myProfile_tvHousingType);
        myProfile_etCountry =  findViewById(R.id.myProfile_etCountry);
        myProfile_tvDate =  findViewById(R.id.myProfile_tvDate);
        myProfile_tvFemaleMaleType =  findViewById(R.id.myProfile_tvFemaleMaleType);
        myProfile_etAlternateEmail =  findViewById(R.id.myProfile_etAlternateEmail);
        addTextChangedListener(myProfile_etName);
        addTextChangedListener(myProfile_etLastName);
        addTextChangedListenerView(myProfile_tvDocumentType);
        addTextChangedListener(myProfile_etDocumentNumber);
        addTextChangedListenerView(myProfile_tvPersonType);
        addTextChangedListener(myProfile_etTel);
        addTextChangedListener(myProfile_etCel);
        addTextChangedListener(myProfile_etAddress);
        addTextChangedListenerView(myProfile_tvHousingType);
        addTextChangedListener(myProfile_etCountry);
        addTextChangedListenerView(myProfile_tvDate);
        addTextChangedListenerView(myProfile_tvFemaleMaleType);
        addTextChangedListener(myProfile_etAlternateEmail);
        loadToolbar();
        loadListenerToTheControl();
        loadDateToDay();
        loadList();
    }

    /**
     * Permite añadir el listener a los textView
     *
     * @param textView textView
     */
    private void addTextChangedListenerView(TextView textView) {
        textView.addTextChangedListener(new TextChangedListenerView<TextView>(textView) {
            @Override
            public void onTextChangedView(TextView target) {
                if (target.equals(myProfile_tvDocumentType)) {
                    setTextFieldState(target, R.id.myProfile_ivDocumentType,
                            R.mipmap.icon_tipo_documento_prf_verde, R.mipmap.icon_tipo_documento_prf_gris);
                } else if (target.equals(myProfile_tvPersonType)) {
                    setTextFieldState(target, R.id.myProfile_ivPersonType,
                            R.mipmap.icon_tipo_persona_prf_verde, R.mipmap.icon_tipo_persona_prf_gris);
                } else if (target.equals(myProfile_tvHousingType)) {
                    setTextFieldState(target, R.id.myProfile_ivHousingType,
                            R.mipmap.icon_tip_vivienda_prf_verde, R.mipmap.icon_tip_vivienda_prf_gris);
                } else if (target.equals(myProfile_tvDate)) {
                    setTextFieldState(target, R.id.myProfile_ivDate,
                            R.mipmap.icon_cumple_prf_verde, R.mipmap.icon_cumple_prf_gris);
                } else if (target.equals(myProfile_tvFemaleMaleType)) {
                    setTextFieldState(target, R.id.myProfile_ivFemaleMaleType,
                            R.mipmap.icon_genero_prf_verde, R.mipmap.icon_genero_prf_gris);
                }
            }
        });
    }

    /**
     * Permite añadir el listener a los EditText
     *
     * @param editText editText
     */
    private void addTextChangedListener(final EditText editText) {
        editText.addTextChangedListener(new TextChangedListener<EditText>(editText) {
            @Override
            public void onTextChanged(EditText target, Editable s) {
                updateInfoPersonal();
                if (target.equals(myProfile_etName)) {
                    setTextFieldState(target, R.id.myProfile_ivName, R.mipmap.icon_nombre_prf_verde,
                            R.mipmap.icon_nombre_prf_gris);
                } else if (target.equals(myProfile_etLastName)) {
                    setTextFieldState(target, R.id.myProfile_ivLastName, R.mipmap.icon_apellido_prf_verde,
                            R.mipmap.icon_apellido_prf_gris);
                } else if (target.equals(myProfile_etDocumentNumber)) {
                    setTextFieldState(target, R.id.myProfile_ivDocumentNumber, R.mipmap.icon_num_doc_prf_verde,
                            R.mipmap.icon_num_doc_prf_gris);
                } else if (target.equals(myProfile_etTel)) {
                    setTextFieldState(target, R.id.myProfile_ivTel, R.mipmap.icon_tel_prf_verde,
                            R.mipmap.icon_tel_prf_gris);
                } else if (target.equals(myProfile_etCel)) {
                    setTextFieldState(target, R.id.myProfile_ivCel, R.mipmap.icon_cel_prf_verde,
                            R.mipmap.icon_cel_prf_gris);
                } else if (target.equals(myProfile_etAddress)) {
                    setTextFieldState(target, R.id.myProfile_ivAddress, R.mipmap.icon_direccion_prf_verde,
                            R.mipmap.icon_direccion_prf_gris);
                } else if (target.equals(myProfile_etCountry)) {
                    setTextFieldState(target, R.id.myProfile_ivCountry, R.mipmap.icon_pais_prf_verde,
                            R.mipmap.icon_pais_prf_gris);
                } else if (target.equals(myProfile_etAlternateEmail)) {
                    setTextFieldState(target, R.id.myProfile_ivAlternateEmail, R.mipmap.icon_email_alterno_prf_verde,
                            R.mipmap.icon_email_alterno_prf_gris);
                }
            }
        });
    }

    /**
     * Permite obtener las listas del customSharedPreferences
     */
    private void loadList() {
        documentsTypesList = getCustomSharedPreferences().getItemGeneralList(Constants.TIPOS_DOCUMENTO_LIST);
        personTypeList = getCustomSharedPreferences().getItemGeneralList(Constants.TIPOS_PERSONA_LIST);
        housingTypeList = getCustomSharedPreferences().getItemGeneralList(Constants.TIPOS_VIVIENDA_LIST);
        femaleMaleTypeList = getCustomSharedPreferences().getItemGeneralList(Constants.GENEROS_LIST);
    }

    /**
     * Permite controlar el tamaño de la pantalla al mostrar el teclado, de ser mas pequeña el botón guardar chambios
     * de ocultará.
     */
    private void buildLinearLayoutSizeOfTheScrollToShowOrHideSaveChangesButton() {
        myProfile_llInfo.getViewTreeObserver().addOnGlobalLayoutListener(() -> {

            final Rect rect = new Rect();
            myProfile_llInfo.getWindowVisibleDisplayFrame(rect);
            int screenHeight = myProfile_llInfo.getRootView().getHeight();
            int keypadHeight = screenHeight - rect.bottom;

            if (keypadHeight > screenHeight * 0.15) {
                myProfile_btnSaveChanges.setVisibility(View.GONE);
            } else {
                new Handler().postDelayed(() -> myProfile_btnSaveChanges.setVisibility(View.VISIBLE), 50);

            }
        });
    }

    /**
     * Permite obtener la fecha actual
     */
    private void loadDateToDay() {
        Calendar calendar = Calendar.getInstance();
        int day = calendar.get(Calendar.DAY_OF_MONTH);
        int month = calendar.get(Calendar.MONTH);
        int year = calendar.get(Calendar.YEAR);
        datePickerDialog = new DatePickerDialog(this, this, year, month, day);
    }

    /**
     * Permite cargar la lista
     *
     * @param listType tipo de lista
     * @param title    titulo de la lista
     * @param textView textview donde va a cargar la lista
     */
    private void loadTiposList(String listType, int title, final TextView textView) {
        final List<ItemGeneral> list = getCustomSharedPreferences().getItemGeneralList(listType);
        final String[] items = getItemsArrayFromItemGeneralList(list);
        AlertDialog alertDialog;
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle(title);
        builder.setItems(items, (dialog, which) -> {
            itemGeneralSelected = list.get(which);
            loadTextItemSelected(textView);
        });
        alertDialog = builder.create();
        alertDialog.show();
    }

    /**
     * Carga los Items con la descripción de la lista seleccionada.
     */
    private void loadTextItemSelected(TextView textView) {
        textView.setText(itemGeneralSelected.getDescripcion());
    }

    /**
     * Obtiene la descripción de cada uno de los documentos.
     *
     * @param tiposItemsList tipo de items de la lista
     * @return La descripción de los documentos.
     */
    private String[] getItemsArrayFromItemGeneralList(List<ItemGeneral> tiposItemsList) {
        String[] items = new String[tiposItemsList.size()];
        for (int i = 0; i < tiposItemsList.size(); i++) {
            items[i] = tiposItemsList.get(i).getDescripcion();
        }
        return items;
    }

    /**
     * Carga los Listener de los controles.
     */
    private void loadListenerToTheControl() {
        myProfile_tvDate.setOnClickListener(view -> datePickerDialog.show());

        myProfile_tvDocumentType.setOnClickListener(view -> {
            hideKeyboard();
            loadTiposList(Constants.TIPOS_DOCUMENTO_LIST, R.string.text_tipo_documento_list, myProfile_tvDocumentType);
        });

        myProfile_tvPersonType.setOnClickListener(view -> {
            hideKeyboard();
            loadTiposList(Constants.TIPOS_PERSONA_LIST, R.string.text_tipo_de_persona_list, myProfile_tvPersonType);
        });

        myProfile_tvHousingType.setOnClickListener(view -> {
            hideKeyboard();
            loadTiposList(Constants.TIPOS_VIVIENDA_LIST, R.string.text_tipo_de_vivienda_list, myProfile_tvHousingType);
        });

        myProfile_tvFemaleMaleType.setOnClickListener(view -> {
            hideKeyboard();
            loadTiposList(Constants.GENEROS_LIST, R.string.text_tipo_de_genero_list, myProfile_tvFemaleMaleType);
        });

        myProfile_btnSaveChanges.setOnClickListener(view -> {
            hideKeyboard();
            setInfoProfile();
        });
    }

    /**
     * Permite establecer el perfil
     */
    private void setInfoProfile() {
        usuario.setCorreoElectronico(myProfile_tvEmail.getText().toString().trim());
        usuario.setNombres(myProfile_etName.getText().toString().trim());
        usuario.setApellido(myProfile_etLastName.getText().toString().trim());
        usuario.setIdTipoIdentificacion(getIdFromListByString(
                myProfile_tvDocumentType.getText().toString().trim(), documentsTypesList));
        usuario.setNumeroIdentificacion(myProfile_etDocumentNumber.getText().toString().trim());
        usuario.setIdTipoPersona(getIdFromListByString(
                myProfile_tvPersonType.getText().toString().trim(), personTypeList));
        usuario.setTelefono(myProfile_etTel.getText().toString().trim());
        usuario.setCelular(myProfile_etCel.getText().toString().trim());
        usuario.setDireccion(myProfile_etAddress.getText().toString().trim());
        usuario.setIdTipoVivienda(getIdFromListByString(
                myProfile_tvHousingType.getText().toString().trim(), housingTypeList));
        usuario.setPais(myProfile_etCountry.getText().toString().trim());
        usuario.setFechaNacimiento(myProfile_tvDate.getText().toString().trim());
        usuario.setIdGenero(getIdFromListByString(
                myProfile_tvFemaleMaleType.getText().toString().trim(), femaleMaleTypeList));
        usuario.setCorreoAlternativo(myProfile_etAlternateEmail.getText().toString().trim());
        usuario.setAceptoTerminosyCondiciones(true);
        usuario.setFechaRegistro(getUsuario().getFechaRegistro());
        usuario.setToken(getUsuario().getToken());
        usuario.setActivo(getUsuario().isActivo());
        usuario.setIdUsuario(getUsuario().getIdUsuario());
        usuario.setContrasenia(getUsuario().getContrasenia());

        getPresenter().validateFieldsProfileRequired(usuario);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        return false;
    }

    /**
     * Permite cargar la iformación del usuario
     */
    private void loadInfoProfile() {
        Usuario usuario = getUsuario();

        if (validation.dataIsNotNull(usuario)) {
            myProfile_tvName.setText(usuario.getNombres() + " " + usuario.getApellido());

            validateDataIsNotNullToDireccion(usuario);
            validateDataIsNotNullToPais(usuario);

            myProfile_tvEmail.setText(usuario.getCorreoElectronico());
            myProfile_etName.setText(usuario.getNombres());
            myProfile_etLastName.setText(usuario.getApellido());
            myProfile_tvDocumentType.setText(getStringFromListById(
                    usuario.getIdTipoIdentificacion(), documentsTypesList));
            myProfile_etDocumentNumber.setText(usuario.getNumeroIdentificacion());

            validateDataIsNotNullToIdTipoPersona(usuario);
            validateDataIsNotNullToTelefono(usuario);
            validateDataIsNotNullToCelular(usuario);
            validateDataIsNotNullToIdTipoVivienda(usuario);
            validateDataIsNotNullToFechaNavimiento(usuario);
            validateDataIsNotNullToIdGenero(usuario);
            validateDataIsNotNullToCorreoAlternativo(usuario);
        }
    }

    private void validateDataIsNotNullToCorreoAlternativo(Usuario usuario) {
        if (validation.dataIsNotNull(usuario.getCorreoAlternativo())) {
            myProfile_etAlternateEmail.setText(usuario.getCorreoAlternativo());
        }
    }

    private void validateDataIsNotNullToIdGenero(Usuario usuario) {
        if (usuario.getIdGenero() != 0) {
            myProfile_tvFemaleMaleType.setText(getStringFromListById(usuario.getIdGenero(),
                    femaleMaleTypeList));
        }
    }

    private void validateDataIsNotNullToFechaNavimiento(Usuario usuario) {
        if (validation.dataIsNotNull(usuario.getFechaNacimiento())) {
            myProfile_tvDate.setText((usuario.getFechaNacimiento().split("T"))[0]);
        }
    }

    private void validateDataIsNotNullToIdTipoVivienda(Usuario usuario) {
        if (usuario.getIdTipoVivienda() != 0) {
            myProfile_tvHousingType.setText(getStringFromListById(usuario.getIdTipoVivienda(),
                    housingTypeList));
        }
    }

    private void validateDataIsNotNullToCelular(Usuario usuario) {
        if (validation.dataIsNotNull(usuario.getCelular())) {
            myProfile_etCel.setText(usuario.getCelular());
        }
    }

    private void validateDataIsNotNullToTelefono(Usuario usuario) {
        if (validation.dataIsNotNull(usuario.getTelefono())) {
            myProfile_etTel.setText(usuario.getTelefono());
        }
    }

    private void validateDataIsNotNullToIdTipoPersona(Usuario usuario) {
        if (usuario.getIdTipoPersona() != 0) {
            myProfile_tvPersonType.setText(getStringFromListById(usuario.getIdTipoPersona(),
                    personTypeList));
        }
    }

    private void validateDataIsNotNullToPais(Usuario usuario) {
        if (validation.dataIsNotNull(usuario.getPais())) {
            myProfile_tvCountry.setVisibility(View.VISIBLE);
            myProfile_tvCountry.setText(usuario.getPais());
            myProfile_etCountry.setText(usuario.getPais());
        } else {
            myProfile_tvCountry.setVisibility(View.GONE);
        }
    }

    private void validateDataIsNotNullToDireccion(Usuario usuario) {
        if (validation.dataIsNotNull(usuario.getDireccion())) {
            myProfile_tvAddress.setVisibility(View.VISIBLE);
            myProfile_tvAddress.setText(usuario.getDireccion());
            myProfile_etAddress.setText(usuario.getDireccion());
        } else {
            myProfile_tvAddress.setVisibility(View.GONE);
        }
    }

    /**
     * Permite obtener la descripcion a través del id
     *
     * @param id   id de la lista
     * @param list tipo de lista
     * @return descripcion del id
     */
    private String getStringFromListById(int id, List<ItemGeneral> list) {
        String text = "";
        for (ItemGeneral item : list) {
            if (item.getId() == id) {
                return item.getDescripcion();
            }
        }
        return text;
    }

    /**
     * Permite obtener el id a traves de la descripcion
     *
     * @param text String de la lista
     * @param list tipo de lista
     * @return id asociado a la descripcion
     */
    private int getIdFromListByString(String text, List<ItemGeneral> list) {
        int id = 0;
        for (ItemGeneral item : list) {
            if (item.getDescripcion().equals(text)) {
                return item.getId();
            }
        }
        return id;
    }

    /**
     * Permite cargar el toolbar
     */
    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar( toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(view -> onBackPressed());
    }

    /**
     * Permite ocultar el teclado.
     */
    private void hideKeyboard() {
        View view = getCurrentFocus();
        if (view != null) {
            InputMethodManager inputManager = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
            inputManager.hideSoftInputFromWindow(view.getWindowToken(), InputMethodManager.HIDE_NOT_ALWAYS);
        }
    }

    /**
     * Permite cambiar el estado del editText
     *
     * @param editText    editText a cambiar
     * @param imageView   imageView a cambiar
     * @param mipmapGreen imagen activa
     * @param mipmapGray  imagen ivactiva
     */
    private void setTextFieldState(EditText editText, int imageView, int mipmapGreen, int mipmapGray) {
        if (editText.getText().length() > 0) {
            ((ImageView) findViewById(imageView)).setImageResource(mipmapGreen);
        } else {
            ((ImageView) findViewById(imageView)).setImageResource(mipmapGray);
        }
    }

    /**
     * Permite cambiar el estado del textView
     *
     * @param textView    textView a cambiar
     * @param imageView   imageView a cambiar
     * @param mipmapGreen imagen activa
     * @param mipmapGray  imagen ivactiva
     */
    private void setTextFieldState(TextView textView, int imageView, int mipmapGreen, int mipmapGray) {
        if (textView.getText().length() > 0) {
            ((ImageView) findViewById(imageView)).setImageResource(mipmapGreen);
        } else {
            ((ImageView) findViewById(imageView)).setImageResource(mipmapGray);
        }
    }

    /**
     * Permite actualizar la informacion del encabezado
     */
    private void updateInfoPersonal() {
        myProfile_tvAddress.setVisibility(View.VISIBLE);
        myProfile_tvAddress.setText(myProfile_etAddress.getText().toString());
        myProfile_tvCountry.setVisibility(View.VISIBLE);
        myProfile_tvCountry.setText(myProfile_etCountry.getText().toString());
        myProfile_tvName.setText(myProfile_etName.getText().toString() + " " +
                myProfile_etLastName.getText().toString());
    }

    @Override
    public void onDateSet(DatePicker datePicker, int year, int month, int day) {
        String monthResult = Integer.toString(month + 1);
        monthResult = (monthResult.length() == 1) ? "0" + monthResult : monthResult;
        myProfile_tvDate.setText(year + "-" + monthResult + "-" + day);
    }

    /**
     * Permite establecer el usuario a la variable usuario
     */
    @Override
    public void loadUserInfoInActivity() {
        setUsuario(usuario);
    }
}