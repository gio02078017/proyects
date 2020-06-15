package com.epm.app.mvvm.turn.repository;

import com.epm.app.mvvm.turn.bussinesslogic.ITurnServicesBL;
import com.epm.app.mvvm.turn.network.TurnApiServices;
import com.epm.app.mvvm.turn.network.request.AskTurnParameters;
import com.epm.app.mvvm.turn.network.request.CancelTurnParameters;
import com.epm.app.mvvm.turn.network.request.OfficeDetailParameters;
import com.epm.app.mvvm.turn.network.request.RequestGetNearbyOffices;
import com.epm.app.mvvm.turn.network.response.AssignedTurn;
import com.epm.app.mvvm.turn.network.response.askTurn.TurnResponse;
import com.epm.app.mvvm.turn.network.response.cancelTurn.CancelTurnResponse;
import com.epm.app.mvvm.turn.network.response.nearbyOffices.NearbyOfficesResponse;
import com.epm.app.mvvm.turn.network.response.officeDetail.OfficeDetailResponse;

import javax.inject.Inject;

import io.reactivex.Observable;

public class TurnServicesRepository implements ITurnServicesBL {


    private TurnApiServices turnApiServices;



    @Inject
    public TurnServicesRepository(TurnApiServices turnApiServices) {
        this.turnApiServices = turnApiServices;

    }

    @Override
    public Observable<NearbyOfficesResponse> getNearbyOfficesResponse(String token, RequestGetNearbyOffices solicitudObtenerOficinasCercanas) {
        return turnApiServices.getNearbyOffices(token, solicitudObtenerOficinasCercanas);
    }

    @Override
    public Observable<OfficeDetailResponse> getOfficeDetailResponse(String token, OfficeDetailParameters officeDetailParameters) {
        return turnApiServices.getOfficeDetail(token, officeDetailParameters);
    }

    @Override
    public Observable<TurnResponse> askTurnResponse(String token, AskTurnParameters askTurnParameters) {
        return turnApiServices.askTurn(token,askTurnParameters);
    }

    @Override
    public Observable<CancelTurnResponse> cancelTurnResponse(String token, CancelTurnParameters cancelTurnParameters) {
        return turnApiServices.cancelTurn(token, cancelTurnParameters);
    }



    @Override
    public Observable<AssignedTurn> getAssignedTurn(String token, String idDevice) {
        return turnApiServices.getAssignedTurn(token, idDevice);
    }

}
