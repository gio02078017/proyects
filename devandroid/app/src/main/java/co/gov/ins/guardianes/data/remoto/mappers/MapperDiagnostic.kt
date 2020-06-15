package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.Categories
import co.gov.ins.guardianes.data.remoto.models.Diagnostic
import co.gov.ins.guardianes.data.remoto.models.Recommendations
import co.gov.ins.guardianes.domain.models.Categories as CategoriesDomain
import co.gov.ins.guardianes.domain.models.Diagnostic as Domain
import co.gov.ins.guardianes.domain.models.Recommendations as RecommendationsDomain

fun Diagnostic.fromDomain() = run {
    Domain(
        id,
        text,
        description,
        value,
        categories.map {
            it.fromDomain()
        }
    )
}

fun Categories.fromDomain() = run {
    CategoriesDomain(
        id,
        text,
        description,
        image,
        slug,
        order,
        recommendations.map {
            it.fromDomain()
        }
    )
}

fun Recommendations.fromDomain() = run {
    RecommendationsDomain(
        id,
        text,
        description,
        slug,
        order
    )
}