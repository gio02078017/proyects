package co.gov.ins.guardianes.presentation.view.codeQr.mobilityStatus

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.view.codeQr.mobilityStatus.StatusCodeQrAdapter.ViewHolder
import co.gov.ins.guardianes.view.utils.fromHtml
import kotlin.properties.Delegates

class StatusCodeQrAdapter : RecyclerView.Adapter<ViewHolder>() {

    var items: List<String> by Delegates.observable(
            arrayListOf(),
            { _, _, _ -> notifyDataSetChanged() }
    )

    class ViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private lateinit var textRecommendation: TextView

        fun bind(recommendation: String) {

            textRecommendation = itemView.findViewById(R.id.text_recommedation)

            textRecommendation.fromHtml(recommendation)
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder = run {
        ViewHolder(LayoutInflater.from(parent.context).inflate(R.layout.item_status_code_qr, parent, false))
    }

    override fun getItemCount(): Int = items.size

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        holder.bind(items[position])
    }
}