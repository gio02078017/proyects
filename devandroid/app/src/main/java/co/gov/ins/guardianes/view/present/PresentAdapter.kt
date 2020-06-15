package co.gov.ins.guardianes.view.present

import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.model.ModelBasics
import co.gov.ins.guardianes.view.utils.inflate
import kotlin.properties.Delegates

class PresentAdapter(private val listener: (ModelBasics) -> Unit) :
    RecyclerView.Adapter<PresentAdapter.PresentViewHolder>() {

    var data: ArrayList<ModelBasics> by Delegates.observable(
        arrayListOf(),
        { _, _, _ -> notifyDataSetChanged() }
    )

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): PresentViewHolder =
        PresentViewHolder(parent.inflate(R.layout.item_layout, false))

    override fun getItemCount(): Int = data.size

    override fun onBindViewHolder(holder: PresentViewHolder, position: Int) =
        holder.bind(data[position], listener)

    class PresentViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        lateinit var title: TextView
        private lateinit var description: TextView

        fun bind(item: ModelBasics, listener: (ModelBasics) -> Unit) {

            title = itemView.findViewById(R.id.text1)
            description = itemView.findViewById(R.id.text2)

            title.text = item.title
            description.text = item.description

            itemView.setOnClickListener {
                listener(item)
            }
        }
    }
}