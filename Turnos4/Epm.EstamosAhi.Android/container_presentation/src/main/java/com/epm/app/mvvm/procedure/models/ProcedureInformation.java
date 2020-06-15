package com.epm.app.mvvm.procedure.models;

import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryItem;
import com.epm.app.mvvm.procedure.network.response.Procedure;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonItem;

import java.io.Serializable;

public class ProcedureInformation implements Serializable {

  private GuideProceduresAndRequirementsCategoryItem guideProceduresAndRequirementsCategoryItem;
  private Procedure procedure;
  private TypePersonItem typePersonItem;

    public GuideProceduresAndRequirementsCategoryItem getGuideProceduresAndRequirementsCategoryItem() {
        return guideProceduresAndRequirementsCategoryItem;
    }

    public void setGuideProceduresAndRequirementsCategoryItem(GuideProceduresAndRequirementsCategoryItem guideProceduresAndRequirementsCategoryItem) {
        this.guideProceduresAndRequirementsCategoryItem = guideProceduresAndRequirementsCategoryItem;
    }

    public Procedure getProcedure() {
        return procedure;
    }

    public void setProcedure(Procedure procedure) {
        this.procedure = procedure;
    }

    public TypePersonItem getTypePersonItem() {
        return typePersonItem;
    }

    public void setTypePersonItem(TypePersonItem typePersonItem) {
        this.typePersonItem = typePersonItem;
    }
}
