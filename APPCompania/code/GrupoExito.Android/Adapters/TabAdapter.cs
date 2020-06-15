using Android.Support.V4.App;
using System;
using System.Collections.Generic;

namespace GrupoExito.Android.Adapters
{
    class TabAdapter : FragmentStatePagerAdapter
    {
        private List<Fragment> Fragments = new List<Fragment>();
        private List<String> fragmentTitleList;

        public List<string> FragmentTitleList
        {
            get
            {
                if (fragmentTitleList == null)
                {
                    fragmentTitleList = new List<string>();
                }

                return fragmentTitleList;
            }
            set { fragmentTitleList = value; }
        }

        public TabAdapter(FragmentManager fm) : base(fm)
        {
        }

        public override int Count
        {
            get { return Fragments != null ? Fragments.Count : 0; }
        }

        public override Fragment GetItem(int position)
        {
            return Fragments[position];
        }

        public void AddFragment(Fragment fragment, String title)
        {
            Fragments.Add(fragment);
            FragmentTitleList.Add(title);
        }
    }
}