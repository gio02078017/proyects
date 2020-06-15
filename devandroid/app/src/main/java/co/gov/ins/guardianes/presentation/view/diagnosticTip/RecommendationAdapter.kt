package co.gov.ins.guardianes.presentation.view.diagnosticTip

import android.content.res.ColorStateList
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.Recommendations
import kotlinx.android.synthetic.main.item_recommendations.view.*

class RecommendationAdapter :
    ListAdapter<Recommendations, RecommendationAdapter.Holder>(DiffCallback()) {

    var color: Int = 0

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): Holder {
        return Holder(
            LayoutInflater.from(parent.context)
                .inflate(R.layout.item_recommendations, parent, false)
        )
    }

    override fun onBindViewHolder(holder: Holder, position: Int) {
        holder.bind(getItem(position))
    }

    override fun submitList(list: List<Recommendations>?) {
        super.submitList(list?.sortedBy { it.order })
    }

    inner class Holder(private val view: View) : RecyclerView.ViewHolder(view) {
        fun bind(recommendations: Recommendations) = view.run {
            txvRecommendation.text = recommendations.text.trim()
            imageView3.imageTintList =
                ColorStateList.valueOf(ContextCompat.getColor(context, color))
        }
    }

    class DiffCallback : DiffUtil.ItemCallback<Recommendations>() {
        override fun areItemsTheSame(oldItem: Recommendations, newItem: Recommendations): Boolean {
            return oldItem.id == newItem.id
        }

        override fun areContentsTheSame(
            oldItem: Recommendations,
            newItem: Recommendations
        ): Boolean {
            return oldItem == newItem
        }
    }
}