package co.gov.ins.guardianes.presentation.view.survey

import android.annotation.SuppressLint
import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.os.Looper
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.data.local.db.Injection.participantsDataSource
import co.gov.ins.guardianes.data.local.repository.ParticipantLocalRepositoryImpl
import co.gov.ins.guardianes.data.local.repository.UserPreferencesImpl
import co.gov.ins.guardianes.presentation.view.home.HomeActivity
import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import com.google.firebase.analytics.FirebaseAnalytics
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.disposables.CompositeDisposable
import io.reactivex.schedulers.Schedulers
import kotlinx.android.synthetic.main.base_toolbar.*
import kotlinx.android.synthetic.main.state.*
import java.text.SimpleDateFormat
import java.util.*

class StateActivity : BaseAppCompatActivity() {
    private var id: String? = null
    private var firebaseAnalytics: FirebaseAnalytics? = null
    var disposable = CompositeDisposable()


    override fun onCreate(bundle: Bundle?) {
        super.onCreate(bundle)
        setContentView(R.layout.state)
        initToolbar()
        firebaseAnalytics = FirebaseAnalytics.getInstance(this)
        setDateToday()
        setDataLocal()

        badReport.setOnClickListener {
            onStateBad()
        }
        googReport.setOnClickListener {
            onStateGood()
        }
    }

    private fun initToolbar() {
        onToolbar(toolbar)
        toolbar.setNavigationOnClickListener { onBackPressed() }
        txtTitleBar.text = getString(R.string.symptoms_title)
    }

    private fun setDataLocal() {
        val userPreferences = UserPreferencesImpl(
            getSharedPreferences(
                Constants.Persistence.PREFERENCES_USER,
                Context.MODE_PRIVATE
            ), this
        )
        val dao = participantsDataSource(this)
        val participantLocalRepository = ParticipantLocalRepositoryImpl(dao)
        val mainMember =
            intent.getBooleanExtra(co.gov.ins.guardianes.helper.Constants.Bundle.MAIN_MEMBER, false)
        id = if (mainMember) userPreferences.getUserId() else intent.getStringExtra("id_user")

        if (mainMember) {
            nameUser.text = getString(R.string.hello_user, userPreferences.getUser()!!.firstName)
        } else {
            id.let {
                disposable.add(participantLocalRepository.getParticipantById(id!!)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.from(Looper.getMainLooper(), true))
                    .subscribe { (_, _, _, _, firstName) ->
                        nameUser.text = getString(R.string.hello_user, firstName)
                    })
            }

        }
    }

    private fun setDateToday() {
        val dateFormat = SimpleDateFormat("d 'de' MMMM", Locale("es", "MX"))
        dateFormat.timeZone = TimeZone.getTimeZone("GMT-5")
        val today = Calendar.getInstance().time
        dateFormat.format(today)
        val currentDate = dateFormat.format(today)
        headerState.text = getString(R.string.message_today, currentDate)
    }

    @SuppressLint("InvalidAnalyticsName")
    fun onStateGood() {
        showDiagnostic()
    }


    @SuppressLint("InvalidAnalyticsName")
    fun onStateBad() {
        navigateTo(SymptomActivity::class.java)
    }

    public override fun onResume() {
        super.onResume()
    }

    override fun onDestroy() {
        super.onDestroy()
        disposable.clear()
    }

    private fun showDiagnostic() {
        navigateTo(HomeActivity::class.java, 1)
        val intent = Intent(this@StateActivity, HomeActivity::class.java)
        intent.putExtra("from", "StateActivity")
        startActivity(intent)
    }

}