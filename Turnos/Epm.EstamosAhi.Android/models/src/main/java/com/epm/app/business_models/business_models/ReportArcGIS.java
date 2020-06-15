package com.epm.app.business_models.business_models;


import java.io.File;
import java.util.ArrayList;

/**
 * Created by ocadavid on 27/03/2017.
 */

public class ReportArcGIS {
    private String idType;
    private String nameTypeTemplate;
    private String description;
    private String telephone;
    private String address;
    private String name;
    private String email;
    private String locationReference;
    private int state;
    private int affectedService;
    private String hour;
    private ArrayList<File> fileAttachments;

    public String getIdType() {
        return idType;
    }

    public void setIdType(String idType) {
        this.idType = idType;
    }

    public ReportArcGIS() {
        this.state = 1;
    }

    public String getNameTypeTemplate() {
        return nameTypeTemplate;
    }

    public void setNameTypeTemplate(String nameTypeTemplate) {
        this.nameTypeTemplate = nameTypeTemplate;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getTelephone() {
        return telephone;
    }

    public void setTelephone(String telephone) {
        this.telephone = telephone;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getLocationReference() {
        return locationReference;
    }

    public void setLocationReference(String locationReference) {
        this.locationReference = locationReference;
    }

    public int getState() {
        return state;
    }

    public int getAffectedService() {
        return affectedService;
    }

    public void setAffectedService(int affectedService) {
        this.affectedService = affectedService;
    }

    public String getHour() {
        return hour;
    }

    public void setHour(String hour) {
        this.hour = hour;
    }

    public ArrayList<File> getFileAttachments() {
        return fileAttachments;
    }

    public void setFileAttachments(ArrayList<File> fileAttachments) {
        this.fileAttachments = fileAttachments;
    }
}
