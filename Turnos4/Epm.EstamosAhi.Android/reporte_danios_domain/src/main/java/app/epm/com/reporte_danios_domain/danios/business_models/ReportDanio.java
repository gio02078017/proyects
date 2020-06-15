package app.epm.com.reporte_danios_domain.danios.business_models;

import com.epm.app.business_models.business_models.ETipoServicio;

import java.io.Serializable;
import java.util.ArrayList;

public class ReportDanio implements Serializable{

    private String telephoneUserReport;
    private ArrayList<String> arrayFiles;
    private String address;
    private String describeReport;
    private String typeReporte;
    private String idTypeReporte;
    private String userName;
    private String userEmail;
    private String lugarReferencia;
    private String pointSelectedSerializable;
    private String urlServicio;
    private ETipoServicio tipoServicio;

    public String getIdTypeReporte() {
        return idTypeReporte;
    }

    public void setIdTypeReporte(String idTypeReporte) {
        this.idTypeReporte = idTypeReporte;
    }

    public ETipoServicio getTipoServicio() {
        return tipoServicio;
    }

    public void setTipoServicio(ETipoServicio tipoServicio) {
        this.tipoServicio = tipoServicio;
    }
    public String getUserEmail() {
        return userEmail;
    }

    public void setUserEmail(String userEmail) {
        this.userEmail = userEmail;
    }

    public String getUrlServicio() {
        return urlServicio;
    }

    public void setUrlServicio(String urlServicio) {
        this.urlServicio = urlServicio;
    }

    public String getPointSelectedSerializable() {
        return pointSelectedSerializable;
    }

    public void setPointSelectedSerializable(String pointSelected) {
        this.pointSelectedSerializable = pointSelected;
    }

    public ArrayList<String> getArrayFiles() {
        return arrayFiles;
    }

    public void setArrayFiles(ArrayList<String> arrayFiles) {
        this.arrayFiles = arrayFiles;
    }

    public String getTelephoneUserReport() {
        return telephoneUserReport;
    }

    public void setTelephoneUserReport(String telephoneUserReport) {
        this.telephoneUserReport = telephoneUserReport;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public String getDescribeReport() {
        return describeReport;
    }

    public void setDescribeReport(String describeReport) {
        this.describeReport = describeReport;
    }

    public String getTypeReporte() {
        return typeReporte;
    }

    public void setTypeReporte(String typeReporte) {
        this.typeReporte = typeReporte;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getLugarReferencia() {
        return lugarReferencia;
    }

    public void setLugarReferencia(String lugarReferencia) {
        this.lugarReferencia = lugarReferencia;
    }
}
