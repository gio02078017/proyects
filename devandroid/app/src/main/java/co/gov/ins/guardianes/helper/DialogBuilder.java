package co.gov.ins.guardianes.helper;

import android.content.Context;
import com.afollestad.materialdialogs.MaterialDialog;
import co.gov.ins.guardianes.R;

public final class DialogBuilder extends BaseBuilder {

    public DialogBuilder(final Context context) {
        super(context);
    }

    public MaterialDialog.Builder load() {

        return new MaterialDialog.Builder(getContext())
                                 .titleColorRes(R.color.colombia_primary)
                                 .contentColorRes(R.color.black)
                                 .negativeColorRes(R.color.black)
                                 .positiveColorRes(R.color.colombia_primary);
    }
}
