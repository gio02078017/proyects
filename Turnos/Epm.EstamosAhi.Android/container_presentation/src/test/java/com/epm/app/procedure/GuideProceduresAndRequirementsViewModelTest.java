package com.epm.app.procedure;




import android.graphics.drawable.Drawable;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.BaseTest;
import com.epm.app.R;
import com.epm.app.TestObserver;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryItem;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryResponse;
import com.epm.app.mvvm.procedure.repository.ProcedureServicesRepository;
import com.epm.app.mvvm.procedure.viewModel.GuideProceduresAndRequirementsViewModel;
import com.google.common.truth.Truth;

import org.junit.Before;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;

import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.ConvertUtilities;
import io.reactivex.Observable;

import static org.junit.Assert.*;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class GuideProceduresAndRequirementsViewModelTest extends BaseTest {


    @Mock
    ProcedureServicesRepository procedureServicesRepository;

    @InjectMocks
    GuideProceduresAndRequirementsCategoryResponse guideProceduresAndRequirementsCategoryResponse;

    private List<GuideProceduresAndRequirementsCategoryItem> list;

    @Mock
    GuideProceduresAndRequirementsCategoryItem guideProceduresAndRequirementsCategoryItem;

    @Mock
    public ConvertUtilities convertUtilities;


    public ErrorMessage errorMessage;

    private GuideProceduresAndRequirementsViewModel guideProceduresAndRequirementsViewModel;

    @Before
    public void setUp() throws Exception {
        super.setUp();
        list = new ArrayList<>();
        errorMessage = new ErrorMessage(0, 0);
        convertUtilities = mock(ConvertUtilities.class);
        procedureServicesRepository = mock(ProcedureServicesRepository.class);
        guideProceduresAndRequirementsViewModel = Mockito.spy(new GuideProceduresAndRequirementsViewModel(procedureServicesRepository, customSharedPreferences, validateInternet));
    }

    @Test
    public void getGuideProceduresAndRequirementsCategories_getSuccessGuideProceduresAndRequirementsCategoryIsNull() {
        mockData();
        when(procedureServicesRepository.getGuideProceduresAndRequirementsCategory(customSharedPreferences.getString(Constants.TOKEN))).thenReturn(null);
        guideProceduresAndRequirementsViewModel.getGuideProceduresAndRequirementsCategories();
        TestObserver.test(guideProceduresAndRequirementsViewModel.getListGuideProceduresAndRequirementsCategory())
                .assertNoValue();
    }

    @Test
    public void getGuideProceduresAndRequirementsCategories_getSuccessGuideProceduresAndRequirementsCategoryNotNull() {
        mockData();
        when(procedureServicesRepository.getGuideProceduresAndRequirementsCategory(customSharedPreferences.getString(Constants.TOKEN))).thenReturn(Observable.just(guideProceduresAndRequirementsCategoryResponse));
        guideProceduresAndRequirementsViewModel.getGuideProceduresAndRequirementsCategories();
        TestObserver.test(guideProceduresAndRequirementsViewModel.getListGuideProceduresAndRequirementsCategory())
                .assertHasValue();
    }


    @Test
    public void getListGuideProceduresAndRequirementsCategory_returnLidataListGuideProceduresAndRequirementsCategory() {
        TestObserver<List<GuideProceduresAndRequirementsCategoryItem>> obj = TestObserver.test(guideProceduresAndRequirementsViewModel.getListGuideProceduresAndRequirementsCategory());
        Truth.assert_()
                .that(obj)
                .isNotNull();
    }

    @Test
    public void setGuideProceduresAndRequirementsCategoryItem_setGuideProceduresAndRequirementsCategoryItem() {
        guideProceduresAndRequirementsViewModel.setGuideProceduresAndRequirementsCategoryItem(guideProceduresAndRequirementsCategoryItem);
        GuideProceduresAndRequirementsCategoryItem obj = guideProceduresAndRequirementsViewModel.getGuideProceduresAndRequirementsCategoryItem();
        Truth.assert_()
                .that(obj)
                .isNotNull();
    }

    @Test
    public void setGuideProceduresAndRequirementsCategoryItem_setGuideProceduresAndRequirementsCategoryItemIsNull() {
        guideProceduresAndRequirementsViewModel.setGuideProceduresAndRequirementsCategoryItem(null);
        GuideProceduresAndRequirementsCategoryItem obj = guideProceduresAndRequirementsViewModel.getGuideProceduresAndRequirementsCategoryItem();
        Truth.assert_()
                .that(obj)
                .isNull();
    }

    @Test
    public void validateResponse_whenTheResponseIsNull() {
        guideProceduresAndRequirementsViewModel.validateResponse(null);
        TestObserver.test(guideProceduresAndRequirementsViewModel.getListGuideProceduresAndRequirementsCategory())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheResponseIsNotNull() {
        mockData();
        guideProceduresAndRequirementsViewModel.validateResponse(guideProceduresAndRequirementsCategoryResponse);
        TestObserver.test(guideProceduresAndRequirementsViewModel.getListGuideProceduresAndRequirementsCategory())
                .assertHasValue();
    }

    @Test
    public void validateResponse_whenTheTransactionIsFalse() {
        mockData();
        guideProceduresAndRequirementsCategoryResponse.setTransactionState(false);
        guideProceduresAndRequirementsViewModel.validateResponse(guideProceduresAndRequirementsCategoryResponse);
        TestObserver.test(guideProceduresAndRequirementsViewModel.getListGuideProceduresAndRequirementsCategory())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheTransactionIsTrue() {
        mockData();
        guideProceduresAndRequirementsCategoryResponse.setTransactionState(true);
        guideProceduresAndRequirementsViewModel.validateResponse(guideProceduresAndRequirementsCategoryResponse);
        TestObserver.test(guideProceduresAndRequirementsViewModel.getListGuideProceduresAndRequirementsCategory())
                .assertHasValue();
    }


    @Test
    public void validateResponse_whenTheListOfCategoriesComesNull() {
        mockData();
        guideProceduresAndRequirementsCategoryResponse.setGuideProceduresAndRequirementsCategoryItems(null);
        guideProceduresAndRequirementsViewModel.validateResponse(guideProceduresAndRequirementsCategoryResponse);
        TestObserver.test(guideProceduresAndRequirementsViewModel.getListGuideProceduresAndRequirementsCategory())
                .assertNoValue();
    }

    @Test
    public void validateResponse_whenTheListOfCategoriesComesIsNotNull() {
        mockData();
        guideProceduresAndRequirementsViewModel.validateResponse(guideProceduresAndRequirementsCategoryResponse);
        TestObserver.test(guideProceduresAndRequirementsViewModel.getListGuideProceduresAndRequirementsCategory())
                .assertHasValue();
    }

    @Test
    public void validateIfListOfCategoriesIsNullOrEmpty_whenTheResponseIsNull() {
        mockData();
        boolean condition = guideProceduresAndRequirementsViewModel.validateIfListOfCategoriesIsNullOrEmpty(null);
        Truth.assert_()
                .that(condition)
                .isFalse();
    }

    @Test
    public void validateIfListOfCategoriesIsNullOrEmpty_whenTheResponseIsNotNull() {
        mockData();
        boolean condition = guideProceduresAndRequirementsViewModel.validateIfListOfCategoriesIsNullOrEmpty(list);
        Truth.assert_()
                .that(condition)
                .isTrue();
    }

    @Test
    public void validateIfListOfCategoriesIsNullOrEmpty_whenTheResponseIsEmpty() {
        mockData();
        boolean condition = guideProceduresAndRequirementsViewModel.validateIfListOfCategoriesIsNullOrEmpty(new ArrayList<>());
        Truth.assert_()
                .that(condition)
                .isFalse();
    }

    @Test
    public void validateIfListOfCategoriesIsNullOrEmpty_whenTheResponseIsNotEmpty() {
        mockData();
        boolean condition = guideProceduresAndRequirementsViewModel.validateIfListOfCategoriesIsNullOrEmpty(list);
        Truth.assert_()
                .that(condition)
                .isTrue();
    }

    @Test
    public void getItemSoonMoreCategory_returnOneInstanceOfGuideProceduresAndRequirementsCategoryItem() {
        mockData();
        GuideProceduresAndRequirementsCategoryItem obj = guideProceduresAndRequirementsViewModel.getItemSoonMoreCategory();
        Truth.assert_()
                .that(obj)
                .isNotNull();
    }

   /* @Test
    public void getIconState_returnOneInstanceOfGuideProceduresAndRequirementsCategoryItem() {
        mockData();
        when(procedureServicesRepository.getGuideProceduresAndRequirementsCategory(customSharedPreferences.getString(Constants.TOKEN))).thenReturn(Observable.just(guideProceduresAndRequirementsCategoryResponse));
        guideProceduresAndRequirementsViewModel.drawInformation();
        TestObserver.test(guideProceduresAndRequirementsViewModel.getIconState())
                .assertHasValue();

    }*/


    @Test
    public void testValidateShowErrorGuideProceduresAndRequirementsCategoryError() {
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        guideProceduresAndRequirementsViewModel.validateShowError(errorMessage);
        assertEquals(Objects.requireNonNull(guideProceduresAndRequirementsViewModel.getError().getValue()).getMessage(), errorMessage.getMessage());
    }

    @Test
    public void testValidateShowErrorGuideProceduresAndRequirementsCategoryError401() {
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        guideProceduresAndRequirementsViewModel.validateShowError(errorMessage);
        assertEquals(guideProceduresAndRequirementsViewModel.getErrorUnauthorized(), (int) errorMessage.getMessage());
    }


    @Test
    public void getIconStatusButton_verifyThatTheGrayArrowIconReturnsWhenTheItemStatusIsInactive() {
        int icon = guideProceduresAndRequirementsViewModel.getIconStatusButton(false);
        assertEquals(icon, R.drawable.ic_arrow_right_turns_gray);
    }

    @Test
    public void getIconStatusButton_verifyThatTheGrayArrowIconReturnsWhenTheItemStatusIsNotInactive() {
        int icon = guideProceduresAndRequirementsViewModel.getIconStatusButton(true);
        assertNotEquals(icon, R.drawable.ic_arrow_right_turns_gray);
    }


    @Test
    public void getIconStatusButton_verifyThatTheGreenArrowIconReturnsWhenTheItemStatusIsInactive() {
        int icon = guideProceduresAndRequirementsViewModel.getIconStatusButton(true);
        assertEquals(icon, R.drawable.ic_arrow_right_turns_green);
    }

    @Test
    public void getIconStatusButton_verifyThatTheGreenArrowIconReturnsWhenTheItemStatusIsNotInactive() {
        int icon = guideProceduresAndRequirementsViewModel.getIconStatusButton(false);
        assertNotEquals(icon, R.drawable.ic_arrow_right_turns_green);
    }

    private void mockData() {
        guideProceduresAndRequirementsCategoryResponse.setTransactionState(true);
        guideProceduresAndRequirementsCategoryResponse.setGuideProceduresAndRequirementsCategoryItems(list);
        guideProceduresAndRequirementsCategoryItem.setCategoryId("");
        list.add(guideProceduresAndRequirementsCategoryItem);
    }


}