using APBD_03.exception;
using APBD_03.service;

namespace APBD_03.model.containers;

public class LiquidContainer(
decimal heightCm,
decimal weightKg,
decimal depthCm,
decimal maxPayloadKg)
    : Container(new ContainerSerialNumber(ContainerType.Liquid), heightCm, weightKg, depthCm, maxPayloadKg)
{
    public override void LoadCargo(decimal payload)
    {
        var newPayload = CargoMassKg + payload;
        if (newPayload > (MaxPayloadKg * 0.9m))
        {
            ReportService.ReportDangerousSituation("LiquidContainer was tried to be loaded more than 90%.");
            return;
        }
        CargoMassKg = newPayload;
        Console.WriteLine($"Cargo loaded on ==> {SerialNum}");
    }
}