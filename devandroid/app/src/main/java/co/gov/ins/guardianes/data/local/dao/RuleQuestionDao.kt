package co.gov.ins.guardianes.data.local.dao

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import co.gov.ins.guardianes.data.local.entities.RuleQuestionEntity
import io.reactivex.Completable
import io.reactivex.Flowable

@Dao
interface RuleQuestionDao {

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertRuleQuestions(ruleQuestions: List<RuleQuestionEntity>): Completable

    @Query("SELECT * FROM symptomRuleQuestion")
    fun getRuleQuestions(): Flowable<List<RuleQuestionEntity>>

}