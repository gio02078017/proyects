package app.epm.com.utilities.helpers;

import java.io.Serializable;

public class TypeDanioOrFraude implements Serializable {

    private String nameType;
    private String id;


    public String getNameType() {
        return nameType;
    }

    public void setNameType(String nameType) {
        this.nameType = nameType;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }
}
