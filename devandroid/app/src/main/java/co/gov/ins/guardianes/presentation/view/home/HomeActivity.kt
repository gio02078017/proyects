package co.gov.ins.guardianes.presentation.view.home

import android.content.Intent
import android.os.Bundle
import android.view.Gravity
import android.view.MenuItem
import android.view.View
import android.widget.AdapterView
import android.widget.AdapterView.OnItemClickListener
import android.widget.ImageView
import android.widget.TextView
import androidx.appcompat.app.ActionBarDrawerToggle
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.GravityCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import co.gov.and.coronapp.bluetrace.services.RefreshBearerTokenDelegate
import co.gov.ins.guardianes.BuildConfig
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.helper.Constants
import co.gov.ins.guardianes.helper.Constants.Url.DATA_TRATMENT
import co.gov.ins.guardianes.manager.PrefManager
import co.gov.ins.guardianes.presentation.models.MenuHome
import co.gov.ins.guardianes.presentation.models.VersionApp
import co.gov.ins.guardianes.presentation.view.bluetrace.BluetraceSendActivity
import co.gov.ins.guardianes.presentation.view.codeQr.DocumentPdfActivity
import co.gov.ins.guardianes.presentation.view.how_is_colombia.HowIsColombiaFragment
import co.gov.ins.guardianes.presentation.view.info.CallMinistryFragment
import co.gov.ins.guardianes.presentation.view.notification.NotificationActivity
import co.gov.ins.guardianes.presentation.view.participant.ParticipantsActivity
import co.gov.ins.guardianes.presentation.view.permissions_and_privacy.PermissionsPrivacyActivity
import co.gov.ins.guardianes.presentation.view.place.PlaceActivity
import co.gov.ins.guardianes.presentation.view.quarantineHome.QuarantineHome
import co.gov.ins.guardianes.presentation.view.scheduled.ScheduleActivity
import co.gov.ins.guardianes.presentation.view.share_app.ShareAppActivity
import co.gov.ins.guardianes.presentation.view.splash.UpdateActivity
import co.gov.ins.guardianes.presentation.view.terms_use.TermsOfUseActivity
import co.gov.ins.guardianes.presentation.view.welcome.WelcomeIntro
import co.gov.ins.guardianes.util.Constants.CodeQR.PDF_URL
import co.gov.ins.guardianes.util.Constants.CodeQR.TXT_CONTENT
import co.gov.ins.guardianes.util.Constants.CodeQR.TXT_TOOLBAR
import co.gov.ins.guardianes.util.Constants.EVENT.MENU_ATTENTION_LINES
import co.gov.ins.guardianes.util.Constants.EVENT.MENU_HEALTH_CENTERS
import co.gov.ins.guardianes.util.Constants.EVENT.MENU_LAST_NEWS
import co.gov.ins.guardianes.util.Constants.EVENT.MENU_PRIVACITY
import co.gov.ins.guardianes.util.Constants.EVENT.MENU_SHARE
import co.gov.ins.guardianes.util.Constants.EVENT.MENU_TRACES
import co.gov.ins.guardianes.util.ext.goneDelayAnimate
import co.gov.ins.guardianes.util.ext.visibilityFade
import co.gov.ins.guardianes.view.menu.CoronappAbout
import co.gov.ins.guardianes.view.present.PresentActivity
import com.google.android.material.bottomnavigation.BottomNavigationItemView
import com.google.android.material.bottomnavigation.BottomNavigationMenuView
import kotlinx.android.synthetic.main.home_activity.*
import kotlinx.android.synthetic.main.home_menu_header.*
import org.koin.androidx.viewmodel.ext.android.viewModel
import smartdevelop.ir.eram.showcaseviewlib.GuideView
import smartdevelop.ir.eram.showcaseviewlib.config.DismissType
import java.text.SimpleDateFormat
import java.util.*


class HomeActivity : AppCompatActivity(), OnItemClickListener, RefreshBearerTokenDelegate {

    private lateinit var drawerToggle: ActionBarDrawerToggle
    private val homeViewModel: HomeViewModel by viewModel()

