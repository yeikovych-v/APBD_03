using APBD_03.exception;
using APBD_03.service;

namespace APBD_03.model.containers;

public class LiquidHazardousContainer(
    decimal heightCm,
    decimal weightKg,
    decimal depthCm,
    decimal maxPayloadKg)
    : LiquidContainer(heightCm, weightKg, depthCm, maxPayloadKg),
        IHazardNotifier
{
    public void SendTextNotification()
    {
        Console.WriteLine("A Hazardous Situation has occured with LiquidHazardousContainer: [" + SerialNum + "]");
    }
    
    public override void LoadCargo(decimal payload)
    {
        var newPayload = CargoMassKg + payload;
        if (newPayload > (MaxPayloadKg * 0.5m))
        {
            ReportService.ReportDangerousSituation("LiquidHazardousContainer was tried to be loaded more than 50%.");
            return;
        }
        CargoMassKg = newPayload;
        Console.WriteLine($"Cargo loaded on ==> {SerialNum}");
    }
}