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
import co.gov.ins.guardianes.presentation.models.Categories
import co.gov.ins.guardianes.util.ext.*
import kotlinx.android.synthetic.main.item_diagnostic_tip.view.*

class DiagnosticTipAdapter :
    ListAdapter<Categories, DiagnosticTipAdapter.Holder>(DiffCallback()) {

    var diagnosticColor: Int = 0
    private val recommendationAdapter by lazy { RecommendationAdapter() }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): Holder {
        return Holder(
            LayoutInflater.from(parent.context)
                .inflate(R.layout.item_diagnostic_tip, parent, false)
        )
    }

    override fun onBindViewHolder(holder: Holder, position: Int) {
        holder.bind(getItem(position))
    }

    override fun submitList(list: List<Categories>?) {
        super.submitList(list?.sortedBy { it.order })
    }

    inner class Holder(private val view: View) : RecyclerView.ViewHolder(view) {

        fun bind(categories: Categories) = view.run {

            txvCategory.text = categories.text

            rcvFindings.visibility = if (categories.isSelect) {
                imgIcon.loadImage(categories.image.toIcon())
                imgExpand.animate().rotation(180f)
                txvCategory.setTextColor(ContextCompat.getColor(context, diagnosticColor))
                imgIcon.imageTintList =
                    ColorStateList.valueOf(ContextCompat.getColor(context, diagnosticColor))
                imgExpand.imageTintList =
                    ColorStateList.valueOf(ContextCompat.getColor(context, diagnosticColor))
                rcvFindings.expand()
                View.VISIBLE
            } else {
                imgIcon.loadImage(categories.image.toIconNormal())
                imgExpand.animate().rotation(0f)
                txvCategory.setTextColor(ContextCompat.getColor(context, R.color.text_blue))
                imgIcon.imageTintList =
                    ColorStateList.valueOf(ContextCompat.getColor(context, R.color.text_blue))
                imgExpand.imageTintList =
                    ColorStateList.valueOf(ContextCompat.getColor(context, R.color.text_blue))
                rcvFindings.collapse()
                View.GONE
            }

            rcvFindings.apply {
                adapter = recommendationAdapter
                itemAnimator = null
            }

            itemView.setOnClickListener {
                recommendationAdapter.color = diagnosticColor
                recommendationAdapter.submitList(categories.recommendations)
                currentList.forEach {
                    it.isSelect = if (it.id == categories.id) !categories.isSelect else false
                }
                notifyDataSetChanged()
            }
        }
    }

    class DiffCallback : DiffUtil.ItemCallback<Categories>() {
        override fun areItemsTheSame(oldItem: Categories, newItem: Categories): Boolean {
            return oldItem.id == newItem.id
        }

        override fun areContentsTheSame(oldItem: Categories, newItem: Categories): Boolean {
            return oldItem == newItem
        }
    }
}