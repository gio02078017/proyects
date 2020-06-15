package app.epm.com.utilities.helpers;

public class ErrorMessage {

    private Integer title;
    private Integer message;

    public ErrorMessage(Integer title, Integer message) {
        this.title = title;
        this.message = message;
    }

    public ErrorMessage() {
    }

    public int getTitle() {
        return title;
    }

    public void setTitle(Integer title) {
        this.title = title;
    }

    public Integer getMessage() {
        return message;
    }

    public void setMessage(Integer message) {
        this.message = message;
    }
}
