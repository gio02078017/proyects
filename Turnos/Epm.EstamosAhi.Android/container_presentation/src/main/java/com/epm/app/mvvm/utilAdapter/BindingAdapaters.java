package com.epm.app.mvvm.utilAdapter;

import androidx.lifecycle.LifecycleOwner;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.Observer;
import androidx.databinding.BindingAdapter;
import androidx.databinding.BindingConversion;
import androidx.databinding.InverseBindingAdapter;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.drawable.Drawable;
import androidx.annotation.Nullable;
import androidx.cardview.widget.CardView;
import androidx.appcompat.widget.SwitchCompat;

import android.util.Log;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;

import java.io.IOException;
import java.net.URL;

import app.epm.com.utilities.controls.FactoryControls;

public  class BindingAdapaters {

    @BindingAdapter("mutableText")
    public static void Text(final TextView textView, final MutableLiveData<String> date) {
       date.observeForever(s -> {
           if(date.getValue() != null) {
               textView.setText(date.getValue());
           }
       });
    }


    @BindingAdapter("mutableEditText")
    public static void editText(final EditText editText, final MutableLiveData<String> date) {
             date.observeForever(s -> {
                 if (date.getValue() != null){
                     editText.setText(date.getValue());
                 }
             });
    }

    @BindingAdapter("mutableButtonEnable")
    public static void buttonEnable(final Button button,final MutableLiveData<Boolean> date) {
        date.observeForever(aBoolean -> {
            if(aBoolean != null) {
                button.setEnabled(aBoolean);
            }
        });
    }

    @BindingAdapter("mutableTextViewEnable")
    public static void TextViewEnable(final TextView textView,final MutableLiveData<Boolean> date) {
        date.observeForever(aBoolean -> {
            if(aBoolean != null){
                textView.setEnabled(date.getValue().booleanValue());
            }

        });
    }

    @BindingAdapter("mutableLinearLayoutVisibility")
    public static void LinearLayoutVisibility(LinearLayout linearLayout, MutableLiveData<Integer> date){
        date.observeForever(s -> {
            if(date != null) linearLayout.setVisibility(date.getValue());
        });
    }

    @BindingAdapter("mutableProgressVisibility")
    public static void progressVisibility(ProgressBar progressBar, MutableLiveData<Integer> date){
        date.observeForever(s -> {
            if(date != null) progressBar.setVisibility(date.getValue());
        });
    }

    @BindingAdapter("mutableLoadImage")
    public static void loadImage(ImageView imageView,MutableLiveData<Drawable> image){
            image.observeForever(images ->{
                if(image != null) {
                    imageView.setImageDrawable(images);
                }
            });
    }

    @BindingAdapter("mutableLoadImageURL")
    public static void setImageUrl(ImageView imageView, String url) {
        if (url != null) {
            try {
                URL urls = new URL(url);
                Bitmap bmp = BitmapFactory.decodeStream(urls.openConnection().getInputStream());
                imageView.setImageBitmap(bmp);
            } catch (IOException e) {
               Log.e("Error",e.getMessage());
            }

        }
    }

    @BindingAdapter("mutableColorTextView")
    public static void loadColorTextView(TextView textView,MutableLiveData<Integer> color){
        color.observeForever(colors -> {
            if(colors != null) {
                textView.setTextColor(colors);
            }
        });

    }

    @BindingAdapter("mutableColorCardView")
    public static void loadColorCardView(CardView cardView, MutableLiveData<Integer> color){
        color.observeForever(colors -> {
            if(colors != null){
                cardView.setBackgroundColor(colors);
            }
        });
    }

    @BindingAdapter("mutableEnabledSwitchCompat")
    public static void loadEnableSwitch(SwitchCompat swicth, MutableLiveData<Boolean> enabled){
        enabled.observeForever(aBoolean -> {
            if(aBoolean != null){
               swicth.setEnabled(aBoolean);
            }
        });
    }

    @BindingAdapter("mutableTextViewVisibility")
    public static void TextViewVisibility(final TextView textView,final MutableLiveData<Integer> date) {
        date.observeForever(aBoolean -> {
            if(aBoolean != null){
                textView.setVisibility(date.getValue());
            }

        });
    }

    @BindingAdapter("mutableTextTypeFace")
    public static void setTypeface(TextView v, String style) {
        switch (style) {
            case "bold":
                v.setTypeface(FactoryControls.getTypefaceBold());
                break;
            case "normal":
                v.setTypeface(FactoryControls.getTypefaceNormal());
                break;
            default:
                v.setTypeface(FactoryControls.getTypefaceNormal());
                break;
        }
    }
}
