using APBD_03.model;
using APBD_03.model.containers;

namespace APBD_03.service;

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