package app.epm.com.utilities.helpers;

import android.content.Context;
import android.content.DialogInterface;
import android.graphics.Rect;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.text.InputFilter;
import android.view.LayoutInflater;
import android.view.View;
import android.view.Window;
import android.view.inputmethod.InputMethodManager;
import android.widget.EditText;
import android.widget.TextView;

import java.util.TimerTask;

import app.epm.com.utilities.R;

/**
 * Created by leidycarolinazuluagabastidas on 13/03/17.
 */

public class CustomAlertDialogInputData extends AppCompatActivity{

    public void showAlertDialogInputData(Window windows, LayoutInflater inflate, Context context, int title, final EditText editText,String text, int length) {
        AlertDialog.Builder builder = new AlertDialog.Builder(context);
        LayoutInflater inflater = inflate;
        View content = inflater.inflate(R.layout.custom_alert_dialog_input_data, null);
        Rect displayRectangle = new Rect();
        Window window = windows;
        window.getDecorView().getWindowVisibleDisplayFrame(displayRectangle);
        content.setMinimumWidth((int) (displayRectangle.width() * 0.9f));
        content.setMinimumHeight((int) (displayRectangle.height() * 0.9f));
        final EditText etInputData = (EditText) content.findViewById(R.id.edt_InputData);
        builder.setView(content).setPositiveButton(R.string.text_save_info, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                editText.setText(etInputData.getText().toString().trim());
                dialogInterface.dismiss();
            }
        }).setNegativeButton(R.string.text_cancelar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                dialogInterface.dismiss();
            }
        });
        etInputData.setFilters(new InputFilter[]{new InputFilter.LengthFilter(length)});
        etInputData.setText(text);
        etInputData.setSelection(text.length());
        builder.setTitle(title);
        builder.show();
        openKeyboard(etInputData, context);
    }

    private void openKeyboard(final EditText editText, final Context context) {
        editText.postDelayed(new TimerTask() {
            @Override
            public void run() {
                editText.requestFocus();
                InputMethodManager imm = (InputMethodManager) context.getSystemService(Context.INPUT_METHOD_SERVICE);
                imm.showSoftInput(editText, InputMethodManager.SHOW_IMPLICIT);
            }
        }, 400);
    }


}
