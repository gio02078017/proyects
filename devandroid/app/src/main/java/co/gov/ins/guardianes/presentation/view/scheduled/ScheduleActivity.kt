package co.gov.ins.guardianes.presentation.view.scheduled

import android.Manifest.permission
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.os.Bundle
import android.widget.Toast
import androidx.core.app.ActivityCompat
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.Schedule
import co.gov.ins.guardianes.util.ext.progressHiddenDelay
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import kotlinx.android.synthetic.main.activity_schedule.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class ScheduleActivity : BaseAppCompatActivity() {

    private val scheduleAdapter = ScheduleAdapter()
    private val scheduleViewModel: ScheduleViewModel by viewModel()
    private val permissionCall = 1

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_schedule)
        onToolbar(toolbar)
        toolbar.apply {
            setNavigationOnClickListener {
                onBackPressed()
            }
        }
        txtTitleBar.text = getString(R.string.line_number)
        initRecyclerView()
        initObserver()
        scheduleViewModel.getSchedule()
    }

    private fun initRecyclerView() {
        rvCallNumber.apply {
            adapter = scheduleAdapter
        }

        srlSchedules.setOnRefreshListener {
            scheduleViewModel.getSchedule()
        }
    }

    private fun initObserver() {
        scheduleAdapter.getPhoneLiveData.observe(this, Observer {
            starActivityCall(it)
        })

        scheduleViewModel.getScheduleData.observe(this, Observer {
            renderState(it)
        })
    }

    private fun renderState(state: ScheduleState?) {
        srlSchedules.isRefreshing = false
        when (state) {
            is ScheduleState.Loading -> {
                srlSchedules.isRefreshing = true
            }

            is ScheduleState.Success -> {
                scheduleAdapter.items = state.data
                srlSchedules.progressHiddenDelay()
            }

            is ScheduleState.Error -> {
                state.msg?.let {
                    Toast.makeText(this, it, Toast.LENGTH_LONG).show()
                } ?: run {
                    Toast.makeText(this, getString(R.string.error_default), Toast.LENGTH_LONG)
                        .show()
                }
            }
        }
    }

    private fun starActivityCall(schedule: Schedule) {
        val intent = Intent(Intent.ACTION_CALL, Uri.parse("tel:" + schedule.hotLines.first().phone))
        if (ActivityCompat.checkSelfPermission(
                this,
                permission.CALL_PHONE
            ) == PackageManager.PERMISSION_GRANTED
        ) {
            startActivity(intent)
        } else {
            ActivityCompat.requestPermissions(
                this, arrayOf(permission.CALL_PHONE),
                permissionCall
            )
        }
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        when (requestCode) {
            permissionCall -> {
                if (grantResults.isNotEmpty()
                    && grantResults[0] == PackageManager.PERMISSION_GRANTED
                ) {
                    scheduleAdapter.getPhoneLiveData.value?.let {
                        starActivityCall(it)
                    }
                }
            }
        }
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
    }
}
