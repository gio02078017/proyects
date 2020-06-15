package co.gov.ins.guardianes.presentation.view.how_is_colombia

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.View.GONE
import android.view.View.VISIBLE
import android.view.ViewGroup
import android.widget.*
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.domain.models.HowIsColombia
import co.gov.ins.guardianes.helper.Constants
import co.gov.ins.guardianes.presentation.view.how_is_colombia.HowIsColombiaState.Loading
import co.gov.ins.guardianes.presentation.view.share_app.ShareAppActivity
import co.gov.ins.guardianes.util.Constants.EVENT.SEE_INTERACTIVE_MAP
import co.gov.ins.guardianes.util.Constants.EVENT.SHARE_IT_APP
import co.gov.ins.guardianes.util.ext.goneDelay
import co.gov.ins.guardianes.view.utils.fromHtml
import co.gov.ins.guardianes.view.web.WebViewActivity
import org.koin.androidx.viewmodel.ext.android.viewModel
import java.text.NumberFormat
import java.util.*

class HowIsColombiaFragment : Fragment() {

    private val howIsColombiaViewModel: HowIsColombiaViewModel by viewModel()
    private lateinit var confirmedCasesText: TextView
    private lateinit var relativeMap: RelativeLayout
    private lateinit var invitationText: TextView
    private lateinit var newCasesToday: TextView
    private lateinit var showMapText: TextView
    private lateinit var recovered: TextView
    private lateinit var sharedText: TextView
    private lateinit var insFont: TextView
    private lateinit var dead: TextView

    private lateinit var sharedView: ImageView

    private lateinit var linearShared: LinearLayout

    private lateinit var progress: ProgressBar

    override fun onCreateView(
            inflater: LayoutInflater, container: ViewGroup?,
            savedInstanceState: Bundle?
    ): View? {

        val view = inflater.inflate(R.layout.fragment_how_is_colombia, container, false)

        instanceVariables(view)
        showHtml()
        onClick()
        initViewModel()
        return view
    }

    private fun initViewModel() {
        activity?.let { activity ->
            howIsColombiaViewModel.getHowIsColombiaState.observe(activity, Observer {
                renderState(it)
            })
        }
    }

    private fun renderState(state: HowIsColombiaState) {
        progress.visibility = GONE
        when (state) {
            is Loading -> progress.visibility = VISIBLE
            is HowIsColombiaState.Success -> {
                progress.goneDelay()
                showData(state.data)
            }
        }
    }

    private fun instanceVariables(view: View) {
        confirmedCasesText = view.findViewById(R.id.confirmed_cases_text)
        invitationText = view.findViewById(R.id.invitation_text)
        newCasesToday = view.findViewById(R.id.new_cases_today)
        linearShared = view.findViewById(R.id.linear_shared)
        recovered = view.findViewById(R.id.recovered)
        showMapText = view.findViewById(R.id.show_map_text)
        relativeMap = view.findViewById(R.id.relative_map)
        sharedText = view.findViewById(R.id.shared_text)
        sharedView = view.findViewById(R.id.shared_view)
        insFont = view.findViewById(R.id.ins_font)
        dead = view.findViewById(R.id.dead)

        progress = view.findViewById(R.id.progressBar)
    }

    private fun showHtml() {
        invitationText.fromHtml(getString(R.string.ya_somos_800))
        sharedText.fromHtml(getString(R.string.share_app))
        showMapText.fromHtml(getString(R.string.show_map))
        insFont.fromHtml(getString(R.string.ins_font))
    }

    private fun onClick() {
        relativeMap.setOnClickListener {
            howIsColombiaViewModel.createEvent(SEE_INTERACTIVE_MAP)
            val intent = Intent(context, WebViewActivity::class.java).apply {
                putExtra("title", "Estado de los casos")
                putExtra(Constants.Bundle.EXTRA_URL, Constants.Url.STATUS_MAP)
            }
            startActivity(intent)
        }

        sharedView.setOnClickListener {
            howIsColombiaViewModel.createEvent(SHARE_IT_APP)
            shareApp()
        }

        sharedText.setOnClickListener {
            howIsColombiaViewModel.createEvent(SHARE_IT_APP)
            shareApp()
        }
    }

    private fun showData(data: HowIsColombia) {
        data.run {
            val number =
                    NumberFormat.getNumberInstance(Locale.US).format(usersSupporting).replace(",", ".")
            invitationText.fromHtml(getString(R.string.ya_somos_800).replace("0", number))
            confirmedCasesText.text = confirmedCases.toString()
            newCasesToday.text = confirmedCasesToday.toString()
            recovered.text = recoveredPatients.toString()
            dead.text = deaths.toString()
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

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        when (requestCode) {
            SHARE_APP -> {
                startActivity(Intent(context, ShareAppActivity::class.java))
            }
        }
    }

    override fun onResume() {
        super.onResume()
        howIsColombiaViewModel.getData()
    }

    companion object {
        private const val SHARE_APP = 3030
    }
}
