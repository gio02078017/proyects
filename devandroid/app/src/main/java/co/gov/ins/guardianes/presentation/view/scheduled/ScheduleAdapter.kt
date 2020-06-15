package co.gov.ins.guardianes.presentation.view.scheduled

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.data.remoto.models.HotLine
import co.gov.ins.guardianes.presentation.models.Schedule
import co.gov.ins.guardianes.util.ext.fromHtml
import kotlinx.android.synthetic.main.item_number.view.*

class ScheduleAdapter : RecyclerView.Adapter<ScheduleAdapter.ViewHolder>() {


    private val phoneLiveData = MutableLiveData<Schedule>()
    val getPhoneLiveData: LiveData<Schedule>
        get() = phoneLiveData

    var items: List<Schedule> = emptyList()
        set(value) {
            field = value
            notifyDataSetChanged()
        }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int) = run {
        val view =
            LayoutInflater.from(parent.context).inflate(R.layout.item_number, parent, false)
        ViewHolder(view)
    }

    override fun getItemCount() = items.size

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        holder.bind(items[position])
    }

    inner class ViewHolder(
        private val view: View
    ) : RecyclerView.ViewHolder(view) {
        fun bind(schedule: Schedule) = view.run {
            txvNameNumber.text = schedule.entityName
            txvNumber.text = context.fromHtml(context.getPhones(schedule.hotLines))
            itemView.setOnClickListener {
                phoneLiveData.value = schedule
            }
        }
    }

    fun Context.getPhones(hotLine: List<HotLine>): String = run {
        var phones = ""
        for (data in hotLine) {
            phones += if (hotLine.first().phone == data.phone) this.getString(
                R.string.color_phone,
                data.phone
            ) else this.getString(R.string.space_phones, data.phone)
        }
        return phones
    }

}
