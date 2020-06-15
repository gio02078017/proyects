package com.epm.app.mvvm.turn.views.dialogs;

import android.content.Context;
import android.os.Bundle;
import androidx.annotation.Nullable;
import com.google.android.material.bottomsheet.BottomSheetDialogFragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;

import com.epm.app.R;

public class ChooseMapProviderDialogFragment extends BottomSheetDialogFragment {

    private BottomSheetListener mListener;
    private LinearLayout lyGoogleMaps, lyWaze;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.dialog_choose_map_provider, container, false);

        lyGoogleMaps = view.findViewById(R.id.lyGoogleMaps);
        lyWaze = view.findViewById(R.id.lyWaze);

        startListener();

        return view;
    }

    private void startListener(){
        lyGoogleMaps.setOnClickListener(view1 -> {
            mListener.onChoseGoogleMapsClicked();
            dismiss();
        });

        lyWaze.setOnClickListener(view12 -> {
            mListener.onChoseWaseClicked();
            dismiss();
        });
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        try{
            mListener = (BottomSheetListener)context;
        }catch(ClassCastException e){
            throw new ClassCastException(context.toString() + "must implement BottomSheetListener");
        }
    }

    @Override
    public void onDetach() {
        mListener = null;
        super.onDetach();
    }

    public interface BottomSheetListener {
        void onChoseGoogleMapsClicked();
        void onChoseWaseClicked();
    }
}