    private lateinit var comingFrom: String
    private var isShowHome: Boolean = false

    private lateinit var menuIcon: ImageView
    private lateinit var menuExit: ImageView



    override fun onCreate(bundle: Bundle?) {
        super.onCreate(bundle)
        setContentView(R.layout.home_activity)
        comingFrom = intent?.extras?.getString("from").orEmpty()
        isShowHome = intent.extras?.getBoolean("isShowHome") ?: false
        initToolbar()
        initBind()
        changeStyleNavigationBottom()
        initObserver()
        navigationBottomItemSelect()
        homeViewModel.getListMenu()
        nav_view.selectedItemId = R.id.navigation_home

        menuIcon = findViewById(R.id.menu_icon)
        menuExit = findViewById(R.id.close)

        menuIcon.setOnClickListener {
            dlHome.openDrawer(GravityCompat.END)
        }

        menuExit.setOnClickListener {
            dlHome.closeDrawer(GravityCompat.END)
        }
    }

    override fun onResume() {
        super.onResume()
        homeViewModel.getVersion()
    }

    private fun initObserver() {
        homeViewModel.getItemLiveData.observe(this, Observer {
            renderState(it)
        })
    }

    private fun renderState(state: HomeState) {
        when (state) {
            is HomeState.Success -> {
                list_view.adapter = HomeAdapter(this, state.data)
                list_view.onItemClickListener = this
            }
            is HomeState.SuccessVersion -> showData(state.data)
        }
    }

    override fun onStart() {
        super.onStart()
        changeIconNoti()
    }

    private fun showData(data: VersionApp) {
        data.minimumVersion.toIntOrNull()?.let {
            if (BuildConfig.VERSION_CODE < it && !isNotifyNewVersion()) {
                showWindow()
            }
        }
    }

    private fun showWindow() {
        val intent = Intent(this, UpdateActivity::class.java)
        startActivity(intent)
    }

    private fun changeStyleNavigationBottom() {
        val menuView = nav_view.getChildAt(0) as BottomNavigationMenuView
        for (i in 0 until menuView.childCount) {
            val item = menuView.getChildAt(i) as BottomNavigationItemView
            val smallLabel = item.findViewById<TextView>(R.id.smallLabel)
            val largeLabel = item.findViewById<TextView>(R.id.largeLabel)
            smallLabel.apply {
                maxLines = 3
                gravity = Gravity.CENTER
            }
            largeLabel.apply {
                maxLines = 3
                gravity = Gravity.CENTER
            }
        }
    }

    private fun coachMark(){
        if (!homeViewModel.getCoachMark()) {
            GuideView.Builder(this)
                    .setContentText(getString(R.string.info_coach))
                    .setDismissType(DismissType.anywhere)
                    .setTargetView(noti)
                    .setContentTextSize(15)
                    .build()
                    .show()
            homeViewModel.setCoachMark(true)
        }
    }

    private fun changeIconNoti(){
        if (homeViewModel.getPermission())
            noti.background = getDrawable(R.drawable.ic_desactive_noti)
        else
            noti.background = getDrawable(R.drawable.ic_active_noti)
    }

    private fun initBind() {

        changeIconNoti()

        noti.setOnClickListener {
            if (homeViewModel.getPermission()) {
                notiNot.visibilityFade(true)
                notiNot.goneDelayAnimate()
            }else {
                startActivity(Intent(this, NotificationActivity::class.java))
            }
        }

        close.setOnClickListener {
            dlHome.closeDrawer(GravityCompat.END)
        }
        drawerToggle = ActionBarDrawerToggle(
                this,
                dlHome,
                toolbar,
                R.string.empty,
                R.string.empty
        )
        drawerToggle.apply {
            isDrawerIndicatorEnabled = false
            setToolbarNavigationClickListener {
                if (dlHome.isDrawerOpen(GravityCompat.END)) {
                    dlHome.closeDrawer(GravityCompat.END)
                } else {
                    dlHome.openDrawer(GravityCompat.END)
                }
            }
        }
        dlHome.addDrawerListener(drawerToggle)
    }

