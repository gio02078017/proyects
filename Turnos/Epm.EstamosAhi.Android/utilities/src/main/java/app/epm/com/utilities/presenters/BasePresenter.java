package app.epm.com.utilities.presenters;

import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class BasePresenter<T extends IBaseView> {

    private T view;
    private IValidateInternet validateInternet;

    public void inject(T view, IValidateInternet validateInternet) {
        this.view = view;
        this.validateInternet = validateInternet;
    }

    public T getView() {
        return view;
    }

    public void setView(T view) {
        this.view = view;
    }

    public IValidateInternet getValidateInternet() {
        return validateInternet;
    }

    public void setValidateInternet(IValidateInternet validateInternet) {
        this.validateInternet = validateInternet;
    }
}
