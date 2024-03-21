using APBD_03.model;
using APBD_03.service;

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