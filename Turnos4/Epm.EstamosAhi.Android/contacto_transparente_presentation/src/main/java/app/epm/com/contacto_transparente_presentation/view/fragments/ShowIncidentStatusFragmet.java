package app.epm.com.contacto_transparente_presentation.view.fragments;

import android.graphics.Rect;
import android.os.Bundle;
import android.support.v7.app.AlertDialog;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.Button;
import android.widget.TextView;

import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.utilities.helpers.CustomAlertDialogInputData;
import app.epm.com.utilities.view.fragments.BaseFragment;

/**
 * Created by leidycarolinazuluagabastidas on 17/03/17.
 */

public class ShowIncidentStatusFragmet extends BaseFragment {

    private Incidente incident;
    private View view;
    private TextView status_tvCreateDate;
    private TextView status_tvStatus;
    private TextView status_tvCalendar;
    private TextView status_tvTracingDate;
    private Button status_btnSee;
    private View status_vLine;
    private CustomAlertDialogInputData customAlertDialogInputData;

    public static ShowIncidentStatusFragmet consultStatusIncident(Incidente incidente) {
        ShowIncidentStatusFragmet fragmet = new ShowIncidentStatusFragmet();
        fragmet.setIncident(incidente);
        return fragmet;
    }

    public void setIncident(Incidente incident) {
        this.incident = incident;
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        this.view = inflater.inflate(R.layout.fragment_show_incident_status, container, false);
        loadViews();
        showConsultIncident();
        return view;
    }

    private void showConsultIncident() {
        if (incident.getIdEstado() != 1) {
            status_tvCalendar.setVisibility(View.VISIBLE);
            status_tvTracingDate.setVisibility(View.VISIBLE);
            status_vLine.setVisibility(View.VISIBLE);
            status_tvTracingDate.setText(incident.getFechaDeSeguimiento());
        }
        status_tvCreateDate.setText(incident.getFechaCreacion());
        status_tvStatus.setText(incident.getNombreDelEstado());
        if (incident.getInformeDeLaDenuncia() != null && !incident.getInformeDeLaDenuncia().isEmpty()) {
            status_btnSee.setVisibility(View.VISIBLE);
        }
    }

    private void loadViews() {
        status_tvCreateDate =  view.findViewById(R.id.status_tvCreateDate);
        status_tvStatus =  view.findViewById(R.id.status_tvStatus);
        status_tvCalendar =  view.findViewById(R.id.status_tvCalendar);
        status_tvTracingDate =  view.findViewById(R.id.status_tvTracingDate);
        status_btnSee =  view.findViewById(R.id.status_btnSee);
        status_vLine = view.findViewById(R.id.status_vLine);
        loadListener();
    }

    private void loadListener() {

        final Window window = getActivity().getWindow();
        final LayoutInflater inflate = getActivity().getLayoutInflater();

        status_btnSee.setOnClickListener(view -> showAlertDialogConsult(incident.getInformeDeLaDenuncia()));
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
}
