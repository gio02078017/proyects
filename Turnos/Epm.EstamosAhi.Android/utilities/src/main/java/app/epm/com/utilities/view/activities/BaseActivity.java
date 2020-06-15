package app.epm.com.utilities.view.activities;

import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import androidx.annotation.Nullable;
import androidx.core.view.GravityCompat;
import androidx.core.view.MenuItemCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.customview.widget.ViewDragHelper;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.FragmentActivity;

import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageButton;
import android.widget.TextView;

import com.epm.app.business_models.business_models.Usuario;
import com.google.gson.Gson;

import java.lang.reflect.Field;

import app.epm.com.utilities.R;
import app.epm.com.utilities.custom_controls.CustomAlertDialog;
import app.epm.com.utilities.helpers.CustomApplicationContext;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomNotificationHelper;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.InformationOffice;
import app.epm.com.utilities.helpers.RateApp;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.ConvertUtilities;
import app.epm.com.utilities.utils.INotificationObserver;
import app.epm.com.utilities.utils.IStateTurnObserver;
import app.epm.com.utilities.utils.NotificationManager;
import app.epm.com.utilities.utils.StateTurnManager;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class BaseActivity<T extends BasePresenter> extends AppCompatActivity implements IBaseView, ICustomNotificationHelper, INotificationObserver, IStateTurnObserver {

    private T presenter;
    private IValidateInternet validateInternet;
    private ProgressDialog progressDialog;
    private CustomAlertDialog customAlertDialog;
    private Usuario usuario;
    private String login;
    private boolean invitado;
    protected DrawerLayout generalDrawerLayout;
    protected CustomSharedPreferences customSharedPreferences;
    private ImageButton btnMenuHamburguer;
    private IdDispositive idDispositive;
    private ConvertUtilities convertUtilities;
    protected TextView numberNotifications;
    private boolean doubleClick;
    private static String packageName;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.validateInternet = new ValidateInternet(this);
        this.customAlertDialog = new CustomAlertDialog(this);
        this.customSharedPreferences = new CustomSharedPreferences(this);
        this.idDispositive = new IdDispositive(this);
        this.convertUtilities = new ConvertUtilities(this);
        loadData();
        NotificationManager.getInstance().getNotificationSubject().attach(this);
        StateTurnManager.getInstance().getStateTurnSubject().attach(this);

    }

    @Override
    protected void onResume() {
        super.onResume();
        loadCustomNotification(this);
        invalidateOptionsMenu();
        doubleClick = false;
    }

    public void loadCustomNotification(ICustomNotificationHelper customNotificationHelper) {
        CustomApplicationContext.getInstance().setCustomNotificationHelper(customNotificationHelper);
    }

    private void loadData() {
        Intent intent = getIntent();
        this.usuario = (Usuario) intent.getSerializableExtra(Constants.USUARIO);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == Constants.DEFAUL_REQUEST_CODE) {
            if (data != null) {
                usuario = (Usuario) data.getSerializableExtra(Constants.USUARIO);
            }
            switch (resultCode) {
                case Constants.ACTION_START:
                    setResultData(Constants.ACTION_START);
                    finish();
                    break;
                case Constants.LOGIN:
                    setResultData(Constants.LOGIN);
                    finish();
                    break;
                case Constants.SESSION:
                    setResultData(Constants.SESSION);
                    finish();
                    break;
                case Constants.UNAUTHORIZED:
                    setResultData(Constants.UNAUTHORIZED);
                    finish();
                    break;
                case Constants.ACTION_TO_EXECUTE_FROM_ONE_SIGNAL:
                    setResult(Constants.ACTION_TO_EXECUTE_FROM_ONE_SIGNAL);
                    finish();
                    break;
                case Constants.ACTION_TO_EXECUTE_NOTICIAS:
                    setResult(Constants.ACTION_TO_EXECUTE_NOTICIAS);
                    finish();
                    break;
                case Constants.ACTION_TO_EXECUTE_LINEAS:
                    setResult(Constants.ACTION_TO_EXECUTE_LINEAS);
                    finish();
                    break;
                case Constants.ACTION_TO_EXECUTE_SERVICIO_AL_CLIENTE:
                    setResult(Constants.ACTION_TO_EXECUTE_SERVICIO_AL_CLIENTE);
                    finish();
                    break;
                case Constants.ACTION_TO_EXECUTE_REPORTE_FRAUDES:
                    setResult(Constants.ACTION_TO_EXECUTE_REPORTE_FRAUDES);
                    finish();
                    break;
                case Constants.ACTION_TO_EXECUTE_CONTACTO_TRANSPARENTE:
                    setResult(Constants.ACTION_TO_EXECUTE_CONTACTO_TRANSPARENTE);
                    finish();
                    break;
                case Constants.ACTION_TO_EXECUTE_DANIOS:
                    setResult(Constants.ACTION_TO_EXECUTE_DANIOS);
                    finish();
                    break;
                case Constants.ACTION_TO_EVENTOS:
                    setResult(Constants.ACTION_TO_EVENTOS);
                    finish();
                    break;
                case Constants.ACTION_TO_ESTACIONES_DE_GAS:
                    setResult(Constants.ACTION_TO_ESTACIONES_DE_GAS);
                    finish();
                    break;
                case Constants.ACTION_TO_ALERTAS_HIDROITUANGO:
                    setResult(Constants.ACTION_TO_ALERTAS_HIDROITUANGO);
                    finish();
                    break;
                case Constants.ACTION_TO_RED_ALERTAS_HIDROITUANGO:
                    setResult(Constants.ACTION_TO_RED_ALERTAS_HIDROITUANGO);
                    finish();
                    break;
                case Constants.ACTION_TO_ATTENDED_SHIFT:
                    setResult(Constants.ACTION_TO_ATTENDED_SHIFT);
                    finish();
                    break;
                case Constants.ACTION_TO_TURN_ADVANCE:
                    setResult(Constants.ACTION_TO_TURN_ADVANCE);
                    finish();
                    break;
                default:
                    break;
            }
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_general, menu);
        final MenuItem itemNotification = menu.findItem(R.id.action_notifications);
        validateIfHidenOrShowBellNotification(itemNotification);
        View actionView = MenuItemCompat.getActionView(itemNotification);
        numberNotifications = actionView.findViewById(R.id.notification_badge);
        if (customSharedPreferences.getInt(Constants.NUMBER_NOTIFICATIONS) != null) {
            setNumberNotifications(customSharedPreferences.getInt(Constants.NUMBER_NOTIFICATIONS));
        }
        actionView.setOnClickListener(v -> onOptionsItemSelected(itemNotification));
        return true;
    }

    private void validateIfHidenOrShowBellNotification(MenuItem itemNotification) {
        itemNotification.setVisible(customSharedPreferences.getBoolean(Constants.SHOW_BELL));
    }


    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        if (id == R.id.menuGeneral_menuHamburguer && generalDrawerLayout != null) {
            generalDrawerLayout.openDrawer(GravityCompat.END);
        }
        if (id == R.id.action_notifications) {
            notificationsCenter();
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onStop() {
        if (generalDrawerLayout != null && generalDrawerLayout.isDrawerOpen(GravityCompat.END)) {
            hideMenu();
        }
        super.onStop();
    }

    @Override
    public void showAlertDialogGeneralInformationOnUiThread(final int title, final int message) {
        runOnUiThread(() -> showAlertDialogGeneralInformation(title, message));
    }

    @Override
    public void showAlertDialogGeneralInformationOnUiThread(final int title, final String message) {
        runOnUiThread(() -> showAlertDialogGeneralInformation(title, message));
    }

    @Override
    public void showAlertDialogGeneralInformationOnUiThread(final String title, final String message) {
        runOnUiThread(() -> showAlertDialogGeneralInformation(title, message));
    }

    @Override
    public void showAlertDialogGeneralInformationOnUiThread(final String title, final int message) {
        runOnUiThread(() -> showAlertDialogGeneralInformation(title, message));
    }

    @Override
    public void showAlertDialogUnauthorized(final int title, final int message) {
        runOnUiThread(() -> showAlertDialogGeneralUnauthoried(title, message));
    }

    @Override
    public void showProgressDIalog(final int text) {
        runOnUiThread(() -> {
            try {
                progressDialog.setMessage(getResources().getString(text));
                progressDialog.show();
            } catch (Exception e) {
                Log.e("Exception", e.toString());
            }
        });
    }

    @Override
    public void dismissProgressDialog() {
        this.progressDialog.dismiss();
    }

    public boolean getStateDialog() {
        return this.progressDialog.isShowing();
    }

    @Override
    public void startActivityForResult(Intent intent, int requestCode) {
        intent = getDefaultIntent(intent);
        super.startActivityForResult(intent, requestCode);
    }

    public Intent getDefaultIntent(Intent intent) {
        intent.putExtra(Constants.USUARIO, usuario);
        return intent;
    }

    @Override
    public void onBackPressed() {
        if (generalDrawerLayout != null && generalDrawerLayout.isDrawerOpen(GravityCompat.END)) {
            generalDrawerLayout.closeDrawer(GravityCompat.END);
        } else {
            customOnBackPressed();
        }
    }

    protected void customOnBackPressed() {
        setResultData(RESULT_OK);
        super.onBackPressed();
    }

    @Override
    public void finishActivity() {
        this.finish();
    }

    public CustomAlertDialog getCustomAlertDialog() {
        return customAlertDialog;
    }

    public void setResultData(int result) {
        Intent intent = new Intent();
        intent.putExtra(Constants.USUARIO, usuario);
        setResult(result, intent);
    }

    /**
     * Envía reporte a Google Analytics.
     *
     * @param report Texto a enviar.
     */
    public void sendReportToGoogleAnalytics(String report) {
        CustomApplicationContext.getInstance().sendReport(report);
    }

    public void setResultData(int result, Intent intent) {
        intent.putExtra(Constants.USUARIO, usuario);
        setResult(result, intent);
    }

    public void createProgressDialog() {
        this.progressDialog = new ProgressDialog(this);
        this.progressDialog.setCancelable(false);
    }

    private void showAlertDialogGeneralInformation(int title, int message) {
        customAlertDialog.showAlertDialog(title, message, false, R.string.text_aceptar, getDefaulPositiveButtonOnClickListener(), null);
    }

    private void showAlertDialogGeneralInformation(int title, String message) {
        customAlertDialog.showAlertDialog(title, message, false, R.string.text_aceptar, getDefaulPositiveButtonOnClickListener(), null);
    }

    private void showAlertDialogGeneralInformation(String title, String message) {
        customAlertDialog.showAlertDialog(title, message, false, R.string.text_aceptar, getDefaulPositiveButtonOnClickListener(), null);
    }

    private void showAlertDialogGeneralInformation(String title, int message) {
        customAlertDialog.showAlertDialog(title, message, false, R.string.text_aceptar, getDefaulPositiveButtonOnClickListener(), null);
    }

    private void showAlertDialogGeneralUnauthoried(int title, int message) {
        customAlertDialog.showAlertDialog(title, message, false, R.string.text_aceptar, getPositiveButtonOnClickListener(), null);
    }

    private DialogInterface.OnClickListener getDefaulPositiveButtonOnClickListener() {
        return (dialog, which) -> {
            dialog.dismiss();
            generalPositiveAction();
        };
    }

    private DialogInterface.OnClickListener getPositiveButtonOnClickListener() {
        return (dialog, which) -> {
            setResultData(Constants.UNAUTHORIZED);
            finish();
        };
    }

    public T getPresenter() {
        return presenter;
    }

    public void setPresenter(T presenter) {
        this.presenter = presenter;
    }

    public IValidateInternet getValidateInternet() {
        if (validateInternet == null) {
            validateInternet = new ValidateInternet(this);
        }
        return validateInternet;
    }

    public void setValidateInternet(IValidateInternet validateInternet) {
        this.validateInternet = validateInternet;
    }

    public boolean userIsAuthenticated() {
        return usuario != null;
    }

    public void loadDrawerLayout(int idGeneralDrawerLayout) {
        this.generalDrawerLayout = findViewById(idGeneralDrawerLayout);

    }

    public void loadSwipeDrawerLayout() {
        try {
            Field mDragger = generalDrawerLayout.getClass().getDeclaredField("mRightDragger");
            mDragger.setAccessible(true);
            ViewDragHelper draggerObj = (ViewDragHelper) mDragger.get(generalDrawerLayout);
            Field mEdgeSize = draggerObj.getClass().getDeclaredField("mEdgeSize");
            mEdgeSize.setAccessible(true);
            int edge = mEdgeSize.getInt(draggerObj);
            mEdgeSize.setInt(draggerObj, edge * 15);
        } catch (NoSuchFieldException e) {
            Log.i("Menu", "Error al desplegarse  el menu hamburguesa" + e.getMessage());
        } catch (IllegalAccessException e) {
            Log.i("MenuHm", "Error al desplegarse  el menu hamburguesa" + e.getMessage());
        } catch (NullPointerException e) {
            Log.i("MenuHamburguer", "Error al desplegarse  el menu hamburguesa" + e.getMessage());
        }
    }

    public Usuario getUsuario() {
        return usuario;
    }

    public void setUsuario(Usuario usuario) {
        this.usuario = usuario;
    }

    public void hideMenu() {
        if (generalDrawerLayout != null) {
            generalDrawerLayout.closeDrawers();
        }
    }

    /**
     * Override para ejecutar en el landingActivity
     */
    public void loginGuest() {
        //The method is not used.
    }

    public String getName() {
        String nameDefault = getResources().getString(R.string.title_appreciated_user);
        if (getUsuario() != null) {
            if (getUsuario().getNombres() != null && !getUsuario().getNombres().isEmpty()) {
                return nameDefault = getUsuario().getNombres();
            } else {
                return nameDefault;
            }
        }
        return nameDefault;
    }

    public String getEmail() {
        String email = Constants.EMPTY_STRING;
        if (getUsuario() != null) {
            if (getUsuario().getCorreoElectronico().matches(Constants.REGULAR_EXPRESSION_CORRECT_EMAIL)) {
                return email = getUsuario().getCorreoElectronico();
            } else {
                return email;
            }
        }
        return email;
    }

    @Override
    public void notifyFromOneSignal() {
        String actionToExecute = customSharedPreferences.getString(Constants.ACTION_TO_EXECUTE);
        //customSharedPreferences.addBoolean(Constants.SHOW_BELL,false);
        if (actionToExecute != null) {
            switch (actionToExecute) {
                case Constants.TYPE_FACTURA_NOTIFICATION:
                    manageActionFactura();
                    break;
                case Constants.MODULE_NOTICIAS:
                    manageActionNoticias();
                    break;
                case Constants.MODULE_LINEAS_DE_ATENCION:
                    manageActionLineas();
                    break;
                case Constants.MODULE_SERVICIO_AL_CLIENTE:
                    manageActionServicioAlCliente();
                    break;
                case Constants.MODULE_REPORTE_FRAUDES:
                    manageActionReporteFraudes();
                    break;
                case Constants.MODULE_FACTURA:
                    manageActionFactura();
                    break;
                case Constants.MODULE_CONTACTO_TRANSPARENTE:
                    manageActionContactoTransparente();
                    break;
                case Constants.MODULE_REPORTE_DANIOS:
                    manageActionReporteDanios();
                    break;
                case Constants.MODULE_EVENTOS:
                    manageActionEventos();
                    break;
                case Constants.MODULE_ESTACIONES_DE_GAS:
                    manageActionEstacionesDeGas();
                    break;
                case Constants.MODULE_DE_ALERTASHIDROITUANGO:
                    manageActionAlertasHidroitunago();
                    break;
                case Constants.MODULE_TURNO_ATENDIDO:
                case Constants.MODULE_TURNO_ABANDONADO:
                    manageActionAttendedShift();
                    break;
                case Constants.MODULE_TURNO_AVANCE:
                    manageActionTurnAdvance();
                    break;
                default:
                    break;
            }
        }
        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, null);
    }

    public void setNumberNotifications(int numberNotifications) {
        if (numberNotifications > 0) {
            this.numberNotifications.setVisibility(View.VISIBLE);
            this.numberNotifications.setText(String.valueOf(numberNotifications));
        } else {
            this.numberNotifications.setVisibility(View.GONE);
        }

    }

    public void manageActionEstacionesDeGas() {
        goToTheActivity(Constants.GAS_STATIONS);
    }

    public void manageActionAlertasHidroitunago() {
        goToTheActivity(Constants.HIDROITUANGO_ALERS);
    }

    public void manageActionAttendedShift() {
        goToTheActivity(Constants.ATTENDED_SHIFT);
    }

    public void manageActionTurnAdvance() {
        try {
            InformationOffice informationOffice = new Gson().fromJson(getCustomSharedPreferences().getString(Constants.INFORMATION_OFFICE_JSON), InformationOffice.class);
            if (informationOffice != null) {
                Class clazz = Class.forName(customSharedPreferences.getString(Constants.SHIFT_INFORMATION_ACTIVITY));
                Intent intent = new Intent(this, clazz);
                intent.putExtra(Constants.INFORMATION_OFFICE, informationOffice);
                intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivityWithOutDoubleClick(intent);
            } else {
                goToTheActivity(Constants.ATENTION_CHANNEL, true);
            }
        } catch (ClassNotFoundException e) {
            Log.e(Constants.EXCEPTION_STRING, e.toString());
        }

    }

    public void manageActionEventos() {
        goToTheActivity(Constants.EVENTS_ACTIVITY);
    }

    public void manageActionReporteDanios() {
        goToTheActivity(Constants.DAMAGE_REPORT);
    }

    public void manageActionContactoTransparente() {
        goToTheActivity(Constants.TRANSPARENT_CONTACT);
    }

    public void manageActionReporteFraudes() {
        goToTheActivity(Constants.FRAUD_REPORT);
    }

    public void manageActionServicioAlCliente() {
        goToTheActivity(Constants.CUSTOMER_SERVICE);
    }

    public void manageActionLineas() {
        goToTheActivity(Constants.ATENTION_LINES_ACTIVITY);
    }

    public void manageActionNoticias() {
        goToTheActivity(Constants.NEWS_ACTIVITY);
    }

    public void manageActionFactura() {
        Intent intent;
        if (getUsuario() != null && getUsuario().isInvitado() == false) {
            sendReportToGoogleAnalytics(Constants.FACTURA_REGISTRADO);
            goToTheActivity(Constants.FACTURA_CLASS);
        } else {
            try {
                sendReportToGoogleAnalytics(Constants.FACTURA_INVITADO);
                Class clazz = Class.forName(customSharedPreferences.getString(Constants.CHECK_INVOCE));
                intent = new Intent(this, clazz);
                intent.putExtra(Constants.TRUE, true);
                intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivityWithOutDoubleClick(intent);
            } catch (ClassNotFoundException e) {
                Log.e(Constants.EXCEPTION_STRING, e.toString());
            }
        }

    }

    public CustomSharedPreferences getCustomSharedPreferences() {
        if (customSharedPreferences == null) {
            customSharedPreferences = new CustomSharedPreferences(this);
        }
        return customSharedPreferences;
    }

    public void setCustomSharedPreferences(CustomSharedPreferences customSharedPreferences) {
        this.customSharedPreferences = customSharedPreferences;
    }

    @Override
    public void validateShowAlertQualifyApp() {
        if (RateApp.validateShowAlertQualifyApp(customSharedPreferences)) {
            showAlertQualifyApp();
        }
    }

    private void showAlertQualifyApp() {
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setMessage(R.string.text_rate_app);
        builder.setCancelable(false);
        builder.setNeutralButton(R.string.text_later, (dialog, which) -> {
            customSharedPreferences.addInt(Constants.OPEN_QUALIFY_APP, Constants.ONE);
            dialog.dismiss();
        });
        builder.setNegativeButton(R.string.text_no_thanks, (dialog, which) -> {
            customSharedPreferences.addInt(Constants.OPEN_QUALIFY_APP, Constants.THREE);
            dialog.dismiss();
        });
        builder.setPositiveButton(R.string.text_go_store, (dialog, which) -> {
            customSharedPreferences.addInt(Constants.OPEN_QUALIFY_APP, Constants.TWELVE);
            openPlayStore();
        });
        builder.show();
    }

    private void openPlayStore() {
        CustomApplicationContext.getInstance().getMenuFragmentListener().rateApp();
    }

    protected void notificationsCenter() {
        try {
            Class clazz = Class.forName(customSharedPreferences.getString(Constants.NOTIFICATION_CENTER_CLASS));
            Intent intent = new Intent(this, clazz);
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
            intent.putExtra(Constants.PACKAGE_NAME_TO_NOTIFICATIONS,this.getClass().getPackage().getName());
            startActivityWithOutDoubleClick(intent);
        } catch (ClassNotFoundException e) {
            Log.e(Constants.EXCEPTION_STRING, e.toString());
        }

    }


    @Override
    protected void onPause() {
        new ValidateServiceCode();
        super.onPause();
    }

    @Override
    public void updateCounter() {
        invalidateOptionsMenu();
    }


    public void generalPositiveAction() {

    }

    private void goToTheActivity(String activity , boolean cleanHistory) {
        try {
            Class clazz = Class.forName(customSharedPreferences.getString(activity));
            Intent intent = new Intent(this, clazz);
            if(cleanHistory){intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);}
            startActivityWithOutDoubleClick(intent);

        } catch (ClassNotFoundException e) {
            Log.e(Constants.EXCEPTION_STRING, e.toString());
        }
    }

    private void goToTheActivity(String activity) {
        try {
            Class clazz = Class.forName(customSharedPreferences.getString(activity));
            Intent intent = new Intent(this, clazz);
           // intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
            startActivityWithOutDoubleClick(intent);

        } catch (ClassNotFoundException e) {
            Log.e(Constants.EXCEPTION_STRING, e.toString());
        }
    }

    public void startActivityWithOutDoubleClick(Intent intent){
        if(!doubleClick){
            packageName = this.getClass().getPackage().getName();
            doubleClick = true;
            startActivityForResult(intent,Constants.DEFAUL_REQUEST_CODE);
        }
    }

    public void startActivityWithOutDoubleClick(Intent intent, FragmentActivity context){
        if(!doubleClick){
            doubleClick = true;
            context.startActivityForResult(intent,Constants.DEFAUL_REQUEST_CODE);
        }
    }

    public void startActivityWithOutDoubleClick(Intent intent,int resultIntent){
        if(!doubleClick){
            doubleClick = true;
            startActivityForResult(intent,resultIntent);
        }
    }

    @Override
    public void changeState() {
        try {
            if(packageName != null && (packageName.contains(Constants.TURN_SERVICE_PACKAGE) || packageName.contains(Constants.PROCEDURE_SERVICE_PACKAGE))){
                Class clazz = Class.forName(customSharedPreferences.getString(Constants.LANDING_CLASS));
                Intent intent = new Intent(this, clazz);
                intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
            }
        } catch (ClassNotFoundException e) {
            Log.e(Constants.EXCEPTION_STRING, e.toString());
        }


    }





}