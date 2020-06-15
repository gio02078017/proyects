package co.gov.ins.guardianes.presentation.view.welcome

import android.os.Bundle
import androidx.appcompat.widget.Toolbar
import androidx.viewpager.widget.ViewPager
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.view.base.BaseFragmentActivity
import kotlinx.android.synthetic.main.welcome.*


class WelcomeActivity : BaseFragmentActivity() {
    private var welcomePagerAdapter: WelcomePagerAdapter? = null
    override fun onCreate(bundle: Bundle?) {
        super.onCreate(bundle)
        setContentView(R.layout.welcome)
        welcomePagerAdapter =
            WelcomePagerAdapter(supportFragmentManager, this, Welcome.values())
        view_pager.adapter = welcomePagerAdapter
        page_indicator.setViewPager(view_pager)
        view_pager.addOnPageChangeListener(object : ViewPager.OnPageChangeListener {
            override fun onPageScrolled(
                position: Int,
                positionOffset: Float,
                positionOffsetPixels: Int
            ) {
                if (position == 0) {
                    val fragment =
                        welcomePagerAdapter!!.instantiateItem(view_pager, 0) as WelcomePageFragment
                    fragment.getHtml1()
                }
                if (position == 1) {
                    val fragment =
                        welcomePagerAdapter!!.instantiateItem(view_pager, 1) as WelcomePageFragment
                    fragment.getHtml2()
                }
                if (position == 2) {
                    val fragment =
                        welcomePagerAdapter!!.instantiateItem(view_pager, 2) as WelcomePageFragment
                    fragment.getHtml3()
                }
                if (position == 3) {
                    val fragment =
                            welcomePagerAdapter!!.instantiateItem(view_pager, 3) as WelcomePageFragment
                    fragment.getHtml4()
                }
            }

            override fun onPageSelected(position: Int) {
                if (position == 3) {
                    val fragment =
                        welcomePagerAdapter!!.instantiateItem(view_pager, 3) as WelcomePageFragment
                    fragment.bnHome()
                }
            }

            override fun onPageScrollStateChanged(state: Int) {}
        })
    }

    public override fun onResume() {
        super.onResume()
    }

    override fun onToolbar(toolbar: Toolbar) {}
}