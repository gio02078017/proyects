package app.epm.com.contacto_transparente_presentation.view.fragments;

import android.graphics.Rect;
import android.os.Bundle;
import android.support.v7.app.AlertDialog;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;

import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.utilities.helpers.CustomAlertDialogInputData;
import app.epm.com.utilities.view.fragments.BaseFragment;

/**
 * Created by leidycarolinazuluagabastidas on 17/03/17.
 */

public class ShowIncidentFragment extends BaseFragment {

    private Incidente incident;
    private View view;
    private EditText data_etPlace;
    private EditText data_etDescription;
    private EditText data_etPeople;
    private EditText data_etCompany;
    private EditText data_etCalendar;
    private EditText data_etName;
    private EditText data_etEmail;
    private EditText data_etPhone;
    private EditText data_etGroup;
    private ImageView data_ivPlace;
    private ImageView data_ivDescription;
    private ImageView data_ivPeolple;
    private ImageView data_ivCompany;
    private TextView data_tvName;
    private TextView data_tvEmail;
    private TextView data_tvPhone;
    private TextView data_tvGroup;
    private View data_vLineCalendar;
    private CustomAlertDialogInputData customAlertDialogInputData;

    public static ShowIncidentFragment consultDataIncident(Incidente incident) {
        ShowIncidentFragment fragment = new ShowIncidentFragment();
        fragment.setIncident(incident);
        return fragment;
    }

    public void setIncident(Incidente incident) {
        this.incident = incident;
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        this.view = inflater.inflate(R.layout.fragment_show_incident,container,false);
        customAlertDialogInputData = new CustomAlertDialogInputData();
        loadViews();
        showConsultIncident();
        return view;
    }

    private void showConsultIncident() {
        if(!incident.isAnonimo()){
            data_tvName.setVisibility(View.VISIBLE);
            data_etName.setVisibility(View.VISIBLE);
            data_tvEmail.setVisibility(View.VISIBLE);
            data_etEmail.setVisibility(View.VISIBLE);
            data_tvPhone.setVisibility(View.VISIBLE);
            data_etPhone.setVisibility(View.VISIBLE);
            data_tvGroup.setVisibility(View.VISIBLE);
            data_etGroup.setVisibility(View.VISIBLE);
            data_etName.setText(incident.getNombreContacto());
            data_etEmail.setText(incident.getCorreoElectronicoContacto());
            data_etPhone.setText(incident.getTelefonoContacto());
            data_etGroup.setText(incident.getNombreGrupoInteres());
            data_vLineCalendar.setVisibility(view.GONE);
        }
        data_etPlace.setText(incident.getLugarEnDondeSucedio());
        countLineTextView(data_etPlace,data_ivPlace);
        data_etDescription.setText(incident.getDescripcion());
        countLineTextView(data_etDescription, data_ivDescription);
        data_etPeople.setText(incident.getPersonasInvolucradas());
        countLineTextView(data_etPeople,data_ivPeolple);
        data_etCompany.setText(incident.getPersonasInvolucradasEnLaEmpresa());
        countLineTextView(data_etCompany,data_ivCompany);
        data_etCalendar.setText(incident.getFechaDeteccion());
        data_vLineCalendar.setVisibility(view.VISIBLE);
    }

    private void loadViews() {
        data_etPlace = view.findViewById(R.id.data_etPlace);
        data_etDescription =  view.findViewById(R.id.data_etDescription);
        data_etPeople = view.findViewById(R.id.data_etPeople);
        data_etCompany =  view.findViewById(R.id.data_etCompany);
        data_etCalendar =  view.findViewById(R.id.data_etCalendar);
        data_etName = view.findViewById(R.id.data_etName);
        data_etEmail =  view.findViewById(R.id.data_etEmail);
        data_etPhone =  view.findViewById(R.id.data_etPhone);
        data_etGroup =  view.findViewById(R.id.data_etGroup);
        data_ivPlace =  view.findViewById(R.id.data_ivPlace);
        data_ivDescription =  view.findViewById(R.id.data_ivDescription);
        data_ivPeolple = view.findViewById(R.id.data_ivPeolple);
        data_ivCompany =  view.findViewById(R.id.data_ivCompany);
        data_tvName =  view.findViewById(R.id.data_tvName);
        data_tvEmail =  view.findViewById(R.id.data_tvEmail);
        data_tvPhone =  view.findViewById(R.id.data_tvPhone);
        data_tvGroup =  view.findViewById(R.id.data_tvGroup);
        data_vLineCalendar = view.findViewById(R.id.data_vLineCalendar);
        loadListener();
    }

    private void loadListener() {

        final Window window = getActivity().getWindow();
        final LayoutInflater inflate = getActivity().getLayoutInflater();

        data_ivPlace.setOnClickListener(view -> showAlertDialogConsult(incident.getLugarEnDondeSucedio()));

        data_ivDescription.setOnClickListener(view -> showAlertDialogConsult(incident.getDescripcion()));

        data_ivPeolple.setOnClickListener(view -> showAlertDialogConsult(incident.getPersonasInvolucradas()));

        data_ivCompany.setOnClickListener(view -> showAlertDialogConsult(incident.getPersonasInvolucradasEnLaEmpresa()));
    }

    private void showAlertDialogConsult(String description) {
        AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        LayoutInflater inflater = getActivity().getLayoutInflater();
        View view = inflater.inflate(app.epm.com.utilities.R.layout.custom_alert_dialog_describe_consult,null);
        Rect displayRectangle = new Rect();
        Window window = getActivity().getWindow();
        window.getDecorView().getWindowVisibleDisplayFrame(displayRectangle);
        view.setMinimumWidth((int)(displayRectangle.width() * 0.9f));
        view.setMinimumHeight((int)(displayRectangle.height() * 0.9f));
        TextView tvDescription =  view.findViewById(app.epm.com.utilities.R.id.tv_description);
        builder.setView(view).setPositiveButton(app.epm.com.utilities.R.string.text_aceptar, (dialogInterface, i) -> dialogInterface.dismiss());
        tvDescription.setText(description);
        builder.show();
    }



    private void countLineTextView(final EditText editText, final ImageView imageView) {
        editText.post(() -> {
            if(editText.getLineCount() >= 3){
                editText.setLines(3);
                imageView.setVisibility(View.VISIBLE);
            }
        });
    }
}
