package co.gov.ins.guardianes.presentation.view.place

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.Place
import co.gov.ins.guardianes.util.Constants
import com.google.android.material.bottomsheet.BottomSheetDialogFragment
import kotlinx.android.synthetic.main.clinic_bottom_sheet.*
import org.parceler.Parcels

class PlaceBottomSheet : BottomSheetDialogFragment() {

    private var place: Place? = null
    private val placeLiveData = MutableLiveData<Place>()
    val getPlaceLiveData: LiveData<Place>
        get() = placeLiveData

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {
            place = Parcels.unwrap(it.getParcelable(Constants.Key.PLACE))
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.clinic_bottom_sheet, container)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        place?.let { place ->
            txvName.text = place.name
            txvAddress.text = formatAddress(place)
            btnRouter.setOnClickListener {
                placeLiveData.value = place
                dismiss()
            }
        }
    }

    private fun formatAddress(point: Place): String {
        var address = ""
        if (!point.city.isNullOrEmpty()) {
            address += point.address
        }
        if (!point.city.isNullOrEmpty()) {
            address += ", " + point.city
        }
        if (!point.city.isNullOrEmpty()) {
            address += " - " + point.state
        }
        return address
    }

    companion object {
        fun newInstance(place: Place): PlaceBottomSheet {
            val fragment = PlaceBottomSheet()
            val args = Bundle().apply {
                putParcelable(Constants.Key.PLACE, Parcels.wrap(place))
            }
            fragment.arguments = args
            return fragment
        }
    }
}