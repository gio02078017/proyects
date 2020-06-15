using Android.Support.V7.Widget;
using GrupoExito.Android.Activities;
using GrupoExito.Android.Activities.Generic;
using System;

namespace GrupoExito.Android.Utilities
{
    public class CustomScrollListener : RecyclerView.OnScrollListener
    {
        public delegate void LoadMoreEventHandler(object sender, EventArgs e);
        public event LoadMoreEventHandler LoadMoreEvent;
        private GridLayoutManager LayoutManager;

        public CustomScrollListener(GridLayoutManager layoutManager)
        {
            LayoutManager = layoutManager;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            AndroidApplication.CurrentActivity.RunOnUiThread(() =>
            {
                var visibleItemCount = recyclerView.ChildCount;
                var totalItemCount = recyclerView.GetAdapter().ItemCount;
                var pastVisiblesItems = LayoutManager.FindFirstVisibleItemPosition();
                var pos = LayoutManager.FindLastCompletelyVisibleItemPosition();

                if (pos >= totalItemCount)
                {
                    LoadMoreEvent(this, null);

                }else if(pos +1 == totalItemCount)
                {
                    LoadMoreEvent(this, null);
                }

            });    
        }
    }
}