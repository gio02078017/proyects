package co.gov.ins.guardianes.data.local.dao

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import co.gov.ins.guardianes.data.local.entities.QuestionEntity
import io.reactivex.Completable
import io.reactivex.Flowable

@Dao
interface QuestionDao {

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertQuestions(questions: List<QuestionEntity>): Completable

    @Query("SELECT * FROM symptomQuestion")
    fun getQuestions(): Flowable<List<QuestionEntity>>

}