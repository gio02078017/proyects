using Android.Support.V7.Widget;
using System;

namespace GrupoExito.Android.Widgets
{
    public class CustomRecyclerScroll : RecyclerView.OnScrollListener
    {

        private int visibleThreshold = 5;
        private int currentPage = 0;
        private readonly int startingPageIndex = 0;
        public delegate void LoadMoreEventHandler(object sender, EventArgs e);
        public event LoadMoreEventHandler LoadMoreEvent;
        public RecyclerView.LayoutManager MLayoutManager { get; set; }

        public CustomRecyclerScroll()
        {
        }

        public CustomRecyclerScroll(LinearLayoutManager layoutManager)
        {
            this.MLayoutManager = layoutManager;
        }

        public CustomRecyclerScroll(GridLayoutManager layoutManager)
        {
            this.MLayoutManager = layoutManager;
            visibleThreshold = visibleThreshold * layoutManager.SpanCount;
        }

        public CustomRecyclerScroll(StaggeredGridLayoutManager layoutManager)
        {
            this.MLayoutManager = layoutManager;
            visibleThreshold = visibleThreshold * layoutManager.SpanCount;
        }

        public int GetLastVisibleItem(int[] lastVisibleItemPositions)
        {
            int maxSize = 0;

            for (int i = 0; i < lastVisibleItemPositions.Length; i++)
            {
                if (i == 0)
                {
                    maxSize = lastVisibleItemPositions[i];
                }
                else if (lastVisibleItemPositions[i] > maxSize)
                {
                    maxSize = lastVisibleItemPositions[i];
                }
            }

            return maxSize;
        }

        public override void OnScrolled(RecyclerView view, int dx, int dy)
        {
            base.OnScrolled(view, dx, dy);

            if (dy > 0)
            {
                visibleThreshold = 2;

                int lastItem = ((GridLayoutManager)this.MLayoutManager).FindLastCompletelyVisibleItemPosition();
                int currentTotalCount = this.MLayoutManager.ItemCount;

                if (currentTotalCount <= lastItem + visibleThreshold)
                {
                    LoadMoreEvent(this, null);
                }
            }
        }

        public void ResetState()
        {
            this.currentPage = this.startingPageIndex;
        }
    }
}