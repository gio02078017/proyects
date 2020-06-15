package app.epm.com.contacto_transparente_presentation.view.Adapters;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;

import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_presentation.view.fragments.ShowIncidentFragment;
import app.epm.com.contacto_transparente_presentation.view.fragments.ShowIncidentStatusFragmet;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluaga on 30/08/16.
 */

public class SwipeConsultDescription extends FragmentPagerAdapter {


    private Incidente incidentConsult;
    // Cantidad de fragmentos para usar en el swipe.
    private static int NUM_ITEMS = 2;

    // Constructor.
    public SwipeConsultDescription(FragmentManager fragmentManager, Incidente incidentConsult) {
        super(fragmentManager);
        this.incidentConsult = incidentConsult;
    }


    // Permite obtener el número de fragmentos.
    @Override
    public int getCount() {
        return NUM_ITEMS;
    }


    // Permite obtener el fragmento a mostrar.
    @Override
    public Fragment getItem(int position) {
        switch (position) {
            case 0:
                return new ShowIncidentStatusFragmet().consultStatusIncident(incidentConsult);
            case 1:
                return ShowIncidentFragment.consultDataIncident(incidentConsult);
            default:
                return null;
        }
    }

    @Override
    public CharSequence getPageTitle(int position) {
        switch (position) {
            case 0:
                return Constants.TITLE_FRAGMENT_STATUS;
            case 1:
                return Constants.TITLE_FRAGMENT_DATA;
            default:
                return null;
        }
    }
}
