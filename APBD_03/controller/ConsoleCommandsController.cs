using APBD_03.model;
using APBD_03.model.containers;
using APBD_03.model.repository;
using APBD_03.repository;
using APBD_03.service;

namespace APBD_03.controller;

public static class ConsoleCommandsController
{
    public static void DisplayFeaturesTest()
    {
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

    public static void PrintSeparator()
    {
        Console.WriteLine(
            "============================================================================================================================================");
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

    public static void InitConsoleInterface()
    {
        PrintInfoLog();
        var command = RequestCommands();
        while (!string.Equals(command, "exit"))
        {
            PrintInfoLog();
            ExecuteCommand(command);
            command = RequestCommands();
        }
    }

    private static void PrintInfoLog()
    {
        PrintCargoStatistics();
        PrintSeparator();
        PrintShipStatistics();
        PrintSeparator();
        PrintPossibleActions();
        PrintSeparator();
    }

    private static void ExecuteCommand(string command)
    {
        Console.WriteLine($"Command: {command}");
        var commandParams = command.Split(" ");
        switch (commandParams[0])
        {
            case "addship":
            {
                ExecuteAddShip();
                Console.WriteLine("Successfully added new Ship.");
                break;
            }
            case "delship":
            {
                ExecuteDelShip();
                Console.WriteLine("Successfully removed Ship.");
                break;
            }
            case "addcon":
            {
                ExecuteAddCon(int.Parse(commandParams[1]));

                break;
            }
            case "delcon":
            {
                ExecuteDelCon(int.Parse(commandParams[1]));
                break;
            }
            case "trans":
            {
                ExecuteTrans(int.Parse(commandParams[1]), int.Parse(commandParams[2]));
                break;
            }
        }
    }

    private static void ExecuteAddShip()
    {
        ShipRepository.Add(MockService.GenerateRandomShip());
    }

    private static void ExecuteDelShip()
    {
        Console.Write("Type ship id to delete: ");
        var idStr = Console.ReadLine();
        while (string.IsNullOrEmpty(idStr) || !CalculationService.IsInt(idStr) ||
               !ShipRepository.HasWithId(int.Parse(idStr)))
        {
            Console.WriteLine("Given Id is empty/not a number/given ship does not exist.");
            Console.Write("Type ship id to delete: ");
            idStr = Console.ReadLine();
        }

        var id = int.Parse(idStr);
        ShipRepository.RemoveById(id);
    }

    private static void ExecuteAddCon(int shipId)
    {
        if (!ShipRepository.HasWithId(shipId))
        {
            Console.WriteLine("Ship with given id does not exist.");
            return;
        }

        ShipRepository.FindById(shipId).Add(MockService.GenerateRandomGasContainer());
    }

    private static void ExecuteDelCon(int shipId)
    {
        if (!ShipRepository.HasWithId(shipId))
        {
            Console.WriteLine("Ship with given id does not exist.");
            return;
        }

        var ship = ShipRepository.FindById(shipId);
        if (ship.ContainerCount() != 0)
        {
            Console.WriteLine($"Available IDs are from 0 to {ship.ContainerCount() - 1}");
            Console.Write("Type container id to delete: ");
            var idStr = Console.ReadLine();
            while (string.IsNullOrEmpty(idStr) || !CalculationService.IsInt(idStr) ||
                   !ContainerRepository.HasWithId(int.Parse(idStr)) || !ship.Contains(int.Parse(idStr)))
            {
                Console.WriteLine("Given Id is empty/not a number/given container does not exist.");
                Console.WriteLine($"Available IDs are from 0 to {ship.ContainerCount() - 1}");
                Console.Write("Type container id to delete: ");
                idStr = Console.ReadLine();
            }

            var id = int.Parse(idStr);
            ship.Remove(id);
            return;
        }

        Console.WriteLine("No containers on the ship - cannot delete.");
    }

    private static void ExecuteTrans(int shipFrom, int shipTo)
    {
        if (!ShipRepository.HasWithId(shipFrom) || !ShipRepository.HasWithId(shipTo))
        {
            Console.WriteLine($"Ship or Ships with given IDs [{shipFrom}], [{shipTo}] does not exist.");
            return;
        }

        var from = ShipRepository.FindById(shipFrom);
        var to = ShipRepository.FindById(shipTo);
        if (from.ContainerCount() != 0)
        {
            Console.WriteLine($"Available IDs are from 0 to {from.ContainerCount() - 1}");
            Console.Write("Type container id to transfer: ");
            var idStr = Console.ReadLine();
            while (string.IsNullOrEmpty(idStr) || !CalculationService.IsInt(idStr) ||
                   !ContainerRepository.HasWithId(int.Parse(idStr)) || !from.Contains(int.Parse(idStr)))
            {
                Console.WriteLine("Given Id is empty/not a number/given container does not exist.");
                Console.WriteLine($"Available IDs are from 0 to {from.ContainerCount() - 1}");
                Console.Write("Type container id to transfer: ");
                idStr = Console.ReadLine();
            }

            var id = int.Parse(idStr);
            ShipService.TransferContainerFromTo(from, to, from.GetContainerById(id));
            return;
        }

        Console.WriteLine("No containers on the ship - cannot transfer.");
    }


    private static string RequestCommands()
    {
        string command;
        bool isValidCommand;

        do
        {
            Console.Write("Please enter a command: ");
            command = Console.ReadLine() ?? string.Empty;

            isValidCommand = IsValidCommand(command);

            if (!isValidCommand)
            {
                Console.WriteLine("Invalid command. Please try again.");
            }
        } while (!isValidCommand);

        return command;
    }

    private static bool IsValidCommand(string command)
    {
        command = command.Trim();
        if (string.IsNullOrEmpty(command)) return false;
        var commandParams = command.Split(" ");
        try
        {
            return commandParams[0] switch
            {
                "exit" => true,
                "addship" => true,
                "delship" => true,
                "addcon" => IsValidAddCon(int.Parse(commandParams[1])),
                "delcon" => IsValidDelCon(int.Parse(commandParams[1])),
                "trans" => IsValidTrans(int.Parse(commandParams[1]), int.Parse(commandParams[2])),
                _ => false
            };
        }
        catch (IndexOutOfRangeException _)
        {
            Console.WriteLine(_.StackTrace);
            return false;
        }
        catch (FormatException _)
        {
            Console.WriteLine(_.StackTrace);
            return false;
        }
    }

    private static bool IsValidTrans(int fromId, int toId)
    {
        if (ShipRepository.HasWithId(fromId) && ShipRepository.HasWithId(toId)) return true;
        Console.WriteLine($"Cannot find ships with given ids: [{fromId}], [{toId}]. Either one or two are incorrect!");
        return false;
    }

    private static bool IsValidDelCon(int fromId)
    {
        if (ContainerRepository.HasWithId(fromId)) return true;
        Console.WriteLine($"Cannot find container with given id[{fromId}].");
        return false;
    }

    private static bool IsValidAddCon(int toId)
    {
        if (ContainerRepository.HasWithId(toId)) return true;
        Console.WriteLine($"Cannot find container with given id[{toId}].");
        return false;
    }

    private static void PrintPossibleActions()
    {
        Console.WriteLine("0. exit  <>  Close app.");
        Console.WriteLine("1. addship  <>  Add a container ship.");
        Console.WriteLine("2. delship  <>  Remove a container ship.");
        Console.WriteLine("3. addcon  shipId  <>  Add container to a container ship with given ship id (0-n).");
        Console.WriteLine("4. delcon  shipId  <>  Remove container from a container ship with given ship id (0-n).");
        Console.WriteLine(
            "5. trans fromId toId  <>  Transfer container from a container ship to the other container ship.");
    }

    private static void PrintShipStatistics()
    {
        Console.WriteLine("List of Ships: ");
        var index = 0;
        foreach (var s in ShipRepository.FindAll())
        {
            Console.WriteLine($"Ship with ID [{index}]");
            Console.WriteLine(s.ToString());
            index++;
        }
    }

    private static void PrintCargoStatistics()
    {
        Console.WriteLine("List of Containers: ");
        var index = 0;
        foreach (var c in ContainerRepository.FindAll())
        {
            Console.WriteLine($"Container with ID [{index}]");
            Console.WriteLine(c.ToString());
            index++;
        }
    }
}