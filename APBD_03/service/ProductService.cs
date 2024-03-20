using APBD_03.model;

namespace APBD_03.service;

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
            Console.WriteLine("Unable to store product [" + expected + "], the temperature is not suitable: [" + tempC +
                              "]. Needed temp [" + expectedTemp + "].");
            return false;
        }

        return true;
    }

    public static decimal GetSuitableTempForProduct(Product product)
    {
        return _productTemps[product];
    }
}