package co.gov.ins.guardianes.presentation.view.home

import android.bluetooth.BluetoothAdapter
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.appcompat.app.AlertDialog
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import co.gov.and.coronapp.bluetrace.BluetraceUtils
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.GenerateQrResponse
import co.gov.ins.guardianes.presentation.view.codeQr.AdviceActivity
import co.gov.ins.guardianes.presentation.view.codeQr.CodeQrState
import co.gov.ins.guardianes.presentation.view.codeQr.CodeQrViewModel
import co.gov.ins.guardianes.presentation.view.codeQr.PermissionActivity
import co.gov.ins.guardianes.presentation.view.codeQr.mobilityStatus.StatusCodeQrActivity
import co.gov.ins.guardianes.presentation.view.home.HomeLoginState.Success
import co.gov.ins.guardianes.presentation.view.participant.ParticipantsActivity
import co.gov.ins.guardianes.presentation.view.semaphone.SemaphoreActivity
import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.util.Constants.EVENT.MOBILITY_STATUS
import co.gov.ins.guardianes.util.Constants.EVENT.REPORT_SYMPTOMS_BUTTON
import co.gov.ins.guardianes.util.ext.formatDate
import co.gov.ins.guardianes.util.ext.to24HoursPassed
import kotlinx.android.synthetic.main.home_activity.*
import kotlinx.android.synthetic.main.home_login_fragment.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class HomeLoginFragment : Fragment() {

    private val homeLoginViewModel: HomeLoginViewModel by viewModel()
    private val codeQrViewModel: CodeQrViewModel by viewModel()
    private lateinit var dataCodeQr: GenerateQrResponse
    private lateinit var lastDiagnosis: String
    private lateinit var homeActivity: HomeActivity

    var mBluetoothAdapter: BluetoothAdapter? = null

    override fun onCreateView(
            inflater: LayoutInflater, container: ViewGroup?,
            savedInstanceState: Bundle?
    ): View? {

        return inflater.inflate(R.layout.home_login_fragment, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)

        val filter = IntentFilter(BluetoothAdapter.ACTION_STATE_CHANGED)
        homeActivity.registerReceiver(mReceiver, filter)
        txvNameUser.text = getString(R.string.title_name2, homeLoginViewModel.getUser()?.firstName)
        checkBluetoothStatus()
        eventOnClick()
        observers()
        startBlueTraceService()
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)
        homeActivity = context as HomeActivity
    }

    private fun observers() {
        homeLoginViewModel.homeLiveData.observe(homeActivity, Observer {
            when (it) {
                is Success -> {
                    texvLastDiagnosis.text = getString(R.string.last_diagnosis, it.data.formatDate())
                    lastDiagnosis = it.data
                }
            }
        })
        codeQrViewModel.codeQrLiveData.observe(homeActivity, Observer {
            when (it) {
                is CodeQrState.SuccessValidate -> {
                    dataCodeQr = it.data
                }
                is CodeQrState.Error -> {
                }
            }
        })
    }

    private fun eventOnClick() {
        btnReport.setOnClickListener {
            homeLoginViewModel.createEvent(REPORT_SYMPTOMS_BUTTON)
            startActivity(Intent(homeActivity, ParticipantsActivity::class.java))
        }


        clQr.setOnClickListener {
            homeLoginViewModel.createEvent(MOBILITY_STATUS)

            if (this::lastDiagnosis.isInitialized) {
                val isPassed = lastDiagnosis.to24HoursPassed()

                if (this::dataCodeQr.isInitialized) {
                    if (!isPassed && !dataCodeQr.validQr) {
                        startActivity(Intent(homeActivity, PermissionActivity::class.java))
                    } else {
                        val intent: Intent = if (dataCodeQr.qrType == Constants.QrType.GREEN)
                            Intent(homeActivity, AdviceActivity::class.java)
                        else
                            Intent(homeActivity, StatusCodeQrActivity::class.java)

                        startActivity(intent)
                    }
                } else{
                    alertDialog()
                }
            } else {
                alertDialog()
            }
        }
    }

    private fun startBlueTraceService() {
        BluetraceUtils.setTokenDelegate(homeActivity)
        BluetraceUtils.startBluetoothMonitoringService(homeActivity.applicationContext)
    }

    private fun alertDialog() {
        AlertDialog.Builder(homeActivity, R.style.AlertDialogTheme).apply {
            setTitle(getString(R.string.alert))
            setMessage(getString(R.string.self_diagnosis_message))
            setCancelable(false)
            setPositiveButton(getString(R.string.understood)) { _, _ ->
                startActivity(Intent(homeActivity, SemaphoreActivity::class.java))
            }
        }.show()
    }

    override fun onResume() {
        super.onResume()
        homeLoginViewModel.queryLastSelfDiagnosis()
        codeQrViewModel.validateQr(codeQrViewModel.getUser()?.id.toString())
    }

    companion object {
        fun newInstance() = HomeLoginFragment()
    }

    private val mReceiver: BroadcastReceiver = object : BroadcastReceiver() {
        override fun onReceive(context: Context, intent: Intent) {
            val action = intent.action
            if (action == BluetoothAdapter.ACTION_STATE_CHANGED) {
                val state = intent.getIntExtra(BluetoothAdapter.EXTRA_STATE,
                        BluetoothAdapter.ERROR)
                when (state) {
                    BluetoothAdapter.STATE_OFF -> checkBluetoothStatus()
                    BluetoothAdapter.STATE_ON -> checkBluetoothStatus()
                }
            }
        }
    }

    private fun checkBluetoothStatus() {
        mBluetoothAdapter = BluetoothAdapter.getDefaultAdapter()
        if (mBluetoothAdapter?.isEnabled!!)
        {
            ivBluetooth.setImageDrawable(ContextCompat.getDrawable(homeActivity, R.drawable.ic_bt_orange))
            tvBluetooth.setText(R.string.bluetooth_enable_home)
            tvBluetooth.setTextColor(ContextCompat.getColor(homeActivity, R.color.tab_select))
        }
        else
        {
            ivBluetooth.setImageDrawable(ContextCompat.getDrawable(homeActivity, R.drawable.ic_bt_blue))
            tvBluetooth.setText(R.string.bluetooth_disable_home)
            tvBluetooth.setTextColor(ContextCompat.getColor(homeActivity, R.color.text_blue))
        }
    }
}
