package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.presentation.models.Categories
import co.gov.ins.guardianes.presentation.models.Diagnostic
import co.gov.ins.guardianes.presentation.models.Recommendations
import co.gov.ins.guardianes.domain.models.Diagnostic as Domain
import co.gov.ins.guardianes.domain.models.Categories as CategoriesDomain
import co.gov.ins.guardianes.domain.models.Recommendations as RecommendationsDomain

fun Domain.fromDomain() = run {
    Diagnostic(
            id,
            text,
            value,
            categories.map {
                it.fromDomain()
            }
    )
}

fun CategoriesDomain.fromDomain() = run {
    Categories(
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

fun RecommendationsDomain.fromDomain() = run {
    Recommendations(
            id,
            text,
            description,
            slug,
            order
    )
}