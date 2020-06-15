package com.epm.app.mvvm.turn.viewModel;

import android.arch.lifecycle.MutableLiveData;
import android.arch.lifecycle.ViewModel;
import android.content.res.Resources;
import android.graphics.drawable.Drawable;

import com.epm.app.mvvm.turn.models.ChannelsOfAttentionMenu;
import com.epm.app.mvvm.turn.models.ChannelsOfAttentionMenuItem;
import com.epm.app.mvvm.turn.repository.JsonStringRepository;
import app.epm.com.utilities.utils.ConvertUtilities;

import javax.inject.Inject;

import app.epm.com.utilities.utils.Constants;

public class ChannelsOfAttentionViewModel extends ViewModel {

    private MutableLiveData<ChannelsOfAttentionMenu>  listChannelsOfAttention;
    public final MutableLiveData<String>  textItemChannelsOfAttentionDescription;
    public final MutableLiveData<String> btnAccionChannelsOfAttention;
    public final MutableLiveData<Drawable> imageItemChannelsOfAttention;
    private Resources resources;
    private ChannelsOfAttentionMenuItem channelsOfAttentionMenuItem;

    @Inject
    public JsonStringRepository jsonStringRepository;

    @Inject
    public ChannelsOfAttentionViewModel(Resources resources){
        this.textItemChannelsOfAttentionDescription = new MutableLiveData<>();
        this.btnAccionChannelsOfAttention = new MutableLiveData<>();
        this.imageItemChannelsOfAttention = new MutableLiveData<>();
        this.listChannelsOfAttention = new MutableLiveData<>();
        this.resources = resources;
    }

    public void getChannelsOfAttentionMenu(){
        if(jsonStringRepository.getDataChannelsOfAttentionMenu() != null && jsonStringRepository.getDataChannelsOfAttentionMenu().getChannelsOfAttentionMenuItems().size() > 0 ) {
            listChannelsOfAttention.setValue(jsonStringRepository.getDataChannelsOfAttentionMenu());
        }
    }

    public ChannelsOfAttentionMenuItem getChannelsOfAttentionMenuItem() {
        return channelsOfAttentionMenuItem;
    }

    public void setChannelsOfAttentionMenuItem(ChannelsOfAttentionMenuItem channelsOfAttentionMenuItem) {
        this.channelsOfAttentionMenuItem = channelsOfAttentionMenuItem;
    }

    public void drawInformation(){
        this.textItemChannelsOfAttentionDescription.setValue(this.channelsOfAttentionMenuItem.getTextItemChannelsOfAttentionDescription());
        this.btnAccionChannelsOfAttention.setValue(this.channelsOfAttentionMenuItem.getTextButtonChannelsOfAttention());
        this.imageItemChannelsOfAttention.setValue(resources.getDrawable(ConvertUtilities.resourceId(Constants.TYPE_RESOURCE_DRAWABLE,channelsOfAttentionMenuItem.getImageItemChannelsOfAttention())));
    }

    public MutableLiveData<ChannelsOfAttentionMenu> getListChannelsOfAttention() {
        return listChannelsOfAttention;
    }

    public void setListChannelsOfAttention(MutableLiveData<ChannelsOfAttentionMenu> listChannelsOfAttention) {
        this.listChannelsOfAttention = listChannelsOfAttention;
    }
}
