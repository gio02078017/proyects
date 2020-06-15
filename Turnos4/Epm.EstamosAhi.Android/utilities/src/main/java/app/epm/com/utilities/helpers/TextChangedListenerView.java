package app.epm.com.utilities.helpers;

import android.text.Editable;
import android.text.TextWatcher;

/**
 * Created by leidycarolinazuluagabastidas on 7/12/16.
 */

public abstract class TextChangedListenerView<T> implements TextWatcher {
    private T target;

    public TextChangedListenerView(T target) {
        this.target = target;
    }

    @Override
    public void beforeTextChanged(CharSequence s, int start, int count, int after) {}

    @Override
    public void onTextChanged(CharSequence s, int start, int before, int count) {}

    @Override
    public void afterTextChanged(Editable s) {
        this.onTextChangedView(target);
    }

    public abstract void onTextChangedView(T target);

}
