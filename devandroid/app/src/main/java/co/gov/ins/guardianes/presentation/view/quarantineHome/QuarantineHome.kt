package co.gov.ins.guardianes.presentation.view.quarantineHome

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.helper.Constants
import co.gov.ins.guardianes.helper.Constants.Bundle.TITLE
import co.gov.ins.guardianes.helper.Constants.Url.URL_EDUCATION
import co.gov.ins.guardianes.helper.Constants.Url.URL_INNOVA
import co.gov.ins.guardianes.presentation.view.healthTip.HealthTipActivity
import co.gov.ins.guardianes.util.Constants.EVENT.ECONOMIC_ALTERNATIVES
import co.gov.ins.guardianes.util.Constants.EVENT.HOME_CARE_TIPS
import co.gov.ins.guardianes.util.Constants.EVENT.PLATFORMS_TO_LEARN
import co.gov.ins.guardianes.view.web.WebViewActivity
import kotlinx.android.synthetic.main.fragment_in_the_house.*
import org.koin.androidx.viewmodel.ext.android.viewModel


class QuarantineHome : Fragment() {

    private val quarantineHomeViewModel: QuarantineHomeViewModel by viewModel()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_in_the_house, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)

        educationalContent.setOnClickListener {
            quarantineHomeViewModel.createEvent(PLATFORMS_TO_LEARN)
            openWeb(getString(R.string.educacion_digital), URL_EDUCATION)
        }

        workImplementation.setOnClickListener {
            quarantineHomeViewModel.createEvent(ECONOMIC_ALTERNATIVES)
            openWeb(getString(R.string.teletrabajo), URL_INNOVA)
        }

        helpquarantineHome.setOnClickListener {
            quarantineHomeViewModel.createEvent(HOME_CARE_TIPS)
            startActivity(Intent(context, HealthTipActivity::class.java))
        }
    }

    private fun openWeb(title: String, urlWeb: String) {
        val intent = Intent(context, WebViewActivity::class.java).apply {
            putExtra(TITLE, title)
            putExtra(Constants.Bundle.EXTRA_URL, urlWeb)
        }
        startActivity(intent)
    }

}
