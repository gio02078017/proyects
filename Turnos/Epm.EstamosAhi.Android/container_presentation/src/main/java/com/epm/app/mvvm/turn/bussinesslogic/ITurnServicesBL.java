package com.epm.app.mvvm.turn.bussinesslogic;

import com.epm.app.mvvm.turn.network.request.AskTurnParameters;
import com.epm.app.mvvm.turn.network.request.CancelTurnParameters;
import com.epm.app.mvvm.turn.network.request.OfficeDetailParameters;
import com.epm.app.mvvm.turn.network.request.RequestGetNearbyOffices;
import com.epm.app.mvvm.turn.network.response.AssignedTurn;
import com.epm.app.mvvm.turn.network.response.askTurn.TurnResponse;
import com.epm.app.mvvm.turn.network.response.cancelTurn.CancelTurnResponse;
import com.epm.app.mvvm.turn.network.response.nearbyOffices.NearbyOfficesResponse;
import com.epm.app.mvvm.turn.network.response.officeDetail.OfficeDetailResponse;

import io.reactivex.Observable;

public interface ITurnServicesBL {

    Observable<NearbyOfficesResponse> getNearbyOfficesResponse(String token, RequestGetNearbyOffices solicitudObtenerOficinasCercanas);
    Observable<OfficeDetailResponse> getOfficeDetailResponse(String token, OfficeDetailParameters officeDetailParameters);
    Observable<TurnResponse> askTurnResponse(String token, AskTurnParameters askTurnParameters);
    Observable<CancelTurnResponse> cancelTurnResponse(String token, CancelTurnParameters cancelTurnParameters);
    Observable<AssignedTurn> getAssignedTurn(String token, String idDevice);

}
