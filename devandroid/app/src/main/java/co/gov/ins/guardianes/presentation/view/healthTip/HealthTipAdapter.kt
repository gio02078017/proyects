package co.gov.ins.guardianes.presentation.view.healthTip

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.HealthTip
import kotlinx.android.synthetic.main.item_health_tip.view.*

class HealthTipAdapter : RecyclerView.Adapter<HealthTipAdapter.ViewHolder>() {

    private val tipLiveData = MutableLiveData<HealthTip>()
    val getTipLiveData: LiveData<HealthTip>
        get() = tipLiveData

    var items: List<HealthTip> = ArrayList()
    set(value) {
        field = value
        notifyDataSetChanged()
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int) = run {
        val view =
            LayoutInflater.from(parent.context).inflate(R.layout.item_health_tip, parent, false)
        ViewHolder(view)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        holder.bind(items[position])
    }

    override fun getItemCount() = items.size

    inner class ViewHolder(
        private val view: View
    ) : RecyclerView.ViewHolder(view) {
        fun bind(HealthTip: HealthTip) = view.run {
            txvTitleTip.text = HealthTip.title
            itemView.setOnClickListener {
                tipLiveData.value = HealthTip
            }
        }
    }


}
