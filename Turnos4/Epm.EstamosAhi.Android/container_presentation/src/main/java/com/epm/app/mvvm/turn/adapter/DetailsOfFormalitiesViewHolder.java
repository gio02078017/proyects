package com.epm.app.mvvm.turn.adapter;

import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import com.epm.app.R;
import com.epm.app.mvvm.turn.models.DetailOfFormalities;

class DetailsOfFormalitiesViewHolder extends ChildViewHolder {

    private TextView mMoviesTextView;

    public DetailsOfFormalitiesViewHolder(View itemView) {
        super(itemView);
        mMoviesTextView = (TextView) itemView.findViewById(R.id.textDescriptionItem);

    }

    public void bind(DetailOfFormalities movies) {
        mMoviesTextView.setText(movies.getName());
    }
}
