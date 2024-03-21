using APBD_03.service;

namespace APBD_03.model;

public class ContainerSerialNumber(ContainerType type)
{
    private string FirstPart { get; } = "KON";
    private ContainerType Type { get; } = type;
    private int Id { get; } = SerialNumGeneratorService.GenerateSerialNumFor(type);

    public override string ToString()
    {
        return FirstPart + "-" + CharFromType(Type) + "-" + Id;
    }

    private string CharFromType(ContainerType containerType)
    {
        return containerType switch
        {
            ContainerType.Refrigerated => "R",
            ContainerType.Gas => "G",
            ContainerType.Liquid => "L",
            ContainerType.LiquidHazardous => "LH",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        ContainerSerialNumber o = (ContainerSerialNumber)obj;

        return FirstPart == o.FirstPart &&
               Type == o.Type &&
               Id == o.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstPart, Type, Id);
    }
}