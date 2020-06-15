package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiSymptom
import co.gov.ins.guardianes.data.remoto.mappers.fromDomain
import co.gov.ins.guardianes.domain.models.FormsResponse
import co.gov.ins.guardianes.domain.repository.SymptomRepository
import io.reactivex.Single
import co.gov.ins.guardianes.data.remoto.models.FormsResponse as Data

class SymptomRepositoryImpl(
    private val apiSymptom: ApiSymptom
) : SymptomRepository {

    override fun getFormData(authorization: String): Single<FormsResponse> =
        apiSymptom.getQuestions(authorization).flatMap {
            if (it.error == true) {
                Single.error(Throwable())
            } else {
                Single.just(it.fromDomain())
            }
        }

    fun Data.fromDomain() =
        FormsResponse(
            question = question.map {
                it.fromDomain()
            },
            diagnostics = diagnostics.map {
                it.fromDomain()
            },
            rulesDiagnostics = rulesDiagnostics.map {
                it.fromDomain()
            },
            rulesQuestion = rulesQuestion.map {
                it.fromDomain()
            }
        )
}