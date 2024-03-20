using APBD_03.model.containers;

namespace APBD_03.model.repository;

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
            if (c.SerialNum == serialNumber) return c;
        }

        return null;
    }

    public static List<Container> FindAll()
    {
        return _containers;
    }
}