    private fun initToolbar() {
        setSupportActionBar(toolbar)
        toolbar.apply {
            toolbar_title.text = getString(R.string.corona_app_text)
        }
    }

    private fun navigationBottomItemSelect() {
        nav_view.setOnNavigationItemSelectedListener { menuItem ->
            when (menuItem.itemId) {
                R.id.navigation_home -> {
                    validateUser()
                }
                R.id.navigation_how_colombia -> {
                    replace(HowIsColombiaFragment())
                }
                R.id.navigation_quarantine_home -> {
                    replace(QuarantineHome())
                }
                R.id.navigation_info -> {
                    replace(CallMinistryFragment())
                }
            }
            true
        }
    }

    private fun validateUser() {
        homeViewModel.getUser()?.let {
            if (homeViewModel.isToken()) {
                if (isNotifiedUpdate()) {
                    if (isNotifiedNewTerms()) {
                        if (compareDates()) {
                            if (comingFrom == "StateActivity") {
                                replace(HomeReportedFragment.newInstance())
                            } else {
                                coachMark()
                                noti.visibility = View.VISIBLE
                                replace(HomeLoginFragment.newInstance())
                            }
                        } else {
                            if (isShowHome) {
                                coachMark()
                                noti.visibility = View.VISIBLE
                                replace(HomeLoginFragment.newInstance())
                            } else {
                                startActivityForResult(Intent(this, ParticipantsActivity::class.java), PARTICIPANTS)
                            }
                        }
                    } else {
                        showTermsDialog()
                    }
                } else {
                    replace(HomeOldUserFragment.newInstance())
                }
            } else {
                replace(HomeNotLoginFragment.newInstance())
            }
        } ?: run {
            replace(HomeNotLoginFragment.newInstance())
        }
    }

    private fun replace(fragmentClass: Fragment) {
        supportFragmentManager.beginTransaction()
                .setCustomAnimations(
                        R.anim.slide_in_right,
                        android.R.anim.fade_out
                )
                .replace(R.id.frame_layout, fragmentClass).commit()
    }

    override fun onPostCreate(bundle: Bundle?) {
        super.onPostCreate(bundle)
        drawerToggle.syncState()
    }

    override fun onOptionsItemSelected(menuItem: MenuItem): Boolean {
        return drawerToggle.onOptionsItemSelected(menuItem) || super.onOptionsItemSelected(menuItem)
    }

    private fun setOnboardingViewed() {
        val prefManager = PrefManager(this)
        prefManager.putBoolean(Constants.Bundle.ONBOARDING_VIEWED, true)
    }

    private fun setNotifyUpdate() {
        val prefManager = PrefManager(this)
        prefManager.putBoolean(Constants.Bundle.UPDATE_NOTIFIED, true)
    }

    private fun isNotifiedUpdate(): Boolean {
        return PrefManager(this).getBoolean(Constants.Bundle.UPDATE_NOTIFIED, false)
    }

    private fun showTermsDialog() {
        val builder = AlertDialog.Builder(this, R.style.AlertDialogTheme)
        builder.setMessage("Hemos actualizado los términos y condiciones de Coronapp")
        builder.setCancelable(false)
        setNotifyNewTerms()
        builder.setPositiveButton("Aceptar") { _, _ ->
            replace(HomeLoginFragment.newInstance())
        }
        builder.setNeutralButton("Consultar") { _, _ ->
            startActivityForResult(Intent(this, TermsOfUseActivity::class.java), TERMS_USE)
        }
        builder.show()
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        when (requestCode) {
            TERMS_USE -> {
                showTermsDialog()
            }
            PARTICIPANTS -> {
                validateUser()
            }
            SHARE_APP -> {
                startActivity(Intent(this, ShareAppActivity::class.java))
            }
        }
    }

    private fun isNotifiedNewTerms(): Boolean {
        return PrefManager(this).getBoolean(Constants.Bundle.TERM_NOTIFIED, false)
    }

    private fun setNotifyNewTerms() {
        PrefManager(this).putBoolean(Constants.Bundle.TERM_NOTIFIED, true)
    }

