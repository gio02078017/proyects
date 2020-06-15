package com.epm.app.mvvm.comunidad.network.response;

public class ErrorService {

    private Throwable throwable = null;

    public Throwable getThrowable() {
        return throwable;
    }

    public void setThrowable(Throwable throwable) {
        this.throwable = throwable;
    }
}
