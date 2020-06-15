package com.epm.app.mvvm.turn.repository;

import android.app.Application;
import android.content.Context;

import com.epm.app.mvvm.turn.models.ChannelsOfAttentionMenu;
import com.epm.app.mvvm.turn.models.CustomerServiceMenu;
import com.epm.app.mvvm.util.CallChannelsOfAttentionMenu;
import com.epm.app.mvvm.util.CallCustomerMenu;
import com.epm.app.mvvm.util.CallFileResourceRaw;
import com.google.gson.Gson;

import javax.inject.Inject;

public class JsonStringRepository {

    public CustomerServiceMenu listCustomerServiceMenu;
    public CallCustomerMenu callCustomerMenu;
    public CallChannelsOfAttentionMenu callChannelsOfAttentionMenu;
    private Context context;

    @Inject
    public JsonStringRepository(Context context) {
        this.context = context;
    }


    public CustomerServiceMenu getDataMenu(){

        Gson gson = new Gson();
        callCustomerMenu = new CallCustomerMenu(context);
        listCustomerServiceMenu = gson.fromJson(callCustomerMenu.readFile(), CustomerServiceMenu.class);
        return listCustomerServiceMenu;

    }

    public ChannelsOfAttentionMenu getDataChannelsOfAttentionMenu(){

        Gson gson = new Gson();
        callChannelsOfAttentionMenu = new CallChannelsOfAttentionMenu(context);
        ChannelsOfAttentionMenu listChannelsOfAttentionMenu = gson.fromJson(callChannelsOfAttentionMenu.readFile(), ChannelsOfAttentionMenu.class);
        return listChannelsOfAttentionMenu;
    }
}