    private fun isNotifyNewVersion(): Boolean {
        return PrefManager(this).getBoolean(Constants.Bundle.NEW_VERSION_NOTIFIED, false)
    }

    override fun onBackPressed() {
        when {
            dlHome.isDrawerOpen(GravityCompat.END) -> {
                dlHome.closeDrawer(GravityCompat.END)
            }
            else -> super.onBackPressed()
        }
    }

    override fun onItemClick(adapterView: AdapterView<*>, view: View, position: Int, id: Long) {
        dlHome.closeDrawer(GravityCompat.END)
        val item = adapterView.getItemAtPosition(position) as MenuHome
        when (item.title) {
            R.string.line -> {
                homeViewModel.createEvent(MENU_ATTENTION_LINES)
                startActivity(Intent(this, ScheduleActivity::class.java))
            }
            R.string.center_menu -> {
                homeViewModel.createEvent(MENU_HEALTH_CENTERS)
                startActivity(Intent(this, PlaceActivity::class.java))
            }
            R.string.news_menu -> {
                homeViewModel.createEvent(MENU_LAST_NEWS)
                startActivity(Intent(this, PresentActivity::class.java))
            }
            R.string.permi_privacy -> {
                homeViewModel.createEvent(MENU_PRIVACITY)
                startActivity(Intent(this, PermissionsPrivacyActivity::class.java))
            }
            R.string.acerca_corona_app -> {
                startActivity(Intent(this, CoronappAbout::class.java))
            }
            R.string.share_coronapp -> {
                homeViewModel.createEvent(MENU_SHARE)
                shareApp()
            }
            R.string.send_traces -> {
                homeViewModel.createEvent(MENU_TRACES)
                startActivity(Intent(this, BluetraceSendActivity::class.java))
            }
            R.string.text_treatment -> {

                val intent = Intent(this, DocumentPdfActivity::class.java).apply {
                    putExtra(TXT_TOOLBAR, getString(R.string.text_treatment))
                    putExtra(TXT_CONTENT, getString(R.string.text_treatment))
                    putExtra(PDF_URL, DATA_TRATMENT)
                }
                startActivity(intent)
            }
        }
    }

    private fun compareDates(): Boolean {
        val sdf = SimpleDateFormat("dd/MM/yyyy", Locale.getDefault())
        val currentDate = sdf.format(Calendar.getInstance().time)
        val preferenceDate = PrefManager(this).get(Constants.Pref.CURRENT_DATE, String::class.java)
        PrefManager(this).put(Constants.Pref.CURRENT_DATE, currentDate)

        preferenceDate?.let {
            return if (preferenceDate == currentDate) {
                true
            } else {
                PrefManager(this).put(Constants.Pref.CURRENT_DATE, currentDate)
                false
            }
        } ?: run {
            return false
        }
    }

    private fun shareApp() {
        val shareIntent = Intent().apply {
            action = Intent.ACTION_SEND
            putExtra(Intent.EXTRA_SUBJECT, Constants.General.APP_NAME)
            putExtra(Intent.EXTRA_TEXT, getString(R.string.message_to_share) + getString(R.string.url_to_share))
            type = "text/plain"
        }
        startActivityForResult(shareIntent, SHARE_APP)
    }

    fun logout() {
        if (PrefManager(this).clear()) {
            homeViewModel.preferenceClear()
            setOnboardingViewed()
            setNotifyUpdate()
            setNotifyNewTerms()
            val intent = Intent(this, WelcomeIntro::class.java).apply {
                flags = Intent.FLAG_ACTIVITY_CLEAR_TASK or Intent.FLAG_ACTIVITY_NEW_TASK
            }
            startActivity(intent)
        }
    }

    override fun toRefreshBearerToken(function: (result: Boolean) -> Unit) {
        homeViewModel.refreshToken(function)
    }

    override fun toChangeBearerToken(function: (result: Boolean) -> Unit) {
        homeViewModel.changeToken(function)
    }

    companion object {
        private const val TERMS_USE = 1030
        private const val SYMPTOM = 2030
        private const val PARTICIPANTS = 2031
        private const val SHARE_APP = 3030
    }
}
