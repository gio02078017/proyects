package com.epm.app.mvvm.procedure.views.fragment;

import androidx.lifecycle.ViewModelProviders;
import android.content.Context;
import androidx.databinding.DataBindingUtil;

import android.os.Bundle;
import androidx.annotation.Nullable;
import com.google.android.material.bottomsheet.BottomSheetDialogFragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.epm.app.R;
import com.epm.app.databinding.FragmentFormalitiesCareChannelsBinding;
import com.epm.app.mvvm.procedure.network.response.DetailOfTheTransactionResponse;
import com.epm.app.mvvm.procedure.viewModel.FormalitiesCareChannelsViewModel;
import com.epm.app.mvvm.procedure.viewModel.iViewModel.IFormalitiesCareChannelsViewModel;

public class FormalitiesCareChannelsFragment extends BottomSheetDialogFragment {

    FragmentFormalitiesCareChannelsBinding binding;
    static IFormalitiesCareChannelsFragmentListener listener;
    static IFormalitiesCareChannelsViewModel formalitiesCareChannelsViewModel;
    static DetailOfTheTransactionResponse detailOfTheTransactionResponse;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        formalitiesCareChannelsViewModel = (IFormalitiesCareChannelsViewModel) ViewModelProviders.of(this).get(FormalitiesCareChannelsViewModel.class);
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                              Bundle savedInstanceState) {
        binding = DataBindingUtil.inflate(inflater,R.layout.fragment_formalities_care_channels, container, false);
        startListener();
        loadObservable();
        formalitiesCareChannelsViewModel.loadFormalities(detailOfTheTransactionResponse);
        return binding.getRoot();
    }

    public void loadService(DetailOfTheTransactionResponse detailOfTheTransactionResponse){
       FormalitiesCareChannelsFragment.detailOfTheTransactionResponse = detailOfTheTransactionResponse;
    }

    private void loadObservable(){
        formalitiesCareChannelsViewModel.getLineAttentionVisibility().observe( getActivity(), visible -> {
            if(visible != null){
                binding.bottomLineAttention.setVisibility(visible);
            }

        });
        formalitiesCareChannelsViewModel.getVisitVisibility().observe(getActivity(), visible -> {
            if(visible != null){
                binding.bottomVisit.setVisibility(visible);
            }
        });
    }

    private void startListener(){
        binding.bottomLineAttention.setOnClickListener(view1 -> {
            listener.onChoseLineAttentionClicked();
            dismiss();
        });
        binding.bottomVisit.setOnClickListener(view12 -> {
            listener.onChoseVisitClicked();
            dismiss();
        });
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        try{
            listener = (IFormalitiesCareChannelsFragmentListener)context;
        }catch(ClassCastException e){
            throw new ClassCastException(context.toString() + "must implement BottomSheetListener");
        }
    }

    @Override
    public void onDetach() {
        listener = null;
        super.onDetach();
    }

    public interface IFormalitiesCareChannelsFragmentListener {
        void onChoseLineAttentionClicked();
        void onChoseVisitClicked();
    }

}
