package com.epm.app.mvvm.utilAdapter;

import android.arch.lifecycle.LifecycleOwner;
import android.arch.lifecycle.MutableLiveData;
import android.arch.lifecycle.Observer;
import android.content.Context;
import android.content.DialogInterface;
import android.content.res.ColorStateList;
import android.databinding.BindingAdapter;
import android.databinding.BindingConversion;
import android.databinding.InverseBindingAdapter;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.ColorDrawable;
import android.graphics.drawable.Drawable;
import android.support.annotation.Nullable;
import android.support.v7.widget.CardView;
import android.support.v7.widget.SwitchCompat;
import android.text.Editable;
import android.text.TextWatcher;
import android.text.style.BackgroundColorSpan;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.Switch;
import android.widget.TextView;

import com.epm.app.R;

import java.io.IOException;
import java.net.URL;

import app.epm.com.utilities.controls.FactoryControls;
import app.epm.com.utilities.custom_controls.CustomAlertDialog;

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
