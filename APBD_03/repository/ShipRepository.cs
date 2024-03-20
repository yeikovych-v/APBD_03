using APBD_03.model;

namespace APBD_03.repository;

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
}