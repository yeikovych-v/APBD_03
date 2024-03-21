using APBD_03.model;

namespace APBD_03.service;

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
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}