package co.gov.ins.guardianes.presentation.view.info

import android.Manifest
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.app.ActivityCompat
import androidx.fragment.app.Fragment
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.util.Constants.EVENT.CALL_BUTTON
import kotlinx.android.synthetic.main.call_ministry_fragment.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class CallMinistryFragment : Fragment() {

    private val callHomeViewModel: CallHomeViewModel by viewModel()
    private val permissionCall = 14


    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.call_ministry_fragment, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)

        btnCall.setOnClickListener {
            callHomeViewModel.createEvent(CALL_BUTTON)
            starActivityCall(getString(R.string.number_ministry))
        }
    }

    private fun starActivityCall(phone: String) {
        val intent = Intent(Intent.ACTION_CALL, Uri.parse("tel:$phone"))
        if (ActivityCompat.checkSelfPermission(
                context!!,
                Manifest.permission.CALL_PHONE
            ) == PackageManager.PERMISSION_GRANTED
        ) {
            startActivity(intent)
        } else {
            ActivityCompat.requestPermissions(
                activity!!, arrayOf(Manifest.permission.CALL_PHONE),
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
                    starActivityCall(getString(R.string.number_ministry))
                }
            }
        }
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
    }

}
