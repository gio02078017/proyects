package com.epm.app.mvvm.turn.network.response;


import com.epm.app.mvvm.turn.network.response.officeDetail.Oficina;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class AssignedTurn {

    @SerializedName("TurnoDispositivo")
    @Expose
    private ShiftDevice shiftDevice;
    @SerializedName("Oficina")
    @Expose
    private Oficina office;
    @SerializedName("Mensaje")
    @Expose
    private Mensaje message;

    public ShiftDevice getShiftDevice() {
        return shiftDevice;
    }

    public void setShiftDevice(ShiftDevice shiftDevice) {
        this.shiftDevice = shiftDevice;
    }

    public Oficina getOffice() {
        return office;
    }

    public void setOffice(Oficina office) {
        this.office = office;
    }

    public Mensaje getMessage() {
        return message;
    }

    public void setMessage(Mensaje message) {
        this.message = message;
    }

}