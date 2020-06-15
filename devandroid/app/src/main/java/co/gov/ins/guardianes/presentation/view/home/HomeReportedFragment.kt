package co.gov.ins.guardianes.presentation.view.home
import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.view.participant.ParticipantsActivity
import co.gov.ins.guardianes.util.Constants.EVENT.REPORT_SYMPTOMS_BUTTON
import kotlinx.android.synthetic.main.home_login_fragment.*
import org.koin.androidx.viewmodel.ext.android.viewModel


class HomeReportedFragment : Fragment() {

    private val homeLoginViewModel: HomeLoginViewModel by viewModel()


    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.home_login_fragment, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        btnReport.setOnClickListener {
            homeLoginViewModel.createEvent(REPORT_SYMPTOMS_BUTTON)
            startActivity(Intent(requireActivity(), ParticipantsActivity::class.java))

        }

        txvNameUser.text = getString(R.string.title_name2, homeLoginViewModel.getUser()?.firstName)
    }

    companion object {
        fun newInstance() = HomeReportedFragment()
    }
}
