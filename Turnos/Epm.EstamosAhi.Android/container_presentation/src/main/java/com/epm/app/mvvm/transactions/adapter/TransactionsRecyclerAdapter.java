package com.epm.app.mvvm.transactions.adapter;

import android.content.Context;
import android.content.res.Resources;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.RecyclerView;

import com.epm.app.R;
import com.epm.app.databinding.ItemFastTransitionBinding;
import com.epm.app.mvvm.transactions.models.Transaction;
import com.epm.app.mvvm.transactions.viewModel.ItemFastTransitionViewModel;

import java.util.List;

public class TransactionsRecyclerAdapter  extends RecyclerView.Adapter<TransactionsRecyclerAdapter.CustomViewHolder> {

    private List<Transaction> fastTransactions;
    private Context context;
    private ItemFastTransitionBinding itemBinding;
    private TransactionsRecyclerAdapter.OnFastTransitionRecyclerListener onFastTransitionRecyclerListener;
    private Resources resources;

    public TransactionsRecyclerAdapter( Context context, Resources resources) {
        this.context = context;
        onFastTransitionRecyclerListener = (OnFastTransitionRecyclerListener) context;
        this.resources =  resources;
    }
    @NonNull
    @Override
    public TransactionsRecyclerAdapter.CustomViewHolder onCreateViewHolder(@NonNull ViewGroup viewGroup, int viewType) {
        LayoutInflater layoutInflater = LayoutInflater.from(viewGroup.getContext());
        itemBinding = DataBindingUtil.inflate(layoutInflater, viewType, viewGroup, false);
        return new CustomViewHolder(itemBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull TransactionsRecyclerAdapter.CustomViewHolder holder, int position) {
        ItemFastTransitionViewModel itemViewModel = new ItemFastTransitionViewModel();
        itemViewModel.setResources(resources);
        itemViewModel.setTransation(fastTransactions.get(position));
        itemBinding.setItemViewModel(itemViewModel);
        itemViewModel.drawInformation(position);
        itemBinding.cardViewFastTransaction.setOnClickListener(v -> {
            if(fastTransactions.get(position).isActive())
                onFastTransitionRecyclerListener.onItemClick(position);
        });

    }

    public void setFastTransactions(List<Transaction> fastTransactions) {
        this.fastTransactions = fastTransactions;
    }

    @Override
    public int getItemViewType(int position) {
        return R.layout.item_fast_transition;
    }

    @Override
    public int getItemCount() {
        return fastTransactions.size();
    }

    public class CustomViewHolder extends RecyclerView.ViewHolder {
        public CustomViewHolder(@NonNull ItemFastTransitionBinding itemView) {
            super(itemView.getRoot());
        }
    }

    public interface OnFastTransitionRecyclerListener {
        void onItemClick(int position);
    }
}
