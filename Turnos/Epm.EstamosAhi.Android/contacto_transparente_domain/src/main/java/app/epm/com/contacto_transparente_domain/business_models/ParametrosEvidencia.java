package app.epm.com.contacto_transparente_domain.business_models;

import java.util.ArrayList;

/**
 * Created by leidycarolinazuluagabastidas on 16/03/17.
 */

public class ParametrosEvidencia {

    private String codigoDelActoIndebido;
    private ArrayList<Evidencia> evidenciasDelActoIndebido;

    public String getCodigoDelActoIndebido() {
        return codigoDelActoIndebido;
    }

    public void setCodigoDelActoIndebido(String codigoDelActoIndebido) {
        this.codigoDelActoIndebido = codigoDelActoIndebido;
    }

    public ArrayList<Evidencia> getEvidenciasDelActoIndebido() {
        return evidenciasDelActoIndebido;
    }

    public void setEvidenciasDelActoIndebido(ArrayList<Evidencia> evidenciasDelActoIndebido) {
        this.evidenciasDelActoIndebido = evidenciasDelActoIndebido;
    }
}
