package co.gov.ins.guardianes.presentation.view.participant

import android.annotation.SuppressLint
import android.os.Bundle
import android.widget.Toast
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.helper.Constants
import co.gov.ins.guardianes.presentation.view.add_participant.AddParticipantActivity
import co.gov.ins.guardianes.presentation.view.semaphone.SemaphoreActivity
import co.gov.ins.guardianes.util.ext.progressHiddenDelay
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import kotlinx.android.synthetic.main.activity_participants.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class ParticipantsActivity : BaseAppCompatActivity() {

    //region variables
    private val participantsAdapter by lazy { ParticipantsAdapter() }
    private val participantsViewModel: ParticipantsViewModel by viewModel()
    private var lastReport: String = ""
    //endregion

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_participants)
        initToolbar()
        initObserver()
        initRecycler()
        intiBind()
        initView()
        initDate()
    }

    private fun intiBind() {
        addProfile.setOnClickListener {
            val bundle = Bundle().apply {
                putBoolean(Constants.Bundle.MAIN_MEMBER, false)
                putBoolean(Constants.Bundle.NEW_MEMBER, true)
            }
            navigateTo(AddParticipantActivity::class.java, bundle)
        }
    }

    @SuppressLint("SetTextI18n")
    private fun initView() {
        txtTitle.text = "Hola, ${participantsViewModel.getMainUser()?.firstName}"
    }

    private fun initToolbar() {
        onToolbar(toolbar)
        toolbar.setNavigationOnClickListener {
            onBackPressed()
        }
        txtTitleBar.text = getString(R.string.report_symptom)
    }

    private fun initDate() {
        participantsViewModel.queryLastUserDiagnosis()
    }

    private fun initObserver() {
        participantsViewModel.getParticipantsLiveDataState.observe(this, Observer {
            renderState(it)
        })

        participantsAdapter.getTipLiveData.observe(this, Observer {
            val bundle = Bundle().apply {
                if (participantsViewModel.getUser()?.id ?: 0 != it.id)
                    putString("id_user", it.id)
            }
            navigateTo(SemaphoreActivity::class.java, bundle)
        })
    }

    private fun renderState(state: ParticipantState) {
        when (state) {
            is ParticipantState.Loading -> {
                srlParticipants.isRefreshing = true
            }

            is ParticipantState.Success -> {
                participantsViewModel.queryLastSelfDiagnosis()
                participantsAdapter.items = state.data
                participantsAdapter.lastReport = lastReport
                srlParticipants.progressHiddenDelay()
            }

            is ParticipantState.SuccessDate -> {
                participantsAdapter.itemsDate = state.data
                srlParticipants.progressHiddenDelay()
            }

            is ParticipantState.SuccessUser -> {
                lastReport = state.data
            }

            is ParticipantState.Error -> {
                state.msg?.let {
                    Toast.makeText(this, it, Toast.LENGTH_LONG).show()
                } ?: run {
                    Toast.makeText(this, getString(R.string.error_default), Toast.LENGTH_LONG)
                        .show()
                }
                srlParticipants.progressHiddenDelay()
            }
        }
    }

    override fun onResume() {
        super.onResume()
        participantsViewModel.getParticipants()
    }

    private fun initRecycler() {
        rvParticipants.apply {
            adapter = participantsAdapter
        }

        srlParticipants.setOnRefreshListener {
            participantsViewModel.getParticipants()
        }
    }
}
