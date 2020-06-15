package com.epm.app.mvvm.turn.models;



public class StateMessage<T> {

    private T title;
    private T message;

    public T getTitle() {
        return title;
    }

    public void setTitle(T title) {
        this.title = title;
    }

    public T getMessage() {
        return message;
    }

    public void setMessage(T message) {
        this.message = message;
    }
}
