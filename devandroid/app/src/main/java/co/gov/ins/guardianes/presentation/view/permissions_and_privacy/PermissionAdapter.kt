package co.gov.ins.guardianes.presentation.view.permissions_and_privacy

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.Permission
import kotlinx.android.synthetic.main.item_share_info.view.*

class PermissionAdapter : RecyclerView.Adapter<PermissionAdapter.ViewHolder>() {


    private val permissionLiveData = MutableLiveData<Permission>()
    val getPermissionLiveData: LiveData<Permission>
        get() = permissionLiveData

    var items: List<Permission> = emptyList()
        set(value) {
            field = value
            notifyDataSetChanged()
        }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int) = run {
        val view =
            LayoutInflater.from(parent.context).inflate(R.layout.item_share_info, parent, false)
        ViewHolder(view)
    }

    override fun getItemCount() = items.size

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        holder.bind(items[position])
    }

    inner class ViewHolder(
        private val view: View
    ) : RecyclerView.ViewHolder(view) {
        fun bind(permission: Permission) = view.run {
            name.text = permission.title
            body.text = permission.subTitle
            setIcon(permission.icon)
            cbWork.setOnCheckedChangeListener { _, isChecked ->
                permission.accept = isChecked
                permissionLiveData.value = permission
                if (permission.accept && permission.id == "4") {
                    items.forEachIndexed { index, permission ->
                        if (permission.id != "4" && permission.accept) {
                            permission.accept = false
                            notifyItemChanged(index)
                        }
                    }
                } else if (permission.accept && permission.id != "4"){
                    items.forEachIndexed { index, permission ->
                        if (permission.id == "4" && permission.accept) {
                            permission.accept = false
                            notifyItemChanged(index)
                        }
                    }
                }
            }

            cbWork.isChecked = permission.accept
        }
    }

    private fun View.setIcon(mIcon: String) {
        when(mIcon){
            "icon_x_black" -> {
                icon.background = context.getDrawable(R.drawable.ic_not_permission)
            }
            "icon_briefcase_black" -> {
                icon.background = context.getDrawable(R.drawable.ic_work_permission)
            }
            "icon_cross_black" -> {
                icon.background = context.getDrawable(R.drawable.ic_eps_permission)
            }
            "icon_interrogation_black" -> {
                icon.background = context.getDrawable(R.drawable.ic_arl_permission)
            }
            else -> {
                icon.background = context.getDrawable(R.drawable.ic_hoja)
            }
        }
    }


}
