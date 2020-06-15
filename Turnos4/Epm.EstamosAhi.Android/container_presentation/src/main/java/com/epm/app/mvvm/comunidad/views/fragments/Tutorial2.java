package com.epm.app.mvvm.comunidad.views.fragments;


import android.arch.lifecycle.ViewModelProviders;
import android.databinding.DataBindingUtil;
import android.net.Uri;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import com.epm.app.R;
import com.epm.app.databinding.FragmentTutorial2Binding;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.ITutorialViewModel;
import com.epm.app.mvvm.comunidad.viewModel.TutorialViewModel;


import app.epm.com.utilities.view.fragments.BaseFragment;

public class Tutorial2 extends BaseFragment {


    ITutorialViewModel tutorialViewModel;
    private View vista;
    Button btnSaltarIntro;
    private OnFragmentInteractionListener mListener;


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        tutorialViewModel = ViewModelProviders.of(getActivity()).get(TutorialViewModel.class);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        FragmentTutorial2Binding binding = DataBindingUtil.inflate(inflater,R.layout.fragment_tutorial2, container, false);
        vista=binding.getRoot();
        binding.setTutorialView2((TutorialViewModel) tutorialViewModel);
        return vista;
    }



    public interface OnFragmentInteractionListener {
        void onFragmentInteraction(Uri uri);
    }
}
