using APBD_03.model;
using APBD_03.model.containers;
using APBD_03.model.repository;
using APBD_03.repository;
using APBD_03.service;

public class Program
{
    public static void Main(string[] args)
    {
        InitDb();
        PrintSeparator();
        
        LoadCargo();
        PrintSeparator();
        
        LoadCargoOnShips();
        PrintSeparator();
        
        RemoveCargoFromShips();
        PrintSeparator();

        UnloadCargo();
        PrintSeparator();

        LoadCargoToShipAndReplaceToOther();
        PrintSeparator();
    }

    private static void LoadCargoToShipAndReplaceToOther()
    {
        var shipOne = ShipRepository.FindAll().First();
        var shipTwo = ShipRepository.FindAll().Last();
        var container = ContainerRepository.FindAll().First();

        shipOne.Add(container);
        Console.WriteLine("Before transfer: ");
        Console.WriteLine("");
        Console.WriteLine("SHIP 01: ");
        Console.WriteLine(shipOne);
        Console.WriteLine("");
        Console.WriteLine("SHIP 02: ");
        Console.WriteLine(shipTwo);
        ShipService.TransferContainerFromTo(shipOne, shipTwo, container);
        Console.WriteLine("After transfer: ");
        Console.WriteLine("");
        Console.WriteLine("SHIP 01: ");
        Console.WriteLine(shipOne);
        Console.WriteLine("");
        Console.WriteLine("SHIP 02: ");
        Console.WriteLine(shipTwo);
    }

    private static void UnloadCargo()
    {
        foreach (var c in ContainerRepository.FindAll())
        {
            c.UnloadCargo();
        }
    }

    private static void PrintSeparator()
    {
        Console.WriteLine("============================================================================================================================================");
    }

    private static void RemoveCargoFromShips()
    {
        foreach (var s in ShipRepository.FindAll())
        {
            foreach (var c in ContainerRepository.FindAll())
            {
                s.Remove(c);
            }
        }
    }

    private static void LoadCargoOnShips()
    {
        foreach (var s in ShipRepository.FindAll())
        {
            s.AddAll(ContainerRepository.FindAll());
        }
    }

    private static void LoadCargo()
    {
        foreach (var c in ContainerRepository.FindAll())
        {
            if (c is RefrigeratorContainer)
            {
                RefrigeratorContainer refrigerator = (RefrigeratorContainer)c;
                refrigerator.LoadCargo(20, Product.Bananas);
                continue;
            }
            c.LoadCargo(20);
        }
    }

    private static void InitDb()
    {
        ShipRepository.AddAll(MockService.MockShips());
        ContainerRepository.AddAll(MockService.MockContainers());
    }
}