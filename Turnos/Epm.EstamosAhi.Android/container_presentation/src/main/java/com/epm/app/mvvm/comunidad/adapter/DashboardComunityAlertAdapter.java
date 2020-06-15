package com.epm.app.mvvm.comunidad.adapter;

import android.content.Context;
import androidx.databinding.DataBindingUtil;
import androidx.databinding.ViewDataBinding;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.epm.app.R;
import com.epm.app.databinding.ItemInformationOfInterestBinding;
import com.epm.app.databinding.ItemUpdateSubscriptionBinding;
import com.epm.app.mvvm.comunidad.viewModel.ItemsRecyclerViewModel;

import java.util.List;

public class DashboardComunityAlertAdapter extends RecyclerView.Adapter<DashboardComunityAlertAdapter.CustomViewHolder> {

    private Context context;
    private final int SUBSCRIPTION = 0;
    private final int INFORMATION = 1;
    private final int layoutSubscription = R.layout.item_update_subscription;
    private final int layoutInformation = R.layout.item_information_of_interest;
    private OnDashboardComunityAlertListener onDashboardComunityAlertListener;
    private List<ItemsRecyclerViewModel> items;


    public DashboardComunityAlertAdapter(Context context,List<ItemsRecyclerViewModel> items,OnDashboardComunityAlertListener onDashboardComunityAlertListener) {
        this.context = context;
        this.onDashboardComunityAlertListener = onDashboardComunityAlertListener;
        this.items = items;
    }


    @NonNull
    @Override
    public CustomViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater layoutInflater = LayoutInflater.from(parent.getContext());
        ViewDataBinding binding;
        ItemUpdateSubscriptionBinding bindingUpdate;
        ItemInformationOfInterestBinding bindinInformation;
        switch (viewType){
            case layoutSubscription:
                bindingUpdate = DataBindingUtil.inflate(layoutInflater, viewType,parent,false);
                return new CustomViewHolder(bindingUpdate,onDashboardComunityAlertListener);

            case layoutInformation:
                bindinInformation = DataBindingUtil.inflate(layoutInflater, viewType,parent,false);
                return new CustomViewHolder(bindinInformation,onDashboardComunityAlertListener);
                default:
                    bindingUpdate = DataBindingUtil.inflate(layoutInflater, viewType,parent,false);
                    return new CustomViewHolder(bindingUpdate,onDashboardComunityAlertListener);

        }


    }

    @Override
    public void onBindViewHolder(@NonNull CustomViewHolder viewHolder, int position) {
        switch (viewHolder.getItemViewType()){
            case layoutSubscription:
                ItemUpdateSubscriptionBinding itemUpdateSubscriptionBinding = viewHolder.bindingSubscription;
                itemUpdateSubscriptionBinding.setItemViewmodel(items.get(position));
                itemUpdateSubscriptionBinding.cardUpdate.setEnabled(true);
                itemUpdateSubscriptionBinding.cardUpdate.setOnClickListener(v -> {
                    v.setEnabled(false);
                    onDashboardComunityAlertListener.onItemClick(position);
                });
                break;
            case layoutInformation:
                ItemInformationOfInterestBinding itemInformationOfInterestBinding  = viewHolder.bindingInformation;
                itemInformationOfInterestBinding.setItemViewmodel(items.get(position));
                itemInformationOfInterestBinding.cardInformation.setEnabled(true);
                itemInformationOfInterestBinding.cardInformation.setOnClickListener(v ->{
                    v.setEnabled(false);
                    onDashboardComunityAlertListener.onItemClick(position);
                });
                break;
        }
    }

    @Override
    public int getItemCount() {
        return items.size();
    }

    @Override
    public int getItemViewType(int position) {
        return getLayoutIdForPosition(position);
    }

    private int getLayoutIdForPosition(int position) {
        return (position == SUBSCRIPTION)?layoutSubscription:layoutInformation;

    }


    class CustomViewHolder extends RecyclerView.ViewHolder{

        private OnDashboardComunityAlertListener onDashboardComunityAlertListener;

        private ItemInformationOfInterestBinding bindingInformation;
        private ItemUpdateSubscriptionBinding bindingSubscription;
        public CustomViewHolder(@NonNull ItemInformationOfInterestBinding binding,OnDashboardComunityAlertListener onDashboardComunityAlertListener) {
            super(binding.getRoot());
            this.bindingInformation = binding;
            this.onDashboardComunityAlertListener = onDashboardComunityAlertListener;
        }

        public CustomViewHolder(@NonNull ItemUpdateSubscriptionBinding bindingUpdate, OnDashboardComunityAlertListener onDashboardComunityAlertListener) {
            super(bindingUpdate.getRoot());
            this.bindingSubscription = bindingUpdate;
            this.onDashboardComunityAlertListener = onDashboardComunityAlertListener;
        }


    }

    public interface OnDashboardComunityAlertListener{
        void onItemClick(int position);
    }
}
