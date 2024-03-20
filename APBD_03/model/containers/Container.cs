using APBD_03.exception;

namespace APBD_03.model.containers;

public abstract class Container(
    ContainerSerialNumber serialNum,
    decimal heightCm,
    decimal weightKg,
    decimal depthCm,
    decimal maxPayloadKg)
{
    public ContainerSerialNumber SerialNum { get; } = serialNum;
    protected decimal CargoMassKg { get; set; } = 0;
    protected decimal HeightCm { get; } = heightCm;
    public decimal WeightKg { get; } = weightKg;
    protected decimal DepthCm { get; } = depthCm;
    protected decimal MaxPayloadKg { get; } = maxPayloadKg;

    public virtual void LoadCargo(decimal payload)
    {
        var newMass = CargoMassKg + payload;
        if (newMass > MaxPayloadKg) throw new OverfillException("Container cannot hold more cargo than its designed.");
        CargoMassKg = newMass;
        Console.WriteLine($"Cargo loaded on ==> {SerialNum}");
    }

    public virtual decimal UnloadCargo()
    {
        var cargo = CargoMassKg;
        CargoMassKg = 0;
        Console.WriteLine($"Cargo was unloaded from ==> {SerialNum} <== Left Cargo {CargoMassKg}");
        return cargo;
    }

    public override string ToString()
    {
        return $"Container: Serial Number={SerialNum}, Cargo Mass={CargoMassKg}kg, Height={HeightCm}cm, Weight={WeightKg}kg, Depth={DepthCm}cm, Max Payload={MaxPayloadKg}kg";
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Container o = (Container)obj;
        
        return SerialNum.Equals(o.SerialNum) &&
               CargoMassKg == o.CargoMassKg &&
               HeightCm == o.HeightCm &&
               WeightKg == o.WeightKg &&
               DepthCm == o.DepthCm &&
               MaxPayloadKg == o.MaxPayloadKg;
    }
}