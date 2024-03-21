using System.Text;

namespace APBD_03;

public class Program
{
    public static void Main(string[] args)
    {
        InitDb();
        ConsoleCommandsController.PrintSeparator();
        ConsoleCommandsController.InitConsoleInterface();
    }

    private static void InitDb()
    {
        ShipRepository.AddAll(MockService.MockShips());
        ContainerRepository.AddAll(MockService.MockContainers());
    }

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

            var ship = ShipRepository.FindById(shipId);
            Console.WriteLine("Types(case does not matter): [GAS], [REF], [LIQ], [LQH].");
            Console.Write("Type container type to create: ");
            var conType = Console.ReadLine();
            while (string.IsNullOrEmpty(conType) || !ContainerTypeService.IsValidContainerType(conType))
            {
                Console.WriteLine("Given container type is incorrect. ");
                Console.WriteLine("Types(case does not matter): [GAS], [REF], [LIQ], [LQH].");
                Console.Write("Type container type to create: ");
                conType = Console.ReadLine();
            }

            var type = ContainerTypeService.Parse(conType);
            ship.Add(MockService.GenerateRandomContainer(type));
        }

        private static void ExecuteDelCon(int shipId)
        {
            if (!ShipRepository.HasWithId(shipId))
            {
                Console.WriteLine("Ship with given id does not exist.");
                return;
            }

            var ship = ShipRepository.FindById(shipId);
            if (ship.GetContainerCount() != 0)
            {
                Console.WriteLine($"Available IDs are from 0 to {ship.GetContainerCount() - 1}");
                Console.Write("Type container id to delete: ");
                var idStr = Console.ReadLine();
                while (string.IsNullOrEmpty(idStr) || !CalculationService.IsInt(idStr) ||
                       !ContainerRepository.HasWithId(int.Parse(idStr)) || !ship.Contains(int.Parse(idStr)))
                {
                    Console.WriteLine("Given Id is empty/not a number/given container does not exist.");
                    Console.WriteLine($"Available IDs are from 0 to {ship.GetContainerCount() - 1}");
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
            if (from.GetContainerCount() != 0)
            {
                Console.WriteLine($"Available IDs are from 0 to {from.GetContainerCount() - 1}");
                Console.Write("Type container id to transfer: ");
                var idStr = Console.ReadLine();
                while (string.IsNullOrEmpty(idStr) || !CalculationService.IsInt(idStr) ||
                       !ContainerRepository.HasWithId(int.Parse(idStr)) || !from.Contains(int.Parse(idStr)))
                {
                    Console.WriteLine("Given Id is empty/not a number/given container does not exist.");
                    Console.WriteLine($"Available IDs are from 0 to {from.GetContainerCount() - 1}");
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
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static bool IsValidTrans(int fromId, int toId)
        {
            if (ShipRepository.HasWithId(fromId) && ShipRepository.HasWithId(toId)) return true;
            Console.WriteLine(
                $"Cannot find ships with given ids: [{fromId}], [{toId}]. Either one or two are incorrect!");
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
            Console.WriteLine(
                "4. delcon  shipId  <>  Remove container from a container ship with given ship id (0-n).");
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

    public static class ShipRepository
    {
        private static List<Ship> _ships = new();

        public static void Add(Ship ship)
        {
            _ships.Add(ship);
        }

        public static void AddAll(List<Ship> ships)
        {
            _ships.AddRange(ships);
        }

        public static List<Ship> FindAll()
        {
            return _ships;
        }

        public static bool HasWithId(int id)
        {
            return CalculationService.IsIdInListRange(id, _ships.Count);
        }

        public static void RemoveById(int id)
        {
            _ships.Remove(_ships[id]);
        }

        public static Ship FindById(int shipId)
        {
            return _ships[shipId];
        }
    }

    public static class ContainerRepository
    {
        private static List<Container> _containers = new();

        public static void Add(Container container)
        {
            _containers.Add(container);
        }

        public static void AddAll(List<Container> containers)
        {
            foreach (var c in containers) Add(c);
        }

        public static Container? FindBySerialNum(ContainerSerialNumber serialNumber)
        {
            foreach (var c in _containers)
            {
                if (Equals(c.SerialNum, serialNumber)) return c;
            }

            return null;
        }

        public static List<Container> FindAll()
        {
            return _containers;
        }

        public static bool HasWithId(int id)
        {
            return CalculationService.IsIdInListRange(id, _containers.Count);
        }
    }

    public static class ShipService
    {
        public static void TransferContainerFromTo(Ship from, Ship to, Container container)
        {
            if (from.Contains(container))
            {
                from.Remove(container);
                to.Add(container);
            }
        }
    }

    public static class SerialNumGeneratorService
    {
        private static Dictionary<ContainerType, int> _lastSerialNums = new();

        public static int GenerateSerialNumFor(ContainerType type)
        {
            if (!_lastSerialNums.ContainsKey(type)) InitContainerType(type);

            int lastVal = _lastSerialNums[type];
            _lastSerialNums[type]++;

            return lastVal;
        }

        private static void InitContainerType(ContainerType type)
        {
            _lastSerialNums.Add(type, 1);
        }
    }

    public static class ReportService
    {
        public static void ReportDangerousSituation(string message)
        {
            Console.Write("REPORT :: ");
            Console.WriteLine(message);
        }
    }

    public static class ProductService
    {
        private static Dictionary<Product, decimal> _productTemps = InitProductTemps();

        private static Dictionary<Product, decimal> InitProductTemps()
        {
            Dictionary<Product, decimal> dict = new();
            foreach (var product in (Product[])Enum.GetValues(typeof(Product)))
            {
                const decimal min = -10m;
                const decimal max = 16m;
                var random = (decimal)new Random().NextDouble() * (max - min) + min;
                dict.Add(product, random);
            }

            return dict;
        }

        public static bool IsValidContainerForProduct(decimal tempC, Product expected, Product given)
        {
            if (given.ToString() != expected.ToString())
            {
                Console.WriteLine("Unable to store product [" + given + "], in this container only product [" +
                                  expected + "] is allowed.");
                return false;
            }

            var expectedTemp = _productTemps[expected];
            if (expectedTemp != tempC)
            {
                Console.WriteLine("Unable to store product [" + expected + "], the temperature is not suitable: [" +
                                  tempC +
                                  "]. Needed temp [" + expectedTemp + "].");
                return false;
            }

            return true;
        }

        public static decimal GetSuitableTempForProduct(Product product)
        {
            return _productTemps[product];
        }

        public static Product GenerateRandomProduct()
        {
            Random random = new Random();

            int randomIndex = random.Next(_productTemps.Count);

            return new List<Product>(_productTemps.Keys)[randomIndex];
        }
    }

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

    public static class ContainerTypeService
    {
        public static bool IsValidContainerType(string type)
        {
            type = type.Trim().ToLower();
            return type switch
            {
                "gas" => true,
                "ref" => true,
                "liq" => true,
                "lqh" => true,
                _ => false
            };
        }

        public static ContainerType Parse(string type)
        {
            type = type.Trim().ToLower();
            return type switch
            {
                "gas" => ContainerType.Gas,
                "ref" => ContainerType.Refrigerated,
                "liq" => ContainerType.Liquid,
                "lqh" => ContainerType.LiquidHazardous,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unexpected Container Type.")
            };
        }
    }

    public static class CalculationService
    {
        public static decimal TonsToKg(decimal tons)
        {
            return tons * 1000;
        }

        public static bool IsIdInListRange(int num, int size)
        {
            return num >= 0 && num + 1 <= size;
        }

        public static bool IsInt(string str)
        {
            try
            {
                int.Parse(str);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

    public class Ship(decimal maxSpeedKnots, int maxNumContainers, decimal maxWeightContainersTons)
    {
        private List<Container> ShipCargo { get; } = new();
        private decimal MaxSpeedKnots { get; } = maxSpeedKnots;
        private int MaxNumContainers { get; } = maxNumContainers;
        private decimal MaxWeightContainersTons { get; } = maxWeightContainersTons;

        public bool Add(Container container)
        {
            if (MaxWeightReachedWith(container.WeightKg)) return false;
            ShipCargo.Add(container);
            Console.WriteLine(container + " <== Was added to the ship.");
            return true;
        }

        private bool MaxWeightReachedWith(decimal containerWeightKg)
        {
            return (CalcCurrentWeight() + containerWeightKg) > CalculationService.TonsToKg(MaxWeightContainersTons);
        }

        private decimal CalcCurrentWeight()
        {
            var sum = 0m;
            foreach (var c in ShipCargo)
            {
                sum += c.WeightKg;
            }

            return sum;
        }

        public void AddAll(List<Container> containers)
        {
            foreach (var c in containers) Add(c);
        }

        public void Remove(Container container)
        {
            ShipCargo.Remove(container);
            Console.WriteLine(container + " <== Was removed from the ship.");
        }

        public void Remove(int id)
        {
            var toRemove = ShipCargo[id];
            ShipCargo.Remove(toRemove);
            Console.WriteLine(toRemove + " <== Was removed from the ship.");
        }

        public bool Replace(int index, Container container)
        {
            if (ShipCargo.Count - 1 < index) return false;
            ShipCargo[index] = container;
            return true;
        }

        public bool Replace(ContainerSerialNumber serialNumber, Container container)
        {
            Container? toReplace = ContainerRepository.FindBySerialNum(serialNumber);
            if (toReplace == null) return false;

            for (var index = 0; index < ShipCargo.Count; index++)
            {
                var cargo = ShipCargo[index];
                if (toReplace.SerialNum.Equals(cargo.SerialNum)) ShipCargo[index] = toReplace;
            }

            return true;
        }

        public bool Contains(Container container)
        {
            return ShipCargo.Contains(container);
        }

        public bool Contains(int id)
        {
            return id >= 0 && ShipCargo.Count >= id + 1;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Ship Data:");
            sb.AppendLine($"Ship Max Speed(knots): {MaxSpeedKnots}");
            sb.AppendLine($"Containers Max Number: {MaxNumContainers}");
            sb.AppendLine($"Containers Max Weight(tons): {MaxWeightContainersTons}");
            sb.AppendLine("Ship Containers:");

            foreach (var c in ShipCargo)
            {
                sb.AppendLine(c.ToString());
            }

            return sb.ToString();
        }

        public int GetContainerCount()
        {
            return ShipCargo.Count;
        }

        public Container GetContainerById(int id)
        {
            return ShipCargo[id];
        }
    }

    public enum Product
    {
        Bananas,
        Chocolate,
        Fish,
        Meat,
        IceCream,
        FrozenPizza,
        Cheese,
        Sausages,
        Butter,
        Eggs
    }

    public interface IHazardNotifier
    {
        void SendTextNotification();
    }

    public enum ContainerType
    {
        Refrigerated,
        Gas,
        Liquid,
        LiquidHazardous
    }

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
            if (newMass > MaxPayloadKg)
                throw new OverfillException("Container cannot hold more cargo than its designed.");
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
            return
                $"Container: Serial Number={SerialNum}, Cargo Mass={CargoMassKg}kg, Height={HeightCm}cm, Weight={WeightKg}kg, Depth={DepthCm}cm, Max Payload={MaxPayloadKg}kg";
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

        public override int GetHashCode()
        {
            return HashCode.Combine(SerialNum, CargoMassKg, HeightCm, WeightKg, DepthCm, MaxPayloadKg);
        }
    }

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
                ReportService.ReportDangerousSituation(
                    "LiquidHazardousContainer was tried to be loaded more than 50%.");
                return;
            }

            CargoMassKg = newPayload;
            Console.WriteLine($"Cargo loaded on ==> {SerialNum}");
        }
    }

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
            throw new MethodNotSupportedException(
                "Cannot load RefrigeratorContainer without knowing the product type.");
        }
    }

    public class OverfillException(string message) : SystemException(message);

    public class MethodNotSupportedException(string message) : SystemException(message);
}