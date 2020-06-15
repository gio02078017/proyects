package com.epm.app.app_utilities_presentation.views.adapter;


import android.content.Context;
import androidx.recyclerview.widget.RecyclerView;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.epm.app.app_utilities_presentation.R;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.utilities.helpers.FileManager;
import app.epm.com.utilities.helpers.IFileManager;
import app.epm.com.utilities.utils.Constants;

public abstract class RecyclerViewAttachImageAdapter extends RecyclerView.Adapter<RecyclerViewAttachImageAdapter.CustomViewHolder> {
    private List<String> listFilesName;
    private final Context context;
    private final IFileManager fileManager;
    private int widthLayout, heightLayout;
    private String tempNameImage = "";

    public RecyclerViewAttachImageAdapter(Context context) {
        this.context = context;
        this.fileManager = new FileManager(context);
    }

    public abstract void setCount();

    public void setFiles(ArrayList<String> arrayFiles) {
        this.listFilesName = arrayFiles;
    }

    public void setSize(int width, int height) {
        this.widthLayout = width;
        this.heightLayout = (int)(height * 0.74);
    }

    public class CustomViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener {
        final ImageView deleteImage, attachImageAudio;
        final TextView attach_tamanio;

        public CustomViewHolder(View itemView) {
            super(itemView);
            deleteImage =  itemView.findViewById(R.id.attach_delete);
            attachImageAudio =  itemView.findViewById(R.id.attach_image_audio);
            attach_tamanio =  itemView.findViewById(R.id.attach_tamanio);
            deleteImage.setOnClickListener(this);
        }

        @Override
        public void onClick(View v) {
            try {
                if (v.equals(deleteImage)) {
                    listFilesName.remove(getAdapterPosition());
                    notifyItemRemoved(getAdapterPosition());
                    notifyItemRangeChanged(getAdapterPosition(), getItemCount());
                    setCount();
                }
            }catch(ArrayIndexOutOfBoundsException e){
                Log.e("Error Exception", e.getMessage());
            }
        }
    }

    @Override
    public CustomViewHolder onCreateViewHolder(ViewGroup viewGroup, int viewType) {
        View view = LayoutInflater.from(viewGroup.getContext()).inflate(R.layout.item_attach_image, viewGroup, false);
        return new CustomViewHolder(view);
    }

    @Override
    public void onBindViewHolder(final CustomViewHolder holder, int position) {
        String fileName = listFilesName.get(position);
        if (fileName.contains(Constants.PREFIX_FILE_AUDIO)) {
            Glide.with(context).load(R.mipmap.icon_file_audio_attach)
                    .override(widthLayout, heightLayout)
                    .into(holder.attachImageAudio);
        } else {
            Glide.with(context).load(fileName)
                    .override(widthLayout, heightLayout)
                    .into(holder.attachImageAudio);
        }

        holder.deleteImage.setVisibility(View.VISIBLE);
        holder.attach_tamanio.setText(fileManager.getSizeFile(fileName));
    }

    @Override
    public int getItemCount() {
        return listFilesName.size();
    }
}
