package app.epm.com.reporte_fraudes_domain.business_models;

import com.epm.app.business_models.business_models.ETipoServicio;
import com.esri.arcgisruntime.geometry.Point;

import java.io.Serializable;
import java.util.ArrayList;

import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 17/04/17.
 */

public class ReporteDeFraude implements Serializable {

    private String idType;
    private String telephone;
    private ArrayList<String> arrayFiles;
    private String address;
    private String sector;
    private String municipality;
    private String describeReport;
    private String horary;
    private String typeReport;
    private String userName;
    private String userEmail;
    private String referencePlace;
    private String pointSelectedSerializable;
    private String urlServices;
    private ETipoServicio typeService;

    public String getIdType() {
        return idType;
    }

    public void setIdType(String idType) {
        this.idType = idType;
    }

    public String getTelephone() {
        return telephone;
    }

    public void setTelephone(String telephone) {
        this.telephone = telephone;
    }

    public ArrayList<String> getArrayFiles() {
        return arrayFiles;
    }

    public void setArrayFiles(ArrayList<String> arrayFiles) {
        this.arrayFiles = arrayFiles;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public String getSector() {
        return sector;
    }

    public void setSector(String sector) {
        this.sector = sector;
    }

    public String getMunicipality() {
        return municipality;
    }

    public void setMunicipality(String municipality) {
        this.municipality = municipality;
    }

    public String getDescribeReport() {
        return describeReport;
    }

    public void setDescribeReport(String describeReport) {
        this.describeReport = describeReport;
    }

    public String getHorary() {
        return horary;
    }

    public void setHorary(String horary) {
        this.horary = horary;
    }

    public String getTypeReport() {
        return typeReport;
    }

    public void setTypeReport(String typeReport) {
        this.typeReport = typeReport;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getUserEmail() {
        return userEmail;
    }

    public void setUserEmail(String userEmail) {
        this.userEmail = userEmail;
    }

    public String getReferencePlace() {
        return referencePlace;
    }

    public void setReferencePlace(String referencePlace) {
        this.referencePlace = referencePlace;
    }

    public String getPointSelectedSerializable() {
        return pointSelectedSerializable;
    }

    public void setPointSelectedSerializable(String pointSelected) {
        this.pointSelectedSerializable = pointSelected;
    }

    public String getUrlServices() {
        return urlServices;
    }

    public void setUrlServices(String urlServices) {
        this.urlServices = urlServices;
    }

    public ETipoServicio getTypeService() {
        return typeService;
    }

    public void setTypeService(ETipoServicio typeService) {
        this.typeService = typeService;
    }
}
