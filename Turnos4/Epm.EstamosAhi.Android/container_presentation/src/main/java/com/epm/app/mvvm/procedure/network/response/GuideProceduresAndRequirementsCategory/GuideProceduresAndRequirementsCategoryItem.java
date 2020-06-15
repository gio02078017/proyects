
package com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.io.Serializable;

public class GuideProceduresAndRequirementsCategoryItem implements Serializable {

    @SerializedName("Id")
    @Expose
    private String categoryId;
    @SerializedName("NombreCategoria")
    @Expose
    private String categoryName;
    @SerializedName("Activo")
    @Expose
    private boolean state;
    @SerializedName("Orden")
    @Expose
    private Integer order;
    @SerializedName("NombreImagen")
    @Expose
    private String imagenName;

    public GuideProceduresAndRequirementsCategoryItem() { }

    public GuideProceduresAndRequirementsCategoryItem(String categoryId, String categoryName, boolean state) {
        this.categoryId = categoryId;
        this.categoryName = categoryName;
        this.state = state;
    }

    public String getCategoryId() {
        return categoryId;
    }

    public void setCategoryId(String categoryId) {
        this.categoryId = categoryId;
    }

    public String getCategoryName() {
        return categoryName;
    }

    public void setCategoryName(String categoryName) {
        this.categoryName = categoryName;
    }

    public boolean isState() {
        return state;
    }

    public void setState(boolean state) {
        this.state = state;
    }

    public Integer getOrder() {
        return order;
    }

    public void setOrder(Integer order) {
        this.order = order;
    }

    public String getImagenName() {
        return imagenName;
    }

    public void setImagenName(String imagenName) {
        this.imagenName = imagenName;
    }
}
