package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.CategoriesEntity
import co.gov.ins.guardianes.data.local.entities.DiagnosticEntity
import co.gov.ins.guardianes.data.local.entities.RecommendationsEntity
import co.gov.ins.guardianes.domain.models.Diagnostic as Domain
import co.gov.ins.guardianes.domain.models.Categories as CategoriesDomain
import co.gov.ins.guardianes.domain.models.Recommendations as RecommendationsDomain

fun DiagnosticEntity.fromDomain() = run {
    Domain (
            id,
            text,
            description,
            value,
            categories.map {
                it.fromDomain()
            }
    )
}

fun CategoriesEntity.fromDomain() = run {
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

fun RecommendationsEntity.fromDomain() = run {
    RecommendationsDomain(
            id,
            text,
            description,
            slug,
            order
    )
}

fun Domain.fromEntity() = run {
    DiagnosticEntity (
            id,
            text,
            description,
            value,
            categories.map {
                it.fromEntity()
            }
    )
}

fun CategoriesDomain.fromEntity() = run {
    CategoriesEntity(
            id,
            text,
            description,
            image,
            slug,
            order,
            recommendations.map {
                it.fromEntity()
            }
    )
}

fun RecommendationsDomain.fromEntity () = run {
    RecommendationsEntity(
            id,
            text,
            description,
            slug,
            order
    )
}