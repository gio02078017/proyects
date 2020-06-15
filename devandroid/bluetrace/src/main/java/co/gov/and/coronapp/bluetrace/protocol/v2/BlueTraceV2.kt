package co.gov.and.coronapp.bluetrace.protocol.v2

import co.gov.and.coronapp.bluetrace.TracerApp
import co.gov.and.coronapp.bluetrace.logging.CentralLog
import co.gov.and.coronapp.bluetrace.protocol.BlueTraceProtocol
import co.gov.and.coronapp.bluetrace.protocol.CentralInterface
import co.gov.and.coronapp.bluetrace.protocol.PeripheralInterface
import co.gov.and.coronapp.bluetrace.services.BluetoothMonitoringService
import co.gov.and.coronapp.bluetrace.streetpass.CentralDevice
import co.gov.and.coronapp.bluetrace.streetpass.ConnectionRecord
import co.gov.and.coronapp.bluetrace.streetpass.PeripheralDevice

class BlueTraceV2 : BlueTraceProtocol(
        versionInt = 2,
        peripheral = V2Peripheral(),
        central = V2Central()
)

class V2Peripheral : PeripheralInterface {

    private val TAG = "V2Peripheral"

    override fun prepareReadRequestData(protocolVersion: Int): ByteArray {
        return V2ReadRequestPayload(
                v = protocolVersion,
                id = BluetoothMonitoringService.broadcastMessage?.tempID ?: "Missing TempID",
                o = TracerApp.ORG,
                peripheral = TracerApp.asPeripheralDevice(),
                aux = BluetoothMonitoringService.deviceID
        ).getPayload()
    }

    override fun processWriteRequestDataReceived(
            dataWritten: ByteArray,
            centralAddress: String
    ): ConnectionRecord? {
        try {
            val dataWrittenConn =
                    V2WriteRequestPayload.fromPayload(
                            dataWritten
                    )

            return ConnectionRecord(
                    version = dataWrittenConn.v,
                    msg = dataWrittenConn.id,
                    org = dataWrittenConn.o,
                    peripheral = TracerApp.asPeripheralDevice(),
                    central = CentralDevice(dataWrittenConn.mc, centralAddress),
                    rssi = dataWrittenConn.rs,
                    txPower = null,
                    aux = dataWrittenConn.aux
            )
        } catch (e: Throwable) {
            CentralLog.e(TAG, "Failed to deserialize write payload ${e.message}")
        }
        return null
    }
}

class V2Central : CentralInterface {

    private val TAG = "V2Central"

    override fun prepareWriteRequestData(
            protocolVersion: Int,
            rssi: Int,
            txPower: Int?,
            deviceID: String
    ): ByteArray {
        return V2WriteRequestPayload(
                v = protocolVersion,
                id = BluetoothMonitoringService.broadcastMessage?.tempID ?: "Missing TempID",
                o = TracerApp.ORG,
                central = TracerApp.asCentralDevice(),
                rs = rssi,
                aux = deviceID
        ).getPayload()
    }

    override fun processReadRequestDataReceived(
            dataRead: ByteArray,
            peripheralAddress: String,
            rssi: Int,
            txPower: Int?
    ): ConnectionRecord? {
        try {
            val readData =
                    V2ReadRequestPayload.fromPayload(
                            dataRead
                    )
            val peripheral =
                    PeripheralDevice(readData.mp, peripheralAddress)

            return ConnectionRecord(
                    version = readData.v,
                    msg = readData.id,
                    org = readData.o,
                    peripheral = peripheral,
                    central = TracerApp.asCentralDevice(),
                    rssi = rssi,
                    txPower = txPower,
                    aux = readData.aux
            )
        } catch (e: Throwable) {
            CentralLog.e(TAG, "Failed to deserialize read payload ${e.message}")
        }

        return null
    }
}
