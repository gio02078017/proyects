package app.epm.com.utilities.entities;

public class FeaturesPointMapItem {
    private String name;
    private String itemSearch;
    private String value;
    private boolean require;
    private boolean empty;

    public FeaturesPointMapItem(String name, String itemSearch, String require, String empty) {
        this.name = name;
        this.itemSearch = itemSearch;
        this.require = require.equalsIgnoreCase("1") ? true: false;
        this.empty = empty.equalsIgnoreCase("1") ? true: false;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getItemSearch() {
        return itemSearch;
    }

    public void setItemSearch(String itemSearch) {
        this.itemSearch = itemSearch;
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }

    public boolean isRequire() {
        return require;
    }

    public void setRequire(boolean require) {
        this.require = require;
    }

    public boolean isEmpty() {
        return empty;
    }

    public void setEmpty(boolean empty) {
        this.empty = empty;
    }
}
