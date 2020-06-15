package com.epm.app.business_models.dto;

public class MapDTO {

    private String Id;

    private String Menu;

    private BaseMapsDTO BaseMaps;

    private EditFeatureLayersDTO EditFeatureLayers;

    public String getId() {
        return Id;
    }

    public void setId(String id) {
        Id = id;
    }

    public String getMenu() {
        return Menu;
    }

    public void setMenu(String menu) {
        Menu = menu;
    }

    public BaseMapsDTO getBaseMaps() {
        return BaseMaps;
    }

    public void setBaseMaps(BaseMapsDTO baseMaps) {
        BaseMaps = baseMaps;
    }

    public EditFeatureLayersDTO getEditFeatureLayers() {
        return EditFeatureLayers;
    }

    public void setEditFeatureLayers(EditFeatureLayersDTO editFeatureLayers) {
        EditFeatureLayers = editFeatureLayers;
    }
}
