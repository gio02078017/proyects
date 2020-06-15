package com.epm.app.mvvm.turn.network.response.officeDetail;

import com.epm.app.mvvm.turn.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.io.Serializable;

public class OfficeDetailResponse implements Serializable {

    @SerializedName("Turno")
    @Expose
    private Turno turno;
    @SerializedName("Oficina")
    @Expose
    private Oficina oficina;
    @SerializedName("DispositivoTieneTurnoAsignado")
    @Expose
    private Boolean dispositivoTieneTurnoAsignado;
    @SerializedName("AplicaSolicitudTurno")
    @Expose
    private Boolean aplicaSolicitudTurno;
    @SerializedName("TiempoParaRefrescarDetalleOficina")
    @Expose
    private Integer tiempoParaRefrescarDetalleOficina;
    @SerializedName("TiempoDeEsperaDetalleOficina")
    @Expose
    private Integer tiempoDeEsperaDetalleOficina;
    @SerializedName("TransactionServiceMessage")
    @Expose
    private Mensaje mensaje;

    public Turno getTurno() {
        return turno;
    }

    public void setTurno(Turno turno) {
        this.turno = turno;
    }

    public Oficina getOficina() {
        return oficina;
    }

    public void setOficina(Oficina oficina) {
        this.oficina = oficina;
    }

    public Boolean getDispositivoTieneTurnoAsignado() {
        return dispositivoTieneTurnoAsignado;
    }

    public void setDispositivoTieneTurnoAsignado(Boolean dispositivoTieneTurnoAsignado) {
        this.dispositivoTieneTurnoAsignado = dispositivoTieneTurnoAsignado;
    }

    public Boolean getAplicaSolicitudTurno() {
        return aplicaSolicitudTurno;
    }

    public void setAplicaSolicitudTurno(Boolean aplicaSolicitudTurno) {
        this.aplicaSolicitudTurno = aplicaSolicitudTurno;
    }

    public Integer getTiempoParaRefrescarDetalleOficina() {
        return tiempoParaRefrescarDetalleOficina;
    }

    public void setTiempoParaRefrescarDetalleOficina(Integer tiempoParaRefrescarDetalleOficina) {
        this.tiempoParaRefrescarDetalleOficina = tiempoParaRefrescarDetalleOficina;
    }

    public Integer getTiempoDeEsperaDetalleOficina() {
        return tiempoDeEsperaDetalleOficina;
    }

    public void setTiempoDeEsperaDetalleOficina(Integer tiempoDeEsperaDetalleOficina) {
        this.tiempoDeEsperaDetalleOficina = tiempoDeEsperaDetalleOficina;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

}
