using APBD_03.model;

namespace APBD_03.service;

public static class SerialNumGeneratorService
{
    private static Dictionary<ContainerType, int> _lastSerialNums = new();

    public static int GenerateSerialNumFor(ContainerType type)
    {
        if (!_lastSerialNums.ContainsKey(type)) InitContainerType(type);
        
        int lastVal = _lastSerialNums[type];
        
        return lastVal++;
    }

    private static void InitContainerType(ContainerType type)
    {
        _lastSerialNums.Add(type, 1);
    }
}