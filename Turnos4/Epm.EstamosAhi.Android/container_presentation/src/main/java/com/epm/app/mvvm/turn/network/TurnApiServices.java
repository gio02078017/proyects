package com.epm.app.mvvm.turn.network;

import com.epm.app.mvvm.turn.network.request.AskTurnParameters;
import com.epm.app.mvvm.turn.network.request.CancelTurnParameters;
import com.epm.app.mvvm.turn.network.request.OfficeDetailParameters;
import com.epm.app.mvvm.turn.network.request.RequestGetNearbyOffices;
import com.epm.app.mvvm.turn.network.response.AssignedTurn;
import com.epm.app.mvvm.turn.network.response.askTurn.TurnResponse;
import com.epm.app.mvvm.turn.network.response.cancelTurn.CancelTurnResponse;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryResponse;
import com.epm.app.mvvm.turn.network.response.nearbyOffices.NearbyOfficesResponse;
import com.epm.app.mvvm.turn.network.response.officeDetail.OfficeDetailResponse;

import io.reactivex.Observable;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface TurnApiServices {

    @GET("Procedure/ObtenerCategorias")
    Observable<GuideProceduresAndRequirementsCategoryResponse> getGuideProceduresAndRequirementsCategory(
            @Header("authToken") String token);

    @POST("Oficina/ObtenerOficinasCercanas")
    Observable<NearbyOfficesResponse> getNearbyOffices(
            @Header("authToken") String token,
            @Body RequestGetNearbyOffices solicitudObtenerOficinasCercanas);

    @POST("Oficina/ObtenerDetalleOficinaParaTurnos")
    Observable<OfficeDetailResponse> getOfficeDetail(
            @Header("authToken") String token,
            @Body OfficeDetailParameters officeDetailParameters );

    @POST("Turno/SolicitarTurnoPorOficina")
    Observable<TurnResponse>askTurn(
            @Header("authToken") String token,
            @Body AskTurnParameters AskTurnParameters );

    @POST("Turno/CancelarTurno")
    Observable<CancelTurnResponse> cancelTurn(
            @Header("authToken") String token,
            @Body CancelTurnParameters cancelTurnParameters );

    @GET("Turno/ObtenerTurnoAsignado")
    Observable<AssignedTurn> getAssignedTurn(
            @Header("authToken") String token,
            @Query("idDispositivo") String idDevice);
}
