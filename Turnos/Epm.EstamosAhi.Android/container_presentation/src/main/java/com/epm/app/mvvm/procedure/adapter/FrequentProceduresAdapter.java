package com.epm.app.mvvm.procedure.adapter;

import android.content.Context;
import androidx.databinding.DataBindingUtil;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.epm.app.R;
import com.epm.app.databinding.ItemFrequentProcedureBinding;
import com.epm.app.mvvm.procedure.network.response.Procedure;
import com.epm.app.mvvm.procedure.viewModel.ItemFrequentProcedureViewModel;

import java.util.List;

public class FrequentProceduresAdapter  extends RecyclerView.Adapter<FrequentProceduresAdapter.CustomViewHolder> {

    private List<Procedure> frequentProceduresList;
    private Context context;
    private  ItemFrequentProcedureBinding itemBinding;
    private FrequentProceduresAdapter.OnFrequentProceduresRecyclerListener onFrequentProceduresRecyclerListener;

    public FrequentProceduresAdapter( Context context) {
        this.context = context;
        onFrequentProceduresRecyclerListener = (OnFrequentProceduresRecyclerListener) context;
    }

    @NonNull
    @Override
    public FrequentProceduresAdapter.CustomViewHolder onCreateViewHolder(@NonNull ViewGroup viewGroup, int viewType) {
        LayoutInflater layoutInflater = LayoutInflater.from(viewGroup.getContext());
        itemBinding = DataBindingUtil.inflate(layoutInflater, viewType, viewGroup, false);
        return new CustomViewHolder(itemBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull FrequentProceduresAdapter.CustomViewHolder customViewHolder, int position) {
        ItemFrequentProcedureViewModel itemViewModel = new ItemFrequentProcedureViewModel();
        itemViewModel.setProcedure(frequentProceduresList.get(position));
        itemBinding.setItemFrequentProcedure(itemViewModel);
        itemViewModel.drawInformation();
        itemBinding.cardViewFrequentProcedure.setOnClickListener(v -> {
            if(frequentProceduresList.get(position).isActive())
             onFrequentProceduresRecyclerListener.onItemClick(position);
        });
    }

    @Override
    public int getItemViewType(int position) {
        return R.layout.item_frequent_procedure;
    }

    @Override
    public int getItemCount() {
        return frequentProceduresList.size();
    }

    public class CustomViewHolder extends RecyclerView.ViewHolder {

        public CustomViewHolder(@NonNull ItemFrequentProcedureBinding itemView) {
            super(itemView.getRoot());
        }
    }


    public void setFrequentProceduresList(List<Procedure> frequentProceduresList) {
        this.frequentProceduresList = frequentProceduresList;
    }

    public interface OnFrequentProceduresRecyclerListener {
        void onItemClick(int position);
    }
}
