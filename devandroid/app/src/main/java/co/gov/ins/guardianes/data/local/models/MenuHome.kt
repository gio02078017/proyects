package co.gov.ins.guardianes.data.local.models

import co.gov.ins.guardianes.R

data class MenuHome(
        val title: Int,
        val icon: Int
)

val listMenu = listOf(
        MenuHome(R.string.line, R.drawable.ic_line_phone),
        MenuHome(R.string.center_menu, R.drawable.ic_help_menu),
        MenuHome(R.string.news_menu, R.drawable.ic_new_menu),
        MenuHome(R.string.permi_privacy, R.drawable.ic_checkmark_circle),
        MenuHome(R.string.acerca_corona_app, R.drawable.ic_coronapp_menu),
        MenuHome(R.string.share_coronapp, R.drawable.ic_share_menu),
        MenuHome(R.string.send_traces, R.drawable.ic_bluetooth),
        MenuHome(R.string.text_treatment, R.drawable.ic_file_document_outline)
)