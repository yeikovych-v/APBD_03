namespace APBD_03.model.containers;

public class GasContainer(
    decimal heightCm,
    decimal weightKg,
    decimal depthCm,
    decimal maxPayloadKg,
    decimal pressureAtm)
    : Container(new ContainerSerialNumber(ContainerType.Gas), heightCm, weightKg, depthCm, maxPayloadKg),
        IHazardNotifier
{
    protected decimal PressureAtm { get; } = pressureAtm;

    public override decimal UnloadCargo()
    {
        var cargo = CargoMassKg * 0.95m;
        CargoMassKg *= 0.05m;
        Console.WriteLine($"Cargo was unloaded from ==> {SerialNum} <== Left Cargo {CargoMassKg}");
        return cargo;
    }


    public void SendTextNotification()
    {
        Console.WriteLine("A Hazardous Situation has occured with GasContainer: [" + SerialNum + "]");
    }
}