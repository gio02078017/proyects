package com.epm.app.mvvm.comunidad.views.fragments;


import androidx.lifecycle.LifecycleOwner;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;
import android.content.Context;
import android.content.res.ColorStateList;
import androidx.databinding.DataBindingUtil;
import android.graphics.Color;
import android.os.Build;
import android.os.Bundle;
import androidx.annotation.Nullable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.epm.app.R;
import com.epm.app.databinding.FragmentConfigurationBinding;
import com.epm.app.mvvm.comunidad.viewModel.ConfigurationNotificationViewModel;

import javax.inject.Inject;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.fragments.BaseFragmentDi;

public class ConfigurationFragment extends BaseFragmentDi {


    @Inject
    ViewModelProvider.Factory viewModelFactory;

    static ConfigurationNotificationViewModel configurationNotificationViewModel;
    static FragmentConfigurationBinding binding;
    static Context context;
    static OnConfigurationRecyclerListener onConfigurationRecyclerListener;


    private void backPressed(){
        binding.arrowBack.setOnClickListener(v -> {
            onConfigurationRecyclerListener.onItemBack();
        });
    }

    private void loadColors() {
        int[][] states = new int[][] {
                new int[] {-android.R.attr.state_enabled}, // disabled
                new int[] {-android.R.attr.state_checked}, // unchecked
                new int[] { android.R.attr.state_checked}  // pressed
        };

        int[] colors = new int[] {
                Color.parseColor(Constants.SWITCH_COLOR_DISABLED),
                Color.parseColor(Constants.SWITCH_COLOR_OFF),
                Color.WHITE
        };

        ColorStateList myList = new ColorStateList(states, colors);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            binding.switchAlerts.setThumbTintList(myList);
            binding.switchCustomerService.setThumbTintList(myList);
        }

    }

    public void tryAgain(){
        configurationNotificationViewModel.verifyNotification();
    }

    private void loadObservable(){
        configurationNotificationViewModel.getCheckedAlerts().observe((LifecycleOwner) context, checked -> {
            if(checked != null){
                binding.switchAlerts.setChecked(checked);
            }
        });
        configurationNotificationViewModel.getCheckedService().observe((LifecycleOwner) context, checked -> {
            if(checked != null){
                binding.switchCustomerService.setChecked(checked);
            }
        });
        configurationNotificationViewModel.getIsError().observe((LifecycleOwner) context, errorMessage -> onConfigurationRecyclerListener.showError(errorMessage.getTitle(),errorMessage.getMessage()));
        configurationNotificationViewModel.getEnabledAlerts().observe((LifecycleOwner) context, enabled -> {
            if(enabled != null){
                binding.switchAlerts.setEnabled(enabled);
            }
        });
        configurationNotificationViewModel.getEnabledService().observe((LifecycleOwner) context, enabled -> {
            if(enabled != null){
                binding.switchCustomerService.setEnabled(enabled);
            }
        });

    }

    private void loadClicks(){
        binding.switchAlerts.setOnCheckedChangeListener((buttonView, isChecked) -> {
            configurationNotificationViewModel.getCheckedAlerts().setValue(isChecked);
            configurationNotificationViewModel.updateStatusNotification(Constants.TYPE_SUSCRIPTION_ALERTAS,isChecked);
        });
        binding.switchCustomerService.setOnCheckedChangeListener((buttonView, isChecked) -> {
            configurationNotificationViewModel.getCheckedService().setValue(isChecked);
            configurationNotificationViewModel.updateStatusNotification(Constants.TYPE_SUBSCRIPTION_CUSTOMER_SERVICE,isChecked);
        });
    }


    public static ConfigurationFragment newInstance(ConfigurationNotificationViewModel configurationNotificationView,OnConfigurationRecyclerListener onConfigurationRecyclerListene,
                                                    Context contexto) {
        Bundle args = new Bundle();
        configurationNotificationViewModel = configurationNotificationView;
        ConfigurationFragment fragment = new ConfigurationFragment();
        onConfigurationRecyclerListener = onConfigurationRecyclerListene;
        context = contexto;
        configurationNotificationViewModel.verifyNotification();
        fragment.loadClicks();
        fragment.loadObservable();
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        binding = DataBindingUtil.inflate(inflater,R.layout.fragment_configuration, container, false);
        View view = binding.getRoot();
        loadColors();
        backPressed();
        return view;
    }

    public interface OnConfigurationRecyclerListener {
        void onItemBack();
        void showError(int titleError, int error);
        void showErrorUnauthorized(int titleError, int error);
    }



}
