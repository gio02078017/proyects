package com.epm.app.mvvm.comunidad.adapter;

import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.RectF;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import androidx.recyclerview.widget.ItemTouchHelper;
import android.view.View;


import com.epm.app.R;
import com.epm.app.mvvm.comunidad.views.activities.NotificationCenterActivity;

import app.epm.com.utilities.helpers.BaseMapper;

public class swipeHelperTest extends ItemTouchHelper.SimpleCallback {

    private NotificationsRecyclerAdapter notification;
    private Paint p = new Paint();
    private NotificationCenterActivity context;


    public swipeHelperTest(int dragDirs, int swipeDirs, NotificationsRecyclerAdapter notificationsRecyclerAdapter, NotificationCenterActivity context) {
        super(dragDirs, swipeDirs);
        this.notification = notificationsRecyclerAdapter;
        this.context = context;
    }

    @Override
    public float getSwipeThreshold(RecyclerView.ViewHolder viewHolder) {
        return ItemTouchHelper.LEFT;
    }

    @Override
    public void onSwiped(@NonNull RecyclerView.ViewHolder viewHolder, int i) {

    }

    @Override
    public boolean onMove(@NonNull RecyclerView recyclerView, @NonNull RecyclerView.ViewHolder viewHolder, @NonNull RecyclerView.ViewHolder viewHolder1) {
        return false;
    }


    @Override
    public void onChildDraw(@NonNull Canvas c, @NonNull RecyclerView recyclerView, @NonNull RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, boolean isCurrentlyActive) {
        if (actionState == ItemTouchHelper.ACTION_STATE_SWIPE) {
            View itemView = viewHolder.itemView;
            Bitmap icon;
            float height = (float) itemView.getBottom() - (float) itemView.getTop();

            if(dX <0) {
                p.setColor(Color.parseColor("#FF0000"));
                RectF background = new RectF((float) itemView.getRight() + dX, (float) itemView.getTop(), (float) itemView.getRight(), (float) itemView.getBottom());
                c.drawRect(background, p);
                icon = BaseMapper.getBitmapFromVectorDrawable(context, R.drawable.ic_delete);

                float iconWidth = icon.getWidth();
                float iconHeight = icon.getHeight();

                float rightPosition = itemView.getRight() - iconWidth;
                float leftPosition = rightPosition - iconWidth;
                float topPosition = itemView.getTop() + ((height - iconHeight) / 2);
                float bottomPosition = topPosition + iconHeight;

                RectF iconDest = new RectF(leftPosition, topPosition, rightPosition, bottomPosition);
                c.drawBitmap(icon, null, iconDest, p);

            }

        }
        super.onChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
    }


    public interface deleteNotfication{
        void onClick(int pos);
    }
}
