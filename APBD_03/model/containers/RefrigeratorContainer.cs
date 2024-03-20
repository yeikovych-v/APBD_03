using APBD_03.exception;
using APBD_03.service;

namespace APBD_03.model.containers;

public class RefrigeratorContainer(
    decimal heightCm,
    decimal weightKg,
    decimal depthCm,
    decimal maxPayloadKg,
    Product productStored,
    decimal tempC)
    : Container(new ContainerSerialNumber(ContainerType.Refrigerated), heightCm, weightKg, depthCm, maxPayloadKg)
{
    private Product ProductStored { get; } = productStored;
    private decimal TempC { get; } = tempC;

    public void LoadCargo(decimal payload, Product product)
    {
        if (!ProductService.IsValidContainerForProduct(TempC, ProductStored, product)) return;
        var newPayload = payload + CargoMassKg;
        if (newPayload > MaxPayloadKg)
            throw new OverfillException("RefrigeratorContainer cannot hold more cargo than its designed.");
        CargoMassKg = newPayload;
        Console.WriteLine($"Cargo loaded on ==> {SerialNum}");
    }

    public override void LoadCargo(decimal payload)
    {
        throw new MethodNotSupportedException("Cannot load RefrigeratorContainer without knowing the product type.");
    }
}