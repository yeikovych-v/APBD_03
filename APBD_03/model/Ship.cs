using System.Text;
using APBD_03.model.containers;
using APBD_03.model.repository;
using APBD_03.service;

namespace APBD_03.model;

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
}