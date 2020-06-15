package com.epm.app.mvvm.turn.models;

import java.util.List;

public interface ParentListItem {


    List<?> getChildItemList();
    boolean isInitiallyExpanded();

}
