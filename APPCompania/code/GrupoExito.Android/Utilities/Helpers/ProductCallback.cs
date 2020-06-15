using Android.Support.V7.Util;
using GrupoExito.Entities;
using System.Collections.Generic;

namespace GrupoExito.Android.Utilities
{
    public class ProductCallback : DiffUtil.Callback
    {
        private List<Product> oldList;
        private List<Product> newList;

        public ProductCallback(List<Product> oldList, List<Product> newList)
        {
            this.oldList = oldList;
            this.newList = newList;
        }

        public override int OldListSize => oldList.Count;

        public override int NewListSize => newList.Count;

        public override bool AreItemsTheSame(int oldItemPosition, int newItemPosition)
        {
            return oldList[oldItemPosition].Id == newList[newItemPosition].Id;
        }

        public override bool AreContentsTheSame(int oldItemPosition, int newItemPosition)
        {
            return oldList[oldItemPosition].Equals(newList[newItemPosition]);
        }

        public override Java.Lang.Object GetChangePayload(int oldItemPosition, int newItemPosition)
        {
            return base.GetChangePayload(oldItemPosition, newItemPosition);
        }
    }
}