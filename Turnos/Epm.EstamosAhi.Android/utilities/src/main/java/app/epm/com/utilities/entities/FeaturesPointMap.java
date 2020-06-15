package app.epm.com.utilities.entities;

import java.util.ArrayList;

public class FeaturesPointMap {

    private ArrayList<FeaturesPointMapItem> listFeaturesPointMapItem;
    private boolean verified;

    public ArrayList<FeaturesPointMapItem> getListFeaturesPointMapItem() {
        return listFeaturesPointMapItem;
    }

    public void setListFeaturesPointMapItem(ArrayList<FeaturesPointMapItem> listFeaturesPointMapItem) {
        this.listFeaturesPointMapItem = listFeaturesPointMapItem;
    }

    public boolean isVerified() {
        return verified;
    }

    public void setVerified(boolean verified) {
        this.verified = verified;
    }
}
