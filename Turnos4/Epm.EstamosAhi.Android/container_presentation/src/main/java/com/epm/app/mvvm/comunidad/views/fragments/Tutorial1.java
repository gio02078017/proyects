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
import com.epm.app.databinding.FragmentTutorial1Binding;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.ITutorialViewModel;
import com.epm.app.mvvm.comunidad.viewModel.TutorialViewModel;

import app.epm.com.utilities.view.fragments.BaseFragment;


public class Tutorial1 extends BaseFragment {

    Button btnSaltarIntro;
    View vista;
    //@Named("Tutorial")@Inject
    ITutorialViewModel tutorialViewModel;
    private OnFragmentInteractionListener mListener;


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        tutorialViewModel = ViewModelProviders.of(getActivity()).get(TutorialViewModel.class);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        FragmentTutorial1Binding binding = DataBindingUtil.inflate(inflater,R.layout.fragment_tutorial1, container, false);
        vista=binding.getRoot();
        binding.setTutorialView((TutorialViewModel) tutorialViewModel);
        return vista;
    }



    public interface OnFragmentInteractionListener {
        void onFragmentInteraction(Uri uri);
    }
}
