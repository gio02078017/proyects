package com.epm.app.business_models.business_models;

/**
 * Created by josetabaresramirez on 17/11/16.
 */

public class RepositoryError extends Exception {

    private int idError;

    public RepositoryError(String message) {
        super(message);
    }

    public int getIdError() {
        return idError;
    }

    public void setIdError(int idError) {
        this.idError = idError;
    }
}
