package app.epm.com.utilities.controls;

import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.widget.EditText;

/**
 * Created by josetabaresramirez on 9/11/16.
 */

public class TextWatcherCurrency implements TextWatcher {

    private EditText editText;
    private String beforeText;

    public TextWatcherCurrency(EditText editText) {
        this.editText = editText;
    }

    @Override
    public void beforeTextChanged(CharSequence s, int start, int count, int after) {
        beforeText = s.toString();
    }

    @Override
    public void onTextChanged(CharSequence s, int start, int before, int count) {
        editText.removeTextChangedListener(this);
        if (s.length() > 26) {
            editText.setText(beforeText);
            editText.setSelection(beforeText.length());
            editText.addTextChangedListener(this);
            return;
        }
        String currency = s.toString().replaceAll("[$,.]", "");
        try {
            currency = Formats.getCurrency(Long.parseLong(currency));
            editText.setText(currency);
            try {
                editText.setSelection(currency.length());
            } catch (IndexOutOfBoundsException e) {
                Log.e("Exception", e.toString());
            }
        } catch (NumberFormatException numberFormatException) {
            Log.e("TextWatcherCurrency - onTextChanged", numberFormatException.getMessage());
            editText.setText(currency.isEmpty() ? currency : beforeText);
            editText.setSelection(currency.isEmpty() ? currency.length() : beforeText.length());
        } finally {
            editText.addTextChangedListener(this);
        }
    }

    @Override
    public void afterTextChanged(Editable s) {
        //The method is not used.
    }
}