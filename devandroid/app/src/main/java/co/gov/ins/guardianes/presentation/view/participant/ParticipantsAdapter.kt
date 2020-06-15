package co.gov.ins.guardianes.presentation.view.participant

import android.annotation.SuppressLint
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.LastDiagnostic
import co.gov.ins.guardianes.presentation.models.Participant
import co.gov.ins.guardianes.util.ext.formatDate
import co.gov.ins.guardianes.view.utils.inflate
import kotlinx.android.synthetic.main.parent_item.view.*

class ParticipantsAdapter : RecyclerView.Adapter<ParticipantsAdapter.ViewHolder>() {

    private val tipLiveData = MutableLiveData<Participant>()
    val getTipLiveData: LiveData<Participant>
        get() = tipLiveData

    var lastReport: String = ""

    var items: List<Participant> = emptyList()
        set(value) {
            field = value
            notifyDataSetChanged()
        }
    var itemsDate: List<LastDiagnostic> = emptyList()
        set(value) {
            field = value
            notifyDataSetChanged()
        }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int) =
            ViewHolder(parent.inflate(R.layout.parent_item, false))

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        holder.bind(items[position])
    }

    override fun getItemCount() = items.size

    inner class ViewHolder(
            private val view: View
    ) : RecyclerView.ViewHolder(view) {

        @SuppressLint("SetTextI18n")
        fun bind(participant: Participant) = view.run {
            var lastDate = ""
            tvLastDate.text = lastDate
            if (participant.relationship.isEmpty()) {
                txvUserName.text = "Yo, ${participant.firstName.trim()}"
                lastDate = lastReport
            } else {
                txvUserName.text = "${participant.firstName.trim()}, ${participant.relationship}"
                lastDate = itemsDate.lastOrNull { it.id == participant.id }?.date ?: ""
            }
            if (lastDate.isNotBlank()) {
                tvLastDate.text = "Último reporte: ${lastDate.formatDate()}"
            }

            val imageUser = when (context.getString(R.string.male)) {
                context.getString(R.string.female) -> R.drawable.avatar_default_female

                context.getString(R.string.male) -> R.drawable.ic_persona

                else -> R.drawable.perfil_avatar
            }
            ivUser.setImageResource(imageUser)
            itemView.setOnClickListener {
                tipLiveData.value = participant
            }
        }
    }

}
