package com.epm.app.mvvm.procedure.adapter;

import android.view.View;
import android.widget.TextView;

import com.epm.app.R;

class DetailsOfFormalitiesViewHolder extends ChildViewHolder {

    private TextView mMoviesTextView;

    public DetailsOfFormalitiesViewHolder(View itemView) {
        super(itemView);
        mMoviesTextView = itemView.findViewById(R.id.textDescriptionItem);

    }

    public void bind(String description) {
        mMoviesTextView.setText(description);
    }
}
