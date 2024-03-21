using APBD_03.model;
using APBD_03.model.containers;

namespace APBD_03.service;

public class MockService
{
    public static List<Ship> MockShips()
    {
        List<Ship> ships = new();
        ships.Add(new Ship(25, 100, 2000));
        ships.Add(new Ship(30, 150, 2500));
        ships.Add(new Ship(35, 200, 3000));
        ships.Add(new Ship(40, 250, 3500));
        ships.Add(new Ship(45, 300, 4000));
        return ships;
    }

    public static List<Container> MockContainers()
    {
        List<Container> containers = new();
        containers.AddRange(MockGasContainers());
        containers.AddRange(MockLiquidContainers());
        containers.AddRange(MockLiquidHazardContainers());
        containers.AddRange(MockRefrigeratorContainers());

        return containers;
    }

    private static List<GasContainer> MockGasContainers()
    {
        List<GasContainer> gas = new();
        gas.Add(new GasContainer(100, 200, 50, 500, 2.0m));
        gas.Add(new GasContainer(110, 210, 55, 510, 2.2m));
        gas.Add(new GasContainer(120, 220, 60, 520, 2.4m));
        gas.Add(new GasContainer(130, 230, 65, 530, 2.6m));
        gas.Add(new GasContainer(140, 240, 70, 540, 2.8m));
        return gas;
    }

    private static List<LiquidContainer> MockLiquidContainers()
    {
        List<LiquidContainer> liquid = new();
        liquid.Add(new LiquidContainer(100, 200, 50, 500));
        liquid.Add(new LiquidContainer(110, 210, 55, 510));
        liquid.Add(new LiquidContainer(120, 220, 60, 520));
        liquid.Add(new LiquidContainer(130, 230, 65, 530));
        liquid.Add(new LiquidContainer(140, 240, 70, 540));
        return liquid;
    }

    private static List<LiquidHazardousContainer> MockLiquidHazardContainers()
    {
        List<LiquidHazardousContainer> liquidHazard = new();
        liquidHazard.Add(new LiquidHazardousContainer(100, 200, 50, 500));
        liquidHazard.Add(new LiquidHazardousContainer(110, 210, 55, 510));
        liquidHazard.Add(new LiquidHazardousContainer(120, 220, 60, 520));
        liquidHazard.Add(new LiquidHazardousContainer(130, 230, 65, 530));
        liquidHazard.Add(new LiquidHazardousContainer(140, 240, 70, 540));
        return liquidHazard;
    }

    private static List<RefrigeratorContainer> MockRefrigeratorContainers()
    {
        List<RefrigeratorContainer> refrigerator = new();
        refrigerator.Add(new RefrigeratorContainer(100, 200, 50, 500, Product.Bananas,
            ProductService.GetSuitableTempForProduct(Product.Bananas)));
        refrigerator.Add(new RefrigeratorContainer(110, 210, 55, 510, Product.Chocolate,
            ProductService.GetSuitableTempForProduct(Product.Chocolate)));
        refrigerator.Add(new RefrigeratorContainer(120, 220, 60, 520, Product.Fish,
            ProductService.GetSuitableTempForProduct(Product.Fish)));
        refrigerator.Add(new RefrigeratorContainer(130, 230, 65, 530, Product.Meat,
            ProductService.GetSuitableTempForProduct(Product.Meat)));
        refrigerator.Add(new RefrigeratorContainer(140, 240, 70, 540, Product.IceCream,
            ProductService.GetSuitableTempForProduct(Product.IceCream)));
        return refrigerator;
    }

    public static Ship GenerateRandomShip()
    { 
        var random = new Random();
        
        var maxSpeedKnots = random.Next(15, 62); 
        var maxNumContainers = random.Next(50, 526); 
        var maxWeightContainersTons = Convert.ToDecimal(random.NextDouble() * (5461 - 981) + 981);

        return new Ship(maxSpeedKnots, maxNumContainers, maxWeightContainersTons);
    }

    public static GasContainer GenerateRandomGasContainer()
    {
        var random = new Random();
        
        var heightCm = random.Next(78, 146); 
        var weightKg = random.Next(164, 1025); 
        var depthCm = random.Next(33, 156); 
        var maxPayloadKg = random.Next(394, 981);
        
        var pressureAtm = Convert.ToDecimal(random.NextDouble() * (9.22 - 0.56) + 0.56); 
        
        return new GasContainer(heightCm, weightKg, depthCm, maxPayloadKg, pressureAtm);
    }

    public static Container GenerateRandomContainer(ContainerType type)
    {
        return type switch
        {
            ContainerType.Gas => GenerateRandomGasContainer(),
            ContainerType.Liquid => GenerateRandomLiquidContainer(),
            ContainerType.Refrigerated => GenerateRandomRefrigeratedContainer(),
            ContainerType.LiquidHazardous => GenerateRandomLiquidHazardousContainer(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unexpected Container Type.")
        };
    }

    private static LiquidContainer GenerateRandomLiquidContainer()
    {
        var random = new Random();
        
        var heightCm = random.Next(78, 146); 
        var weightKg = random.Next(164, 1025); 
        var depthCm = random.Next(33, 156); 
        var maxPayloadKg = random.Next(394, 981);

        return new LiquidContainer(heightCm, weightKg, depthCm, maxPayloadKg);
    }

    private static RefrigeratorContainer GenerateRandomRefrigeratedContainer()
    {
        var random = new Random();
        
        var heightCm = random.Next(78, 146); 
        var weightKg = random.Next(164, 1025); 
        var depthCm = random.Next(33, 156); 
        var maxPayloadKg = random.Next(394, 981);
        var product = ProductService.GenerateRandomProduct();
        var temp = ProductService.GetSuitableTempForProduct(product);
        
        return new RefrigeratorContainer(heightCm, weightKg, depthCm, maxPayloadKg, product, temp);
    }

    private static LiquidHazardousContainer GenerateRandomLiquidHazardousContainer()
    {
        var random = new Random();
        
        var heightCm = random.Next(78, 146); 
        var weightKg = random.Next(164, 1025); 
        var depthCm = random.Next(33, 156); 
        var maxPayloadKg = random.Next(394, 981);

        return new LiquidHazardousContainer(heightCm, weightKg, depthCm, maxPayloadKg);
    }
}