package co.gov.ins.guardianes.presentation.view.codeQr.mobilityStatus

import android.bluetooth.BluetoothAdapter
import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.View.INVISIBLE
import android.view.View.VISIBLE
import androidx.appcompat.app.AlertDialog
import androidx.core.content.ContextCompat
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.LinearLayoutManager
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.GenerateQrResponse
import co.gov.ins.guardianes.presentation.view.codeQr.PermissionActivity
import co.gov.ins.guardianes.presentation.view.codeQr.mobilityStatus.StatusCodeQrState.*
import co.gov.ins.guardianes.presentation.view.home.HomeActivity
import co.gov.ins.guardianes.presentation.view.semaphone.SemaphoreActivity
import co.gov.ins.guardianes.util.Constants.QrType.RED
import co.gov.ins.guardianes.util.Constants.QrType.YELLOW
import co.gov.ins.guardianes.util.ext.*
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import co.gov.ins.guardianes.view.utils.fromHtml
import kotlinx.android.synthetic.main.activity_status_code_qr.*
import org.koin.androidx.viewmodel.ext.android.viewModel


class StatusCodeQrActivity : BaseAppCompatActivity() {

    private val statusCodeViewModel: StatusCodeViewModel by viewModel()
    private var isShowLas24: Boolean = false
    var mBluetoothAdapter: BluetoothAdapter? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_status_code_qr)

        initToolbar()
        observersLiveData()
        eventOnClick()


        statusCodeViewModel.getInformationQr()
        statusCodeViewModel.queryLastSelfDiagnosis()
    }

    private fun eventOnClick() {
        bnReportSymptom.setOnClickListener {
            if (isShowLas24) {
                startActivity(Intent(applicationContext, SemaphoreActivity::class.java))
            } else {
                startActivity(Intent(this, PermissionActivity::class.java))
            }
        }

        txtvGenerateNewValid.setOnClickListener {
            if (isShowLas24)
                alertDialog()
            else
                startActivity(Intent(applicationContext, PermissionActivity::class.java))
        }
    }

    private fun initToolbar() {
        onToolbar(toolbar)
        toolbar.setNavigationOnClickListener {
            onBackPressed()
        }
    }

    override fun onBackPressed() {
        super.onBackPressed()
        startActivity(Intent(this, HomeActivity::class.java))
        finish()
    }

    private fun observersLiveData() {
        statusCodeViewModel.statusQrLiveData.observe(this, Observer {
            renderStatus(it)
        })
    }

    private fun renderStatus(statusCodeQr: StatusCodeQrState) {
        when (statusCodeQr) {
            is InformationQr -> {
                showInformation(statusCodeQr.data)
            }
            is QrInvalid -> {
                codeInvalidQr(statusCodeQr.data)
            }
            is IsShowAlertDialig -> {
                isShowLas24 = statusCodeQr.isShow
            }
        }
    }

    private fun codeInvalidQr(data: GenerateQrResponse) {

        if (data.existsCompany == 1) {
            textColorStatusQr.fromHtml(data.qrType.toQrTypeCompany())
        } else {
            textColorStatusQr.fromHtml(data.qrType.toQrTypeNoCompany())
        }

        txtvDateValid.text = getString(R.string.status_invalid)

        viewShadow.visibility = VISIBLE
        bnReportSymptom.visibility = VISIBLE

        linearFramework.background = ContextCompat.getDrawable(
                this,
                R.drawable.framework_white
        )

        textColorStatusQr.setTextColor(
                ContextCompat.getColor(this, R.color.grey_400)
        )

        codeQrImage.setImageBitmap("Code invalid".generateCodeQr())
    }

    private fun showInformation(data: GenerateQrResponse) {

        if (data.existsCompany == 1) {
            textColorStatusQr.fromHtml(data.qrType.toQrTypeCompany())
        } else {
            textColorStatusQr.fromHtml(data.qrType.toQrTypeNoCompany())
        }

        codeQrImage.setImageBitmap(data.codeQr.generateCodeQr())

        textColorStatusQr.setTextColor(
                ContextCompat.getColor(this, data.qrType.toColorQrType())
        )

        linearFramework.background = ContextCompat.getDrawable(
                this,
                data.qrType.toColorBackground()
        )


        if(checkBluetoothStatus())
        {
            txtBluetooth.setText(R.string.bluetooth_enable)
            if(data.qrType == RED)
            {
                ivBluetooth.setImageDrawable(ContextCompat.getDrawable(this, R.drawable.ic_bt_red))
                ivLeftWave.setImageDrawable(ContextCompat.getDrawable(this, R.drawable.ic_leftred_wave))
                ivRightWave.setImageDrawable(ContextCompat.getDrawable(this, R.drawable.ic_rightred_wave))
            }
            else if (data.qrType == YELLOW){
                ivBluetooth.setImageDrawable(ContextCompat.getDrawable(this, R.drawable.ic_bt_yellow))
                ivLeftWave.setImageDrawable(ContextCompat.getDrawable(this, R.drawable.ic_leftyellow_wave))
                ivRightWave.setImageDrawable(ContextCompat.getDrawable(this, R.drawable.ic_rightyellow_wave))
            }
            else {
                ivBluetooth.setImageDrawable(ContextCompat.getDrawable(this, R.drawable.ic_bt_green))
                ivLeftWave.setImageDrawable(ContextCompat.getDrawable(this, R.drawable.ic_leftgreen_wave))
                ivRightWave.setImageDrawable(ContextCompat.getDrawable(this, R.drawable.ic_rightgreen_wave))
            }

        }
        else{
            txtBluetooth.setText(R.string.bluetooth_disable)
            txtBluetooth.setTextColor(ContextCompat.getColor(this, R.color.grey_400))
            ivBluetooth.setImageDrawable(ContextCompat.getDrawable(this, R.drawable.ic_bt_grey))
        }

        if (data.qrType == RED) {
            txtvGenerateNewValid.visibility = VISIBLE
            txtvDateValid.visibility = INVISIBLE

        } else {
            txtvDateValid.fromHtml(
                    getString(R.string.valid_status_qr, data.expirationDate)
            )
            txtvDateValid.visibility = VISIBLE
            txtvGenerateNewValid.visibility = INVISIBLE
        }

        addMessagesList(data.qrMessage)
    }

    private fun addMessagesList(qrMessage: List<String>) {

        listRecommendations.layoutManager = LinearLayoutManager(this)

        val adapter = StatusCodeQrAdapter()
        listRecommendations.adapter = adapter
        adapter.items = qrMessage
    }

    private fun alertDialog() {
        AlertDialog.Builder(this, R.style.AlertDialogTheme).apply {
            setTitle(getString(R.string.alert))
            setMessage(getString(R.string.self_diagnosis_message))
            setCancelable(false)
            setPositiveButton(getString(R.string.understood)) { _, _ ->
                startActivity(Intent(applicationContext, SemaphoreActivity::class.java))
            }
        }.show()
    }

    private fun checkBluetoothStatus(): Boolean {
         mBluetoothAdapter = BluetoothAdapter.getDefaultAdapter()
        return mBluetoothAdapter?.isEnabled!!
    }


}
